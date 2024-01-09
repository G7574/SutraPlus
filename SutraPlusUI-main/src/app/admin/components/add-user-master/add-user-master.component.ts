import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AdminServicesService } from '../../services/admin-services.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Constants } from 'src/app/share/models/constants';
import { Location } from '@angular/common';
@Component({
  selector: 'app-add-user-master',
  templateUrl: './add-user-master.component.html',
  styleUrls: ['./add-user-master.component.scss']
})
export class AddUserMasterComponent implements OnInit {
  
  financialYear!: string | null;
  customerCode!: string | null;
  userDetails!: any;
  userEmail: any;
  globalCompanyId!: string | null;
  formList: any;
  groupNames: any = [];
  formNames: any;
  uniqueGroupNames: any = [];
  groupingData : {"groupName":string,"formData":{"formId":number,"formName":string} [],"accessId":number[]} [] = [];
  addUser!: FormGroup;
  submitted:boolean=false;
  error:any;
  accessId:number[]=[];
  accessModuleEnabled:boolean=false;
  constructor(private location: Location,private router: Router,private adminService: AdminServicesService,private spinner: NgxSpinnerService,private fb: FormBuilder,private toastr: ToastrService) {}

  ngOnInit(): void { 

    this.addUser = new FormGroup({
      firstName: new FormControl(''),
      lastName: new FormControl(''),
      mobileNo: new FormControl(''),
      email: new FormControl(''),
      userType: new FormControl('')
    });

    this.addUser = this.fb.group({
      firstName: [
        '',
        [Validators.required],
      ],
      lastName: [
        '',
        [Validators.required],
      ],

      mobileNo: [
        '',
        [Validators.required,Validators.pattern(Constants.PHONE_REGEXP)],
      ],
      email: [
        '',
        [Validators.required,Validators.pattern(Constants.EMAIL_REGEXP)],
      ],
      userType: [
        '',
        [Validators.required],
      ]
    });

    this.setTheme();
    this.financialYear = sessionStorage.getItem('financialYear');
    this.customerCode = sessionStorage.getItem('globalCustomerCode');
    this.userDetails = sessionStorage.getItem('userDetails');
    this.userDetails = JSON.parse(this.userDetails);
    this.userEmail = this.userDetails?.result?.UserEmailId;
    this.globalCompanyId = sessionStorage.getItem('companyID');
    this.getUserList();
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
    this.router.navigate(['/admin/create-user'])
  }

  getUserList() {
    this.adminService.getUserFormList('').subscribe({
      next: (res: any) => {
        this.spinner.show();
        this.formList = res.FormList;

        for (let i = 0; i < this.formList.length; i++) {
          const groupName = this.formList[i].GroupName;
          if (!this.groupNames.includes(groupName)) {
            this.groupNames.push(groupName);
          }
        } 
     
        console.log(this.groupNames);
        for(let data of this.groupNames)
        {  
          this.getFormsNames(data);
        }
        this.spinner.hide()
      },
      error: (error: any) => {
        this.spinner.hide();
      }
    })
  }

   getFormsNames(groupName:string)
  { 
    let arrayofFormName: {"formId":number,"formName":string} [] = [];
    for(let data of this.formList)
    {
      if(groupName===data.GroupName)
      { 
        arrayofFormName.push({"formId":data.FormId,"formName":data.FormName}); 
      }
    }
    this.groupingData.push({"groupName":groupName,"formData":arrayofFormName,"accessId":[]});
  } 
  
  onSelectItems(event:any,item:number,index:number) 
  {
    if(event.target.checked)
    {
     this.groupingData[index].accessId.push(item);
    }
    else 
    {
      let indexofaccessId = this.groupingData[index].accessId.indexOf(item);
      this.groupingData[index].accessId.splice(indexofaccessId, 1);
    }
   }
   
   selectAll(event: any,items:any,index:number)
  {
    if(event.target.checked)
    {
      this.groupingData[index].accessId=[];
      for(let item of items)
      {
        this.groupingData[index].accessId.push(item.formId);
      }
      }
    else 
    { 
      this.groupingData[index].accessId = [];
    }
   } 

  get add(): { [key: string]: AbstractControl } {
    return this.addUser.controls;
  }

  onSubmit(): void {
    this.submitted = true;
    debugger;
    if (this.addUser.valid) {

      this.accessId = [];
      if(this.accessModuleEnabled) 
      {
      for(let data of this.groupingData)
      {
        for (let id of data.accessId)
        {
          this.accessId.push(id);
        }
      }
    }
    else 
    {
      for(let data of this.formList)
      {
        this.accessId.push(data.FormId);
      }
    }

      let userDetails = {
        "UserCreate": {
          "UserType": this.addUser.get('userType')?.value,
          "UserName": this.addUser.get('email')?.value,
          "FirstName": this.addUser.get('firstName')?.value,
          "LastName": this.addUser.get('lastName')?.value,
          "PhoneNo": this.addUser.get('mobileNo')?.value,
        },
        "AccessData":this.accessId
      }
      this.spinner.show()
      this.adminService.createUser(userDetails).subscribe({
        next: (res: any) => {
          if (res === 'User Added Successfully...!') {
            this.toastr.success('User Added Successfully!');
            this.location.back();           
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

  onChangeUserType(userType:string)
  {
    if(userType==='User')
    {
      this.accessModuleEnabled=true;
    }
    else
    {
      this.accessModuleEnabled=false;
    }
  }

  reset()
  {
    this.submitted = false;
    this.addUser.reset();
    this.addUser.controls['userType'].setValue("");
    this.accessModuleEnabled=true;
    this.groupingData=[];
    this.accessId = [];
    this.getUserList();
  }
  
}
