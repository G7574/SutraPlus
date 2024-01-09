import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2'
import { AdminServicesService } from '../../../../services/admin-services.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject } from 'rxjs';
import { CommonService } from 'src/app/share/services/common.service';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-export-invoice-dashboard',
  templateUrl: './export-invoice-dashboard.component.html',
  styleUrls: ['./export-invoice-dashboard.component.scss']
})
export class ExportInvoiceDashboardComponent implements OnInit {
  invoiceList: any[] = [];
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
  totalInvoice: number = 0;


  @ViewChild('invoiceDialog') invoiceDialog!: TemplateRef<any>;

  constructor(private router: Router,
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    public commonService: CommonService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.financialYear = sessionStorage.getItem('financialYear');
    this.customerCode = sessionStorage.getItem('globalCustomerCode');
    this.userDetails = sessionStorage.getItem('userDetails')
    this.userDetails = JSON.parse(this.userDetails);
    this.userEmail = this.userDetails?.result?.UserEmailId;
    this.globalCompanyId = sessionStorage.getItem('companyID')

    this.getInvoiceList();
    this.pagesObj.subscribe(res => {
      res ? this.getPages() : '';
    });
  }

  getInvoiceList() {
    let partyDetails = {
      "SearchText": this.searchText || '',
      "Page": {
        "PageNumber": 1,
        "PageSize": 100
      },
      "InvoiceData": {
        "CompanyId": this.globalCompanyId,
        "InvoiceType": "SalesExport"
      }
    }

    this.adminService.getInvoiceList(partyDetails).subscribe({
      next: (res: any) => {
        this.spinner.show();
        if (!res.HasErrors && res?.Data !== null) {
          this.invoiceList = res.records;
          this.totalInvoice = res.totalCount;
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
    this.router.navigate(['/admin/add-export-invoice'])
  }

  onEdit(data: any): void {

    sessionStorage.setItem('invoiceDataAll', JSON.stringify(data));
    this.router.navigate(['/admin/view-export-invoice'])
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
      this.getInvoiceList();
    }

    if (text.length >= 3) {
      this.errorMsg = false;
      this.searchText = text;
      this.getInvoiceList();
    }
  }

  next() {
    if (this.commonService.gettotalPages(this.totalInvoice) > 5) {
      let lastIndex = (this.pages.length - 1);
      let nextPage = this.pages[lastIndex];
      this.pages.shift();
      this.pages.push(nextPage + 1);
      this.pageNumber++;
      this.getInvoiceList();
    }
    else {
      this.pageNumber++;
      this.getInvoiceList();
    }
  }

  previous() {
    if (this.commonService.gettotalPages(this.totalInvoice) > 5) {
      this.pages.pop();
      let lastPage = this.pages[0];
      this.pages.unshift(lastPage - 1);
      this.pageNumber--;
      this.getInvoiceList();
    }
    else {
      this.pageNumber--;
      this.getInvoiceList();
    }
  }

  getPages() {
    this.pages = [];
    for (let i = 1; i <= this.commonService.gettotalPages(this.totalInvoice); i++) {
      this.pages.length < 5 ? this.pages.push(i) : '';
    }
    this.pagesObj.complete();
  }

  changePage(currentPage: number) {
    this.pageChanged = true;
    this.pageNumber = currentPage;
    this.getInvoiceList();
  }

  openViewInvoiceModal() {
    const dialogRef = this.dialog.open(this.invoiceDialog, {
      width: '80%', // 80% of the screen width
      height: '80%' // 80% of the screen height
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }
}
