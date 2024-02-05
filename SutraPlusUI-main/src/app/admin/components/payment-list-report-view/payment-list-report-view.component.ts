import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { AdminServicesService } from '../../services/admin-services.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { CommonService } from 'src/app/share/services/common.service';
import { MatDialog } from '@angular/material/dialog';
import * as pdfMake from 'pdfmake/build/pdfmake';
import { ca } from 'date-fns/locale';
import * as pdfFonts from 'pdfmake/build/vfs_fonts';

@Component({
  selector: 'app-payment-list-report-view',
  templateUrl: './payment-list-report-view.component.html',
  styleUrls: ['./payment-list-report-view.component.scss']
})
export class PaymentListReportViewComponent implements OnInit {

  error: any;
  invoiceList: any[] = [];
  pageNumber: number = 1;
  pages: number[] = [];
  pagesObj = new BehaviorSubject(false);
  pageChanged: boolean = false;
  totalInvoice: number = -1;

  constructor(
    private router: Router,
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    public commonService: CommonService,
    private activatedRoute: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.getInvoiceList();
  }

  getInvoiceList() {

    // const sendingString = sessionStorage.getItem('vouchersList');
    // console.log(sendingString);
    // if (sendingString) {
    //   this.invoiceList = JSON.parse(sendingString);
    // } else {
    //   this.invoiceList = [];
    // }

  }

  exportPdf(data: any[],pdf:string) {

    const documentDefinition = {
      content: [
        {
          table: {
            headerRows: 1,
            widths: ['*', '*', '*'],
            body: [
              ['Party Name', 'Cheque No', 'Amount'],
              ...data.map(item => [
                item.LedgerName + '-' + item.Place || '-',
                item.chequeNo || '-',
                item.payAmount || '-'
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
      pdfMake.createPdf(documentDefinition, null, null, pdfFonts.pdfMake.vfs).download('Demo.' + pdf);
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

}
