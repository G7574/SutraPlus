import { Component, OnInit } from '@angular/core';
import { NgbCalendar, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { party } from 'src/app/share/models/party';
import { AdminServicesService } from '../../services/admin-services.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { CommonService } from 'src/app/share/services/common.service';

@Component({
  selector: 'app-trial-balance',
  templateUrl: './trial-balance.component.html',
  styleUrls: ['./trial-balance.component.scss']
})
export class TrialBalanceComponent implements OnInit {
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

  getInvoiceList() {

    let partyDetails = {
      TrialBalance: {
        CompanyId: this.globalCompanyId,
        TransDate: this.formatDate(this.ngbDateToDate(this.startDate)),
      }
    };

    this.adminService.GetTrialBalance(partyDetails).subscribe({
      next: (res: any) => {
        this.groupTotals = res.groupTotals;
        this.invoiceList = res.result;
        this.grandTotalCredit = res.Credit;
        this.grandTotalDebit = res.Debit;

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


}
