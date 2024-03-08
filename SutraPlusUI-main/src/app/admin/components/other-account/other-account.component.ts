import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { AdminServicesService } from '../../services/admin-services.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject } from 'rxjs';
import { CommonService } from 'src/app/share/services/common.service';

@Component({
  selector: 'app-other-account',
  templateUrl: './other-account.component.html',
  styleUrls: ['./other-account.component.scss']
})
export class OtherAccountComponent implements OnInit {

  LedgerList: any = [];
  error: any;
  userDetails: any;
  financialYear: any;
  customerCode: any;
  userEmail: any;
  globalCompanyId: any;
  totalAccounts: number = 0;
  searchText: string = '';
  errorMsg: boolean = false;
  pageNumber: number = 1;
  pages: number[] = [];
  pagesObj = new BehaviorSubject(false);
  pageChanged: boolean = false;

  constructor(
    private router: Router,
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    public commonService: CommonService
  ) { }

  ngOnInit(): void {
    this.setTheme();
    this.financialYear = sessionStorage.getItem('financialYear');
    this.customerCode = sessionStorage.getItem('globalCustomerCode');
    this.userDetails = sessionStorage.getItem('userDetails')
    this.userDetails = JSON.parse(this.userDetails);
    this.userEmail = this.userDetails?.result?.UserEmailId;
    this.globalCompanyId = sessionStorage.getItem('companyID');

    this.getLedger();
    this.pagesObj.subscribe(res => {
      res ? this.getPages() : '';
    });
  }

  addAccount(): void {
    this.router.navigate(['/admin/add-other-account'])
  }

  setTheme(): void {
    let themeCode: any = sessionStorage.getItem('themeCode');
    let btns = Array.from(document.getElementsByClassName('btn-primary'));
    for (let x of btns) {
      const y = <HTMLElement>x;
      y.style.background = y.style.borderColor = themeCode;
    }
  }

  onEdit(id: any) {
    this.router.navigate(['/admin/edit-other-account/' + id])
  }

  delete(): void {
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
          'Record has been deleted.',
          'success'
        )
      }
    })
  }

  getLedger() {
    let obj = {
      // "LedgerData": {
      //   "CompanyId": this.globalCompanyId,
      // },
      // "SearchText": this.searchText,
      // "Page": {
      //   "PageNumber": this.pageNumber,
      //   "PageSize": 10
      // }

      "LedgerData": {
        "CompanyId": this.globalCompanyId,
        "LedgerType": "Sales Other Ledger",
        "Country": ""
      },
      "SearchText": this.searchText || "",
      "Page": {
        "PageNumber": this.pageNumber,
        "PageSize": "10"
      }
    }


    this.adminService.getLedgerListForOtherAccounts(obj).subscribe({
      next: (res: any) => {
        this.spinner.show();

        if (!res.HasErrors && res?.Data !== null) {
          this.LedgerList = res.records;
          this.totalAccounts = res.totalCount;
          this.pagesObj.next(true);
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

  onSearch(text: string) {
    text.length < 3 ? this.errorMsg = true : this.error = false;

    if (text.length === 0) {
      this.searchText = '';
      this.errorMsg = false;
      this.getLedger();
    }

    if (text.length >= 3) {
      this.errorMsg = false;
      this.searchText = text;
      this.getLedger();
    }
  }

  next() {
    if (this.commonService.gettotalPages(this.totalAccounts) > 5) {
      let lastIndex = (this.pages.length - 1);
      let nextPage = this.pages[lastIndex];
      this.pages.shift();
      this.pages.push(nextPage + 1);
      this.pageNumber++;
      this.getLedger();
    }
    else {
      this.pageNumber++;
      this.getLedger();
    }
  }

  previous() {
    if (this.commonService.gettotalPages(this.totalAccounts) > 5) {
      this.pages.pop();
      let lastPage = this.pages[0];
      this.pages.unshift(lastPage - 1);
      this.pageNumber--;
      this.getLedger();
    }
    else {
      this.pageNumber--;
      this.getLedger();
    }
  }

  getPages() {
    this.pages = [];
    for (let i = 1; i <= this.commonService.gettotalPages(this.totalAccounts); i++) {
      this.pages.length < 5 ? this.pages.push(i) : '';
    }
    this.pagesObj.complete();
  }

  changePage(currentPage: number) {
    this.pageChanged = true;
    this.pageNumber = currentPage;
    this.getLedger();
  }


}
