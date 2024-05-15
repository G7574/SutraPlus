import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { DxReportViewerComponent } from 'devexpress-reporting-angular';
import { ActionId } from 'devexpress-reporting/dx-webdocumentviewer';
import { environment } from 'src/environments/environment';
import { predefinedDateRanges } from 'devexpress-reporting/dx-webdocumentviewer'
import { fetchSetup } from '@devexpress/analytics-core/analytics-utils';
import * as $ from 'jquery';
import { formatDate } from '@angular/common';
import { saveAs } from 'file-saver';
import { HttpClient } from '@angular/common/http';
import { AdminServicesService } from '../../services/admin-services.service';
import { NgbCalendar, NgbDateStruct, NgbDatepickerNavigateEvent } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-month-wise-report',
  templateUrl: './month-wise-report.component.html',
  styleUrls: ['./month-wise-report.component.scss']
})
export class MonthWiseReportComponent implements OnInit {

	model: NgbDateStruct;
  today = inject(NgbCalendar).getToday();

  constructor(private http: HttpClient,  private calendar: NgbCalendar,  private adminService: AdminServicesService) {

  }

  onNavigate(event: NgbDatepickerNavigateEvent) {
    this.startDate = event.next;
  }

  startDate: any;
  endDate: any;
  onchangeValue: string = "All";
  financialYear: string = "";
  minYear : NgbDateStruct;
  maxYear : NgbDateStruct;
  minYear1 : NgbDateStruct;
  maxYear1 : NgbDateStruct;

  ngOnInit(): void {

    // Additional initialization logic can be added here
    // const currentDate1 = new Date();
    // this.startDate = this.formatDate(new Date(currentDate1.getFullYear(), currentDate1.getMonth(), 1));
    // this.endDate = this.formatDate(new Date(currentDate1.getFullYear(), currentDate1.getMonth() + 1, 0));

    this.financialYear = sessionStorage.getItem('financialYear');
    let [startYear, endYear] = this.financialYear.split("-");


    const currentDate = this.calendar.getToday();
    this.startDate = { year: currentDate.year, month: currentDate.month, day: 1 };
    this.endDate = this.calendar.getNext(this.startDate, 'm', 1);

    $("#startDate").val(this.formatDate(this.ngbDateToDate(this.startDate)));
    $("#endDate").val(this.formatDate(this.ngbDateToDate(this.endDate)));

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

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
    // return `2022-04-01`;
  }

  title = 'DXReportDesignerSample';
  // If you use the ASP.NET Core backend:
  getDesignerModelAction = "/DXXRD/GetDesignerModel";
  // The report name.
  //reportName = "MonthView";

  reportName = "MonthView" +
  "&StartDate=" + this.formatDate(new Date(new Date().getFullYear(), new Date().getMonth(), 1)) +
  "&EndDate=" + this.formatDate(new Date(new Date().getFullYear(), new Date().getMonth()+ 1, 0)) +
  "&companyidrecord=" + sessionStorage.getItem('companyID') +
  "&vochtype1=0&vochtype1=99" +
  "|" +
  "dataBaseName=" + sessionStorage.getItem("dataBaseName") +
  "&DataSource=" + sessionStorage.getItem("DataSource") +
  "&UserID=" + sessionStorage.getItem("UserID") +
  "&Password=" + sessionStorage.getItem("Password");


  // reportName = "MonthView" + "&StartDate=" + this.formatDate(new Date(new Date().getFullYear(), new Date().getMonth(), 1)) + "&EndDate=" +
  // this.formatDate(new Date(new Date().getFullYear(), new Date().getMonth()+ 1, 0)) + "&companyidrecord=" + sessionStorage.getItem('companyID') +
  // "&vochtype1=0&vochtype1=99" + "&dataBaseName=" + sessionStorage.getItem("dataBaseName") + "&DataSource=" + sessionStorage.getItem("DataSource") +
  // "&UserID=" + sessionStorage.getItem("UserID") + "&Password=" + sessionStorage.getItem("Password");

  // The backend application URL.
  host = environment.Reportingapi;
  yearSelection!: FormGroup
  currentYear!: number

  @ViewChild(DxReportViewerComponent, { static: false }) viewer: DxReportViewerComponent;
  reportUrl: string = "MonthView";
  // The built-in controller in the back-end ASP.NET Core Reporting application.
  invokeAction: string = '/DXXRDV';


  print() {
    fetchSetup.fetchSettings = {
      headers: { 'BeforeRender': 'BeforeRender' },
      beforeSend: (requestParameters) => {
          requestParameters.credentials = 'include';
      }
  };

    // this.viewer.bindingSender.Print();
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

  ParametersSubmitted(event: any) {
    event.args.Parameters.filter(function (p: any) { return p.Key == "StartDate"; })[0].Value = new Date();
    event.args.Parameters.filter(function (p: any) { return p.Key == "EndDate"; })[0].Value = new Date();
}

setParameterALL() {
  let globalCompanyId = sessionStorage.getItem('companyID');
  this.viewer.bindingSender.OpenReport("MonthView" + "&StartDate=" + this.formatDate(this.ngbDateToDate(this.startDate)) + "&EndDate=" + this.formatDate(this.ngbDateToDate(this.endDate)) + "&companyidrecord=" + globalCompanyId + "&vochtype1=0&vochtype1=99"+
  "|" +
  "dataBaseName=" + sessionStorage.getItem("dataBaseName") +
  "&DataSource=" + sessionStorage.getItem("DataSource") +
  "&UserID=" + sessionStorage.getItem("UserID") +
  "&Password=" + sessionStorage.getItem("Password"));
}

setParameterVP() {
  let globalCompanyId = sessionStorage.getItem('companyID');
  this.viewer.bindingSender.OpenReport(this.reportUrl + "&StartDate=" + this.formatDate(this.ngbDateToDate(this.startDate)) + "&EndDate=" + this.formatDate(this.ngbDateToDate(this.endDate)) + "&companyidrecord=" + globalCompanyId + "&vochtype1=2&vochtype1=4"+
  "|" +
  "dataBaseName=" + sessionStorage.getItem("dataBaseName") +
  "&DataSource=" + sessionStorage.getItem("DataSource") +
  "&UserID=" + sessionStorage.getItem("UserID") +
  "&Password=" + sessionStorage.getItem("Password"));
}
setParameterVS() {
  let globalCompanyId = sessionStorage.getItem('companyID');
  this.viewer.bindingSender.OpenReport(this.reportUrl + "&StartDate=" + this.formatDate(this.ngbDateToDate(this.startDate)) + "&EndDate=" + this.formatDate(this.ngbDateToDate(this.endDate)) + "&companyidrecord=" + globalCompanyId + "&vochtype1=9&vochtype1=13"+
  "|" +
  "dataBaseName=" + sessionStorage.getItem("dataBaseName") +
  "&DataSource=" + sessionStorage.getItem("DataSource") +
  "&UserID=" + sessionStorage.getItem("UserID") +
  "&Password=" + sessionStorage.getItem("Password"));
}
setParameterVPR() {
  let globalCompanyId = sessionStorage.getItem('companyID');
  this.viewer.bindingSender.OpenReport(this.reportUrl + "&StartDate=" + this.formatDate(this.ngbDateToDate(this.startDate)) + "&EndDate=" + this.formatDate(this.ngbDateToDate(this.endDate)) + "&companyidrecord=" + globalCompanyId + "&vochtype1=14"+
  "|" +
  "dataBaseName=" + sessionStorage.getItem("dataBaseName") +
  "&DataSource=" + sessionStorage.getItem("DataSource") +
  "&UserID=" + sessionStorage.getItem("UserID") +
  "&Password=" + sessionStorage.getItem("Password"));
}
setParameterVSR() {
  let globalCompanyId = sessionStorage.getItem('companyID');
  this.viewer.bindingSender.OpenReport(this.reportUrl + "&StartDate=" + this.formatDate(this.ngbDateToDate(this.startDate)) + "&EndDate=" + this.formatDate(this.ngbDateToDate(this.endDate)) + "&companyidrecord=" + globalCompanyId + "&vochtype1=6&vochtype2=6"+
  "|" +
  "dataBaseName=" + sessionStorage.getItem("dataBaseName") +
  "&DataSource=" + sessionStorage.getItem("DataSource") +
  "&UserID=" + sessionStorage.getItem("UserID") +
  "&Password=" + sessionStorage.getItem("Password"));
}

export(format : string) {

  fetch("https://localhost:54688/DXXRD/Export?format=" + format)
  .then(response => response.blob())
  .then(data => {
      console.log(data);
      saveAs(data, 'MonthWiseReport.' + format);

  });

}

onChange(event: any){
  this.onchangeValue = event.target.value;
}

generateRepo() {

  switch (this.onchangeValue)
  {
      case "All":
        this.setParameterALL();
          break;
      case "Purchase":
          this.setParameterVP();
          break;
      case "Sales":
          this.setParameterVS();
          break;
      case "PurchaseReturn":
          this.setParameterVPR();
          break;
      case "SalesReturn":
          this.setParameterVSR();
          break;
      case "CreditNote":
          this.setParameterVCN();
          break;
      case "DebitNote":
          this.setParameterVDN();
          break;
  }
}

setParameterVCN() {
  let globalCompanyId = sessionStorage.getItem('companyID');
  this.viewer.bindingSender.OpenReport(this.reportUrl + "&StartDate=" + this.formatDate(this.ngbDateToDate(this.startDate)) + "&EndDate=" + this.formatDate(this.ngbDateToDate(this.endDate)) + "&companyidrecord=" + globalCompanyId + "&vochtype1=9"+
  "|" +
  "dataBaseName=" + sessionStorage.getItem("dataBaseName") +
  "&DataSource=" + sessionStorage.getItem("DataSource") +
  "&UserID=" + sessionStorage.getItem("UserID") +
  "&Password=" + sessionStorage.getItem("Password"));
}
setParameterVDN() {
  let globalCompanyId = sessionStorage.getItem('companyID');
  this.viewer.bindingSender.OpenReport(this.reportUrl + "&StartDate=" + this.formatDate(this.ngbDateToDate(this.startDate)) + "&EndDate=" + this.formatDate(this.ngbDateToDate(this.endDate)) + "&companyidrecord=" + globalCompanyId + "&vochtype1=15"+
  "|" +
  "dataBaseName=" + sessionStorage.getItem("dataBaseName") +
  "&DataSource=" + sessionStorage.getItem("DataSource") +
  "&UserID=" + sessionStorage.getItem("UserID") +
  "&Password=" + sessionStorage.getItem("Password"));
}


}
