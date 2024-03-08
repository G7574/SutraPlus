import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AdminServicesService } from 'src/app/admin/services/admin-services.service';
import { party } from 'src/app/share/models/party';
import { get } from 'lodash-es';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-akada-entry-options-dialog',
  templateUrl: './akada-entry-options-dialog.component.html',
  styleUrls: ['./akada-entry-options-dialog.component.scss']
})
export class AkadaEntryOptionsDialogComponent {

  date: any = null;
  oldDate: any = null;
  partyName: any;
  globalCompanyId: any;
  partyList: party[] = [];
  ledgerId: any;

  constructor(private formBuilder: FormBuilder,
    private toastr: ToastrService,
    public dialogRef: MatDialogRef<AkadaEntryOptionsDialogComponent>,
    private spinner: NgxSpinnerService,
    private adminService: AdminServicesService,
    private dialog: MatDialog) {
    this.globalCompanyId = sessionStorage.getItem('companyID');
    this.date = sessionStorage.getItem("date");
    this.oldDate = sessionStorage.getItem("date");
  }

  onDateChange(oldDate: string) {

  }

  closeDialog(): void {
    this.dialogRef.close();
  }

  alterParty() {

    if(this.ledgerId > 0) {

    } else {
      this.toastr.error("Please Select the Party !");
      return;
    }

    Swal.fire({
      title: 'Are you sure?',
      text: "Do you really want to Change the Party!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Confirm'
    }).then((result) => {
      if (result.isConfirmed) {
        let akadaData = {
          "AkadaData": {
            "CompanyId": this.globalCompanyId,
            "NewLedgerId": this.ledgerId,
            "LedgerId": sessionStorage.getItem("ledgerId"),
            "TransDate": this.oldDate,
            "CommodityId": sessionStorage.getItem("commodityId"),
          }
        }

        this.adminService.UpdateAkadaParty(akadaData).subscribe({
                next: (res: any) => {
                  window.location.reload();
                },
                error: (error: any) => {
                  this.toastr.error("Something went wrong while UpdateAkadaParty");
                },
              });
      }
    })
  }

  deletePosting() {
    Swal.fire({
      title: 'Are you sure?',
      text: "Do you really want to Delete Posting!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Confirm'
    }).then((result) => {
      if (result.isConfirmed) {

      }
    })
  }

  deleteBill() {
    Swal.fire({
      title: 'Are you sure?',
      text: "Do you really want to Delete Bill!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Confirm'
    }).then((result) => {
      if (result.isConfirmed) {
        let akadaData = {
          "AkadaData": {
            "CompanyId": this.globalCompanyId,
            "LedgerId": sessionStorage.getItem("ledgerId"),
            "TransDate": this.oldDate,
            "CommodityId": sessionStorage.getItem("commodityId"),
          }
        }

        this.adminService.DeleteAllAkadaEntry(akadaData).subscribe({
                next: (res: any) => {
                  window.location.reload();
                },
                error: (error: any) => {
                  this.toastr.error("Something went wrong while deleting all");
                },
              });
      }
    })
  }

  transferAllTo() {
    Swal.fire({
          title: 'Are you sure?',
          text: "Do you really want to Transfer All!",
          icon: 'warning',
          showCancelButton: true,
          confirmButtonColor: '#3085d6',
          cancelButtonColor: '#d33',
          confirmButtonText: 'Confirm'
        }).then((result) => {
          if (result.isConfirmed) {
            let akadaData = {
              "AkadaData": {
                "CompanyId": this.globalCompanyId,
                "LedgerId": sessionStorage.getItem("ledgerId"),
                "NewTransDate": this.date,
                "TransDate": this.oldDate,
                "CommodityId": sessionStorage.getItem("commodityId"),
              }
            }

            this.adminService.TransferTo(akadaData).subscribe({
                    next: (res: any) => {
                      window.location.reload();
                    },
                    error: (error: any) => {
                      this.toastr.error("Something went wrong while Transfering all");
                    },
                  });
          }
        })
  }

  alterDate() {

    Swal.fire({
      title: 'Are you sure?',
      text: "Do you really want to Change the Date!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Confirm'
    }).then((result) => {
      if (result.isConfirmed) {

        let akadaData = {
          "AkadaData": {
            "CompanyId": this.globalCompanyId,
            "LedgerId": sessionStorage.getItem("ledgerId"),
            "NewTransDate": this.date,
            "TransDate": this.oldDate,
            "CommodityId": sessionStorage.getItem("commodityId"),
          }
        }

        this.adminService.UpdateAkadaTransDate(akadaData).subscribe({
                next: (res: any) => {
                  window.location.reload();
                },
                error: (error: any) => {
                  this.toastr.error("Something went wrong while updating date");
                },
              });
      }

    })

  }

  getPartyList(text: string) {
    console.log("get party list text", text);
    if (text.length >= 1) {
      let partyDetails = {
        "LedgerData": {
          "CompanyId": this.globalCompanyId,
          "LedgerType": "Sales Ledger",
          "Country": ""
        },
        "SearchText": text,
        "Page": {
          "PageNumber": "1",
          "PageSize": "10"
        }
      }
      this.adminService.getLedgerList(partyDetails).subscribe({
        next: (res: any) => {
          for (let data of res.records) {
            data.ledgerName = `${data.ledgerName} - ${data.place}`;
          }
          this.partyList = res.records;

        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Something went wrong');
        },
      });
    }
    else {
      this.partyList = [];
    }
  }

  onClickTest($event: any, item: any) {
    this.ledgerId = item.ledgerId;
  }

}
