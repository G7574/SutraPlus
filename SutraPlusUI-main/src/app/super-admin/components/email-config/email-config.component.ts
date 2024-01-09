import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AdminServicesService } from 'src/app/admin/services/admin-services.service';
import { Constants } from 'src/app/share/models/constants';
import { Location } from '@angular/common';
import { SuperAdminServiceService } from '../../services/super-admin-service.service';

@Component({
  selector: 'app-email-config',
  templateUrl: './email-config.component.html',
  styleUrls: ['./email-config.component.scss']
})

export class EmailConfigComponent {

  financialYear!: string | null;
  customerCode!: string | null;
  userDetails!: any;
  userEmail: any;
  globalCompanyId!: string | null;
  formList: any;
  groupNames: any = [];
  formNames: any;
  uniqueGroupNames: any = [];
  groupingData: { "groupName": string, "formData": { "formId": number, "formName": string }[], "accessId": number[] }[] = [];

  submitted: boolean = false;
  error: any;
  accessId: number[] = [];
  accessModuleEnabled: boolean = false;

  emailConfig!: FormGroup;
  hidePassword = true;

  constructor(
    private location: Location,
    private router: Router,
    private superAdminService: SuperAdminServiceService,
    private spinner: NgxSpinnerService,
    private fb: FormBuilder,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {

    this.emailConfig = new FormGroup({
      fromEmail: new FormControl(''),
      password: new FormControl(''),
      emailServerHost: new FormControl(''),
      emailServerPort: new FormControl(''),
    });

    this.emailConfig = this.fb.group({
      fromEmail: ['', [Validators.required, Validators.pattern(Constants.EMAIL_REGEXP)]],
      password: ['', Validators.required],
      emailServerHost: ['', Validators.required],
      emailServerPort: ['', Validators.required]
    });

    this.setTheme();
    this.financialYear = sessionStorage.getItem('financialYear');
    this.customerCode = sessionStorage.getItem('globalCustomerCode');
    this.userDetails = sessionStorage.getItem('userDetails');
    this.userDetails = JSON.parse(this.userDetails);
    this.userEmail = this.userDetails?.result?.UserEmailId;
    this.globalCompanyId = sessionStorage.getItem('companyID');
    this.getEmailConfig();
  }

  setTheme(): void {
    let themeCode: any = sessionStorage.getItem('themeCode');
    let btns = Array.from(document.getElementsByClassName('btn-primary'));
    for (let x of btns) {
      const y = <HTMLElement>x;
      y.style.background = y.style.borderColor = themeCode;
    }
  }

  back(): void {
    this.router.navigate(['/super-admin/dashboard']);
  }

  getEmailConfig() {
    this.superAdminService.getEmailConfig().subscribe({
      next: (res: any) => {
        console.log("email config", res);
        this.spinner.show();

        // Extract the email configuration from the response
        const emailConfig = res.result;
        this.emailConfig.patchValue({
          fromEmail: emailConfig.FromEmail,
          password: emailConfig.Password,
          emailServerHost: emailConfig.EmailServerHost,
          emailServerPort: emailConfig.EmailServerPort
        });

        this.spinner.hide();
      },
      error: (error: any) => {
        console.error("error getting email config", error);
        this.spinner.hide();
      }
    })
  }


  getFormsNames(groupName: string) {
    let arrayofFormName: { "formId": number, "formName": string }[] = [];
    for (let data of this.formList) {
      if (groupName === data.GroupName) {
        arrayofFormName.push({ "formId": data.FormId, "formName": data.FormName });
      }
    }
    this.groupingData.push({ "groupName": groupName, "formData": arrayofFormName, "accessId": [] });
  }

  onSubmit(): void {
    this.submitted = true;
    if (this.emailConfig.valid) {

      let emailConfig = {
        "EmailConfig": {
          "FromEmail": this.emailConfig.get('fromEmail')?.value,
          "Password": this.emailConfig.get('password')?.value,
          "EmailServerHost": this.emailConfig.get('emailServerHost')?.value,
          "EmailServerPort": this.emailConfig.get('emailServerPort')?.value,
        },
        "AccessData": this.accessId
      }

      this.spinner.show()
      this.superAdminService.saveEmailConfig(emailConfig).subscribe({
        next: (res: any) => {
          console.log("saved email", res);
          if (res.status) {
            this.toastr.success('Email Configuration Saved Successfully!');
            // this.location.back();
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

  reset() {
    this.submitted = false;
    this.emailConfig.reset();
    this.accessModuleEnabled = true;
    this.groupingData = [];
    this.accessId = [];
    this.getEmailConfig();
  }
}
