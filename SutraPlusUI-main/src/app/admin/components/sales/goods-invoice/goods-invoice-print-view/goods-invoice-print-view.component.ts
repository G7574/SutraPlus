import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-goods-invoice-print-view',
  templateUrl: './goods-invoice-print-view.component.html',
  styleUrls: ['./goods-invoice-print-view.component.scss']
})
export class GoodsInvoicePrintViewComponent implements OnInit{

  ngOnInit(): void {
    (<HTMLInputElement>document.getElementById("InvoiceHTML")).innerHTML = sessionStorage.getItem("HTMLContent");
  }

  onPrint() {
    window.print();
  }

}
