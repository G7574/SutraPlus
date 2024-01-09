import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AdminServicesService } from 'src/app/admin/services/admin-services.service';
import { party } from 'src/app/share/models/party';

@Component({
  selector: 'app-akada-entry',
  templateUrl: './akada-entry.component.html',
  styleUrls: ['./akada-entry.component.scss']
})
export class AkadaEntryComponent implements OnInit {

  globalCompanyId: any;
  partyName: any;

  entries: any[] = [];
  commodities: any = [];
  selectedCommodity: any = null;

  // with ngModel
  date: any = null;
  commodityId: any = null;
  commodityName: any = null;
  lotNo: any = null;
  noOfBags: number = null;
  rate: number = 0;
  mark: any = null;
  totalWeight: any = null;
  amount: any = null;
  gridEntries: any[] = [];
  dynamicFields: any[] = [];
  ledgerId: any;
  voucherId: number;
  voucherNumber: number;

  selectedParty: any;

  searchTerm: string;
  partyList: party[] = [];

  // 
  commission: number = 0;
  cess: number = 0;
  taxableValue: number = 0;
  sgst: number = 0;
  otherExpenses: number = 0;
  roundOff: number = 0;
  grandTotal: number = 0;
  partyInvNo: number = 0;
  gstin: string = '';


  constructor(private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private adminService: AdminServicesService) {
    this.globalCompanyId = sessionStorage.getItem('companyID');
    this.partyName = sessionStorage.getItem('companyName');
  }

  ngOnInit() {


    this.dynamicFields = Array(this.noOfBags).fill(0);
    this.getAllCommodities();
  }

  onNoOfBagsChange(value: any) {
    this.dynamicFields = Array(value).fill(null);  // Initialize dynamicFields with null values
    this.calculateTotalAndAmount();
  }

  onRateChange() {
    // Recalculate totalWeight and amount whenever the rate changes
    this.calculateTotalAndAmount();
  }

  onDynamicFieldChange() {
    // Recalculate totalWeight and amount whenever the value of a dynamic field changes
    this.calculateTotalAndAmount();
  }

  calculateTotalAndAmount() {
    // Reset total
    this.totalWeight = 0;

    // Calculate total
    this.dynamicFields.forEach(field => {
      this.totalWeight += +field || 0;  // The '+' converts the string to a number
    });

    // Calculate amount
    let rate = +this.rate || 0;
    const totalWeight = +this.totalWeight || 0;
    const amount = Math.round((rate * totalWeight) / 100 * 100) / 100;

    // Update the value of the 'amount' form control
    this.amount = amount;
  }

  onNoClick(): void {
    this.date = null;
    this.commodityId = null;
    this.commodityName = null;
    this.lotNo = null;
    this.noOfBags = null;
    this.rate = 0;
    this.mark = null;
    this.totalWeight = null;
    this.amount = null;
    this.dynamicFields = [];
    this.selectedParty = null;
    this.voucherId = null;
    this.voucherNumber = null;
  }

  saveAkadaEntry() {
    // console.log(this.akadaEntryForm.value);
    console.log("save");
    const entry = {
      date: this.date,
      commodityId: this.commodityId,
      commodityName: this.commodityName,
      lotNo: this.lotNo,
      noOfBags: this.noOfBags,
      rate: this.rate,
      mark: this.mark,
      totalWeight: this.totalWeight,
      amount: this.amount,
      dynamicFields: this.dynamicFields
    };
    this.entries.push(entry);
    console.log("entry and dynacmic values", entry);
    let akadaData = {
      "AkadaData": {
        "CompanyId": this.globalCompanyId,
        "LotNo": this.lotNo,
        "Rate": this.rate,
        "NoOfBags": this.noOfBags,
        "TotalWeight": this.totalWeight,
        "Amount": this.amount,
        "Mark": this.mark,
        "CommodityId": this.selectedCommodity?.CommodityId || 0,
        "CommodityName": this.selectedCommodity?.CommodityName || ' ',
        "GridEntries": this.dynamicFields,
        "VoucherId": this.voucherId,
        "VoucherNumber": this.voucherNumber,
        "LedgerId": this.globalCompanyId || 0,
        "TranctDate": this.date,
        "Cess": this.cess,
        "SGST": this.sgst,
        "Taxable": this.taxableValue,
        "PartyInvoiceNumber": this.partyInvNo,
      }
    }

    console.log("akadaData", akadaData); // This will print the JSON string to the console

    this.adminService.saveAkadaEntry(akadaData).subscribe({
      next: (res: any) => {
        if (res == true) {
          // this.addCompany.reset();
          this.spinner.hide();
          this.toastr.success('Data Added Successfully!');
        } else {
          this.toastr.error('Something went wrong');
        }
      },
      error: (error: any) => {
        this.spinner.hide();
        // this.error = error;
        this.toastr.error('Something went wrong');
      },
    });
    // Implement your save logic here...
  }

  getAllCommodities() {
    let data = {
      "data": "data",     // dummy text just for calling the api
    };

    console.log("akadaData", data); // This will print the JSON string to the console

    this.adminService.getAllCommodities(data).subscribe({
      next: (res: any) => {
        // console.log("commodities", res);
        if (res) {
          // this.addCompany.reset();
          if (res.Commodities) {
            this.commodities = res.Commodities;
            // console.log("this.commodities", this.commodities);

          }
          this.spinner.hide();
        } else {
          this.toastr.error('Something went wrong');
        }
      },
      error: (error: any) => {
        this.spinner.hide();
        // this.error = error;
        this.toastr.error('Something went wrong');
      },
    });
  }

  getPartyList(text: string) {
    console.log("get party list text", text);
    if (text.length >= 1) {
      let partyDetails = {
        "LedgerData": {
          "CompanyId": this.globalCompanyId,
          "LedgerType": "Sales Ledger",
          "Country": ""
        },
        "SearchText": text,
        "Page": {
          "PageNumber": "1",
          "PageSize": "10"
        }
      }
      this.adminService.getLedgerList(partyDetails).subscribe({
        next: (res: any) => {
          for (let data of res.records) {
            data.ledgerName = `${data.ledgerName} - ${data.place}`;
          }
          this.partyList = res.records;
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Something went wrong');
        },
      });
    }
    else {
      this.partyList = [];
    }
  }

  onClickTest($event: any, item: any) {
    this.ledgerId = item.ledgerId;
    let invoiceData;
    let payload = {
      SalesDetails: {
        CompanyId: this.globalCompanyId,
        LedgerId: item.ledgerId,
        DealerType: item.dealerType,
      },
    };
    this.adminService.getVoucherTypeAndInvoiceNo(payload).subscribe({
      next: (res: any) => {
        this.spinner.show();
        if (!res.HasErrors && res?.Data !== null) {
          console.log("get voucher and invoice", res);
          invoiceData = res;
        } else {
          this.toastr.error(res.Errors[0].Message);
        }
        this.spinner.hide();
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong');
      },
    });
  }

}
