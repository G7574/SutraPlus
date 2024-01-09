import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SuperAdminServiceService } from '../../services/super-admin-service.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Constants } from 'src/app/share/models/constants';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {

  currentYear!: number
  loginForm!: FormGroup;
  submitted = false;
  financialYear!: string | null;
  error: any;

  get loginFrm(): { [key: string]: AbstractControl } {
    return this.loginForm.controls;
  }

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private superAdminService: SuperAdminServiceService,
    private spinner: NgxSpinnerService,
    private toastrService: ToastrService
  ) { }

  ngOnInit(): void {
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
    this.financialYear = sessionStorage.getItem('financialYear')
  }

  sendEmail(): void {
    this.submitted = true;
    if (this.loginForm.valid) {
      let EmailData = {
        "EmailData": {
          "Email": this.loginForm.get('username')?.value,
        }
      }

      this.spinner.show();
      this.superAdminService.sendPassword(EmailData).subscribe({
        next: (res: any) => {
          if (!res.HasErrors && res !== null) {
            this.spinner.hide();
            if (res?.IsSuccess == true) {
              this.toastrService.success('Email sent successfully...');
              this.router.navigate(['/super-admin-login'])
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
    this.router.navigate(['super-admin-login'])
  }

}
