import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';
import { AdminServicesService } from '../../services/admin-services.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-bill-expense-rate-dailog',
  templateUrl: './bill-expense-rate-dailog.component.html',
  styleUrls: ['./bill-expense-rate-dailog.component.scss']
})
export class BillExpenseRateDailogComponent implements OnInit {

  constructor(
    private adminService: AdminServicesService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
  ) {

  }

  ngOnInit(): void {
    this.partyName = sessionStorage.getItem('partyName')

    this.Cess = sessionStorage.getItem('cessRate');
    this.WeighManFee = sessionStorage.getItem('weighManFeeRate');
    this.bag = sessionStorage.getItem('packingRate');
    this.Hamali = sessionStorage.getItem('hamaliRate');
    this.Dalali = sessionStorage.getItem('dalaliRate');

  }

  partyName : string = "";
  bag : any;
  Hamali : any;
  WeighManFee: any;
  Dalali: any;
  Cess: any;
  checkEd: boolean = false;

  onBagValueChange(event : any) {
    this.bag = event.target.value;
  }

  onBoolValueChange(event : any) {
    this.checkEd = event.target.checked;
  }

  onCessValueChange(event : any) {
    this.Cess = event.target.value;
  }

  onDalaliValueChange(event : any) {
    this.Dalali = event.target.value;
  }

  onWeightValueChange(event : any) {
    this.WeighManFee = event.target.value;
  }

  onHamaliValueChange(event : any) {
    this.Hamali = event.target.value;
  }

  dialog(): void {

    let text = "";

    if(this.checkEd) {
      text = "Do you really want to make changes in all the records ?";
    } else {
      text = "Do you really want to change this record ?";
    }

    Swal.fire({
      title: 'Are you sure?',
      text: text,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Confirm'
    }).then((result) => {
      if (result.isConfirmed) {
        this.submit();
      }
    })
  }

  submit() {
    this.spinner.show();
    let partyDetails = {
        "bag": this.bag,
        "Hamali": this.Hamali,
        "WeighManFee": this.WeighManFee,
        "Dalali": this.Dalali,
        "Cess": this.Cess,
        "checkEd": this.checkEd,
        "ledgerId": sessionStorage.getItem("ledgerId"),
    }

    this.adminService.updateData(partyDetails).subscribe({
      next: (res: any) => {
        if(res == true) {
          this.toastr.success('Data Updated Successfully');
        } else {
          this.toastr.error('Something went wrong');
        }
        this.spinner.hide();
      },
      error: (error: any) => {
        this.toastr.error('Something went wrong');
        this.spinner.hide();
      },
    });
  }

}
