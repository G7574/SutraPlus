import { Component } from '@angular/core';
import { DCLineItem } from '../../models/dc-line-item.model';
import { DCSummary } from '../../models/dc-summary.model';
import { AdminServicesService } from '../../../../services/admin-services.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
/**
 *
 */

@Component({
  selector: 'app-create-dc',
  templateUrl: './create-dc.component.html',
  styleUrls: ['./create-dc.component.scss'],
})

export class CreateDcComponent {
  formData: DCSummary = new DCSummary();
  BNo:number = 0;
  dcLineItem:DCLineItem=new DCLineItem();
  dcProperties: string[];
  dcLineItemProperties: string[];
  dcLineItems: DCLineItem[] = [];
  globalCompanyId: string = "";
  FeedBack: FormGroup;
  constructor(private fb: FormBuilder,private adminService:AdminServicesService,private toastr: ToastrService){};

  ngOnInit(): void {
   // For simplicity, you can manually list properties or dynamically fetch them from your model
   this.getBNo();
    this.dcProperties = Object.keys(this.formData);
    this.dcLineItemProperties = Object.keys(new DCLineItem());
    this.globalCompanyId = sessionStorage.getItem('companyID');
    this.FeedBack = this.fb.group({
    Rows: this.fb.array([this.createRow()])
     
    });
    
  }
  currentYear = new Date().getFullYear();
  createRow(): FormGroup {
    return this.fb.group({
      BNo: ['', Validators.required],
      InvoiceNumber: [null, Validators.required],
      Description: ['', Validators.required],
      Total: [null, Validators.required],
      PrivateMark: ['', Validators.required],
      ChangedWeightKg: [null, Validators.required],
      ActualWeightKg: [null, Validators.required],
    });
  }
  get formArr() {
    return this.FeedBack.get('Rows') as FormArray;
  }
  addNewRow() {
    this.formArr.push(this.createRow());
  }
  onSubmit(){

  }
  deleteRow(index: number) {
    if (this.formArr.length > 1) {
      this.formArr.removeAt(index);
    }
  }
  // Set the minimum and maximum dates to the first and last day of the current year
  minDate = `${this.currentYear}-01-01`;
  maxDate = `${this.currentYear}-12-31`;


  getBNo(){
    let CompanyDetails = {
      CompanyId: Number(sessionStorage.getItem('companyID')),
    };

    this.adminService.GetBNo(CompanyDetails).subscribe({
      next:(res:any)=>{
        this.formData.BNo=res;
      },
      error:(error:any)=>{
        this.toastr.error('Bno get error');
      },
    });
   return 0;
  }

  saveLineItem(){
    const data = this.dcLineItem;
    
    this.adminService.saveDC(data).subscribe({
      next: (res: any) => {
        if (res === 'Success') {
          this.toastr.success('User Added Successfully!');
        }
      },
      error: (error: any) => {
        this.toastr.error('Something went wrong');
      },
    });
    console.log("save dc form data", data);
    console.log("on saveLineItem form data", data);
  }

  saveDC() {
    const data = this.formData;

    this.adminService.saveDC(data).subscribe({
      next: (res: any) => {
        if (res === 'Success') {
          this.toastr.success('User Added Successfully!');
        }
      },
      error: (error: any) => {
        this.toastr.error('Something went wrong');
      },
    });
    
    if (this.FeedBack.valid) {
      const formData = this.FeedBack.value;
  
      // Create an array to store instances of DCLineItem
      const dcLineItems: DCLineItem[] = [];
  
      // Map each form row to a DCLineItem instance
      for (const formRow of formData.Rows) {
        const dcLineItem: DCLineItem = {
          BNo: formRow.BNo,
          CompanyId:Number(this.globalCompanyId),
          InvoiceNumber: formRow.InvoiceNumber,
          Date:data.Date,
          MethodOfPacking:"Machine",
          Description: formRow.Description,
          PrivateMark: formRow.PrivateMark,
          ActualWeightKg: formRow.ActualWeightKg,
          ChangedWeightKg: formRow.ChangedWeightKg,
          Total: formRow.Total,
          Freight: 1.0,
        };
  
        // Push the mapped DCLineItem to the array
        dcLineItems.push(dcLineItem);
      }
  
      for (const dcLineItem of dcLineItems) {
        this.adminService.saveDC(dcLineItem).subscribe({
          next: (res: any) => {
            if (res === 'Success') {
              this.toastr.success(dcLineItem.BNo.toString() +'Added Successfully!');
              
              
            }
          },
          error: (error: any) => {
            this.toastr.error(dcLineItem.BNo.toString() +'Something went wrong');
          },
          complete: () => {
            this.toastr.success('Data Saved...!');
            this.FeedBack.reset();
            
          },
        });
      }
    }


  }
}
