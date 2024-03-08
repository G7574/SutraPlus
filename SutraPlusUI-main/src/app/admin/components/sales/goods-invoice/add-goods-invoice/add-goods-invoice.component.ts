import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AdminServicesService } from 'src/app/admin/services/admin-services.service';
import { CalculateService } from './../../../../services/calculate.service';
import { CommonService } from 'src/app/share/services/common.service';
import { party } from 'src/app/share/models/party';
import { product } from 'src/app/share/models/product';
import { add, format, parse } from 'date-fns';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { el, th, tr } from 'date-fns/locale';
import { DatePipe, formatDate } from '@angular/common';
import { Items } from '../../models/items';
import { Location } from '@angular/common';
import ro from 'date-fns/locale/ro/index.js';
import { isNil, sum, isEmpty } from 'lodash-es';
import { DecimalpointPipe } from './../../../../../pipe/decimalpoint.pipe';
import { TabContentComponent } from '@coreui/angular';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthenticationServiceService } from 'src/app/authentication/services/authentication-service.service';
import { SuperAdminServiceService } from 'src/app/super-admin/services/super-admin-service.service';
import { debounceTime, distinctUntilChanged, of, switchMap } from 'rxjs';
import * as pdfMake from 'pdfmake/build/pdfmake';
import { ca } from 'date-fns/locale';
import * as pdfFonts from 'pdfmake/build/vfs_fonts';
import jsPDF from 'jspdf';
import { json } from 'stream/consumers';
import { Console } from 'console';
import Swal from 'sweetalert2';

declare var $: any;
declare function GetCurrDateReturnStr(): any;

@Component({
  selector: 'app-add-goods-invoice',
  templateUrl: './add-goods-invoice.component.html',
  styleUrls: ['./add-goods-invoice.component.scss'],
  providers: [DecimalpointPipe],
})
export class AddGoodsInvoiceComponent implements OnInit {
  submitted: boolean = false;
  public crdrvisible = false;
  public visible = false;
  public editVisible = false;
  selectedCar?: any;
  cars = [
    { id: 1, name: 'Volvo' },
    { id: 2, name: 'Saab' },
    { id: 3, name: 'Opel' },
    { id: 4, name: 'Audi' },
  ];
  globalCompanyId!: number;
  showOtherCharges: boolean = true;
  ledgerId!: number;
  searchText!: string;
  partyList: party[] = [];
  otherAccList: party[] = [];
  stateList: { Statecode: string; Statename: string }[] = [];
  todayDate?: string;
  productList: product[] = [];
  kgPerBag!: number;
  NoOfBags!: number;
  totalQty!: number;
  Rate!: number;
  getParty!: FormGroup;
  formGroup1!: FormGroup;
  formGroup2!: FormGroup;
  formGroup3!: FormGroup;
  formGroup4!: FormGroup;

  minDate = '';
  maxDate = '';
  LedgerName = '';

  TotalCreditAmount!: number;
  TotalDebitAmount!: number;


  // ?: string;

  i!: number;
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
  invType: any;
  isShow: boolean = true;
  showPrintButton: boolean = false;

  public dynamicArray: Array<Items> = [];
  invoiceNo!: number;
  ExpenseName1!: string;
  ExpenseName2!: string;
  ExpenseName3!: string;
  newRate!: number;
  secondTime: boolean = false;
  companyPlace: string | null;
  totalNoOfBags: number = 1;
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
  advPaid: number = 0;
  otherChargesInfo: boolean = false;
  checkDealerType: boolean = true;
  isThirdParty: boolean = false;
  isEinVoice: boolean = false;
  partySelected: boolean = false;
  companySelected: boolean = false;
  lorrySubmitted: boolean = false;
  isnotThirdParty: boolean = false;
  isDebitNote: boolean = true;
  CRDRDetails: any;

  // dispatcher details
  dispatcherName: string | null = null;
  dispatcherAddress1: string | null = null;
  dispatcherAddress2: string | null = null;
  dispatcherPlace: string | null = null;
  dispatcherPIN: string | null = null;
  dispatcherStatecode: string | null = null;
  countryCode: string | null = null;
  shipBillNo: string | null = null;
  forCur: string | null = null;
  portName: string | null = null;
  refClaim: string | null = null;
  shipBillDate: Date | null = null;
  expDuty: string | null = null;

  dispactDetailVisible = false;

  // invoice response modal
  invoiceResponseModelVisible = false;
  invoiceNoToGetReponse: string | null = null;

  // invoice response props
  status: string;
  irn: string;
  invoiceNumber: string;
  ackNo: string;
  signedQR: string;

  @ViewChild('eInvoiceModal') invoiceMatDialog!: TemplateRef<any>;
  saveEInvoiceForm: FormGroup = new FormGroup({});
  invoiceDialogRef!: any;
  eInvoiceModalVisible = false;

  // lorry details autocomplete
  lorryDetailsAutoCompleteList: [] = [];
  transporterOptions: string[] = [];
  LorryNoOptions: string[] = [];
  // ownerOptions: string[] = [];
  owner: string = ' ';
  ownerOptions: string[] = [];

  driverOptions: string[] = [];
  CompleteInvoice = '';
  TransporterCopy = '';
  CustomerCopy = '';
  Type : any = 3;

  constructor(
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    public commonService: CommonService,
    private datePipe: DatePipe,
    private location: Location,
    private decimal: DecimalpointPipe,
    private calculate: CalculateService,
    private activatedRoute: ActivatedRoute,
    private formBuilder: FormBuilder,
    private dialog: MatDialog,
    private authenticationService: AuthenticationServiceService,
    private superAdminService: SuperAdminServiceService,
    private changeDetectionRef: ChangeDetectorRef,
    private route: ActivatedRoute,
    private router: Router

  ) {
    this.globalCompanyId = Number(sessionStorage.getItem('companyID'));
    this.companyPlace = sessionStorage.getItem('companyPlace');
    var date = new Date();
    this.todayDate = formatDate(new Date(), 'yyyy-MM-dd', 'en-US');
    this.activatedRoute.queryParams.subscribe(params => {
      this.invType = params['InvoiceType'];
      this.voucherTypeId = params['VochType'];
      this.invoiceNo =  params['InvoiceNo'];
    });


    var TempminDate1 = sessionStorage.getItem("startdate")?.toString().split('T')[0];
    this.minDate = TempminDate1 == undefined ? '' : TempminDate1;

    var TempMaxDate1 = sessionStorage.getItem("enddate")?.toString().split('T')[0];
    this.maxDate = TempMaxDate1 == undefined ? '' : TempMaxDate1;

    var TemptodayDate = new Date(this.todayDate);
    var TempMaxDate2 = new Date(this.maxDate);

    if (TemptodayDate > TempMaxDate2) {
      this.todayDate = this.maxDate;
    }

    this.newRate = 0;

    //create formGroup4 FormGroup
    this.formGroup4 = new FormGroup({
      poNo: new FormControl(''),
      eWayBill: new FormControl(''),
      transporter: new FormControl(''),
      LorryNo: new FormControl(''),
      owner: new FormControl(''),
      driver: new FormControl(''),
      dlNo: new FormControl(''),
      checkPost: new FormControl(''),
      frieght_bag: new FormControl(''),
      totalFrieght: new FormControl(''),
      advancePaid: new FormControl(''),
      balanceFrieght: new FormControl(''),
      frieght_Plus_Less: new FormControl(''),
      tds: new FormControl(''),
      partyName: new FormControl('', [Validators.required]),
      address1: new FormControl('', [Validators.required]),
      address2: new FormControl('', [Validators.required]),
      place: new FormControl('', [Validators.required]),
      pinCode: new FormControl('', [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(6),
      ]),
      state: new FormControl('', [Validators.required]),
      stateCode: new FormControl('', [Validators.required]),
      distance: new FormControl(''),
      note: new FormControl(''),
    });
  }

  /**
   * get all form control instance
   */
  get partyControl(): { [key: string]: AbstractControl } {
    return this.getParty.controls;
  }
  get fGroup1Control(): { [key: string]: AbstractControl } {
    return this.formGroup1.controls;
  }
  get fGroup2Control(): { [key: string]: AbstractControl } {
    return this.formGroup2.controls;
  }
  get fGroup3Control(): { [key: string]: AbstractControl } {
    return this.formGroup3.controls;
  }
  get fGroup4Control(): { [key: string]: AbstractControl } {
    return this.formGroup4.controls;
  }

  ngOnInit(): void {

    //this.Type = this.route.snapshot.queryParamMap.get('Type')?.toString();

    console.log("this.Type -> " + this.Type);

    this.getDispatcherDetails();

    this.spinner.show();

    if (this.invType === "GinningInvoice") {
      this.isShow = false;
    }

    if (this.invType === "DebitNote") {
      this.isShow = false;
      this.isDebitNote = false;
      this.showOtherCharges = false;
    }

    if(this.invType === "CreditNote") {
      this.showOtherCharges = false;
    }

    // autocomplete // list can have large no of values so it will make the sytem slower
    // this.getLorryDetailAutoComplete();

    //list of states
    this.getStateList();

    // validate e invoice enabled..
    this.validateEinvoiceEnabled(this.globalCompanyId);

    //create getParty FormGroup
    this.getParty = new FormGroup({
      dealerType: new FormControl('', [Validators.required]),
      pan: new FormControl('', [Validators.required]),
      gst: new FormControl(''),
      BillDate: new FormControl(''),
      voucherType: new FormControl(''),
      invoiceNo: new FormControl('', [Validators.required]),
      state: new FormControl('', [Validators.required]),
      country: new FormControl('', [Validators.required]),
      place: new FormControl('', [Validators.required]),
      address1: new FormControl('', [Validators.required]),
      address2: new FormControl('', [Validators.required]),
      pin: new FormControl('', [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(6),
      ]),
      cellNo: new FormControl('', [Validators.required]),
      emailId: new FormControl('', [
        Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$'),
      ]),
      ledgerName: new FormControl(''),
      DrpParty: new FormControl(null),
      ledgerId: new FormControl('', [Validators.required]),
      legalName: new FormControl('', [Validators.required]),
    });


    //create formGroup1 FormGroup
    this.formGroup1 = new FormGroup({
      otherChargesAny: new FormControl(null),
      otherChargesAnyValue: new FormControl(''),
      DrpExpenses1:new FormControl(null),
      otherCharges1: new FormControl(''),
      DrpExpenses2: new FormControl(null),
      otherCharges2: new FormControl(''),
      DrpExpenses3: new FormControl(null),
      otherCharges3: new FormControl(''),
    });

    //create formGroup2 FormGroup
    this.formGroup2 = new FormGroup({
      taxableAmt: new FormControl(''),
      discount: new FormControl(''),
      sezSale: new FormControl(''),
      totalgstAmt: new FormControl(''),
      totalcgstAmt: new FormControl(''),
      totalsgstAmt: new FormControl(''),
      totaligstAmt: new FormControl(''),
      totalGst: new FormControl(''),
      totalGstAmt: new FormControl(''),
      roundOff: new FormControl(''),
      grandTotal: new FormControl(''),
    });

    //create formGroup3 FormGroup
    this.formGroup3 = new FormGroup({
      address: new FormControl('', [Validators.required]),
      ToPlace: new FormControl('', [Validators.required]),
      FromPlace: new FormControl('', [Validators.required]),
    });

    // this.formGroup4.get('transporter').valueChanges.pipe(
    //   debounceTime(300),
    //   distinctUntilChanged(),
    // ).subscribe(value => {
    //   // Call your API with the typed value
    //   this.adminService.getLorryDetailAutoComplete({ CompanyId: this.globalCompanyId, SearchText: value, Type: 'Transporter' }).subscribe({
    //     next: (res: any) => {
    //       // console.log("transporter options", res);  // Ad
    //       if (res && res.BillSummaries && res.BillSummaries.length > 0) {
    //         this.transporterOptions = res.BillSummaries.map((item: any) => item.Transporter);
    //         // console.log("transporter options list ", this.transporterOptions);
    //         this.changeDetectionRef.detectChanges();
    //       }
    //     },
    //     error: (error: any) => {
    //       // Handle the error
    //     },
    //   });
    // });

    // this.formGroup4.get('LorryNo').valueChanges.pipe(
    //   debounceTime(300),
    //   distinctUntilChanged(),
    // ).subscribe(value => {
    //   this.adminService.getLorryDetailAutoComplete({ CompanyId: this.globalCompanyId, SearchText: value, Type: 'LorryNo' }).subscribe({
    //     next: (res: any) => {
    //       // console.log("LorryNo options", res);  // Ad
    //       if (res && res.BillSummaries && res.BillSummaries.length > 0) {
    //         this.LorryNoOptions = res.BillSummaries.map((item: any) => item.LorryNo);
    //         this.changeDetectionRef.detectChanges();
    //       }
    //     },
    //     error: (error: any) => {
    //       // Handle the error
    //     },
    //   });
    // });

    // this.formGroup4.get('owner').valueChanges.pipe(
    //   debounceTime(300),
    //   distinctUntilChanged(),
    // ).subscribe(value => {
    //   this.adminService.getLorryDetailAutoComplete({ CompanyId: this.globalCompanyId, SearchText: value, Type: 'Owner' }).subscribe({
    //     next: (res: any) => {
    //       // console.log("Owner options", res);  // Ad
    //       if (res && res.BillSummaries && res.BillSummaries.length > 0) {
    //         this.ownerOptions = res.BillSummaries.map((item: any) => item.Owner);
    //         // console.log("owner options", this.ownerOptions);
    //         this.changeDetectionRef.detectChanges();
    //       }
    //     },
    //     error: (error: any) => {
    //       // Handle the error
    //     },
    //   });
    // });

    // this.formGroup4.get('driver').valueChanges.pipe(
    //   debounceTime(300),
    //   distinctUntilChanged(),
    // ).subscribe(value => {
    //   // Call your API with the typed value
    //   this.adminService.getLorryDetailAutoComplete({ CompanyId: this.globalCompanyId, SearchText: value, Type: 'Driver' }).subscribe({
    //     next: (res: any) => {
    //       // console.log("Driver options", res);  // Ad
    //       if (res && res.BillSummaries && res.BillSummaries.length > 0) {
    //         this.driverOptions = res.BillSummaries.map((item: any) => item.Driver);
    //         console.log(this.driverOptions);
    //         this.changeDetectionRef.detectChanges();
    //       }
    //     },
    //     error: (error: any) => {
    //       // Handle the error
    //     },
    //   });
    // });

    // try {
    //   this.assignOptionToFormControl("transporter",this.transporterOptions.toString());
    //   this.assignOptionToFormControl("LorryNo",this.LorryNoOptions.toString());
    //   this.assignOptionToFormControl("owner",this.ownerOptions.toString());
    //   this.assignOptionToFormControl("driver",this.driverOptions.toString());
    // } catch(e) {
    //   console.log("Something went wrong -> " + e);
    // }

    // define a newDynamic Object with intial Values used to set ItemDetails and push to dynamicArry variable
    this.newDynamic = {
      Sno: (this.dynamicArray.length + 1),
      CommodityName: '',
      CommodityId: null,
      WeightPerBag: '',
      NoOfBags: '',
      TotalWeight: '',
      Rate: '',
      MOU: '',
      Taxable: '',
      Amount: '',
      Remark: '',
      SgstAmount: 0,
      CgstAmount: 0,
      IgstAmount: 0,
      NoOfDocra: 0,
      SgstRate: 0,
      CgstRate: 0,
      IgstRate: 0,
    };
    this.dynamicArray.push(this.newDynamic);

    this.saveEInvoiceForm = this.formBuilder.group({
      pin: ['', Validators.required],
      legalName: ['', Validators.required],
      portalUserName: ['', Validators.required],
      portalPw: ['', Validators.required],
      portalEmail: ['', Validators.required],
      einvoiceKey: ['', Validators.required],
      einvoiceSkey: ['', Validators.required],
      einvoiceUserName: ['', Validators.required],
      einvoicePassword: ['', Validators.required],
      pan: ['', Validators.required],
      addressLine2: ['', Validators.required],
      cellphone: ['', Validators.required],
      eInvoiceReq: [1, Validators.required]
    });

    // get company
    this.getCompany();
    this.getParty.controls['BillDate'].setValue(this.todayDate);

    this.spinner.hide();

    //this.getParty.controls['ledgerName'].setValue('Kapil');


    if(String(this.invoiceNo) == '' ||  this.invoiceNo == undefined || String(this.invoiceNo) == 'undefined'){
    var adf = '';
    }
    else
    {
      this.getSingleInvoiceDetails();
    }

    this.GetEinvoiceKey();

  }

  EinvoiceKey : boolean = false;

  GetEinvoiceKey() {
    let payload = {
      SalesDetails: {
        CompanyId: this.globalCompanyId,
      },
    };

    this.adminService.getEinvoiceKey(payload).subscribe({
      next: (res: any) => {
        this.EinvoiceKey = res.EinvoiceKey;
      },
      error: (error: any) => {
        this.toastr.error('Something went wrong');
      },
    });
  }


  searching(text: string, type:string){

      // Call your API with the typed value
      this.adminService.getLorryDetailAutoComplete({ CompanyId: this.globalCompanyId, SearchText: text, Type: type }).subscribe({
        next: (res: any) => {
          // console.log("transporter options", res);  // Ad
          if (res && res.BillSummaries && res.BillSummaries.length > 0) {

            switch (type) {
              case "Transporter":
                this.transporterOptions = this.getUniqueValues(res.BillSummaries.map((item: any) => item.Transporter));
                break;

              case "LorryNo":
                this.LorryNoOptions = this.getUniqueValues(res.BillSummaries.map((item: any) => item.LorryNo));
                break;

              case "Owner":
                this.ownerOptions = this.getUniqueValues(res.BillSummaries.map((item: any) => item.Owner));
                break;

              case "Driver":
                this.driverOptions = this.getUniqueValues(res.BillSummaries.map((item: any) => item.Driver));
                break;
            }

            // console.log("transporter options list ", this.transporterOptions);
            this.changeDetectionRef.detectChanges();
          }
        },
        error: (error: any) => {
          // Handle the error
        },
      });
  }

  getUniqueValues(array: any[]): any[] {
    return Array.from(new Set(array));
  }


  // open and close of Lorry popup
  toggleLiveDemo() {

    if(this.route.snapshot.queryParamMap.get('InvoiceType')?.toString() == 'GoodsInvoice' ||
      this.route.snapshot.queryParamMap.get('InvoiceType')?.toString() == 'ExportInvoice' ||
      this.route.snapshot.queryParamMap.get('InvoiceType')?.toString() == 'ProfarmaInvoice' ||
      this.route.snapshot.queryParamMap.get('InvoiceType')?.toString() == 'DeemedExport' ||
      this.route.snapshot.queryParamMap.get('InvoiceType')?.toString() == 'PurchaseReturn') {

      for(let index = 0; index < this.dynamicArray.length; index++) {
        if(!this.dynamicArray[index].CommodityId){
          this.toastr.error('Please select Item.');
          return;
        }

        if(!this.dynamicArray[index].WeightPerBag){
          if(this.dynamicArray[index].WeightPerBag != 0){
          this.toastr.error('Please add KG/Bag.');
          return;
          }
        }
        if(!this.dynamicArray[index].NoOfBags){
          this.toastr.error('Please add No. of Bags.');
          return;
        }
        if(!this.dynamicArray[index].TotalWeight){
          this.toastr.error('Please add TotalWeight.');
          return;
        }
        if(!this.dynamicArray[index].Rate){
          this.toastr.error('Please add Rate.');
          return;
        }
        if(!this.dynamicArray[index].Amount){
          this.toastr.error('Please add Amount.');
          return;
        }

        // if(!this.dynamicArray[index].Remark){
        //   this.toastr.error('Please add Remark.');
        //   return;
        // }

      }
    } else if(this.route.snapshot.queryParamMap.get('InvoiceType')?.toString() == 'GinningInvoice') {
      for(let index = 0; index < this.dynamicArray.length; index++) {
        if(!this.dynamicArray[index].CommodityId){
          this.toastr.error('Please select Item.');
          return;
        }

        if(!this.dynamicArray[index].TotalWeight){
          this.toastr.error('Please add TotalWeight.');
          return;
        }
        if(!this.dynamicArray[index].Rate){
          this.toastr.error('Please add Rate.');
          return;
        }
        if(!this.dynamicArray[index].Amount){
          this.toastr.error('Please add Amount.');
          return;
        }

      }
    } else if(this.route.snapshot.queryParamMap.get('InvoiceType')?.toString() == 'DebitNote') {
      for(let index = 0; index < this.dynamicArray.length; index++) {
        if(!this.dynamicArray[index].CommodityId){
          this.toastr.error('Please select Item.');
          return;
        }

        // if(!this.dynamicArray[index].Amount){
        //   this.toastr.error('Please add Amount.');
        //   return;
        // }

      }
    }


    // if(true){
    //   this.toastr.error('Please fill mondatory fields');
    //   return
    // }
    // console.log("toggle live demo .. lorry details");
    this.visible = !this.visible;

  }

  toggleEInvoiceModal() {
    this.eInvoiceModalVisible = !this.eInvoiceModalVisible;
  }

  // open and close of Edit popup
  editDemo() {
    this.editVisible = !this.editVisible;
  }

  toggleCrDrDetails() {
    if(this.crdrvisible == false)
    {
      this.crdrvisible = true;
    }
    else
    {
      this.crdrvisible = false;
    }
  }

  //save Lorry details after validations
  saveLorryDetails() {
    if(this.balanceFr < 0){
      this.toastr.error('Balance Freight should not be greater than Advance Paid.');
      return;
    }
    // validate the lorry form based on condition and return boolean
    if (this.validateLorryDetails()) {

      this.selectFrieght();

      this.otherChargesInfo = true;
      this.visible = !this.visible;


      this.LineItemTotal(0, 0, 'SaveLorryDetails');
      //lastly added
      this.GetTotal();


    } else {
      this.toastr.error('Please fill mondatory fields');
    }
  }

  handleLiveDemoChange(event: any) {
    this.visible = event;
  }
  handleEditDemoChange(event: any) {
    this.editVisible = event;
  }

  handleEInvoiceModal(event: any) {
    this.eInvoiceModalVisible = event;
  }

  getSingleInvoiceDetails() {



    let partyDetails = {
      "SalesInvoice": {
        "CompanyId": this.globalCompanyId,
        "LedgerId": 0,
        "InvoiceNO": this.invoiceNo,
        "VouchType": this.voucherTypeId,
        "DisplayInvNo": '0',
        "InvoiceType": "SalesGood"
      }
    }

    this.spinner.show()
    this.adminService.getItemList(partyDetails).subscribe({
      next: (res: any) => {
        // console.log(res);

        if (!res.HasErrors && res?.Data !== null) {
          console.log(JSON.parse(res.InvoiceData));
          console.log(JSON.parse(res.ItemData));

          var InvoiceData =JSON.parse(res.InvoiceData);
          var Invoice_ItemData = JSON.parse(res.ItemData);

          debugger;

          let OnePartyDetails:party[] = [{
            "ledgerName": InvoiceData['LedgerName'],
            "ledgerId": InvoiceData['LedgerID'],
            "place": "Kapil",
            "dealerType": "Kapil",
            "pan": "Kapil",
            "gstn": "Kapil",
            "state": "Kapil",
            "invoiceNo": "Kapil",
            "voucherType": "Kapil"
        }]

        this.partyList = OnePartyDetails;
        this.getParty.controls['DrpParty'].setValue(InvoiceData['LedgerID']);

        //invoiceNo



        this.dynamicArray =[];

        var i = 0;
        for(i=0;i<Invoice_ItemData.length;i++)
        {
          this.newDynamic = {
            Sno: (this.dynamicArray.length + 1),
            CommodityName: Invoice_ItemData[i]['CommodityName'],
            CommodityId: Invoice_ItemData[i]['CommodityId'],
            WeightPerBag: Invoice_ItemData[i]['WeightPerBag'],
            NoOfBags: Invoice_ItemData[i]['NoOfBags'],
            TotalWeight: Invoice_ItemData[i]['TotalWeight'],
            Rate: Invoice_ItemData[i]['Rate'],
            Taxable: Invoice_ItemData[i]['Taxable'],
            Amount:  Invoice_ItemData[i]['Amount'] ,
            Remark: Invoice_ItemData[i]['Mark'],
            SgstAmount: Invoice_ItemData[i]['SGST'],
            CgstAmount: Invoice_ItemData[i]['CGST'],
            IgstAmount: Invoice_ItemData[i]['IGST'],

            SgstRate: Invoice_ItemData[i]['SGSTRate'],
            CgstRate: Invoice_ItemData[i]['CGSTRate'],
            IgstRate: Invoice_ItemData[i]['IGSTRate'],
            NoOfDocra: 0
          };
          this.dynamicArray.push(this.newDynamic);

          let OneProductDetails = {
            commodityName: Invoice_ItemData[i]['CommodityName'],
            _Id: parseFloat(Invoice_ItemData[i]['CommodityId']),
          }
          this.productList.push(OneProductDetails);

          this.ledgerId = InvoiceData['LedgerID'];
          this.LedgerName = InvoiceData['LedgerName'];
          this.voucherTypeId = InvoiceData['VoucherTypeID'];
          this.dealerType = InvoiceData['DealerType'];
          this.PANNumber = InvoiceData['PAN'];
          this.voucherTypeText = InvoiceData['VoucherName'];

              //create formGroup4 FormGroup
              this.formGroup4.patchValue({
                poNo: InvoiceData['Ponumber'],
                eWayBill: InvoiceData['EwayBillNo'],
                transporter: InvoiceData['Transporter'],
                LorryNo: InvoiceData['LorryNo'],
                owner: InvoiceData['LorryOwnerName'],
                driver: InvoiceData['DriverName'],
                dlNo: InvoiceData['Dlno'],
                checkPost: InvoiceData['CheckPost'],
                frieght_bag: InvoiceData['FrieghtPerBag'],
                totalFrieght: InvoiceData['TotalFrieght'],
                advancePaid: InvoiceData['Advance'],
                balanceFrieght: InvoiceData['Balance'],
                frieght_Plus_Less: (InvoiceData['frieghtPlus'] == null ? '' : InvoiceData['frieghtPlus']),
                tds: InvoiceData['TDS'],
                partyName: InvoiceData['DeliveryName'],
                address1: InvoiceData['DeliveryAddress1'],
                address2: InvoiceData['DeliveryAddress2'],
                place: InvoiceData['DeliveryPlace'],
                pinCode: InvoiceData['DelPinCode'],
                state: InvoiceData['DeliveryState'],
                stateCode: InvoiceData['DeliveryStateCode'],
                distance: InvoiceData['DeliveryDistance'],
                note: InvoiceData['DeliveryNote']
              });



              this.formGroup3.patchValue({
                FromPlace:InvoiceData['FromPlace'],
                ToPlace:InvoiceData['ToPlace'],
                address : InvoiceData['LedgerName']
              })





              this.otherAccList = [];

              this.ExpenseName1 = InvoiceData['ExpenseName1'];
              this.ExpenseName2 = InvoiceData['ExpenseName2'];
              this.ExpenseName3 = InvoiceData['ExpenseName3'];

              let OtherChargesDrp1 = {
                ledgerName: this.ExpenseName1,
                ledgerId: 0
              }

              let OtherChargesDrp2 = {
                ledgerName: this.ExpenseName2,
                ledgerId: 0
              }

              let OtherChargesDrp3 = {
                ledgerName: this.ExpenseName3,
                ledgerId: 0
              }

              if(this.ExpenseName1 != '')
              {
                this.otherAccList.push(OtherChargesDrp1);
              }
              if(this.ExpenseName2 != '')
              {
                this.otherAccList.push(OtherChargesDrp2);
              }
              if(this.ExpenseName3 != '')
              {
                this.otherAccList.push(OtherChargesDrp3);
              }


              this.formGroup1.controls['DrpExpenses1'].setValue(this.ExpenseName1);
              this.formGroup1.controls['DrpExpenses2'].setValue(this.ExpenseName2);
              this.formGroup1.controls['DrpExpenses3'].setValue(this.ExpenseName3);


              this.formGroup1.patchValue({
                //otherChargesAny: InvoiceData['LedgerName'],
                //otherChargesAnyValue: InvoiceData['LedgerName'],
                otherCharges1: InvoiceData['ExpenseAmount1'],
                otherCharges2: InvoiceData['ExpenseAmount2'],
                otherCharges3: InvoiceData['ExpenseAmount3'],
              });


              this.formGroup2.patchValue({
                taxableAmt: InvoiceData['TaxableValue'],
                discount: '',
                sezSale: InvoiceData['IsSEZ'],
                totalgstAmt: '0',
                totalcgstAmt: InvoiceData['CGSTValue'],
                totalsgstAmt: InvoiceData['SGSTValue'],
                totaligstAmt: InvoiceData['IGSTValue'],
                totalGst: '',
                totalGstAmt: '',
                roundOff: InvoiceData['RoundOff'],
                grandTotal: ''
              });


              this.defaultState = InvoiceData['State'];

              this.getParty.patchValue({
                dealerType: this.dealerType,
                pan: InvoiceData['PAN'],
                gst: InvoiceData['GST'],
                ledgerName: InvoiceData['LedgerName'],
                ledgerId: InvoiceData['LedgerID'],
                legalName: InvoiceData['LegalName'],
                BillDate: InvoiceData['TrancDate'],
                voucherType: InvoiceData['VoucherName'],
                invoiceNo: InvoiceData['VochNo'],
                state: this.defaultState,
                address1: InvoiceData['Address1'],
                address2: InvoiceData['Address2'],
                pin: InvoiceData['PIN'],
                cellNo: InvoiceData['CellNo'],
                emailId: ''
              });

              if (this.voucherTypeText == 'Export Sale') {
                this.igstShow = true;
                this.cgstShow = this.sgstShow = false;
              } else if (this.voucherTypeText == 'Local Sale') {
                this.igstShow = false;
                this.cgstShow = this.sgstShow = true;
              } else if (this.voucherTypeText == 'Interstate Sale') {
                this.igstShow = true;
                this.cgstShow = this.sgstShow = false;
              } else if (this.voucherTypeText == 'URD Sale') {
                this.igstShow = true;
                this.cgstShow = this.sgstShow = false;
              } else {
              }


              this.GetTotal();
              console.log("this.defaultState -> " + this.defaultState);
              //this.onStateCode();

        }



        }
        this.spinner.hide();
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong');
      },
    });
  }







  getPartyList(text: string) {
    if (text.length >= 1) {
      let partyDetails = {
        LedgerData: {
          CompanyId: this.globalCompanyId,
          LedgerType: 'Sales Ledger',
          Country: '',
        },
        SearchText: text,
        Page: {
          Page: '1',
          PageSize: '10',
        },
      };
      this.adminService.getLedgerList(partyDetails).subscribe({
        next: (res: any) => {
          for (let data of res.records) {
            data.ledgerName = `${data.ledgerName} - ${data.place}`;


          }
          // this.partyList = res.records;
          // console.log(this.partyList);

          let OnePartyDetails:party[] = [{
            "ledgerName": "Kapil",
            "ledgerId": 14,
            "place": "Kapil",
            "dealerType": "Kapil",
            "pan": "Kapil",
            "gstn": "Kapil",
            "state": "Kapil",
            "invoiceNo": "Kapil",
            "voucherType": "Kapil"
        }]

        debugger;
        this.partyList = res.records;
        console.log(OnePartyDetails);
        console.log(res.records);


          this.partyList = res.records;
          // console.log(this.partyList);
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Something went wrong');
        },
      });
    } else {
      this.partyList = [];
    }
  }

  emailId : any;

  onClickTest($event: any, item: any) {
    this.ledgerId = item.ledgerId;
    console.log("item.LedgerId -> " +item.LedgerId);
    let invoiceData;
    let payload = {
      SalesDetails: {
        CompanyId: this.globalCompanyId,
        LedgerId: item.ledgerId,
        DealerType: item.dealerType,
        InvoiceType: this.invType
      },
    };
    this.adminService.getVoucherTypeAndInvoiceNo(payload).subscribe({
      next: (res: any) => {
        this.spinner.show();
        debugger;
        if (!res.HasErrors && res?.Data !== null) {
          invoiceData = res;
          this.setData(item);
          this.setData2(invoiceData);
          this.partySelected = true;

          // this.formGroup4.get("transporter").setValue(invoiceData.Transporter);
          // this.formGroup4.get("LorryNo").setValue(invoiceData.LorryNo);
          // this.formGroup4.get("owner").setValue(invoiceData.Owner);
          // this.formGroup4.get("driver").setValue(invoiceData.Driver);

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
        LedgerData: {
          CompanyId: this.globalCompanyId,
          LedgerType: 'Sales Other Ledger',
        },
        SearchText: text,
        Page: {
          PageNumber: '1',
          PageSize: '10',
        },
      };

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
    } else {
      this.otherAccList = [];
    }
  }

  onOtherAcc1(item: any) {
    this.ExpenseName1 = item.ledgerName;
  }

  onOtherAcc2(item: any) {
    this.ExpenseName2 = item.ledgerName;
  }
  onOtherAcc3(item: any) {
    this.ExpenseName3 = item.ledgerName;
  }

  setData(list: any): void {
    debugger;
    if (list) {
      let address, address1, address2;
      if (
        list.address1 != '' &&
        list.address1 != null &&
        list.address1 != undefined
      ) {
        address1 = list.address1;
      } else {
        address1 = '-';
      }

      if (
        list.address2 != '' &&
        list.address2 != null &&
        list.address2 != undefined
      ) {
        address2 = list.address2;
      } else {
        address2 = '-';
      }

      address = `${address1} ${address2}`;

      if (address == '- -') {
        address = '-';
      } else {
        address = `${address1} ${address2}`;
      }

      if (list.dealerType == 'Un-Registered Dealer') {
        this.checkDealerType = false;
      } else {
        this.checkDealerType = true;
      }
      this.LedgerName = list.ledgerName;
      this.formGroup4.controls['partyName'].setValue(list.ledgerName);
      this.getParty.controls['dealerType'].setValue(list.dealerType);
      this.getParty.controls['pan'].setValue(list.pan);
      this.getParty.controls['place'].setValue(list.place);
      this.getParty.controls['gst'].setValue(list.gstn);
      this.getParty.controls['state'].setValue(list.state);
      this.getParty.controls['country'].setValue(list.country);
      this.getParty.controls['address1'].setValue(list.address1);
      this.getParty.controls['address2'].setValue(list.address2);
      this.getParty.controls['pin'].setValue(list.pin);
      this.getParty.controls['emailId'].setValue(list.emailId);
      this.emailId = list.emailId;
      console.log("this.emailId -> " + this.emailId);
      this.getParty.controls['cellNo'].setValue(list.cellNo);
      this.getParty.controls['ledgerId'].setValue(list.ledgerId);
      this.getParty.controls['legalName'].setValue(list.legalName);
      this.getParty.controls['ledgerName'].setValue(list.ledgerName);

      this.cellNo = list.cellNo;

      this.formGroup3.controls['address'].setValue(address);
      this.formGroup3.controls['FromPlace'].setValue(this.companyPlace);

      this.formGroup3.controls['ToPlace'].setValue(list.place);
      this.formGroup4.controls['address1'].setValue(list.address1);
      this.formGroup4.controls['address2'].setValue(list.address2);
      this.formGroup4.controls['place'].setValue(list.place);
      this.formGroup4.controls['pinCode'].setValue(list.pin);
      this.dealerType = list.dealerType;
      this.PANNumber = list.pan;
      this.GSTNumber = list.gstn;
      this.defaultState = list.state;
      console.log(" defaultState -> " + this.defaultState);
    }
  }

  cellNo : any;
  setData2(list: any): void {
    if (list) {
      this.getParty.controls['voucherType'].setValue(list.VoucherType);
      this.getParty.controls['invoiceNo'].setValue(list.InvoiceNo);
      this.formGroup3.controls['FromPlace'].setValue(this.companyPlace);
      this.voucherTypeText = list.VoucherType;
      this.voucherTypeId = list.VoucherId;
      this.invoiceNo = list.InvoiceNo;

      if (list.VoucherType == 'Export Sale') {
        this.igstShow = true;
        this.cgstShow = this.sgstShow = false;
      } else if (list.VoucherType == 'Local Sale') {
        this.igstShow = false;
        this.cgstShow = this.sgstShow = true;
      } else if (list.VoucherType == 'Interstate Sale') {
        this.igstShow = true;
        this.cgstShow = this.sgstShow = false;
      } else if (list.VoucherType == 'URD Sale') {
        this.igstShow = true;
        this.cgstShow = this.sgstShow = false;
      } else {
      }
    }
  }
  /**
   * Get the list of states
   */
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
        SearchText: text,
        Page: {
          PageNumber: 1,
          PageSize: 10,
        },
      };
      this.adminService.getProductList(UserDetails).subscribe({
        next: (res: any) => {
          if (this.secondTime == false) {
            debugger;
            this.productList = res.records;
          } else {
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
    } else {
      this.productList = [];
    }
  }

  getNewProductList(text: string): void {
    if (text.length >= 1) {
      let UserDetails = {
        SelesItem: {
          Name: 'tur',
          GSTType: '34.3000',
        },
      };
      this.adminService.getNewProductList(UserDetails).subscribe({
        next: (res: any) => {
          this.productList = res.records;
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Something went wrong');
        },
      });
    } else {
      this.productList = [];
    }
  }

  getToday(): string {
    return new Date().toISOString().split('T')[0];
  }

  isAdvancePaidShow : boolean = false;
  isAdvancePaidShowFirstOne : boolean = false;
  advancePaintAmount = "0";
  payLess = 0
  advancePaidText: string = 'Advance Paid';

  GetTotal(): void {
    debugger;
    this.totalNoOfBags = 0;
    this.totalQuantity = 0;
    this.totalAmount = 0;
    this.taxableAmt = 0;
    this.grandTotalAmount = 0;

    var TotalGST = 0;
    var TotalIGSTAmt = 0;
    var TotalSGSTAmt = 0;
    var TotalCGSTAmt = 0;

    this.formGroup2.controls['totalGstAmt'].setValue(0);

    for (var i = 0; i < this.dynamicArray.length; i++) {
      this.totalNoOfBags += Number(this.dynamicArray[i].NoOfBags);
      this.totalQuantity += Number(this.dynamicArray[i].TotalWeight);
      this.totalAmount += Number(this.dynamicArray[i].Amount);
      this.taxableAmt += Number(this.dynamicArray[i].Taxable);
      TotalGST += Number(this.dynamicArray[i].IgstAmount) + Number(this.dynamicArray[i].CgstAmount) + Number(this.dynamicArray[i].SgstAmount);

      TotalIGSTAmt += Number(this.dynamicArray[i].IgstAmount);
      TotalSGSTAmt += Number(this.dynamicArray[i].SgstAmount);
      TotalCGSTAmt += Number(this.dynamicArray[i].CgstAmount);
    }

    this.taxableAmt = parseFloat(this.taxableAmt.toFixed(2));

    if(this.advPaid > 0) {

        if(this.EinvoiceKey && this.formGroup4.controls['frieght_Plus_Less'].value == 'Party Lorry Frieght') {

            this.payLess = this.advPaid;
            this.advancePaintAmount = this.payLess.toString();
            this.isAdvancePaidShow = true;
            this.isAdvancePaidShowFirstOne = false;

        } else if(this.EinvoiceKey && this.formGroup4.controls['frieght_Plus_Less'].value == 'Own Lorry Frieght') {

            this.payLess = this.balanceFr;
            this.advancePaintAmount = this.payLess.toString();
            this.isAdvancePaidShow = true;
            this.isAdvancePaidShowFirstOne = false;

        } else {

          if(this.formGroup4.controls['frieght_Plus_Less'].value == 'Party Lorry Frieght') {

            this.payLess = this.advPaid;
            this.advancePaintAmount = this.payLess.toString();
            this.isAdvancePaidShowFirstOne = true;

          } else if(this.formGroup4.controls['frieght_Plus_Less'].value == 'Own Lorry Frieght') {

            this.payLess = this.balanceFr;
            this.advancePaintAmount = this.payLess.toString();
            this.isAdvancePaidShowFirstOne = true;

          } else {
            this.isAdvancePaidShow = false;
            this.isAdvancePaidShowFirstOne = false;
          }

        }
    } else {
      this.isAdvancePaidShow = false;
      this.isAdvancePaidShowFirstOne = false;
    }

    this.formGroup2.controls['totalGstAmt'].setValue(this.SeperateComma(TotalGST.toFixed(2)));

    debugger;
    console.log("CHECKING -> " + this.isAdvancePaidShow + " and " + this.isAdvancePaidShowFirstOne)
    if(this.isAdvancePaidShow || this.isAdvancePaidShowFirstOne) {

      if(this.formGroup4.controls['frieght_Plus_Less'].value == 'Party Lorry Frieght') {
        this.advancePaidText = "Advance Lorry Frieght"
        this.grandTotalAmount = (this.totalAmount + TotalGST + (this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value)) +
        (this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value)) +
        (this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value)) +
        Number(this.payLess) +
        (this.formGroup1.controls['otherChargesAnyValue'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherChargesAnyValue'].value)));
      } else if(this.formGroup4.controls['frieght_Plus_Less'].value == 'Own Lorry Frieght') {
        this.advancePaidText = "Less Lorry Frieght"
        this.grandTotalAmount = (this.totalAmount + TotalGST + (this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value)) +
        (this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value)) +
        (this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value)) +
        (this.formGroup1.controls['otherChargesAnyValue'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherChargesAnyValue'].value)) -
        Number(this.balanceFr));
        this.advancePaintAmount = "-" + this.payLess.toString();
      } else {
        this.grandTotalAmount = (this.totalAmount + TotalGST + (this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value)) +
        (this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value)) +
        (this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value)) +
        (this.formGroup1.controls['otherChargesAnyValue'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherChargesAnyValue'].value)));
      }

    } else {
        this.grandTotalAmount = (this.totalAmount + TotalGST + (this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value)) +
        (this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value)) +
        (this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value)) +
        (this.formGroup1.controls['otherChargesAnyValue'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherChargesAnyValue'].value)));
    }

    this.formGroup2.controls['totalcgstAmt'].setValue(this.SeperateComma(TotalCGSTAmt.toFixed(2)));
    this.formGroup2.controls['totalsgstAmt'].setValue(this.SeperateComma(TotalCGSTAmt.toFixed(2)));
    this.formGroup2.controls['totaligstAmt'].setValue(this.SeperateComma(TotalIGSTAmt.toFixed(2)));

    // console.log(this.grandTotalAmount);
    console.log("grandTotalAmount BEFORE -> " + this.grandTotalAmount)
    let roundOff = Math.round(this.grandTotalAmount);
    let roundOffValue = Number(roundOff - this.grandTotalAmount).toFixed(2);
    this.formGroup2.controls['roundOff'].setValue(roundOffValue);
    this.grandTotalAmount = this.SeperateComma(roundOff);
    console.log("grandTotalAmount AFTER -> " + this.grandTotalAmount)

  }

  calculateTotalQuantity(): void {
    let sumVal: number = 0;

    for (var i = 0; i < this.dynamicArray.length; i++) {
      let tq = Number(this.dynamicArray[i].TotalWeight);
      sumVal = Number(sumVal + tq);
    }
    // to do
    //this.calculate.calculateTotal(this.dynamicArray,'NoOfBags')
    this.totalQuantity = Number(sumVal);
  }

  calculateTotalAmount(): void {
    // debugger;
    let sumVal: number = 0;
    let totalGst: number = 0;
    let totalSgst: number = 0;
    let totalCgst: number = 0;
    let totalIgst: number = 0;
    for (var i = 0; i < this.dynamicArray.length; i++) {
      sumVal = Number(
        (sumVal + parseFloat(this.dynamicArray[i].Amount)).toFixed(2)
      );

      totalSgst = Number(
        (totalSgst + Number(this.dynamicArray[i].SgstAmount)).toFixed(2)
      );

      totalCgst = Number(
        (totalCgst + Number(this.dynamicArray[i].CgstAmount)).toFixed(2)
      );

      totalIgst = Number(
        (totalIgst + Number(this.dynamicArray[i].IgstAmount)).toFixed(2)
      );

      this.taxableAmt += Number(
        (Number(this.dynamicArray[i].Taxable)).toFixed(2));
    }
    if (this.igstShow) {
      totalSgst = 0;
      totalCgst = 0;
      totalGst = totalIgst;
    } else {
      totalIgst = 0;
      totalGst = Number(totalSgst + totalCgst);
    }

    // to do
    //this.calculate.calculateTotal(this.dynamicArray,'NoOfBags')

    // this.totalAmount = this.taxableAmt = this.grandTotalAmount = Number(sumVal)
    this.totalAmount = Number(sumVal);

    // let gstRateAmt = Number(sumVal) * (this.newRate / 100);
    // let gstRateAmt=Number(totalGst);
    this.formGroup2.controls['igstRate'].setValue(Number(totalIgst));
    this.formGroup2.controls['sgstRate'].setValue(Number(totalSgst));
    this.formGroup2.controls['cgstRate'].setValue(Number(totalCgst));
    this.formGroup2.controls['totalGstAmt'].setValue(Number(totalGst));


    sessionStorage.setItem('grandTotal', String(this.grandTotalAmount));
    // this.calculateAmount()
  }

  changeKgBag(val: any, i: any): void {
    this.dynamicArray[i].WeightPerBag = val;
    if (this.dynamicArray[i].NoOfBags > 0) {
      this.dynamicArray[i].TotalWeight = Number(
        this.dynamicArray[i].WeightPerBag * this.dynamicArray[i].NoOfBags
      );
    } else {
      this.dynamicArray[i].TotalWeight = Number(
        this.dynamicArray[i].WeightPerBag * 0
      );
    }
    this.calculateTotalQuantity();
  }

  changeNoOfBag(val: any, i: any): void {

    this.dynamicArray[i].NoOfBags = val;
    if (this.dynamicArray[i].WeightPerBag > 0) {
      this.dynamicArray[i].TotalWeight = Number(
        this.dynamicArray[i].WeightPerBag * this.dynamicArray[i].NoOfBags
      );
    } else {
      this.dynamicArray[i].TotalWeight = Number(
        this.dynamicArray[i].NoOfBags * 0
      );
    }

    this.calculateTotalQuantity();
  }

  changeQty(val: any, i: any): void {

    this.dynamicArray[i].TotalWeight = val;
    let amt =
      (this.dynamicArray[i].TotalWeight * this.dynamicArray[i].Rate) / 100;
    amt = this.decimal.transform(amt);
    this.dynamicArray[i].Amount = Number(amt);
    this.dynamicArray[i].CgstAmount = !this.igstShow
      ? Number((amt * this.dynamicArray[i].CgstRate) / 100)
      : 0;
    this.dynamicArray[i].SgstAmount = !this.igstShow
      ? Number((amt * this.dynamicArray[i].CgstRate) / 100)
      : 0;
    this.dynamicArray[i].IgstAmount = this.igstShow
      ? Number((amt * this.dynamicArray[i].IgstRate) / 100)
      : 0;
    this.calculateTotalQuantity();
    this.calculateTotalAmount();
  }

  LineItemTotal(val: any, i: any, Caller: string) {

    if (this.dynamicArray[i].WeightPerBag.toString() == '') {
      this.dynamicArray[i].WeightPerBag = 0;
    }

    if (Caller == 'KgBag') {
      this.dynamicArray[i].WeightPerBag = val;
    }

    if (Caller == 'NoofBags') {
      this.dynamicArray[i].NoOfBags = val;
    }

    if (Caller == 'Rate') {
      this.dynamicArray[i].Rate = val;
    }

    if (Caller == 'TotalQty') {
      this.dynamicArray[i].TotalWeight = val;
    }
    else {
      if (this.dynamicArray[i].WeightPerBag > 0 && this.dynamicArray[i].NoOfBags > 0) {
        this.dynamicArray[i].TotalWeight = this.dynamicArray[i].WeightPerBag * this.dynamicArray[i].NoOfBags;
      }
    }

    // Code starts for Total Qty. //

    // Code Ends for Total Qty. //

    var amt = 0;



    if (this.dynamicArray[i].MOU == 'QUINTAL') {
      amt = ((this.dynamicArray[i].TotalWeight * this.dynamicArray[i].Rate) / 100);
      this.dynamicArray[i].Amount = parseFloat(amt.toFixed(2));
    }
    else {
      amt = (this.dynamicArray[i].TotalWeight * this.dynamicArray[i].Rate);
      this.dynamicArray[i].Amount = parseFloat(amt.toFixed(2));
    }



    this.dynamicArray[i].Taxable = parseFloat(amt.toFixed(2));

    if (this.isEinVoice == false) {

      var OtherExpenseLorryChrges = Number((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value))) + Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value))) + Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value)));

      if (this.advPaid > 0) {
        OtherExpenseLorryChrges += this.advPaid;
      }

      if (OtherExpenseLorryChrges > 0) {
        this.dynamicArray[0].Taxable = Number(amt) + OtherExpenseLorryChrges;
      }
      else {
        this.dynamicArray[i].Taxable = Number(amt);
      }


    }
    else {
      this.dynamicArray[i].Taxable = Number(amt);
    }


    if (Caller == 'Amount' && !this.isDebitNote) {
      //this.dynamicArray[i].Amount = parseFloat(val.toFixed(2));
      this.dynamicArray[i].Amount = val;
      this.dynamicArray[i].Taxable = val;
      if (sessionStorage.getItem('State')?.toUpperCase() == this.defaultState?.toUpperCase()) {
      this.dynamicArray[i].CgstAmount = Number((this.dynamicArray[i].Taxable * this.dynamicArray[i].CgstRate) / 100)
     this.dynamicArray[i].SgstAmount =  Number((this.dynamicArray[i].Taxable * this.dynamicArray[i].SgstRate) / 100);
     this.dynamicArray[i].IgstAmount = 0;

     this.sgstShow = true;
     this.cgstShow = true;
     this.igstShow = false;

      }else {

        this.sgstShow = false;
     this.cgstShow = false;
     this.igstShow = true;

        this.dynamicArray[i].CgstAmount = 0;
        this.dynamicArray[i].SgstAmount = 0;
        this.dynamicArray[i].IgstAmount = this.igstShow
          ? Number((this.dynamicArray[i].Taxable * this.dynamicArray[i].IgstRate) / 100)
          : 0;
      }
    } else {

    if (sessionStorage.getItem('State')?.toUpperCase() == this.defaultState?.toUpperCase()) {

      // this.sgstShow = true;
      // this.cgstShow = true;
      // this.igstShow = false;

      this.dynamicArray[i].CgstAmount = !this.igstShow
        ? Number((this.dynamicArray[i].Taxable * this.dynamicArray[i].CgstRate) / 100)
        : 0;
      this.dynamicArray[i].SgstAmount = !this.igstShow
        ? Number((this.dynamicArray[i].Taxable * this.dynamicArray[i].SgstRate) / 100)
        : 0;
      this.dynamicArray[i].IgstAmount = 0;
    }
    else {

      // this.sgstShow = false;
      // this.cgstShow = false;
      // this.igstShow = true;

      this.dynamicArray[i].CgstAmount = 0;
      this.dynamicArray[i].SgstAmount = 0;
      this.dynamicArray[i].IgstAmount = this.igstShow
        ? Number((this.dynamicArray[i].Taxable * this.dynamicArray[i].IgstRate) / 100)
        : 0;
    }

  }


    this.GetTotal();

  }

  addRow(index: number) {
    // this.newDynamic = { CommodityId: "", WeightPerBag: "", NoOfBags: "", TotalWeight: "", Rate: "", Amount: "", Remark: "", GrandTotal: this.grandTotalAmount, SgstAmount: "", CgstAmount: "", IgstAmount: "", NoOfDocra: "" };
    this.newDynamic = {
      Sno: (this.dynamicArray.length + 1),
      CommodityName: '',
      CommodityId: null,
      WeightPerBag: '',
      NoOfBags: '',
      TotalWeight: '',
      Rate: '',
      Taxable: '',
      Amount: '',
      Remark: '',
      SgstAmount: '',
      CgstAmount: '',
      IgstAmount: '',
      NoOfDocra: '',
    };
    this.dynamicArray.push(this.newDynamic);
    this.secondTime = true;
    return true;
  }

  deleteRow(index: number) {
    if (this.dynamicArray.length == 1) {
      return false;
    } else {
      this.dynamicArray.splice(index, 1);
      this.GetTotal();
      return true;
    }
  }




  changeTotFr(amt: any): void {
    if (amt) {
      this.totalFr = amt;

      if (this.advancePaid == 0 || this.advancePaid == undefined) {
        this.balanceFr = amt - 0;
      } else {
        this.balanceFr = amt - Number(this.advancePaid);
      }
    }
  }

  changeAdvPaid(amt: any): void {
    if (amt) {
      this.advancePaid = amt;
      this.balanceFr = this.totalFr - this.advancePaid;
      if(this.balanceFr < 0){
        this.toastr.error('Balance Freight should not be greater than Advance Paid.');
        return;
      }
      console.log("this.balanceFr -> " + this.balanceFr);
      this.selectFrieght();
    }
  }

  productItemClick($event: any, item: any, ind: number) {

    if (item.commodityName.length >= 1) {
      let UserDetails = {
        SelesItem: {
          Name: '',
          GSTType: item.igst,
        },
      };

      this.adminService.getNewProductList(UserDetails).subscribe({
        next: (res: any) => {

          this.productList = [];
          this.productList = res.Commodities;
          this.newRate = res.Commodities[0].IGST;
          this.dynamicArray[ind].MOU = item.mou;
          this.dynamicArray[ind].SgstRate = item.sgst;
          this.dynamicArray[ind].CgstRate = item.cgst;
          this.dynamicArray[ind].IgstRate = item.igst;
          this.dynamicArray[ind].CommodityId = item.commodityId;
          this.dynamicArray[ind].CommodityName = item.commodityName;
          this.dynamicArray[ind].NoOfDocra = 0;

          // console.log(item.CommodityName);

          this.formGroup2.controls['sgstRate'].setValue(item.sgst);
          this.formGroup2.controls['cgstRate'].setValue(item.cgst);
          this.formGroup2.controls['igstRate'].setValue(item.igst);
          this.formGroup2.controls['totalGst'].setValue(item.igst);
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Something went wrong');
        },
      });
    } else {
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

  SeperateComma(value: any) {

    if(value == undefined)
    {
      value = 0;
    }


    if (!this.isDebitNote) { return value; }
    var x = value;
    x = x.toString();
    var afterPoint = '';
    if (x.indexOf('.') > 0)
      afterPoint = x.substring(x.indexOf('.'), x.length);
    x = Math.floor(x);
    x = x.toString();
    var lastThree = x.substring(x.length - 3);
    var otherNumbers = x.substring(0, x.length - 3);
    if (otherNumbers != '')
      lastThree = ',' + lastThree;
    var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree + afterPoint;
    return res
  }

  OfficeCopy: any;

  onPrint() {

    let payload = {
      SalesDetails: {
        CompanyId: this.globalCompanyId,
        LedgerId: this.ledgerId,
        VoucherNumber: this.invoiceNo,
        VoucherType: this.voucherTypeId,
      },
    };

    this.adminService.getInvType(payload).subscribe({
      next: (res: any) => {
      if(this.route.snapshot.queryParamMap.get('InvoiceType')?.toString() == 'GoodsInvoice' || this.route.snapshot.queryParamMap.get('InvoiceType')?.toString() == 'SalesReturn') {
            if(res.einvreq == 1 && res.frieghtPlus == 1) {
              this.invType = 3;
            } else if(res.einvreq == 1 && res.frieghtPlus == 0) {
              this.invType = 4;
            } else if(res.einvreq == 0 && res.frieghtPlus == 1) {
              this.invType = 1;
            } else if(res.einvreq == 0 && res.frieghtPlus == 0) {
              this.invType = 2;
            }
        } else if(this.route.snapshot.queryParamMap.get('InvoiceType')?.toString() == 'GinningInvoice') {
          this.invType = 7;
        } else if(this.route.snapshot.queryParamMap.get('InvoiceType')?.toString() == 'DebitNote' || this.route.snapshot.queryParamMap.get('InvoiceType')?.toString() == 'CreditNote') {
          this.invType = 5;
        }

        this.Type = this.invType;

      },
      error: (error: any) => {
        this.toastr.error('Something went wrong');
      },
    });

    this.ClientCode = "RGP";
    this.vochType= "9";
    //this.invType = "3";

    let partyDetails = {
      "SalesInvoice": {
        "CompanyId": this.globalCompanyId,
        "LedgerId": this.ledgerId,
        "InvoiceNO": this.invoiceNo,
        "VouchType": this.voucherTypeId,
        "InvoiceType": this.invType
      }
      // "SalesInvoice": {
      //   "CompanyId":1,
      //   "LedgerId": 46,
      //   "InvoiceNO": 1103,
      //   "VouchType": 9,
      //   "InvoiceType": this.invType
      // }
    }

        // "InvoiceNO": this.vochNo,

    console.log("CompanyId -> " + partyDetails.SalesInvoice.CompanyId);
    console.log("LedgerId -> " + partyDetails.SalesInvoice.LedgerId);
    console.log("InvoiceNO -> " + partyDetails.SalesInvoice.InvoiceNO);
    console.log("VouchType -> " + partyDetails.SalesInvoice.VouchType);
    console.log("InvoiceType -> " + partyDetails.SalesInvoice.InvoiceType);

    this.adminService.getItemList(partyDetails).subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          // this.InvoiceData = res.ItemData;
          this.InvoiceData = JSON.parse(res.combinedDataaa);
          // var data = JSON.parse(res.InvoiceData);
          // this.InvoiceData[0].push(data);

          this.CompleteInvoice = "";
          this.TransporterCopy = '';
          this.CustomerCopy = '';

          this.CreateInvoice('Office Copy');
          this.CreateInvoice('Customer Copy');
          this.CreateInvoice('Transporter Copy');

          this.openNewTab('<html><head> <style>td, th {        border: 1px solid black;    text-align: left;font-size: 12px;    }table {    border-collapse: collapse;    width: 100%;}th, td {    padding: 7px;}th {-webkit-print-color-adjust: exact;background-color: gainsboro;} </style></head><body style="font-size: 12px;">' +this.OfficeCopy + '</body></html>');
        } else {
          this.toastr.error(res.Errors[0].Message);
        }
        this.spinner.hide();
      },
      error: (error: any) => {
        console.log(error);
        this.spinner.hide();
        this.toastr.error('Something went wrong');
      },
    });

  //  (<HTMLInputElement>document.getElementById("InvoiceHTML")).innerHTML = '<html><head> <style>td, th {        border: 1px solid black;    text-align: left;font-size: 12px;    }table {    border-collapse: collapse;    width: 100%;}th, td {    padding: 7px;}th {-webkit-print-color-adjust: exact;background-color: gainsboro;} </style></head><body style="font-size: 12px;">' +this.OfficeCopy + '</body></html>';



  }

  convertWeightToCustomFormat(weight: number): string {
    let Qntl: number = 0;

    while (weight >= 100) {
        Qntl++;
        weight -= 100;
    }

    let WeightinString: string = "";
    if (weight < 10) {
        WeightinString = `${Qntl}-0${weight.toFixed(2)}`;
    } else {
        WeightinString = `${Qntl}-${weight.toFixed(2)}`;
    }

    return WeightinString;
  }


  // changes starts from here

  InvoiceData:any;
  vochType:any;
  ClientCode: any;

  CreateInvoice(InvoiceType:any)
  {
    debugger;
    var HtmlBody = '';

HtmlBody +=' ';


     debugger;
HtmlBody +='        <table style="width:100%;">';
HtmlBody +='            <tbody> <tr>';


//base64logo
if(this.InvoiceData[0]['base64logo'] != '' && this.InvoiceData[0]['base64logo'] != undefined)
{
  HtmlBody +='<td style="width:15%;border: 0px solid white;text-align: center;">';
  //HtmlBody +='<img style="width:125px;" src='+ this.EnrollmentPath + this.InvoiceData[0]['logoPath'] +'>';
  HtmlBody +='<img style="width:125px;" src=' + 'data:image/jpeg;base64,' + this.InvoiceData[0]['base64logo'] +'>';
  HtmlBody +='</td>';
  HtmlBody +='                    <td style="width:85%;    border: 0px solid white;text-align:center;padding-top: 0%;"> ';
}
else
{
  HtmlBody +='                    <td style="width:100%;border: 1px solid white;text-align:center;padding-top: 0%;"> ';
}


HtmlBody +='                        <div style="text-align:center; font-size: 28px;">';
HtmlBody +='                            <span id="FirmName" style="color:black;">'+ this.InvoiceData[0]['companyName'] +'</span>';
HtmlBody +='                        </div>';
HtmlBody +='                       <div style="font-weight: bold;font-size:15px;color:black;" >'+ this.InvoiceData[0]['Address1'] +'</div>';

HtmlBody +='                       <div style="font-weight: bold;font-size:15px;" >District : '+ this.InvoiceData[0]['companyDistrict']  + ', &nbsp; ' + this.InvoiceData[0]['companyPlace'] + ', &nbsp; State : ' + this.InvoiceData[0]['companyState'] +'</div>';
HtmlBody +='                       <div style="font-weight: bold;font-size:15px;color:black;" >GSTIN : '+ this.InvoiceData[0]['companyGSTIN'] +'</div>';
HtmlBody +='                       <div style="font-weight: bold;font-size:15px;color:black;" >'+ this.InvoiceData[0]['secondLineForReport'] +'</div>';
HtmlBody +='                       <div style="font-weight: bold;font-size:15px;color:black;" >'+ this.InvoiceData[0]['thirdLineForReport'] +'</div>';

HtmlBody +='               </td>                  ';


//SignQRCODE
debugger;
if(this.InvoiceData[0]['signQRCODE'] != '' && this.InvoiceData[0]['signQRCODE']  != undefined)
{
  HtmlBody +='<td style="width:15%;border: 0px solid white;text-align: center;">';
  HtmlBody +='<img style="width:125px;" src="'+ this.InvoiceData[0]['signQRCODE'] +'">';
  HtmlBody +='</td>';
  HtmlBody +='                    <td style="width:85%;    border: 0px solid white;text-align:center;padding-top: 0%;"> ';
}
else
{
  HtmlBody +='                    <td style="width:100%;border: 1px solid white;text-align:center;padding-top: 0%;"> ';
}
HtmlBody +='                </tr>';

HtmlBody +='            </tbody>';
HtmlBody +='        </table>';


if(this.InvoiceData[0]['irnNo'] != '')
{
  HtmlBody +='        <table style="width:100%;"><tbody><tr><td style="text-align:center;width:66.7%;border-left: 0px solid black;"><h6 style="margin: 0%;">IRNO : ' + this.InvoiceData[0]['irnNo'] +'</h6>';
  HtmlBody +='                    </td>';
  HtmlBody +='                    <td style="    border-right: 0px solid black;text-align:center;">   ';
  HtmlBody +='				<h6 style="margin: 0%;">ACK No : ' + this.InvoiceData[0]['ackno'] +'</h6>';
  HtmlBody +='                    </td>';
  HtmlBody +='                </tr>';
  HtmlBody +='            </tbody>';
  HtmlBody +='        </table>';
}

debugger;
var BillTitle = '';
if(parseFloat(this.InvoiceData[0]['CSGSTValue']) > 0 || parseFloat(this.InvoiceData[0]['IGSTValue']) > 0)
{
if(this.vochType == 9 || this.vochType == 10 || this.vochType == 1)
{
  BillTitle = 'Tax Invoice';
}
else if(this.vochType == 9 || this.vochType == 10 || this.vochType == 11 && (parseFloat(this.InvoiceData[0]['CSGSTValue']) == 0 && parseFloat(this.InvoiceData[0]['IGSTValue']) == 0) )
{
  BillTitle = 'Bill of Supply';
}
else if(this.vochType == 12)
{
  BillTitle = 'Export Invoice';
}
else if(this.vochType == 6 || this.vochType == 8)
{
  BillTitle = 'Credit Note';
}
else if(this.vochType == 14 ||this.vochType == 15)
{
  BillTitle = 'Debit Note';
}
else if(this.vochType == 16)
{
  BillTitle = 'Performa Invoice';
}
}
else if(this.vochType == 9 || this.vochType == 10 || this.vochType == 11 && (parseFloat(this.InvoiceData[0]['CSGSTValue']) == 0 && parseFloat(this.InvoiceData[0]['IGSTValue']) == 0) )
{
  BillTitle = 'Bill of Supply';
}
else if(this.vochType == 12)
{
  BillTitle = 'Export Invoice';
}
else if(this.vochType == 6 || this.vochType == 8)
{
  BillTitle = 'Credit Note';
}
else if(this.vochType == 14 ||this.vochType == 15)
{
  BillTitle = 'Debit Note';
}
else if(this.vochType == 16)
{
  BillTitle = 'Performa Invoice';
}







if(this.Type == '5')
{
  HtmlBody +='        <table style="width:100%;"><tbody><tr><td style="text-align:center;width:33.33%;border-left: 0px solid black;"><h5 style="margin: 0%;">'+ BillTitle +'</h5>';
  HtmlBody +='                    </td>';
    HtmlBody +='                    <td style="    border-right: 0px solid black;text-align:center;width:33.33%;">   ';
  HtmlBody +='				<h5 style="margin: 0%;">'+ InvoiceType +'</h5>';
  HtmlBody +='                    </td>';
  HtmlBody +='                </tr>';
  HtmlBody +='            </tbody>';
  HtmlBody +='        </table>';
}
else
{
  HtmlBody +='        <table style="width:100%;"><tbody><tr><td style="text-align:center;width:33.33%;border-left: 0px solid black;"><h5 style="margin: 0%;">'+ BillTitle +'</h5>';
  HtmlBody +='                    </td>';
  HtmlBody +='		    <td style="text-align:center;width:33.33%;">';
  HtmlBody +='                         <h5 style="margin: 0%;">PO Number:'+ this.InvoiceData[0]['Ponumber'] +'</h5>';
  HtmlBody +='                    </td>';
  HtmlBody +='                    <td style="    border-right: 0px solid black;text-align:center;width:33.33%;">   ';
  HtmlBody +='				<h5 style="margin: 0%;">'+ InvoiceType +'</h5>';
  HtmlBody +='                    </td>';
  HtmlBody +='                </tr>';
  HtmlBody +='            </tbody>';
  HtmlBody +='        </table>';
}




HtmlBody +=' <table style="width:100%;">';
HtmlBody +='            <tbody>';
HtmlBody +='                <tr>';
HtmlBody +='                    <td style="width:50%;border-left: 0px solid black;border-top: 0px solid black;">';
HtmlBody +='                         <span id="txtCustName" style="">Reverse Charge : </span><span style="font-weight:bold;">  NO</span>';

if(this.Type != '5')
{
HtmlBody +='                         <span id="txtCustAddress" style="font-weight: bold; float:right;">E-WAY NO : <br /> '+ this.InvoiceData[0]['EwayBillNo'] +'</span>';
}

HtmlBody +='                         <br /><span id="txtArea" >Invoice No :</span><span style="font-weight: bold;"> '+ this.InvoiceData[0]['displayinvNo'] +'</span><br />';
HtmlBody +='                         <span id="txtDistrict" > Invoice Date :</span><span style="font-weight: bold;"> '+ this.InvoiceData[0]['TranctDate'] +'</span><br />';
HtmlBody +='                         <span id="txtState" >State</span> : <span style="font-weight: bold;"> Karnataka</span>';
HtmlBody +='                         <span style="float:right;"> <span id="PCustomerGSTNo" >State Code : </span><span style="font-weight:bold;">29</span></span>';
HtmlBody +='                    </td>';

HtmlBody +='                    <td   style="border-left: 0px solid black;border-top: 0px solid black;border-right: 0px solid black;">   ';

if(this.Type == '5' || this.Type == '7' )
{
  HtmlBody +='';
}
else
{
HtmlBody +='                         <span id="Span1" >Transport Mode :</span><span style="font-weight: bold;"> By Road </span> ' + (this.InvoiceData[0]['paymentterms'] != '' ? ' <span> Payment terms </span> <span style="font-weight:bold;"> '+ (this.InvoiceData[0]['paymentterms'] )  +'</span> ' : '' ) + ' <br />';
HtmlBody +='                         <span id="Span2" >Vehicle NO :</span><span style="font-weight: bold;">  '+ this.InvoiceData[0]['LorryNo'] +'</span>' + (this.InvoiceData[0]['deliveryterms'] != '' ? ' <span> Delivery terms </span> <span style="font-weight:bold;"> '+ (this.InvoiceData[0]['deliveryterms'] )  +'</span> ' : '' ) + ' <br />';
HtmlBody +='                         <span id="Span3" >Date of Supply :</span><span style="font-weight: bold;"> '+ this.InvoiceData[0]['TranctDate'] +'</span><br />';
HtmlBody +='                         <span id="Span4" ">Place of Supply :</span><span style="font-weight: bold;"> '+ this.InvoiceData[0]['DeliveryPlace'] +'</span><br />';
}


HtmlBody +='                    </td>';
HtmlBody +='                </tr>';
HtmlBody +='            </tbody>';
HtmlBody +='        </table>';

HtmlBody +='<table style="width:100%;">';
HtmlBody +='            <tbody>';
HtmlBody +='                <tr>';
HtmlBody +='                    <td style="width:50%;    vertical-align: initial;   border-bottom: 0px solid black; border-left: 0px solid black;border-top: 0px solid black;border-right: 0px solid black;">';
HtmlBody +='                         <span id="Span5" ><span style="font-weight:bold; font-size: 12px;">Name and Address of Receiver / Billed To : </span> <br /><span style="font-size:14px;font-weight:bold; color:black;">'+ this.InvoiceData[0]['LedgerName'] +'</span> <br/>'+ this.InvoiceData[0]['Address1'] +'<br/> '+ this.InvoiceData[0]['Address2']+'<br/> '+ this.InvoiceData[0]['place'] +'</span><br />';
HtmlBody +='                         <span id="Span6" >GSTIN :</span> <span style="font-weight:bold; color:black;">'+ this.InvoiceData[0]['GST'] +'</span>';
if(this.InvoiceData[0]['ledgerfssai'] != '')
{
HtmlBody +='                         <span style="float:right;"><span id="Span8" >FSSAI : </span><span style="font-weight:bold;"> '+ this.InvoiceData[0]['ledgerfssai'] +'</span></span>';
}
HtmlBody +='                       <br> <span id="Span7" >State : </span><span style="font-weight:bold;"> '+ this.InvoiceData[0]['State']+'-'+this.InvoiceData[0]['stateCode2'] +'</span>';
HtmlBody +='                         <span style="float:right;"><span id="Span8" >Contact No : </span><span style="font-weight:bold;"> '+ this.InvoiceData[0]['ledgerCELLNO'] +'</span></span>';
HtmlBody +='                    </td>';

HtmlBody +='                    <td  style="  border-bottom: 0px solid black;  border-top: 0px solid black;border-right: 0px solid black;    vertical-align: initial;">   ';

if(this.Type == '5' || this.Type == '7')
{
  HtmlBody +='';

}
else
{
  HtmlBody +='                         <span id="Span9" ><span style="font-size: 12px;font-weight:bold;">Details of Consingee / Shipped To : </span> <br /><span style="font-size: 14px;font-weight:bold;">'+ this.InvoiceData[0]['DeliveryName'] +'</span><br/>'+ this.InvoiceData[0]['DeliveryAddress1']  +'<br/> '+ this.InvoiceData[0]['DeliveryAddress2']+'<br/> '+ this.InvoiceData[0]['DeliveryPlace'] +'</span><br />';
  HtmlBody +='                        <br> <span id="Span10">'+ this.InvoiceData[0]['DeliveryState'] +'</span>';
  HtmlBody +='                         <span style="float:right;"> <span id="Span11" >State Code :</span><span style="font-weight:bold;">  '+ this.InvoiceData[0]['DeliveryStateCode'] +'</span></span>';
}


HtmlBody +='                    </td>';
HtmlBody +='                </tr>';
HtmlBody +='            </tbody>';
HtmlBody +='        </table>';

HtmlBody +='        <table >';
HtmlBody +='            <thead>';
HtmlBody +='              <tr>';
HtmlBody +='                <th style="border-left: 0px solid black;width:5%;font-weight: bold;font-size: 12px;text-align:center;">SL No.</th>';


if(this.Type == '6')
{
HtmlBody +='                <th style="width:35%;font-weight: bold;font-size: 12px;text-align:left;">Item Description</th>';
}
else
{
HtmlBody +='                <th style="width:25%;font-weight: bold;font-size: 12px;text-align:left;">Item Description</th>';
}

HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">HSN Code</th>';

if(this.Type == '5')
{
  HtmlBody +='';
}
else if(this.Type == '6' || this.Type == '7')
{
  HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">Qty</th>';
  HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">Rate</th>';
}
else
{
HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">UOM</th>';
HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">No of Bags</th>';
HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">Weight Per Bag</th>';

HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">Weight (Qntl-Kg)</th>';
HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">Rate Per Quintal</th>';
}

HtmlBody +='                <th style="border-right: 0px solid black;width:10%;font-weight: bold;font-size: 12px;text-align:center;">Amount</th>';
HtmlBody +='              </tr>';
HtmlBody +='            </thead>';
HtmlBody +='            <tbody id="PrintTBody">';

var p = 0;
let totalNumberOfBags = 0;
let totalWeightQnt = 0;
let totalAmount = 0;
for(p=0;p<this.InvoiceData[0].Itemresult.length; p++)
{


  HtmlBody +='            <tr>';
  HtmlBody +='            <td style="border-left: 0px solid black;">'+ (p + 1) +'</td>';
  HtmlBody +='            <td style="overflow-wrap: anywhere;">'+ this.InvoiceData[0].Itemresult[p]['CommodityName'] + ' ' + this.InvoiceData[0].Itemresult[p]['Mark'] +'</td>';
  HtmlBody +='            <td style="text-align:center;">'+ this.InvoiceData[0].Itemresult[p]['hsn'] +'</td>';

  if(this.Type == '5')
  {
    HtmlBody +='';
  }
  else if(this.Type == '6' || this.Type == '7')
  {
    HtmlBody +='            <td style="text-align:center;">'+ this.InvoiceData[0].Itemresult[p]['lineItemQty'] +'</td>';
    HtmlBody +='            <td style="text-align:right;">'+ this.SeperateComma(parseFloat(this.InvoiceData[0].Itemresult[p]['Rate']).toFixed(2)) +'</td>';
  }
  else
  {
  HtmlBody +='            <td style="text-align:center;">'+ this.InvoiceData[0].Itemresult[p]['mou'] +'</td>';
  HtmlBody +='            <td style="text-align:right;">'+ this.InvoiceData[0].Itemresult[p]['NoOfBags'] +'</td>';

  totalNumberOfBags += this.InvoiceData[0].Itemresult[p]['NoOfBags'];

  if(parseFloat(this.InvoiceData[0].Itemresult[p]['WeightPerBag']) > 0)
  {
  HtmlBody +='            <td style="text-align:right;">'+ this.InvoiceData[0].Itemresult[p]['WeightPerBag'] +'</td>';
  }
  else
  {
    HtmlBody +='            <td style="text-align:right;"></td>';
  }
  let formattedWgt: string = this.convertWeightToCustomFormat(this.InvoiceData[0].Itemresult[p]['TotalWeight']);
  HtmlBody +='            <td style="text-align:right;">'+ formattedWgt +'</td>';
  totalWeightQnt += Number(this.InvoiceData[0].Itemresult[p]['TotalWeight']);
  HtmlBody +='            <td style="text-align:right;">'+ this.SeperateComma(parseFloat(this.InvoiceData[0].Itemresult[p]['Rate']).toFixed(2)) +'</td>';
}

  HtmlBody +='            <td style="border-right: 0px solid black;text-align:right;">'+ this.SeperateComma(parseFloat(this.InvoiceData[0].Itemresult[p]['Amount']).toFixed(2)) +'</td>';
  totalAmount += Number(this.InvoiceData[0].Itemresult[p]['Amount']);
  HtmlBody +='            </tr>';


  }



//////// Blank Rows Start /////////


// var BlankRowsCount = 10 - this.InvoiceData.length;
var BlankRowsCount = 10 - this.InvoiceData[0].Itemresult.length;

var u = 0;
for(u=0;u<BlankRowsCount; u++)
{
  HtmlBody +='            <tr>';
  HtmlBody +='            <td style="border-left: 0px solid black;padding:1.06%;border: 0px;"></td>';
  HtmlBody +='            <td style="overflow-wrap: anywhere;border: 0px;"></td>';
  HtmlBody +='            <td style="text-align:center;border: 0px;"></td>';

  if(this.Type == '5')
  {
    HtmlBody +='';
  }
  else
  {

    if(this.Type == '6' || this.Type == '7')
    {
  HtmlBody +='            <td style="text-align:center;border: 0px;"></td>';
  HtmlBody +='            <td style="text-align:right;border: 0px;"></td>';

    }
else
{
  HtmlBody +='            <td style="text-align:center;border: 0px;"></td>';
  HtmlBody +='            <td style="text-align:right;border: 0px;"></td>';

  if(parseFloat(this.InvoiceData[0]['WeightPerBag']) > 0)
  {
  HtmlBody +='            <td style="text-align:right;border: 0px;"></td>';
  }
  else
  {
    HtmlBody +='            <td style="text-align:right;border: 0px;"></td>';
  }
  HtmlBody +='            <td style="text-align:center;border: 0px;"></td>';
  HtmlBody +='            <td style="text-align:right;border: 0px;"></td>';
}
  }
  HtmlBody +='            <td style="border-right: 0px solid black;text-align:right;border: 0px;"></td>';
  HtmlBody +='            </tr>';

}


//////// Blank Rows End //////////











  HtmlBody +='            <tr style="background: gainsboro;font-weight: bold;">';
  HtmlBody +='            <td style="border-left: 0px solid black;"></td>';
  HtmlBody +='            <td></td>';
  HtmlBody +='            <td style="text-align:center;"></td>';

  if(this.Type == '5')
{
  HtmlBody +='';
}
else
{

  if(this.Type=='6' || this.Type == '7')
  {
    HtmlBody +='            <td style="text-align:center;">'+ this.InvoiceData[0]['TotalWeight'] +'</td>';
    HtmlBody +='            <td style="text-align:center;"></td>';
  }
  else
  {




  HtmlBody +='            <td style="text-align:center;"></td>';
  // HtmlBody +='            <td style="text-align:right;">'+ this.InvoiceData[0]['totalBags'] +'</td>';
  HtmlBody +='            <td style="text-align:right;">'+ totalNumberOfBags +'</td>';


  //HtmlBody +='            <td style="text-align:right;"></td>';
  HtmlBody +='            <td style="text-align:right;padding:0%;font-size:12px;"></td>';



  if(totalWeightQnt == 0) {
    HtmlBody +='            <td style="text-align:right;padding:0%;font-size:12px;"></td>';
  } else {
    let formattedWeight: string = this.convertWeightToCustomFormat(totalWeightQnt);
    HtmlBody +='            <td style="text-align:right;padding:0%;font-size:12px;">'+ formattedWeight +'</td>';
  }

  HtmlBody +='            <td style="text-align:right;"></td>';
}
}
  HtmlBody +='            <td style="border-right: 0px solid black;text-align:right;">'+ this.SeperateComma(totalAmount.toFixed(2)) +'</td>';
  HtmlBody +='            </tr>';



HtmlBody +='            </tbody>';
HtmlBody +='        </table>';



HtmlBody +='        <table style="border-top: 0px solid white;border-bottom: 0px solid white;">';
HtmlBody +='                    <tr>';
HtmlBody +='                        <td style="border-left: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;width:40%;">';

if(this.Type == '5' || this.Type == '7')
{
  HtmlBody +='';
}
else
{
HtmlBody +='                            <div style="font-weight:bold;">Lorry Details :</div>';
HtmlBody +='                            <div>Lorry No: '+ this.InvoiceData[0]['LorryNo'] +'</div>';
HtmlBody +='                            <div>Lorry Owner : '+ this.InvoiceData[0]['LorryOwnerName'] +'</div>';
HtmlBody +='                            <div>Lorry Driver : '+ this.InvoiceData[0]['DriverName'] +'</div>';
HtmlBody +='                            <div>DL No : '+ this.InvoiceData[0]['Dlno'] +'</div>';
HtmlBody +='                            <div>Transporter : '+ this.InvoiceData[0]['Transporter'] +'</div>';
HtmlBody +='                            <br />';
HtmlBody +='				<div style="font-weight:bold;">Freight Details :</div>';
HtmlBody +='                            <div>Freight/Bag : <span id="PCompanyPanno">'+ this.InvoiceData[0]['FrieghtPerBag'] +'</span></div>';
HtmlBody +='                            <div>Total Freight : <span id="PGSTNO">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['TotalFrieght']).toFixed(2)) +'</span></div>';
HtmlBody +='                            <div>Advance Paid : <span id="Span12">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['Advance']).toFixed(2)) +'</span></div>';
HtmlBody +='                            <div>Freight Payable : <span id="Span13">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['Balance']).toFixed(2)) +'</span></div>';
}
HtmlBody +='                        </td>';





if(this.Type == '6')
{
HtmlBody +='                        <td style="border-left: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;width: 22.5%;">';
}
else
{
HtmlBody +='                        <td style="border-left: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;width:20%;">';
}



HtmlBody +='                            <div>Seller PAN: '+ this.InvoiceData[0]['companyPAN'] +'</div>';
HtmlBody +='                            <div>Buyer PAN : '+ this.InvoiceData[0]['ledgerPAN'] +'</div><br>';

debugger;


if(this.Type == '1')
{
  HtmlBody +='                            <div>Dispatched From : '+ this.InvoiceData[0]['FromPlace'] +'</div>';
  HtmlBody +='                            <div>Dispatched To : '+ this.InvoiceData[0]['ToPlace'] +'</div>';
}

if(this.Type == '2')
{
  HtmlBody +='                            <div>Dispatched From : '+ this.InvoiceData[0]['FromPlace'] +'</div>';
  HtmlBody +='                            <div>Dispatched To : '+ this.InvoiceData[0]['ToPlace'] +'</div>';
}

if(this.Type == '3')
{
  HtmlBody +='                            <div>Dispatched From : '+ this.InvoiceData[0]['FromPlace'] +'</div>';
  HtmlBody +='                            <div>Dispatched To : '+ this.InvoiceData[0]['ToPlace'] +'</div>';
}

if(this.Type == '4')
{
  HtmlBody +='                            <div>Dispatched From : '+ this.InvoiceData[0]['FromPlace'] +'</div>';
  HtmlBody +='                            <div>Dispatched To : '+ this.InvoiceData[0]['ToPlace'] +'</div>';
}

if(this.Type == '6')
{
  HtmlBody +='                            <div>Dispatched From : '+ this.InvoiceData[0]['FromPlace'] +'</div>';
  HtmlBody +='                            <div>Dispatched To : '+ this.InvoiceData[0]['ToPlace'] +'</div>';
}





HtmlBody +='                        </td>';






HtmlBody +='                      <td style="border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;width:20%;">';





debugger;
if(this.Type == '1')
{


  HtmlBody +='        <div>'+ this.InvoiceData[0]['frieghtLabel'] +'</div>';

  if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='        <div>'+ this.InvoiceData[0]['ExpenseName1'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName2'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName3'] +'</div>';
}



HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.InvoiceData[0]['sgstLabel'] +'</div>';
HtmlBody +='				<div>'+ this.InvoiceData[0]['cgstLabel'] +'</div>';
}
else
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['igstLabel'] +'</div>';
}










if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['tcsLabel'] +'</div>';
}

HtmlBody +='				<div>Round Off</div>';






HtmlBody +='                        </td>';
HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;text-align:right">';


if(parseFloat(this.InvoiceData[0]['frieghtinBill']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['frieghtinBill']).toFixed(2)) +'</div>';
}

if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount1']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount2']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount3']).toFixed(2)) +'</div>';
}


HtmlBody +='                           <div style="font-weight:bold;color:black;">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['TaxableValue']).toFixed(2)) +'</div>';


if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['SGSTValue']).toFixed(2)) +'</div>';
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['CSGSTValue']).toFixed(2)) +'</div>';
}
else
{
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['IGSTValue']).toFixed(2)) +'</div>';
}



if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['tcsValue']).toFixed(2)) +'</div>';
}

HtmlBody +='				<div>'+ this.InvoiceData[0]['RoundOff'].toFixed(2) +'</div>';






}


debugger;
if(this.Type == '7')
{


  HtmlBody +='        <div>'+ this.InvoiceData[0]['frieghtLabel'] +'</div>';

  if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='        <div>'+ this.InvoiceData[0]['ExpenseName1'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName2'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName3'] +'</div>';
}



HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.InvoiceData[0]['sgstLabel'] +'</div>';
HtmlBody +='				<div>'+ this.InvoiceData[0]['cgstLabel'] +'</div>';
}
else
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['igstLabel'] +'</div>';
}










if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['tcsLabel'] +'</div>';
}

HtmlBody +='				<div>Round Off</div>';






HtmlBody +='                        </td>';
HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;text-align:right">';


if(parseFloat(this.InvoiceData[0]['frieghtinBill']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['frieghtinBill']).toFixed(2)) +'</div>';
}

if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount1']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount2']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount3']).toFixed(2)) +'</div>';
}


HtmlBody +='                           <div style="font-weight:bold;color:black;">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['TaxableValue']).toFixed(2)) +'</div>';


if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['SGSTValue']).toFixed(2)) +'</div>';
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['CSGSTValue']).toFixed(2)) +'</div>';
}
else
{
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['IGSTValue']).toFixed(2)) +'</div>';
}



if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['tcsValue']).toFixed(2)) +'</div>';
}

HtmlBody +='				<div>'+ this.InvoiceData[0]['RoundOff'].toFixed(2) +'</div>';






}


debugger;
if(this.Type == '6')
{


  HtmlBody +='        <div>'+ this.InvoiceData[0]['frieghtLabel'] +'</div>';

  if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='        <div>'+ this.InvoiceData[0]['ExpenseName1'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName2'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName3'] +'</div>';
}



HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.InvoiceData[0]['sgstLabel'] +'</div>';
HtmlBody +='				<div>'+ this.InvoiceData[0]['cgstLabel'] +'</div>';
}
else
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['igstLabel'] +'</div>';
}










if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['tcsLabel'] +'</div>';
}

HtmlBody +='				<div>Round Off</div>';






HtmlBody +='                        </td>';
HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;text-align:right">';


if(parseFloat(this.InvoiceData[0]['frieghtinBill']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['frieghtinBill']).toFixed(2)) +'</div>';
}

if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount1']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount2']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount3']).toFixed(2)) +'</div>';
}


HtmlBody +='                           <div style="font-weight:bold;color:black;">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['TaxableValue']).toFixed(2)) +'</div>';


if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['SGSTValue']).toFixed(2)) +'</div>';
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['CSGSTValue']).toFixed(2)) +'</div>';
}
else
{
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['IGSTValue']).toFixed(2)) +'</div>';
}



if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['tcsValue']).toFixed(2)) +'</div>';
}

HtmlBody +='				<div>'+ this.InvoiceData[0]['RoundOff'].toFixed(2) +'</div>';






}





debugger;
if(this.Type == '2')
{




  if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='        <div>'+ this.InvoiceData[0]['ExpenseName1'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName2'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName3'] +'</div>';
}



HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.InvoiceData[0]['sgstLabel'] +'</div>';
HtmlBody +='				<div>'+ this.InvoiceData[0]['cgstLabel'] +'</div>';
}
else
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['igstLabel'] +'</div>';
}









if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['tcsLabel'] +'</div>';
}

HtmlBody +='				<div>Round Off</div>';


HtmlBody +='				<div><b>Invoice Amount</b></div>';

HtmlBody +='        <div>'+ this.InvoiceData[0]['frieghtLabel'] +'</div>';




HtmlBody +='                        </td>';
HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;;text-align:right">';



if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount1']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount2']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount3']).toFixed(2)) +'</div>';
}


HtmlBody +='                           <div style="font-weight:bold;color:black">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['TaxableValue']).toFixed(2)) +'</div>';


if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['SGSTValue']).toFixed(2)) +'</div>';
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['CSGSTValue']).toFixed(2)) +'</div>';
}
else
{
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['IGSTValue']).toFixed(2)) +'</div>';
}








if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['tcsValue']).toFixed(2)) +'</div>';
}

HtmlBody +='				<div>'+ this.InvoiceData[0]['RoundOff'].toFixed(2) +'</div>';



debugger;
HtmlBody +='				<div><b>'+ this.SeperateComma((parseFloat(this.InvoiceData[0]['TaxableValue']) + parseFloat(this.InvoiceData[0]['IGSTValue']) + parseFloat(this.InvoiceData[0]['SGSTValue']) + parseFloat(this.InvoiceData[0]['CSGSTValue']) + parseFloat(this.InvoiceData[0]['tcsValue']) + parseFloat(this.InvoiceData[0]['RoundOff'])).toFixed(2)) +'</b></div>';
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['frieghtinBill']).toFixed(2)) +'</div>';

}



debugger;
if(this.Type == '3')
{



HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.InvoiceData[0]['sgstLabel'] +'</div>';
HtmlBody +='				<div>'+ this.InvoiceData[0]['cgstLabel'] +'</div>';
}
else
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['igstLabel'] +'</div>';
}


HtmlBody +='        <div>'+ this.InvoiceData[0]['frieghtLabel'] +'</div>';





if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='        <div>'+ this.InvoiceData[0]['ExpenseName1'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName2'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName3'] +'</div>';
}

if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['tcsLabel'] +'</div>';
}

HtmlBody +='				<div>Round Off</div>';




HtmlBody +='                        </td>';
HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;text-align:right">';

HtmlBody +='                           <div style="font-weight:bold;color:black;">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['TaxableValue']).toFixed(2)) +'</div>';


if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['SGSTValue']).toFixed(2)) +'</div>';
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['CSGSTValue']).toFixed(2)) +'</div>';
}
else
{
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['IGSTValue']).toFixed(2)) +'</div>';
}


if(parseFloat(this.InvoiceData[0]['frieghtinBill'])> 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['frieghtinBill']).toFixed(2)) +'</div>';
}

if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount1']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount2']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount3']).toFixed(2)) +'</div>';
}



if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['tcsValue']).toFixed(2)) +'</div>';
}

HtmlBody +='				<div>'+ this.InvoiceData[0]['RoundOff'].toFixed(2) +'</div>';






}











debugger;
if(this.Type == '4')
{



HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.InvoiceData[0]['sgstLabel'] +'</div>';
HtmlBody +='				<div>'+ this.InvoiceData[0]['cgstLabel'] +'</div>';
}
else
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['igstLabel'] +'</div>';
}







if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='        <div>'+ this.InvoiceData[0]['ExpenseName1'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName2'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName3'] +'</div>';
}

if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['tcsLabel'] +'</div>';
}

HtmlBody +='				<div>Round Off</div>';

HtmlBody +='				<div style="font-weight: bold;">Invoice Amount</div>';


  HtmlBody +='        <div>'+ this.InvoiceData[0]['frieghtLabel'] +'</div>';

HtmlBody +='                        </td>';
HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;width:10%;text-align:right">';

HtmlBody +='                           <div style="font-weight:bold;color:black;">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['TaxableValue']).toFixed(2)) +'</div>';


if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['SGSTValue']).toFixed(2)) +'</div>';
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['CSGSTValue']).toFixed(2)) +'</div>';
}
else
{
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['IGSTValue']).toFixed(2)) +'</div>';
}




if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount1']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount2']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount3']).toFixed(2)) +'</div>';
}



if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['tcsValue']).toFixed(2)) +'</div>';
}

HtmlBody +='				<div>'+ this.InvoiceData[0]['RoundOff'].toFixed(2) +'</div>';


debugger;
var GrandTotal = 0;

GrandTotal = parseFloat(this.InvoiceData[0]['TaxableValue']) + parseFloat(this.InvoiceData[0]['SGSTValue']) + parseFloat(this.InvoiceData[0]['CSGSTValue']) + parseFloat(this.InvoiceData[0]['IGSTValue'])  + parseFloat(this.InvoiceData[0]['ExpenseAmount1']) + parseFloat(this.InvoiceData[0]['ExpenseAmount2']) + parseFloat(this.InvoiceData[0]['ExpenseAmount3']) + parseFloat(this.InvoiceData[0]['tcsValue']);

HtmlBody +='				<div style="font-weight: bold;">₹'+ this.SeperateComma(GrandTotal.toFixed(2)) +'</div>';


HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['frieghtinBill']).toFixed(2)) +'</div>';
}

debugger;
if(this.Type == '5')
{



HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.InvoiceData[0]['sgstLabel'] +'</div>';
HtmlBody +='				<div>'+ this.InvoiceData[0]['cgstLabel'] +'</div>';
}
else
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['igstLabel'] +'</div>';
}







if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='        <div>'+ this.InvoiceData[0]['ExpenseName1'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName2'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName3'] +'</div>';
}

if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['tcsLabel'] +'</div>';
}

HtmlBody +='				<div>Round Off</div>';






HtmlBody +='                        </td>';
HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;text-align:right">';

HtmlBody +='                           <div style="font-weight:bold;color:black;">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['TaxableValue']).toFixed(2)) +'</div>';


if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['SGSTValue']).toFixed(2)) +'</div>';
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['CSGSTValue']).toFixed(2)) +'</div>';
}
else
{
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['IGSTValue']).toFixed(2)) +'</div>';
}




if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount1']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount2']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount3']).toFixed(2)) +'</div>';
}


debugger;
if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['tcsValue']).toFixed(2)) +'</div>';
}

HtmlBody +='				<div>'+ this.InvoiceData[0]['RoundOff'].toFixed(2) +'</div>';



var GrandTotal = 0;

GrandTotal = parseFloat(this.InvoiceData[0]['TaxableValue']) + parseFloat(this.InvoiceData[0]['SGSTValue']) + parseFloat(this.InvoiceData[0]['CSGSTValue']) + parseFloat(this.InvoiceData[0]['IGSTValue'])  + parseFloat(this.InvoiceData[0]['ExpenseAmount1']) + parseFloat(this.InvoiceData[0]['ExpenseAmount2']) + parseFloat(this.InvoiceData[0]['ExpenseAmount3']) + parseFloat(this.InvoiceData[0]['tcsValue']);






}


HtmlBody +='                        </td>';



HtmlBody +='            </tr>';



HtmlBody +='<tr>';
HtmlBody +='<td style="font-weight:bold;border-left: 0px solid black;" colspan="2">Note : '+ this.InvoiceData[0]['note1'] +'</td>';

HtmlBody +='<td style="font-weight:bold;">Grand Total</td>';
HtmlBody +='<td style="border-right: 0px solid black;font-weight:bold;text-align:right;color:black;">₹ '+ this.SeperateComma(parseFloat(this.InvoiceData[0]['billAmount']).toFixed(2)) +'</td>';
HtmlBody +='</tr>';
HtmlBody +='        </table>';
HtmlBody +='  <table style="border-top: 0px solid white;">';
HtmlBody +='            <tr>';
HtmlBody +='                <td style="border-top: 0px solid white;    border-left: 0px solid white;border-right: 0px solid white;">';
HtmlBody +='                    <span style="font-weight:bold;color:black;">'+ this.InvoiceData[0]['inWords'] +'</span>';
HtmlBody +='                    <span style="font-weight:bold;" id="txtAmount"></span>';

HtmlBody +='                </td>';
HtmlBody +='            </tr>';
HtmlBody +='        </table>';
HtmlBody +='  <table style="border-top: 0px solid white;">';
HtmlBody +='<tr>';
HtmlBody +='<th style="  border-left: 0px solid black;  font-weight: bold;font-size: 12px;text-align:center;">Bank Name</th>';
HtmlBody +=' <th style="   font-weight: bold;font-size: 12px;text-align:center;">IFSC Code</th>';
HtmlBody +='<th style=" border-right: 0px solid black;  font-weight: bold;font-size: 12px;text-align:center;">Account No</th>';

HtmlBody +='</tr>';
HtmlBody +='           <tr>';
HtmlBody +='                <td style="border-left: 0px solid black;border-top: 0px solid white;text-align:center; ">'+ this.InvoiceData[0]['bank1'] +'';

HtmlBody +='                </td>';
HtmlBody +=' <td style="border-top: 0px solid white;text-align:center;">'+ this.InvoiceData[0]['ifsC1'] +'';

HtmlBody +='                </td>';
HtmlBody +=' <td style="text-align:center;border-right: 0px solid black;border-top: 0px solid white;">'+ this.InvoiceData[0]['accountNo1'] +'';

HtmlBody +='                </td>';
HtmlBody +='            </tr>';

HtmlBody +='</tr>';
//if(this.InvoiceData[0]['bank2'] != '')
//{
HtmlBody +='           <tr>';
HtmlBody +='                <td style="border-left: 0px solid black;border-top: 0px solid white;text-align:center;">'+ (this.InvoiceData[0]['bank2'] != '' ? this.InvoiceData[0]['bank2'] : '-') +'';

HtmlBody +='                </td>';
HtmlBody +=' <td style="border-top: 0px solid white;text-align:center;">'+ this.InvoiceData[0]['ifsC2'] +'';

HtmlBody +='                </td>';
HtmlBody +=' <td style="border-right: 0px solid black;border-top: 0px solid white;text-align:center;">'+ this.InvoiceData[0]['accountNo2'] +'';

HtmlBody +='                </td>';
HtmlBody +='            </tr>';
//}
HtmlBody +='</tr>';

//if(this.InvoiceData[0]['bank3'] != '')
//{
HtmlBody +='           <tr>';
HtmlBody +='                <td style="border-left: 0px solid black;border-top: 0px solid white;text-align:center;">'+ (this.InvoiceData[0]['bank3'] != '' ? this.InvoiceData[0]['bank3'] : '-') +'';

HtmlBody +='                </td>';
HtmlBody +=' <td style="border-top: 0px solid white;text-align:center;">'+ this.InvoiceData[0]['ifsC3'] +'';

HtmlBody +='                </td>';
HtmlBody +=' <td style="border-right: 0px solid black;border-top: 0px solid white;text-align:center;">'+ this.InvoiceData[0]['account3'] +'';

HtmlBody +='                </td>';
HtmlBody +='            </tr>';
//}

HtmlBody +='        </table>';
HtmlBody +=' <table style="border-top: 0px solid white;">';
HtmlBody +='            <tr>';
HtmlBody +='                <td style="    border-left: 0px solid black;border-right: 0px solid black;border-top: 0px solid white;">';
HtmlBody +='                    <span style="">Terms of Business :</span>';
HtmlBody +='                    <span style="font-weight:bold;">1) Interest from date of purchase will be charged @ 2% per month.</span> <br />';
HtmlBody +='                    <span style="font-weight:bold;margin-left:116px;">'+ this.InvoiceData[0]['jurisLine'] +'</span><br />';

if(this.ClientCode != 'AVS')
{
  if(this.Type != '7')
  {
HtmlBody +='                    <span style="font-weight:bold;margin-left:116px;">3) We are not reponsible for any loss in weight or damage during transit.</span>';
  }
}
HtmlBody +='                </td>';
HtmlBody +='            </tr>';
HtmlBody +='        </table>';

HtmlBody +=' <table style="border-top: 0px solid white;width:100%;">';
HtmlBody +='            <tr>';
HtmlBody +='<td style=" width:20%;   border-left: 0px solid black;border-top: 0px solid white;border-right: 0px solid white;text-align:left;">';

if(this.Type != '7')
{
HtmlBody +='<span> Driver/Transporters</span><br><br><br>';
HtmlBody +='<span style="">Authorised Signature</span>';
}

HtmlBody +='</td>';
HtmlBody +='<td style="width:20%; border-top: 0px solid white;border-left: 0px solid white;border-right: 0px solid white;text-align:right;">';

if(this.Type != '7')
{
HtmlBody +='<span></span><br><br><br>';
HtmlBody +='<span>Common Seal</span>';
}
HtmlBody +='</td>';
HtmlBody +='<td style=" width:60%;   border-right: 0px solid black;border-top: 0px solid white;border-left: 0px solid white;">';
HtmlBody +='<h6 style="text-align:right;">Certified that the particulars given above are true and correct</h6>';
HtmlBody +='<span></span><br>';
HtmlBody +='<span style="margin-left:55%;">Authorised Signature</span>';
HtmlBody +='</td>';
HtmlBody +='            </tr>';
HtmlBody +='        </table>';



this.CompleteInvoice += ' <div   style="border: 1px solid black;border-bottom: 0px solid white;page-break-after: always;">' +HtmlBody + '</div>';

if(InvoiceType == 'Customer Copy')
{
    this.CustomerCopy = '<html><head> <style> body {font-family: Tahoma, Verdana, Segoe, sans-serif;}  td, th {        border: 1px solid black;    text-align: left;font-size: 13px;    }table {    border-collapse: collapse;    width: 100%;}th, td {    padding: 2px;}th {-webkit-print-color-adjust: exact;background-color: gainsboro;} </style></head><body style="font-size: 12px;"> <div   style="border: 1px solid black;border-bottom: 0px solid white;">' +HtmlBody + '</div></body></html>';
  }
else if(InvoiceType == 'Transporter Copy')
{
  this.TransporterCopy = '<html><head> <style>body {font-family: Tahoma, Verdana, Segoe, sans-serif;} td, th {        border: 1px solid black;    text-align: left;font-size: 13px;    }table {    border-collapse: collapse;    width: 100%;}th, td {    padding: 2px;}th {-webkit-print-color-adjust: exact;background-color: gainsboro;} </style></head><body style="font-size: 12px;"> <div   style="border: 1px solid black;border-bottom: 0px solid white;">' +HtmlBody + '</div></body></html>';
}
else if(InvoiceType == 'Office Copy')
{
  this.OfficeCopy = '<html><head> <style> body {font-family: Tahoma, Verdana, Segoe, sans-serif;} td, th {        border: 1px solid black;    text-align: left;font-size: 13px;    }table {    border-collapse: collapse;    width: 100%;}th, td {    padding: 2px;}th {-webkit-print-color-adjust: exact;background-color: gainsboro;} </style></head><body style="font-size: 12px;"> <div   style="border: 1px solid black;border-bottom: 0px solid white;">' +HtmlBody + '</div></body></html>';
}



  }

//   CreateInvoice(InvoiceType:any)
//   {
//     debugger;
//     var HtmlBody = '';

// HtmlBody +=' ';


//      debugger;
// HtmlBody +='        <table style="width:100%;">';
// HtmlBody +='            <tbody> <tr>';


// //base64logo
// // if(this.dynamicArray[0]['base64logo'] != '')
// if(false)
// {
//   // HtmlBody +='<td style="width:15%;border: 0px solid white;text-align: center;">';
//   // //HtmlBody +='<img style="width:125px;" src='+ this.EnrollmentPath + this.dynamicArray[0]['logoPath'] +'>';
//   // HtmlBody +='<img style="width:125px;" src=' + 'data:image/jpeg;base64,' + this.dynamicArray[0]['base64logo'] +'>';
//   // HtmlBody +='</td>';
//   // HtmlBody +='                    <td style="width:85%;    border: 0px solid white;text-align:center;padding-top: 0%;"> ';
// }
// else
// {
//   HtmlBody +='                    <td style="width:100%;border: 1px solid white;text-align:center;padding-top: 0%;"> ';
// }


// HtmlBody +='                        <div style="text-align:center; font-size: 28px;">';
// HtmlBody +='                            <span id="FirmName" style="color:black;">'+ this.dispatcherName +'</span>';
// HtmlBody +='                        </div>';
// HtmlBody +='                       <div style="font-weight: bold;font-size:15px;color:black;" >'+ this.dispatcherAddress1 +'</div>';

// HtmlBody +='                       <div style="font-weight: bold;font-size:15px;" >District : '+ this.dispatcherAddress2  + ', &nbsp; ' + this.companyPlace + ', &nbsp; State : ' + "KA" +'</div>';
// // HtmlBody +='                       <div style="font-weight: bold;font-size:15px;color:black;" >GSTIN : '+ this.dynamicArray[0]['companyGSTIN'] +'</div>';
// // HtmlBody +='                       <div style="font-weight: bold;font-size:15px;color:black;" >'+ this.dynamicArray[0]['secondLineForReport'] +'</div>';
// // HtmlBody +='                       <div style="font-weight: bold;font-size:15px;color:black;" >'+ this.dynamicArray[0]['thirdLineForReport'] +'</div>';

// HtmlBody +='               </td>                  ';


// //SignQRCODE
// debugger;
// if(false)
// {
//   // HtmlBody +='<td style="width:15%;border: 0px solid white;text-align: center;">';
//   // HtmlBody +='<img style="width:125px;" src="'+ this.dynamicArray[0]['signQRCODE'] +'">';
//   // HtmlBody +='</td>';
//   // HtmlBody +='                    <td style="width:85%;    border: 0px solid white;text-align:center;padding-top: 0%;"> ';
// }
// else
// {
//   HtmlBody +='                    <td style="width:100%;border: 1px solid white;text-align:center;padding-top: 0%;"> ';
// }
// HtmlBody +='                </tr>';

// HtmlBody +='            </tbody>';
// HtmlBody +='        </table>';


// if(this.irn != "")
// {
//   HtmlBody +='        <table style="width:100%;"><tbody><tr><td style="text-align:center;width:66.7%;border-left: 0px solid black;"><h6 style="margin: 0%;">IRNO : ' + this.irn +'</h6>';
//   HtmlBody +='                    </td>';
//   HtmlBody +='                    <td style="    border-right: 0px solid black;text-align:center;">   ';
//   HtmlBody +='				<h6 style="margin: 0%;">ACK No : ' + this.ackNo +'</h6>';
//   HtmlBody +='                    </td>';
//   HtmlBody +='                </tr>';
//   HtmlBody +='            </tbody>';
//   HtmlBody +='        </table>';
// }

// debugger;
// var BillTitle = 'DEMO';
// if(this.dynamicArray[0].CgstAmount > 0 || this.dynamicArray[0].IgstAmount > 0)
// {
// if(this.voucherTypeId == 9 || this.voucherTypeId == 10 || this.voucherTypeId == 1)
// {
//   BillTitle = 'Tax Invoice';
// }
// }
// else if(this.voucherTypeId == 9 || this.voucherTypeId == 10 || this.voucherTypeId == 11 && (this.dynamicArray[0].CgstAmount == 0 && this.dynamicArray[0].IgstAmount == 0) )
// {
//   BillTitle = 'Bill of Supply';
// }
// else if(this.voucherTypeId == 12)
// {
//   BillTitle = 'Export Invoice';
// }
// else if(this.voucherTypeId == 6 || this.voucherTypeId == 8)
// {
//   BillTitle = 'Credit Note';
// }
// else if(this.voucherTypeId == 14 ||this.voucherTypeId == 15)
// {
//   BillTitle = 'Debit Note';
// }
// else if(this.voucherTypeId == 16)
// {
//   BillTitle = 'Performa Invoice';
// }



// if(this.Type == '5')
// {
//   HtmlBody +='        <table style="width:100%;"><tbody><tr><td style="text-align:center;width:33.33%;border-left: 0px solid black;"><h5 style="margin: 0%;">'+ BillTitle +'</h5>';
//   HtmlBody +='                    </td>';
//     HtmlBody +='                    <td style="    border-right: 0px solid black;text-align:center;width:33.33%;">   ';
//   HtmlBody +='				<h5 style="margin: 0%;">'+ InvoiceType +'</h5>';
//   HtmlBody +='                    </td>';
//   HtmlBody +='                </tr>';
//   HtmlBody +='            </tbody>';
//   HtmlBody +='        </table>';
// }
// else
// {
//   HtmlBody +='        <table style="width:100%;"><tbody><tr><td style="text-align:center;width:33.33%;border-left: 0px solid black;"><h5 style="margin: 0%;">'+ BillTitle +'</h5>';
//   HtmlBody +='                    </td>';
//   HtmlBody +='		    <td style="text-align:center;width:33.33%;">';
//   HtmlBody +='                         <h5 style="margin: 0%;">PO Number:'+ this.formGroup4.value.poNo +'</h5>';
//   HtmlBody +='                    </td>';
//   HtmlBody +='                    <td style="    border-right: 0px solid black;text-align:center;width:33.33%;">   ';
//   HtmlBody +='				<h5 style="margin: 0%;">'+ InvoiceType +'</h5>';
//   HtmlBody +='                    </td>';
//   HtmlBody +='                </tr>';
//   HtmlBody +='            </tbody>';
//   HtmlBody +='        </table>';
// }




// HtmlBody +=' <table style="width:100%;">';
// HtmlBody +='            <tbody>';
// HtmlBody +='                <tr>';
// HtmlBody +='                    <td style="width:50%;border-left: 0px solid black;border-top: 0px solid black;">';
// HtmlBody +='                         <span id="txtCustName" style="">Reverse Charge : </span><span style="font-weight:bold;">  NO</span>';

// if(this.Type != '5')
// {
// HtmlBody +='                         <span id="txtCustAddress" style="font-weight: bold; float:right;">E-WAY NO : <br /> '+ this.formGroup4.value.eWayBill +'</span>';
// }

// HtmlBody +='                         <br /><span id="txtArea" >Invoice No :</span><span style="font-weight: bold;"> '+ "0" +'</span><br />';
// // HtmlBody +='                         <span id="txtDistrict" > Invoice Date :</span><span style="font-weight: bold;"> '+ this.dynamicArray[0]['TranctDate'] +'</span><br />';
// HtmlBody +='                         <span id="txtState" >State</span> : <span style="font-weight: bold;"> Karnataka</span>';
// HtmlBody +='                         <span style="float:right;"> <span id="PCustomerGSTNo" >State Code : </span><span style="font-weight:bold;"> 29 </span></span>';
// HtmlBody +='                    </td>';

// HtmlBody +='                    <td   style="border-left: 0px solid black;border-top: 0px solid black;border-right: 0px solid black;">   ';

// if(this.Type == '5' || this.Type == '7' )
// {
//   HtmlBody +='';
// }
// else
// {
// // HtmlBody +='                         <span id="Span1" >Transport Mode :</span><span style="font-weight: bold;"> By Road </span> ' + (this.dynamicArray[0]['paymentterms'] != '' ? ' <span> Payment terms </span> <span style="font-weight:bold;"> '+ (this.dynamicArray[0]['paymentterms'] )  +'</span> ' : '' ) + ' <br />';
// // HtmlBody +='                         <span id="Span2" >Vehicle NO :</span><span style="font-weight: bold;">  '+ this.formGroup4.value.LorryNo +'</span>' + (this.dynamicArray[0]['deliveryterms'] != '' ? ' <span> Delivery terms </span> <span style="font-weight:bold;"> '+ (this.dynamicArray[0]['deliveryterms'] )  +'</span> ' : '' ) + ' <br />';
// // HtmlBody +='                         <span id="Span3" >Date of Supply :</span><span style="font-weight: bold;"> '+ this.dynamicArray[0]['TranctDate'] +'</span><br />';
// HtmlBody +='                         <span id="Span4" ">Place of Supply :</span><span style="font-weight: bold;"> '+ this.formGroup4.value.place +'</span><br />';
// }


// HtmlBody +='                    </td>';
// HtmlBody +='                </tr>';
// HtmlBody +='            </tbody>';
// HtmlBody +='        </table>';

// HtmlBody +='<table style="width:100%;">';
// HtmlBody +='            <tbody>';
// HtmlBody +='                <tr>';
// HtmlBody +='                    <td style="width:50%;    vertical-align: initial;   border-bottom: 0px solid black; border-left: 0px solid black;border-top: 0px solid black;border-right: 0px solid black;">';
// HtmlBody +='                         <span id="Span5" ><span style="font-weight:bold; font-size: 12px;">Name and Address of Receiver / Billed To : </span> <br /><span style="font-size:14px;font-weight:bold; color:black;">'+ this.LedgerName +'</span> <br/>'+ this.formGroup4.value.address1 +'<br/> '+ this.formGroup4.value.address1 +'<br/> '+ "-PLACE-" +'</span><br />';
// // HtmlBody +='                         <span id="Span6" >GSTIN :</span> <span style="font-weight:bold; color:'+ this.InvoiceSettingsData[0].['partyGSTINColumn'] +';">'+ this.dynamicArray[0]['GST'] +'</span>';
// // if(this.dynamicArray[0]['ledgerfssai'] != '')
// // {
// // HtmlBody +='                         <span style="float:right;"><span id="Span8" >FSSAI : </span><span style="font-weight:bold;"> '+ this.dynamicArray[0]['ledgerfssai'] +'</span></span>';
// // }
// // HtmlBody +='                       <br> <span id="Span7" >State : </span><span style="font-weight:bold;"> '+ this.defaultState +'-'+ "STATE CODE" +'</span>';
// // HtmlBody +='                         <span style="float:right;"><span id="Span8" >Contact No : </span><span style="font-weight:bold;"> '+ this.dynamicArray[0]['ledgerCELLNO'] +'</span></span>';
// // HtmlBody +='                    </td>';

// HtmlBody +='                    <td  style="  border-bottom: 0px solid black;  border-top: 0px solid black;border-right: 0px solid black;    vertical-align: initial;">   ';

// if(this.Type == '5' || this.Type == '7')
// {
//   HtmlBody +='';

// }
// else
// {
//   HtmlBody +='                         <span id="Span9" ><span style="font-size: 12px;font-weight:bold;">Details of Consingee / Shipped To : </span> <br /><span style="font-size: 14px;font-weight:bold;">'+ this.formGroup4.value.partyName +'</span><br/>'+ this.formGroup4.value.address1  +'<br/> '+ this.formGroup4.value.address2 +'<br/> '+ this.formGroup4.value.place +'</span><br />';
//   HtmlBody +='                        <br> <span id="Span10">'+ this.formGroup4.value.state  +'</span>';
//   HtmlBody +='                         <span style="float:right;"> <span id="Span11" >State Code :</span><span style="font-weight:bold;">  '+ this.formGroup4.value.stateCode +'</span></span>';
// }


// HtmlBody +='                    </td>';
// HtmlBody +='                </tr>';
// HtmlBody +='            </tbody>';
// HtmlBody +='        </table>';

// HtmlBody +='        <table >';
// HtmlBody +='            <thead>';
// HtmlBody +='              <tr>';
// HtmlBody +='                <th style="border-left: 0px solid black;width:5%;font-weight: bold;font-size: 12px;text-align:center;">SL No.</th>';


// if(this.Type == '6')
// {
// HtmlBody +='                <th style="width:35%;font-weight: bold;font-size: 12px;text-align:left;">Item Description</th>';
// }
// else
// {
// HtmlBody +='                <th style="width:25%;font-weight: bold;font-size: 12px;text-align:left;">Item Description</th>';
// }

// HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">HSN Code</th>';

// if(this.Type == '5')
// {
//   HtmlBody +='';
// }
// else if(this.Type == '6' || this.Type == '7')
// {
//   HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">Qty</th>';
//   HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">Rate</th>';
// }
// else
// {
// HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">UOM</th>';
// HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">No of Bags</th>';
// HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">Weight Per Bag</th>';

// HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">Weight (Qntl-Kg)</th>';
// HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">Rate Per Quintal</th>';
// }

// HtmlBody +='                <th style="border-right: 0px solid black;width:10%;font-weight: bold;font-size: 12px;text-align:center;">Amount</th>';
// HtmlBody +='              </tr>';
// HtmlBody +='            </thead>';
// HtmlBody +='            <tbody id="PrintTBody">';

// var p = 0;
// for(p=0;p<this.dynamicArray.length; p++)
// {



//   HtmlBody +='            <tr>';
//   HtmlBody +='            <td style="border-left: 0px solid black;">'+ (p + 1) +'</td>';
//   HtmlBody +='            <td style="overflow-wrap: anywhere;">'+ this.dynamicArray[p].CommodityName + ' ' + this.dynamicArray[p].Remark +'</td>';
//   // HtmlBody +='            <td style="text-align:center;">'+ this.dynamicArray[p].hsn['hsn'] +'</td>';

//   if(this.Type == '5')
//   {
//     HtmlBody +='';
//   }
//   else if(this.Type == '6' || this.Type == '7')
//   {
//     // HtmlBody +='            <td style="text-align:center;">'+ this.dynamicArray[p]['lineItemQty'] +'</td>';
//     HtmlBody +='            <td style="text-align:right;">'+ this.SeperateComma(parseFloat(this.dynamicArray[p].Rate.toString()).toFixed(2)) +'</td>';
//   }
//   else
//   {
//   HtmlBody +='            <td style="text-align:center;">'+ this.dynamicArray[p].MOU +'</td>';
//   HtmlBody +='            <td style="text-align:right;">'+ this.dynamicArray[p].NoOfBags +'</td>';

//   if(parseFloat(this.dynamicArray[p].WeightPerBag.toString()) > 0)
//   {
//   HtmlBody +='            <td style="text-align:right;">'+ this.dynamicArray[p].NoOfBags +'</td>';
//   }
//   else
//   {
//     HtmlBody +='            <td style="text-align:right;"></td>';
//   }
//   HtmlBody +='            <td style="text-align:right;">'+ this.dynamicArray[p].NoOfBags +'</td>';
//   HtmlBody +='            <td style="text-align:right;">'+ this.SeperateComma(parseFloat(this.dynamicArray[p].Rate.toString()).toFixed(2)) +'</td>';
// }

//   HtmlBody +='            <td style="border-right: 0px solid black;text-align:right;">'+ this.SeperateComma(parseFloat(this.dynamicArray[p].TotalWeight.toString()).toFixed(2)) +'</td>';
//   HtmlBody +='            </tr>';


//   }



// //////// Blank Rows Start /////////


// var BlankRowsCount = 10 - this.dynamicArray.length;

// var u = 0;
// for(u=0;u<BlankRowsCount; u++)
// {
//   HtmlBody +='            <tr>';
//   HtmlBody +='            <td style="border-left: 0px solid black;padding:1.06%;border: 0px;"></td>';
//   HtmlBody +='            <td style="overflow-wrap: anywhere;border: 0px;"></td>';
//   HtmlBody +='            <td style="text-align:center;border: 0px;"></td>';

//   if(this.Type == '5')
//   {
//     HtmlBody +='';
//   }
//   else
//   {

//     if(this.Type == '6' || this.Type == '7')
//     {
//   HtmlBody +='            <td style="text-align:center;border: 0px;"></td>';
//   HtmlBody +='            <td style="text-align:right;border: 0px;"></td>';

//     }
// else
// {
//   HtmlBody +='            <td style="text-align:center;border: 0px;"></td>';
//   HtmlBody +='            <td style="text-align:right;border: 0px;"></td>';

//   if(parseFloat(this.dynamicArray[0].WeightPerBag.toString()) > 0)
//   {
//   HtmlBody +='            <td style="text-align:right;border: 0px;"></td>';
//   }
//   else
//   {
//     HtmlBody +='            <td style="text-align:right;border: 0px;"></td>';
//   }
//   HtmlBody +='            <td style="text-align:center;border: 0px;"></td>';
//   HtmlBody +='            <td style="text-align:right;border: 0px;"></td>';
// }
//   }
//   HtmlBody +='            <td style="border-right: 0px solid black;text-align:right;border: 0px;"></td>';
//   HtmlBody +='            </tr>';

// }


// //////// Blank Rows End //////////











//   HtmlBody +='            <tr style="background: gainsboro;font-weight: bold;">';
//   HtmlBody +='            <td style="border-left: 0px solid black;"></td>';
//   HtmlBody +='            <td></td>';
//   HtmlBody +='            <td style="text-align:center;"></td>';

//   if(this.Type == '5')
// {
//   HtmlBody +='';
// }
// else
// {

//   if(this.Type=='6' || this.Type == '7')
//   {
//     HtmlBody +='            <td style="text-align:center;">'+ this.dynamicArray[0].TotalWeight +'</td>';
//     HtmlBody +='            <td style="text-align:center;"></td>';
//   }
//   else
//   {




//   HtmlBody +='            <td style="text-align:center;"></td>';
//   HtmlBody +='            <td style="text-align:right;">'+ this.dynamicArray[0].NoOfBags +'</td>';


//   HtmlBody +='            <td style="text-align:right;"></td>';

//   HtmlBody +='            <td style="text-align:right;padding:0%;font-size:12px;">'+ this.dynamicArray[0].TotalWeight +'</td>';
//   HtmlBody +='            <td style="text-align:right;"></td>';
// }
// }
//   HtmlBody +='            <td style="border-right: 0px solid black;text-align:right;">'+ this.SeperateComma(parseFloat(this.dynamicArray[0].Amount).toFixed(2)) +'</td>';
//   HtmlBody +='            </tr>';


// HtmlBody +='            </tbody>';
// HtmlBody +='        </table>';



// HtmlBody +='        <table style="border-top: 0px solid white;border-bottom: 0px solid white;">';
// HtmlBody +='                    <tr>';
// HtmlBody +='                        <td style="border-left: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;width:40%;">';

// if(this.Type == '5' || this.Type == '7')
// {
//   HtmlBody +='';
// }
// else
// {
// HtmlBody +='                            <div style="font-weight:bold;">Lorry Details :</div>';
// HtmlBody +='                            <div>Lorry No: '+ this.formGroup4.value.LorryNo +'</div>';
// HtmlBody +='                            <div>Lorry Owner : '+ this.formGroup4.value.owner +'</div>';
// HtmlBody +='                            <div>Lorry Driver : '+ this.formGroup4.value.driver +'</div>';
// HtmlBody +='                            <div>DL No : '+ this.formGroup4.value.dlNo +'</div>';
// HtmlBody +='                            <div>Transporter : '+ this.formGroup4.value.transporter +'</div>';
// HtmlBody +='                            <br />';
// HtmlBody +='				<div style="font-weight:bold;">Freight Details :</div>';
// HtmlBody +='                            <div>Freight/Bag : <span id="PCompanyPanno">'+ this.formGroup4.value.frieght_bag  +'</span></div>';
// HtmlBody +='                            <div>Total Freight : <span id="PGSTNO">'+ this.SeperateComma(parseFloat(this.formGroup4.value.totalFrieght).toFixed(2)) +'</span></div>';
// HtmlBody +='                            <div>Advance Paid : <span id="Span12">'+ this.SeperateComma(parseFloat(this.formGroup4.value.advancePaid).toFixed(2)) +'</span></div>';
// HtmlBody +='                            <div>Freight Payable : <span id="Span13">'+ this.SeperateComma(parseFloat(this.formGroup4.value.balanceFrieght).toFixed(2)) +'</span></div>';
// }
// HtmlBody +='                        </td>';





// if(this.Type == '6')
// {
// HtmlBody +='                        <td style="border-left: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;width: 22.5%;">';
// }
// else
// {
// HtmlBody +='                        <td style="border-left: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;width:20%;">';
// }



// HtmlBody +='                            <div>Seller PAN: '+ this.PANNumber +'</div>';
// // HtmlBody +='                            <div>Buyer PAN : '+ this.dynamicArray[0]['ledgerPAN'] +'</div><br>';

// debugger;


// if(this.Type == '1')
// {
//   HtmlBody +='                            <div>Dispatched From : '+ this.formGroup3.value.FromPlace +'</div>';
//   HtmlBody +='                            <div>Dispatched To : '+ this.formGroup3.value.ToPlace +'</div>';
// }

// if(this.Type == '2')
// {
//   HtmlBody +='                            <div>Dispatched From : '+ this.formGroup3.value.FromPlace +'</div>';
//   HtmlBody +='                            <div>Dispatched To : '+ this.formGroup3.value.ToPlace +'</div>';
// }

// if(this.Type == '3')
// {
//   HtmlBody +='                            <div>Dispatched From : '+ this.formGroup3.value.FromPlace +'</div>';
//   HtmlBody +='                            <div>Dispatched To : '+ this.formGroup3.value.ToPlace +'</div>';
// }

// if(this.Type == '4')
// {
//   HtmlBody +='                            <div>Dispatched From : '+ this.formGroup3.value.FromPlace +'</div>';
//   HtmlBody +='                            <div>Dispatched To : '+ this.formGroup3.value.ToPlace +'</div>';
// }

// if(this.Type == '6')
// {
//   HtmlBody +='                            <div>Dispatched From : '+ this.formGroup3.value.FromPlace +'</div>';
//   HtmlBody +='                            <div>Dispatched To : '+ this.formGroup3.value.ToPlace +'</div>';
// }





// HtmlBody +='                        </td>';






// HtmlBody +='                      <td style="border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;width:20%;">';





// debugger;
// if(this.Type == '1')
// {


//   HtmlBody +='        <div>'+ this.formGroup4.value.frieght_bag +'</div>';

//   if((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value)) > 0)
// {
// HtmlBody +='        <div>'+ this.ExpenseName1 +'</div>';
// }
// if((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value)) > 0){
// HtmlBody +='				<div>'+ this.ExpenseName2 +'</div>';
// }
// if((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value)) > 0){
// HtmlBody +='				<div>'+ this.ExpenseName3 +'</div>';
// }



// HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

// if(this.formGroup4.value.stateCode == '29')
// {
// HtmlBody +='                            <div>'+ this.dynamicArray[0].SgstAmount +'</div>';
// HtmlBody +='				<div>'+ this.dynamicArray[0].CgstAmount +'</div>';
// }
// else
// {
// HtmlBody +='				<div>'+ this.dynamicArray[0].IgstAmount +'</div>';
// }










// // if(parseFloat(this.dynamicArray[0]['tcsValue']) > 0)
// // {
// // HtmlBody +='				<div>'+ this.dynamicArray[0]['tcsLabel'] +'</div>';
// // }

// HtmlBody +='				<div>Round Off</div>';






// HtmlBody +='                        </td>';
// HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;text-align:right">';


// if(parseFloat(this.formGroup4.value.totalFrieght) > 0)
// {
// HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.formGroup4.value.totalFrieght).toFixed(2)) +'</div>';
// }

// if((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value)) > 0)
// {
// HtmlBody +='                           <div>'+ this.SeperateComma((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value)).toFixed(2)) +'</div>';
// }
// if((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value)) > 0){
// HtmlBody +='                           <div>'+ this.SeperateComma((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value))) +'</div>';
// }
// if((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value)) > 0){
// HtmlBody +='                           <div>'+ this.SeperateComma((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value))) +'</div>';
// }


// // HtmlBody +='                           <div style="font-weight:bold;color:'+ this.InvoiceSettingsData[0]['taxableValueColumn'] +';">'+ this.SeperateComma(parseFloat(this.dynamicArray[0]['TaxableValue']).toFixed(2)) +'</div>';


// if(this.formGroup4.value.stateCode == '29')
// {
// HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].SgstAmount.toString()).toFixed(2)) +'</div>';
// HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].CgstAmount.toString()).toFixed(2)) +'</div>';
// }
// else
// {
// HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].IgstAmount.toString()).toFixed(2)) +'</div>';
// }



// // if(parseFloat(this.dynamicArray[0]['tcsValue']) > 0)
// // {
// // HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0]['tcsValue']).toFixed(2)) +'</div>';
// // }

// // HtmlBody +='				<div>'+ this.dynamicArray[0]['RoundOff'].toFixed(2) +'</div>';






// }


// debugger;
// if(this.Type == '7')
// {


//   HtmlBody +='        <div>'+ this.formGroup4.value.frieght_bag +'</div>';

//   if((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value)) > 0)
// {
// HtmlBody +='        <div>'+ this.ExpenseName1 +'</div>';
// }
// if((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value)) > 0){
// HtmlBody +='				<div>'+ this.ExpenseName2 +'</div>';
// }
// if((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value)) > 0){
// HtmlBody +='				<div>'+ this.ExpenseName3 +'</div>';
// }



// HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

// if(this.formGroup4.value.stateCode == '29')
// {
// HtmlBody +='                            <div>'+ this.dynamicArray[0].SgstAmount +'</div>';
// HtmlBody +='				<div>'+ this.dynamicArray[0].CgstAmount +'</div>';
// }
// else
// {
// HtmlBody +='				<div>'+ this.dynamicArray[0].IgstAmount +'</div>';
// }










// // if(parseFloat(this.dynamicArray[0]['tcsValue']) > 0)
// // {
// // HtmlBody +='				<div>'+ this.dynamicArray[0]['tcsLabel'] +'</div>';
// // }

// HtmlBody +='				<div>Round Off</div>';






// HtmlBody +='                        </td>';
// HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;text-align:right">';


// if(parseFloat(this.formGroup4.value.totalFrieght) > 0)
// {
// HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.formGroup4.value.totalFrieght).toFixed(2)) +'</div>';
// }

// if((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value)) > 0)
// {
// HtmlBody +='                           <div>'+ this.SeperateComma((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value))) +'</div>';
// }
// if((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value)) > 0){
// HtmlBody +='                           <div>'+ this.SeperateComma((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value))) +'</div>';
// }
// if((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value)) > 0){
// HtmlBody +='                           <div>'+ this.SeperateComma((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value))).toFixed(2) +'</div>';
// }


// // HtmlBody +='                           <div style="font-weight:bold;color:'+ this.InvoiceSettingsData[0]['taxableValueColumn'] +';">'+ this.SeperateComma(parseFloat(this.dynamicArray[0]['TaxableValue']).toFixed(2)) +'</div>';


// if(this.formGroup4.value.stateCode == '29')
// {
// HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].SgstAmount.toString()).toFixed(2)) +'</div>';
// HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].CgstAmount.toString()).toFixed(2)) +'</div>';
// }
// else
// {
// HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].IgstAmount.toString()).toFixed(2)) +'</div>';
// }



// // if(parseFloat(this.dynamicArray[0]['tcsValue']) > 0)
// // {
// // HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0]['tcsValue']).toFixed(2)) +'</div>';
// // }

// // HtmlBody +='				<div>'+ this.dynamicArray[0]['RoundOff'].toFixed(2) +'</div>';






// }


// debugger;
// if(this.Type == '6')
// {


//   HtmlBody +='        <div>'+ this.formGroup4.value.frieght_bag +'</div>';

//   if((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value)) > 0)
// {
// HtmlBody +='        <div>'+ (this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value)) +'</div>';
// }
// if((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value)) > 0){
// HtmlBody +='				<div>'+ (this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value)) +'</div>';
// }
// if((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value)) > 0){
// HtmlBody +='				<div>'+ (this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value)) +'</div>';
// }



// HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

// if(this.formGroup4.value.stateCode == '29')
// {
// HtmlBody +='                            <div>'+ this.dynamicArray[0].SgstAmount +'</div>';
// HtmlBody +='				<div>'+ this.dynamicArray[0].CgstAmount +'</div>';
// }
// else
// {
// HtmlBody +='				<div>'+ this.dynamicArray[0].IgstAmount +'</div>';
// }










// // if(parseFloat(this.dynamicArray[0]['tcsValue']) > 0)
// // {
// // HtmlBody +='				<div>'+ this.dynamicArray[0]['tcsLabel'] +'</div>';
// // }

// HtmlBody +='				<div>Round Off</div>';






// HtmlBody +='                        </td>';
// HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;text-align:right">';


// if(parseFloat(this.formGroup4.value.totalFrieght) > 0)
// {
// HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.formGroup4.value.totalFrieght).toFixed(2)) +'</div>';
// }

// if(Number((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value))) > 0)
// {
// HtmlBody +='                           <div>'+ this.SeperateComma(Number((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value)))) +'</div>';
// }
// if(Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value))) > 0){
// HtmlBody +='                           <div>'+ this.SeperateComma(Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value)))).toFixed(2) +'</div>';
// }
// if(Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value))) > 0){
// HtmlBody +='                           <div>'+ this.SeperateComma(Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value)))) +'</div>';
// }


// // HtmlBody +='                           <div style="font-weight:bold;color:'+ this.InvoiceSettingsData[0]['taxableValueColumn'] +';">'+ this.SeperateComma(parseFloat(this.dynamicArray[0]['TaxableValue']).toFixed(2)) +'</div>';


// if(this.formGroup4.value.stateCode == '29')
// {
// HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].SgstAmount.toString()).toFixed(2)) +'</div>';
// HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].CgstAmount.toString()).toFixed(2)) +'</div>';
// }
// else
// {
// HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].IgstAmount.toString()).toFixed(2)) +'</div>';
// }



// // if(parseFloat(this.dynamicArray[0]['tcsValue']) > 0)
// // {
// // HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0]['tcsValue']).toFixed(2)) +'</div>';
// // }

// // HtmlBody +='				<div>'+ this.dynamicArray[0]['RoundOff'].toFixed(2) +'</div>';






// }





// debugger;
// if(this.Type == '2')
// {



// if(Number((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value))) > 0)
// {
// HtmlBody +='        <div>'+ this.ExpenseName1 +'</div>';
// }
// if(Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value))) > 0){
// HtmlBody +='				<div>'+ this.ExpenseName2 +'</div>';
// }
// if(Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value))) > 0){
// HtmlBody +='				<div>'+ this.ExpenseName3 +'</div>';
// }



// HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

// if(this.formGroup4.value.stateCode == '29')
// {
// HtmlBody +='                            <div>'+ this.dynamicArray[0].SgstAmount +'</div>';
// HtmlBody +='				<div>'+ this.dynamicArray[0].CgstAmount +'</div>';
// }
// else
// {
// HtmlBody +='				<div>'+ this.dynamicArray[0].IgstAmount +'</div>';
// }









// // if(parseFloat(this.dynamicArray[0]['tcsValue']) > 0)
// // {
// // HtmlBody +='				<div>'+ this.dynamicArray[0]['tcsLabel'] +'</div>';
// // }

// HtmlBody +='				<div>Round Off</div>';


// HtmlBody +='				<div><b>Invoice Amount</b></div>';

// // HtmlBody +='        <div>'+ this.dynamicArray[0]['frieghtLabel'] +'</div>';




// HtmlBody +='                        </td>';
// HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;;text-align:right">';



// if(Number((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value))) > 0)
// {
// HtmlBody +='                           <div>'+ this.SeperateComma(Number((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value)))) +'</div>';
// }
// if(Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value))) > 0){
// HtmlBody +='                           <div>'+ this.SeperateComma(Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value)))).toFixed(2) +'</div>';
// }
// if(Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value))) > 0){
// HtmlBody +='                           <div>'+ this.SeperateComma(Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value)))) +'</div>';
// }


// // HtmlBody +='                           <div style="font-weight:bold;color:'+ this.InvoiceSettingsData[0]['taxableValueColumn'] +';">'+ this.SeperateComma(parseFloat(this.dynamicArray[0]['TaxableValue']).toFixed(2)) +'</div>';


// if(this.formGroup4.value.stateCode == '29')
// {
// HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].SgstAmount.toString()).toFixed(2)) +'</div>';
// HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].CgstAmount.toString()).toFixed(2)) +'</div>';
// }
// else
// {
// HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].IgstAmount.toString()).toFixed(2)) +'</div>';
// }








// // if(parseFloat(this.dynamicArray[0]['tcsValue']) > 0)
// // {
// // HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0]['tcsValue']).toFixed(2)) +'</div>';
// // }

// // HtmlBody +='				<div>'+ this.dynamicArray[0]['RoundOff'].toFixed(2) +'</div>';



// debugger;
// // HtmlBody +='				<div><b>'+ this.SeperateComma((parseFloat(this.dynamicArray[0]['TaxableValue']) + parseFloat(this.dynamicArray[0].IgstAmount.toString()) + parseFloat(this.dynamicArray[0].SgstAmount.toString()) + parseFloat(this.dynamicArray[0].CgstAmount.toString()) + parseFloat(this.dynamicArray[0]['tcsValue']) + parseFloat(this.dynamicArray[0]['RoundOff'])).toFixed(2)) +'</b></div>';
// HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.formGroup4.value.totalFrieght).toFixed(2)) +'</div>';

// }



// debugger;
// if(this.Type == '3')
// {



// HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

// if(this.formGroup4.value.stateCode == '29')
// {
// HtmlBody +='                            <div>'+ this.dynamicArray[0].SgstAmount +'</div>';
// HtmlBody +='				<div>'+ this.dynamicArray[0].CgstAmount +'</div>';
// }
// else
// {
// HtmlBody +='				<div>'+ this.dynamicArray[0].IgstAmount +'</div>';
// }


// // HtmlBody +='        <div>'+ this.dynamicArray[0]['frieghtLabel'] +'</div>';





// if(Number((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value))) > 0)
// {
// HtmlBody +='        <div>'+ this.ExpenseName1 +'</div>';
// }
// if(Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value))) > 0){
// HtmlBody +='				<div>'+ this.ExpenseName2 +'</div>';
// }
// if(Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value))) > 0){
// HtmlBody +='				<div>'+ this.ExpenseName3 +'</div>';
// }

// // if(parseFloat(this.dynamicArray[0]['tcsValue']) > 0)
// // {
// // HtmlBody +='				<div>'+ this.dynamicArray[0]['tcsLabel'] +'</div>';
// // }

// HtmlBody +='				<div>Round Off</div>';




// HtmlBody +='                        </td>';
// HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;text-align:right">';

// // HtmlBody +='                           <div style="font-weight:bold;color:'+ this.InvoiceSettingsData[0]['taxableValueColumn'] +';">'+ this.SeperateComma(parseFloat(this.dynamicArray[0]['TaxableValue']).toFixed(2)) +'</div>';


// if(this.formGroup4.value.stateCode == '29')
// {
// HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].SgstAmount.toString()).toFixed(2)) +'</div>';
// HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].CgstAmount.toString()).toFixed(2)) +'</div>';
// }
// else
// {
// HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].IgstAmount.toString()).toFixed(2)) +'</div>';
// }


// if(parseFloat(this.formGroup4.value.totalFrieght)> 0)
// {
// HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.formGroup4.value.totalFrieght).toFixed(2)) +'</div>';
// }

// if(Number((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value))) > 0)
// {
// HtmlBody +='                           <div>'+ this.SeperateComma(Number((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value)))) +'</div>';
// }
// if(Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value))) > 0){
// HtmlBody +='                           <div>'+ this.SeperateComma(Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value)))).toFixed(2) +'</div>';
// }
// if(Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value))) > 0){
// HtmlBody +='                           <div>'+ this.SeperateComma(Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value)))) +'</div>';
// }



// // if(parseFloat(this.dynamicArray[0]['tcsValue']) > 0)
// // {
// // HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0]['tcsValue']).toFixed(2)) +'</div>';
// // }

// // HtmlBody +='				<div>'+ this.dynamicArray[0]['RoundOff'].toFixed(2) +'</div>';






// }











// if(this.Type == '4')
// {



// HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

// if(this.formGroup4.value.stateCode == '29')
// {
// HtmlBody +='                            <div>'+ this.dynamicArray[0].SgstAmount +'</div>';
// HtmlBody +='				<div>'+ this.dynamicArray[0].CgstAmount +'</div>';
// }
// else
// {
// HtmlBody +='				<div>'+ this.dynamicArray[0].IgstAmount +'</div>';
// }







// if(Number((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value))) > 0)
// {
// HtmlBody +='        <div>'+ this.ExpenseName1 +'</div>';
// }
// if(Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value))) > 0){
// HtmlBody +='				<div>'+ this.ExpenseName2 +'</div>';
// }
// if(Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value))) > 0){
// HtmlBody +='				<div>'+ this.ExpenseName3 +'</div>';
// }

// // if(parseFloat(this.dynamicArray[0]['tcsValue']) > 0)
// // {
// // HtmlBody +='				<div>'+ this.dynamicArray[0]['tcsLabel'] +'</div>';
// // }

// HtmlBody +='				<div>Round Off</div>';

// HtmlBody +='				<div style="font-weight: bold;">Invoice Amount</div>';


// //   HtmlBody +='        <div>'+ this.dynamicArray[0]['frieghtLabel'] +'</div>';

// HtmlBody +='                        </td>';
// HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;width:10%;text-align:right">';

// // HtmlBody +='                           <div style="font-weight:bold;color:'+ this.InvoiceSettingsData[0]['taxableValueColumn'] +';">'+ this.SeperateComma(parseFloat(this.dynamicArray[0]['TaxableValue']).toFixed(2)) +'</div>';


// if(this.formGroup4.value.stateCode == '29')
// {
// HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].SgstAmount.toString()).toFixed(2)) +'</div>';
// HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].CgstAmount.toString()).toFixed(2)) +'</div>';
// }
// else
// {
// HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].IgstAmount.toString()).toFixed(2)) +'</div>';
// }




// if(Number((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value))) > 0)
// {
// HtmlBody +='                           <div>'+ this.SeperateComma(Number((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value)))) +'</div>';
// }
// if(Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value))) > 0){
// HtmlBody +='                           <div>'+ this.SeperateComma(Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value)))).toFixed(2) +'</div>';
// }
// if(Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value))) > 0){
// HtmlBody +='                           <div>'+ this.SeperateComma(Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value)))) +'</div>';
// }


// // if(parseFloat(this.dynamicArray[0]['tcsValue']) > 0)
// // {
// // HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0]['tcsValue']).toFixed(2)) +'</div>';
// // }

// // HtmlBody +='				<div>'+ this.dynamicArray[0]['RoundOff'].toFixed(2) +'</div>';



// var GrandTotal = 0;

// // GrandTotal = parseFloat(this.dynamicArray[0]['TaxableValue']) + parseFloat(this.dynamicArray[0].SgstAmount.toString()) + parseFloat(this.dynamicArray[0].CgstAmount.toString()) + parseFloat(this.dynamicArray[0].IgstAmount.toString())  + parseFloat(this.dynamicArray[0]['ExpenseAmount1']) + Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value))) + Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value))) + parseFloat(this.dynamicArray[0]['tcsValue']);


// HtmlBody +='				<div style="font-weight: bold;">₹'+ this.SeperateComma(GrandTotal.toFixed(2)) +'</div>';


// // HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.formGroup4.value.totalFrieght).toFixed(2)) +'</div>';
//   }

// debugger;
// if(this.Type == '5')
// {



// HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

// if(this.formGroup4.value.stateCode == '29')
// {
// HtmlBody +='                            <div>'+ this.dynamicArray[0].SgstAmount +'</div>';
// HtmlBody +='				<div>'+ this.dynamicArray[0].CgstAmount +'</div>';
// }
// else
// {
// HtmlBody +='				<div>'+ this.dynamicArray[0].IgstAmount +'</div>';
// }




// if(Number((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value))) > 0)
// {
// HtmlBody +='        <div>'+ this.ExpenseName1 +'</div>';
// }
// if(Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value))) > 0){
// HtmlBody +='				<div>'+ this.ExpenseName2 +'</div>';
// }
// if(Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value))) > 0){
// HtmlBody +='				<div>'+ this.ExpenseName3 +'</div>';
// }

// // if(parseFloat(this.dynamicArray[0]['tcsValue']) > 0)
// // {
// // HtmlBody +='				<div>'+ this.dynamicArray[0]['tcsLabel'] +'</div>';
// // }

// HtmlBody +='				<div>Round Off</div>';






// HtmlBody +='                        </td>';
// HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;text-align:right">';

// // HtmlBody +='                           <div style="font-weight:bold;color:'+ this.InvoiceSettingsData[0]['taxableValueColumn'] +';">'+ this.SeperateComma(parseFloat(this.dynamicArray[0]['TaxableValue']).toFixed(2)) +'</div>';


// if(this.formGroup4.value.stateCode == '29')
// {
// HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].SgstAmount.toString()).toFixed(2)) +'</div>';
// HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].CgstAmount.toString()).toFixed(2)) +'</div>';
// }
// else
// {
// HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0].IgstAmount.toString()).toFixed(2)) +'</div>';
// }




// if(Number((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value))) > 0)
// {
// HtmlBody +='                           <div>'+ this.SeperateComma(Number((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value)))) +'</div>';
// }
// if(Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value))) > 0){
// HtmlBody +='                           <div>'+ this.SeperateComma(Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value)))).toFixed(2)  +'</div>';
// }
// if(Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value))) > 0){
// HtmlBody +='                           <div>'+ this.SeperateComma(Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value)))) +'</div>';
// }

// // debugger;
// // if(parseFloat(this.dynamicArray[0]['tcsValue']) > 0)
// // {
// // HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.dynamicArray[0]['tcsValue']).toFixed(2)) +'</div>';
// // }

// // HtmlBody +='				<div>'+ this.dynamicArray[0]['RoundOff'].toFixed(2) +'</div>';



// var GrandTotal = 0;

// // GrandTotal = parseFloat(this.dynamicArray[0]['TaxableValue']) + parseFloat(this.dynamicArray[0].SgstAmount.toString()) + parseFloat(this.dynamicArray[0].CgstAmount.toString()) + parseFloat(this.dynamicArray[0].IgstAmount.toString())  + parseFloat(this.dynamicArray[0]['ExpenseAmount1']) + Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value))) + Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value))) + parseFloat(this.dynamicArray[0]['tcsValue']);



// HtmlBody +='				<div style="font-weight: bold;">₹'+ this.SeperateComma(GrandTotal.toFixed(2)) +'</div>';


// // HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['frieghtinBill']).toFixed(2)) +'</div>';


// }


// HtmlBody +='                        </td>';



// HtmlBody +='            </tr>';



// HtmlBody +='<tr>';
// // HtmlBody +='<td style="font-weight:bold;border-left: 0px solid black;" colspan="2">Note : '+ this.dynamicArray[0]['note1'] +'</td>';

// HtmlBody +='<td style="font-weight:bold;">Grand Total</td>';
// HtmlBody +='<td style="border-right: 0px solid black;font-weight:bold;text-align:right;color:'+ this.grandTotalAmount +';">₹ '+ this.SeperateComma(parseFloat(this.dynamicArray[0].Amount).toFixed(2)) +'</td>';
// HtmlBody +='</tr>';
// HtmlBody +='        </table>';
// HtmlBody +='  <table style="border-top: 0px solid white;">';
// HtmlBody +='            <tr>';
// HtmlBody +='                <td style="border-top: 0px solid white;    border-left: 0px solid white;border-right: 0px solid white;">';
// // HtmlBody +='                    <span style="font-weight:bold;color:'+ this.InvoiceSettingsData[0]['inwordsColumnn'] +';">'+ this.dynamicArray[0]['inWords'] +'</span>';
// HtmlBody +='                    <span style="font-weight:bold;" id="txtAmount"></span>';

// HtmlBody +='                </td>';
// HtmlBody +='            </tr>';
// HtmlBody +='        </table>';
// HtmlBody +='  <table style="border-top: 0px solid white;">';
// HtmlBody +='<tr>';
// HtmlBody +='<th style="  border-left: 0px solid black;  font-weight: bold;font-size: 12px;text-align:center;">Bank Name</th>';
// HtmlBody +=' <th style="   font-weight: bold;font-size: 12px;text-align:center;">IFSC Code</th>';
// HtmlBody +='<th style=" border-right: 0px solid black;  font-weight: bold;font-size: 12px;text-align:center;">Account No</th>';

// HtmlBody +='</tr>';
// HtmlBody +='           <tr>';
// // HtmlBody +='                <td style="border-left: 0px solid black;border-top: 0px solid white;text-align:center; ">'+ this.dynamicArray[0]['bank1'] +'';

// HtmlBody +='                </td>';
// // HtmlBody +=' <td style="border-top: 0px solid white;text-align:center;">'+ this.dynamicArray[0]['ifsC1'] +'';

// HtmlBody +='                </td>';
// // HtmlBody +=' <td style="text-align:center;border-right: 0px solid black;border-top: 0px solid white;">'+ this.dynamicArray[0]['accountNo1'] +'';

// HtmlBody +='                </td>';
// HtmlBody +='            </tr>';

// HtmlBody +='</tr>';
// //if(this.dynamicArray[0]['bank2'] != '')
// //{
// HtmlBody +='           <tr>';
// // HtmlBody +='                <td style="border-left: 0px solid black;border-top: 0px solid white;text-align:center;">'+ (this.dynamicArray[0]['bank2'] != '' ? this.dynamicArray[0]['bank2'] : '-') +'';

// HtmlBody +='                </td>';
// // HtmlBody +=' <td style="border-top: 0px solid white;text-align:center;">'+ this.dynamicArray[0]['ifsC2'] +'';

// HtmlBody +='                </td>';
// // HtmlBody +=' <td style="border-right: 0px solid black;border-top: 0px solid white;text-align:center;">'+ this.dynamicArray[0]['accountNo2'] +'';

// HtmlBody +='                </td>';
// HtmlBody +='            </tr>';
// //}
// HtmlBody +='</tr>';

// //if(this.dynamicArray[0]['bank3'] != '')
// //{
// HtmlBody +='           <tr>';
// // HtmlBody +='                <td style="border-left: 0px solid black;border-top: 0px solid white;text-align:center;">'+ (this.dynamicArray[0]['bank3'] != '' ? this.dynamicArray[0]['bank3'] : '-') +'';

// HtmlBody +='                </td>';
// // HtmlBody +=' <td style="border-top: 0px solid white;text-align:center;">'+ this.dynamicArray[0]['ifsC3'] +'';

// HtmlBody +='                </td>';
// // HtmlBody +=' <td style="border-right: 0px solid black;border-top: 0px solid white;text-align:center;">'+ this.dynamicArray[0]['account3'] +'';

// HtmlBody +='                </td>';
// HtmlBody +='            </tr>';
// //}

// HtmlBody +='        </table>';
// HtmlBody +=' <table style="border-top: 0px solid white;">';
// HtmlBody +='            <tr>';
// HtmlBody +='                <td style="    border-left: 0px solid black;border-right: 0px solid black;border-top: 0px solid white;">';
// HtmlBody +='                    <span style="">Terms of Business :</span>';
// HtmlBody +='                    <span style="font-weight:bold;">1) Interest from date of purchase will be charged @ 2% per month.</span> <br />';
// // HtmlBody +='                    <span style="font-weight:bold;margin-left:116px;">'+ this.dynamicArray[0]['jurisLine'] +'</span><br />';

// // if(this.ClientCode != 'AVS')
// // {
// //   if(this.Type != '7')
// //   {
// // HtmlBody +='                    <span style="font-weight:bold;margin-left:116px;">3) We are not reponsible for any loss in weight or damage during transit.</span>';
// //   }
// // }
// HtmlBody +='                </td>';
// HtmlBody +='            </tr>';
// HtmlBody +='        </table>';

// HtmlBody +=' <table style="border-top: 0px solid white;width:100%;">';
// HtmlBody +='            <tr>';
// HtmlBody +='<td style=" width:20%;   border-left: 0px solid black;border-top: 0px solid white;border-right: 0px solid white;text-align:left;">';

// if(this.Type != '7')
// {
// HtmlBody +='<span> Driver/Transporters</span><br><br><br>';
// HtmlBody +='<span style="">Authorised Signature</span>';
// }

// HtmlBody +='</td>';
// HtmlBody +='<td style="width:20%; border-top: 0px solid white;border-left: 0px solid white;border-right: 0px solid white;text-align:right;">';

// if(this.Type != '7')
// {
// HtmlBody +='<span></span><br><br><br>';
// HtmlBody +='<span>Common Seal</span>';
// }
// HtmlBody +='</td>';
// HtmlBody +='<td style=" width:60%;   border-right: 0px solid black;border-top: 0px solid white;border-left: 0px solid white;">';
// HtmlBody +='<h6 style="text-align:right;">Certified that the particulars given above are true and correct</h6>';
// HtmlBody +='<span></span><br>';
// HtmlBody +='<span style="margin-left:55%;">Authorised Signature</span>';
// HtmlBody +='</td>';
// HtmlBody +='            </tr>';
// HtmlBody +='        </table>';



// this.CompleteInvoice += ' <div   style="border: 1px solid black;border-bottom: 0px solid white;page-break-after: always;">' +HtmlBody + '</div>';

// if(InvoiceType == 'Customer Copy')
// {
//     this.CustomerCopy = '<html><head> <style> body {font-family: Tahoma, Verdana, Segoe, sans-serif;}  td, th {        border: 1px solid black;    text-align: left;font-size: 13px;    }table {    border-collapse: collapse;    width: 100%;}th, td {    padding: 2px;}th {-webkit-print-color-adjust: exact;background-color: gainsboro;} </style></head><body style="font-size: 12px;"> <div   style="border: 1px solid black;border-bottom: 0px solid white;">' +HtmlBody + '</div></body></html>';
//   }
// else if(InvoiceType == 'Transporter Copy')
// {
//   this.TransporterCopy = '<html><head> <style>body {font-family: Tahoma, Verdana, Segoe, sans-serif;} td, th {        border: 1px solid black;    text-align: left;font-size: 13px;    }table {    border-collapse: collapse;    width: 100%;}th, td {    padding: 2px;}th {-webkit-print-color-adjust: exact;background-color: gainsboro;} </style></head><body style="font-size: 12px;"> <div   style="border: 1px solid black;border-bottom: 0px solid white;">' +HtmlBody + '</div></body></html>';
// }
// else if(InvoiceType == 'Office Copy')
// {
//   this.OfficeCopy = '<html><head> <style> body {font-family: Tahoma, Verdana, Segoe, sans-serif;} td, th {        border: 1px solid black;    text-align: left;font-size: 13px;    }table {    border-collapse: collapse;    width: 100%;}th, td {    padding: 2px;}th {-webkit-print-color-adjust: exact;background-color: gainsboro;} </style></head><body style="font-size: 12px;"> <div   style="border: 1px solid black;border-bottom: 0px solid white;">' +HtmlBody + '</div></body></html>';
// }



//   }

  openNewTab(data:any) {

    sessionStorage.setItem('HTMLContent', data);

    const url = this.router.createUrlTree(['/'], { fragment: 'GoodsInvoicePrintView' }).toString();
    window.open(url, '_blank');
  }

  // changes ends here

  isValid(valAd:string) : Boolean {
    if(valAd.length > 0) {

    } else {
      return false;
    }
    return true;
  }
  storeTotalAmount:any=0;
  storetotalWeightInString:any=0;
  storetotalBags:any=0;
  IsServiceInvoice : boolean = false;

  showConfirmationDialog(): void {
    Swal.fire({
      title: 'Confirmation',
      text: 'Are you Sure to Create this Invoice ?',
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes',
      cancelButtonText: 'No'
    }).then((result) => {
      if (result.isConfirmed) {
          this.onSave();
      } else {

      }
    });
  }

  onSave(): void {

    if(this.route.snapshot.queryParamMap.get('InvoiceType')?.toString() == 'GinningInvoice')
    {
      this.IsServiceInvoice = true;
    }

    var Flag = true;
    var RowNumber = 0;
    var ErrorMsg = 'Please enter ';
    var e = 0;

    if(this.balanceFr < 0){
      this.toastr.error('Balance Freight should not be greater than Advance Paid.');
      return;
    }

    const valAd11: string = this.getParty.get('dealerType').value;
    if(!this.isValid(valAd11)) {
      this.toastr.error("dealerType is Mandatory");
      this.editDemo();
      return;
    }

    const valAd13: string = this.getParty.get('gst').value;
    if(!this.isValid(valAd13)) {
      this.toastr.error("GSTTIN is Mandatory");
      this.editDemo();
      return;
    }


    const val: string = this.getParty.get('gst').value;
    if(val.length != 15) {
      this.toastr.error("GSTIN number is should of 15 characters");
      this.editDemo();
      return;
    }

    const valPlace: string = this.getParty.get('place').value;
    if(!this.isValid(valPlace)) {
      this.toastr.error("Place is Mandatory");
      this.editDemo();
      return;
    }
    // const valAd: string = this.formGroup4.value.address1;
    const valAd: string = this.getParty.get('address1').value;
    if(!this.isValid(valAd)) {
      this.toastr.error("Address 1 is Mandatory");
      this.editDemo();
      return;
    }


    // const valAd2: string = this.formGroup4.value.address2;
    const valAd2: string = this.getParty.get('address2').value;
    if(!this.isValid(valAd2)) {
      this.toastr.error("Address 2 is Mandatory");
      this.editDemo();
      return;
    }



    var pinVal : string = this.getParty.get('pin')?.value;
    if(pinVal == null)
       pinVal = ""
    if(!this.isValid(pinVal)) {
      this.toastr.error("Pin is Mandatory");
      this.editDemo();
      return;
    }




    const sateVal : string = this.getParty.get('state')?.value;
    if(!this.isValid(sateVal)) {
      this.toastr.error("State is Mandatory");
      this.editDemo();
      return;
    }
    const CountryVal : string = this.getParty.get('country')?.value;
    if(!this.isValid(CountryVal)) {
      this.toastr.error("Country is Mandatory");
      this.editDemo();
      return;
    }
    // console.log(" Checking -> " + this.formGroup4.value.state);
    // const stateVal : string = this.formGroup4.value.state;
    // if(stateVal.length > 0) {

    // } else {
    //   this.toastr.error("State is Mandatory");
    //   return;
    // }

    // if(this.countryCode != null) {
    //   if(this.countryCode.length == 0) {
    //     this.toastr.error("Country Code is Mandatory");
    //     return;
    //   }
    // } else {
    //   this.toastr.error("Country Code is Mandatory");
    //   return;
    // }

    // if(this.PANNumber == null) {
    //   this.toastr.error("PAN Number is Mandatory");
    //   return;
    // } else {
    //   if(this.PANNumber.length == 0) {
    //     this.toastr.error("PAN Number is Mandatory");
    //     return;
    //   }
    // }

    if(this.getParty.get('emailId').value == null) {
      this.toastr.error("Email is Mandatory");
      this.editDemo();
      return;
    } else {
      if(this.getParty.get('emailId').value.length == 0) {
        this.toastr.error("Email is Mandatory");
        this.editDemo();
        return;
      }
    }

    if(this.getParty.get('cellNo').value == null) {
      this.toastr.error("Mobile Number is Mandatory");
      this.editDemo();
      return;
    } else {
      const vl : string = this.getParty.get('cellNo').value;
      if(vl.length != 10) {
        this.toastr.error("Mobile Number should be 10 digits only");
        this.editDemo();
        return;
      }
    }

    var legalNameVal : string = this.getParty.get('legalName')?.value;
    if(legalNameVal == null)
       legalNameVal = "";
    if(!this.isValid(legalNameVal)) {
      this.toastr.error("legalName is Mandatory");
      this.editDemo();
      return;
    }

    const panVal : string = this.getParty.get('pan')?.value;
    if(!this.isValid(panVal)) {
      this.toastr.error("Pan is Mandatory");
      this.editDemo();
      return;
    }
    try {

      for (e = 0; e < this.dynamicArray.length; e++) {

        this.storetotalWeightInString = this.storetotalWeightInString + this.dynamicArray[e].TotalWeight;
        this.storetotalBags = this.storetotalBags + this.dynamicArray[e].NoOfBags;
        this.storeTotalAmount = this.storeTotalAmount + this.dynamicArray[e].Amount;

            if (this.dynamicArray[e].CommodityId == 0 || this.dynamicArray[e].CommodityId.toString() == '' || this.dynamicArray[e].CommodityId.toString() == 'null' || this.dynamicArray[e].CommodityId == null) {
              ErrorMsg += 'Item,';
              Flag = false;
            }

            if (this.dynamicArray[e].TotalWeight == 0 && this.isDebitNote) {
              ErrorMsg += 'Total Qty,';
              Flag = false;
            }

            if (this.dynamicArray[e].NoOfBags == 0 && this.isShow) {
              ErrorMsg += 'No of bags,';
              Flag = false;
            }
            if (Flag == false && this.isShow) {
              ErrorMsg += ' at Row number ' + (e + 1) + ',';
            }
          }

    } catch (e) {

      this.toastr.error("Something went wrong");
      return;

    }

    if (Number((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value))) > 0) {
      if (this.ExpenseName1 == '' || this.ExpenseName1 == 'null' || this.ExpenseName1 == null) {
        ErrorMsg += ' Valid Expense 1 Account';
        Flag = false;
      }
    }


    if (Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value))) > 0) {
      if (this.ExpenseName2 == '' || this.ExpenseName2 == 'null' || this.ExpenseName2 == null) {
        ErrorMsg += ' Valid Expense 2 Account';
        Flag = false;
      }
    }

    if (Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value))) > 0) {
      if (this.ExpenseName3 == '' || this.ExpenseName3 == 'null' || this.ExpenseName3 == null) {
        ErrorMsg += ' Valid Expense 3 Account';
        Flag = false;
      }
    }

    if (Flag == false) {
      this.toastr.error(ErrorMsg.slice(0, -1));
    }

    let currentFinaYr = sessionStorage.getItem('financialYear');

    this.submitted = true;
    if (
      Flag == true &&
      this.getParty.valid &&
      this.formGroup1.valid &&
      this.formGroup2.valid &&
      this.formGroup3.valid
    ) {
      let payload = {
        InvioceData: {
          CompanyId: this.globalCompanyId,
          LedgerId: this.ledgerId,
          CurrentFinanceYear: currentFinaYr,
          advancePaintAmount: this.payLess,
          IsServiceInvoice : this.IsServiceInvoice,
          LedgerName: this.LedgerName,
          OriginalInvDate: this.datePipe.transform(this.getParty.value.BillDate, 'yyyy-MM-dd'),
          VochType: this.voucherTypeId,
          VoucherName: this.getParty.value.voucherType,
          DealerType: this.dealerType,
          PAN: this.PANNumber,
          GST: this.getParty.value.gst,
          InvoiceType: 'SalesGood', //refer all types of invoice
          InoviceNo: this.getParty.value.invoiceNo,
          State: this.defaultState,
          NoOfBags: !isNil(this.totalNoOfBags) && !isEmpty(this.totalNoOfBags) ? this.totalNoOfBags : 0,
          TotalWeight: this.totalQuantity,
          ExpenseName1: this.ExpenseName1,
          ExpenseName2: this.ExpenseName2,
          ExpenseName3: this.ExpenseName3,
          ExpenseAmount1: Number((this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value))),
          ExpenseAmount2: Number((this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value))),
          ExpenseAmount3: Number((this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value))),
          TaxableValue: this.taxableAmt,
          Discount: Number(this.formGroup2.value.discount),
          Sgstvalue: Number(this.formGroup2.value.totalsgstAmt.replaceAll(',','')),
          Cgstvalue: Number(this.formGroup2.value.totalcgstAmt.replaceAll(',','')),
          Igstvalue: Number(this.formGroup2.value.totaligstAmt.replaceAll(',','')),
          IsSez: Number(this.disableRates == true ? 1 : 0),
          RoundOff: Number(this.formGroup2.value.roundOff),
          TotalAmount: Number(this.totalAmount),
          BillAmount: Number(this.grandTotalAmount.toString().replaceAll(',','')),
          Address: this.formGroup3.value.address,
          FromPlace: this.formGroup3.value.FromPlace,
          ToPlace: this.formGroup3.value.ToPlace,
        },
        LorryDetails: {
          PoNumber: this.formGroup4.value.poNo,
          EwaybillNo: this.formGroup4.value.eWayBill,
          Transporter: this.formGroup4.value.transporter,
          LorryNo: this.formGroup4.value.LorryNo,
          Owner: this.formGroup4.value.owner,
          Driver: this.formGroup4.value.driver,
          Dlno: this.formGroup4.value.dlNo,
          CheckPost: this.formGroup4.value.checkPost,
          FrieghtPerBag: Number(this.formGroup4.value.frieght_bag),
          TotalFrieght: Number(this.formGroup4.value.totalFrieght),
          AdvanceFrieght: Number(this.formGroup4.value.advancePaid),
          BalanceFrieght: Number(this.formGroup4.value.balanceFrieght),
          FrieghtPlus_Less: this.formGroup4.value.frieght_Plus_Less,
          TDS: Number(this.formGroup4.value.tds),
          DeliveryName: this.formGroup4.value.partyName,
          DeliveryAddress1: this.formGroup4.value.address1,
          DeliveryAddress2: this.formGroup4.value.address2,
          DeliveryPlace: this.formGroup4.value.place,
          DeliveryPin: this.formGroup4.value.pinCode,
          DeliveryState: this.formGroup4.value.state,
          DeliveryStateCode: this.formGroup4.value.stateCode,
          Distance: Number(this.formGroup4.value.distance),
          Dcnote: this.formGroup4.value.note,

          DispatcherName: this.dispatcherName || ' ',
          DispatcherAddress1: this.dispatcherAddress1 || ' ',
          DispatcherAddress2: this.dispatcherAddress2 || ' ',
          DispatcherPlace: this.dispatcherPlace || ' ',
          DispatcherPIN: this.dispatcherPIN || ' ',
          DispatcherStatecode: this.dispatcherStatecode || ' ',
          CountryCode: this.countryCode || ' ',
          ShipBillNo: this.shipBillNo || ' ',
          ForCur: this.forCur || ' ',
          PortName: this.portName || ' ',
          RefClaim: this.refClaim || ' ',
          ShipBillDate: this.shipBillDate || ' ',
          ExpDuty: this.expDuty || ' '
        },
        // ItemData: this.dynamicArray,
        ItemData: this.formatDynamicArray(this.dynamicArray),
      };

      this.spinner.show();
      this.adminService.addGoodsInvoice(payload).subscribe({
        next: (res: any) => {
          debugger;
          if (res.split('|||||')[0] == 'Added Successfully...!') {
            this.toastr.success('Invoice Created Successfully!');
            this.spinner.hide();
            this.showPrintButton = true;
            debugger;
            this.getCRDRList(res.split('|||||')[1], res.split('|||||')[2]);

            this.invoiceNoToGetReponse = res.split('|||||')[3];

            //this.location.back();

          }

          if (res == 'Duplicate InoviceNo...!') {
            this.toastr.error('Invoice number is already exist!');
            $('#invoiceNo').focus();
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

    if (this.getParty.valid == false) {
      this.editDemo();
    }

  }



  formatDynamicArray(dynamicArray: any) {
    let dy: any = [];

    console.log(dynamicArray);

    dynamicArray.forEach((items: any) => {
      const a = {
        "Sno": items.Sno,
        "CommodityName": items.CommodityName,
        "CommodityId": items.CommodityId,
        "WeightPerBag": items.WeightPerBag,
        "NoOfBags": !isEmpty(items.NoOfBags) ? items.NoOfBags : 0,
        "TotalWeight": items.TotalWeight,
        "Rate": !isEmpty(items.Rate) ? items.Rate : 0,
        "MOU": items.MOU,
        "Taxable": items.Taxable,
        "Amount": items.Amount,
        "Remark": items.Remark,
        "SgstAmount": items.SgstAmount,
        "CgstAmount": items.CgstAmount,
        "IgstAmount": items.IgstAmount,
        "NoOfDocra": items.NoOfDocra,
        "SgstRate": items.SgstRate,
        "CgstRate": items.CgstRate,
        "IgstRate": items.IgstRate
      }
      dy.push(a);
    })
    return dy;
  }
    // Function to get dispatcher details
    getDispatcherDetails(): void {

      let partyDetails = {
        "SalesInvoice": {
          "CompanyId": this.globalCompanyId
        }
      }
      this.adminService.getDispatcherDetails(partyDetails).subscribe({
        next: (response: any) => {
          this.updateDispatcherVariables(response);
        },
        error: (error: any) => {
          console.error('Error fetching dispatcher details:', error);
        }
      });

    }

    // Function to update variables based on the response
    updateDispatcherVariables(response: any): void {
      this.dispatcherName = response.CompanyName || ' ';
      this.dispatcherAddress1 = response.AddressLine1 || ' ';
      this.dispatcherAddress2 = response.AddressLine2 || ' ';
      this.dispatcherPlace = response.Place || ' ';
      this.dispatcherPIN = response.PIN || ' ';
      this.dispatcherStatecode="29";
    }
  back(): void {
    this.location.back();
  }

  roundToTwoDigit(value: any): any {
    const a = this.formGroup1.get('value')?.value;

    return parseFloat(a).toFixed(2);
  }

  totalGst(a: any, b: any) {
    return this.roundToTwoDigit(sum([a, b]));
  }

  updatePartyDetails() {
    this.submitted = true;

    console.log("this.getParty.valid -> " + this.getParty.valid);

    if (this.getParty.valid) {
      const payload = { LedgerData: this.getParty.value };
      // console.log(payload);
      this.spinner.show();
      this.adminService.updateLedgerDetails(payload).subscribe({
        next: (res: any) => {
          if (res) {
            //this.toastr.success('Updated Successfully!');
            this.editDemo();
            this.spinner.hide();
            // this.location.back();
            this.submitted = false;
          }
        },
        error: (error: any) => {
          this.submitted = false;
          this.spinner.hide();
          this.toastr.error('Something went wrong');
        },
      });
    } else {
      this.toastr.error('Please fill mandatory fields');
    }
  }

  /**
   * Set E invoice enambled or not
   * @param companyId(int)
   */
  validateEinvoiceEnabled(companyId: Number): void {
    this.adminService.getSingleCompany(companyId).subscribe({
      next: (res: any) => {
        // console.log(res.CompanyList);
        if (res) {
          this.companySelected = true;
          this.isEinVoice = !isNil(res.CompanyList) ? true : false;
        }
      },
      error: (error: any) => {
        this.toastr.error('Something went wrong');
      },
    });
  }

  selectFrieght() {
    let adv = this.formGroup4.controls['advancePaid'].value;
    //adv = Number(adv < 0) ? Number(adv * -1) : Number(adv);
    console.log("adv -> " + adv);

    // if (this.formGroup4.controls['frieght_Plus_Less'].value == 'Own Lorry Frieght') {
    //   if (Number(this.formGroup4.controls['totalFrieght'].value) > adv) {
    //     this.formGroup1.controls['otherChargesAnyValue'].setValue((Number(this.formGroup4.controls['totalFrieght'].value) - adv) * -1);
    //   }
    //   else
    //   if (adv > 0) {
    //     this.formGroup1.controls['otherChargesAnyValue'].setValue(Number(adv));
    //   }
    // }
    // this.isThirdParty = false;

    // if (Number(this.formGroup1.controls['otherChargesAnyValue'].value) != 0) {
    //   this.isThirdParty = true;
    // }

    // if (Number(this.formGroup1.controls['otherChargesAnyValue'].value) > 0) {
    //   this.formGroup1.controls['otherChargesAny'].setValue('Advance Lorry Frieght');
    // }
    // else if (Number(this.formGroup1.controls['otherChargesAnyValue'].value) < 0) {
    //   this.formGroup1.controls['otherChargesAny'].setValue('Less Lorry Frieght');
    // }

    //lastly added
   // this.GetTotal();

  }

  calculateAmount() {
    const totalCal = {
      isEinVoice: this.isEinVoice,
      itemDetails: this.dynamicArray,
      lorryAdvance: this.formGroup4.value.advancePaid ?? 0,
      ledgerstate: 'KA',
      companyState: 'KA',
      newRate: this.newRate,
    };
    this.calculate.calculate(totalCal);
  }

  /**********************refactor code********************* */

  /**
   * set Gst Controls */
  setGstControls(gstRateAmt: any) {
    this.formGroup2.controls['sgstRate'].setValue(Number(gstRateAmt / 2));
    this.formGroup2.controls['cgstRate'].setValue(Number(gstRateAmt / 2));
    this.formGroup2.controls['totalGstAmt'].setValue(gstRateAmt);
  }
  /**
   * Set payment details controls
   */
  setPaymentDetails() { }

  /**
   * ValidateForm based on E invoice enabeled.
   * If enabled then validate all mandatory Delivery address fields,
   * else not validate any fields.
   * @returns boolean
   */
  validateLorryDetails(): boolean {
    this.lorrySubmitted = this.isEinVoice ? true : false;
    return this.isEinVoice ? this.formGroup4.valid : true;
  }



  CalculateTotalFrieght() {
    this.formGroup4.controls['totalFrieght'].setValue(parseFloat(this.formGroup4.value.frieght_bag) * this.totalNoOfBags);
    this.totalFr = parseFloat(this.formGroup4.value.frieght_bag) * this.totalNoOfBags;
  }

  openEinvoiceModal() {
    this.invoiceDialogRef = this.dialog.open(this.invoiceMatDialog, {
      width: '50%', // Customize modal size (optional)
      disableClose: false, // Prevent closing by clicking outside (optional)
    });
  }

  onNoClick(): void {
    this.invoiceDialogRef.close();
  }

  saveEInvoice() {
    // console.log("save e invoice");

    if (this.saveEInvoiceForm.valid) {
      let companyDetails = {
        "CompanyData": {
          "CompanyId": this.globalCompanyId,
          "LegalName": this.saveEInvoiceForm.get('legalName')?.value,
          "PortalUserName": this.saveEInvoiceForm.get('portalUserName')?.value,
          "PortalPw": this.saveEInvoiceForm.get('portalPw')?.value,
          "PortalEmail": this.saveEInvoiceForm.get('portalEmail')?.value,
          "EinvoiceKey": this.saveEInvoiceForm.get('einvoiceKey')?.value,
          "EinvoiceSkey": this.saveEInvoiceForm.get('einvoiceSkey')?.value,
          "EinvoiceUserName": this.saveEInvoiceForm.get('einvoiceUserName')?.value,
          "EinvoicePassword": this.saveEInvoiceForm.get('einvoicePassword')?.value,
          "Pan": this.saveEInvoiceForm.get('pan')?.value,
          "AddressLine2": this.saveEInvoiceForm.get('addressLine2')?.value,
          "CellPhone": this.saveEInvoiceForm.get('cellphone')?.value,
          "EInvoiceReq": this.saveEInvoiceForm.get('eInvoiceReq')?.value,
          "Pin": this.saveEInvoiceForm.get('pin')?.value,
        }
      }

      this.spinner.show();
      // console.log("save e invoice", companyDetails);

      this.authenticationService.saveEInvoice(companyDetails).subscribe({
        next: (res: any) => {
          if (res == true) {
            // this.addCompany.reset();
            this.invoiceDialogRef.close();
            this.spinner.hide();
            this.toastr.success('Company Updated Successfully!');
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
  }


  getCompany(): void {
    this.spinner.show();

    let companyDetails = {
      "CompanyId": this.globalCompanyId,
    };

    this.authenticationService.getEInvoiceByCompanyId(companyDetails).subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          // console.log("get company", res);
          let companyData = res; // Get the company data
          this.saveEInvoiceForm.patchValue({
            pin: companyData.Pin ?? '',
            portalUserName: companyData.PortalUserName ?? '',
            portalPw: companyData.PortalPw ?? '',
            portalEmail: companyData.PortalEmail ?? '',
            einvoiceKey: companyData.EinvoiceKey ?? '',
            einvoiceSkey: companyData.EinvoiceSkey ?? '',
            einvoiceUserName: companyData.EinvoiceUserName ?? '',
            einvoicePassword: companyData.EinvoicePassword ?? '',
            pan: companyData.Pan ?? '',
            addressLine2: companyData.AddressLine2 ?? '',
            cellphone: companyData.CellPhone ?? '',
            eInvoiceReq: companyData.EInvoiceReq ?? 0
          });
        } else {
          this.toastr.error('Something went wrong');
        }
        this.spinner.hide();
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong');
      },
    });
  }


  getCRDRList(invoiceno: string, VochType: string) {
    this.toggleCrDrDetails();
    let payload = {
      InvioceData: {
        CompanyId: this.globalCompanyId,
        VochTypeID: VochType,
        vochno: invoiceno,
      }
    }
    this.adminService.getCRDRDetails(payload).subscribe({
      next: (res: any) => {
        // console.log(res.CRDRDetails);
        this.CRDRDetails = res.CRDRDetails;

        this.TotalCreditAmount = 0;
        this.TotalDebitAmount = 0;

        var i = 0;
        for (i = 0; i < this.CRDRDetails.length; i++) {
          this.TotalCreditAmount += this.CRDRDetails[i]['Credit'];
          this.TotalDebitAmount += this.CRDRDetails[i]['Debit'];
        }

        this.TotalCreditAmount = this.SeperateComma(this.TotalCreditAmount.toFixed(2));
        this.TotalDebitAmount = this.SeperateComma(this.TotalDebitAmount.toFixed(2));


      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong');
      },
    });
  }

  // open and close of Lorry popup
  toggleDispatDetailModal() {
    this.dispactDetailVisible = !this.dispactDetailVisible;
  }

  // open and close of Lorry popup
  toggleGetinvoiceResponseModal() {
    this.invoiceResponseModelVisible = !this.invoiceResponseModelVisible;

  }

  openDispatcherDetailModal() {

  }

  saveDispatcherDetails() {

  }

  getInvoiceResponse() {

    let payload = {
      InvoiceDetails: {
        InvoiceNumber: this.invoiceNoToGetReponse,
        CompanyId: this.globalCompanyId,
      }
    }
    this.adminService.GetInvoiceResponse(payload).subscribe({
      next: (res: any) => {

        const ackNo = res.ackNo;
        const irn = res.irn;
        const invoiceNumber = res.inoiceNumber;
        const signedQR = res.signedQR;
        const status = res.status;

        this.status = status;

        if(res.status == "0") {
          this.invoiceNumber = ackNo;
        } else {
          this.irn = irn;
          this.ackNo = ackNo;
          this.signedQR = signedQR;
        }

        console.log("Invoice Response 1:", ackNo);
        console.log("Invoice Response 2:", irn);
        console.log("Invoice Response 3:", invoiceNumber);
        console.log("Invoice Response 4:", signedQR);
        console.log("Invoice Response 5:", status);

      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong');
      },
    });
  }

  getLorryDetailAutoComplete(): void {
    let data = {
      CompanyId: this.globalCompanyId,
    };
    this.spinner.show();
    this.adminService.getLorryDetailAutoComplete(data).subscribe({
      next: (res: any) => {
        console.log("lorry details autocomplete list", res);
        if (!res.HasErrors && res?.BillSummaries !== null) {
          this.lorryDetailsAutoCompleteList = res.BillSummaries;
          // console.log("lorryDetailsAutoCompleteList", this.lorryDetailsAutoCompleteList);


          if (this.lorryDetailsAutoCompleteList) {
            this.transporterOptions = this.lorryDetailsAutoCompleteList.filter((item: any) => item.Transporter != null).map((item: any) => item.Transporter);
            this.LorryNoOptions = this.lorryDetailsAutoCompleteList.filter((item: any) => item.LorryNo != null).map((item: any) => item.LorryNo);
            this.ownerOptions = this.lorryDetailsAutoCompleteList.filter((item: any) => item.Owner != null).map((item: any) => item.Owner);
            this.driverOptions = this.lorryDetailsAutoCompleteList.filter((item: any) => item.Driver != null).map((item: any) => item.Driver);
          }

        } else {
          this.toastr.error('Something went wrong');
        }
        this.spinner.hide();
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong');
      },
    });
  }

  assignOptionToFormControl(type: string, option: string) {
    switch (type) {
      case 'transporter':
        this.formGroup4.get('transporter').setValue(option);
        break;
      case 'LorryNo':
        this.formGroup4.get('LorryNo').setValue(option);
        break;
      case 'owner':
        this.formGroup4.get('owner').setValue(option);
        break;
      case 'driver':
        this.formGroup4.get('driver').setValue(option);
        break;
      default:
        console.log(`No form control found for type ${type}`);
    }
  }


  // searchOwner(value: any) {
  //   // Implement your search logic here
  //   // Update the ownerOptions array based on the search results
  //   console.log("search owner", value);
  //   if (!value) {
  //     return;
  //   }
  //   of(value).pipe(
  //     debounceTime(300),
  //     distinctUntilChanged(),
  //     switchMap(value =>
  //       this.adminService.getLorryDetailAutoComplete({ CompanyId: this.globalCompanyId, SearchText: value, Type: 'Owner' })
  //     )
  //   ).subscribe({
  //     next: (res: any) => {
  //       console.log("Owner options", res);  // Ad
  //       if (res && res.BillSummaries && res.BillSummaries.length > 0) {
  //         this.ownerOptions = res.BillSummaries.map((item: any) => item.Owner);
  //       }
  //     },
  //     error: (error: any) => {
  //       // Handle the error
  //     },
  //   });
  // }


}
