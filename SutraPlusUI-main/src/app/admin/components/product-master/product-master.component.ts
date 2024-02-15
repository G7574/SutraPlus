  import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { AdminServicesService } from '../../services/admin-services.service';
import { BehaviorSubject } from 'rxjs';
import { CommonService } from 'src/app/share/services/common.service';

@Component({
  selector: 'app-product-master',
  templateUrl: './product-master.component.html',
  styleUrls: ['./product-master.component.scss']
})
export class ProductMasterComponent implements OnInit {

  productList: any;
  error: any;
  financialYear!: string | null;
  customerCode!: string | null;
  userDetails: any;
  userEmail!: string | null;
  p: any;
  totalProduct: number = 0;
  searchText: string = '';
  errorMsg: boolean = false;
  pageNumber: number = 1;
  pages: number[] = [];
  pagesObj = new BehaviorSubject(false);
  pageChanged: boolean = false;
  constructor(
    private adminService: AdminServicesService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    public commonService: CommonService
  ) { }

  ngOnInit(): void {
    this.setTheme();
    this.pagesObj.subscribe(res => {
      res ? this.getPages() : '';
    });

    this.financialYear = sessionStorage.getItem('financialYear');
    this.customerCode = sessionStorage.getItem('globalCustomerCode');
    this.userDetails = sessionStorage.getItem('userDetails')
    this.userDetails = JSON.parse(this.userDetails);
    this.userEmail = this.userDetails?.result?.UserEmailId;

    this.getProductList();
  }

  setTheme(): void {
    let themeCode: any = sessionStorage.getItem('themeCode');
    let btns = Array.from(document.getElementsByClassName('btn-primary'));
    for (let x of btns) {
      const y = <HTMLElement>x;
      y.style.background = y.style.borderColor = themeCode;
    }

    let chcks = Array.from(document.getElementsByClassName('form-check-input'));
    for (let x of chcks) {
      const y = <HTMLElement>x;
      y.style.backgroundColor = y.style.borderColor = themeCode;
    }
  }

  addProduct(): void {
    this.router.navigate(['/admin/add-product']);
  }

  onDelete(): void {
    Swal.fire({
      title: 'Are you sure?',
      text: "Do you want to delete this record!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Confirm'
    }).then((result) => {
      if (result.isConfirmed) {
        Swal.fire(
          'Deleted!',
          'Your file has been deleted.',
          'success'
        )
      }
    })
  }

  onEdit(item: any): void {
    this.router.navigate(['/admin/edit-product/' + item._Id])
  }

  getProductList(): void {
    let UserDetails = {
      "SearchText": this.searchText,
      "Page": {
        "PageNumber": this.pageNumber,
        "PageSize": 10
      }
    }
    this.searchText.length === 0 ? this.spinner.show() : '';
    this.adminService.getProductList(UserDetails).subscribe({
      next: (res: any) => {
        this.spinner.hide();
        if (!res.HasErrors && res?.Data !== null) {
          this.productList = res.records;
          this.totalProduct = res.totalCount;
          this.pagesObj.next(true);
        } else {
          this.toastr.error('Something went wrong');
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

  onSearch(text: string) {
    text.length < 3 ? this.errorMsg = true : this.error = false;

    if (text.length === 0) {
      this.searchText = '';
      this.errorMsg = false;
      this.getProductList();
    }

    if (text.length >= 3) {
      this.errorMsg = false;
      this.searchText = text;
      this.getProductList();
    }
  }

  next() {
    if (this.commonService.gettotalPages(this.totalProduct) > 5) {
      let lastIndex = (this.pages.length - 1);
      let nextPage = this.pages[lastIndex];
      this.pages.shift();
      this.pages.push(nextPage + 1);
      this.pageNumber++;
      this.getProductList();
    }
    else {
      this.pageNumber++;
      this.getProductList();
    }
  }

  previous() {
    if (this.commonService.gettotalPages(this.totalProduct) > 5) {
      this.pages.pop();
      let lastPage = this.pages[0];
      this.pages.unshift(lastPage - 1);
      this.pageNumber--;
      this.getProductList();
    }
    else {
      this.pageNumber--;
      this.getProductList();
    }
  }

  getPages() {
    this.pages = [];
    for (let i = 1; i <= this.commonService.gettotalPages(this.totalProduct); i++) {
      this.pages.length < 5 ? this.pages.push(i) : '';
    }
    this.pagesObj.complete();
  }

  changePage(currentPage: number) {
    this.pageChanged = true;
    this.pageNumber = currentPage;
    this.getProductList();
  }

}
