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
  selector: 'app-bank-journal-entries',
  templateUrl: './bank-journal-entries.component.html',
  styleUrls: ['./bank-journal-entries.component.scss']
})
export class BankJournalEntriesComponent {
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
      voucherType: new FormControl(''),
      CreditAccount: new FormControl(''),
      DebitAccount: new FormControl(''),
      VikriBillNo : new FormControl(''),
      Amount : new FormControl(''),
      Narration: new FormControl(''),
      
    });

    this.addParty = this.fb.group({
      TransDate: ['', [Validators.required]],
      voucherType: new FormControl('', [Validators.required]),
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

    this.getVoucherTypeList();

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





  onSubmit(): void {
    this.submitted = true;
    debugger;
    this.submitted = true;

    this.addParty.patchValue
          ({
            CreditAccount: null,
            DebitAccount: null
          });


    if(this.addParty.valid)
    {
       if(this.particularrow.length == 0)
       {
          this.particularrow.push({ "AccountName": this.CreditAccountName,"AccountID": this.CreditAccountID , "Sno": (this.particularrow.length + 1), "CreditAmount": this.addParty.get('Amount')?.value, "DebitAmount": 0,"Narration":this.addParty.get('Narration')?.value,"VikriBillno":this.addParty.get('VikriBillNo')?.value,"TransDate":this.addParty.get('TransDate')?.value,"CompanyID":this.globalCompanyId,"LedgerNameForNarration":'' });
          
          this.addParty.patchValue
          ({
            CreditAccount: '',
            DebitAccount: ''
          });

          this.CreditAccountName = '';
          this.CreditAccountID  = '';  
       }
       else
       {
        if(this.addParty.get('Amount')?.value > 0)
        {
        if((this.DebitTotals + this.addParty.get('Amount')?.value) > this.CreditTotals)
        {
         alert('Invalid Amount');
        }
        else
        {
          this.particularrow.push({ "AccountName": this.DebitAccountName,"AccountID":this.DebitAccountID, "Sno": (this.particularrow.length + 1), "CreditAmount": 0, "DebitAmount": this.addParty.get('Amount')?.value,"Narration":this.addParty.get('Narration')?.value,"VikriBillno":this.addParty.get('VikriBillNo')?.value,"TransDate":this.addParty.get('TransDate')?.value,"VoucherTypeID":this.addParty.get('voucherType')?.value,"CompanyID":this.globalCompanyId,"LedgerNameForNarration":'' });
        }
      }
      else
      {
        alert('Invalid Amount');
      }
       }
       console.log(this.particularrow); 
       this.GetTotals();
       this.addParty.controls['Amount'].setValue(this.CreditTotals - this.DebitTotals);
    } 
  }




  getVoucherTypeList() {
          
    this.adminService.getVoucherType([]).subscribe({
      next: (res: any) => {
        
        this.VoucherTypeList = res.VoucherTypes;
        console.log(this.VoucherTypeList);
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong');
      },
    });
  
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

SelectCreditAccount(selectedCreditAccountName:string, SelectedCreditAccountID:string)
{
  this.CreditAccountName = selectedCreditAccountName;
  this.CreditAccountID  = SelectedCreditAccountID;  
  
}

SelectDebitAccount(selectedDebitAccountName:string, SelectedDebitAccountID:string)
{
  this.DebitAccountName = selectedDebitAccountName;
  this.DebitAccountID  = SelectedDebitAccountID;  
}



GetTotals()
{
    this.CreditTotals = 0;
    this.DebitTotals = 0;

    var i = 0;
    for(i=0;i<this.particularrow.length; i++)
    { 
      if(i == 0)
      {
        this.CreditTotals += parseFloat(this.particularrow[i]['CreditAmount']);
      }
      else{
        this.DebitTotals += parseFloat(this.particularrow[i]['DebitAmount']);
      }
    }
}


OnSave()
{
  debugger;

  for(var i=0;i < this.particularrow.length;  i++)
  {
      if(i == 0)
      {
        this.particularrow[0]['LedgerNameForNarration'] = this.particularrow[1]['AccountName']
      }
      else
      {
        this.particularrow[i]['LedgerNameForNarration'] = this.particularrow[0]['AccountName']
      }
  }

  

  let payload = {
    BankJournalEntries: this.formatDynamicArray(this.particularrow),
    VoucherTypeID:this.addParty.get('voucherType')?.value,
    CompanyID: this.globalCompanyId
  };

  console.log(this.particularrow);
  console.log(this.formatDynamicArray(this.particularrow));

  this.adminService.AddBankJournalEntries(payload).subscribe({
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


formatDynamicArray(dynamicArray: any) {
  let dy:any = [];
  dynamicArray.forEach((items: any) => {
    const a = {
      "Sno": items.Sno,
      "AccountName": items.Data,
      "LedgerID": items.AccountID,
      "VoucherTypeID": items.voucherType,
      "TransDate": items.TransDate,
      "AccountID": items.AccountID,
      "CreditAmount": items.CreditAmount,
      "DebitAmount": items.DebitAmount,
      "Narration": items.Narration, 
      "VikriBillno": items.VikriBillno,
      "CompanyID":this.globalCompanyId,
      "LedgerNameForNarration": items.LedgerNameForNarration
    }
    dy.push(a);
  })
  return dy;
}




}
