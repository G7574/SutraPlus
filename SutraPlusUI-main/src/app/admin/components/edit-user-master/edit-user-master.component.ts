import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AdminServicesService } from '../../services/admin-services.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Constants } from 'src/app/share/models/constants';
import { Location } from '@angular/common';
@Component({
  selector: 'app-edit-user-master',
  templateUrl: './edit-user-master.component.html',
  styleUrls: ['./edit-user-master.component.scss']
})
export class EditUserMasterComponent implements OnInit {

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
  accessId:string[]=[];
  accessModuleEnabled:boolean=false;
  userId?:string | null;
  accessData:any;
  constructor(private route: ActivatedRoute,private location: Location,private router: Router,private adminService: AdminServicesService,private spinner: NgxSpinnerService,private fb: FormBuilder,private toastr: ToastrService) {}

  ngOnInit(): void {
    this.setTheme();

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
    this.userId = this.route.snapshot.paramMap.get('id');
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
    this.router.navigate(['/admin/create-user']);
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
     
        for(let data of this.groupNames)
        {  
          this.getFormsNames(data);
        }
        this.getUserById();
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
    if (this.addUser.valid) {

      this.accessId = [];
      if(this.accessModuleEnabled) 
      {
      for(let data of this.groupingData)
      {
        for (let id of data.accessId)
        {
          this.accessId.push(id.toString());
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
          "UserId":Number(this.userId),
          "UserType": this.addUser.get('userType')?.value,
          "UserName": this.addUser.get('email')?.value,
          "FirstName": this.addUser.get('firstName')?.value,
          "LastName": this.addUser.get('lastName')?.value,
          "PhoneNo": this.addUser.get('mobileNo')?.value,
        },
        "AccessData":this.accessId
      }
      this.spinner.show()
      debugger;
      this.adminService.updateUser(userDetails).subscribe({
        next: (res: any) => {
          if (res === true) {
            this.toastr.success('User Updated Successfully!');
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

  getUserById() {    
    let userDetails = {
      CreateUser:{Id:this.userId}
    };
    this.adminService.getUserById(userDetails).subscribe({
      next: (res: any) => {
        
        this.spinner.show();
        const accdata = res.Accessdata.filter((x: any) => x.IsAccess);        
        this.accessData = { ...res, ...{ Accessdata: accdata } };       
        this.setData();
        this.getAccessId();
        this.spinner.hide()
      },
      error: (error: any) => {
        this.spinner.hide();
      }
    });
  }

  setData() {
      this.addUser.controls['firstName'].setValue(this.accessData.UserData[0].FirstName);
      this.addUser.controls['lastName'].setValue(this.accessData.UserData[0].LastName);
      this.addUser.controls['mobileNo'].setValue(this.accessData.UserData[0].PhoneNo);
      this.addUser.controls['email'].setValue(this.accessData.UserData[0].UserName);
      this.addUser.controls['userType'].setValue(this.accessData.UserData[0].UserType);
      this.accessData.UserData[0].UserType==='User' ? this.accessModuleEnabled=true : this.accessModuleEnabled=false;
    }

  getAccessId()
  { 
    for(let data of this.accessData.Accessdata)
    {
      for(let item of this.groupingData)
      {
        for(let list of item.formData)
        {
          if(data.FormId===list.formId)
          {
             item.accessId.push(data.FormId)
          }
        }
      }
    }
  }
  
}
