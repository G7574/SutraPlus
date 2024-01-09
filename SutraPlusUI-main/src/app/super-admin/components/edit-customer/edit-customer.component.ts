import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SuperAdminServiceService } from '../../services/super-admin-service.service';
import { IDropdownSettings, } from 'ng-multiselect-dropdown';
import { ToastrService } from 'ngx-toastr';
import { Constants } from 'src/app/share/models/constants';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-edit-customer',
  templateUrl: './edit-customer.component.html',
  styleUrls: ['./edit-customer.component.scss']
})
export class EditCustomerComponent implements OnInit {

  dropdownList: any[] = [];
  dropdownSettings: IDropdownSettings = {};
  error: any;
  editCustomer!: FormGroup;
  changeForm1!: FormGroup;
  changeForm2!: FormGroup;
  isVisibleForm1!: boolean;
  isVisibleForm2!: boolean;
  public visible = false;
  submitted = false;
  customerList: any;
  stateList: any;
  financialYearList: any;
  customerId: any;
  dropdownData:any = [];
  constructor(
    private router: Router,
    private superAdminService: SuperAdminServiceService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) { }

  get add(): { [key: string]: AbstractControl } {
    return this.editCustomer.controls;
  }

  get emailChange(): { [key: string]: AbstractControl } {
    return this.changeForm1.controls;
  }

  get otpChange(): { [key: string]: AbstractControl } {
    return this.changeForm2.controls;
  }

  ngOnInit(): void {
    this.isVisibleForm1 = true
    this.isVisibleForm2 = false
    this.getFinancialYear();

    this.editCustomer = new FormGroup({
      firmName: new FormControl(''),
      address: new FormControl(''),
      gstIn: new FormControl(''),
      city: new FormControl(''),
      state: new FormControl(''),
      zipCode: new FormControl(''),
      financialYear: new FormControl(''),
      firstName: new FormControl(''),
      lastName: new FormControl(''),
      mobile: new FormControl(''),
      email: new FormControl(''),
      code: new FormControl('')
    })
    this.editCustomer = this.fb.group({
      firmName: [
        '',
        [Validators.required],
      ],
      address: [
        '',
        [],
      ],
      gstIn: [
        '',
        [Validators.required,Validators.pattern(Constants.gstPattern)],
      ],
      city: [
        '',
        [Validators.required],
      ],
      state: [
        '',
        [Validators.required],
      ],
      zipCode: [
        '',
        [Validators.required,Validators.pattern(Constants.zipCode)],
      ],
      financialYear: [
        '',
        [Validators.required],
      ],
      firstName: [
        '',
        [Validators.required],
      ],
      lastName: [
        '',
        [Validators.required],
      ],
      mobile: [
        '',
        [Validators.required,Validators.pattern(Constants.PHONE_REGEXP)],
      ],
      email: [
        '',
        [Validators.required, Validators.pattern(Constants.EMAIL_REGEXP)],
      ],
      code: [
        '',
        [Validators.required],
      ]
    })

    this.changeForm1 = new FormGroup({
      changedEmail: new FormControl(''),
    })
    this.changeForm1 = this.fb.group({
      changedEmail: [
        '',
        [Validators.required, Validators.email],
      ],
    })

    this.changeForm2 = new FormGroup({
      otp: new FormControl(''),
    })
    this.changeForm2 = this.fb.group({
      otp: [
        '',
        [Validators.required, Validators.pattern(Constants.OTP_NUMBERS_REGEXP)],
      ],
    })

    // this.dropdownList = [
    //   { item_id: 2021, item_text: '2021-2022' },
    //   { item_id: 2022, item_text: '2022-2023' },
    //   { item_id: 2023, item_text: '2023-2024' },
    // ];
    this.dropdownSettings = {
      idField: 'Year',
      textField: 'FinYear',
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
    };

    this.customerId = this.route.snapshot.paramMap.get('id');
    this.getCustomerById(this.customerId);
    this.getStateList();
  }

  getCustomerById(customerId: any) {
    this.spinner.show();
    this.superAdminService.getCustomerByID(customerId).subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
         
          let financialYear = res.FinancialYear; 
          
          for(let data of financialYear)
          {
             let temp = data.Description.split('-'); 
             this.dropdownData.push({"Year":temp[0],"FinYear":data.Description});
          }

          let list = res.CustomerSingleList;
          this.setData(list)
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

  setData(customerDetails: any) { 
    if (customerDetails) {
      this.editCustomer.controls['firmName'].setValue(customerDetails[0].Name);
      this.editCustomer.controls['address'].setValue(customerDetails[0].Address);
      this.editCustomer.controls['gstIn'].setValue(customerDetails[0].GSTNo);
      this.editCustomer.controls['city'].setValue(customerDetails[0].City);
      this.editCustomer.controls['state'].setValue(customerDetails[0].State);
      this.editCustomer.controls['zipCode'].setValue(customerDetails[0].Pin);
      this.editCustomer.controls['financialYear'].setValue(this.dropdownData);
      this.editCustomer.controls['firstName'].setValue(customerDetails[0].FirstName);
      this.editCustomer.controls['lastName'].setValue(customerDetails[0].LastName);
      this.editCustomer.controls['mobile'].setValue(customerDetails[0].Mobile);
      this.editCustomer.controls['email'].setValue(customerDetails[0].Email);
      this.editCustomer.controls['code'].setValue(customerDetails[0].Code);
    }
  }

  backToList(): void {
    this.router.navigate(['/super-admin/'])
  }

  sendOTP(): void {
    this.submitted = true
    if (this.changeForm1.valid) {

      let OTPGenerate = {
        "OTPGenerate": {
          "CustomerId": this.customerId,
          "NewMail": this.changeForm1.get('changedEmail')?.value,
        }
      }

      this.spinner.show();
      this.superAdminService.sendOtp(OTPGenerate).subscribe({
        next: (res: any) => {
          if (!res.HasErrors && res !== null) {
            this.spinner.hide();
            this.isVisibleForm1 = false
            this.isVisibleForm2 = true
          } else {
            this.toastr.error(res.Errors[0].Message);
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

  toggleLiveDemo(): void {
    this.visible = !this.visible;
    this.isVisibleForm1 = true;
    this.isVisibleForm2 = false;
    this.changeForm1.reset();
    this.changeForm2.reset();
    this.submitted = false
  }

  handleLiveDemoChange(event: any) {
    this.visible = event;
  }

  getStateList(): void {
    this.superAdminService.getStates().subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          this.stateList = res.StateList
        } else {
          this.toastr.error(res.Errors[0].Message);
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

  getFinancialYear(): void {
    this.superAdminService.getFinancialYears().subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          this.financialYearList = this.dropdownList = res.FinancialYear
        } else {
          this.toastr.error('Financial years not found');
          this.error = res.Errors[0].Message;
        }
      },
      error: (error: any) => {
        // this.spinner.hide();
        this.error = error;
        this.toastr.error('Something went wrong');
      },
    });
  }

  verifyOTP(): void {
    this.submitted = true
  }

  update(): void {
    this.submitted = true;
    if (this.editCustomer.valid) {
      let customerDetails = {
        "UpdateCustomer": {
          "Id": this.customerId, 
          "Mobile": this.editCustomer.get('mobile')?.value,
          "FirstName": this.editCustomer.get('firstName')?.value,
          "LastName": this.editCustomer.get('lastName')?.value,
          "Address": this.editCustomer.get('address')?.value
        }
      }
      this.spinner.show()
      this.superAdminService.editCustomer(customerDetails).subscribe({
        next: (res: any) => {
          if (res == true) {
            this.editCustomer.reset();
            this.toastr.success('Customer Updated Successfully!');
            this.router.navigate(['/super-admin']);
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

}
