import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AdminServicesService } from '../../services/admin-services.service';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Constants } from 'src/app/share/models/constants';
import { CommonService } from 'src/app/share/services/common.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-edit-party-master',
  templateUrl: './edit-party-master.component.html',
  styleUrls: ['./edit-party-master.component.scss'],
})
export class EditPartyMasterComponent implements OnInit {
  error: any;
  partyId!: any;
  submitted: boolean = false;
  editParty!: FormGroup;
  stateList: any;
  ledgerList: any;
  financialYear!: string | null;
  customerCode!: string | null;
  userDetails: any;
  userEmail: any;
  dealerList: any;
  accGroupList: any;
  globalCompanyId!: string | null;
  countryList: any;
  customerId: any;
  userType: any;
  userId: any;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private fb: FormBuilder,
    private location: Location,
    private commonService: CommonService
  ) { }

  get add(): { [key: string]: AbstractControl } {
    return this.editParty.controls;
  }

  ngOnInit(): void {
    this.partyId = this.route.snapshot.paramMap.get('id');
    this.financialYear = sessionStorage.getItem('financialYear');
    this.customerCode = sessionStorage.getItem('globalCustomerCode');
    this.userDetails = sessionStorage.getItem('userDetails');
    this.userDetails = JSON.parse(this.userDetails);
    this.userEmail = this.userDetails?.result?.UserEmailId;
    this.userType = this.userDetails?.result?.UserType;
    this.userId = this.userDetails?.result?.UserId;
    this.globalCompanyId = sessionStorage.getItem('companyID');

    this.editParty = new FormGroup({
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

    this.editParty = this.fb.group({
      ledgerName: ['', [Validators.required]],
      ledgerType: ['', [Validators.required]],
      dealerType: ['', [Validators.required]],
      address1: ['', [Validators.required]],
      address2: [''],
      place: ['', [Validators.required]],
      state: ['', [Validators.required]],
      gstin: ['', Validators.pattern(Constants.gstPattern)],
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
      ],
      deductChk: [''],
      perc: [''],
      bankName: [''],
      ifscCode: [
        '',
        [Validators.pattern(Constants.ALPHANUMERIC)],
      ],
      accNo: [''],
      pan: [
        '',
        [ Validators.pattern(Constants.ALPHANUMERIC)],
      ],
      openingBal: [''],
      creditDebit: [''],
      bookPage: [''],
    });

    this.getCountryList();
    this.getStateList();
    this.getLedgerType();
    this.getDealerList();
    this.getAccGroup();
    this.getPartyById(this.partyId);
  }

  ngAfterViewInit() {
    this.commonService.setTheme();
  }

  back(): void {
    this.router.navigate(['/admin/party-details']);
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

  getPartyById(partyId: number) {
    this.spinner.show();
    let partyDetails = {
      "LedgerData": {
        "CompanyId": this.globalCompanyId,
        "LedgerId": partyId,
      },
    };

    this.adminService.getPartyById(partyDetails).subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          let financialYear = res.FinancialYear;
          let list = res.LedgerList;
          this.setData(list);
        } else {
          this.toastr.error('Financial years not found');
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

  setData(partyDetails: any) {
    if (partyDetails) {
      this.editParty.controls['ledgerName'].setValue(
        partyDetails[0].LedgerName
      );
      this.editParty.controls['ledgerType'].setValue(
        partyDetails[0].LedgerType
      );
      this.editParty.controls['dealerType'].setValue(
        partyDetails[0].DealerType
      );
      this.editParty.controls['address1'].setValue(partyDetails[0].Address1);
      this.editParty.controls['address2'].setValue(partyDetails[0].Address2);
      this.editParty.controls['place'].setValue(partyDetails[0].Place);
      this.editParty.controls['state'].setValue(partyDetails[0].State);
      this.editParty.controls['gstin'].setValue(partyDetails[0].Gstn);
      this.editParty.controls['contactDetails'].setValue(
        partyDetails[0].ContactDetails
      );
      this.editParty.controls['country'].setValue(partyDetails[0].Country);
      this.editParty.controls['accGroup'].setValue(
        partyDetails[0].AccountingGroupId
      );
      this.editParty.controls['cellPhone'].setValue(partyDetails[0].CellNo);
      this.editParty.controls['email'].setValue(partyDetails[0].EmailId);
      this.editParty.controls['fssai'].setValue(partyDetails[0].Fssai);
      this.editParty.controls['deductChk'].setValue(partyDetails[0].deductChk);
      this.editParty.controls['perc'].setValue(partyDetails[0].Tdsdeducted);
      this.editParty.controls['bankName'].setValue(partyDetails[0].BankName);
      this.editParty.controls['ifscCode'].setValue(partyDetails[0].Ifsc);
      this.editParty.controls['accNo'].setValue(partyDetails[0].AccountNo);
      this.editParty.controls['pan'].setValue(partyDetails[0].Pan);
      this.editParty.controls['openingBal'].setValue(
        partyDetails[0].OpeningBalance
      );
      this.editParty.controls['creditDebit'].setValue(partyDetails[0].CrDr);
      this.editParty.controls['bookPage'].setValue(
        partyDetails[0].ManualBookPageNo
      );
    }
  }

  onSubmit() {
    this.submitted = true;
    if (this.editParty.valid) {
      let partyDetails = {
        LedgerData: {
          CompanyId: this.globalCompanyId,
          LedgerName: this.editParty.get('ledgerName')?.value,
          LedgerType: this.editParty.get('ledgerType')?.value,
          LedgerId: this.partyId,
          DealerType: this.editParty.get('dealerType')?.value,
          Address1: this.editParty.get('address1')?.value,
          Address2: this.editParty.get('address2')?.value,
          Place: this.editParty.get('place')?.value,
          State: this.editParty.get('state')?.value,
          Gstn: this.editParty.get('gstin')?.value,
          ContactDetails: this.editParty.get('contactDetails')?.value,
          Country: this.editParty.get('country')?.value,
          AccountingGroupId: this.editParty.get('accGroup')?.value,
          CellNo: this.editParty.get('cellPhone')?.value,
          EmailId: this.editParty.get('email')?.value,
          Fssai: this.editParty.get('fssai')?.value,
          Tdsdeducted: this.editParty.get('perc')?.value,
          BankName: this.editParty.get('bankName')?.value,
          Ifsc: this.editParty.get('ifscCode')?.value,
          AccountNo: this.editParty.get('accNo')?.value,
          Pan: this.editParty.get('pan')?.value,
          //"OpeningBalance":0,
          //"Credit/Debit":1
          OpeningBalance: this.editParty.get('openingBal')?.value,
          CrDr: this.editParty.get('creditDebit')?.value,
          ManualBookPageNo: this.editParty.get('bookPage')?.value,
          CreatedBy: this.userId,
        },
      };

      this.spinner.show();
      this.adminService.setParty(partyDetails).subscribe({
        next: (res: any) => {
          if (res == true) {
            this.editParty.reset();
            this.toastr.success('Party Updated Successfully!');
            this.location.back();
            // this.router.navigate(['/admin/party-details']);
            this.spinner.hide();
          }
        },
        error: (error: any) => {
          this.spinner.hide();
          this.error = error;
          this.toastr.error('Something went wrong');
        },
      });
    }
  }
}
