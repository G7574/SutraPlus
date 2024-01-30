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


@Component({
  selector: 'app-item-wise-report',
  templateUrl: './item-wise-report.component.html',
  styleUrls: ['./item-wise-report.component.scss']
})
export class ItemWiseReportComponent implements OnInit {
  startDate: any;
  endDate: any;
  selectMonth: any;
  onchangeValue: string = "";
  onMonthChange: string = "";
  financialYear: string = "";
  startYear : string = "";
  endYear : string = "";

  ngOnInit(): void {

    this.financialYear = sessionStorage.getItem('financialYear');
    let [startYear, endYear] = this.financialYear.split("-");

    this.startYear = startYear;
    this.endYear = endYear;

    // const currentDate = new Date();
    // this.startDate = this.formatDate(new Date(Number(this.startYear), 3, 1));
    // this.endDate = this.formatDate(new Date(Number(this.endYear), 2, 31));

    //this.endDate = this.formatDate(new Date(Number(this.startYear), currentDate.getMonth() + 1, 0));

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
  ) { }

  title = 'DXReportDesignerSample';
  // If you use the ASP.NET Core backend:
  getDesignerModelAction = "/DXXRD/GetDesignerModel";
  // The report name.
  reportName = "ItemWise" + "&StartDate=" + this.formatDate(new Date(Number(this.startYear), new Date().getMonth(), 1)) + "&EndDate=" + this.formatDate(new Date(Number(this.endYear), new Date().getMonth()+ 1, 0)) + "&companyidrecord=" + sessionStorage.getItem('companyID') + "&vochtype1=0&vochtype1=99";
  // The backend application URL.
  host = environment.Reportingapi;
  yearSelection!: FormGroup
  currentYear!: number

  @ViewChild(DxReportViewerComponent, { static: false }) viewer: DxReportViewerComponent;
  reportUrl: string = "ItemWise";

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
  this.viewer.bindingSender.OpenReport("ItemWise" + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=0&vochtype1=99");
}


setParameterVP() {
  let globalCompanyId = sessionStorage.getItem('companyID');
  this.viewer.bindingSender.OpenReport(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=2&vochtype1=4");
}
setParameterVS() {
  let globalCompanyId = sessionStorage.getItem('companyID');
  this.viewer.bindingSender.OpenReport(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=9&vochtype1=13");
}
setParameterVPR() {
  let globalCompanyId = sessionStorage.getItem('companyID');
  this.viewer.bindingSender.OpenReport(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=14");
}
setParameterVSR() {
  let globalCompanyId = sessionStorage.getItem('companyID');
  this.viewer.bindingSender.OpenReport(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=6");
}
setParameterVCN() {
  let globalCompanyId = sessionStorage.getItem('companyID');
  this.viewer.bindingSender.OpenReport(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=9");
}
setParameterVDN() {
  let globalCompanyId = sessionStorage.getItem('companyID');
  this.viewer.bindingSender.OpenReport(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=15");
}

export(format : string) {

  fetch("https://localhost:54688/DXXRD/Export?format=" + format)
  .then(response => response.blob())
  .then(data => {
      console.log(data);
      saveAs(data, 'ItemWiseReport.' + format);

  });

}

onChange(event: any){
  this.onchangeValue = event.target.value;
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

  if (typeof this.startDate === 'undefined') {
    this.toastr.error("Start Date is not selected");
    return;
  } else if(typeof this.endDate === 'undefined') {
    this.toastr.error("End Date is not selected");
    return;
  }

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
      case "" :
        this.toastr.error("Report type is not selected");
        break
  }
}

}
