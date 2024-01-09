import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { CommonService } from 'src/app/share/services/common.service';
import { AdminServicesService } from '../services/admin-services.service';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { MatTableDataSource } from '@angular/material/table';
import { ExportAsService, ExportAsConfig } from 'ngx-export-as';
import { ReportDTO, MOCK_DATA } from 'src/app/admin/interfaces/reportDTO';
import jsPDF from 'jspdf';
import 'jspdf-autotable';
import { format } from 'date-fns';

@Component({
  selector: 'app-reports-main',
  templateUrl: './reports-main.component.html',
  styleUrls: ['./reports-main.component.scss'],
})
export class ReportsMainComponent implements OnInit {
  financialYear: any;
  userDetails: any;
  userEmail: any;
  customerCode: any;
  globalCompanyId: any;
  reportType: string | null = null;
  reportTitle: string | null = null;
  startDate: Date | null = null;
  endDate: Date | null = null;

  displayedColumns: string[] = [];
  columnLabels: string[] = [];
  // dataSource =   dataSource = new MatTableDataSource<ReportDTO>(MOCK_DATA);;
  dataSource: any[] = [];

  exportConfig: ExportAsConfig = {
    type: 'pdf',
    elementIdOrContent: 'reportTable',
    download: true,
    options: {
      jsPDF: {
        orientation: 'landscape',
        unit: 'pt',
        format: 'a4',
        compressPDF: true,
        margins: {
          top: 30, // Top margin
          bottom: 30, // Bottom margin
          left: 30, // Left margin
          right: 30, // Right margin
        },
      },
      pdfCallbackFn: this.pdfCallbackFn,
    },
  };

  reportData: any[] = [];
  error: any;
  errorMsg!: boolean;
  searchText!: string;
  pageNumber: number = 1;
  pages: number[] = [];
  pagesObj = new BehaviorSubject(false);
  pageChanged: boolean = false;

  // for pdf
  constructor(
    private router: Router,
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    public commonService: CommonService,
    private activatedRoute: ActivatedRoute,
    private dialog: MatDialog,
    private exportAsService: ExportAsService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.financialYear = sessionStorage.getItem('financialYear');
    this.customerCode = sessionStorage.getItem('globalCustomerCode');
    this.userDetails = sessionStorage.getItem('userDetails');
    this.userDetails = JSON.parse(this.userDetails);
    this.userEmail = this.userDetails?.result?.UserEmailId;
    this.globalCompanyId = sessionStorage.getItem('companyID');

    this.activatedRoute.queryParams.subscribe((params) => {

      this.reportType = params['ReportName'];
      this.reportTitle = camelCaseToWords(params['ReportName']);
      // this.getReport();
      this.displayedColumns = [];
      this.columnLabels = [];
      this.dataSource = [];

      // Trigger change detection
      this.cdr.detectChanges();
    });

  }

  initializeColumnsByReportType() {
    if (this.reportType === 'PurchaseRegister') {
      this.displayedColumns = [
        'ledgerName',
        // 'voucherName',
        'partyInvoiceNumber',
        'gstin',
        'sgst',
        'cgst',
        'igst',
        // 'vochType',
        // 'vochNo',
        'tranctDate',
        'noOfBags',
        'totalWeight',
        'amount',
      ];
      this.columnLabels = [
        'Date',
        'Party Name',
        'GSTIN',
        'Bill Number',
        'No Of Bags',
        'Weight',
        'Taxable Value',
        'SGST',
        'IGST',
        'CGST',
      ];
    } else if (this.reportType === 'MonthwisePurchase') {
      this.displayedColumns = [
        'MonthNo',
        'BasicValue',
        'Taxable',
        'SGST',
        'CGST',
        'IGST',
        'Others',
        'Bill Amount',
      ];
      this.columnLabels = [
        'Month',
        'Basic Value',
        'Taxable Value',
        'SGST',
        'CGST',
        'IGST',
        'Others',
        'Bill Amount',
      ];
    } else if (this.reportType === 'TrialBalance') {
      this.displayedColumns = [
        'GroupName',
        'AccountingGroupId',
        'LedgerName',
        'Place',
        'Credit',
        'Debit'
      ];
      this.columnLabels = [
        'Group Name',
        'Accounting Group Id',
        'Ledger Name',
        'Place',
        'Credit',
        'Debit'
      ];
    } else if (this.reportType === 'PaymentList') {
      this.displayedColumns = [
        'LedgerName',
        'Place',
        'YadiBalance',
        'AccountBalance',
        'AsOnDate'
      ];
      this.columnLabels = [
        'Ledger Name',
        'Place',
        'Yadi Balance',
        'Account Balance',
        'As On Date'
      ];
    } else if (this.reportType === 'StockLedger') {
      this.displayedColumns = [
        'Obstock',
        'Obvalue',
        'PurchaseQty',
        'PurchaseValue',
        'SalesReturnQty',
        'SalesReturnValue',
        'FromProductionQty',
        'FromProductionValue',
        'OnwSalesQty',
        'OnwSalesValue',
        'ToProduction',
        'ClosingStock',
        'Average',
        'ClosingValue'
      ];
      this.columnLabels = [
        'Opening Stock',
        'Opening Value',
        'Purchase Quantity',
        'Purchase Value',
        'Sales Return Quantity',
        'Sales Return Value',
        'From Production Quantity',
        'From Production Value',
        'Own Sales Quantity',
        'Own Sales Value',
        'To Production',
        'Closing Stock',
        'Average',
        'Closing Value'
      ];
    } else if (this.reportType === 'PartywiseTDS') {
      this.displayedColumns = [
        'CompanyName',
        'AddressLine1',
        'Place',
        'GSTIN',
        'PAN',
        'LedgerName',
        'Ledger_GSTIN',
        'Ledger_PAN',
        'TotalCommission',
        'TDSDeducted',
        'CommissionTDS'
      ];
      this.columnLabels = [
        'Company Name',
        'Address Line 1',
        'Place',
        'GSTIN',
        'PAN',
        'Ledger Name',
        'Ledger GSTIN',
        'Ledger PAN',
        'Total Commission',
        'TDS Deducted',
        'Commission TDS'
      ];
    } else if (this.reportType === 'TransactionSummary') {
      this.displayedColumns = [
        'GroupName',
        'AccountingGroupId',
        'LedgerName',
        'AddressLine1',
        'Place',
        'Gstn',
        'PAN',
        'Credit',
        'Debit'
      ];
      this.columnLabels = [
        'Group Name',
        'Accounting Group Id',
        'Ledger Name',
        'Address Line 1',
        'Place',
        'GSTN',
        'PAN',
        'Credit',
        'Debit'
      ];
    } else if (this.reportType === 'PartywiseCess') {
      this.displayedColumns = [
        'PartyName',
        'GSTIN',
        'PAN',
        'Commission',
        'Cess'
      ];
      this.columnLabels = [
        'Party Name',
        'GSTIN',
        'PAN',
        'Commission',
        'Cess'
      ];
    } else if (this.reportType === 'PartyList') {
      this.displayedColumns = [
        'Name',
        'Address1',
        'Address2',
        'Place',
        'PIN',
        'State',
        'GSTIN',
        'PAN'
      ];
      this.columnLabels = [
        'Name',
        'Address-1',
        'Address-2',
        'Place',
        'PIN',
        'State',
        'GSTIN',
        'PAN'
      ];
    } else if (this.reportType === 'PartywisePurchase') {
      this.displayedColumns = [
        'Date',
        'BillNumber',
        'Weight',
        'TaxableValue',
        'SGST',
        'CGST',
        'IGST',
        'Others',
        'BillAmount'
      ];
      this.columnLabels = [
        'Date',
        'Bill Number',
        'Weight',
        'Taxable Value',
        'SGST',
        'CGST',
        'IGST',
        'Others',
        'Bill Amount'
      ];
    } else if (this.reportType === 'DayBook') {
      this.displayedColumns = [
        'Particulars',
        'Credit',
        'Debit'
      ];
      this.columnLabels = [
        'Particulars',
        'Credit',
        'Debit'
      ];
    } else if (this.reportType === 'AccountStatement') {
      this.displayedColumns = [
        'Date',
        'Transaction',
        'DocumentNo',
        'Narration',
        'Credit',
        'Debit',
        'RunningBal'
      ];
      this.columnLabels = [
        'Date',
        'Transaction',
        'Document No',
        'Narration',
        'Credit',
        'Debit',
        'Running Balance'
      ];
    }

  }

  pdfCallbackFn(pdf: any) {
    const noOfPages = pdf.internal.getNumberOfPages();
    for (let i = 1; i <= noOfPages; i++) {
      //pdf.setPage(i);
      // Add page number to footer
      pdf.text(
        // 'Page ' + i + ' of ' + noOfPages,
        'Page ' + i,
        pdf.internal.pageSize.getWidth() / 2,
        pdf.internal.pageSize.getHeight() - 30
      );
      // Add page number to header
      // pdf.text(
      //   'Page ' + i + ' of ' + noOfPages,
      //   pdf.internal.pageSize.getWidth() / 2,
      //   30
      // );
    }
  }

  //   this.exportAsService
  //     .save(this.exportConfig, 'exported-data')
  //     .subscribe(() => { });
  // }

  exportToExcel() {
    this.exportConfig.type = 'csv';
    this.exportConfig.elementIdOrContent = 'reportTable';
    this.exportAsService
      .save(this.exportConfig, 'exported-data')
      .subscribe(() => { });
  }

  getReport() {
    this.initializeColumnsByReportType();

    console.log('From date: ' + this.startDate);
    console.log('To date: ' + this.endDate);

    let reportDetails = {
      SearchText: this.searchText || '',
      // Page: {
      //   PageNumber: this.pageNumber,
      //   PageSize: 20,
      // },
      ReportData: {
        CompanyId: this.globalCompanyId,
        ReportType: this.reportType,
        StartDate: this.startDate,
        EndDate: this.endDate,
      },
    };
    console.log('get  report data', reportDetails);

    this.spinner.show();
    this.adminService.getReport(reportDetails).subscribe({
      next: (res: any) => {
        console.log(`get ${this.reportType} report data = `, res);
        if (!res.HasErrors && res?.Data !== null) {
          // this.reportData = res.records;
          this.dataSource = res;
          // this.getPages();
          //this.pagesObj.next(true);
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

  formatDate(isoDateString: string | null): string | null {
    if (isoDateString === null) {
      return null; // or return a default value or throw an error
    }

    const date = new Date(isoDateString);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // January is 0!
    const year = date.getFullYear();

    return `${day}-${month}-${year}`;
  }

  exportToPDF() {
    const doc = new jsPDF('landscape') as any;
    const self = this; // Create a reference to `this`

    let columns: string[] = this.columnLabels;
    let data: string[][] = [];

    if (this.reportType === 'PurchaseRegister') {
      data = this.dataSource.map((row) => [
        this.formatDate(row.TranctDate) || ' - ',
        row.LedgerName || ' - ',
        row.Gstin || ' - ',
        row.PartyInvoiceNumber || ' - ',
        row.NoOfBags || ' - ',
        row.TotalWeight || ' - ',
        row.Amount || ' - ',
        row.SGST || ' - ',
        row.IGST || ' - ',
        row.CGST || ' - ',
        // row.VoucherName || ' - ',
        // row.VochType || ' - ',
        // row.VochNo || ' - ',
      ]);
    } else if (this.reportType === 'MonthwisePurchase') {
      data = this.dataSource.map((row) => [
        this.mapMonthNumberToName(row.MonthNo) || ' - ',
        row.BasicValue || ' - ',
        row.Taxable || ' - ',
        row.SGST || ' - ',
        row.CGST || ' - ',
        row.IGST || ' - ',
        row.Others || ' - ',
        row.BillAmount || ' - ',
      ]);
    } else if (this.reportType === 'TrialBalance') {
      data = this.dataSource.map((row) => [
        row.GroupName || ' - ',
        row.AccountingGroupId || ' - ',
        row.LedgerName || ' - ',
        row.Place || ' - ',
        row.Credit || ' - ',
        row.Debit || ' - ',
      ]);
    } else if (this.reportType === 'PaymentList') {
      data = this.dataSource.map((row) => [
        row.LedgerName || ' - ',
        row.Place || ' - ',
        row.YadiBalance || ' - ',
        row.AccountBalance || ' - ',
        this.formatDate(row.AsOnDate) || ' - ',
      ]);
    } else if (this.reportType === 'PartywiseTDS') {
      data = this.dataSource.map((row) => [
        row.CompanyName || ' - ',
        row.AddressLine1 || ' - ',
        row.Place || ' - ',
        row.GSTIN || ' - ',
        row.PAN || ' - ',
        row.LedgerName || ' - ',
        row.Ledger_GSTIN || ' - ',
        row.Ledger_PAN || ' - ',
        row.TotalCommission || ' - ',
        row.TDSDeducted || ' - ',
        row.CommissionTDS || ' - ',
      ]);
    } else if (this.reportType === 'StockLedger') {
      data = this.dataSource.map((row) => [
        row.Obstock || ' - ',
        row.Obvalue || ' - ',
        row.PurchaseQty || ' - ',
        row.PurchaseValue || ' - ',
        row.SalesReturnQty || ' - ',
        row.SalesReturnValue || ' - ',
        row.FromProductionQty || ' - ',
        row.FromProductionValue || ' - ',
        row.OnwSalesQty || ' - ',
        row.OnwSalesValue || ' - ',
        row.ToProduction || ' - ',
        row.ClosingStock || ' - ',
        row.Average || ' - ',
        row.ClosingValue || ' - ',
      ]);
    } else if (this.reportType === 'TransactionSummary') {
      data = this.dataSource.map((row) => [
        row.GroupName || ' - ',
        row.AccountingGroupId || ' - ',
        row.LedgerName || ' - ',
        row.AddressLine1 || ' - ',
        row.Place || ' - ',
        row.Gstn || ' - ',
        row.PAN || ' - ',
        row.Credit || ' - ',
        row.Debit || ' - ',
      ]);
    } else if (this.reportType === 'PartywiseCess') {
      data = this.dataSource.map((row) => [
        row.PartyName || ' - ',
        row.GSTIN || ' - ',
        row.PAN || ' - ',
        row.Commission || ' - ',
        row.Cess || ' - ',
      ]);
    } else if (this.reportType === 'PartyList') {
      data = this.dataSource.map((row) => [
        row.Name || ' - ',
        row.Address1 || ' - ',
        row.Address2 || ' - ',
        row.Place || ' - ',
        row.PIN || ' - ',
        row.State || ' - ',
        row.GSTIN || ' - ',
        row.PAN || ' - ',
      ]);
    } else if (this.reportType === 'PartywisePurchase') {
      data = this.dataSource.map((row) => [
        row.Date || ' - ',
        row.BillNumber || ' - ',
        row.Weight || ' - ',
        row.TaxableValue || ' - ',
        row.SGST || ' - ',
        row.CGST || ' - ',
        row.IGST || ' - ',
        row.Others || ' - ',
        row.BillAmount || ' - ',
      ]);
    } else if (this.reportType === 'DayBook') {
      data = this.dataSource.map((row) => [
        row.Particulars || ' - ',
        row.Credit || ' - ',
        row.Debit || ' - ',
      ]);
    } else if (this.reportType === 'AccountStatement') {
      data = this.dataSource.map((row) => [
        row.Date || ' - ',
        row.Transaction || ' - ',
        row.DocumentNo || ' - ',
        row.Narration || ' - ',
        row.Credit || ' - ',
        row.Debit || ' - ',
        row.RunningBal || ' - ',
      ]);
    }



    else {
      // Default columns or handle other report types
    }

    doc.autoTable({
      margin: { top: 50 },
      head: [columns],
      body: data,
      styles: { halign: 'right' }, // This will center align all cells
      columnStyles: self.reportType === 'PartywiseTDS' ? {
        0: { cellWidth: 30, halign: 'left' },
        1: { cellWidth: 30 },
        2: { cellWidth: 30 },
        3: { cellWidth: 30 },
      } : { 0: { halign: 'left' } }, // This will right align the first column. Add more columns as needed.

      didDrawPage: function (data: any) {
        // Add header and footer on each page
        // doc.text('Header', data.settings.margin.left, 10);

        // Add header and footer on each page
        doc.setFontSize(12);
        const pageCenter = doc.internal.pageSize.width / 2;
        doc.text(`Company Name: ${data.companyName || ' - '}`, pageCenter, 20, {
          align: 'center',
        });
        doc.text(
          `Address Line 1: ${data.addressLine1 || ' - '}, Address Line 2: ${data.addressLine2 || ' - '
          }, Place: ${data.place || ' - '}`,
          pageCenter,
          28,
          { align: 'center' }
        );

        if (self.startDate !== null && self.endDate !== null && self.reportType !== 'MonthwisePurchase') {
          doc.text(
            `${self.reportTitle || 'Report'} From ${self.startDate} TO ${self.endDate}`,
            pageCenter,
            36,
            { align: 'center' }
          );
        } else if (self.reportType === 'MonthwisePurchase') {
          doc.text(
            `${self.reportTitle || 'Report'}`,
            pageCenter,
            36,
            { align: 'center' }
          );
        } else {
          doc.text(
            `${self.reportTitle || 'Report'}`,
            pageCenter,
            36,
            { align: 'center' }
          );
        }
        // doc.text('Footer', data.settings.margin.left, doc.internal.pageSize.height - 10);

        // Add page number
        doc.setFontSize(10);
        var str = 'Page ' + doc.internal.getNumberOfPages();
        // ' of ' +
        // doc.internal.getNumberOfPages();
        doc.text(str, data.settings.margin.left + 250, 45); // Adjust the position according to your needs
      },
    });

    // Get current date and time
    const currentDate = new Date();

    // Format the date as DD-MM-YYYY
    const formattedDate = format(currentDate, 'dd-MM-yyyy');

    // Save the report with the formatted date in the file name
    doc.save(`${this.reportType}_${formattedDate}.pdf`);
  }

  mapMonthNumberToName(monthNumber: string): string {
    if (monthNumber === 'Total') {
      return 'Total';
    }
    const monthNo = parseInt(monthNumber, 10);
    return monthNames[monthNo] || 'Unknown';
  }


}

export const monthNames: { [key: number]: string } = {
  1: 'April',
  2: 'May',
  3: 'June',
  4: 'July',
  5: 'August',
  6: 'September',
  7: 'October',
  8: 'November',
  9: 'December',
  10: 'January',
  11: 'February',
  12: 'March'
};

function camelCaseToWords(str: string) {
  return str
    .replace(/([A-Z]+)/g, ' $1')
    .replace(/([A-Z][a-z])/g, ' $1')
    .trim();
}

// didDrawCell: function (data: any) {
//   // Check if this is the last row
//   console.log("data", data);
//   if (data.row.index === data.table.body.length - 1) {
//     data.cell.styles.fontStyle = 'bold';
//   }
// },

// getPages() {
//   this.pages = [];
//   for (
//     let i = 1;
//     // i <= this.commonService.gettotalPages();
//     i++
//   ) {
//     this.pages.length < 5 ? this.pages.push(i) : '';
//   }
//   this.pagesObj.complete();
// }

// generateDynamicTable(): string {
//   // Start the table
//   let dynamicTable = `
//     <div class="ng-container" id="reportTable">
//       <div *ngIf="dataSource && dataSource.length > 0" class="text-center">
//         <h1>Company Name: ${this.dataSource[0].companyName}</h1>
//         <p>
//           Address Line 1: ${this.dataSource[0].addressLine1} , Address Line 2:
//           ${this.dataSource[0].addressLine2}, Place: ${this.dataSource[0].place}
//         </p>
//         <p><strong>${this.reportType || "Report"} From ${this.startDate} TO ${this.endDate}</strong></p>
//       </div>
//       <div class="table-responsive table-bordered" style="padding: 5%; padding-bottom: 50px;">
//         <table class="table table-responsive table-bordered">
//           <thead>
//             <tr>
//               <th>Date</th>
//               <th>Party Name</th>
//               <th>GSTIN</th>
//               <th>Bill Number</th>
//               <th>No Of Bags</th>
//               <th>Weight</th>
//               <th>Taxable Value</th>
//               <th>SGST</th>
//               <th>IGST</th>
//               <th>CGST</th>
//               <th>VochType</th>
//               <th>VochNo</th>
//             </tr>
//           </thead>
//           <tbody>
//   `;

//   // Add each row of data
//   for (let row of this.dataSource) { // replace this with your table's data
//     dynamicTable += `
//                 <tr>
//                 <td>${this.formatDate(row.TranctDate) || ' - '}</td>
//                 <td>${row.LedgerName || ' - '}</td>
//                 <td>${row.Gstin || ' - '}</td>
//                 <td>${row.PartyInvoiceNumber || ' - '}</td>
//                 <td>${row.NoOfBags || ' - '}</td>
//                 <td>${row.TotalWeight || ' - '}</td>
//                 <td>${row.Amount || ' - '}</td>
//                 <td>${row.SGST || ' - '}</td>
//                 <td>${row.IGST || ' - '}</td>
//                 <td>${row.VoucherName || ' - '}</td>
//                 <td>${row.VochType || ' - '}</td>
//                 <td>${row.VochNo || ' - '}</td>
//               </tr>
//     `;
//   }

//   // End the table
//   dynamicTable += `
//           </tbody>
//         </table>
//       </div>
//     </div>
//   `;

//   return dynamicTable;
// }

// exportToPDF() {
//   console.log("export pdf");

//   // Generate the dynamic table
//   const dynamicTableContent = this.generateDynamicTable();

//   // Update the content and type in the exportConfig
//   this.exportConfig.type = 'pdf';
//   this.exportConfig.elementIdOrContent = dynamicTableContent;

//   this.exportAsService
//     .save(this.exportConfig, 'exported-data')
//     .subscribe(() => {
//       // Handle success or do something else
//     });
// }
