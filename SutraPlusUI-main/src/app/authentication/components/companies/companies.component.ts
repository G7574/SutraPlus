import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationServiceService } from '../../services/authentication-service.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { CommonService } from 'src/app/share/services/common.service';

@Component({
  selector: 'app-companies',
  templateUrl: './companies.component.html',
  styleUrls: ['./companies.component.scss']
})
export class CompaniesComponent implements OnInit {
  currentYear!: number
  financialYear!: string | null;
  companyList: any
  error: any;
  companyName!: string | null;
  p: any
  userDetails: any;
  userEmail: any;
  userType:any;

  constructor(
    private router: Router,
    private authenticationService: AuthenticationServiceService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    public commonService: CommonService
  ) { }

  ngOnInit(): void {
    this.currentYear = new Date().getFullYear();
    this.financialYear = sessionStorage.getItem('financialYear');
    this.companyName = sessionStorage.getItem('companyName');
    this.userDetails = sessionStorage.getItem('userDetails')
    this.userDetails = JSON.parse(this.userDetails);
    this.userType = this.userDetails['result'].UserType;




    this.userEmail = this.userDetails?.result?.UserEmailId;
    this.getData();
  }

  ngAfterViewInit() {
    this.commonService.setTheme();
  }

  goToDashboard(data: any): void {
    this.spinner.show();
    ;
    sessionStorage.setItem('companyName', data.CompanyName);
    sessionStorage.setItem('companyID', data.CompanyId);
    sessionStorage.setItem('logo', data.Logo);
    sessionStorage.setItem('companyPlace', data.Place);
    sessionStorage.setItem('State', data.State);

    this.userDetails = sessionStorage.getItem('userDetails')
    let userObj = JSON.parse(this.userDetails);

    // if(userObj?.result?.UserType == 'Administrator'){
    //   this.router.navigate(['/admin']);
    // }else{
    //   this.router.navigate(['/user']);
    // }
    this.spinner.hide();
    this.router.navigate(['/admin']);
  }

  getData(): void {
    this.spinner.show();

    this.authenticationService.getCompany('').subscribe({
      next: (res: any) => {
        if (!res.HasErrors && res?.Data !== null) {
          this.companyList = res.CompanyList
          // sessionStorage.setItem('companyName', this.companyList[0]?.Name)
          this.spinner.hide();
          if (this.companyList.length) {
          }
          else {
          }
        } else {
          this.toastr.error(res.Errors[0].Message);
          this.error = res.Errors[0].Message;
        }
      },
      error: (error: any) => {
        this.spinner.hide();
        this.error = error;
        this.toastr.error('Something went wrong');
      },
    });
  }

  addNew(): void {
    this.router.navigate(['/new-company'])
  }

  onEdit(item: any): void {
    this.router.navigate(['/edit-company/' + item.CompanyId])
  }

  onDelete(): void {
    Swal.fire({
      title: 'Are you sure?',
      text: "Do you want to delete this record!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Confirm'
    }).then((result) => {
      if (result.isConfirmed) {
        Swal.fire(
          'Deleted!',
          'Your file has been deleted.',
          'success'
        )
      }
    })
  }

}
