import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { SuperAdminServiceService } from '../../services/super-admin-service.service'
import { Constants } from 'src/app/share/models/constants';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public showPassword: any;
  currentYear!: number
  loginForm!: FormGroup;
  submitted = false;
  financialYear!: string | null;
  error: any;
  userDetails: any;

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
      username: new FormControl(''),
      password: new FormControl(''),
    });
    this.loginForm = this.fb.group({
      username: [
        '',
        [Validators.required, Validators.pattern(Constants.EMAIL_REGEXP)],
      ],
      password: [
        '',
        [Validators.required],
      ],
    });

    this.currentYear = new Date().getFullYear();
    this.financialYear = sessionStorage.getItem('financialYear')
  }

  login(): void {
    
    this.submitted = true;
    if (this.loginForm.valid) {

      let UserDetails = {
        "UserDetails": {
          "UserEmailId": this.loginForm.get('username')?.value,
          "Password": this.loginForm.get('password')?.value,
        }
      }

      this.spinner.show();
      this.superAdminService.login(UserDetails).subscribe({
        next: (res: any) => {
          if (!res.HasErrors && res !== null) {
            this.spinner.hide();
            if (res?.result?.IsSuccess == true) {
              sessionStorage.setItem('userDetails', JSON.stringify(res));
              this.router.navigate(['/super-admin/dashboard'])
            } else {
              this.toastrService.error('User not found');
            }
          } else {
            this.toastrService.error('Something went wrong');
          }
        }
      });
    }
  }

  forgotPassword(): void {
    this.router.navigate(['forgot-password'])
  }

}
