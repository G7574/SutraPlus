import { Component, OnInit } from '@angular/core';
import { AdminServicesService } from '../../services/admin-services.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import * as pdfMake from 'pdfmake/build/pdfmake';
import { ca } from 'date-fns/locale';
import * as pdfFonts from 'pdfmake/build/vfs_fonts';

@Component({
  selector: 'app-select-bank-dailog',
  templateUrl: './select-bank-dailog.component.html',
  styleUrls: ['./select-bank-dailog.component.scss']
})
export class SelectBankDailogComponent implements OnInit {

  chequeDate : any;
  select : any;
  banks: any[] = [];
  defaultDate : string;
  bankName: string = "";
  RTGS : string = "";
  bankCode : Number;

  constructor(
    private router: Router,
    private adminService: AdminServicesService,
    private toastr: ToastrService,
  ) { }

  ngOnInit(): void {

    const today = new Date();
    const year = today.getFullYear();
    const month = ('0' + (today.getMonth() + 1)).slice(-2);
    const day = ('0' + today.getDate()).slice(-2);
    this.defaultDate = `${year}-${month}-${day}`;

    this.getData();
  }

  getData() {

    const companyId = parseInt(sessionStorage.getItem('companyID') || '0');
    this.adminService.getBanksName(companyId).subscribe({
      next: (res: any) => {
        this.banks = res.bankNames;
      },
      error: (error: any) => {
        console.error('Error fetching banks:', error);
      }
    });

  }

  savePayment() {

    // if(this.bankName == "" || this.RTGS == "" || this.defaultDate == "" || this.select == "" || this.bankCode == undefined) {
    //   return;
    // }

    const sendingString = sessionStorage.getItem('invoiceList');
    const invoiceList = JSON.parse(sendingString);

    const companyId = parseInt(sessionStorage.getItem('companyID') || '0');
    const paymentData = {
      "paymentData" : {
        bankName: this.bankName,
        RTGS: this.RTGS,
        Date: this.defaultDate,
        select: this.select,
        bankCode: this.bankCode,
        CompanyId: companyId,
        invoiceList: invoiceList
      }
    };

    this.adminService.saveAndGetPaymentList(paymentData).subscribe({
      next: (res: any) => {
        this.toastr.success('Operation Successfull!');
        this.exportPdf(invoiceList, "pdf");

        //sessionStorage.setItem('vouchersList', JSON.stringify(res));
        //this.openNewTab();
      },
      error: (error: any) => {
        this.toastr.success('Something went wrong !');
        console.error('Error fetching banks:', error);
      }
    });

  }

  exportPdf(data: any[], pdf: string) {
    console.log("Data :: " + data);
    if (!Array.isArray(data)) {
      console.error('Data is not an array');
      return;
    }

    const documentDefinition = {
      content: [
        {
          table: {
            headerRows: 1,
            widths: ['*', '*', '*'],
            body: [
              ['Party Name', 'Cheque No', 'Amount'],
              ...data.map(item => [
                item.LedgerName || '-',
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


  openNewTab() {
    const url = this.router.createUrlTree(['/'], { fragment: 'PaymentListReportView' }).toString();
    window.open(url, '_blank');
  }

  onFirstChequeNumberChange(event : any) {

  }

  onSelectChange(event: any) {
    this.select = event.target.value;
  }

  onSelectCodeChange(event: any) {
    this.bankCode = event.target.value;
  }

  onChequeDateChange(event: any) {

  }

  onRTGSValueChange(event : any) {
    this.RTGS = event.target.value;
  }

  onBankChange(event: any){
    this.bankName = event.target.value;
  }

}
