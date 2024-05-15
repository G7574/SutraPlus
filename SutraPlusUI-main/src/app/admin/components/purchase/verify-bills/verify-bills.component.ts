import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { start } from 'repl';
import { AdminServicesService } from 'src/app/admin/services/admin-services.service';
import { Ledger } from '../../sales/models/ladger.model';
import { NgbCalendar, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-verify-bills',
  templateUrl: './verify-bills.component.html',
  styleUrls: ['./verify-bills.component.scss']
})
export class VerifyBillsComponent implements OnInit {

  globalCompanyId: any;
  entries: any[] = [];
  commoditiesEntries: any[] = [];
  startDate: any;
  endDate: any;
  filteredEntries: any[] = [];

  ngOnInit(): void {

    this.financialYear = sessionStorage.getItem('financialYear');
    let [startYear, endYear] = this.financialYear.split("-");

    const currentDate = this.calendar.getToday();
    this.startDate = { year: currentDate.year, month: currentDate.month, day: 1 };
    this.endDate = this.calendar.getNext(this.startDate, 'm', 1);

    this.minYear = { year: Number(startYear), month: 4, day: 1 };
    this.maxYear = { year: Number(endYear), month: 3, day: 31 };

    this.minYear1 = { year: Number(startYear), month: 4, day: 1 };
    this.maxYear1 = { year: Number(endYear), month: 3, day: 31 };
 
    // this.startDate = (new Date()).toISOString().substring(0,10);
    // this.endDate = (new Date()).toISOString().substring(0,10);
    this.getData();
  }

  ngbDateToDate(date: NgbDateStruct): Date {
    if (date === null) {
      return null;
    }
    return new Date(date.year, date.month - 1, date.day);
  }

  onPost() {
    const entriesWithBillNumbers = this.filteredEntries.filter(entry => entry.billNumber);
    const Data = {
      "CompanyId": this.globalCompanyId,
      data: {
        InvioceData: entriesWithBillNumbers
      }
    };
    this.adminService.UpdateInvoice(Data).subscribe({
      next: (res: any) => {
        this.getData();
        this.toastr.success('Record updated successfully.');
        this.entries = res.Data;
        this.filteredEntries = this.entries;
        this.commoditiesEntries = res.commodityData;
        this.spinner.hide();

      },
      error: (error: any) => {
        this.spinner.hide();

      },
    });

  }

  financialYear: string = "";
  minYear : NgbDateStruct;
  maxYear : NgbDateStruct;

  minYear1 : NgbDateStruct;
  maxYear1 : NgbDateStruct;

  constructor(private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private calendar: NgbCalendar,
    private adminService: AdminServicesService) {
      this.globalCompanyId = sessionStorage.getItem('companyID');
  }

  // filterData() {
  //   this.filteredEntries = this.entries.filter(entry => {
  //     const entryDate = entry.tranctdate.substring(0, 10);
  //     return entryDate >= this.startDate && entryDate <= this.endDate;
  //   });
  // }

  filterData() {
    this.filteredEntries = this.entries.filter(entry => {
      const entryDate = new Date(entry.tranctdate.substring(0, 10));
      const startDate = new Date((this.formatDate(this.ngbDateToDate(this.startDate))));
      const endDate = new Date((this.formatDate(this.ngbDateToDate(this.endDate))));
      return entryDate >= startDate && entryDate <= endDate;
    });
  }


  onEditClick(index: any) {
    sessionStorage.setItem("editVerifyBillData",JSON.stringify(this.filteredEntries[index]));
    this.router.navigateByUrl('/admin/EditVerifyBill');
  }

  onUpdateClick(index: any) {
    sessionStorage.setItem("editVerifyBillData",JSON.stringify(this.filteredEntries[index]));

    this.spinner.show();

    let akadaData = {
      "AkadaData": {
        "CompanyId": this.globalCompanyId,
        "LedgerId": this.filteredEntries[index].ledger_id,
        "CommodityId": this.filteredEntries[index].commodityId,
        "LotNo": this.filteredEntries[index].lotNo,
        "PartyInvoiceNumber": this.filteredEntries[index].billNumber
      }
    }

    this.adminService.UpdatePartyInvoiceNumber(akadaData).subscribe({
      next: (res: any) => {
        this.spinner.hide();
        this.getData();
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong');
      },
    });

  }

  onStartDateChange(event:any){
    const startDateValue = event.target.value;
    const [year, month, day] = startDateValue.split('-');
    this.startDate = this.formatDate(new Date(Number(year), month, day));
  }

  onEndChange(event: any) {
    const endDateValue = event.target.value;
    const [year, month, day] = endDateValue.split('-');
    this.endDate = this.formatDate(new Date(Number(year), month, day));
  }

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth())).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
  }

  getTotal(property: string): number {
    return this.filteredEntries.reduce((total, entry) => total + entry[property], 0);
  }

  getData() {

    this.spinner.show();

    let akadaData = {
      "AkadaData": {
        "CompanyId": this.globalCompanyId,
      }
    }

    this.adminService.GetBills(akadaData).subscribe({
      next: (res: any) => {

        this.entries = res.Data;
        this.filteredEntries = this.entries;
        this.filteredEntries.forEach(element => {
          // var roundOff = Grand_Total - Convert.ToDouble(Grand_Total), 2);

          // Grand_Total = Math.Round(Convert.ToDouble(Grand_Total + roundOff), 2);
        });
        this.commoditiesEntries = res.commodityData;
        this.spinner.hide();

      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong');
      },
    });

  }

}
