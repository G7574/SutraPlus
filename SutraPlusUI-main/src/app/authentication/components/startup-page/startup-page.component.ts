import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationServiceService } from '../../services/authentication-service.service'
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-startup-page',
  templateUrl: './startup-page.component.html',
  styleUrls: ['./startup-page.component.scss']
})
export class StartupPageComponent implements OnInit {
  currentYear!: number
  yearList: any[] = [];
  error: any;
  customerId: any

  errorShow1!: boolean;
  errorShow2!: boolean;

  constructor(
    private router: Router,
    private authenticationService: AuthenticationServiceService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.currentYear = new Date().getFullYear();

    this.customerId = this.route.snapshot.paramMap.get('id');
    sessionStorage.setItem('globalCustomerCode', this.customerId)
    this.getYears(this.customerId)
  }

  getYears(id: any): void {
    this.spinner.show();
    this.authenticationService.getYearList(id).subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          this.yearList = res.YearList

          if (this.yearList.length > 0) {
            sessionStorage.setItem('companyName', this.yearList[0].Name)
            this.errorShow1 = true;
            this.errorShow2 = false
          }
          else {
            sessionStorage.setItem('companyName', '')
            this.errorShow1 = false;
            this.errorShow2 = true;
          }
        } else {
          // this.toastr.error(res.Errors[0].Message);
          // this.error = res.Errors[0].Message;
          this.toastr.error('Customer Code not found. Please provide correct URL');
          this.error = 'Customer Code not found. Please provide correct URL';
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

  goToLogin(data: any): void {
    this.router.navigate(['/login']);
    console.log(data);
    sessionStorage.setItem('tenantId', btoa(data.Id));
    sessionStorage.setItem('themeCode', data.ThemeCode);
    sessionStorage.setItem('financialYear', data.Description);
    sessionStorage.setItem('financialYearValue', data.Year);
    sessionStorage.setItem('startdate', data.StartDate);
    sessionStorage.setItem('enddate', data.EndDate);
  }
}
