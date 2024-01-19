import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject } from 'rxjs';
import { CommonService } from 'src/app/share/services/common.service';
import { MatDialog } from '@angular/material/dialog';
import { AdminServicesService } from '../../services/admin-services.service';
import { ConsoleService } from '@ng-select/ng-select/lib/console.service';
import { colorSerializable } from 'devexpress-reporting/scopes/reporting-chart-internal-series';

@Component({
  selector: 'app-payment-list',
  templateUrl: './payment-list.component.html',
  styleUrls: ['./payment-list.component.scss']
})
export class PaymentListComponent implements OnInit {
  invoiceList: any[] = [];
  error: any;
  userDetails: any;
  financialYear: any;
  customerCode: any;
  userEmail: any;
  globalCompanyId: any;
  errorMsg!: boolean;
  SearchText: string ="";
  pageNumber: number = 1;
  pages: number[] = [];
  pagesObj = new BehaviorSubject(false);
  pageChanged: boolean = false;
  totalInvoice: number = -1;
  startDate: any;
  endDate: any;
  balance: any;

  @ViewChild('invoiceDialog') invoiceDialog!: TemplateRef<any>;

  constructor(
    private router: Router,
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    public commonService: CommonService,
    private activatedRoute: ActivatedRoute,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    const currentDate = new Date();
    // this.startDate = this.formatDate(new Date(currentDate.getFullYear(), currentDate.getMonth(), 1));
    //this.endDate = this.formatDate(new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0));
    this.financialYear = sessionStorage.getItem('financialYear');
    this.customerCode = sessionStorage.getItem('globalCustomerCode');
    this.userDetails = sessionStorage.getItem('userDetails');
    this.userDetails = JSON.parse(this.userDetails);
    this.userEmail = this.userDetails?.result?.UserEmailId;
    this.globalCompanyId = sessionStorage.getItem('companyID');
    // this.SearchText = sessionStorage.getItem("SearchText");
    this.activatedRoute.queryParams.subscribe((params) => {

      this.getInvoiceList();
    });
    this.pagesObj.subscribe((res) => {
      res ? this.getPages() : '';
    });
  }

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
    // return `2022-04-01`;
  }

  onPlace(text: string) {
    this.SearchText = text;
  }

  onBalance(text: string){
    this.balance = text;
  }

  onDate(date : string) {
    this.endDate = date;
  }

  getInvoiceList() {

    if(this.SearchText){
    }
    else
    this.SearchText = ""


    if(this.balance == ''){
      this.balance = -1;
    }
    else
    this.balance = this.balance


    let partyDetails = {
      SearchText: this.SearchText,
      Balance: this.balance,
      Date: this.endDate ,
      Page: {
        PageNumber: this.pageNumber,
        PageSize: 20,
      },
      InvoiceData: {
        CompanyId: this.globalCompanyId,
        InvoiceType: "GoodsInvoice",
      },
    };

    this.adminService.getInvoiceList(partyDetails).subscribe({
      next: (res: any) => {
        this.spinner.show();
        if (!res.HasErrors && res?.Data !== null) {
          this.invoiceList = res.records;
          this.totalInvoice = res.totalCount;
          this.getPages();
          this.pagesObj.next(true);
        } else {
          this.toastr.error(res.Errors[0].Message);
          this.error = res.Errors[0].Message;
          this.getPages();
        }
        this.spinner.hide();
      },
      error: (error: any) => {
        this.spinner.hide();
        this.error = error;
        this.toastr.error('Something went wrong');
        console.log(error);
      },
    });
  }

  fireQuery() {

    console.log("search : " + this.SearchText);
    console.log("balance : " + this.balance);

    this.SearchText.length < 3 ? (this.errorMsg = true) : (this.error = false);
    if (this.SearchText.length === 0) {
      this.SearchText = '';
      this.errorMsg = false;
      this.getInvoiceList();
    }

    if (this.SearchText.length >= 3) {
      this.errorMsg = false;
      this.SearchText = this.SearchText;
      this.getInvoiceList();
    }
  }

  next() {
    if (this.commonService.gettotalPages(this.totalInvoice) > 5) {
      let lastIndex = this.pages.length - 1;
      let nextPage = this.pages[lastIndex];
      this.pages.shift();
      this.pages.push(nextPage + 1);
      this.pageNumber++;
      this.getInvoiceList();
    } else {
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
    } else {
      this.pageNumber--;
      this.getInvoiceList();
    }
  }

  getPages() {
    this.pages = [];
    for (
      let i = 1;
      i <= this.commonService.gettotalPages(this.totalInvoice);
      i++
    ) {
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
      width: '60%', // 80% of the screen width
      height: '60%', // 80% of the screen height
    });

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }
}
