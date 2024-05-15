import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AdminServicesService } from 'src/app/admin/services/admin-services.service';
import { CommonService } from 'src/app/share/services/common.service';
import { party } from 'src/app/share/models/party';
import { product } from 'src/app/share/models/product';
import { add, format } from 'date-fns';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { el, th, tr } from 'date-fns/locale';
import { DatePipe, formatDate } from '@angular/common';
import { Items } from '../../models/items'
import { Location } from '@angular/common';
import ro from 'date-fns/locale/ro/index.js';
declare var $: any;

@Component({
  selector: 'app-add-export-invoice',
  templateUrl: './add-export-invoice.component.html',
  styleUrls: ['./add-export-invoice.component.scss']
})
export class AddExportInvoiceComponent implements OnInit {
  submitted: boolean = false
  public visible = false;
  selectedCar?: any;
  cars = [
    { id: 1, name: 'Volvo' },
    { id: 2, name: 'Saab' },
    { id: 3, name: 'Opel' },
    { id: 4, name: 'Audi' },
  ];
  globalCompanyId!: number;
  ledgerId!: number;
  searchText!: string;
  partyList: party[] = [];
  otherAccList: party[] = [];
  stateList: { "Statecode": string, "Statename": string }[] = [];
  todayDate?: string
  productList: product[] = [];
  kgPerBag!: number;
  noOfBags!: number;
  totalQty!: number;
  rate!: number;
  getParty!: FormGroup;
  formGroup1!: FormGroup;
  formGroup2!: FormGroup;
  formGroup3!: FormGroup;
  formGroup4!: FormGroup;

  i!: number;
  // dynamicArray: string[] = [];
  newDynamic: any = {};
  disableRates: boolean = false;
  // SGSTRate!: number;
  // CGSTRate!: number;
  // IGSTRate!: number;
  totalFr!: number;
  advancePaid!: number;
  balanceFr!: number;
  itemClicked!: string;
  sgstShow: boolean = true;
  cgstShow: boolean = true;
  igstShow: boolean = true;
  voucherTypeText!: string;
  voucherTypeId!: number;
  myDate = new Date();

  public dynamicArray: Array<Items> = [];
  invoiceNo!: number;
  ExpenseName1!: string;
  ExpenseName2!: string;
  ExpenseName3!: string;
  newRate!: number;
  secondTime: boolean = false;
  companyPlace: string | null;
  totalNoOfBags!: number;
  totalQuantity!: number;
  totalAmount!: number;
  othrChrgs1!: number;
  othrChrgs2!: number;
  othrChrgs3!: number;
  taxableAmt!: number;
  grandTotalAmount!: number;
  discountAmount!: number;
  dealerType!: string;
  PANNumber!: string;
  GSTNumber!: string;
  defaultState!: string;
  totalOtherCharges: number = 0;
  advPaid!: number;
  otherChargesInfo: boolean = false

  constructor(private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    public commonService: CommonService,
    private datePipe: DatePipe,
    private location: Location) {

    this.globalCompanyId = Number(sessionStorage.getItem('companyID'));
    this.companyPlace = sessionStorage.getItem('companyPlace');
    var date = new Date();
    this.todayDate = formatDate(new Date(), 'dd-MM-yyyy', 'en-US')
    this.newRate = 0
  }

  ngOnInit(): void {
    this.spinner.show()
    this.getStateList();

    this.getParty = new FormGroup({
      dealerType: new FormControl('', [Validators.required]),
      pan: new FormControl('', [Validators.required]),
      gst: new FormControl(''),
      voucherType: new FormControl('', [Validators.required]),
      invoiceNo: new FormControl('', [Validators.required]),
      state: new FormControl('', [Validators.required])
    });

    this.formGroup1 = new FormGroup({
      otherChargesAny: new FormControl(''),
      otherChargesAnyValue: new FormControl(''),
      otherCharges1: new FormControl(''),
      otherCharges2: new FormControl(''),
      otherCharges3: new FormControl(''),
    });

    this.formGroup2 = new FormGroup({
      taxableAmt: new FormControl(''),
      discount: new FormControl(''),
      sezSale: new FormControl(''),
      sgstRate: new FormControl(''),
      cgstRate: new FormControl(''),
      igstRate: new FormControl(''),
      totalGst: new FormControl(''),
      totalGstAmt: new FormControl(''),
      roundOff: new FormControl(''),
      grandTotal: new FormControl(''),
    });

    this.formGroup4 = new FormGroup({
      poNo: new FormControl('', [Validators.required]),
      eWayBill: new FormControl('', [Validators.required]),
      transporter: new FormControl('', [Validators.required]),
      lorryNo: new FormControl('', [Validators.required]),
      owner: new FormControl('', [Validators.required]),
      driver: new FormControl('', [Validators.required]),
      dlNo: new FormControl('', [Validators.required]),
      checkPost: new FormControl('', [Validators.required]),
      frieght_bag: new FormControl('', [Validators.required]),
      totalFrieght: new FormControl('', [Validators.required]),
      advancePaid: new FormControl('', [Validators.required]),
      balanceFrieght: new FormControl('', [Validators.required]),
      frieght_Plus_Less: new FormControl(''),
      tds: new FormControl('', [Validators.required]),
      partyName: new FormControl('', [Validators.required]),
      address1: new FormControl('', [Validators.required]),
      address2: new FormControl('', [Validators.required]),
      place: new FormControl('', [Validators.required]),
      pinCode: new FormControl('', [Validators.required]),
      state: new FormControl('', [Validators.required]),
      stateCode: new FormControl('', [Validators.required]),
      distance: new FormControl('', [Validators.required]),
      note: new FormControl('', [Validators.required]),
    });

    this.formGroup3 = new FormGroup({
      address: new FormControl('', [Validators.required]),
      toPlace: new FormControl('', [Validators.required]),
      fromPlace: new FormControl('', [Validators.required]),
    });

    this.newDynamic = { CommodityId: "", WeightPerBag: "", NoOfBags: "", TotalWeight: "", Rate: "", Amount: "", Remark: "", SgstAmount: 0, CgstAmount: 0, IgstAmount: 0, NoOfDocra: 0 };
    // this.newDynamic = { CommodityId: "", WeightPerBag: "", NoOfBags: "", TotalWeight: "", Rate: "", Amount: "", Remark: "", GrandTotal: this.grandTotalAmount, SgstAmount: 0, CgstAmount: 0, IgstAmount: 0, NoOfDocra: 0 };
    this.dynamicArray.push(this.newDynamic);

    this.spinner.hide();
  }

  toggleLiveDemo() {
    this.visible = !this.visible;
  }

  saveLorryDetails() {
    if (this.formGroup4.valid) {
      this.formGroup1.controls['otherChargesAny'].setValue(this.formGroup4.value.frieght_Plus_Less)
      this.formGroup1.controls['otherChargesAnyValue'].setValue(this.formGroup4.value.advancePaid)
      this.taxableAmt = this.grandTotalAmount = Number(this.formGroup4.value.advancePaid) + Number(this.totalAmount)

      this.taxableAmt = this.grandTotalAmount = Number(this.taxableAmt.toFixed(2))
      sessionStorage.setItem('grandTotal', String(this.grandTotalAmount))
      this.calculateTotalTaxableAmount();
      this.calculateRoundOff();
      this.otherChargesInfo = true
      this.visible = !this.visible;
    } else {
      this.toastr.error('Please fill mondatory fields')
    }

  }

  handleLiveDemoChange(event: any) {
    this.visible = event;
  }

  getPartyList(text: string) {
    if (text.length >= 1) {
      let partyDetails = {
        "LedgerData": {
          "CompanyId": this.globalCompanyId,
          "LedgerType": "Sales Ledger",
          "Country": "Export"
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
    this.ledgerId = item.ledgerId
    let invoiceData;
    let payload = {
      "SalesDetails": {
        "CompanyId": this.globalCompanyId,
        "LedgerId": item.ledgerId,
        "DealerType": item.dealerType
      }
    }
    this.adminService.getVoucherTypeAndInvoiceNo(payload).subscribe({
      next: (res: any) => {
        this.spinner.show();
        if (!res.HasErrors && res?.Data !== null) {
          invoiceData = res;
          this.setData(item)
          this.setData2(invoiceData)
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

  getOtherAccList(text: string) {
    if (text.length >= 1) {
      let partyDetails = {
        "LedgerData": {
          "CompanyId": this.globalCompanyId,
          "LedgerType": "Sales Other Ledger"
        },
        "SearchText": text,
        "Page":
        {
          "PageNumber": "1",
          "PageSize": "10"
        }
      }

      this.adminService.getLedgerList(partyDetails).subscribe({
        next: (res: any) => {
          // for (let data of res.records) {
          //   data.ledgerName = `${data.ledgerName} - ${data.place}`;
          // }
          this.otherAccList = res.records;
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

  onOtherAcc1(item: any) {
    this.ExpenseName1 = item.ledgerName
  }

  onOtherAcc2(item: any) {
    this.ExpenseName2 = item.ledgerName
  }
  onOtherAcc3(item: any) {
    this.ExpenseName3 = item.ledgerName
  }

  setData(list: any): void {
    if (list) {
      let address, address1, address2
      if (list.address1 != '' && list.address1 != null && list.address1 != undefined) {
        address1 = list.address1
      } else {
        address1 = '-'
      }

      if (list.address2 != '' && list.address2 != null && list.address2 != undefined) {
        address2 = list.address2
      } else {
        address2 = '-'
      }

      address = `${address1} ${address2}`

      if (address == '- -') {
        address = '-'
      } else {
        address = `${address1} ${address2}`
      }

      this.formGroup4.controls['partyName'].setValue(list.ledgerName)
      this.getParty.controls['dealerType'].setValue(list.dealerType);
      this.getParty.controls['pan'].setValue(list.pan);
      this.getParty.controls['gst'].setValue(list.gstn);
      this.getParty.controls['state'].setValue(list.state);
      this.formGroup3.controls['address'].setValue(address);
      this.formGroup3.controls['fromPlace'].setValue(this.companyPlace);
      this.formGroup3.controls['toPlace'].setValue(list.place);
      this.formGroup4.controls['address1'].setValue(list.address1);
      this.formGroup4.controls['address2'].setValue(list.address2);
      this.dealerType = list.dealerType;
      this.PANNumber = list.pan;
      this.GSTNumber = list.gstn;
      this.defaultState = list.state
    }
  }

  setData2(list: any): void {
    if (list) {
      this.getParty.controls['voucherType'].setValue(list.VoucherType);
      this.getParty.controls['invoiceNo'].setValue(list.InvoiceNo);
      this.formGroup3.controls['fromPlace'].setValue(this.companyPlace);
      this.voucherTypeText = list.VoucherType
      this.voucherTypeId = list.VoucherId
      this.invoiceNo = list.InvoiceNo

      if (list.VoucherType == 'Export Sale') {
        this.igstShow = true
        this.cgstShow = this.sgstShow = false
      } else if (list.VoucherType == 'Local Sale') {
        this.igstShow = false
        this.cgstShow = this.sgstShow = true
      } else if (list.VoucherType == 'Interstate Sale') {
        this.igstShow = true
        this.cgstShow = this.sgstShow = false
      } else if ((list.VoucherType == 'URD Sale')) {
        this.igstShow = true
        this.cgstShow = this.sgstShow = false
      }
      else {

      }
    }
  }

  getStateList(): void {
    this.adminService.getStates().subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          this.stateList = res.StateList;
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

  getProductList(text: string): void {

    // if (this.secondTime == false) {
    if (text.length >= 1) {
      let UserDetails = {
        "SearchText": text,
        "Page": {
          "PageNumber": 1,
          "PageSize": 10
        }
      }
      this.adminService.getProductList(UserDetails).subscribe({
        next: (res: any) => {
          if (this.secondTime == false) {
            this.productList = res.records
          }
          else {
            this.productList = [];
            for (let data of res.records) {
              if (data.igst === this.newRate) {
                this.productList.push(data);
              }
            }
          }
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Something went wrong');
        },
      });
    }
    else {
      this.productList = [];
    }
  }

  getNewProductList(text: string): void {
    if (text.length >= 1) {
      let UserDetails = {
        "SelesItem": {
          "Name": "tur",
          "GSTType": "34.3000"
        }
      }
      this.adminService.getNewProductList(UserDetails).subscribe({
        next: (res: any) => {
          this.productList = res.records
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Something went wrong');
        },
      });
    }
    else {
      this.productList = [];
    }
  }

  getToday(): string {
    return new Date().toISOString().split('T')[0]
  }

  calculateTotalNoOfBags(): void {
    let sumVal: number = 0;
    for (var i = 0; i < this.dynamicArray.length; i++) {
      let tq = Number(this.dynamicArray[i].NoOfBags)
      sumVal = Number(sumVal + tq);
    }
    this.totalNoOfBags = sumVal
  }

  calculateTotalQuantity(): void {
    let sumVal: number = 0;

    for (var i = 0; i < this.dynamicArray.length; i++) {
      let tq = Number(this.dynamicArray[i].TotalWeight)
      sumVal = Number(sumVal + tq);
    }
    this.totalQuantity = Number(sumVal)
  }

  calculateTotalAmount(): void {
    let sumVal: number = 0;
    for (var i = 0; i < this.dynamicArray.length; i++) {
      sumVal = Number((sumVal + this.dynamicArray[i].Amount).toFixed(2));
    }
    this.totalAmount = this.taxableAmt = this.grandTotalAmount = Number(sumVal)
    sessionStorage.setItem('grandTotal', String(this.grandTotalAmount))
  }

  changeKgBag(val: any, i: any): void {
    this.dynamicArray[i].WeightPerBag = val
    if (this.dynamicArray[i].NoOfBags > 0) {
      this.dynamicArray[i].TotalWeight = Number(this.dynamicArray[i].WeightPerBag * this.dynamicArray[i].NoOfBags)
    } else {
      this.dynamicArray[i].TotalWeight = Number(this.dynamicArray[i].WeightPerBag * 0)
    }
    this.calculateTotalQuantity();
  }

  changeNoOfBag(val: any, i: any): void {
    this.dynamicArray[i].NoOfBags = val
    if (this.dynamicArray[i].WeightPerBag > 0) {
      this.dynamicArray[i].TotalWeight = Number(this.dynamicArray[i].WeightPerBag * this.dynamicArray[i].NoOfBags)
    } else {
      this.dynamicArray[i].TotalWeight = Number(this.dynamicArray[i].NoOfBags * 0)
    }
    this.calculateTotalNoOfBags();
    this.calculateTotalQuantity();
  }

  changeQty(val: any, i: any): void {
    this.dynamicArray[i].TotalWeight = val
    this.dynamicArray[i].Amount = Number((this.dynamicArray[i].TotalWeight * this.dynamicArray[i].Rate) / 100)
    this.calculateTotalQuantity();
    this.calculateTotalAmount();
  }

  calAmt(rate: any, i: any): void {
    this.dynamicArray[i].Rate = rate
    let amt = (this.dynamicArray[i].TotalWeight * rate) / 100
    this.dynamicArray[i].Amount = Number(amt.toFixed(2))
    this.calculateTotalAmount();
    this.calculateRoundOff();
  }

  addRow(index: number) {
    // this.newDynamic = { CommodityId: "", WeightPerBag: "", NoOfBags: "", TotalWeight: "", Rate: "", Amount: "", Remark: "", GrandTotal: this.grandTotalAmount, SgstAmount: "", CgstAmount: "", IgstAmount: "", NoOfDocra: "" };
    this.newDynamic = { CommodityId: "", WeightPerBag: "", NoOfBags: "", TotalWeight: "", Rate: "", Amount: "", Remark: "", SgstAmount: "", CgstAmount: "", IgstAmount: "", NoOfDocra: "" };
    this.dynamicArray.push(this.newDynamic);
    this.secondTime = true
    return true;
  }

  deleteRow(index: number) {
    if (this.dynamicArray.length == 1) {
      return false;
    } else {
      this.dynamicArray.splice(index, 1);
      this.calculateTotalNoOfBags();
      this.calculateTotalQuantity();
      this.calculateTotalAmount();
      return true;
    }
  }

  fieldsChange(values: any): void {
    this.disableRates = values.currentTarget.checked
    if (this.disableRates) {
      this.formGroup2.controls['sgstRate'].setValue(0);
      this.formGroup2.controls['cgstRate'].setValue(0);
      this.formGroup2.controls['igstRate'].setValue(0);
      this.formGroup2.controls['igstRate'].setValue(0);
      this.formGroup2.controls['totalGst'].setValue(0);
      this.formGroup2.controls['totalGstAmt'].setValue(0);

      let grndTotal = sessionStorage.getItem('grandTotal');

      let perc = Number(this.grandTotalAmount) * (0 / 100)
      let val = Number(perc + Number(grndTotal)).toFixed(2)
      this.grandTotalAmount = Number(val)
      this.calculateRoundOff();
    }
  }

  onChangeSGST() {
    this.formGroup2.controls['cgstRate'].setValue(this.formGroup2.value.sgstRate);
    let sum = Number(this.formGroup2.value.sgstRate) + Number(this.formGroup2.value.cgstRate);
    this.formGroup2.value.sgstRate.charAt(0) !== '.' ? this.formGroup2.controls['totalGst'].setValue(sum) : '';
    let grndTotal = sessionStorage.getItem('grandTotal');

    let perc = Number(grndTotal) * (sum / 100)
    let val = Number(perc + Number(grndTotal)).toFixed(2)
    this.formGroup2.controls['totalGstAmt'].setValue(perc.toFixed(2))
    this.grandTotalAmount = Number(val)
    this.calculateRoundOff();
  }

  onChangeCGST() {
    this.formGroup2.controls['sgstRate'].setValue(this.formGroup2.value.cgstRate);
    let sum = Number(this.formGroup2.value.sgstRate) + Number(this.formGroup2.value.cgstRate);
    this.formGroup2.value.cgstRate.charAt(0) !== '.' ? this.formGroup2.controls['totalGst'].setValue(sum) : '';
    let grndTotal = sessionStorage.getItem('grandTotal');

    let perc = Number(grndTotal) * (sum / 100)
    let val = Number(perc + Number(grndTotal)).toFixed(2)
    this.formGroup2.controls['totalGstAmt'].setValue(perc.toFixed(2))
    this.grandTotalAmount = Number(val)
    this.calculateRoundOff();
  }

  onChangeIGST() {
    this.formGroup2.controls['totalGst'].setValue(this.formGroup2.value.igstRate);
    let grndTotal = sessionStorage.getItem('grandTotal')

    let perc = Number(grndTotal) * (this.formGroup2.value.igstRate / 100)
    this.formGroup2.controls['totalGstAmt'].setValue(perc.toFixed(2))
    let val = Number(perc + Number(grndTotal)).toFixed(2)
    this.grandTotalAmount = Number(val)
    this.calculateRoundOff();
  }

  changeTotFr(amt: any): void {
    if (amt) {
      this.totalFr = amt

      if (this.advancePaid == 0 || this.advancePaid == undefined) {
        this.balanceFr = amt - 0
      } else {
        this.balanceFr = amt - Number(this.advancePaid)
      }
    }
  }

  changeAdvPaid(amt: any): void {
    if (amt) {
      this.advancePaid = amt
      this.balanceFr = this.totalFr - this.advancePaid
    }
  }

  productItemClick($event: any, item: any, ind: number) {
    if (item.commodityName.length >= 1) {
      let UserDetails = {
        "SelesItem": {
          "Name": "",
          "GSTType": item.igst
        }
      }
      this.adminService.getNewProductList(UserDetails).subscribe({
        next: (res: any) => {
          this.productList = []
          this.productList = res.Commodities
          this.newRate = res.Commodities[0].IGST
          this.dynamicArray[ind].SgstAmount = item.sgst
          this.dynamicArray[ind].CgstAmount = item.cgst
          this.dynamicArray[ind].IgstAmount = item.igst
          this.dynamicArray[ind].CommodityId = item.commodityId
          this.dynamicArray[ind].NoOfDocra = 0
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Something went wrong');
        },
      });
    }
    else {
      this.productList = [];
    }
  }

  onStateChange(e: any): void {
    let filteredUsers: any[] = [];
    for (let i = 0; i < this.stateList.length; i++) {
      if (this.stateList[i].Statename === e.target.value) {
        filteredUsers = [...filteredUsers, this.stateList[i]];
      }
    }
    this.formGroup4.controls['stateCode'].setValue(filteredUsers[0].Statecode);
  }

  calculateTotalTaxableAmount(): void {
    let other1 = (this.othrChrgs1 != undefined || '' ? this.othrChrgs1 : 0);
    let other2 = (this.othrChrgs2 != undefined || '' ? this.othrChrgs2 : 0);
    let other3 = (this.othrChrgs3 != undefined || '' ? this.othrChrgs3 : 0);
    this.totalOtherCharges = Number(other1) + Number(other2) + Number(other3) + Number(this.advPaid);
    this.totalOtherCharges = Number(this.totalOtherCharges.toFixed(2));
    this.taxableAmt = this.grandTotalAmount = Number(this.totalOtherCharges) + Number(this.totalAmount)
    sessionStorage.removeItem('grandTotal');
    sessionStorage.setItem('grandTotal', String(this.grandTotalAmount))
  }

  otherChrgs1(str: any): void {
    this.calculateTotalTaxableAmount();
    this.calculateRoundOff();
  }

  otherChrgs2(str: any): void {
    this.calculateTotalTaxableAmount();
    this.calculateRoundOff();
  }

  otherChrgs3(str: any): void {
    this.calculateTotalTaxableAmount();
    this.calculateRoundOff();
  }

  calculateRoundOff(): void {
    let taxableAmt = this.grandTotalAmount;
    let roundOff = Math.round(taxableAmt);
    let roundOffValue = Number(roundOff - taxableAmt).toFixed(2)
    this.formGroup2.controls['roundOff'].setValue(roundOffValue);
    this.grandTotalAmount = roundOff
  }

  onChangeDiscount(value: any): void {

    let val = Number(value);
    let taxAmt = Number(this.taxableAmt);
    let perc = ((taxAmt) * (val / 100))
    let res = (Number(taxAmt) - Number(perc))

    this.grandTotalAmount = Number(res.toFixed(2))
    sessionStorage.setItem('grandTotal', String(this.grandTotalAmount))

    this.calculateRoundOff()
  }

  onSave(): void {
    this.submitted = true;
    if (this.getParty.valid && this.formGroup1.valid && this.formGroup2.valid && this.formGroup3.valid) {
      let payload = {
        "InvioceData": {
          "CompanyId": this.globalCompanyId,
          "LedgerId": this.ledgerId,
          "OriginalInvDate": this.datePipe.transform(this.myDate, 'yyyy-MM-dd'),
          "VochType": this.voucherTypeId,
          "VoucherName": this.getParty.value.voucherType,
          "DealerType": this.dealerType,
          "PAN": this.PANNumber,
          "GST": this.getParty.value.gst,
          "InvoiceType": "SalesExport", //refer all types of invoice
          "InoviceNo": this.getParty.value.invoiceNo,
          "State": this.defaultState,
          "ExpenseName1": this.ExpenseName1,
          "ExpenseName2": this.ExpenseName2,
          "ExpenseName3": this.ExpenseName3,
          "ExpenseAmount1": Number(this.othrChrgs1),
          "ExpenseAmount2": Number(this.othrChrgs2),
          "ExpenseAmount3": Number(this.othrChrgs3),
          "TaxableValue": this.taxableAmt,
          "Discount": Number(this.formGroup2.value.discount),
          "Sgstvalue": Number(this.formGroup2.value.sgstRate),
          "Csgstvalue": Number(this.formGroup2.value.cgstRate),
          "Igstvalue": Number(this.formGroup2.value.igstRate),
          "IsSez": Number(this.disableRates == true ? 1 : 0),
          "RoundOff": Number(this.formGroup2.value.roundOff),
          "TotalAmount": Number(this.grandTotalAmount),
          "Address": this.formGroup3.value.address,
          "FromPlace": this.formGroup3.value.fromPlace,
          "ToPlace": this.formGroup3.value.toPlace,
        },
        "LorryDetails": {
          "PoNumber": this.formGroup4.value.poNo,
          "EwaybillNo": this.formGroup4.value.eWayBill,
          "Transporter": this.formGroup4.value.transporter,
          "LorryNo": this.formGroup4.value.lorryNo,
          "Owner": this.formGroup4.value.owner,
          "Driver": this.formGroup4.value.driver,
          "Dlno": this.formGroup4.value.dlNo,
          "CheckPost": this.formGroup4.value.checkPost,
          "FrieghtPerBag": Number(this.formGroup4.value.frieght_bag),
          "TotalFrieght": Number(this.formGroup4.value.totalFrieght),
          "AdvanceFrieght": Number(this.formGroup4.value.advancePaid),
          "BalanceFrieght": Number(this.formGroup4.value.balanceFrieght),
          "FrieghtPlus_Less": this.formGroup4.value.frieght_Plus_Less,
          "TDS": Number(this.formGroup4.value.tds),
          "DeliveryName": this.formGroup4.value.partyName,
          "DeliveryAddress1": this.formGroup4.value.address1,
          "DeliveryAddress2": this.formGroup4.value.address2,
          "DeliveryPlace": this.formGroup4.value.place,
          "DeliveryPin": this.formGroup4.value.pinCode,
          "DeliveryState": this.formGroup4.value.state,
          "DeliveryStateCode": this.formGroup4.value.stateCode,
          "Distance": Number(this.formGroup4.value.distance),
          "Dcnote": this.formGroup4.value.note,
        },
        "ItemData": this.dynamicArray
      }

      this.spinner.show()
      this.adminService.addGoodsInvoice(payload).subscribe({
        next: (res: any) => {
          if (res == 'Added Successfully...!') {
            this.toastr.success('Invoice Created Successfully!');
            this.spinner.hide();
            this.location.back();
          }

          if (res == 'Duplicate InoviceNo...!') {
            this.toastr.error('Invoice number is already exist!');
            $('#invoiceNo').focus()
            this.spinner.hide();
          }
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Something went wrong');
        },
      });
    } else {
      this.toastr.error('Please fill mandatory fields');
    }
  }

  back(): void {
    this.location.back();
  }
}
