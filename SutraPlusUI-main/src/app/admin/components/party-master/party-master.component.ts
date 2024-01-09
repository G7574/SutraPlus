import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2'
import { AdminServicesService } from '../../services/admin-services.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject } from 'rxjs';
import { CommonService } from 'src/app/share/services/common.service';

@Component({
  selector: 'app-party-master',
  templateUrl: './party-master.component.html',
  styleUrls: ['./party-master.component.scss']
})
export class PartyMasterComponent implements OnInit {

  partyList: any[] = [];
  error: any;
  userDetails: any;
  financialYear: any;
  customerCode: any;
  userEmail: any;
  globalCompanyId: any;
  errorMsg!: boolean;
  searchText!: string;
  pageNumber: number = 1;
  pages: number[] = [];
  pagesObj = new BehaviorSubject(false);
  pageChanged: boolean = false;
  totalParty: number = 0;
  constructor(private router: Router,
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    public commonService: CommonService
  ) { }

  ngOnInit(): void {
    this.financialYear = sessionStorage.getItem('financialYear');
    this.customerCode = sessionStorage.getItem('globalCustomerCode');
    this.userDetails = sessionStorage.getItem('userDetails')
    this.userDetails = JSON.parse(this.userDetails);
    this.userEmail = this.userDetails?.result?.UserEmailId;
    this.globalCompanyId = sessionStorage.getItem('companyID')

    this.getPartyList();
    this.pagesObj.subscribe(res => {
      res ? this.getPages() : '';
    });
  }

  getPartyList() {

    let partyDetails = {
      "LedgerData": {
        "CompanyId": this.globalCompanyId,
        "LedgerType": "Sales Ledger",
        "Country": ""
      },
      "SearchText": this.searchText || '',
      "Page":
      {
        "PageNumber": this.pageNumber,
        "PageSize": "10"
      }
    }
    console.log("party details", partyDetails);
    this.adminService.getLedgerList(partyDetails).subscribe({
      next: (res: any) => {
        this.spinner.show();
        if (!res.HasErrors && res?.Data !== null) {
          this.partyList = res.records;
          this.totalParty = res.totalCount;
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

  addParty(): void {
    this.router.navigate(['/admin/add-party-details'])
  }

  onEdit(id: number): void {
    this.router.navigate(['/admin/edit-party-details/' + id])
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

  onSearch(text: string) {
    text.length < 3 ? this.errorMsg = true : this.error = false;

    if (text.length === 0) {
      this.searchText = '';
      this.errorMsg = false;
      this.getPartyList();
    }

    if (text.length >= 3) {
      this.errorMsg = false;
      this.searchText = text;
      this.getPartyList();
    }
  }

  next() {

    if (this.commonService.gettotalPages(this.totalParty) > 5) {
      let lastIndex = (this.pages.length - 1);
      let nextPage = this.pages[lastIndex];
      this.pages.shift();
      this.pages.push(nextPage + 1);
      this.pageNumber++;
      this.getPartyList();
    }
    else {
      this.pageNumber++;
      this.getPartyList();
    }
  }

  previous() {
    if (this.commonService.gettotalPages(this.totalParty) > 5) {
      this.pages.pop();
      let lastPage = this.pages[0];
      this.pages.unshift(lastPage - 1);
      this.pageNumber--;
      this.getPartyList();
    }
    else {
      this.pageNumber--;
      this.getPartyList();
    }
  }

  getPages() {
    this.pages = [];
    for (let i = 1; i <= this.commonService.gettotalPages(this.totalParty); i++) {
      this.pages.length < 5 ? this.pages.push(i) : '';
    }
    this.pagesObj.complete();
  }

  changePage(currentPage: number) {
    this.pageChanged = true;
    this.pageNumber = currentPage;
    this.getPartyList();
  }

}
