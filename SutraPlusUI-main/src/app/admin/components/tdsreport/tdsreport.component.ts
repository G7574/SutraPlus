import { Component, TemplateRef, ViewChild } from '@angular/core';
import * as XLSX from 'xlsx';
import * as pdfMake from 'pdfmake/build/pdfmake';
import * as pdfFonts from 'pdfmake/build/vfs_fonts';
import { BehaviorSubject, Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { AdminServicesService } from '../../services/admin-services.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { CommonService } from 'src/app/share/services/common.service';
import { MatDialog } from '@angular/material/dialog';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-tdsreport',
  templateUrl: './tdsreport.component.html',
  styleUrls: ['./tdsreport.component.scss']
})
export class TDSReportComponent {

  invoiceList: any[] = [];
  error: any;
  userDetails: any;
  financialYear: any;
  customerCode: any;
  userEmail: any;
  globalCompanyId: any;
  errorMsg!: boolean;
  pageNumber: number = 1;
  pages: number[] = [];
  pagesObj = new BehaviorSubject(false);
  pageChanged: boolean = false;
  totalInvoice: number = -1;
  balance: any;
  data$: Observable<any>;

  @ViewChild('invoiceDialog') invoiceDialog!: TemplateRef<any>;

  constructor(
    private router: Router,
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    public commonService: CommonService,
    private activatedRoute: ActivatedRoute,
    private dialog: MatDialog,
  ) { }

  ngOnInit(): void {
    const currentDate = new Date();
    // this.startDate = this.formatDate(new Date(currentDate.getFullYear(), currentDate.getMonth(), 1));
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

  getInvoiceList() {

    let partyDetails = {
        Page: {
        PageNumber: this.pageNumber,
        PageSize: 20,
      },
      ReportData: {
        CompanyId: this.globalCompanyId,
        ReportType: "TDSReport",
      },
    };

    this.adminService.getReport(partyDetails).subscribe({
      next: (res: any) => {
        this.spinner.show();
        if (!res.HasErrors && res?.Data !== null) {
          this.invoiceList = res;
          this.totalInvoice = res.length;
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

  exportAsPdf(pdf: string) {
    this.exportPdf(this.invoiceList, pdf);
  }

  exportToExcel(): void {

    const headers = ['Sr. No.', 'Party Name', 'TotalCommission', 'TDSBalance'];
    const excelData = [
      headers,
      ...this.invoiceList.map((item, index) => [
        index + 1,
        `${item.LedgerName} - ${item.Place}`,
        item.TotalCommission,
        item.TDSBalance
      ])
    ];

    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(excelData);
    const workbook: XLSX.WorkBook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };
    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    this.saveAsExcelFile(excelBuffer, 'TDSReport');
  }

  private saveAsExcelFile(buffer: any, fileName: string): void {
    const data: Blob = new Blob([buffer], { type: 'application/octet-stream' });
    const url: string = window.URL.createObjectURL(data);
    const a: HTMLAnchorElement = document.createElement('a');
    a.href = url;
    a.download = fileName + '.xlsx';
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    window.URL.revokeObjectURL(url);
  }

  exportPdf(data: any[], pdf: string) {

    const documentDefinition = {
      content: [
        {
          table: {
            headerRows: 1,
            widths: ['*', '*', '*'],
            body: [
              ['Party Name', 'TotalCommission', 'TDSBalance'],
              ...data.map(item => [

                item.LedgerName + '-' + item.Place || '-',
                item.TotalCommission || '-',
                item.TDSBalance || '-'
              ])
            ]
          }
        }
      ],
      layout: {
        defaultBorder: false
      }
    };

    try {
      pdfMake.createPdf(documentDefinition, null, null, pdfFonts.pdfMake.vfs).download('TDSReport.' + pdf);
    } catch (error) {
      console.error('Error generating PDF:', error);
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
