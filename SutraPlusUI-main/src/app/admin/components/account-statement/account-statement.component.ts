import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { party } from 'src/app/share/models/party';
import { AdminServicesService } from '../../services/admin-services.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { CommonService } from 'src/app/share/services/common.service';
import { DatePipe } from '@angular/common';
import { DecimalpointPipe } from 'src/app/pipe/decimalpoint.pipe';
import { CalculateService } from '../../services/calculate.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { AuthenticationServiceService } from 'src/app/authentication/services/authentication-service.service';
import { SuperAdminServiceService } from 'src/app/super-admin/services/super-admin-service.service';
import { start } from 'repl';
import { NgbCalendar, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-account-statement',
  templateUrl: './account-statement.component.html',
  styleUrls: ['./account-statement.component.scss']
})
export class AccountStatementComponent implements OnInit{
  invoiceList: any[] = [];
  startDate: any;
  endDate: any;
  financialYear: string = "";
  partyList: party[] = [];
  ledgerId: number = 0;
  totalInvoice: number = 0;

  globalCompanyId!: number;
  minYear : NgbDateStruct;
  maxYear : NgbDateStruct;
  minYear1 : NgbDateStruct;
  maxYear1 : NgbDateStruct;

  constructor(
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    public commonService: CommonService,
    private calendar: NgbCalendar,
  ) {
    this.globalCompanyId = Number(sessionStorage.getItem('companyID'));
    this.financialYear = sessionStorage.getItem('financialYear');
    let [startYear, endYear] = this.financialYear.split("-");

    const currentDate = new Date();
    const currentYear = currentDate.getFullYear();
    const endDate = currentYear === Number(endYear) ?
    this.formatDate(currentDate) :
    this.formatDate(new Date(Number(endYear), 11, 31));

    this.startDate = this.formatDate(new Date(Number(startYear), currentDate.getMonth() + 1, 1));
    this.endDate = this.formatDate(new Date());

    const currentDates = this.calendar.getToday();
    this.startDate = { year: currentDates.year, month: currentDates.month, day: 1 };
    this.endDate = this.calendar.getNext(this.startDate, 'm', 1);


    this.minYear = { year: Number(startYear), month: 4, day: 1 };
    this.maxYear = { year: Number(endYear), month: 3, day: 31 };

    this.minYear1 = { year: Number(startYear), month: 4, day: 1 };
    this.maxYear1 = { year: Number(endYear), month: 3, day: 31 };

  }

  ngbDateToDate(date: NgbDateStruct): Date {
    if (date === null) {
      return null;
    }
    return new Date(date.year, date.month - 1, date.day);
  }

  ngOnInit(): void {

  }

  fun(num:any) :number {
    return Number(num);
  }

  formatToIndianRupees(amount: number): string {

    let [beforeDecimal, afterDecimal] = amount.toFixed(2).split('.');

    beforeDecimal = beforeDecimal.replace(/\B(?=(\d{3})+(?!\d))/g, ',');

    return `₹${beforeDecimal}.${afterDecimal}`;
}

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
    // return `2022-04-01`;
  }

  getPartyList(text: string) {
    if (text.length >= 1) {
      let partyDetails = {
        LedgerData: {
          CompanyId: this.globalCompanyId,
          LedgerType: 'Sales Ledger',
          Country: '',
        },
        SearchText: text,
        Page: {
          Page: '1',
          PageSize: '10',
        },
      };
      this.adminService.getLedgerList(partyDetails).subscribe({
        next: (res: any) => {
          for (let data of res.records) {
            data.ledgerName = `${data.ledgerName} - ${data.place}`;


          }
          // this.partyList = res.records;
          // console.log(this.partyList);

          let OnePartyDetails:party[] = [{
            "ledgerName": "Kapil",
            "ledgerId": 14,
            "place": "Kapil",
            "dealerType": "Kapil",
            "pan": "Kapil",
            "gstn": "Kapil",
            "state": "Kapil",
            "invoiceNo": "Kapil",
            "voucherType": "Kapil"
        }]

        debugger;
        this.partyList = res.records;
        console.log(OnePartyDetails);
        console.log(res.records);


          this.partyList = res.records;
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Something went wrong');
        },
      });
    } else {
      this.partyList = [];
    }
  }


  onClickTest($event: any, item: any) {
    this.ledgerId = item.ledgerId;
  }

  openingBalacnce : any;

  onSubmit() {

    if(this.ledgerId == 0 ) {
      this.toastr.error("Please Select the Party Before Submitting !");
      return;
    }
    this.invoiceList = [];
    this.spinner.show();
    this.getOpeningBalance();
  }

  getOpeningBalance() {

    let partyDetails = {
      Data: {
        LedgerId: this.ledgerId,
        CompanyId: this.globalCompanyId,
        TransDateStart: this.formatDate(this.ngbDateToDate(this.startDate)),
        TransDateEnd: this.formatDate(this.ngbDateToDate(this.endDate))
      }
    };

    this.adminService.GetOpeningBalance(partyDetails).subscribe({
      next: (res: any) => {

        this.openingBalacnce = res.result;

        if(this.openingBalacnce < 0) {
          this.openingBalacnce = this.openingBalacnce * -1;
        }

        this.getInvoiceList();
        this.spinner.hide();
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong');
      },
    });
  }

  getTotalCredit(): number {
    return this.invoiceList.reduce((total, item) => total + (item.Credit || 0), 0);
  }

  getTotalDebit(): number {
    return this.invoiceList.reduce((total, item) => total + (item.Debit || 0), 0);
  }

  closingBal: any[] [];
  finalBal : any;

  calculateOpeningBalance() {
    this.closingBal = [];

    let openingBalance = this.openingBalacnce;
    let temp = 0;

    for (let index = 0; index < this.invoiceList.length; index++) {
      const item = this.invoiceList[index];
      const Credit = item.Credit || 0;
      const Debit = item.Debit || 0;

      if (Credit > 0) {
        openingBalance += Credit;
      }

      if (Debit > 0) {
        openingBalance -= Debit;
      }

      temp = index;
      this.closingBal[index] = openingBalance.toFixed(2);
    }
    this.finalBal = this.closingBal[temp];
  }

  getInvoiceList() {

    let partyDetails = {
      Data: {
        LedgerId: this.ledgerId,
        CompanyId: this.globalCompanyId,
        TransDateStart: this.formatDate(this.ngbDateToDate(this.startDate)),
        TransDateEnd: this.formatDate(this.ngbDateToDate(this.endDate))
      }
    };

    this.adminService.GetVoucherDataForAccountStatementPage(partyDetails).subscribe({
      next: (res: any) => {

        this.invoiceList = res.result;
        this.calculateOpeningBalance();
        this.spinner.hide();
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong');
      },
    });
  }


}
