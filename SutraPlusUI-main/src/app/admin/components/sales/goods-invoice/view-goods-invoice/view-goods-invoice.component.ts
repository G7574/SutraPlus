import { Component, Input, OnChanges, OnInit, SimpleChange, SimpleChanges } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { AdminServicesService } from 'src/app/admin/services/admin-services.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { isEmpty } from 'rxjs';
import { isNil, sum } from 'lodash-es';

@Component({
  selector: 'app-view-goods-invoice',
  templateUrl: './view-goods-invoice.component.html',
  styleUrls: ['./view-goods-invoice.component.scss']
})
export class ViewGoodsInvoiceComponent implements OnInit, OnChanges {
  @Input() invoiceType: string | null = null;

  invoiceData: any;

  globalCompanyId!: number;
  itemList: any[] = [];
  dummyData: any = [{
    CommodityName: "CommodityName", WeightPerBag: "WeightPerBag", NoOfBags: "NoOfBags", TotalWeight: "TotalWeight", Rate: 2.2, Amount: 25
  }]
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
    console.log(this.invoiceData)
    this.globalCompanyId = Number(sessionStorage.getItem('companyID'));
    this.getItemDetails();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['invoiceType']) {
      this.invoiceType = changes['invoiceType'].currentValue;
    }
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
        "DisplayInvNo": this.invoiceData.displayinvNo,
        "InvoiceType": "SalesGood"
      }
    }

    this.spinner.show()
    this.adminService.getItemList(partyDetails).subscribe({
      next: (res: any) => {

        if (!res.HasErrors && res?.Data !== null) {
          this.itemList = res.ItemData;
          this.invoiceData = { ... this.invoiceData, ...res.InvoiceData[0] };

          console.log(this.invoiceData)
          this.calculateTotalAmount(res.ItemData);
          this.calculateTotalBags(res.ItemData);
          this.calculateTotalQuantity(res.ItemData)
          this.GSTRate = this.itemList[0]?.IGST ?? 0;
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

  roundToTwoDigit(value: any) {
    if (isNil(value)) {
      return '-';
    }
    return parseFloat(value).toFixed(2);
  }

  totalGst(a: any, b: any) {
    return this.roundToTwoDigit((sum([a, b])));
  }

}
