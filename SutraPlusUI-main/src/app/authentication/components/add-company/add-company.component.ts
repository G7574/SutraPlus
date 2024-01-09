import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationServiceService } from '../../services/authentication-service.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Constants } from 'src/app/share/models/constants';
import { SuperAdminServiceService } from 'src/app/super-admin/services/super-admin-service.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-add-company',
  templateUrl: './add-company.component.html',
  styleUrls: ['./add-company.component.scss']
})
export class AddCompanyComponent implements OnInit {

  currentYear!: number
  financialYear!: string | null;
  companyList: any
  error: any;
  companyName!: string | null;
  addCompany!: FormGroup
  submitted = false;
  stateList: any;
  file!: File;
  imageSource: any;
  imageSrc!: string | null;

  constructor(
    private router: Router,
    private authenticationService: AuthenticationServiceService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private fb: FormBuilder,
    private superAdminService: SuperAdminServiceService
  ) { }

  get addCmp(): { [key: string]: AbstractControl } {
    return this.addCompany.controls;
  }

  ngOnInit(): void {
    this.setTheme();
    this.currentYear = new Date().getFullYear();
    this.financialYear = sessionStorage.getItem('financialYear');
    this.companyName = sessionStorage.getItem('companyName');
    // this.imageSource = '../assets/images/upload-image.jpg'
    this.imageSource = '../assets/images/addCompanyLogo.svg'

    this.addCompany = new FormGroup({
      kannadaName: new FormControl(''),
      shree: new FormControl(''),
      companyName: new FormControl(''),
      natureOfBusiness: new FormControl(''),
      address: new FormControl(''),
      pan: new FormControl(''),
      tan: new FormControl(''),
      apmcCode: new FormControl(''),
      place: new FormControl(''),
      fln: new FormControl(''),
      bin: new FormControl(''),
      district: new FormControl(''),
      email: new FormControl(''),
      state: new FormControl(''),
      cellPhone: new FormControl(''),
      contactNo: new FormControl(''),
      gstin: new FormControl(''),
      firmCode: new FormControl(''),
      shopCode: new FormControl(''),
      iec: new FormControl(''),
      bankName1: new FormControl(''),
      bankName2: new FormControl(''),
      bankName3: new FormControl(''),
      ifsc1: new FormControl(''),
      ifsc2: new FormControl(''),
      ifsc3: new FormControl(''),
      accountNo1: new FormControl(''),
      accountNo2: new FormControl(''),
      accountNo3: new FormControl(''),
    });
    this.addCompany = this.fb.group({
      kannadaName: [''],
      shree: [''],
      companyName: [
        '',
        [Validators.required],
      ],
      natureOfBusiness: [''],
      address: [
        '',
        [Validators.required],
      ],

      pan: [
        '',
        [Validators.required],
      ],
      tan: [''],
      apmcCode: [''],
      place: [''],
      fln: [''],

      bin: [''],
      district: [
        '',
        [Validators.required],
      ],
      email: [
        '',
        [Validators.required, Validators.pattern(Constants.EMAIL_REGEXP)],
      ],
      state: [
        '',
        [Validators.required],
      ],
      cellPhone: [''],

      contactNo: [
        '',
        [Validators.required],
      ],
      gstin: [
        '',
        [Validators.required],
      ],
      firmCode: [''],
      shopCode: [''],
      iec: [''],

      bankName1: [
        '',
        [Validators.required],
      ],
      ifsc1: [
        '',
        [Validators.required],
      ],
      accountNo1: [
        '',
        [Validators.required],
      ],
      bankName2: [''],
      ifsc2: [''],
      accountNo2: [''],
      bankName3: [''],
      ifsc3: [''],
      accountNo3: [''],
    });

    this.getStateList();
  }

  getData() {
    this.spinner.show();
    this.authenticationService.getCompanyList(1).subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          this.companyList = res
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
        alert(error)
      },
    });
  }

  setTheme(): void {
    let themeCode: any = sessionStorage.getItem('themeCode');
    let btns = Array.from(document.getElementsByClassName('btn-primary'));
    for (let x of btns) {
      let y = <HTMLElement>x;
      y.style.background = y.style.borderColor = themeCode;
    }
  }

  addNew(): void {
    this.router.navigate(['/new-company'])
  }

  backToCompany(): void {
    this.router.navigate(['/companies'])
  }

  onSubmit(): void {
    this.submitted = true
    if (this.addCompany.valid) {
      let companyDetails = {
        "CompanyData": {
          "KannadaName": this.addCompany.get('kannadaName')?.value,
          "Shree": this.addCompany.get('shree')?.value,
          "CompanyName": this.addCompany.get('companyName')?.value,
          "Title": this.addCompany.get('natureOfBusiness')?.value,
          "AddressLine1": this.addCompany.get('address')?.value,
          "Pan": this.addCompany.get('pan')?.value,
          "Tan": this.addCompany.get('tan')?.value,
          "Place": this.addCompany.get('place')?.value,
          "Fln": this.addCompany.get('fln')?.value,
          "Bin": this.addCompany.get('bin')?.value,
          "District": this.addCompany.get('district')?.value,
          "Email": this.addCompany.get('email')?.value,
          "State": this.addCompany.get('state')?.value,
          "CellPhone": this.addCompany.get('cellPhone')?.value,
          "ContactDetails": this.addCompany.get('contactNo')?.value,
          "Gstin": this.addCompany.get('gstin')?.value,
          "FirmCode": this.addCompany.get('firmCode')?.value,
          "Apmccode": this.addCompany.get('apmcCode')?.value,
          "Iec": this.addCompany.get('iec')?.value,
          "Bank1": this.addCompany.get('bankName1')?.value,
          "Ifsc1": this.addCompany.get('ifsc1')?.value,
          "AccountNo1": this.addCompany.get('accountNo1')?.value,
          "Bank2": this.addCompany.get('bankName2')?.value,
          "Ifsc2": this.addCompany.get('ifsc2')?.value,
          "AccountNo2": this.addCompany.get('accountNo2')?.value,
          "Bank3": this.addCompany.get('bankName3')?.value,
          "Ifsc3": this.addCompany.get('ifsc3')?.value,
          "Account3": this.addCompany.get('accountNo3')?.value,
          "Logo":
            this.imageSource.changingThisBreaksApplicationSecurity !== undefined
              ? this.imageSource.changingThisBreaksApplicationSecurity.split(
                ','
              )[1]
              : this.imageSource.split(',')[1],
        }
      }

      this.spinner.show()
      this.authenticationService.createCompany(companyDetails).subscribe({
        next: (res: any) => {
          if (res == true) {
            this.addCompany.reset();
            this.spinner.hide();
            this.toastr.success('Company Created Successfully!');
            this.router.navigate(['/companies']);
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

  getStateList(): void {
    this.superAdminService.getStates().subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          this.stateList = res.StateList
        } else {
          this.toastr.error('Something went wrong');
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

  onSelectFile(event: any) {
    this.file = event.target.files[0];
    if (
      this.file.type === 'image/png' ||
      this.file.type === 'image/jpg' ||
      this.file.type === 'image/jpeg' ||
      this.file.type === 'image/svg' ||
      this.file.type === 'image/webp'
    ) {
      if (this.file.size < 500000) {
        const reader = new FileReader();
        reader.readAsDataURL(event.target.files[0]);
        reader.onload = (event: any) => {
          const image = new Image();
          image.src = event.target.result;

          image.onload = (res: any) => {
            const imgBase64Path = event.target.result;

            this.imageSource = imgBase64Path;
          };
        };
      } else {
        Swal.fire({
          title: 'Error',
          text: 'Image exceeds maximum 500kb limit',
          icon: 'error',
          confirmButtonColor: '#3085d6',
          confirmButtonText: 'OK',
        });
      }
    } else {
      Swal.fire({
        title: 'Error',
        text: 'Image format not support (supported formats are jpg, jpeg and png)',
        icon: 'error',
        confirmButtonColor: '#3085d6',
        confirmButtonText: 'OK',
      });
    }
  }
}
