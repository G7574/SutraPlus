import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import * as XLSX from 'xlsx';
import { AdminServicesService } from '../../services/admin-services.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { CommonService } from 'src/app/share/services/common.service';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-day-summary',
  templateUrl: './day-summary.component.html',
  styleUrls: ['./day-summary.component.scss']
})
export class DaySummaryComponent implements OnInit {
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
  date: any;
  partyList: [] = [];

  constructor(
    private router: Router,
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    public commonService: CommonService
  ) { }

  ngOnInit(): void {
    const currentDate = new Date();
    this.date = this.formatDate(currentDate);

    this.globalCompanyId = sessionStorage.getItem('companyID');
    this.getInvoiceList();
  }

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
  }

  getInvoiceList() {
    this.spinner.show();

    let partyDetails = {
      GetAkadaData: {
        CompanyId: this.globalCompanyId,
        DoWeHaveDate: false,
      },
    };

    this.adminService.GetDaySummary(partyDetails).subscribe({
      next: (res: any) => {
        this.spinner.show();
        if (res?.vouchers !== null) {
          this.invoiceList = res.vouchers;
          this.totalInvoice = this.invoiceList.length;
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

  search() {
    this.spinner.show();

    let partyDetails = {
      GetAkadaData: {
        CompanyId: this.globalCompanyId,
        TranctDate: this.date,
        DoWeHaveDate: true,
      },
    };

    this.adminService.GetDaySummary(partyDetails).subscribe({
      next: (res: any) => {
        this.spinner.show();
        if (res?.vouchers !== null) {
          this.invoiceList = res.vouchers;
          this.totalInvoice = this.invoiceList.length;
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

  onStartDateChange(event: any) {
    const startDateValue = event.target.value;
    this.date = startDateValue;
  }

  public crdrvisible = false;

  dispactDetailVisible = false;
  toggleDispatDetailModal() {
    this.dispactDetailVisible = !this.dispactDetailVisible;
  }

  TotalCreditAmount!: number;
  TotalDebitAmount!: number;

  viewClick(data : any){

    this.spinner.show();

    let partyDetails = {
      GetAkadaData: {
        CompanyId: this.globalCompanyId,
        PartyInvoiceNumber: data.Partyinvoicenumber,
        TranctDate: data.Tranctdate,
        VoucherId : data.VoucherId,
        VoucherNo: data.VoucherNo
      },
    };

    this.adminService.GetVocuherDataForDaySummary(partyDetails).subscribe({
      next: (res: any) => {
        this.spinner.show();
        if (res?.vouchers !== null) {
          this.CRDRDetails = res.vouchers;

          this.TotalCreditAmount = 0;
        this.TotalDebitAmount = 0;

        var i = 0;
        for (i = 0; i < this.CRDRDetails.length; i++) {
          this.TotalCreditAmount += this.CRDRDetails[i]['Credit'];
          this.TotalDebitAmount += this.CRDRDetails[i]['Debit'];
        }

        this.TotalCreditAmount = this.SeperateComma(this.TotalCreditAmount.toFixed(2));
        this.TotalDebitAmount = this.SeperateComma(this.TotalDebitAmount.toFixed(2));

          this.toggleCrDrDetails()
        } else {this.toastr.error(res.Errors[0].Message);

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

  SeperateComma(value: any) {

    if(value == undefined)
    {
      value = 0;
    }
    return value;

  }

  toggleCrDrDetails() {
    if(this.crdrvisible == false)
    {
      this.crdrvisible = true;
    }
    else
    {
      this.crdrvisible = false;
    }
  }
  CRDRDetails: any;
  onModalClosed() {
    this.crdrvisible = false;
  }

}
