import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationServiceService } from 'src/app/authentication/services/authentication-service.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent {


  oldPassword: string = '';
  newPassword: string = '';
  confirmPassword: string = '';



  public showPassword: any;
  passwordChangeForm: FormGroup;


  constructor(private fb: FormBuilder,private toastrService: ToastrService, private spinner: NgxSpinnerService,private authenticationService: AuthenticationServiceService,private router: Router,
    ) {

    this.passwordChangeForm = this.fb.group({
      oldPassword: ['', Validators.required],
      newPassword: ['', [Validators.required]],
      confirmPassword: ['', Validators.required]
    }, { validator: this.passwordMatchValidator });
  }
  detailshows: any;

  passwordMatchValidator(group: FormGroup) {
    const newPassword = group.get('newPassword').value;
    const confirmPassword = group.get('confirmPassword').value;
    return newPassword === confirmPassword ? null : { mismatch: true };
  }

  onSubmit() {
      this.spinner.show();
      let model = {
        Email:sessionStorage.getItem("Email"),
        NewPassword:this.passwordChangeForm.get('newPassword').value,
        oldPassword:this.passwordChangeForm.get('oldPassword').value
      }
      let Passmodel={
        UserDetails:model
      }
      this.authenticationService.ChangePassword(Passmodel).subscribe({
        next: (res: any) => {
          this.spinner.hide();
            if (res == true) {
              this.toastrService.success('Password Updated Successfully.');
            } else {
              this.toastrService.error('Old and new password not same.');
            }
          }

      });
    }

  changePassword(detailshow: any){

  }

}
