import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2';
import { AdminServicesService } from '../../../../services/admin-services.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject } from 'rxjs';
import { CommonService } from 'src/app/share/services/common.service';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-goods-invoice-dashboard',
  templateUrl: './goods-invoice-dashboard.component.html',
  styleUrls: ['./goods-invoice-dashboard.component.scss'],
})
export class GoodsInvoiceDashboardComponent implements OnInit {
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
  invlst: any;

  @ViewChild('invoiceDialog') invoiceDialog!: TemplateRef<any>;

  constructor(
    private router: Router,
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    public commonService: CommonService,
    private activatedRoute: ActivatedRoute,
    private route: ActivatedRoute,
    private dialog: MatDialog,
  ) { }

  ngOnInit(): void {
    this.financialYear = sessionStorage.getItem('financialYear');
    this.customerCode = sessionStorage.getItem('globalCustomerCode');
    this.userDetails = sessionStorage.getItem('userDetails');
    this.userDetails = JSON.parse(this.userDetails);
    this.userEmail = this.userDetails?.result?.UserEmailId;
    this.globalCompanyId = sessionStorage.getItem('companyID');
    this.activatedRoute.queryParams.subscribe((params) => {
      this.invlst = params['InvoiceName'];
      this.getInvoiceList();
    });
    this.pagesObj.subscribe((res) => {
      res ? this.getPages() : '';
    });
  }

  invType: any;
  InvoiceData:any;
  vochType:any;
  ClientCode: any;
  CompleteInvoice = '';
  TransporterCopy = '';
  CustomerCopy = '';
  Type : any = 3;
  OfficeCopy: any;
  isDebitNote: boolean = true;

  convertWeightToCustomFormat(weight: number): string {
    let Qntl: number = 0;

    while (weight >= 100) {
        Qntl++;
        weight -= 100;
    }

    let WeightinString: string = "";
    if (weight < 10) {
        WeightinString = `${Qntl}-0${weight.toFixed(2)}`;
    } else {
        WeightinString = `${Qntl}-${weight.toFixed(2)}`;
    }

    return WeightinString;
  }

  invoiceNo!: number;

  onPrint(item:any) {

    this.ClientCode = sessionStorage.getItem("globalCustomerCode");

    let payload = {
      SalesDetails: {
        CompanyId: this.globalCompanyId,
        LedgerId: item.ledgerId,
      },
    };

    this.adminService.GetInvtypeForCalculation(payload).subscribe({
      next: (res: any) => {

        if(this.ClientCode == "UNNATI") {

          this.Type = 6;
        } else {
            if(this.route.snapshot.queryParamMap.get('InvoiceName')?.toString() == 'GoodsInvoice' ||
                  this.route.snapshot.queryParamMap.get('InvoiceName')?.toString() == 'ProfarmaInvoice' ||
                  this.route.snapshot.queryParamMap.get('InvoiceName')?.toString() == 'SalesReturn') {
              if(res.einvreq == 1 && res.frieghtPlus == 1) {
                this.invType = 3;
              } else if(res.einvreq == 1 && res.frieghtPlus == 0) {
                this.invType = 4;
              } else if(res.einvreq == 0 && res.frieghtPlus == 1) {
                this.invType = 1;
              } else if(res.einvreq == 0 && res.frieghtPlus == 0) {
                this.invType = 2;
              }
          } else if(this.route.snapshot.queryParamMap.get('InvoiceName')?.toString() == 'GinningInvoice') {
            this.invType = 7;
          } else if(this.route.snapshot.queryParamMap.get('InvoiceName')?.toString() == 'DebitNote' ||
          this.route.snapshot.queryParamMap.get('InvoiceName')?.toString() == 'CreditNote') {
            this.invType = 5;
          }
            this.Type = this.invType;

          }

      },
      error: (error: any) => {
        this.toastr.error('Something went wrong');
        console.log("error 1 -> " + error);
      },
    });

    if(this.ClientCode == "UNNATI") {

      this.Type = 6;
    }

    console.log("checking" + this.Type)

    //this.ClientCode = "RGP";
    //this.vochType= "9";

    this.vochType =  String(item['vochType']);

    //this.invType = "3";

    let partyDetails = {
      "SalesInvoice": {
        "CompanyId": this.globalCompanyId,
        "LedgerId": item.ledgerId,
        "InvoiceNO": item.vochNo,
        "VouchType": this.vochType,
        "InvoiceType": this.invType
      }
    }

    this.adminService.getItemList(partyDetails).subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          // this.InvoiceData = res.ItemData;
          this.InvoiceData = JSON.parse(res.combinedDataaa);
          // var data = JSON.parse(res.InvoiceData);
          // this.InvoiceData[0].push(data);

          this.CompleteInvoice = "";
          this.TransporterCopy = '';
          this.CustomerCopy = '';

          this.CreateInvoice('Office Copy');
          this.CreateInvoice('Customer Copy');
          this.CreateInvoice('Transporter Copy');

          this.openNewTab('<html><head> <style>td, th {        border: 1px solid black;    text-align: left;font-size: 12px;    }table {    border-collapse: collapse;    width: 100%;}th, td {    padding: 7px;}th {-webkit-print-color-adjust: exact;background-color: gainsboro;} </style></head><body style="font-size: 12px;">' +this.OfficeCopy + '</body></html>');
        } else {
          this.toastr.error(res.Errors[0].Message);
        }
        this.spinner.hide();
      },
      error: (error: any) => {
        console.log(error);
        this.spinner.hide();
        this.toastr.error('Something went wrong');
        console.log("error 2 -> " + error);
      },
    });

  }

  openNewTab(data:any) {

    sessionStorage.setItem('HTMLContent', data);

    const url = this.router.createUrlTree(['/'], { fragment: 'GoodsInvoicePrintView' }).toString();
    window.open(url, '_blank');
  }

  CreateInvoice(InvoiceType:any)
  {
    debugger;
    var HtmlBody = '';

HtmlBody +=' ';

     debugger;
HtmlBody +='        <table style="width:100%;">';
HtmlBody +='            <tbody> <tr>';


//base64logo
if(this.InvoiceData[0]['base64logo'] != '' && this.InvoiceData[0]['base64logo'] != undefined)
{
  HtmlBody +='<td style="width:15%;border: 0px solid white;text-align: center;">';
  //HtmlBody +='<img style="width:125px;" src='+ this.EnrollmentPath + this.InvoiceData[0]['logoPath'] +'>';
  HtmlBody +='<img style="width:125px;" src=' + 'data:image/jpeg;base64,' + this.InvoiceData[0]['base64logo'] +'>';
  HtmlBody +='</td>';
  HtmlBody +='                    <td style="width:85%;    border: 0px solid white;text-align:center;padding-top: 0%;"> ';
}
else
{
  HtmlBody +='                    <td style="width:100%;border: 1px solid white;text-align:center;padding-top: 0%;"> ';
}


HtmlBody +='                        <div style="text-align:center; font-size: 28px;">';
HtmlBody +='                            <span id="FirmName" style="color:black;">'+ this.InvoiceData[0]['companyName'] +'</span>';
HtmlBody +='                        </div>';
HtmlBody +='                       <div style="font-weight: bold;font-size:15px;color:black;" >'+ this.InvoiceData[0]['Address1'] +'</div>';

HtmlBody +='                       <div style="font-weight: bold;font-size:15px;" >District : '+ this.InvoiceData[0]['companyDistrict']  + ', &nbsp; ' + this.InvoiceData[0]['companyPlace'] + ', &nbsp; State : ' + this.InvoiceData[0]['companyState'] +'</div>';
HtmlBody +='                       <div style="font-weight: bold;font-size:15px;color:black;" >GSTIN : '+ this.InvoiceData[0]['companyGSTIN'] +'</div>';
HtmlBody +='                       <div style="font-weight: bold;font-size:15px;color:black;" >'+ this.InvoiceData[0]['secondLineForReport'] +'</div>';
HtmlBody +='                       <div style="font-weight: bold;font-size:15px;color:black;" >'+ this.InvoiceData[0]['thirdLineForReport'] +'</div>';

HtmlBody +='               </td>                  ';


//SignQRCODE
debugger;
if(this.InvoiceData[0]['signQRCODE'] != '' && this.InvoiceData[0]['signQRCODE']  != undefined)
{
  HtmlBody +='<td style="width:15%;border: 0px solid white;text-align: center;">';
  HtmlBody +='<img style="width:125px;" src="'+ this.InvoiceData[0]['signQRCODE'] +'">';
  HtmlBody +='</td>';
  HtmlBody +='                    <td style="width:85%;    border: 0px solid white;text-align:center;padding-top: 0%;"> ';
}
else
{
  HtmlBody +='                    <td style="width:100%;border: 1px solid white;text-align:center;padding-top: 0%;"> ';
}
HtmlBody +='                </tr>';

HtmlBody +='            </tbody>';
HtmlBody +='        </table>';


if(this.InvoiceData[0]['irnNo'] != '')
{
  HtmlBody +='        <table style="width:100%;"><tbody><tr><td style="text-align:center;width:66.7%;border-left: 0px solid black;"><h6 style="margin: 0%;">IRNO : ' + this.InvoiceData[0]['irnNo'] +'</h6>';
  HtmlBody +='                    </td>';
  HtmlBody +='                    <td style="    border-right: 0px solid black;text-align:center;">   ';
  HtmlBody +='				<h6 style="margin: 0%;">ACK No : ' + this.InvoiceData[0]['ackno'] +'</h6>';
  HtmlBody +='                    </td>';
  HtmlBody +='                </tr>';
  HtmlBody +='            </tbody>';
  HtmlBody +='        </table>';
}

debugger;
var BillTitle = '';
if(parseFloat(this.InvoiceData[0]['CSGSTValue']) > 0 || parseFloat(this.InvoiceData[0]['IGSTValue']) > 0)
{
if(this.vochType == 9 || this.vochType == 10 || this.vochType == 1)
{
  BillTitle = 'Tax Invoice';
}
else if(this.vochType == 9 || this.vochType == 10 || this.vochType == 11 && (parseFloat(this.InvoiceData[0]['CSGSTValue']) == 0 && parseFloat(this.InvoiceData[0]['IGSTValue']) == 0) )
{
  BillTitle = 'Bill of Supply';
}
else if(this.vochType == 12)
{
  BillTitle = 'Export Invoice';
}
else if(this.vochType == 6 || this.vochType == 8)
{
  BillTitle = 'Credit Note';
}
else if(this.vochType == 14 ||this.vochType == 15)
{
  BillTitle = 'Debit Note';
}
else if(this.vochType == 16)
{
  BillTitle = 'Performa Invoice';
}
}
else if(this.vochType == 9 || this.vochType == 10 || this.vochType == 11 && (parseFloat(this.InvoiceData[0]['CSGSTValue']) == 0 && parseFloat(this.InvoiceData[0]['IGSTValue']) == 0) )
{
  BillTitle = 'Bill of Supply';
}
else if(this.vochType == 12)
{
  BillTitle = 'Export Invoice';
}
else if(this.vochType == 6 || this.vochType == 8)
{
  BillTitle = 'Credit Note';
}
else if(this.vochType == 14 ||this.vochType == 15)
{
  BillTitle = 'Debit Note';
}
else if(this.vochType == 16)
{
  BillTitle = 'Performa Invoice';
}







if(this.Type == '5')
{
  HtmlBody +='        <table style="width:100%;"><tbody><tr><td style="text-align:center;width:33.33%;border-left: 0px solid black;"><h5 style="margin: 0%;">'+ BillTitle +'</h5>';
  HtmlBody +='                    </td>';
    HtmlBody +='                    <td style="    border-right: 0px solid black;text-align:center;width:33.33%;">   ';
  HtmlBody +='				<h5 style="margin: 0%;">'+ InvoiceType +'</h5>';
  HtmlBody +='                    </td>';
  HtmlBody +='                </tr>';
  HtmlBody +='            </tbody>';
  HtmlBody +='        </table>';
}
else
{
  HtmlBody +='        <table style="width:100%;"><tbody><tr><td style="text-align:center;width:33.33%;border-left: 0px solid black;"><h5 style="margin: 0%;">'+ BillTitle +'</h5>';
  HtmlBody +='                    </td>';
  HtmlBody +='		    <td style="text-align:center;width:33.33%;">';
  HtmlBody +='                         <h5 style="margin: 0%;">PO Number:'+ this.InvoiceData[0]['Ponumber'] +'</h5>';
  HtmlBody +='                    </td>';
  HtmlBody +='                    <td style="    border-right: 0px solid black;text-align:center;width:33.33%;">   ';
  HtmlBody +='				<h5 style="margin: 0%;">'+ InvoiceType +'</h5>';
  HtmlBody +='                    </td>';
  HtmlBody +='                </tr>';
  HtmlBody +='            </tbody>';
  HtmlBody +='        </table>';
}




HtmlBody +=' <table style="width:100%;">';
HtmlBody +='            <tbody>';
HtmlBody +='                <tr>';
HtmlBody +='                    <td style="width:50%;border-left: 0px solid black;border-top: 0px solid black;">';
HtmlBody +='                         <span id="txtCustName" style="">Reverse Charge : </span><span style="font-weight:bold;">  NO</span>';

if(this.Type != '5')
{
HtmlBody +='                         <span id="txtCustAddress" style="font-weight: bold; float:right;">E-WAY NO : <br /> '+ this.InvoiceData[0]['EwayBillNo'] +'</span>';
}

HtmlBody +='                         <br /><span id="txtArea" >Invoice No :</span><span style="font-weight: bold;"> '+ this.InvoiceData[0]['displayinvNo'] +'</span><br />';
HtmlBody +='                         <span id="txtDistrict" > Invoice Date :</span><span style="font-weight: bold;"> '+ this.InvoiceData[0]['TranctDate'] +'</span><br />';
HtmlBody +='                         <span id="txtState" >State</span> : <span style="font-weight: bold;"> Karnataka</span>';
HtmlBody +='                         <span style="float:right;"> <span id="PCustomerGSTNo" >State Code : </span><span style="font-weight:bold;">29</span></span>';
HtmlBody +='                    </td>';

HtmlBody +='                    <td   style="border-left: 0px solid black;border-top: 0px solid black;border-right: 0px solid black;">   ';

if(this.Type == '5' || this.Type == '7' )
{
  HtmlBody +='';
}
else
{
HtmlBody +='                         <span id="Span1" >Transport Mode :</span><span style="font-weight: bold;"> By Road </span> ' + (this.InvoiceData[0]['paymentterms'] != '' ? ' <span> Payment terms </span> <span style="font-weight:bold;"> '+ (this.InvoiceData[0]['paymentterms'] )  +'</span> ' : '' ) + ' <br />';
HtmlBody +='                         <span id="Span2" >Vehicle NO :</span><span style="font-weight: bold;">  '+ this.InvoiceData[0]['LorryNo'] +'</span>' + (this.InvoiceData[0]['deliveryterms'] != '' ? ' <span> Delivery terms </span> <span style="font-weight:bold;"> '+ (this.InvoiceData[0]['deliveryterms'] )  +'</span> ' : '' ) + ' <br />';
HtmlBody +='                         <span id="Span3" >Date of Supply :</span><span style="font-weight: bold;"> '+ this.InvoiceData[0]['TranctDate'] +'</span><br />';
HtmlBody +='                         <span id="Span4" ">Place of Supply :</span><span style="font-weight: bold;"> '+ this.InvoiceData[0]['DeliveryPlace'] +'</span><br />';
}


HtmlBody +='                    </td>';
HtmlBody +='                </tr>';
HtmlBody +='            </tbody>';
HtmlBody +='        </table>';

HtmlBody +='<table style="width:100%;">';
HtmlBody +='            <tbody>';
HtmlBody +='                <tr>';
HtmlBody +='                    <td style="width:50%;    vertical-align: initial;   border-bottom: 0px solid black; border-left: 0px solid black;border-top: 0px solid black;border-right: 0px solid black;">';
HtmlBody +='                         <span id="Span5" ><span style="font-weight:bold; font-size: 12px;">Name and Address of Receiver / Billed To : </span> <br /><span style="font-size:14px;font-weight:bold; color:black;">'+ this.InvoiceData[0]['LedgerName'] +'</span> <br/>'+ this.InvoiceData[0]['Address1'] +'<br/> '+ this.InvoiceData[0]['Address2']+'<br/> '+ this.InvoiceData[0]['place'] +'</span><br />';
HtmlBody +='                         <span id="Span6" >GSTIN :</span> <span style="font-weight:bold; color:black;">'+ this.InvoiceData[0]['GST'] +'</span>';
if(this.InvoiceData[0]['ledgerfssai'] != '')
{
HtmlBody +='                         <span style="float:right;"><span id="Span8" >FSSAI : </span><span style="font-weight:bold;"> '+ this.InvoiceData[0]['ledgerfssai'] +'</span></span>';
}
HtmlBody +='                       <br> <span id="Span7" >State : </span><span style="font-weight:bold;"> '+ this.InvoiceData[0]['State']+'-'+this.InvoiceData[0]['stateCode2'] +'</span>';
HtmlBody +='                         <span style="float:right;"><span id="Span8" >Contact No : </span><span style="font-weight:bold;"> '+ this.InvoiceData[0]['ledgerCELLNO'] +'</span></span>';
HtmlBody +='                    </td>';

HtmlBody +='                    <td  style="  border-bottom: 0px solid black;  border-top: 0px solid black;border-right: 0px solid black;    vertical-align: initial;">   ';

if(this.Type == '5' || this.Type == '7')
{
  HtmlBody +='';

}
else
{
  HtmlBody +='                         <span id="Span9" ><span style="font-size: 12px;font-weight:bold;">Details of Consingee / Shipped To : </span> <br /><span style="font-size: 14px;font-weight:bold;">'+ this.InvoiceData[0]['DeliveryName'] +'</span><br/>'+ this.InvoiceData[0]['DeliveryAddress1']  +'<br/> '+ this.InvoiceData[0]['DeliveryAddress2']+'<br/> '+ this.InvoiceData[0]['DeliveryPlace'] +'</span><br />';
  HtmlBody +='                        <br> <span id="Span10">'+ this.InvoiceData[0]['DeliveryState'] +'</span>';
  HtmlBody +='                         <span style="float:right;"> <span id="Span11" >State Code :</span><span style="font-weight:bold;">  '+ this.InvoiceData[0]['DeliveryStateCode'] +'</span></span>';
}


HtmlBody +='                    </td>';
HtmlBody +='                </tr>';
HtmlBody +='            </tbody>';
HtmlBody +='        </table>';

HtmlBody +='        <table >';
HtmlBody +='            <thead>';
HtmlBody +='              <tr>';
HtmlBody +='                <th style="border-left: 0px solid black;width:5%;font-weight: bold;font-size: 12px;text-align:center;">SL No.</th>';


if(this.Type == '6')
{
HtmlBody +='                <th style="width:35%;font-weight: bold;font-size: 12px;text-align:left;">Item Description</th>';
}
else
{
HtmlBody +='                <th style="width:25%;font-weight: bold;font-size: 12px;text-align:left;">Item Description</th>';
}

HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">HSN Code</th>';

if(this.Type == '5')
{
  HtmlBody +='';
}
else if(this.Type == '6' || this.Type == '7')
{
  HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">Qty</th>';
  HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">Rate</th>';
}
else
{
HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">UOM</th>';
HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">No of Bags</th>';
HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">Weight Per Bag</th>';

HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">Weight (Qntl-Kg)</th>';
HtmlBody +='                <th style="width:10%;font-weight: bold;font-size: 12px;text-align:center;">Rate Per Quintal</th>';
}

HtmlBody +='                <th style="border-right: 0px solid black;width:10%;font-weight: bold;font-size: 12px;text-align:center;">Amount</th>';
HtmlBody +='              </tr>';
HtmlBody +='            </thead>';
HtmlBody +='            <tbody id="PrintTBody">';

var p = 0;
let totalNumberOfBags = 0;
let totalWeightQnt = 0;
let totalAmount = 0;
for(p=0;p<this.InvoiceData[0].Itemresult.length; p++)
{


  HtmlBody +='            <tr>';
  HtmlBody +='            <td style="border-left: 0px solid black;">'+ (p + 1) +'</td>';
  HtmlBody +='            <td style="overflow-wrap: anywhere;">'+ this.InvoiceData[0].Itemresult[p]['CommodityName'] + ' ' + this.InvoiceData[0].Itemresult[p]['Mark'] +'</td>';
  HtmlBody +='            <td style="text-align:center;">'+ this.InvoiceData[0].Itemresult[p]['hsn'] +'</td>';

  if(this.Type == '5')
  {
    HtmlBody +='';
  }
  else if(this.Type == '6' || this.Type == '7')
  {
    HtmlBody +='            <td style="text-align:center;">'+ this.InvoiceData[0].Itemresult[p]['lineItemQty'] +'</td>';
    HtmlBody +='            <td style="text-align:right;">'+ this.SeperateComma(parseFloat(this.InvoiceData[0].Itemresult[p]['Rate']).toFixed(2)) +'</td>';
  }
  else
  {
  HtmlBody +='            <td style="text-align:center;">'+ this.InvoiceData[0].Itemresult[p]['mou'] +'</td>';
  HtmlBody +='            <td style="text-align:right;">'+ this.InvoiceData[0].Itemresult[p]['NoOfBags'] +'</td>';

  totalNumberOfBags += this.InvoiceData[0].Itemresult[p]['NoOfBags'];

  if(parseFloat(this.InvoiceData[0].Itemresult[p]['WeightPerBag']) > 0)
  {
  HtmlBody +='            <td style="text-align:right;">'+ this.InvoiceData[0].Itemresult[p]['WeightPerBag'] +'</td>';
  }
  else
  {
    HtmlBody +='            <td style="text-align:right;"></td>';
  }
  let formattedWgt: string = this.convertWeightToCustomFormat(this.InvoiceData[0].Itemresult[p]['TotalWeight']);
  HtmlBody +='            <td style="text-align:right;">'+ formattedWgt +'</td>';
  totalWeightQnt += Number(this.InvoiceData[0].Itemresult[p]['TotalWeight']);
  HtmlBody +='            <td style="text-align:right;">'+ this.SeperateComma(parseFloat(this.InvoiceData[0].Itemresult[p]['Rate']).toFixed(2)) +'</td>';
}

  HtmlBody +='            <td style="border-right: 0px solid black;text-align:right;">'+ this.SeperateComma(parseFloat(this.InvoiceData[0].Itemresult[p]['Amount']).toFixed(2)) +'</td>';
  totalAmount += Number(this.InvoiceData[0].Itemresult[p]['Amount']);
  HtmlBody +='            </tr>';


  }



//////// Blank Rows Start /////////


// var BlankRowsCount = 10 - this.InvoiceData.length;
var BlankRowsCount = 10 - this.InvoiceData[0].Itemresult.length;

var u = 0;
for(u=0;u<BlankRowsCount; u++)
{
  HtmlBody +='            <tr>';
  HtmlBody +='            <td style="border-left: 0px solid black;padding:1.06%;border: 0px;"></td>';
  HtmlBody +='            <td style="overflow-wrap: anywhere;border: 0px;"></td>';
  HtmlBody +='            <td style="text-align:center;border: 0px;"></td>';

  if(this.Type == '5')
  {
    HtmlBody +='';
  }
  else
  {

    if(this.Type == '6' || this.Type == '7')
    {
  HtmlBody +='            <td style="text-align:center;border: 0px;"></td>';
  HtmlBody +='            <td style="text-align:right;border: 0px;"></td>';

    }
else
{
  HtmlBody +='            <td style="text-align:center;border: 0px;"></td>';
  HtmlBody +='            <td style="text-align:right;border: 0px;"></td>';

  if(parseFloat(this.InvoiceData[0]['WeightPerBag']) > 0)
  {
  HtmlBody +='            <td style="text-align:right;border: 0px;"></td>';
  }
  else
  {
    HtmlBody +='            <td style="text-align:right;border: 0px;"></td>';
  }
  HtmlBody +='            <td style="text-align:center;border: 0px;"></td>';
  HtmlBody +='            <td style="text-align:right;border: 0px;"></td>';
}
  }
  HtmlBody +='            <td style="border-right: 0px solid black;text-align:right;border: 0px;"></td>';
  HtmlBody +='            </tr>';

}


//////// Blank Rows End //////////











  HtmlBody +='            <tr style="background: gainsboro;font-weight: bold;">';
  HtmlBody +='            <td style="border-left: 0px solid black;"></td>';
  HtmlBody +='            <td></td>';
  HtmlBody +='            <td style="text-align:center;"></td>';

  if(this.Type == '5')
{
  HtmlBody +='';
}
else
{

  if(this.Type=='6' || this.Type == '7')
  {
    HtmlBody +='            <td style="text-align:center;">'+ this.InvoiceData[0]['TotalWeight'] +'</td>';
    HtmlBody +='            <td style="text-align:center;"></td>';
  }
  else
  {




  HtmlBody +='            <td style="text-align:center;"></td>';
  // HtmlBody +='            <td style="text-align:right;">'+ this.InvoiceData[0]['totalBags'] +'</td>';
  HtmlBody +='            <td style="text-align:right;">'+ totalNumberOfBags +'</td>';


  //HtmlBody +='            <td style="text-align:right;"></td>';
  HtmlBody +='            <td style="text-align:right;padding:0%;font-size:12px;"></td>';



  if(totalWeightQnt == 0) {
    HtmlBody +='            <td style="text-align:right;padding:0%;font-size:12px;"></td>';
  } else {
    let formattedWeight: string = this.convertWeightToCustomFormat(totalWeightQnt);
    HtmlBody +='            <td style="text-align:right;padding:0%;font-size:12px;">'+ formattedWeight +'</td>';
  }

  HtmlBody +='            <td style="text-align:right;"></td>';
}
}
  HtmlBody +='            <td style="border-right: 0px solid black;text-align:right;">'+ this.SeperateComma(totalAmount.toFixed(2)) +'</td>';
  HtmlBody +='            </tr>';



HtmlBody +='            </tbody>';
HtmlBody +='        </table>';



HtmlBody +='        <table style="border-top: 0px solid white;border-bottom: 0px solid white;">';
HtmlBody +='                    <tr>';
HtmlBody +='                        <td style="border-left: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;width:40%;">';

if(this.Type == '5' || this.Type == '7')
{
  HtmlBody +='';
}
else
{
HtmlBody +='                            <div style="font-weight:bold;">Lorry Details :</div>';
HtmlBody +='                            <div>Lorry No: '+ this.InvoiceData[0]['LorryNo'] +'</div>';
HtmlBody +='                            <div>Lorry Owner : '+ this.InvoiceData[0]['LorryOwnerName'] +'</div>';
HtmlBody +='                            <div>Lorry Driver : '+ this.InvoiceData[0]['DriverName'] +'</div>';
HtmlBody +='                            <div>DL No : '+ this.InvoiceData[0]['Dlno'] +'</div>';
HtmlBody +='                            <div>Transporter : '+ this.InvoiceData[0]['Transporter'] +'</div>';
HtmlBody +='                            <br />';
HtmlBody +='				<div style="font-weight:bold;">Freight Details :</div>';
HtmlBody +='                            <div>Freight/Bag : <span id="PCompanyPanno">'+ this.InvoiceData[0]['FrieghtPerBag'] +'</span></div>';
HtmlBody +='                            <div>Total Freight : <span id="PGSTNO">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['TotalFrieght']).toFixed(2)) +'</span></div>';
HtmlBody +='                            <div>Advance Paid : <span id="Span12">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['Advance']).toFixed(2)) +'</span></div>';
HtmlBody +='                            <div>Freight Payable : <span id="Span13">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['Balance']).toFixed(2)) +'</span></div>';
}
HtmlBody +='                        </td>';





if(this.Type == '6')
{
HtmlBody +='                        <td style="border-left: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;width: 22.5%;">';
}
else
{
HtmlBody +='                        <td style="border-left: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;width:20%;">';
}



HtmlBody +='                            <div>Seller PAN: '+ this.InvoiceData[0]['companyPAN'] +'</div>';
HtmlBody +='                            <div>Buyer PAN : '+ this.InvoiceData[0]['ledgerPAN'] +'</div><br>';

debugger;


if(this.Type == '1')
{
  HtmlBody +='                            <div>Dispatched From : '+ this.InvoiceData[0]['FromPlace'] +'</div>';
  HtmlBody +='                            <div>Dispatched To : '+ this.InvoiceData[0]['ToPlace'] +'</div>';
}

if(this.Type == '2')
{
  HtmlBody +='                            <div>Dispatched From : '+ this.InvoiceData[0]['FromPlace'] +'</div>';
  HtmlBody +='                            <div>Dispatched To : '+ this.InvoiceData[0]['ToPlace'] +'</div>';
}

if(this.Type == '3')
{
  HtmlBody +='                            <div>Dispatched From : '+ this.InvoiceData[0]['FromPlace'] +'</div>';
  HtmlBody +='                            <div>Dispatched To : '+ this.InvoiceData[0]['ToPlace'] +'</div>';
}

if(this.Type == '4')
{
  HtmlBody +='                            <div>Dispatched From : '+ this.InvoiceData[0]['FromPlace'] +'</div>';
  HtmlBody +='                            <div>Dispatched To : '+ this.InvoiceData[0]['ToPlace'] +'</div>';
}

if(this.Type == '6')
{
  HtmlBody +='                            <div>Dispatched From : '+ this.InvoiceData[0]['FromPlace'] +'</div>';
  HtmlBody +='                            <div>Dispatched To : '+ this.InvoiceData[0]['ToPlace'] +'</div>';
}





HtmlBody +='                        </td>';






HtmlBody +='                      <td style="border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;width:20%;">';





debugger;
if(this.Type == '1')
{


  HtmlBody +='        <div>'+ this.InvoiceData[0]['frieghtLabel'] +'</div>';

  if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='        <div>'+ this.InvoiceData[0]['ExpenseName1'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName2'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName3'] +'</div>';
}



HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.InvoiceData[0]['sgstLabel'] +'</div>';
HtmlBody +='				<div>'+ this.InvoiceData[0]['cgstLabel'] +'</div>';
}
else
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['igstLabel'] +'</div>';
}










if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['tcsLabel'] +'</div>';
}

HtmlBody +='				<div>Round Off</div>';






HtmlBody +='                        </td>';
HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;text-align:right">';


if(parseFloat(this.InvoiceData[0]['frieghtinBill']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['frieghtinBill']).toFixed(2)) +'</div>';
}

if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount1']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount2']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount3']).toFixed(2)) +'</div>';
}


HtmlBody +='                           <div style="font-weight:bold;color:black;">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['TaxableValue']).toFixed(2)) +'</div>';


if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['SGSTValue']).toFixed(2)) +'</div>';
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['CSGSTValue']).toFixed(2)) +'</div>';
}
else
{
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['IGSTValue']).toFixed(2)) +'</div>';
}



if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['tcsValue']).toFixed(2)) +'</div>';
}

HtmlBody +='				<div>'+ this.InvoiceData[0]['RoundOff'].toFixed(2) +'</div>';






}


debugger;
if(this.Type == '7')
{


  HtmlBody +='        <div>'+ this.InvoiceData[0]['frieghtLabel'] +'</div>';

  if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='        <div>'+ this.InvoiceData[0]['ExpenseName1'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName2'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName3'] +'</div>';
}



HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.InvoiceData[0]['sgstLabel'] +'</div>';
HtmlBody +='				<div>'+ this.InvoiceData[0]['cgstLabel'] +'</div>';
}
else
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['igstLabel'] +'</div>';
}










if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['tcsLabel'] +'</div>';
}

HtmlBody +='				<div>Round Off</div>';






HtmlBody +='                        </td>';
HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;text-align:right">';


if(parseFloat(this.InvoiceData[0]['frieghtinBill']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['frieghtinBill']).toFixed(2)) +'</div>';
}

if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount1']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount2']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount3']).toFixed(2)) +'</div>';
}


HtmlBody +='                           <div style="font-weight:bold;color:black;">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['TaxableValue']).toFixed(2)) +'</div>';


if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['SGSTValue']).toFixed(2)) +'</div>';
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['CSGSTValue']).toFixed(2)) +'</div>';
}
else
{
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['IGSTValue']).toFixed(2)) +'</div>';
}



if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['tcsValue']).toFixed(2)) +'</div>';
}

HtmlBody +='				<div>'+ this.InvoiceData[0]['RoundOff'].toFixed(2) +'</div>';






}


debugger;
if(this.Type == '6')
{


  HtmlBody +='        <div>'+ this.InvoiceData[0]['frieghtLabel'] +'</div>';

  if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='        <div>'+ this.InvoiceData[0]['ExpenseName1'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName2'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName3'] +'</div>';
}



HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.InvoiceData[0]['sgstLabel'] +'</div>';
HtmlBody +='				<div>'+ this.InvoiceData[0]['cgstLabel'] +'</div>';
}
else
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['igstLabel'] +'</div>';
}










if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['tcsLabel'] +'</div>';
}

HtmlBody +='				<div>Round Off</div>';






HtmlBody +='                        </td>';
HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;text-align:right">';


if(parseFloat(this.InvoiceData[0]['frieghtinBill']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['frieghtinBill']).toFixed(2)) +'</div>';
}

if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount1']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount2']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount3']).toFixed(2)) +'</div>';
}


HtmlBody +='                           <div style="font-weight:bold;color:black;">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['TaxableValue']).toFixed(2)) +'</div>';


if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['SGSTValue']).toFixed(2)) +'</div>';
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['CSGSTValue']).toFixed(2)) +'</div>';
}
else
{
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['IGSTValue']).toFixed(2)) +'</div>';
}



if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['tcsValue']).toFixed(2)) +'</div>';
}

HtmlBody +='				<div>'+ this.InvoiceData[0]['RoundOff'].toFixed(2) +'</div>';






}





debugger;
if(this.Type == '2')
{




  if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='        <div>'+ this.InvoiceData[0]['ExpenseName1'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName2'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName3'] +'</div>';
}



HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.InvoiceData[0]['sgstLabel'] +'</div>';
HtmlBody +='				<div>'+ this.InvoiceData[0]['cgstLabel'] +'</div>';
}
else
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['igstLabel'] +'</div>';
}









if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['tcsLabel'] +'</div>';
}

HtmlBody +='				<div>Round Off</div>';


HtmlBody +='				<div><b>Invoice Amount</b></div>';

HtmlBody +='        <div>'+ this.InvoiceData[0]['frieghtLabel'] +'</div>';




HtmlBody +='                        </td>';
HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;;text-align:right">';



if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount1']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount2']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount3']).toFixed(2)) +'</div>';
}


HtmlBody +='                           <div style="font-weight:bold;color:black">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['TaxableValue']).toFixed(2)) +'</div>';


if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['SGSTValue']).toFixed(2)) +'</div>';
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['CSGSTValue']).toFixed(2)) +'</div>';
}
else
{
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['IGSTValue']).toFixed(2)) +'</div>';
}








if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['tcsValue']).toFixed(2)) +'</div>';
}

HtmlBody +='				<div>'+ this.InvoiceData[0]['RoundOff'].toFixed(2) +'</div>';



debugger;
HtmlBody +='				<div><b>'+ this.SeperateComma((parseFloat(this.InvoiceData[0]['TaxableValue']) + parseFloat(this.InvoiceData[0]['IGSTValue']) + parseFloat(this.InvoiceData[0]['SGSTValue']) + parseFloat(this.InvoiceData[0]['CSGSTValue']) + parseFloat(this.InvoiceData[0]['tcsValue']) + parseFloat(this.InvoiceData[0]['RoundOff'])).toFixed(2)) +'</b></div>';
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['frieghtinBill']).toFixed(2)) +'</div>';

}



debugger;
if(this.Type == '3')
{



HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.InvoiceData[0]['sgstLabel'] +'</div>';
HtmlBody +='				<div>'+ this.InvoiceData[0]['cgstLabel'] +'</div>';
}
else
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['igstLabel'] +'</div>';
}


HtmlBody +='        <div>'+ this.InvoiceData[0]['frieghtLabel'] +'</div>';





if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='        <div>'+ this.InvoiceData[0]['ExpenseName1'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName2'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName3'] +'</div>';
}

if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['tcsLabel'] +'</div>';
}

HtmlBody +='				<div>Round Off</div>';




HtmlBody +='                        </td>';
HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;text-align:right">';

HtmlBody +='                           <div style="font-weight:bold;color:black;">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['TaxableValue']).toFixed(2)) +'</div>';


if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['SGSTValue']).toFixed(2)) +'</div>';
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['CSGSTValue']).toFixed(2)) +'</div>';
}
else
{
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['IGSTValue']).toFixed(2)) +'</div>';
}


if(parseFloat(this.InvoiceData[0]['frieghtinBill'])> 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['frieghtinBill']).toFixed(2)) +'</div>';
}

if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount1']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount2']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount3']).toFixed(2)) +'</div>';
}



if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['tcsValue']).toFixed(2)) +'</div>';
}

HtmlBody +='				<div>'+ this.InvoiceData[0]['RoundOff'].toFixed(2) +'</div>';






}











debugger;
if(this.Type == '4')
{



HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.InvoiceData[0]['sgstLabel'] +'</div>';
HtmlBody +='				<div>'+ this.InvoiceData[0]['cgstLabel'] +'</div>';
}
else
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['igstLabel'] +'</div>';
}







if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='        <div>'+ this.InvoiceData[0]['ExpenseName1'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName2'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName3'] +'</div>';
}

if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['tcsLabel'] +'</div>';
}

HtmlBody +='				<div>Round Off</div>';

HtmlBody +='				<div style="font-weight: bold;">Invoice Amount</div>';


  HtmlBody +='        <div>'+ this.InvoiceData[0]['frieghtLabel'] +'</div>';

HtmlBody +='                        </td>';
HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;width:10%;text-align:right">';

HtmlBody +='                           <div style="font-weight:bold;color:black;">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['TaxableValue']).toFixed(2)) +'</div>';


if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['SGSTValue']).toFixed(2)) +'</div>';
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['CSGSTValue']).toFixed(2)) +'</div>';
}
else
{
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['IGSTValue']).toFixed(2)) +'</div>';
}




if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount1']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount2']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount3']).toFixed(2)) +'</div>';
}



if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['tcsValue']).toFixed(2)) +'</div>';
}

HtmlBody +='				<div>'+ this.InvoiceData[0]['RoundOff'].toFixed(2) +'</div>';


debugger;
var GrandTotal = 0;

GrandTotal = parseFloat(this.InvoiceData[0]['TaxableValue']) + parseFloat(this.InvoiceData[0]['SGSTValue']) + parseFloat(this.InvoiceData[0]['CSGSTValue']) + parseFloat(this.InvoiceData[0]['IGSTValue'])  + parseFloat(this.InvoiceData[0]['ExpenseAmount1']) + parseFloat(this.InvoiceData[0]['ExpenseAmount2']) + parseFloat(this.InvoiceData[0]['ExpenseAmount3']) + parseFloat(this.InvoiceData[0]['tcsValue']);

HtmlBody +='				<div style="font-weight: bold;">₹'+ this.SeperateComma(GrandTotal.toFixed(2)) +'</div>';


HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['frieghtinBill']).toFixed(2)) +'</div>';
}

debugger;
if(this.Type == '5')
{



HtmlBody +='    				<div style="font-weight:bold;">Taxable Value  </div>';

if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.InvoiceData[0]['sgstLabel'] +'</div>';
HtmlBody +='				<div>'+ this.InvoiceData[0]['cgstLabel'] +'</div>';
}
else
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['igstLabel'] +'</div>';
}







if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='        <div>'+ this.InvoiceData[0]['ExpenseName1'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName2'] +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='				<div>'+ this.InvoiceData[0]['ExpenseName3'] +'</div>';
}

if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.InvoiceData[0]['tcsLabel'] +'</div>';
}

HtmlBody +='				<div>Round Off</div>';






HtmlBody +='                        </td>';
HtmlBody +='<td style="    border-right: 0px solid black;border-top: 0px solid white;border-bottom: 0px solid white;text-align:right">';

HtmlBody +='                           <div style="font-weight:bold;color:black;">'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['TaxableValue']).toFixed(2)) +'</div>';


if(this.InvoiceData[0]['stateCode2'] == '29')
{
HtmlBody +='                            <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['SGSTValue']).toFixed(2)) +'</div>';
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['CSGSTValue']).toFixed(2)) +'</div>';
}
else
{
HtmlBody +='			    <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['IGSTValue']).toFixed(2)) +'</div>';
}




if(parseFloat(this.InvoiceData[0]['ExpenseAmount1']) > 0)
{
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount1']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount2']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount2']).toFixed(2)) +'</div>';
}
if(parseFloat(this.InvoiceData[0]['ExpenseAmount3']) > 0){
HtmlBody +='                           <div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['ExpenseAmount3']).toFixed(2)) +'</div>';
}


debugger;
if(parseFloat(this.InvoiceData[0]['tcsValue']) > 0)
{
HtmlBody +='				<div>'+ this.SeperateComma(parseFloat(this.InvoiceData[0]['tcsValue']).toFixed(2)) +'</div>';
}

HtmlBody +='				<div>'+ this.InvoiceData[0]['RoundOff'].toFixed(2) +'</div>';



var GrandTotal = 0;

GrandTotal = parseFloat(this.InvoiceData[0]['TaxableValue']) + parseFloat(this.InvoiceData[0]['SGSTValue']) + parseFloat(this.InvoiceData[0]['CSGSTValue']) + parseFloat(this.InvoiceData[0]['IGSTValue'])  + parseFloat(this.InvoiceData[0]['ExpenseAmount1']) + parseFloat(this.InvoiceData[0]['ExpenseAmount2']) + parseFloat(this.InvoiceData[0]['ExpenseAmount3']) + parseFloat(this.InvoiceData[0]['tcsValue']);






}


HtmlBody +='                        </td>';



HtmlBody +='            </tr>';



HtmlBody +='<tr>';
HtmlBody +='<td style="font-weight:bold;border-left: 0px solid black;" colspan="2">Note : '+ this.InvoiceData[0]['note1'] +'</td>';

HtmlBody +='<td style="font-weight:bold;">Grand Total</td>';
HtmlBody +='<td style="border-right: 0px solid black;font-weight:bold;text-align:right;color:black;">₹ '+ this.SeperateComma(parseFloat(this.InvoiceData[0]['billAmount']).toFixed(2)) +'</td>';
HtmlBody +='</tr>';
HtmlBody +='        </table>';
HtmlBody +='  <table style="border-top: 0px solid white;">';
HtmlBody +='            <tr>';
HtmlBody +='                <td style="border-top: 0px solid white;    border-left: 0px solid white;border-right: 0px solid white;">';
HtmlBody +='                    <span style="font-weight:bold;color:black;">'+ this.InvoiceData[0]['inWords'] +'</span>';
HtmlBody +='                    <span style="font-weight:bold;" id="txtAmount"></span>';

HtmlBody +='                </td>';
HtmlBody +='            </tr>';
HtmlBody +='        </table>';
HtmlBody +='  <table style="border-top: 0px solid white;">';
HtmlBody +='<tr>';
HtmlBody +='<th style="  border-left: 0px solid black;  font-weight: bold;font-size: 12px;text-align:center;">Bank Name</th>';
HtmlBody +=' <th style="   font-weight: bold;font-size: 12px;text-align:center;">IFSC Code</th>';
HtmlBody +='<th style=" border-right: 0px solid black;  font-weight: bold;font-size: 12px;text-align:center;">Account No</th>';

HtmlBody +='</tr>';
HtmlBody +='           <tr>';
HtmlBody +='                <td style="border-left: 0px solid black;border-top: 0px solid white;text-align:center; ">'+ this.InvoiceData[0]['bank1'] +'';

HtmlBody +='                </td>';
HtmlBody +=' <td style="border-top: 0px solid white;text-align:center;">'+ this.InvoiceData[0]['ifsC1'] +'';

HtmlBody +='                </td>';
HtmlBody +=' <td style="text-align:center;border-right: 0px solid black;border-top: 0px solid white;">'+ this.InvoiceData[0]['accountNo1'] +'';

HtmlBody +='                </td>';
HtmlBody +='            </tr>';

HtmlBody +='</tr>';
//if(this.InvoiceData[0]['bank2'] != '')
//{
HtmlBody +='           <tr>';
HtmlBody +='                <td style="border-left: 0px solid black;border-top: 0px solid white;text-align:center;">'+ (this.InvoiceData[0]['bank2'] != '' ? this.InvoiceData[0]['bank2'] : '-') +'';

HtmlBody +='                </td>';
HtmlBody +=' <td style="border-top: 0px solid white;text-align:center;">'+ this.InvoiceData[0]['ifsC2'] +'';

HtmlBody +='                </td>';
HtmlBody +=' <td style="border-right: 0px solid black;border-top: 0px solid white;text-align:center;">'+ this.InvoiceData[0]['accountNo2'] +'';

HtmlBody +='                </td>';
HtmlBody +='            </tr>';
//}
HtmlBody +='</tr>';

//if(this.InvoiceData[0]['bank3'] != '')
//{
HtmlBody +='           <tr>';
HtmlBody +='                <td style="border-left: 0px solid black;border-top: 0px solid white;text-align:center;">'+ (this.InvoiceData[0]['bank3'] != '' ? this.InvoiceData[0]['bank3'] : '-') +'';

HtmlBody +='                </td>';
HtmlBody +=' <td style="border-top: 0px solid white;text-align:center;">'+ this.InvoiceData[0]['ifsC3'] +'';

HtmlBody +='                </td>';
HtmlBody +=' <td style="border-right: 0px solid black;border-top: 0px solid white;text-align:center;">'+ this.InvoiceData[0]['account3'] +'';

HtmlBody +='                </td>';
HtmlBody +='            </tr>';
//}

HtmlBody +='        </table>';
HtmlBody +=' <table style="border-top: 0px solid white;">';
HtmlBody +='            <tr>';
HtmlBody +='                <td style="    border-left: 0px solid black;border-right: 0px solid black;border-top: 0px solid white;">';
HtmlBody +='                    <span style="">Terms of Business :</span>';
HtmlBody +='                    <span style="font-weight:bold;">1) Interest from date of purchase will be charged @ 2% per month.</span> <br />';
HtmlBody +='                    <span style="font-weight:bold;margin-left:116px;">'+ this.InvoiceData[0]['jurisLine'] +'</span><br />';

if(this.ClientCode != 'AVS')
{
  if(this.Type != '7')
  {
HtmlBody +='                    <span style="font-weight:bold;margin-left:116px;">3) We are not reponsible for any loss in weight or damage during transit.</span>';
  }
}
HtmlBody +='                </td>';
HtmlBody +='            </tr>';
HtmlBody +='        </table>';

HtmlBody +=' <table style="border-top: 0px solid white;width:100%;">';
HtmlBody +='            <tr>';
HtmlBody +='<td style=" width:20%;   border-left: 0px solid black;border-top: 0px solid white;border-right: 0px solid white;text-align:left;">';

if(this.Type != '7')
{
HtmlBody +='<span> Driver/Transporters</span><br><br><br>';
HtmlBody +='<span style="">Authorised Signature</span>';
}

HtmlBody +='</td>';
HtmlBody +='<td style="width:20%; border-top: 0px solid white;border-left: 0px solid white;border-right: 0px solid white;text-align:right;">';

if(this.Type != '7')
{
HtmlBody +='<span></span><br><br><br>';
HtmlBody +='<span>Common Seal</span>';
}
HtmlBody +='</td>';
HtmlBody +='<td style=" width:60%;   border-right: 0px solid black;border-top: 0px solid white;border-left: 0px solid white;">';
HtmlBody +='<h6 style="text-align:right;">Certified that the particulars given above are true and correct</h6>';
HtmlBody +='<span></span><br>';
HtmlBody +='<span style="margin-left:55%;">Authorised Signature</span>';
HtmlBody +='</td>';
HtmlBody +='            </tr>';
HtmlBody +='        </table>';



this.CompleteInvoice += ' <div   style="border: 1px solid black;border-bottom: 0px solid white;page-break-after: always;">' +HtmlBody + '</div>';

if(InvoiceType == 'Customer Copy')
{
    this.CustomerCopy = '<html><head> <style> body {font-family: Tahoma, Verdana, Segoe, sans-serif;}  td, th {        border: 1px solid black;    text-align: left;font-size: 13px;    }table {    border-collapse: collapse;    width: 100%;}th, td {    padding: 2px;}th {-webkit-print-color-adjust: exact;background-color: gainsboro;} </style></head><body style="font-size: 12px;"> <div   style="border: 1px solid black;border-bottom: 0px solid white;">' +HtmlBody + '</div></body></html>';
  }
else if(InvoiceType == 'Transporter Copy')
{
  this.TransporterCopy = '<html><head> <style>body {font-family: Tahoma, Verdana, Segoe, sans-serif;} td, th {        border: 1px solid black;    text-align: left;font-size: 13px;    }table {    border-collapse: collapse;    width: 100%;}th, td {    padding: 2px;}th {-webkit-print-color-adjust: exact;background-color: gainsboro;} </style></head><body style="font-size: 12px;"> <div   style="border: 1px solid black;border-bottom: 0px solid white;">' +HtmlBody + '</div></body></html>';
}
else if(InvoiceType == 'Office Copy')
{
  this.OfficeCopy = '<html><head> <style> body {font-family: Tahoma, Verdana, Segoe, sans-serif;} td, th {        border: 1px solid black;    text-align: left;font-size: 13px;    }table {    border-collapse: collapse;    width: 100%;}th, td {    padding: 2px;}th {-webkit-print-color-adjust: exact;background-color: gainsboro;} </style></head><body style="font-size: 12px;"> <div   style="border: 1px solid black;border-bottom: 0px solid white;">' +HtmlBody + '</div></body></html>';
}



  }

  getInvoiceList() {
    let partyDetails = {
      SearchText: this.searchText || '',
      Page: {
        PageNumber: this.pageNumber,
        PageSize: 20,
      },
      InvoiceData: {
        CompanyId: this.globalCompanyId,
        InvoiceType: this.invlst,
      },
    };
    console.log('getInvoiceList partyDetails', partyDetails);

    this.adminService.getInvoiceList(partyDetails).subscribe({
      next: (res: any) => {
        this.spinner.show();
        if (!res.HasErrors && res?.Data !== null) {
          this.invoiceList = res.records;
          this.totalInvoice = res.totalCount;
          this.getPages();
          this.pagesObj.next(true);
        } else {
          this.toastr.error(res.Errors[0].Message);
          this.error = res.Errors[0].Message;
          this.getPages();
        }
        this.spinner.hide();
      },
      error: (error: any) => {
        this.spinner.hide();
        this.error = error;
        this.toastr.error('Something went wrong');
        console.log("error 3 -> " + error);
      },
    });
  }

  addParty(): void {
    let objVal = this.invlst;

    switch (objVal) {
      case 'GoodsInvoice':
        this.router.navigate(['/admin/Invoice'], {
          queryParams: { InvoiceType: 'GoodsInvoice' },
        });
        break;
      case 'GinningInvoice':
        this.router.navigate(['/admin/Invoice'], {
          queryParams: { InvoiceType: 'GinningInvoice' },
        });
        break;
      case 'DebitNote':
        this.router.navigate(['/admin/Invoice'], {
          queryParams: { InvoiceType: 'DebitNote' },
        });
        break;
      case 'ExportInvoice':
        this.router.navigate(['/admin/Invoice'], {
          queryParams: { InvoiceType: 'ExportInvoice' },
        });
        break;
      case 'PurchaseReturn':
        this.router.navigate(['/admin/Invoice'], {
          queryParams: { InvoiceType: 'PurchaseReturn' },
        });
        break;
      case 'ProfarmaInvoice':
        this.router.navigate(['/admin/Invoice'], {
          queryParams: { InvoiceType: 'ProfarmaInvoice' },
        });
        break;
      case 'DeemedExport':
        this.router.navigate(['/admin/Invoice'], {
          queryParams: { InvoiceType: 'DeemedExport' },
        });
        break;
      case 'BuiltyPurchase':
        this.router.navigate(['/admin/Invoice'], {
          queryParams: { InvoiceType: 'BuiltyPurchase' },
        });
        break;
      case 'OtherGSTBills':
        this.router.navigate(['/admin/Invoice'], {
          queryParams: { InvoiceType: 'OtherGSTBills' },
        });
        break;
      case 'VerifyBills':
        this.router.navigate(['/admin/Invoice'], {
          queryParams: { InvoiceType: 'VerifyBills' },
        });
        break;
      case 'DeemedPurchase':
        this.router.navigate(['/admin/Invoice'], {
          queryParams: { InvoiceType: 'DeemedPurchase' },
        });
        break;
      case 'CreditNote':
        this.router.navigate(['/admin/Invoice'], {
          queryParams: { InvoiceType: 'CreditNote' },
        });
        break;
        case 'SalesReturn':
          this.router.navigate(['/admin/Invoice'], {
            queryParams: { InvoiceType: 'SalesReturn' },
          });
          break;

      default:
        break;
    }
  }

  SeperateComma(value: any) {

    if(value == undefined)
    {
      value = 0;
    }


    if (!this.isDebitNote) { return value; }
    var x = value;
    x = x.toString();
    var afterPoint = '';
    if (x.indexOf('.') > 0)
      afterPoint = x.substring(x.indexOf('.'), x.length);
    x = Math.floor(x);
    x = x.toString();
    var lastThree = x.substring(x.length - 3);
    var otherNumbers = x.substring(0, x.length - 3);
    if (otherNumbers != '')
      lastThree = ',' + lastThree;
    var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree + afterPoint;
    return res
  }


  onEdit(data: any): void {
    debugger;
    console.log(data['vochNo']);

    var InvoiceName = '';
    this.activatedRoute.queryParams.subscribe(params => {
      InvoiceName = params['InvoiceName'];
    });


    this.router.navigateByUrl('/admin/Invoice?InvoiceType=' + String(InvoiceName) + '&InvoiceNo=' + String(data['vochNo']) + '&VochType=' + String(data['vochType']));
    //sessionStorage.setItem('invoiceDataAll', JSON.stringify(data));
    //this.router.navigate(['/admin/Invoice?InvoiceType=GoodsInvoice&InvoiceNo=' + data.vochNo]);
    //this.openViewInvoiceModal();
  }

  delete(): void {
    Swal.fire({
      title: 'Are you sure?',
      text: 'Do you want to delete this record!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Confirm',
    }).then((result) => {
      if (result.isConfirmed) {
        Swal.fire('Deleted!', 'Record has been deleted.', 'success');
      }
    });
  }

  onSearch(text: string) {
    text.length < 3 ? (this.errorMsg = true) : (this.error = false);

    if (text.length === 0) {
      this.searchText = '';
      this.errorMsg = false;
      this.getInvoiceList();
    }

    if (text.length >= 3) {
      this.errorMsg = false;
      this.searchText = text;
      this.getInvoiceList();
    }
  }

  next() {
    if (this.commonService.gettotalPages(this.totalInvoice) > 5) {
      let lastIndex = this.pages.length - 1;
      let nextPage = this.pages[lastIndex];
      this.pages.shift();
      this.pages.push(nextPage + 1);
      this.pageNumber++;
      this.getInvoiceList();
    } else {
      this.pageNumber++;
      this.getInvoiceList();
    }
  }

  previous() {
    if (this.commonService.gettotalPages(this.totalInvoice) > 5) {
      this.pages.pop();
      let lastPage = this.pages[0];
      this.pages.unshift(lastPage - 1);
      this.pageNumber--;
      this.getInvoiceList();
    } else {
      this.pageNumber--;
      this.getInvoiceList();
    }
  }

  getPages() {
    this.pages = [];
    for (
      let i = 1;
      i <= this.commonService.gettotalPages(this.totalInvoice);
      i++
    ) {
      this.pages.length < 5 ? this.pages.push(i) : '';
    }
    this.pagesObj.complete();
  }

  changePage(currentPage: number) {
    this.pageChanged = true;
    this.pageNumber = currentPage;
    this.getInvoiceList();
  }

  openViewInvoiceModal() {
    const dialogRef = this.dialog.open(this.invoiceDialog, {
      width: '60%', // 80% of the screen width
      height: '60%', // 80% of the screen height
    });

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }
}
