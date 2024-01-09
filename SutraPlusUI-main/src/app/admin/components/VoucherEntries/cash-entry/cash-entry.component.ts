import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AdminServicesService } from 'src/app/admin/services/admin-services.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Constants } from 'src/app/share/models/constants';
import { Location } from '@angular/common';
import { CommonService } from 'src/app/share/services/common.service';
import { Observable, map, of, startWith, tap } from 'rxjs';
import { party } from 'src/app/share/models/party';

@Component({
  selector: 'app-cash-entry',
  templateUrl: './cash-entry.component.html',
  styleUrls: ['./cash-entry.component.scss']
})
export class CashEntryComponent {
  submitted = false;
  addParty!: FormGroup;
  VoucherTypeList:any;

  searchText!: string;
  partyList: party[] = [];

  @ViewChild("drop") nameField!: ElementRef;

  particularrow: Array<any> = [];

  stateList: any[] = [];
  error: any;
  ledgerList: any[] = [];
  financialYear!: string | null;
  customerCode!: string | null;
  userDetails: any;
  userEmail!: string;
  dealerList: any[] = [];
  accGroupList: any[] = [];
  globalCompanyId!: string | null;
  countryList: any[] = [];
  customerId: any;
  userType: any;
  userId!: number;
  checkDealerType: boolean = true;
  ledgerAutoCompletelst: any;
  banknames: any;
  places: any;
  address1: any;
  address2: any;
  filteredBankNames = <any>[];
  filteredPlaces = <any>[];
  filteredAddress1 = <any>[];
  filteredAddress2 = <any>[];

  CreditTotals!: number;
  DebitTotals!: number;

  CreditAccountName!:string | null;
  CreditAccountID!:string | null;
  DebitAccountName!:string | null;
  DebitAccountID!:string | null;



  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private fb: FormBuilder,
    private location: Location,
    public commonService: CommonService
  ) {
    //this.getAutoComplete();
  }

  get add(): { [key: string]: AbstractControl } {
    return this.addParty.controls;
  }

  editName() {
    this.nameField.nativeElement.focus();
  }

  
  onChangeOption(ctrlName: any) {
    debugger;
    const value = this.addParty.get(ctrlName)?.value;
    if (ctrlName === "bankName") {
      this.filteredBankNames = (this._filter(this.ledgerAutoCompletelst?.BankName, value || ''))
    }
    else if (ctrlName === "address1") {
      this.filteredAddress1 = (this._filter(this.ledgerAutoCompletelst?.Address1, value || ''))
    }
    else if (ctrlName === "address2") {
      this.filteredAddress2 = (this._filter(this.ledgerAutoCompletelst?.Address2, value || ''))
    }
    else if (ctrlName === "place") {
      this.filteredPlaces = (this._filter(this.ledgerAutoCompletelst?.Place, value || ''))
    }

  }

  ngOnInit(): void {

    this.addParty = new FormGroup({
      TransDate: new FormControl(''),
      ReceiptPayment: new FormControl(''),
      SelectAccount: new FormControl(''),
      VikriBillNo : new FormControl(''),
      Amount : new FormControl(''),
      Narration: new FormControl(''),
      
    });

    this.addParty = this.fb.group({
      TransDate: ['', [Validators.required]],
      ReceiptPayment: new FormControl('', [Validators.required]),
      SelectAccount: new FormControl('', [Validators.required]),
      VikriBillNo : new FormControl(''),
      Amount : new FormControl('', [Validators.required]),
      Narration: new FormControl('',[Validators.required]),
    });

    this.customerId = this.route.snapshot.paramMap.get('id');
    this.financialYear = sessionStorage.getItem('financialYear');
    this.customerCode = sessionStorage.getItem('globalCustomerCode');
    this.userDetails = sessionStorage.getItem('userDetails');
    this.userDetails = JSON.parse(this.userDetails);
    this.userEmail = this.userDetails?.result?.UserEmailId;
    this.userType = this.userDetails?.result?.UserType;
    this.userId = this.userDetails?.result?.UserId;
    this.globalCompanyId = sessionStorage.getItem('companyID');
  }



  private _filter(filterArr: any, value: string): any[] {
    const filterValue = this._normalizeValue(value);
    return filterArr.filter((val: any) => this._normalizeValue(val).includes(filterValue));
  }

  private _normalizeValue(value: string): string {
    return value.toLowerCase().replace(/\s/g, '');
  }


  ngAfterViewInit() {
    this.commonService.setTheme();

  }

  back(): void {
    this.location.back();
  }





getPartyList(text: string) {
  debugger;
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
        this.partyList = res.records;
        console.log(this.partyList);
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



OnSave()
{
  debugger;

  let payload = {
    TransDate: this.addParty.get('TransDate')?.value,
    ReceiptPayment:this.addParty.get('ReceiptPayment')?.value,
    SelectAccount:this.CreditAccountID,
    VikriBillNo:this.addParty.get('VikriBillNo')?.value,
    Amount:this.addParty.get('Amount')?.value,
    Narration: this.addParty.get('Narration')?.value,
    CompanyID: this.globalCompanyId
  };

  this.adminService.AddCashEntries(payload).subscribe({
    next: (res: any) => {
      if (res == 'Added Successfully...!') {
        this.toastr.success('Invoice Created Successfully!');
        this.spinner.hide();
        this.location.back();
      }

      if (res == 'Duplicate InoviceNo...!') {
        this.toastr.error('Invoice number is already exist!');

        this.spinner.hide();
      }
    },
    error: (error: any) => {
      this.spinner.hide();
      this.toastr.error('Something went wrong');
    },
  });
}

SelectCreditAccount(selectedCreditAccountName:string, SelectedCreditAccountID:string)
{
  this.CreditAccountName = selectedCreditAccountName;
  this.CreditAccountID  = SelectedCreditAccountID;  
}









}
