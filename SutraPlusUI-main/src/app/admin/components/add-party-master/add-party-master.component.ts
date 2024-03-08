import { ChangeDetectorRef, Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AdminServicesService } from '../../services/admin-services.service';
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
import { Observable, debounceTime, distinctUntilChanged, map, of, startWith, tap } from 'rxjs';
import { isNil } from 'lodash-es';

@Component({
  selector: 'app-add-party-master',
  templateUrl: './add-party-master.component.html',
  styleUrls: ['./add-party-master.component.scss'],
})
export class AddPartyMasterComponent implements OnInit, OnChanges {
  @Input() invoiceType: string | null = null;

  submitted = false;
  addParty!: FormGroup;
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
  // filteredBankNames = <any>[];
  // filteredPlaces = <any>[];
  // filteredAddress1 = <any>[];
  // filteredAddress2 = <any>[];
  address1Options: string[] = [];
  address2Options: string[] = [];
  placeOptions: string[] = [];
  bankNameOptions: string[] = [];


  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private fb: FormBuilder,
    private location: Location,
    public commonService: CommonService,
    private changeDetectionRef: ChangeDetectorRef
  ) {
    // this.getAutoComplete();
  }

  get add(): { [key: string]: AbstractControl } {
    return this.addParty.controls;
  }

  // onChangeOption(ctrlName: any) {
  //   // debugger;
  //   const value = this.addParty.get(ctrlName)?.value;
  //   console.log("value", value);

  //   if (ctrlName === "bankName") {
  //     this.filteredBankNames = (this._filter(this.ledgerAutoCompletelst?.BankName, value || ''))
  //     console.log("filteredBankNames", this.filteredBankNames);
  //   }
  //   else if (ctrlName === "address1") {
  //     this.filteredAddress1 = (this._filter(this.ledgerAutoCompletelst?.Address1, value || ''))
  //   }
  //   else if (ctrlName === "address2") {
  //     this.filteredAddress2 = (this._filter(this.ledgerAutoCompletelst?.Address2, value || ''))
  //   }
  //   else if (ctrlName === "place") {
  //     this.filteredPlaces = (this._filter(this.ledgerAutoCompletelst?.Place, value || ''))
  //   }

  // }

  ngOnInit(): void {

    this.addParty = new FormGroup({
      ledgerName: new FormControl(''),
      ledgerType: new FormControl(''),
      dealerType: new FormControl(''),
      address1: new FormControl(''),
      address2: new FormControl(''),
      place: new FormControl(''),
      state: new FormControl(''),
      gstin: new FormControl(''),
      contactDetails: new FormControl(''),
      country: new FormControl(''),
      accGroup: new FormControl(''),
      cellPhone: new FormControl(''),
      itenMane: new FormControl(''),
      email: new FormControl(''),
      fssai: new FormControl(''),
      deductChk: new FormControl(''),
      perc: new FormControl(''),
      bankName: new FormControl(''),
      ifscCode: new FormControl(''),
      units: new FormControl(''),
      accNo: new FormControl(''),
      pan: new FormControl(''),
      openingBal: new FormControl(''),
      creditDebit: new FormControl(''),
      bookPage: new FormControl(''),
    });

    this.addParty = this.fb.group({
      ledgerName: ['', [Validators.required]],
      ledgerType: ['', [Validators.required]],
      dealerType: ['', [Validators.required]],
      address1: ['', [Validators.required]],
      address2: [''],
      place: ['', [Validators.required]],
      state: ['', [Validators.required]],
      gstin: ['', [Validators.pattern(Constants.gstPattern), Validators.maxLength(15)]],
      contactDetails: [
        '',
        [Validators.required, Validators.pattern(Constants.PHONE_REGEXP)],
      ],
      country: ['', [Validators.required]],
      accGroup: ['', [Validators.required]],
      cellPhone: ['', Validators.pattern(Constants.PHONE_REGEXP)],
      email: [
        '',
        [Validators.pattern(Constants.EMAIL_REGEXP)],
      ],
      fssai: [
        '',
        [Validators.pattern(Constants.ALPHANUMERIC)],
      ],
      deductChk: [''],
      perc: [''],
      bankName: [null],
      ifscCode: [
        '',
        [Validators.pattern(Constants.ALPHANUMERIC)],
      ],
      accNo: [''],
      pan: [
        '',
        [Validators.pattern(Constants.ALPHANUMERIC)],
      ],
      openingBal: [''],
      creditDebit: [''],
      bookPage: [''],


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

    this.validateEinvoiceEnabled(Number(this.globalCompanyId));
    this.getCountryList();
    this.getStateList();
    this.getLedgerType();
    this.getDealerList();
    this.getAccGroup();


    this.addParty.get('address1').valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
    ).subscribe(value => {
      this.adminService.getAddPartyAutoComplete({ CompanyId: this.globalCompanyId, SearchText: value, Type: 'Address1' }).subscribe({
        next: (res: any) => {
          if (res && res.AddPartyAutoComplete && res.AddPartyAutoComplete.length > 0) {
            this.address1Options = res.AddPartyAutoComplete.map((item: any) => item.Address1);
            this.changeDetectionRef.detectChanges();
          }
        },
        error: (error: any) => {
          // Handle the error
        },
      });
    });

    this.addParty.get('address2').valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
    ).subscribe(value => {
      this.adminService.getAddPartyAutoComplete({ CompanyId: this.globalCompanyId, SearchText: value, Type: 'Address2' }).subscribe({
        next: (res: any) => {
          if (res && res.AddPartyAutoComplete && res.AddPartyAutoComplete.length > 0) {
            this.address2Options = res.AddPartyAutoComplete.map((item: any) => item.Address2);
            this.changeDetectionRef.detectChanges();
          }
        },
        error: (error: any) => {
          // Handle the error
        },
      });
    });

    this.addParty.get('place').valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
    ).subscribe(value => {
      this.adminService.getAddPartyAutoComplete({ CompanyId: this.globalCompanyId, SearchText: value, Type: 'Place' }).subscribe({
        next: (res: any) => {
          if (res && res.AddPartyAutoComplete && res.AddPartyAutoComplete.length > 0) {
            this.placeOptions = res.AddPartyAutoComplete.map((item: any) => item.Place);
            this.changeDetectionRef.detectChanges();
          }
        },
        error: (error: any) => {
          // Handle the error
        },
      });
    });

    this.addParty.get('bankName').valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
    ).subscribe(value => {
      this.adminService.getAddPartyAutoComplete({ CompanyId: this.globalCompanyId, SearchText: value, Type: 'BankName' }).subscribe({
        next: (res: any) => {
          if (res && res.AddPartyAutoComplete && res.AddPartyAutoComplete.length > 0) {
            this.bankNameOptions = res.AddPartyAutoComplete.map((item: any) => item.BankName);
            console.log("bank names", this.bankNameOptions);
            this.changeDetectionRef.detectChanges();
          }
        },
        error: (error: any) => {
          // Handle the error
        },
      });
    });

  }


  ngOnChanges(changes: SimpleChanges) {
    if (changes['invoiceType']) {
      this.invoiceType = changes['invoiceType'].currentValue;
    }
  }

  // private _filter(filterArr: any, value: string): any[] {
  //   const filterValue = this._normalizeValue(value);
  //   return filterArr.filter((val: any) => this._normalizeValue(val).includes(filterValue));
  // }
  private _filter(filterArr: any, value: string): any[] {

    if (!filterArr) {
      return [];
    }
    const filterValue = this._normalizeValue(value);
    return filterArr.filter((val: any) => this._normalizeValue(val).includes(filterValue));
  }


  private _normalizeValue(value: string): string {
    return value.toLowerCase().replace(/\s/g, '');
  }

  getCountryList() {
    this.adminService.getCountries().subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          this.countryList = res.CountryDDList;
        } else {
          this.toastr.error(res.Errors[0].Message);
          this.error = res.Errors[0].Message;
        }
        this.spinner.hide();
      },
      error: (error: any) => {
        this.spinner.hide();
        this.error = error;
        this.toastr.error('Something went wrong');
      },
    });
  }

  getStateList(): void {
    this.adminService.getStates().subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          this.stateList = res.StateList;
        } else {
          this.toastr.error(res.Errors[0].Message);
          this.error = res.Errors[0].Message;
        }
        this.spinner.hide();
      },
      error: (error: any) => {
        this.spinner.hide();
        this.error = error;
        this.toastr.error('Something went wrong');
      },
    });
  }

  ngAfterViewInit() {
    this.commonService.setTheme();

  }

  back(): void {
    this.location.back();
  }

  getLedgerType(): void {
    let LedgerType = {
      LedgerType: {
        CustomerFinancialYearId: this.financialYear,
        CustomerCode: this.customerCode,
      },
    };

    this.spinner.show();
    this.adminService.getLedger(LedgerType).subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          this.ledgerList = res.LederTypeDDList;
        } else {
          this.toastr.error('Something went wrong');
          this.error = res.Errors[0].Message;
        }
        this.spinner.hide();
      },
      error: (error: any) => {
        this.spinner.hide();
        this.error = error;
        this.toastr.error('Something went wrong');
      },
    });
  }

  getDealerList(): void {
    let DealerType = {
      DealerType: {
        CustomerFinancialYearId: this.financialYear,
        CustomerCode: this.customerCode,
      },
    };

    this.spinner.show();
    this.adminService.getDealer(DealerType).subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          this.dealerList = res.DealerTypeDDList;
        } else {
          this.toastr.error('Something went wrong');
          this.error = res.Errors[0].Message;
        }
        this.spinner.hide();
      },
      error: (error: any) => {
        this.spinner.hide();
        this.error = error;
        this.toastr.error('Something went wrong');
      },
    });
  }

  getAccGroup(): void {
    let AccounitngGroups = {
      AccounitngGroups: {
        CustomerFinancialYearId: this.financialYear,
        CustomerCode: this.customerCode,
      },
    };

    this.spinner.show();
    this.adminService.getAccGrp(AccounitngGroups).subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          this.accGroupList = res.AccounitngGroupsDDList;
        } else {
          this.toastr.error('Something went wrong');
          this.error = res.Errors[0].Message;
        }
        this.spinner.hide();
      },
      error: (error: any) => {
        this.spinner.hide();
        this.error = error;
        this.toastr.error('Something went wrong');
      },
    });
  }

  getAutoComplete(): void {
    let Autocomplete = {
      Autocomplete: {
        CustomerFinancialYearId: this.financialYear,
        CustomerCode: this.customerCode,
      },
    };
    this.spinner.show();
    this.adminService.getAutocomplete(Autocomplete).subscribe({
      next: (res: any) => {
        console.log("ledger autocomplete list", Autocomplete);
        if (!res.HasErrors && res?.Data !== null) {
          this.ledgerAutoCompletelst = res.LedgerAutoCompletelst;

        } else {
          this.toastr.error('Something went wrong');
          this.error = res.Errors[0].Message;
        }
        this.spinner.hide();
      },
      error: (error: any) => {
        this.spinner.hide();
        this.error = error;
        this.toastr.error('Something went wrong');
      },
    });
  }
  isEinVoice: boolean = false;
  validateEinvoiceEnabled(companyId: Number): void {
    this.adminService.getSingleCompany(companyId).subscribe({
      next: (res: any) => {
        // console.log(res.CompanyList);
        if (res) {
          this.isEinVoice = !isNil(res.CompanyList) ? true : false;
        }
      },
      error: (error: any) => {
        this.toastr.error('Something went wrong');
      },
    });
  }
  Isaddress2: boolean = false;
  IscellPhone: boolean = false;
  Ispan: boolean = false;
  Isemail: boolean = false;
  onDealerChange(e: any): void {
    let val = e.target.value
    if (val === 'Unregistered Dealer') {
      this.checkDealerType = false
      this.Isaddress2 = false;
      this.IscellPhone = false;
      this.Ispan = false;
      this.Isemail = false;
      this.addParty.get('address2').clearValidators();
      this.addParty.get('cellPhone').clearValidators();
      this.addParty.get('pan').clearValidators();
      this.addParty.get('email').clearValidators();
    } else {
      if (this.isEinVoice) {
        this.checkDealerType = true
        this.Isaddress2 = true;
        this.IscellPhone = true;
        this.Ispan = true;
        this.Isemail = true;
        this.addParty.get('address2').setValidators([Validators.required]);
        this.addParty.get('cellPhone').setValidators([Validators.required]);
        this.addParty.get('pan').setValidators([Validators.required]);
        this.addParty.get('email').setValidators([Validators.required]);
      }
    }
    this.addParty.get('address2').updateValueAndValidity();
    this.addParty.get('cellPhone').updateValueAndValidity();
    this.addParty.get('pan').updateValueAndValidity();
    this.addParty.get('email').updateValueAndValidity();
  }

  onSubmit(): void {

    this.submitted = true;
    // if (this.addParty.valid) {
      let partyDetails = {
        LedgerData: {
          CompanyId: this.globalCompanyId,
          LedgerName: this.addParty.get('ledgerName')?.value,
          LedgerType: this.addParty.get('ledgerType')?.value,
          //"LedgerId" auto calculated
          DealerType: this.addParty.get('dealerType')?.value,
          Address1: this.addParty.get('address1')?.value,
          Address2: this.addParty.get('address2')?.value,
          Place: this.addParty.get('place')?.value,
          State: this.addParty.get('state')?.value,
          Gstn: (this.addParty.get('gstin')?.value)?.toUpperCase(),
          ContactDetails: this.addParty.get('contactDetails')?.value,
          Country: this.addParty.get('country')?.value,
          AccountingGroupId: this.addParty.get('accGroup')?.value,
          CellNo: this.addParty.get('cellPhone')?.value,
          EmailId: this.addParty.get('email')?.value,
          Fssai: this.addParty.get('fssai')?.value,
          Tdsdeducted: parseInt(this.addParty.get('perc')?.value), // Integer
          BankName: this.addParty.get('bankName')?.value,
          Ifsc: this.addParty.get('ifscCode')?.value,
          AccountNo: this.addParty.get('accNo')?.value,
          Pan: this.addParty.get('pan')?.value,
          //"OpeningBalance":0,
          //"Credit/Debit":1

          OpeningBalance: parseInt(this.addParty.get('openingBal')?.value == '' ? 0 : this.addParty.get('openingBal')?.value),
          CrDr: this.addParty.get('creditDebit')?.value,
          ManualBookPageNo: parseInt(this.addParty.get('bookPage')?.value == '' ? 0 : this.addParty.get('bookPage')?.value), //Integer
          CreatedBy: this.userId,
        },
      };

      this.spinner.show();

      this.adminService.createParty(partyDetails).subscribe({
        next: (res: any) => {
          if (res == true) {
            this.toastr.success('Party Created Successfully!');
            this.location.back();
          }
          else {
            this.toastr.error('Sorry Duplicate Entry', 'Party Already Existing!');
          }
          this.spinner.hide();

        },
        error: (error: any) => {
          this.spinner.hide();
          this.error = error;
          this.toastr.error('Something went wrong');
        },
      });
    // }
  }

  ifopeningBalance() {
    const openingBal = this.addParty.get('openingBal')?.value;
    return !(Number(openingBal) > 0);
  }

  transform() {
    this.addParty.controls['gstin'].setValue((this.addParty.value.gstin).toUpperCase());
  }

  getAddress1(searchText: string) {

    this.adminService.getAddPartyAutoComplete({ CompanyId: this.globalCompanyId, SearchText: searchText, Type: 'Address1' }).subscribe({
      next: (res: any) => {
        if (res && res.AddPartyAutoComplete && res.AddPartyAutoComplete.length > 0) {
          this.address1Options = res.AddPartyAutoComplete.map((item: any) => item.Address1);
          this.changeDetectionRef.detectChanges();
        }
      },
      error: (error: any) => {
        this.address1Options = [];
      },
    });

  }

  getAddress2(searchText: string) {

    this.adminService.getAddPartyAutoComplete({ CompanyId: this.globalCompanyId, SearchText: searchText, Type: 'Address2' }).subscribe({
      next: (res: any) => {
        if (res && res.AddPartyAutoComplete && res.AddPartyAutoComplete.length > 0) {
          this.address2Options = res.AddPartyAutoComplete.map((item: any) => item.Address2);
          this.changeDetectionRef.detectChanges();
        }
      },
      error: (error: any) => {
        this.address2Options = [];
      },
    });

  }

  getPlace(searchText: string) {
    this.adminService.getAddPartyAutoComplete({ CompanyId: this.globalCompanyId, SearchText: searchText, Type: 'Place' }).subscribe({
      next: (res: any) => {
        if (res && res.AddPartyAutoComplete && res.AddPartyAutoComplete.length > 0) {
          this.placeOptions = res.AddPartyAutoComplete.map((item: any) => item.Place);
          this.changeDetectionRef.detectChanges();
        }
      },
      error: (error: any) => {
        // Handle the error
      },
    });
  }

  selectedParty: any;
  add2: any;
  place: any;

  assignOptionToFormControl(type: string, option: string) {
    switch (type) {
      case 'address1':
        this.addParty.get('address1').setValue(option);
        break;
      case 'address2':
        this.addParty.get('address2').setValue(option);
        break;
      case 'place':
        this.addParty.get('place').setValue(option);
        break;
      case 'bankName':
        this.addParty.get('bankName').setValue(option);
        break;
      default:
        console.log(`No form control found for type ${type}`);
    }
  }

}
