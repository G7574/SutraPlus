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
import { ca, da } from 'date-fns/locale';
import { party } from 'src/app/share/models/party';
import { AdminServicesService } from '../../services/admin-services.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { isNil } from 'lodash-es';
import { Ledger } from '../sales/models/ladger.model';

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
  partySelected: boolean = false;
  partyList: party[] = [];
  public editVisible = false;
  ledgerId!: number;
  globalCompanyId!: number;
  invType: any;
  companySelected: boolean = false;
  isEinVoice: boolean = false;

  partySelected1: boolean = false;
  partyList1: party[] = [];
  public editVisible1 = false;
  ledgerId1!: number;
  groupId: number = -1;
  globalCompanyId1!: number;
  invType1: any;
  companySelected1: boolean = false;
  isEinVoice1: boolean = false;

  groups: any[] = [];

  onGroupChange(event : any) {
    if(event.target.value) {
      this.groupId = event.target.value;
    } else {
      this.groupId = -1;
    }

  }

  getGroups() {

    this.adminService.getAccountGroup(0).subscribe({
      next: (res: any) => {
        this.groups = res.GetAccountGroups;
      },
      error: (error: any) => {
        console.error('Error fetching banks:', error);
      }
    });

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

        this.partyList = res.records;

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

  getPartyList1(text: string) {
    if (text.length >= 1) {
      let partyDetails1 = {
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
      this.adminService.getLedgerList(partyDetails1).subscribe({
        next: (res: any) => {
          for (let data of res.records) {
            data.ledgerName = `${data.ledgerName} - ${data.place}`;


          }

        this.partyList1 = res.records;

          this.partyList1 = res.records;

        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Something went wrong');
        },
      });
    } else {
      this.partyList1 = [];
    }
  }


  onSelectParty(event : any) {
    if(event) {
      this.ledgerId = event.ledgerId;
    } else {
      this.ledgerId = -1;
    }
  }

  onSelectParty1(event : any) {
    if(event) {
      this.ledgerId1 = event.ledgerId;
    } else {
      this.ledgerId1 = -1;
    }
  }

  onClickTest($event: any, item: any) {

  }

  onClickTest1($event: any, item: any) {

  }

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

    this.validateEinvoiceEnabled(this.globalCompanyId);

    this.getGroups();

    // Additional initialization logic can be added here
  }

  validateEinvoiceEnabled(companyId: Number): void {
    this.adminService.getSingleCompany(companyId).subscribe({
      next: (res: any) => {
        // console.log(res.CompanyList);
        if (res) {
          this.companySelected = true;
          this.isEinVoice = !isNil(res.CompanyList) ? true : false;
        }
      },
      error: (error: any) => {
        this.toastr.error('Something went wrong');
      },
    });
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
    private spinner: NgxSpinnerService,
    private adminService: AdminServicesService,
  ) {
    this.globalCompanyId = Number(sessionStorage.getItem('companyID'));
  }

  yearSelection!: FormGroup
  currentYear!: number

export(format : string) {

  fetch(environment.Reportingapi + "/DXXRD/Export?format=" + format)
  .then(response => response.blob())
  .then(data => {
      console.log(data);
      saveAs(data, 'ItemWiseReport.' + format);

  });

}

initReportGen(event: string, report: string,bol : boolean){
  this.onchangeValue = event;
  this.selectedReport = report;
  this.generateRepo(bol);
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

generatePPPR(){
  this.reportName = "Print Party Report"
  if(this.groupId > 0) {
    this.openNewTab("PrintPartyReport" + "&StartDate=" + this.formatDate(new Date(new Date().getFullYear(), new Date().getMonth(), 1)) + "&EndDate=" + this.formatDate(new Date(new Date().getFullYear(), new Date().getMonth()+ 1, 0)) + "&companyidrecord=" + sessionStorage.getItem('companyID') + "&vochtype1=0&vochtype1=99" + "&accountinggroupId=" + this.groupId);
  } else {
    this.openNewTab("PrintPartyReport" + "&StartDate=" + this.formatDate(new Date(new Date().getFullYear(), new Date().getMonth(), 1)) + "&EndDate=" + this.formatDate(new Date(new Date().getFullYear(), new Date().getMonth()+ 1, 0)) + "&companyidrecord=" + sessionStorage.getItem('companyID') + "&vochtype1=0&vochtype1=99");
  }
}

generateRepo(selected : boolean) {

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

  console.log( "selected -> " + selected);
  console.log( "ledgerId -> " + this.ledgerId);
  console.log( "ledgerId1 -> " + this.ledgerId1);

  if(selected) {
    switch (this.onchangeValue)
    {
        case "All":
            this.reportType = "";
            this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=0&vochtype1=99");
            break;
        case "Purchase":
            this.reportType = "Purchase";
            if(this.ledgerId != null && this.ledgerId > 0) {
              this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=2&vochtype1=4" + "&ledgerId=" + this.ledgerId);
            } else {
              this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=2&vochtype1=4");
            }
            break;
        case "Sales":
            this.reportType = "Sales";
            if(this.ledgerId != null && this.ledgerId > 0) {
              this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=9&vochtype1=13" + "&ledgerId=" + this.ledgerId);
            } else {
              this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=9&vochtype1=13");
            }
            break;
        case "PurchaseReturn":
            this.reportType = "Purchare Return";
            if(this.ledgerId != null && this.ledgerId > 0) {
              this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=14" + "&ledgerId=" + this.ledgerId);
            } else {
              this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=14");
            }
            break;
        case "SalesReturn":
            this.reportType = "Sales Return";
            if(this.ledgerId != null && this.ledgerId > 0) {
              this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=6" + "&ledgerId=" + this.ledgerId);
            } else {
              this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=6");
            }
            break;
        case "CreditNote":
            this.reportType = "Credit Note";
            if(this.ledgerId != null && this.ledgerId > 0) {
              this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=9" + "&ledgerId=" + this.ledgerId);
            } else {
              this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=9");
            }
            break;
        case "DebitNote":
            this.reportType = "Debit Note";
            if(this.ledgerId != null && this.ledgerId > 0) {
              this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=15" + "&ledgerId=" + this.ledgerId);
            } else {
              this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=15");
            }
            break;
        case "" :
          this.toastr.error("Report type is not selected");
          break
    }
  } else {
    switch (this.onchangeValue)
  {
      case "All":
          this.reportType = "";
          this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=0&vochtype1=99");
          break;
      case "Purchase":
          this.reportType = "Purchase";
          if(this.ledgerId1 != null && this.ledgerId1 > 0) {
            this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=2&vochtype1=4" + "&ledgerId=" + this.ledgerId1);
          } else {
            this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=2&vochtype1=4");
          }
          break;
      case "Sales":
          this.reportType = "Sales";
          if(this.ledgerId1 != null && this.ledgerId1 > 0) {
            this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=9&vochtype1=13" + "&ledgerId=" + this.ledgerId1);
          } else {
            this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=9&vochtype1=13");
          }
          break;
      case "PurchaseReturn":
          this.reportType = "Purchare Return";
          if(this.ledgerId1 != null && this.ledgerId1 > 0) {
            this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=14" + "&ledgerId=" + this.ledgerId1);
          } else {
            this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=14");
          }
          break;
      case "SalesReturn":
          this.reportType = "Sales Return";
          if(this.ledgerId1 != null && this.ledgerId1 > 0) {
            this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=6" + "&ledgerId=" + this.ledgerId1);
          } else {
            this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=6");
          }
          break;
      case "CreditNote":
          this.reportType = "Credit Note";
          if(this.ledgerId1 != null && this.ledgerId1 > 0) {
            this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=9" + "&ledgerId=" + this.ledgerId1);
          } else {
            this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=9");
          }
          break;
      case "DebitNote":
          this.reportType = "Debit Note";
          if(this.ledgerId1 != null && this.ledgerId1 > 0) {
            this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=15" + "&ledgerId=" + this.ledgerId1);
          } else {
            this.openNewTab(this.reportUrl + "&StartDate=" + $("#startDate").val() + "&EndDate=" + $("#endDate").val() + "&companyidrecord=" + globalCompanyId + "&vochtype1=15");
          }
          break;
      case "" :
        this.toastr.error("Report type is not selected");
        break
  }
  }


}

openNewTab(data:any) {
  console.log( "query -> " + data);
  sessionStorage.setItem('query', data);
  sessionStorage.setItem('headerContent', this.reportName + " " + this.reportType + " Report for the period " + this.startDate + " to " + this.endDate);

  const url = this.router.createUrlTree(['/'], { fragment: 'ReportView' }).toString();
  window.open(url, '_blank');
}

}
