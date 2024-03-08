import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AdminServicesService } from 'src/app/admin/services/admin-services.service';
import { party } from 'src/app/share/models/party';
import { SelectBankDailogComponent } from '../../../select-bank-dailog/select-bank-dailog.component';
import { BillExpenseRateDailogComponent } from '../../../bill-expense-rate-dailog/bill-expense-rate-dailog.component';
import { DecimalPipe } from '@angular/common';
import { add } from 'date-fns';

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
  commodityList: any = [];
  selectedCommodity: any = null;

  markEntries: any[] = [];

  // with ngModel
  date: any = null;
  commodityId: any = null;
  commodityName: any = null;
  lotNo: any = null;
  noOfBags: number = null;
  rate: number = 0;
  mark: any = null;
  totalWeight: any = 0;
  amount: any = null;
  gridEntries: any[] = [];
  dynamicFields: any[] = [];

  ledgerId: any;
  //voucherId: number;
  //voucherNumber: number;

  selectedParty: any;

  searchTerm: string;
  partyList: party[] = [];

  //
  commission: any = 0;
  cess: any = 0;
  taxableValue: any = 0;
  sgst: any = 0;
  cgst: any = 0;
  otherExpenses: any = 0;
  roundOff: any = 0;
  grandTotal: any = 0;
  partyInvNo: any = 0;
  gstin: string = '';

  totalBags : any;
  ttbag: any;
  average : any;
  packing : any = 0;
  hamali: any = 0;
  dalali: any;
  weightManFee: any = 0;

  showTotalWeight : boolean = false;

  lastlyAddedPartyName : any;
  lastlyAddedAate : any;
  lastlyAddedLotNo : any;
  lastltAddedNumOfBags : any;

  constructor(private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private adminService: AdminServicesService,
    private dialog: MatDialog) {
    this.globalCompanyId = sessionStorage.getItem('companyID');
    this.partyName = sessionStorage.getItem('companyName');
  }

  billExpenseRates() {
    this.dialog.open(BillExpenseRateDailogComponent, {
      width: '1000px',
      disableClose: false,
    });
  }

  ngOnInit() {

    this.date = (new Date()).toISOString().substring(0,10);
    this.dynamicFields = Array(this.noOfBags).fill(0);
    this.inputFieldValues = Array(this.noOfBags).fill(0);
    this.getAllCommodities();
    this.getOrderByMarkData();
    this.getLastlyAddedRecordFromInventory();

  }

  initData() {
    this.spinner.show();

    let akadaData = {
      "AkadaData": {
        "CompanyId": this.globalCompanyId,
        "LotNo": this.lotNo,
        "Rate": this.rate,
        "NoOfBags": this.ttbag,
        "TotalWeight": this.totalWeight,
        "Amount": this.totalBags,
        "Mark": this.mark,
        "CommodityId": this.commodityId || 0,
        "CommodityName": this.commodityName || ' ',
        "GridEntries": this.inputFieldValues,
        "LedgerId": this.ledgerId || 0,
        "TranctDate": this.date,
        "Cess": this.cess,
        "SGST": this.sgst,
        "Taxable": this.taxableValue,
        "PartyInvoiceNumber": this.partyInvNo,
      }
    }

    this.adminService.GetEditVerifyBillData(akadaData).subscribe({
      next: (res: any) => {

        this.commission = res.commission;
        this.cess = res.cess;
        this.taxableValue = res.taxableValue;
        this.sgst = res.sgst;
        this.cgst = res.cgst;
        this.roundOff = res.roundOff;
        this.grandTotal = res.grandTotal;
        this.partyInvNo = res.partyInvoiceNumber;
        this.gstin = res.gstin;
        this.weightManFee = res.weightManFee;
        this.packing = res.packing;
        this.hamali = res.hamali;
        //this.date = this.formatDate(res.trancdate);

        this.totalBags = this.addDecimal(this.totalBags);

        this.roundOff = this.addDecimal(Number(this.roundOff));
        this.grandTotal = this.addDecimal(Number(this.grandTotal));
        this.weightManFee = this.addDecimal(Number(this.weightManFee));
        this.packing = this.addDecimal(Number(this.packing));
        this.hamali = this.addDecimal(Number(this.hamali));

        this.commission = this.addDecimal(Number(this.commission));
        this.cess = this.addDecimal(Number(this.cess));
        this.taxableValue = this.addDecimal(Number(this.taxableValue));
        this.sgst = this.addDecimal(Number(this.sgst));
        this.cgst = this.addDecimal(Number(this.cgst));

        this.commodityId = res.commodityId;

        this.commodityName = res.commodityName;
        this.partyName = res.PartyName;

        if (this.commodityId != 0 && this.ledgerId != 0) {
          let send = {
            "AkadaData": {
              "CompanyId": this.globalCompanyId,
              "LotNo": 0,
              "CommodityId": this.commodityId || 0,
              "LedgerId": this.ledgerId || 0,
              "TranctDate": this.date,
            }
          }
          this.getLotData(send);
        }

        this.lotNo = null;
        this.mark = null;
        this.noOfBags = null;
        this.rate = null;
        //this.totalWeight = null;
        this.amount = null;
        this.dynamicFields = [];

        this.spinner.hide();


      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong while getting last added record');
      },
    });

  }

  getLastlyAddedRecordFromInventory() {
    this.spinner.show();
    let akadaData = {
      "AkadaData": {
        "CompanyId": this.globalCompanyId,
      }
    }

    this.adminService.GetLastlyAddedRecord(akadaData).subscribe({
      next: (res: any) => {

            this.lastlyAddedPartyName = res.PartyName;
            this.lastlyAddedAate = res.TranctDate;
            this.lastlyAddedLotNo = res.LotNo;
            this.lastltAddedNumOfBags = res.NoOfBags;
            this.spinner.hide();

      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong while getting last added record');
      },
    });
  }

  onNoOfBagsChange(value: any) {
    this.dynamicFields = Array(value).fill(null);  // Initialize dynamicFields with null values
    this.inputFieldValues = Array(value).fill(null);
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
  showParticularBag : boolean = true;
  totalWeightCalculation() {

    if(this.totalWeight > 0) {
      this.showParticularBag = false;
    } else {
      this.showParticularBag = true;
    }

    let rate = +this.rate || 0;
    const totalWeight = +this.totalWeight || 0;
    const amount = Math.round((rate * totalWeight) / 100 * 100) / 100;

    if(totalWeight > 0) {
      this.showTotalWeight = true;
    } else {
      this.showTotalWeight = false;
    }

    // Update the value of the 'amount' form control
    this.amount = amount;
  }

  calculateTotalAndAmount() {
    // Reset total
    this.totalWeight = 0;

    // Calculate total
    this.inputFieldValues.forEach(field => {
      this.totalWeight += +field || 0;  // The '+' converts the string to a number
    });

    // Calculate amount
    let rate = +this.rate || 0;
    const totalWeight = +this.totalWeight || 0;
    const amount = Math.round((rate * totalWeight) / 100 * 100) / 100;

    if(totalWeight > 0) {
      this.showTotalWeight = true;
    } else {
      this.showTotalWeight = false;
    }

    // Update the value of the 'amount' form control
    this.amount = amount;
  }

  getTotal(property: string): number {
    this.totalBags = this.entries.reduce((total, entry) => total + entry[property], 0);
    return this.entries.reduce((total, entry) => total + entry[property], 0);
  }
  getTotalBag(property: string): number {
    this.ttbag = this.entries.reduce((total, entry) => total + entry[property], 0);
    return this.entries.reduce((total, entry) => total + entry[property], 0);
  }
  getTotl(property: string): number {
    this.totalWeight = this.entries.reduce((total, entry) => total + entry[property], 0);
    return this.markEntries.reduce((total, entry) => total + entry[0][property], 0);
  }

onKeyUp(event: any,ab : boolean) {

  if(this.commodityId == null) {
    this.toastr.error('Please Select the Commodity!');
    return;
  }

  if(this.lotNo == null || this.lotNo == "") {
    return;
  }

console.log("this.lotNo -> " + this.lotNo)

    let akadaData = {
      "AkadaData": {
        "lotNo": this.lotNo,
        "date": this.date,
        "CompanyId": this.globalCompanyId,
        "ledgerId": this.ledgerId,
        "commodityId": this.commodityId,
      }
    }


    this.adminService.GetLotNO(akadaData).subscribe({
      next: (res: any) => {

        if(res == true) {
            if(ab) {
              this.saveAkadaEntry();
            }
        } else {
          this.toastr.error('Cannot use the same Lot No Today !');
        }

      },
      error: (error: any) => {

      },
    });

}

  onNoClick(): void {
    this.date = (new Date()).toISOString().substring(0,10);
    this.commodityId = null;
    this.commodityName = null;
    this.lotNo = null;
    this.noOfBags = null;
    this.rate = 0;
    this.mark = null;
    this.totalWeight = null;
    this.amount = null;
    this.dynamicFields = [];
    this.inputFieldValues = [];
    this.selectedParty = null;
    // this.voucherId = null;
    // this.voucherNumber = null;
  }

  inputFieldValues: string[] = [];

  counter = 0;

  getLotData(send: any) {
    this.adminService.GetLotNoData(send).subscribe({
      next: (res: any) => {

        const entry = res.existingInventory;
        this.entries = entry;

        this.counter++;

        if(this.counter < 3) {
          this.initData();
        } else {
          this.counter = 0;
        }

      },
      error: (error: any) => {
        this.spinner.hide();
        // this.error = error;
        console.log("error2 ->" + error);
        this.toastr.error('Something went wrong while getting lot no data');
      },
    });
  }

  onDateChange(newDate: string) {
    this.getOrderByMarkData();
  }

  getOrderByMarkData() {

    this.markEntries = [];
    let akadaData = {
      "AkadaData": {
        "CompanyId": this.globalCompanyId,
        "TranctDate" : this.date
      }
    }

    this.adminService.GetOrderByMark(akadaData).subscribe({
      next: (res: any) => {

        const entry = res.Mark;
        this.markEntries = entry;

      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong while getting mark records');
      },
    });
  }

  saveAkadaEntry() {
    // console.log(this.akadaEntryForm.value);
    console.log("save" + this.commodityList);
    this.spinner.show();

    let send = {
      "AkadaData": {
        "CompanyId": this.globalCompanyId,
        "LotNo": this.lotNo,
        "CommodityId": this.commodityId || 0,
        "LedgerId": this.ledgerId || 0,
        "TranctDate": this.date,
      }
    }

    if(this.mark == "" || this.mark == null || this.mark == undefined) {
      this.mark = "*-*";
    }

    let akadaData = {
      "AkadaData": {
        "CompanyId": this.globalCompanyId,
        "LotNo": this.lotNo,
        "Rate": this.rate,
        "NoOfBags": this.noOfBags,
        "TotalWeight": this.totalWeight,
        "Amount": this.amount,
        "Mark": this.mark,
        "CommodityId": this.commodityId || 0,
        "CommodityName": this.commodityName || ' ',
        "GridEntries": this.inputFieldValues,
        // "VoucherId": this.voucherId,
        // "VoucherNumber": this.voucherNumber,
        "LedgerId": this.ledgerId || 0,
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

          // this.addCompany.reset();

          this.getLotData(send);

          // const entry = {
          //   date: this.date,
          //   commodityId: this.commodityId,
          //   commodityName: this.commodityName,
          //   lotNo: this.lotNo,
          //   noOfBags: this.noOfBags,
          //   rate: this.rate,
          //   mark: this.mark,
          //   totalWeight: this.totalWeight,
          //   amount: this.amount,
          //   dynamicFields: this.inputFieldValues
          // };

          console.log("res ->" + res);

          this.commission = res.commission;
          this.cess = res.cess;
          this.taxableValue = res.taxableValue;
          this.sgst = res.sgst;
          this.cgst = res.cgst;
          //this.otherExpenses = res.otherExpenses;
          this.roundOff = res.roundOff;
          this.grandTotal = res.grandTotal;
          this.partyInvNo = res.partyInvoiceNumber;
          this.gstin = res.gstin;
          this.weightManFee = res.weightManFee;
          this.packing = res.packing;
          this.hamali = res.hamali;

          this.totalBags = this.addDecimal(this.totalBags);
          this.roundOff = this.addDecimal(Number(this.roundOff));
          this.grandTotal = this.addDecimal(Number(this.grandTotal));
          this.weightManFee = this.addDecimal(Number(this.weightManFee));
          this.packing = this.addDecimal(Number(this.packing));
          this.hamali = this.addDecimal(Number(this.hamali));

          this.commission = this.addDecimal(Number(this.commission));
          this.cess = this.addDecimal(Number(this.cess));
          this.taxableValue = this.addDecimal(Number(this.taxableValue));
          this.sgst = this.addDecimal(Number(this.sgst));
          this.cgst = this.addDecimal(Number(this.cgst));

          // this.entries = [];
          // this.entries.push(entry);



          this.spinner.hide();
         // this.toastr.success('Data Added Successfully!');

          // this.commodityId = null;
          // this.commodityName = null;
          this.lotNo = null;
          this.noOfBags = null;
          this.rate = 0;
          this.mark = null;
          this.totalWeight = null;
          this.amount = null;
          this.dynamicFields = [];
          this.inputFieldValues = [];
          this.selectedParty = null;
          this.showTotalWeight = false;

          this.getLastlyAddedRecordFromInventory();

      },
      error: (error: any) => {
        this.spinner.hide();
        // this.error = error;
        console.log("error ->" + error);
        this.toastr.error('Something went wrong');
      },
    });
    // Implement your save logic here...
  }

  addDecimal(number: number): string {
    if (Number.isInteger(number)) {
      return `${number}.00`;
    } else {
      return number.toFixed(2);
    }
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
            this.commodityList = this.commodities;

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

  getCommodity(text: string) {
    console.log("Search term:", text); // Log the search term
    if (text === null) {
        console.log("Search term is null. Showing all commodities.");
        this.commodityList = this.commodities.map((commodity: any) => commodity.CommodityName);
        console.log("Filtered commodities:", this.commodityList); // Log the filtered commodities
    } else if (!text.trim()) {
        console.log("Input is empty. Showing all commodities.");
        this.commodityList = this.commodities.map((commodity: any) => commodity.CommodityName);
        console.log("Filtered commodities:", this.commodityList); // Log the filtered commodities
    } else {
        console.log("Filtering commodities based on input.");
        this.commodityList = this.commodities
            .filter((commodity: any) => commodity.CommodityName.toLowerCase().includes(text.toLowerCase()))
            .map((commodity: any) => commodity.CommodityName);
        console.log("Filtered commodities:", this.commodityList); // Log the filtered commodities
    }
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

  set($event: any, item: any) {
    console.log("item->" + item);
    this.selectedCommodity = item;
    this.commodityName = item;
    for (let index = 0; index < this.commodities.length; index++) {
        if(this.commodities[index].CommodityName == item){
          this.commodityId = this.commodities[index].CommodityId;
          break;
        }
    }
  }

  onClickTest($event: any, item: any) {
    this.ledgerId = item.ledgerId;

    if(item.dalaliRate == null || item.dalaliRate == "") {
      sessionStorage.setItem('partyName',item.ledgerName);
      sessionStorage.setItem('ledgerId',item.ledgerId);
      sessionStorage.setItem('cessRate',item.cessRate);
      sessionStorage.setItem('weighManFeeRate',item.weighManFeeRate);
      sessionStorage.setItem('packingRate',item.packingRate);
      sessionStorage.setItem('hamaliRate',item.hamaliRate);
      sessionStorage.setItem('dalaliRate',item.dalaliRate);

      this.cess = item.cessRate;
      this.weightManFee = item.weighManFeeRate;
      this.packing = item.packingRate;
      this.hamali = item.hamaliRate;
      this.dalali = item.dalaliRate;

      this.billExpenseRates();
    }

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


    if(this.commodityId != 0 && this.ledgerId != 0) {
      let send = {
        "AkadaData": {
          "CompanyId": this.globalCompanyId,
          "LotNo": 0,
          "CommodityId": this.commodityId || 0,
          "LedgerId": this.ledgerId || 0,
          "TranctDate": this.date,
        }
      }
      this.getLotData(send);
    }




  }


}
