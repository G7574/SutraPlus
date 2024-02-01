import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { DxReportViewerComponent } from 'devexpress-reporting-angular';
import { ActionId } from 'devexpress-reporting/dx-webdocumentviewer';
import { environment } from 'src/environments/environment';
import { predefinedDateRanges } from 'devexpress-reporting/dx-webdocumentviewer'
import { fetchSetup } from '@devexpress/analytics-core/analytics-utils';
import * as $ from 'jquery';
import { saveAs } from 'file-saver';
import { end } from 'pdfkit';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { DataService } from 'src/app/core/services/data.service';
import { ca } from 'date-fns/locale';

@Component({
  selector: 'app-common-report-generator',
  templateUrl: './common-report-generator.component.html',
  styleUrls: ['./common-report-generator.component.scss']
})
export class CommonReportGeneratorComponent implements OnInit {

  startDate: any;
  endDate: any;
  selectMonth: any;
  onchangeValue: string = "";
  onMonthChange: string = "";
  financialYear: string = "";
  startYear : string = "";
  endYear : string = "";
  selectedReport : string = "";
  reportUrl: string = "";

  reportName : string = "";
  reportType : string = "";

  ngOnInit(): void {

    this.financialYear = sessionStorage.getItem('financialYear');
    let [startYear, endYear] = this.financialYear.split("-");

    this.startYear = startYear;
    this.endYear = endYear;

    const currentDate = new Date();
    //this.startDate = this.formatDate(new Date(Number(this.startYear), 3, 1));
    //this.endDate = this.formatDate(new Date(Number(this.endYear), 2, 31));

    this.startDate = this.formatDate(new Date(Number(this.startYear), currentDate.getMonth(), 1));
    this.endDate = this.formatDate(new Date(Number(this.startYear), currentDate.getMonth() + 1, 0));

    $("#startDate").val(this.startDate);
    $("#endDate").val(this.endDate);

    // Additional initialization logic can be added here
  }


  onStartDateChange(event: any) {
    const startDateValue = event.target.value;
    const [year, month, day] = startDateValue.split('-');
    this.startDate = this.formatDate(new Date(Number(year), month, day));

//    updating start select month
      //this.startYear = year;

  }

  onEndChange(event: any) {
    const endDateValue = event.target.value;
    const [year, month, day] = endDateValue.split('-');
    this.endDate = this.formatDate(new Date(Number(year), month, day));

//    updating end select month
      //this.endYear = year;

  }

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
    // return `2022-04-01`;
  }

  constructor(
    private toastr: ToastrService,
    private router: Router,
  ) { }

  yearSelection!: FormGroup
  currentYear!: number

export(format : string) {

  fetch("https://localhost:54688/DXXRD/Export?format=" + format)
  .then(response => response.blob())
  .then(data => {
      console.log(data);
      saveAs(data, 'ItemWiseReport.' + format);

  });

}

initReportGen(event: string, report: string){
  this.onchangeValue = event;
  this.selectedReport = report;
  this.generateRepo();
}

onMonthChangeListener(event: any){
  const currentDate = new Date();
  this.onMonthChange = event.target.value;

  let year = Number(this.startYear);

  if(Number(this.onMonthChange) > 3 && Number(this.onMonthChange) < 13) {

  } else {
    year++;
  }

  if(this.onMonthChange.length > 0) {
    this.startDate = this.formatDate(new Date(year, (Number(this.onMonthChange) -1 ), 1));
    this.endDate = this.formatDate(new Date(year, Number(this.onMonthChange), 0));
  } else {
    this.startDate = this.formatDate(new Date(year, currentDate.getMonth(), 1));
    this.endDate = this.formatDate(new Date(year, currentDate.getMonth() + 1, 0));
  }
}

generateRepo() {

  console.log(this.startDate);
  console.log(this.endDate);

  if (typeof this.startDate === 'undefined' || this.startDate === 'NaN-aN-aN') {
    this.toastr.error("Start Date is not selected");
    return;
  } else if(typeof this.endDate === 'undefined' || this.endDate === 'NaN-aN-aN') {
    this.toastr.error("End Date is not selected");
    return;
  } else if (this.startDate > this.endDate) {
    this.toastr.error("End Date must be after Start Date");
    return;
  }

  let globalCompanyId = sessionStorage.getItem('companyID');

  switch(this.selectedReport) {

    case "party-wise" :
      this.reportUrl = "PartyCaseWise";
      this.reportName = "Party Case Wise"
      break;
    case "party-wise-hamali" :
      this.reportUrl = "PartyWiseCommHamali";
      this.reportName = "Party Wise Commission Hamali"
      break;
    case "list-register-wise" :
      this.reportUrl = "ListAndRegisters";
      this.reportName = "List and Register Wise"
      break;
    case "item-wise" :
      this.reportUrl = "ItemWise";
      this.reportName = "Print Register"
      break;

  }

  switch (this.onchangeValue)
  {
      case "All":
          this.reportType = "";
          this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=0&vochtype1=99");
          break;
      case "Purchase":
          this.reportType = "Purchase";
          this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=2&vochtype1=4");
          break;
      case "Sales":
          this.reportType = "Sales";
          this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=9&vochtype1=13");
          break;
      case "PurchaseReturn":
          this.reportType = "Purchare Return";
          this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=14");
          break;
      case "SalesReturn":
          this.reportType = "Sales Return";
          this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=6");
          break;
      case "CreditNote":
          this.reportType = "Credit Note";
          this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=9");
          break;
      case "DebitNote":
          this.reportType = "Debit Note";
          this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=15");
          break;
      case "" :
        this.toastr.error("Report type is not selected");
        break
  }
}

openNewTab(data:any) {

  sessionStorage.setItem('query', data);
  sessionStorage.setItem('headerContent', this.reportName + " " + this.reportType + " Report for the period " + this.startDate + " to " + this.endDate);

  const url = this.router.createUrlTree(['/'], { fragment: 'ReportView' }).toString();
  window.open(url, '_blank');
}

}
