import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { AdminServicesService } from 'src/app/admin/services/admin-services.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-view-export-invoice',
  templateUrl: './view-export-invoice.component.html',
  styleUrls: ['./view-export-invoice.component.scss']
})
export class ViewExportInvoiceComponent implements OnInit {
  invoiceData: any;
  globalCompanyId!: number;
  itemList: any[]=[];
  // itemList: any = {};
  totalAmount!: number;
  totalBags!: number;
  totalQuant!: number;
  GSTRate!: number;

  constructor(
    private location: Location,
    private adminService: AdminServicesService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.invoiceData = sessionStorage.getItem('invoiceDataAll');
    this.invoiceData = JSON.parse(this.invoiceData)
    this.globalCompanyId = Number(sessionStorage.getItem('companyID'));
    this.getItemDetails();
  }

  back(): void {
    this.location.back();
  }

  getItemDetails() {
    let partyDetails = {
      "SalesInvoice": {
        "CompanyId": this.globalCompanyId,
        "LedgerId": this.invoiceData.ledgerId,
        "InvoiceNO": this.invoiceData.vochNo,
        "VouchType": this.invoiceData.vochType,
        "InvoiceType": "SalesGood"
      }
    }
    this.spinner.show()
    this.adminService.getItemList(partyDetails).subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          this.itemList = res.ItemData;
          this.calculateTotalAmount(res.ItemData);
          this.calculateTotalBags(res.ItemData);
          this.calculateTotalQuantity(res.ItemData)
          this.GSTRate = this.itemList[0].IGST;
        } else {
          this.toastr.error(res.Errors[0].Message);
        }
        this.spinner.hide();
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong');
      },
    });
  }

  calculateTotalBags(data: any): void {
    let sumVal: number = 0;
    for (let i = 0; i < data.length; i++) {
      sumVal = Number((sumVal + data[i].NoOfBags).toFixed(2));
    }
    this.totalBags = Number(sumVal)
  }

  calculateTotalQuantity(data: any): void {
    let sumVal: number = 0;
    for (let i = 0; i < data.length; i++) {
      sumVal = Number((sumVal + data[i].TotalWeight).toFixed(2));
    }
    this.totalQuant = Number(sumVal)
  }

  calculateTotalAmount(data: any): void {
    let sumVal: number = 0;
    for (let i = 0; i < data.length; i++) {
      sumVal = Number((sumVal + data[i].Amount));
    }
    this.totalAmount = Number(sumVal)
  }

}
