import { Component } from '@angular/core';
import { excelModel } from '../models/excel-model';
import { Ledger } from '../models/ladger.model';
import { BillSummary } from '../models/bill-summary.model';
import { AdminServicesService } from '../../../services/admin-services.service';
import * as XLSX from 'xlsx';
import { ToastrService } from 'ngx-toastr';
import { Injectable } from '@angular/core';


@Component({
  selector: 'app-import-e-invoice',
  templateUrl: './import-e-invoice.component.html',
  styleUrls: ['./import-e-invoice.component.scss']
})

@Injectable({
  providedIn: 'root',
})
export class ImportEInvoiceComponent  {
  selectedFile: File | null = null;
  excelDataList: excelModel[] = []; // Declare without initializing
  globalCompanyId: string = "";
  jsonData: any[][] = []; // Initialize it with the data
  columnNames: string[] = [];
  
  constructor(private adminServices:AdminServicesService,private toastr: ToastrService) {

    this.globalCompanyId = sessionStorage.getItem('companyID');
  }

  
  onFileChange(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  saveData(): void {

    if (this.excelDataList.length === 0) {
      this.toastr.warning('The list is empty. Please add data before saving.');
      return;
    }
    console.log(this.excelDataList);
    this.adminServices.SaveExcelData(this.excelDataList).subscribe({
      next: (res: any) => {
        if (res) {
          this.toastr.success('Added Successfully!');
        }
      },
      error: (error: any) => {
        console.log(error);
        this.toastr.error('Something went wrong');
      },
      complete: () => {
        this.toastr.success('Data Saved...!');
      },
    });
  }
  
  onFormSubmit(): void {
    // This method is called when the form is submitted
    this.excelDataList=[];
    if (this.selectedFile) {
      // Process the selected Excel file
      this.readExcel(this.selectedFile);
    } else {
      console.log('No file selected.');
      // You might want to display a message to the user indicating that no file was selected.
    }
  }



  readExcel(file: File): void {
    const reader: FileReader = new FileReader();
  
    reader.onload = (e: any) => {
      const binaryString: string = e.target.result;
      const workBook: XLSX.WorkBook = XLSX.read(binaryString, { type: 'binary' });
  
      // Assuming the data is present in the first sheet
      const sheetName: string = workBook.SheetNames[0];
      const sheet: XLSX.WorkSheet = workBook.Sheets[sheetName];
  
      // Convert sheet data to JSON
      const jsonData: any[][] = XLSX.utils.sheet_to_json(sheet, { header: 1 });
  

      this.columnNames = jsonData[0];
      this.jsonData = jsonData.slice(1); // Exclude the header row

      // Assuming the first row contains column names
      const expectedColumnNames = [
        'Date', 'InvNo', 'PartyName', 'Address', 'Place', 'PartyPIN', 'PartyGSTIN',
        'PartyEmail', 'PartyMobileNo', 'TotalQuantity', 'Rate', 'Goods Values',
        'TaxableValue', 'SGST', 'CGST', 'RoundOff', 'BillAmount'
      ];
      const actualColumnNames: string[] = jsonData[0];

      // Check if the column names match the expected format
      if (!this.areColumnNamesValid(expectedColumnNames, actualColumnNames)) {
        // Display an error message or prevent further processing
        console.log('Invalid file format. Please upload a file with the correct column names.');
        this.toastr.error('Invalid file format. Please upload a valid Excel file.');
        return;
      }
      // Assuming the first row contains column names
      const columnNames: string[] = jsonData[0];
  
      // Process the data starting from the second row
      for (let i = 1; i < jsonData.length; i++) {
        const rowData: any[] = jsonData[i];
        // Create a Ledger object
        const ledger: Ledger = {
          companyId : parseInt(this.globalCompanyId),
          ledgerId: 0,
          ledgerName: "",
          gstin: rowData[columnNames.indexOf('PartyGSTIN')],
          address1: rowData[columnNames.indexOf('Address')],
          address2: rowData[columnNames.indexOf('Address')],
          emailId: rowData[columnNames.indexOf('PartyEmail')],
          cellNo: rowData[columnNames.indexOf('PartyMobileNo')],
          place: rowData[columnNames.indexOf('Place')],
          pin: rowData[columnNames.indexOf('PartyPIN')],
        };
  
        // Create a BillSummary object
        const billSummary: BillSummary = {
          companyId: Number(this.globalCompanyId),
          ledgerId: 0,
          vochType: 0,
          vochNo: 0,
          displayinvNo: rowData[columnNames.indexOf('InvNo')],
          tranctDate: new Date(rowData[columnNames.indexOf('Date')]),
          ponumber: "",
          stateCode2: rowData[columnNames.indexOf('PartyPIN')],
          cessValue: 0,
          csgstValue: +rowData[columnNames.indexOf('CGST')],
          igstValue: 0,
          sgstValue: +rowData[columnNames.indexOf('SGST')],
          billAmount:Number(+rowData[columnNames.indexOf('BillAmount')]),
          taxableValue: +rowData[columnNames.indexOf('TaxableValue')],
          expenseAmount1: 0,
          expenseAmount2: 0,
          expenseAmount3: 0,
          tcsValue: 0,
          advance: 0,
          totalAmount: 0,
          roundOff: +rowData[columnNames.indexOf('RoundOff')],
          isSEZ: 0,
          shipBillNo: "",
          portName: "",
          shipBillDate: new Date(rowData[columnNames.indexOf('Date')]),
          dispatcherName:"",
          dispatcherAddress1: rowData[columnNames.indexOf('Address')],
          dispatcherAddress2: rowData[columnNames.indexOf('Address')],
          dispatcherPlace: rowData[columnNames.indexOf('Place')],
          dispatcherPIN: rowData[columnNames.indexOf('PartyPIN')],
          dispatcherStatecode: "",
          stateCode1: "",
          transporter: "",
          distance: 0,
          lorryNo: "",
          deliveryName: "",
          deliveryAddress1: "",
          deliveryAddress2: "",
          deliveryPlace: "",
          delPinCode: "",
          totalWeight:0,
          partyInvoiceNumber: rowData[columnNames.indexOf('InvNo')],
          deliveryStateCode:"",
          IsServiceInvoice:true,
          // Add other properties based on your SQL query
        };
        // Add the objects to your dataList
        this.excelDataList.push({ Ledger: ledger, BillSummary: billSummary });
      }
    };
  
    reader.readAsBinaryString(file);
  }
   openEInvoicePage(customerId: string): void {
    const url = `https://einvoice.unnatisoftwares.com/invoice/Index?Customerid=${customerId}`;
    window.open(url, '_blank');
  }

  // This function could be triggered by a button click or any other event
  onOpenEInvoiceClick(): void {
    const customerId = 'YSHG'; // Replace this with the actual customer ID
    this.openEInvoicePage(customerId);
  }

  areColumnNamesValid(expectedNames: string[], actualNames: string[]): boolean {
    return expectedNames.every((name, index) => name === actualNames[index]);
  }
}


