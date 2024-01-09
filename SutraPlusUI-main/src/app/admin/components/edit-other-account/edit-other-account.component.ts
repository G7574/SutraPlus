import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AdminServicesService } from '../../services/admin-services.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { CommonService } from 'src/app/share/services/common.service';

@Component({
  selector: 'app-edit-other-account',
  templateUrl: './edit-other-account.component.html',
  styleUrls: ['./edit-other-account.component.scss']
})
export class EditOtherAccountComponent implements OnInit {

  ledgerId?: any;
  addLedger!: FormGroup;
  financialYear!: string | null;
  customerCode!: string | null;
  error: any;
  accGroupList: any;
  submitted:boolean = false;
  userEmail?:string;
  userDetails: any;
  globalCompanyId: any;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private fb: FormBuilder,
    //private location: Location,
    private commonService: CommonService
  ) {}

  ngOnInit(): void {
    this.ledgerId = this.route.snapshot.paramMap.get('id');
    this.financialYear = sessionStorage.getItem('financialYear');
    this.customerCode = sessionStorage.getItem('globalCustomerCode');
    this.userDetails = sessionStorage.getItem('userDetails')
    this.userDetails = JSON.parse(this.userDetails);
    this.userEmail = this.userDetails?.result?.UserEmailId;
    this.globalCompanyId = sessionStorage.getItem('companyID');

    this.addLedger = new FormGroup({
      ledgerName: new FormControl(''),
      place: new FormControl(''),
      accGroup: new FormControl('')
    });

    this.addLedger = this.fb.group({
      ledgerName: [
        '',
        [Validators.required],
      ],
      place: [
        '',
        [Validators.required],
      ],
      accGroup: [
        '',
        [Validators.required],
      ],
    });

    this.getLedgerById(this.ledgerId);
    this.getAccGroup();
  }

  back(): void {
    this.router.navigate(['/admin/other-account']);
  }

  getLedgerById(ledgerId: string) {
    this.spinner.show();
    let partyDetails = {
      LedgerData: {
        CompanyId: this.globalCompanyId,
        LedgerId: ledgerId,
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
      this.addLedger.controls['ledgerName'].setValue(partyDetails[0].LedgerName);
      this.addLedger.controls['place'].setValue(partyDetails[0].Place);
      this.addLedger.controls['accGroup'].setValue(partyDetails[0].AccountingGroupId);
    }
  }

  onSubmit(): void {
    this.submitted = true;
    if (this.addLedger.valid) {
      let obj = {
        "LedgerData": {
          "LedgerName": this.addLedger.get('ledgerName')?.value,
          "Place": this.addLedger.get('place')?.value,
          "AccountingGroupId": this.addLedger.get('accGroup')?.value,
          "CreatedBy": this.userDetails.result.UserId,
          "CompanyId":this.globalCompanyId,
          "LedgerId" :this.ledgerId
        }
      }

      this.spinner.show()
      this.adminService.editLedger(obj).subscribe({
        next: (res: any) => {
          
          if (res == true) {
            this.toastr.success('Account Updated Successfully!');
            this.spinner.hide();
            this.router.navigate(['/admin/other-account']);
            //this.location.back();
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

  get add(): { [key: string]: AbstractControl } {
    return this.addLedger.controls;
  }

  getAccGroup(): void {
    let AccounitngGroups = {
      "AccounitngGroups": {
        "CustomerFinancialYearId": this.financialYear,
        "CustomerCode": this.customerCode
      }
    }

    this.spinner.show()
    this.adminService.getAccGrp(AccounitngGroups).subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          this.accGroupList = res.AccounitngGroupsDDList
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
  

}
