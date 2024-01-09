import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Location } from '@angular/common';
import { AdminServicesService } from '../../services/admin-services.service';
import { CommonService } from 'src/app/share/services/common.service';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.scss']
})
export class EditProductComponent implements OnInit {

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
  productId: any;

  ErrorMsg: string = '';
  flag: boolean = true;

  chnge_isTrading: boolean = false;
  chnge_deductTds: boolean = false;
  chnge_isService: boolean = false;
  chnge_deductItem: boolean = false;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private location: Location,
    private route: ActivatedRoute,
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
      trading: new FormControl(false),
      itsService: new FormControl(false),
      deductItem: new FormControl(false),
      deductTds: new FormControl(false)
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
        0,
        [Validators.required],
      ],
      cgst: [
        0,
        [Validators.required],
      ],

      openingStock: [
        0,
        [Validators.required],
      ],
      value: [
        0,
        [Validators.required],
      ],
      trading: [false],
      itsService: [false],
      deductItem: [false],
      deductTds: [false]
    })

    this.productId = this.route.snapshot.paramMap.get('id');

    this.financialYear = sessionStorage.getItem('financialYear');
    this.customerCode = sessionStorage.getItem('globalCustomerCode');
    this.userDetails = sessionStorage.getItem('userDetails')
    this.userDetails = JSON.parse(this.userDetails);
    this.userEmail = this.userDetails?.result?.UserEmailId;

    this.getUnitsList();
    this.getProductById();
  }


  chkValidations()
  {
      
      this.flag = true;
      this.ErrorMsg = '';
      if(parseFloat(this.addProduct.value.igst) == 18 || parseFloat(this.addProduct.value.igst) == 5 || parseFloat(this.addProduct.value.igst) == 28 || parseFloat(this.addProduct.value.igst) == 12 || parseFloat(this.addProduct.value.igst) == 0)
      {
        this.ErrorMsg += "";        
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



  getProductById() {
    let UserDetails = {
      "ItemProduct": {
        "_Id": this.productId
      }
    }
    this.spinner.show();
    this.adminService.getProductById(UserDetails).subscribe({
      next: (res: any) => {
        this.spinner.hide();
        if (!res.HasErrors && res?.Data !== null) {
          let singleItem = res.ProductIte
          this.setData(singleItem)
        } else {
          this.toastr.error('Product not found');
          this.error = res.Errors[0].Message;
        }
      },
      error: (error: any) => {
        this.spinner.hide();
        this.error = error;
        this.toastr.error('Something went wrong');
      },
    });
  }

  setData(singleItem: any): void {
    
    if (singleItem) {
      this.addProduct.controls['itemName'].setValue(singleItem[0].CommodityName);
      this.addProduct.controls['units'].setValue(singleItem[0].Mou);
      this.addProduct.controls['hsn'].setValue(singleItem[0].HSN);
      this.addProduct.controls['igst'].setValue(singleItem[0].IGST);
      this.addProduct.controls['sgst'].setValue(singleItem[0].SGST);
      this.addProduct.controls['cgst'].setValue(singleItem[0].CGST);
      this.addProduct.controls['openingStock'].setValue(singleItem[0].OpeningStock);
      this.addProduct.controls['value'].setValue(singleItem[0].Obval);
      this.addProduct.controls['trading'].setValue(singleItem[0].IsTrading);
      this.addProduct.controls['itsService'].setValue(singleItem[0].IsService);
      this.addProduct.controls['deductItem'].setValue(singleItem[0].IsVikriCommodity);
      this.addProduct.controls['deductTds'].setValue(singleItem[0].DeductTds);

      this.isTrading = singleItem[0].IsTrading == true ? true : false
      this.isService = singleItem[0].IsService == true ? true : false
      this.deductItem = singleItem[0].IsVikriCommodity == true ? true : false
      this.deductTds = singleItem[0].DeductTds == true ? true : false
    }
  }

  back(): void {
    // this.router.navigate(['/admin/product-master'])
    this.location.back();
  }

  checkboxcheck(event:any,CallBy : string)
  {
    
    const checked = event.target.checked;
    if(CallBy == 'deductItem')
    {
          this.deductItem = checked;
          this.addProduct.controls['deductItem'].setValue(checked);
    }

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
          "_Id": this.productId,
          "CommodityName": this.addProduct.get('itemName')?.value,
          "HSN": this.addProduct.get('hsn')?.value,
          "MOU": this.addProduct.get('units')?.value,
          "IGST": this.addProduct.get('igst')?.value,
          "SGST": this.addProduct.get('sgst')?.value,
          "CGST": this.addProduct.get('cgst')?.value,
          "OpeningStock": this.addProduct.get('openingStock')?.value,
          "OBVAL": this.addProduct.get('value')?.value,
          "IsTrading": this.isTrading,
          "DeductTDS": this.addProduct.get('deductTds')?.value,
          "IsService": this.isService,
          "DeductItem": this.addProduct.get('deductItem')?.value
        }
      }
      this.spinner.show()
      this.adminService.updateProduct(customerDetails).subscribe({
        next: (res: any) => {
          
          if (res == 'Updated') {
            this.addProduct.reset();
            this.toastr.success('Product Updated Successfully!');
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
      
    } else if (str == 'Its Service') {
      this.isService = true;
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


}