import { Component } from '@angular/core';
import { data } from 'jquery';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { DataService } from 'src/app/core/services/data.service';
import { SuperAdminServiceService } from 'src/app/super-admin/services/super-admin-service.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent {
  user = {
    firstName: '',
    lastName: '',
    mobileNumber: '',
    photoUrl: ''
  };

  photoPath: string = "";

  constructor(private dataService: DataService, private spinner: NgxSpinnerService, private superAdminService: SuperAdminServiceService, private toastrService: ToastrService) {}

  ngOnInit() {
    this.getUser();
  }

  getUser() {
    this.spinner.show();
    const email = sessionStorage.getItem('Email');
    let model ={
      UserEmailId :email
    }
    const userData = {
      UserDetails:model
    };

    this.superAdminService.getUser(userData).subscribe(
      (response) => {
        this.user = {
          firstName: response?.FirstName || '',
          lastName: response?.LastName || '',
          mobileNumber: response?.PhoneNo || '',
          photoUrl: '',
        };
        this.photoPath = environment.api + response?.ProfileImage
        console.log('User data retrieved successfully:', this.user);
        console.log('Photo Path:', this.photoPath);
        this.spinner.hide();
      },
      (error) => {
        console.error('Error retrieving user data:', error);
        this.toastrService.error('Error retrieving user data');
        this.spinner.hide();
      }
    );
  }

  updateProfile() {
    this.spinner.show();
    const email = sessionStorage.getItem('Email');

    let user = {
      UserEmailId:email,
      firstName: this.user.firstName,
      lastName: this.user.lastName,
      mobileNumber: this.user.mobileNumber,
      profileImage: this.user.photoUrl
    };

    const userData = {
      UserDetails:user
    };

    this.superAdminService.updateUser(userData).subscribe(
      (response) => {
        console.log('User data Updated successfully:', this.user);
        this.toastrService.success('User Data Updated Successfully');
        this.spinner.hide();
      },
      (error) => {
        console.error('Error updating user data:', error);
        this.toastrService.error('Error Updating User Data');
        this.spinner.hide();
      }
    );

  }

  onPhotoChange(event: any) {
    const fileInput = event.target;
    const files = fileInput.files;

    if (files && files.length > 0) {
      const selectedFile = files[0];
      const reader = new FileReader();

      reader.onload = (e: any) => {
        //this.user.photoUrl = e.target.result;
        this.photoPath = e.target.result;
        const base64String = e.target.result.split(',')[1];
        this.user.photoUrl = base64String;
      };

      reader.readAsDataURL(selectedFile);
    }
  }
}
