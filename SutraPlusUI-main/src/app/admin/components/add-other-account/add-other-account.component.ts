import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/share/services/common.service';
import { AdminServicesService } from '../../services/admin-services.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Location } from '@angular/common';
@Component({
  selector: 'app-add-other-account',
  templateUrl: './add-other-account.component.html',
  styleUrls: ['./add-other-account.component.scss']
})
export class AddOtherAccountComponent implements OnInit {

  addLedger!: FormGroup;
  financialYear!: string | null;
  customerCode!: string | null;
  error: any;
  accGroupList: any;
  submitted:boolean = false;
  userEmail?:string;
  userDetails: any;
  globalCompanyId: any;
  constructor(private router: Router,
    // private route: ActivatedRoute,
       private adminService: AdminServicesService,
       private toastr: ToastrService,
       private spinner: NgxSpinnerService,
       private fb: FormBuilder,
       private commonService: CommonService,
       private location: Location
  ) { }

  ngOnInit(): void { 

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
      ],
      accGroup: [
        '',
        [Validators.required],
      ],
    });

    this.financialYear = sessionStorage.getItem('financialYear');
    this.customerCode = sessionStorage.getItem('globalCustomerCode');
    this.userDetails = sessionStorage.getItem('userDetails')
    this.userDetails = JSON.parse(this.userDetails);
    this.userEmail = this.userDetails?.result?.UserEmailId;
    this.globalCompanyId = sessionStorage.getItem('companyID');

    this.getAccGroup();
    
  }
  ngAfterViewInit() {
    this.commonService.setTheme();
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
    this.adminService.getOtherAccGrp(AccounitngGroups).subscribe({
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

  onSubmit(): void {
    this.submitted = true;
    if (this.addLedger.valid) {
      let obj = {
        "LedgerData": {
          "LedgerName": this.addLedger.get('ledgerName')?.value,
          "Place": this.addLedger.get('place')?.value,
          "AccountingGroupId": this.addLedger.get('accGroup')?.value,
          "CreatedBy": this.userDetails.result.UserId,
          "CompanyId":this.globalCompanyId
        }
      }

      this.spinner.show()
      this.adminService.createLedger(obj).subscribe({
        next: (res: any) => {
          if (res == true) {
            this.toastr.success('Account Created Successfully!');
            this.spinner.hide();
            this.location.back();
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

  back(): void {
    this.router.navigate(['/admin/other-account'])
  } 

}
