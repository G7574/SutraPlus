import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IDropdownSettings, } from 'ng-multiselect-dropdown';
import { SuperAdminServiceService } from '../../services/super-admin-service.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { Constants } from 'src/app/share/models/constants';

@Component({
  selector: 'app-add-customer',
  templateUrl: './add-customer.component.html',
  styleUrls: ['./add-customer.component.scss']
})
export class AddCustomerComponent implements OnInit {

  financialYearList: any[] = [];

  dropdownList: any[] = [];
  dropdownSettings: IDropdownSettings = {};
  error: any;
  addCustomer!: FormGroup
  stateList: any;
  submitted = false

  constructor(
    private router: Router,
    private superAdminService: SuperAdminServiceService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private fb: FormBuilder
  ) { }

  get add(): { [key: string]: AbstractControl } {
    return this.addCustomer.controls;
  }

  ngOnInit(): void {
    this.setTheme();
    this.getStateList();
    this.getFinancialYear();

    this.addCustomer = new FormGroup({
      email: new FormControl(''),
      mobile: new FormControl(''),
      lastName: new FormControl(''),
      firstName: new FormControl(''),
      financialYear: new FormControl(''),
      zipCode: new FormControl(''),
      state: new FormControl(''),
      city: new FormControl(''),
      gstIn: new FormControl(''),
      address: new FormControl(''),
      firmName: new FormControl(''),
      code:new FormControl('')
    })
    this.addCustomer = this.fb.group({
      firmName: [
        '',
        [Validators.required],
      ],
      address: [
        '',
        [],
      ],
      gstIn: [
        '',[Validators.required,Validators.pattern(Constants.gstPattern)]
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
      [Validators.required]
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


    // this.dropdownList = [
    //   { item_id: 2021, item_text: '2021-2022' },
    //   { item_id: 2022, item_text: '2022-2023' },
    //   { item_id: 2023, item_text: '2023-2024' },
    //   { item_id: 2024, item_text: '2024-2025' },
    //   { item_id: 2025, item_text: '2025-2026' }
    // ];
    // this.dropdownList = this.financialYearList;
    this.dropdownSettings = {
      idField: 'Year',
      textField: 'FinYear',
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
    };
  }

  backToList(): void {
    this.router.navigate(['/super-admin'])
  }

  setTheme(): void {
    let themeCode: any = sessionStorage.getItem('themeCode');
    let btns = Array.from(document.getElementsByClassName('btn-primary'));
    for (let x of btns) {
      let y = <HTMLElement>x;
      y.style.background = y.style.borderColor = themeCode;
    }
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

  onSubmit(): void {
    this.submitted = true
    if (this.addCustomer.valid) {
      let customerDetails = {
        "Customer": {
          "Name": this.addCustomer.get('firmName')?.value,
          "Address": this.addCustomer.get('address')?.value,
          "City": this.addCustomer.get('city')?.value,
          "State": this.addCustomer.get('state')?.value,
          "Pin": this.addCustomer.get('zipCode')?.value,
          "Mobile": this.addCustomer.get('mobile')?.value,
          "Email": this.addCustomer.get('email')?.value,
          "FirstName": this.addCustomer.get('firstName')?.value,
          "LastName": this.addCustomer.get('lastName')?.value,
          "ContactPerson": this.addCustomer.get('firstName')?.value + ' ' + this.addCustomer.get('lastName')?.value,
          "GSTNo": this.addCustomer.get('gstIn')?.value,
          "Code":this.addCustomer.get('code')?.value
        },
        "YearData" : this.addCustomer.get('financialYear')?.value,
      }
      this.spinner.show()
      this.superAdminService.createCustomer(customerDetails).subscribe({
        next: (res: any) => {
          if (res == 'Customer Added Successfully...!') {
            this.addCustomer.reset();
            this.toastr.success('Customer Created Successfully!');
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

  onReset()
  {
    this.submitted=false;
    this.addCustomer.reset();
    this.addCustomer.value.state = '';
  }
}
