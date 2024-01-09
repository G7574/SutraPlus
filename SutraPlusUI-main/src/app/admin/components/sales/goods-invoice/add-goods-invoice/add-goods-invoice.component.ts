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
import { ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthenticationServiceService } from 'src/app/authentication/services/authentication-service.service';
import { SuperAdminServiceService } from 'src/app/super-admin/services/super-admin-service.service';
import { debounceTime, distinctUntilChanged, of, switchMap } from 'rxjs';
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
  ledgerId!: number;
  searchText!: string;
  partyList: party[] = [];
  otherAccList: party[] = [];
  stateList: { Statecode: string; Statename: string }[] = [];
  todayDate?: string;
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
  lorryNoOptions: string[] = [];
  // ownerOptions: string[] = [];
  owner: string = ' ';
  ownerOptions: string[] = [];

  driverOptions: string[] = [];

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
    private changeDetectionRef: ChangeDetectorRef

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
      lorryNo: new FormControl(''),
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
      distance: new FormControl('', [Validators.required]),
      note: new FormControl('', [Validators.required]),
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
    
    this.getDispatcherDetails();

    this.spinner.show();

    if (this.invType === "GinningInvoice") {
      this.isShow = false;
    }

    if (this.invType === "DebitNote") {
      this.isShow = false;
      this.isDebitNote = false;
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
      voucherType: new FormControl('', [Validators.required]),
      invoiceNo: new FormControl('', [Validators.required]),
      state: new FormControl('', [Validators.required]),
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
      toPlace: new FormControl('', [Validators.required]),
      fromPlace: new FormControl('', [Validators.required]),
    });

    this.formGroup4.get('transporter').valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
    ).subscribe(value => {
      // Call your API with the typed value
      this.adminService.getLorryDetailAutoComplete({ CompanyId: this.globalCompanyId, SearchText: value, Type: 'Transporter' }).subscribe({
        next: (res: any) => {
          // console.log("transporter options", res);  // Ad
          if (res && res.BillSummaries && res.BillSummaries.length > 0) {
            this.transporterOptions = res.BillSummaries.map((item: any) => item.Transporter);
            // console.log("transporter options list ", this.transporterOptions);
            this.changeDetectionRef.detectChanges();
          }
        },
        error: (error: any) => {
          // Handle the error
        },
      });
    });

    this.formGroup4.get('lorryNo').valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
    ).subscribe(value => {
      this.adminService.getLorryDetailAutoComplete({ CompanyId: this.globalCompanyId, SearchText: value, Type: 'LorryNo' }).subscribe({
        next: (res: any) => {
          // console.log("LorryNo options", res);  // Ad
          if (res && res.BillSummaries && res.BillSummaries.length > 0) {
            this.lorryNoOptions = res.BillSummaries.map((item: any) => item.LorryNo);
            this.changeDetectionRef.detectChanges();
          }
        },
        error: (error: any) => {
          // Handle the error
        },
      });
    });

    this.formGroup4.get('owner').valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
    ).subscribe(value => {
      this.adminService.getLorryDetailAutoComplete({ CompanyId: this.globalCompanyId, SearchText: value, Type: 'Owner' }).subscribe({
        next: (res: any) => {
          // console.log("Owner options", res);  // Ad
          if (res && res.BillSummaries && res.BillSummaries.length > 0) {
            this.ownerOptions = res.BillSummaries.map((item: any) => item.Owner);
            // console.log("owner options", this.ownerOptions);
            this.changeDetectionRef.detectChanges();
          }
        },
        error: (error: any) => {
          // Handle the error
        },
      });
    });

    this.formGroup4.get('driver').valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
    ).subscribe(value => {
      // Call your API with the typed value
      this.adminService.getLorryDetailAutoComplete({ CompanyId: this.globalCompanyId, SearchText: value, Type: 'Driver' }).subscribe({
        next: (res: any) => {
          // console.log("Driver options", res);  // Ad
          if (res && res.BillSummaries && res.BillSummaries.length > 0) {
            this.driverOptions = res.BillSummaries.map((item: any) => item.Driver);
            this.changeDetectionRef.detectChanges();
          }
        },
        error: (error: any) => {
          // Handle the error
        },
      });
    });

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
  }

  // open and close of Lorry popup
  toggleLiveDemo() {
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
    // validate the lorry form based on condition and return boolean
    if (this.validateLorryDetails()) {

      this.selectFrieght();

      this.otherChargesInfo = true;
      this.visible = !this.visible;


      this.LineItemTotal(0, 0, 'SaveLorryDetails');



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
                lorryNo: InvoiceData['LorryNo'],
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
                fromPlace:InvoiceData['FromPlace'],
                toPlace:InvoiceData['ToPlace'],
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

  onClickTest($event: any, item: any) {
    this.ledgerId = item.ledgerId;
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
      this.getParty.controls['gst'].setValue(list.gstn);
      this.getParty.controls['state'].setValue(list.state);
      this.getParty.controls['address1'].setValue(list.address1);
      this.getParty.controls['address2'].setValue(list.address2);
      this.getParty.controls['pin'].setValue(list.pin);
      this.getParty.controls['emailId'].setValue(list.emailId);
      debugger;
      this.getParty.controls['cellNo'].setValue(list.cellNo);
      this.getParty.controls['ledgerId'].setValue(list.ledgerId);
      this.getParty.controls['legalName'].setValue(list.legalName);
      this.getParty.controls['ledgerName'].setValue(list.ledgerName);

      this.formGroup3.controls['address'].setValue(address);
      this.formGroup3.controls['fromPlace'].setValue(this.companyPlace);
      this.formGroup3.controls['toPlace'].setValue(list.place);
      this.formGroup4.controls['address1'].setValue(list.address1);
      this.formGroup4.controls['address2'].setValue(list.address2);
      this.formGroup4.controls['place'].setValue(list.place);
      this.formGroup4.controls['pinCode'].setValue(list.pin);
      this.dealerType = list.dealerType;
      this.PANNumber = list.pan;
      this.GSTNumber = list.gstn;
      this.defaultState = list.state;
    }
  }

  setData2(list: any): void {
    if (list) {
      this.getParty.controls['voucherType'].setValue(list.VoucherType);
      this.getParty.controls['invoiceNo'].setValue(list.InvoiceNo);
      this.formGroup3.controls['fromPlace'].setValue(this.companyPlace);
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


    this.formGroup2.controls['totalGstAmt'].setValue(this.SeperateComma(TotalGST.toFixed(2)));

    debugger;
    this.grandTotalAmount = (this.totalAmount + TotalGST + (this.formGroup1.controls['otherCharges1'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges1'].value)) + (this.formGroup1.controls['otherCharges2'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges2'].value)) + (this.formGroup1.controls['otherCharges3'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherCharges3'].value)) + (this.formGroup1.controls['otherChargesAnyValue'].value == '' ? 0 : parseFloat(this.formGroup1.controls['otherChargesAnyValue'].value)));

    this.formGroup2.controls['totalcgstAmt'].setValue(this.SeperateComma(TotalCGSTAmt.toFixed(2)));
    this.formGroup2.controls['totalsgstAmt'].setValue(this.SeperateComma(TotalCGSTAmt.toFixed(2)));
    this.formGroup2.controls['totaligstAmt'].setValue(this.SeperateComma(TotalIGSTAmt.toFixed(2)));

    // console.log(this.grandTotalAmount);

    let roundOff = Math.round(this.grandTotalAmount);
    let roundOffValue = Number(roundOff - this.grandTotalAmount).toFixed(2);
    this.formGroup2.controls['roundOff'].setValue(roundOffValue);
    this.grandTotalAmount = this.SeperateComma(roundOff);


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
    debugger;

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
    debugger;
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
    debugger;
    if (this.dynamicArray[i].WeightPerBag.toString() == '') {
      this.dynamicArray[i].WeightPerBag = 0;
    }

    if (Caller == 'KgBag') {
      this.dynamicArray[i].WeightPerBag = val;
    }

    if (Caller == 'NoofBags') {
      this.dynamicArray[i].NoOfBags = val;
    }

    if (Caller == 'rate') {
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
    if (Caller == 'Amount' && !this.isDebitNote) {
      this.dynamicArray[i].Amount = parseFloat(val.toFixed(2));
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



    if (sessionStorage.getItem('State')?.toUpperCase() == this.defaultState?.toUpperCase()) {
      this.dynamicArray[i].CgstAmount = !this.igstShow
        ? Number((this.dynamicArray[i].Taxable * this.dynamicArray[i].CgstRate) / 100)
        : 0;
      this.dynamicArray[i].SgstAmount = !this.igstShow
        ? Number((this.dynamicArray[i].Taxable * this.dynamicArray[i].SgstRate) / 100)
        : 0;
      this.dynamicArray[i].IgstAmount = 0;
    }
    else {
      this.dynamicArray[i].CgstAmount = 0;
      this.dynamicArray[i].SgstAmount = 0;
      this.dynamicArray[i].IgstAmount = this.igstShow
        ? Number((this.dynamicArray[i].Taxable * this.dynamicArray[i].IgstRate) / 100)
        : 0;
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
      this.selectFrieght();
    }
  }

  productItemClick($event: any, item: any, ind: number) {
    debugger;
    if (item.commodityName.length >= 1) {
      let UserDetails = {
        SelesItem: {
          Name: '',
          GSTType: item.igst,
        },
      };
      this.adminService.getNewProductList(UserDetails).subscribe({
        next: (res: any) => {
          debugger;
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








  onSave(): void {
    debugger;
    var Flag = true;
    var RowNumber = 0;
    var ErrorMsg = 'Please enter ';
    var e = 0;

    console.log(this.dynamicArray);
    for (e = 0; e < this.dynamicArray.length; e++) {

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
          FromPlace: this.formGroup3.value.fromPlace,
          ToPlace: this.formGroup3.value.toPlace,
        },
        LorryDetails: {
          PoNumber: this.formGroup4.value.poNo,
          EwaybillNo: this.formGroup4.value.eWayBill,
          Transporter: this.formGroup4.value.transporter,
          LorryNo: this.formGroup4.value.lorryNo,
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

            debugger;
            this.getCRDRList(res.split('|||||')[1], res.split('|||||')[2]);


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
    debugger;
    if (this.formGroup4.controls['frieght_Plus_Less'].value == 'Own Lorry Frieght') {
      if (Number(this.formGroup4.controls['totalFrieght'].value) > adv) {
        this.formGroup1.controls['otherChargesAnyValue'].setValue((Number(this.formGroup4.controls['totalFrieght'].value) - adv) * -1);
      }
      else if (adv > 0) {
        this.formGroup1.controls['otherChargesAnyValue'].setValue(Number(adv));
      }
    }
    this.isThirdParty = false;
    if (Number(this.formGroup1.controls['otherChargesAnyValue'].value) != 0) {
      this.isThirdParty = true;
    }

    if (Number(this.formGroup1.controls['otherChargesAnyValue'].value) > 0) {
      this.formGroup1.controls['otherChargesAny'].setValue('Advance Lorry Frieght');
    }
    else if (Number(this.formGroup1.controls['otherChargesAnyValue'].value) < 0) {
      this.formGroup1.controls['otherChargesAny'].setValue('Less Lorry Frieght');
    }


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
        // console.log("Invoice Response:", res);
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
            this.lorryNoOptions = this.lorryDetailsAutoCompleteList.filter((item: any) => item.LorryNo != null).map((item: any) => item.LorryNo);
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
      case 'lorryNo':
        this.formGroup4.get('lorryNo').setValue(option);
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