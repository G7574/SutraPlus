import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { AdminServicesService } from '../../services/admin-services.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { CommonService } from 'src/app/share/services/common.service';
import Swal from 'sweetalert2';
import * as XLSX from 'xlsx';
import { NgbCalendar, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-print-akada',
  templateUrl: './print-akada.component.html',
  styleUrls: ['./print-akada.component.scss']
})
export class PrintAkadaComponent implements OnInit {
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
  minYear : NgbDateStruct;
  maxYear : NgbDateStruct;

  constructor(
    private router: Router,
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    public commonService: CommonService,
    private calendar: NgbCalendar
  ) { }

  ngOnInit(): void {
    this.financialYear = sessionStorage.getItem('financialYear');
    let [startYear, endYear] = this.financialYear.split("-");
    this.customerCode = sessionStorage.getItem('globalCustomerCode');
    this.userDetails = sessionStorage.getItem('userDetails');
    this.userDetails = JSON.parse(this.userDetails);
    this.userEmail = this.userDetails?.result?.UserEmailId;
    this.globalCompanyId = sessionStorage.getItem('companyID');

    const currentDate = this.calendar.getToday();
    this.date = { year: currentDate.year, month: currentDate.month, day: 1 };
    
    this.minYear = { year: Number(startYear), month: 4, day: 1 };
    this.maxYear = { year: Number(endYear), month: 3, day: 31 };


    this.getInvoiceList();
  }

  ngbDateToDate(date: NgbDateStruct): Date {
    if (date === null) {
      return null;
    }
    return new Date(date.year, date.month - 1, date.day);
  }


  onStartDateChange(event: any) {
    const startDateValue = event.target.value;
    this.date = startDateValue;
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
        TranctDate: this.formatDate(this.ngbDateToDate(this.date)),
        Search: "",
      },
    };

    this.adminService.GetAkadaData(partyDetails).subscribe({
      next: (res: any) => {
        this.spinner.show();
        if (res?.Data !== null) {
          this.invoiceList = res.Data;
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

    if(this.searchText == undefined || this.searchText == null) {
      this.searchText = "";
    }

    let partyDetails = {
      GetAkadaData: {
        CompanyId: this.globalCompanyId,
        TranctDate: this.formatDate(this.ngbDateToDate(this.date)),
        Search: this.searchText,
      },
    };

    this.adminService.GetDataMyMark(partyDetails).subscribe({
      next: (res: any) => {
        if (res?.GetAkadaData !== null) {
          this.invoiceList = res.Data;
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

  onClickTest($event: any, item: any) {
    this.searchText = item;
  }


  print() {
    window.print();
  }

  exportAsExcel() {
    const customLines = [
      ['Search Filter'],
      ['Date',this.formatDate(this.ngbDateToDate(this.date))],
      ['Search Text',this.searchText]
    ];


    const headers = ['Sr.No.','Party Name',	'Lot No.',	'Mark',	'Bags',	'Weight',	'Rate',	'Amount'	,'Individual' ,'Weights'];
    const excelData = [
      ...customLines,
      headers,
      ...this.invoiceList.map((item, index) => [
        index + 1,
        `${item.PartyName}`,
        item.LotNo,
        item.Mark,
        item.NoOfBags,
        item.TotalWeight,
        item.Rate,
        item.Amount.toFixed(2),
        item.Individualeights
      ])
    ];

    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(excelData);
    const workbook: XLSX.WorkBook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };
    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    this.saveAsExcelFile(excelBuffer, 'Akada Details');
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


  getPartyList(text: string) {
    if (text.length >= 1) {

      let partyDetails = {
        GetAkadaData: {
          CompanyId: this.globalCompanyId,
          TranctDate: this.formatDate(this.ngbDateToDate(this.date)),
          Search: text,
        },
      };

      this.adminService.GetMarks(partyDetails).subscribe({
        next: (res: any) => {
          this.partyList = res.Data;
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

}
