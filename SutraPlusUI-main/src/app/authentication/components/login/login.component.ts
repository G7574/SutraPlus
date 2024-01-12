import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { Constants } from '../../../share/models/constants';
import { AuthenticationServiceService } from '../../services/authentication-service.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { CommonService } from 'src/app/share/services/common.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  public showPassword: any;
  currentYear!: number;
  loginForm!: FormGroup;
  submitted = false;
  financialYear!: string | null;
  error: any;
  companyName!: string | null;
  userDetails: any;

  get loginFrm(): { [key: string]: AbstractControl } {
    return this.loginForm.controls;
  }

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private authenticationService: AuthenticationServiceService,
    private spinner: NgxSpinnerService,
    private toastrService: ToastrService,
    public commonService: CommonService
  ) {}

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      username: new FormControl(''),
      password: new FormControl(''),
    });
    this.loginForm = this.fb.group({
      username: [
        '',
        [Validators.required, Validators.pattern(Constants.EMAIL_REGEXP)],
        // [Validators.required],
      ],
      password: ['', [Validators.required]],
    });

    this.currentYear = new Date().getFullYear();
    this.financialYear = sessionStorage.getItem('financialYear');
    this.companyName = sessionStorage.getItem('companyName');
  }

  ngAfterViewInit() {
    this.commonService.setTheme();
  }

  login(): void {
    this.submitted = true;
    if (this.loginForm.valid) {
      let UserDetails = {
        UserDetails: {
          UserEmailId: this.loginForm.get('username')?.value,
          Password: this.loginForm.get('password')?.value,
        },
      };


      this.spinner.show();
      this.authenticationService.login(UserDetails).subscribe({
        next: (res: any) => {
          this.spinner.hide();
          if (!res.HasErrors && res !== null) {
            if (res?.result?.IsSuccess == true) {
              sessionStorage.setItem('userPassword', this.loginForm.get('password')?.value);
              sessionStorage.setItem('Email', this.loginForm.get('username')?.value);
              sessionStorage.setItem('userDetails', JSON.stringify(res));
              this.router.navigate(['/companies']);
            } else {
              this.toastrService.error('User Not Found. Please try again');
            }
          } else {
            this.toastrService.error('Something went wrong');
          }
        },
        error: (error) => {
          this.spinner.hide();
          this.error = error;
          this.toastrService.error('Something went wrong');
        },
      });
    }
  }

  forgotPassword(): void {
    this.router.navigate(['/forgot-password-admin']);
  }

  backToCompany(): void {
    let custCode = sessionStorage.getItem('globalCustomerCode');

    this.router.navigate(['/landing-page/' + custCode]);
  }
}
