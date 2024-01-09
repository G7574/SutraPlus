import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AdminServicesService } from '../../services/admin-services.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Location } from '@angular/common';
import { CommonService } from '../../../share/services/common.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.scss']
})
export class AddProductComponent implements OnInit {

  submitted = false
  addProduct!: FormGroup
  unitList: any;
  error: any;
  financialYear!: string | null;
  customerCode!: string | null;
  userDetails: any;
  userEmail!: string | null;
  isTrading: boolean = false;
  deductTds: boolean = false;
  isService: boolean = false;
  deductItem: boolean = false;

  ErrorMsg: string = '';
  flag: boolean = true;


  constructor(
    private router: Router,
    private fb: FormBuilder,
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private location: Location,
    public commonService: CommonService
  ) { }

  get add(): { [key: string]: AbstractControl } {
    return this.addProduct.controls;
  }

  ngOnInit(): void {
    this.addProduct = new FormGroup({
      itemName: new FormControl(''),
      units: new FormControl(''),
      hsn: new FormControl(''),
      igst: new FormControl(0),
      sgst: new FormControl(0),
      cgst: new FormControl(0),
      openingStock: new FormControl(0),
      value: new FormControl(0),
      isTrading: new FormControl(false),
      deductTds: new FormControl(false),
      isService: new FormControl(false),
      deductItem: new FormControl(false),

    })
    this.addProduct = this.fb.group({
      itemName: [
        '',
        [Validators.required],
      ],
      units: [
        '',
        [Validators.required],
      ],

      hsn: [
        ''
      ],
      igst: [
        0,
        [Validators.required],
      ],

      sgst: [
        { value: 0, disabled: true },
        [Validators.required],
      ],
      cgst: [
        { value: 0, disabled: true },
        [Validators.required],
      ],

      openingStock: [
        0
      ],
      value: [
       
        0
      ],
      isTrading: [
        false
      ]
      ,
       deductTds: [
        false
      ],
       isService: [
        false
      ],
       deductItem: [
        false
      ],
    })

    this.financialYear = sessionStorage.getItem('financialYear');
    this.customerCode = sessionStorage.getItem('globalCustomerCode');
    this.userDetails = sessionStorage.getItem('userDetails')
    this.userDetails = JSON.parse(this.userDetails);
    this.userEmail = this.userDetails?.result?.UserEmailId;
    this.getUnitsList();
  }

  back(): void {
    // this.router.navigate(['/admin/product-master'])
    this.location.back();
  }


  chkValidations()
  {
      
      this.flag = true;
      this.ErrorMsg = '';
      if(parseFloat(this.addProduct.value.igst) == 18 || parseFloat(this.addProduct.value.igst) == 5 || parseFloat(this.addProduct.value.igst) == 28 || parseFloat(this.addProduct.value.igst) == 12 || parseFloat(this.addProduct.value.igst) == 0)
      {
        this.ErrorMsg += "";
        this.flag = true;
      }
      else
      {
        this.ErrorMsg = "Invalid GST Rate,";
        this.flag = false;
      }

      if(this.isTrading == false && this.isService == false)
      {
        this.ErrorMsg = "Select Trading or Service,";
        this.flag = false;
      }

      if(this.isService == true)
      {
          if( parseFloat(this.addProduct.get('openingStock')?.value) > 0 || parseFloat(this.addProduct.get('value')?.value) > 0)
          {
            this.ErrorMsg = "Opening Stock/Value not allowed for Service.,";
            this.flag = false;
          }
      }

      return this.flag;
  }

  getUnitsList(): void {
    let units = {
      "Units": {
        "CustomerFinancialYearId": this.financialYear,
        "CustomerCode": this.customerCode
      }
    }
    this.adminService.getUnitsList(units).subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          this.unitList = res.UnitDropDownList
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
    
    this.submitted = true;
    if (this.addProduct.valid) {

      if(this.chkValidations() == true)
      {
        
        
        
      let customerDetails = {
        "ItemProduct": {
          "CommodityName": this.addProduct.get('itemName')?.value,
          "HSN": this.addProduct.get('hsn')?.value,
          "MOU": this.addProduct.get('units')?.value,
          "IGST": this.addProduct.get('igst')?.value,
          "SGST": this.addProduct.get('sgst')?.value,
          "CGST": this.addProduct.get('cgst')?.value,
          "OpeningStock": this.addProduct.get('openingStock')?.value,
          "OBVAL": this.addProduct.get('value')?.value,
          "IsTrading":this.isTrading,
          "DeductTDS": this.addProduct.get('deductTds')?.value,
          "IsService": this.isService,
          "DeductItem": this.addProduct.get('deductItem')?.value,
 
        }
      }
      this.spinner.show()
    
      this.adminService.createProduct(customerDetails).subscribe({
        next: (res: any) => {
          if (res == 'Added') {
            this.toastr.success('Product Created Successfully!');
            // this.router.navigate(['/super-admin']);              
            this.spinner.hide();
            this.location.back();
          }
          else
          {
            this.spinner.hide();
            this.toastr.error(res);            
          }
        },
        error: (error: any) => {
          this.spinner.hide();
          this.error = error;
          this.toastr.error('Something went wrong');
        },
      });
    
    }
    else
    {
      this.toastr.error(this.ErrorMsg.slice(0, -1) );
    }
  }
  }





  changeStatus(str: any): void {
    if (str == 'Trading Item') {
      this.isTrading = true;
      this.isService = false;
      this.deductItem = false;
      this.deductTds = false;
    } else if (str == 'Its Service') {
      this.isService = true;
      this.isTrading = false;
      this.deductItem = false;
      this.deductTds = false;
    } else if (str == 'Deduct Item') {
      this.deductItem = true;
      this.isService = false;
      this.isTrading = false;
      this.deductTds = false;
    } else if (str == 'Deduct 34C TDS') {
      this.deductTds = true;
      this.deductItem = false;
      this.isService = false;
      this.isTrading = false;
    }
  }


  onChangeIGST(igst: string) {
    let num = Number(igst);
    if (igst.charAt(0) !== '.') {
      this.addProduct.controls['sgst'].setValue(num / 2);
      this.addProduct.controls['cgst'].setValue(num / 2);
    }
  }

  onChangeSGST() {
    this.addProduct.controls['cgst'].setValue(this.addProduct.value.sgst);
    let sum = Number(this.addProduct.value.sgst) + Number(this.addProduct.value.cgst);
    this.addProduct.value.sgst.charAt(0) !== '.' ? this.addProduct.controls['igst'].setValue(sum) : '';
  }

  onChangeCGST() {
    this.addProduct.controls['sgst'].setValue(this.addProduct.value.cgst);
    let sum = Number(this.addProduct.value.sgst) + Number(this.addProduct.value.cgst);
    this.addProduct.value.cgst.charAt(0) !== '.' ? this.addProduct.controls['igst'].setValue(sum) : '';
  }

  ifOpeningStockValue() {
    const openstock = Number(this.addProduct.value.openingStock);
    let valueCtrl = this.addProduct.get('value');
    if (openstock > 0) {
      valueCtrl?.setValidators(Validators.required)
    } else {
      valueCtrl?.clearValidators();
    }
    valueCtrl?.updateValueAndValidity();
    return openstock > 0;

  }
}
