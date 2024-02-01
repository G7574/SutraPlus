import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnDestroy, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, NavigationStart, Router } from '@angular/router';
import { fetchSetup } from '@devexpress/analytics-core/analytics-utils';
import { DxReportViewerComponent } from 'devexpress-reporting-angular';
import { ActionId } from 'devexpress-reporting/dx-webdocumentviewer';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-item-wise-report-view',
  templateUrl: './item-wise-report-view.component.html',
  styleUrls: ['./item-wise-report-view.component.scss']
})
export class ItemWiseReportViewComponent {

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
    // return `2022-04-01`;
  }

  constructor(private http: HttpClient,private sanitizer: DomSanitizer,private route: ActivatedRoute,private router: Router) {
    this.reportName = sessionStorage.getItem('query');
    this.headerContent = sessionStorage.getItem('headerContent');
    this.handleReportQuery();
  }

  startDate: any;
  endDate: any;
  globalCompanyId: any;
  query: any;
  headerContent: string = "";

   title = 'DXReportDesignerSample';
  // If you use the ASP.NET Core backend:
  getDesignerModelAction = "/DXXRD/GetDesignerModel";
  // The report name.
  reportName = "ItemWise" + "&StartDate=" + this.formatDate(new Date(new Date().getFullYear(), new Date().getMonth(), 1)) + "&EndDate=" + this.formatDate(new Date(new Date().getFullYear(), new Date().getMonth()+ 1, 0)) + "&companyidrecord=" + sessionStorage.getItem('companyID') + "&vochtype1=0&vochtype1=99";
  // The backend application URL.
  host = environment.Reportingapi;
  yearSelection!: FormGroup
  currentYear!: number

  @ViewChild(DxReportViewerComponent, { static: false }) viewer: DxReportViewerComponent;
  reportUrl: string = "ItemWise";

  // The built-in controller in the back-end ASP.NET Core Reporting application.
  invokeAction: string = '/DXXRDV';

  ngOnInit(): void {
    this.reportName = sessionStorage.getItem('query');
    this.headerContent = sessionStorage.getItem('headerContent');
    this.handleReportQuery();
  }

  handleReportQuery() {

    if(sessionStorage.getItem('query') != null) {
      this.query = sessionStorage.getItem('query');
    } else {
      if(this.query != null && this.query != "") {
        this.reportName = this.query;
      }
    }

  }

  @ViewChild('printFrame', { static: true })
  printFrame!: ElementRef;


  printReport() {

    var frameElement = window.open(environment.Reportingapi + "DXXRD/Export?format=pdf", "_blank");

    // frameElement?.addEventListener("load", function (e) {
    //     if (frameElement && frameElement.document.contentType !== "text/html"){
    //       frameElement.print();
    //     }
    // });

    // if (frameElement) {
    //     frameElement.print();
    // } else {
    //     console.error("Failed to open new window");
    // }

  }
      
  onReportOpened(event: any) { ;
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

}
