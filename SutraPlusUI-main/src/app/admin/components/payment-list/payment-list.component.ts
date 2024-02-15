import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import { ActionId } from 'devexpress-reporting/dx-webdocumentviewer';
import { NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CommonService } from 'src/app/share/services/common.service';
import { MatDialog } from '@angular/material/dialog';
import { AdminServicesService } from '../../services/admin-services.service';
import { ConsoleService } from '@ng-select/ng-select/lib/console.service';
import { colorSerializable } from 'devexpress-reporting/scopes/reporting-chart-internal-series';
import { DxReportViewerComponent } from 'devexpress-reporting-angular';
import { DataService } from 'src/app/core/services/data.service';
import { data } from 'jquery';
import { DxDataGridComponent } from 'devextreme-angular';
import * as pdfMake from 'pdfmake/build/pdfmake';
import { ca } from 'date-fns/locale';
import * as pdfFonts from 'pdfmake/build/vfs_fonts';
import { Ledger } from '../sales/models/ladger.model';
import * as XLSX from 'xlsx';
import { constant } from 'lodash-es';
import { SelectBankDailogComponent } from '../select-bank-dailog/select-bank-dailog.component';


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
  data$: Observable<any>;
  payAmount : Number;
  chequeNo : Number;

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
  isTableViewVisible: boolean = true;
  ngOnInit(): void {
    const currentDate = new Date();
    // this.startDate = this.formatDate(new Date(currentDate.getFullYear(), currentDate.getMonth(), 1));
    this.endDate = this.formatDate(new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0));
    this.financialYear = sessionStorage.getItem('financialYear');
    this.customerCode = sessionStorage.getItem('globalCustomerCode');
    this.userDetails = sessionStorage.getItem('userDetails');
    this.userDetails = JSON.parse(this.userDetails);
    this.userEmail = this.userDetails?.result?.UserEmailId;
    this.globalCompanyId = sessionStorage.getItem('companyID');
    // this.SearchText = sessionStorage.getItem("SearchText");
    // this.activatedRoute.queryParams.subscribe((params) => {

    //   this.getInvoiceList();
    // });
    this.pagesObj.subscribe((res) => {
      res ? this.getPages() : '';
    });
  }


  @ViewChild(DxReportViewerComponent, { static: false }) viewer: DxReportViewerComponent;
  reportUrl: string = "rptPaymentList";
  // The built-in controller in the back-end ASP.NET Core Reporting application.
  invokeAction: string = '/DXXRDV';

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
    // return `2022-04-01`;
  }

  onPayAmount(amount:string, index : number) {
    this.payAmount = Number(amount);
    this.invoiceList[index].PayAmount = amount;

   //if(this.invoiceList[index].chequeNo != "undefined") {
     this.onChequeNumber(this.invoiceList[index].chequeNo, index);
    //}

  }

  onChequeNumber(chequeNumber:number, index : number) : Number {


    let enteredChequeNumber;

    if(String(chequeNumber).length > 4 && this.invoiceList[index].payAmount != "undefined" && this.invoiceList[index].payAmount > 0) {
      this.chequeNo = chequeNumber;

      for(var i = 0 ; i < this.invoiceList.length; i++){
        if(this.invoiceList[i].chequeNo != null) {
          if(this.invoiceList[i].chequeNo > 0) {
            enteredChequeNumber = this.invoiceList[i].chequeNo;
            break;
          }
        }
      }

      for(var i = 0 ; i < this.invoiceList.length; i++){
        if(this.invoiceList[i].PayAmount != null) {
          if(this.invoiceList[i].PayAmount > 0 && enteredChequeNumber != null && enteredChequeNumber != "undefined") {
            this.invoiceList[i].chequeNo = enteredChequeNumber;
            enteredChequeNumber = enteredChequeNumber + 1;
          }
        }
      }

      return Number(chequeNumber) + 1;
    } else {
      console.log("index -> " + index);
      this.onChequeNumber(this.invoiceList[index].chequeNo, index);
      return 0;
    }
  }

  storeList() {
    let sending: any[] = [];

    for (let i = 0; i < this.invoiceList.length; i++) {
      if (this.invoiceList[i].PayAmount > 0) {
        sending.push(this.invoiceList[i]);
      }
    }

    const sendingString = JSON.stringify(sending);

    sessionStorage.setItem('invoiceList', sendingString);
  }


  selectBank() {
    this.storeList();
    this.dialog.open(SelectBankDailogComponent, {
      width: '800px',
      disableClose: false,
    });
  }

  onPlace(text: string) {
    this.SearchText = text;
  }

  onBalance(text: string){
    console.log("balance" + text);
    this.balance = text;
  }

  onDate(date : string) {
    this.endDate = date;
  }

  getInvoiceList() {
    console.log("OYEEOYEEE");
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
      ReportData: {
        CompanyId: this.globalCompanyId,
        ReportType: "PaymentList",
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


  title = 'DXReportDesignerSample';
  // If you use the ASP.NET Core backend:
  getDesignerModelAction = "/DXXRD/GetDesignerModel";
  // The report name.
  reportName = "rptPaymentList" + "&StartDate=" + this.formatDate(new Date(new Date().getFullYear(), new Date().getMonth(), 1)) + "&EndDate=" + this.formatDate(new Date(new Date().getFullYear(), new Date().getMonth()+ 1, 0)) + "&companyidrecord=" + sessionStorage.getItem('companyID') + "&vochtype1=0&vochtype1=99";
  // The backend application URL.
  host = environment.Reportingapi;

  ParametersSubmitted(event: any) {
    event.args.Parameters.filter(function (p: any) { return p.Key == "StartDate"; })[0].Value = new Date();
    event.args.Parameters.filter(function (p: any) { return p.Key == "EndDate"; })[0].Value = new Date();
}

  CustomizeMenuActions(event: any) {

    // Hide the "Print" and "PrintPage" actions.
    var printAction = event.args.GetById(ActionId.Print);
    if (printAction)
      printAction.visible = false;
    var printPageAction = event.args.GetById(ActionId.PrintPage);
    if (printPageAction)
      printPageAction.visible = false;
  }

  @ViewChild(DxDataGridComponent, { static: false }) dataGrid!: DxDataGridComponent;
  generateRepo() {
    this.isTableViewVisible = !this.isTableViewVisible;
  }

  exportAsPdf(pdf:string) {
    this.exportPdf(this.invoiceList,pdf);
  }

  exportAsExcel(pdf:string) {
    this.exportPdf(this.invoiceList,pdf);
  }

  concatFields(data: any): string {
    return `${data.LedgerName || ''} - ${data.Place || ''}`;
  }

  exportToExcel(): void {
    const customLines = [
      ['Search Filter'],
      ['Date',this.endDate],
      ['Search Text',this.SearchText],
      ['Balance > ',this.balance]
    ];


    const headers = ['Sr. No.','Party Name', 'Yadi Balance', 'Amount'];
    const excelData = [
      ...customLines,
      headers,
      ...this.invoiceList.map((item, index) => [
        index + 1,
        `${item.LedgerName} - ${item.Place}`,
        item.AsOnDateBalance,
        item.TotalBalance
      ])
    ];

    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(excelData);
    const workbook: XLSX.WorkBook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };
    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    this.saveAsExcelFile(excelBuffer, 'PaymentWiseList');
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

  exportPdf(data: any[],pdf:string) {

    const customLines = [
      ['Search Filter'],
      ['Date',this.endDate],
      ['Search Text',this.SearchText],
      ['Balance > ',this.balance]
    ];

    const documentDefinition = {
      content: [
        {
          table: {
            headerRows: 1,
            widths: ['*', '*', '*'],
            body: [
              ['Party Name', 'Yadi Balance', 'Amount'],
              ...data.map(item => [
                item.LedgerName + '-' + item.Place || '-',
                item.AsOnDateBalance || '-',
                item.TotalBalance || '-'
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
      pdfMake.createPdf(documentDefinition, null, null, pdfFonts.pdfMake.vfs).download('PaymentWiseList.' + pdf);
    } catch (error) {
      console.error('Error generating PDF:', error);
    }

  }

  fireQuery() {

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
