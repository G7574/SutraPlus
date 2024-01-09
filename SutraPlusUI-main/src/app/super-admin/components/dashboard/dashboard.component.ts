import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { SuperAdminServiceService } from '../../services/super-admin-service.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { CommonService } from 'src/app/share/services/common.service';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  customerList: any
  error: any;
  p: any
  totalCustomer:number = 0;
  searchText:string = '';
  errorMsg:boolean=false;
  pageNumber:number = 1 ;
  pages:number[]= [];
  pagesObj = new BehaviorSubject(false);
  pageChanged:boolean=false;
  constructor(
    private router: Router,
    private spinner: NgxSpinnerService,
    private superAdminService: SuperAdminServiceService,
    private toastr: ToastrService,
    public commonService: CommonService
  ) { }

  ngOnInit(): void {
    this.getCustomer();
    this.pagesObj.subscribe(res=>
      {
         res ? this.getPages() : '';
      });
  }

  getCustomer(): void {
    this.spinner.show(); 

    let obj = {"SearchText":this.searchText,"Page":{"PageNumber":this.pageNumber,"PageSize":10}};

    this.superAdminService.getCustomerList(obj).subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          this.customerList = res.records;  
          this.totalCustomer = res.totalCount;
          this.pagesObj.next(true);
        } else {
          this.toastr.error('No record found');
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

  newCustomer(): void {
    this.router.navigate(['/super-admin/add-customer'])
  }

  onEdit(id: number): void {
    this.router.navigate(['/super-admin/edit-customer/' + id])
  }

  capitalizeFirstLetter(str: string): string {
    return str.slice(0, 1).toUpperCase() + str.slice(1);
  }

  changeStatus(item: any, e: any) {
    let currentStatus = item.IsActive;
    let realVal = currentStatus == false ? 'enable' : 'disable'

    Swal.fire({
      title: 'Are you sure?',
      text: 'Do you want to ' + realVal + ' this customer!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Confirm'
    }).then((result) => {
      if (result.isConfirmed) {
        let itemId = {
          "Id": item.Id
        }

        this.superAdminService.activeDeactiveCustomer(currentStatus, itemId).subscribe({
          next: (res: any) => {
            if (!res.HasErrors && res?.Data !== null) {
              if (res == true) {
                this.getCustomer();
                Swal.fire(
                  '' + this.capitalizeFirstLetter(realVal) + 'd',
                  'Customer has been ' + realVal + 'd.',
                  'success'
                )
              }
            }
            this.spinner.hide();
          },
          error: (error: any) => {
            this.spinner.hide();
            this.error = error;
            this.toastr.error('Something went wrong');
          },
        });
      } else {
        this.getCustomer();
      }
    })
  }

  onSearch(text:string)
  {
    text.length < 3 ? this.errorMsg=true : this.error=false; 
    
    if(text.length===0)
    {
      this.searchText=''; 
      this.errorMsg=false;
      this.getCustomer();
    }  

    if(text.length>=3)
    { 
      this.errorMsg=false; 
      this.searchText = text;
      this.getCustomer();
    }
  }

  next()
  {
   if(this.commonService.gettotalPages(this.totalCustomer) > 5)
   {
   let lastIndex = (this.pages.length - 1);
   let nextPage = this.pages[lastIndex];
   this.pages.shift();
   this.pages.push(nextPage+1);
   this.pageNumber++;
   this.getCustomer();
   }
   else 
   {
    this.pageNumber++;
    this.getCustomer();
   }
  }

  previous()
  {
    if(this.commonService.gettotalPages(this.totalCustomer) > 5)
    {
    this.pages.pop();
    let lastPage = this.pages[0];
    this.pages.unshift(lastPage-1);
    this.pageNumber--;
    this.getCustomer();
    }
    else 
    {
      this.pageNumber--;
      this.getCustomer();
    }
  }

  getPages()
  { 
    this.pages = [];
    for(let i = 1; i <= this.commonService.gettotalPages(this.totalCustomer);i++)
    {
      this.pages.length<5 ? this.pages.push(i) : '';
    }
    this.pagesObj.complete();
  }

  changePage(currentPage:number)
  {
    this.pageChanged = true;
    this.pageNumber = currentPage;
    this.getCustomer();
  }

} 
