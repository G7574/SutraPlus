import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { SuperAdminServiceService } from '../../services/super-admin-service.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.scss']
})
export class CustomerComponent implements OnInit {
  customerList: any
  error: any;

  constructor(
    private router: Router,
    private spinner: NgxSpinnerService,
    private superAdminService: SuperAdminServiceService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.getCustomer();
  }

  getCustomer(): void {
    this.spinner.show();
    let obj = {"SearchText":"","Page":{"PageNumber":1,"PageSize":10}};
    this.superAdminService.getCustomerList(obj).subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          this.customerList = res.CustomerList
        } else {
          this.toastr.error('Something went wrong');
          this.error = res.Errors[0].Message;
        }
        this.spinner.hide();
      },
      error: (error: any) => {
        this.spinner.hide();
        this.error = error;
        this.toastr.error('Something went wrong');
        alert(error)
      },
    });
  }

  newCustomer(): void {
    this.router.navigate(['/super-admin/add-customer'])
  }

  onEdit(id: number): void {
    this.router.navigate(['/super-admin/edit-customer/' + id])
  }

}
