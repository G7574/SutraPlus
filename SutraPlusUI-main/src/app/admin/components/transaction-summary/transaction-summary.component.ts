import { Component, OnInit } from '@angular/core';
import { NgbCalendar, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { AdminServicesService } from '../../services/admin-services.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { CommonService } from 'src/app/share/services/common.service';
import { party } from 'src/app/share/models/party';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-transaction-summary',
  templateUrl: './transaction-summary.component.html',
  styleUrls: ['./transaction-summary.component.scss']
})
export class TransactionSummaryComponent implements OnInit {
  invoiceList: any[] = [];
  startDate: any;
  endDate: any;
  financialYear: string = "";
  partyList: party[] = [];
  groupNameList: party[] = [];
  ledgerId: number = 0;
  totalInvoice: number = 0;
  selectedOption: string = '';
  temp: string = '';

  closingBal: any[] [];
  finalBal : any;
  groupTotals: number[] = [];

  grandTotalDebit : any;
  grandTotalCredit : any;

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
    this.getInvoiceList();
    this.pagesObj.subscribe(res => {
      res ? this.getPages() : '';
    });

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
  }

  getPartyList(text: string) {
    if (text.length >= 1) {
      let partyDetails = {
        TrialBalance: {
          CompanyId: this.globalCompanyId,
        },
      };

      this.adminService.getLedgerList(partyDetails).subscribe({
        next: (res: any) => {

          for (let data of res.records) {
            data.ledgerName = `${data.ledgerName} - ${data.place}`;
          }

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
    this.temp = "";
    this.invoiceList = [];
    this.groupTotals = [];
    this.grandTotalDebit = 0;
    this.grandTotalCredit = 0;
    this.spinner.show();
    this.getInvoiceList();

  }

  onChange(event: any) {
    this.selectedOption = event.target.value;
  }

  ttlCredit : any = 0;
  ttlDebit : any = 0;
  pageNumber: number = 1;
  pages: number[] = [];
  pagesObj = new BehaviorSubject(false);
  pageChanged: boolean = false;
  ttOpeningBalance : any = 0;

  next() {
    this.spinner.show();
    if (this.commonService.gettotalPages(this.totalInvoice) > 5) {
        let lastIndex = (this.pages.length - 1);
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
      this.spinner.show();
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

  getInvoiceList() {
    this.spinner.show();
    let partyDetails = {
      TransactionSummary: {
        CompanyId: this.globalCompanyId,
        fromDate: this.formatDate(this.ngbDateToDate(this.startDate)),
        toDate: this.formatDate(this.ngbDateToDate(this.endDate)),
      },
      Page: {
        "PageNumber": this.pageNumber,
        "PageSize": 5
      },
    };

    this.adminService.GetTransactionSummary(partyDetails).subscribe({
      next: (res: any) => {

        this.totalInvoice = res.totalPages;
        this.pagesObj.next(true);

        this.groupTotals = res.groupTotals;
        this.invoiceList = res.result;
        this.grandTotalCredit = res.Credit;
        this.grandTotalDebit = res.Debit;
        this.ttlCredit = res.totalCredit_;
        this.ttlDebit = res.totalDebit_;
        this.ttOpeningBalance = res.ttOpeningBalance;

        if (this.selectedOption != '') {
          this.invoiceList = [];
          this.temp = this.selectedOption;
          for (let group of res.result) {
            if (group.GroupName === this.selectedOption) {
              this.invoiceList.push(group);
            }
          }
        } else {
          this.temp = '';
          this.invoiceList = res.result;
        }


        this.groupNameList = [];

        for (let index = 0; index < res.result.length; index++) {
          const element = res.result[index].GroupName;
          this.groupNameList.push(element);
        }

        this.grandTotalCredit = this.grandTotalCredit.toFixed(2);
        this.grandTotalDebit = this.grandTotalDebit.toFixed(2);

        this.spinner.hide();
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong');
      },
    });
  }

  calculateDebit(ledgerItems: any[]): number {
    let total = 0;
    for (let item of ledgerItems) {
      if(item.Balance < 0) {
        total += (item.Balance * -1 );
      }
    }
    return total;
  }

  calculateCredit(ledgerItems: any[]): number {
    let total = 0;
    for (let item of ledgerItems) {
      if(item.Balance > 0) {
        total += (item.Balance);
      }
    }
    return total;
  }

  calculateOpeningCreditSum(): number {
    let openingCreditSum: number = 0;

    // Iterate through invoiceList to sum up opening credit values
    for (let group of this.invoiceList) {
      for (let item of group.Data) {
        if (item.OpeningBalance > 0) {
          openingCreditSum += item.OpeningBalance;
        }
      }
    }

    return openingCreditSum;
  }

  // Define a function to calculate opening debit sum
  calculateOpeningDebitSum(): number {
    let openingDebitSum: number = 0;

    // Iterate through invoiceList to sum up opening debit values
    for (let group of this.invoiceList) {
      for (let item of group.Data) {
        if (item.OpeningBalance < 0) {
          openingDebitSum += Math.abs(item.OpeningBalance);
        }
      }
    }

    return openingDebitSum;
  }

  calculateTotalCreditSum(): number {
    let totalCreditSum: number = 0;

    // Iterate through invoiceList to sum up total credit values
    for (let group of this.invoiceList) {
      for (let item of group.Data) {
        if (item.Credit && !isNaN(item.Credit)) {
          totalCreditSum += parseFloat(item.Credit);
        }
      }
    }

    return totalCreditSum;
  }

  // Define a function to calculate total debit sum
  calculateTotalDebitSum(): number {
    let totalDebitSum: number = 0;

    // Iterate through invoiceList to sum up total debit values
    for (let group of this.invoiceList) {
      for (let item of group.Data) {
        if (item.Debit && !isNaN(item.Debit)) {
          totalDebitSum += parseFloat(item.Debit);
        }
      }
    }

    return totalDebitSum;
  }

}
