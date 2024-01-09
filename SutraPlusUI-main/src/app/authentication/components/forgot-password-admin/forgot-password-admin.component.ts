import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Constants } from 'src/app/share/models/constants';
import { AuthenticationServiceService } from '../../services/authentication-service.service';

@Component({
  selector: 'app-forgot-password-admin',
  templateUrl: './forgot-password-admin.component.html',
  styleUrls: ['./forgot-password-admin.component.scss']
})
export class ForgotPasswordAdminComponent implements OnInit {

  currentYear!: number
  loginForm!: FormGroup;
  submitted = false;
  financialYear!: string | null;
  error: any;
  companyName!: string | null;

  get loginFrm(): { [key: string]: AbstractControl } {
    return this.loginForm.controls;
  }

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private authenticationService: AuthenticationServiceService,
    private spinner: NgxSpinnerService,
    private toastrService: ToastrService
  ) { }

  ngOnInit(): void {
    this.setTheme();
    
    this.loginForm = new FormGroup({
      username: new FormControl('')
    });
    this.loginForm = this.fb.group({
      username: [
        '',
        [Validators.required, Validators.pattern(Constants.EMAIL_REGEXP)],
      ],
    });

    this.currentYear = new Date().getFullYear();
    this.financialYear = sessionStorage.getItem('financialYear');
    this.companyName = sessionStorage.getItem('companyName')
  }

  forgotPassword(): void {
    this.submitted = true;
    if (this.loginForm.valid) {
      let EmailData = {
        "EmailData": {
          "Email": this.loginForm.get('username')?.value,
          "CustomerFinancialYearId": this.financialYear
        }
      }

      this.spinner.show();
      this.authenticationService.sendPassword(EmailData).subscribe({
        next: (res: any) => {
          if (!res.HasErrors && res !== null) {
            this.spinner.hide();
            if (res?.IsSuccess == true) {
              this.toastrService.success('Email sent successfully...');
              this.router.navigate(['/login'])
            } else {
              this.toastrService.error('Email does not exist');
            }
          } else {
            this.toastrService.error(res.Errors[0].Message);
          }
        },
        error: (error: any) => {
          this.spinner.hide();
          this.error = error;
          this.toastrService.error('Something went wrong');
        },
      });
    }
  }

  backToLogin(): void {
    this.router.navigate(['/login'])
  }

  setTheme(): void {
    let themeCode: any = sessionStorage.getItem('themeCode');
    let btns = Array.from(document.getElementsByClassName('btn-primary'));
    for (let x of btns) {
      const y = <HTMLElement>x;
      y.style.background = y.style.borderColor = themeCode;
    }
  }
}
