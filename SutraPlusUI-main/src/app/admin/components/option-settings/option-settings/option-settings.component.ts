import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AdminServicesService } from 'src/app/admin/services/admin-services.service';
import { CommonService } from 'src/app/share/services/common.service';

@Component({
  selector: 'app-option-settings',
  templateUrl: './option-settings.component.html',
  styleUrls: ['./option-settings.component.scss'],
})
export class OptionSettingsComponent {
  globalCompanyId: number | null;

  selectedTCSOption: string;

  option1: boolean;
  option2: boolean;
  option194Q: boolean;
  outLetId: number;

  constructor(
    private adminService: AdminServicesService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    public commonService: CommonService
  ) {
    this.globalCompanyId = Number(sessionStorage.getItem('companyID'));
    this.outLetId = this.globalCompanyId;
  }

  ngOnInit() {
    this.getOptionSettings();
  }

  saveOption() {
    if (this.selectedTCSOption === 'TcsInInvoice') {
      // Execute code for when TCS in Invoice is selected
    } else if (this.selectedTCSOption === 'TcsInReciept') {
      // Execute code for when TCS in Receipt is selected
    }
  }
  // Function to handle API response
  private handleApiResponse(res: any): void {
    this.spinner.hide();
    if (!res.HasErrors && res?.Data !== null) {
      // Handle successful response if needed
    } else {
      this.toastr.error('Something went wrong');
    }
  }

  // Updated saveTCSOption function
  saveTCSOption() {
    console.log('selectedTCSOption:', this.selectedTCSOption);
    const data = {
      TCSREQ: this.selectedTCSOption === 'TcsInInvoice' ? 1 : 0,
      TCSREQinReceipt: this.selectedTCSOption === 'TcsInReceipt' ? 1 : 0,
      CompanyId: this.outLetId,
      Update: 'TCS',
    };
    console.log('data:', data);
    this.saveOptionSettings(data);
  }

  // Updated saveTDSOption function
  saveTDSOption() {
    const data = {
      AskTCS: this.option194Q ? 1 : 0,
      AskIndTCS: this.option194Q ? 1 : 0, // Adjust this based on your actual requirements
      CompanyId: this.outLetId,
      Update: 'TDS',
    };

    console.log('TDS Option Data:', data); // Add this line for debugging

    this.saveOptionSettings(data);
  }

  // Common function to save option settings
  private saveOptionSettings(data: any): void {
    this.adminService.saveOptionSettings(data).subscribe({
      next: (res: any) => this.handleApiResponse(res),
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong');
      },
    });
  }

  getOptionSettings() {
    let data = { CompanyId: this.outLetId };
    this.adminService.getOptionSettings(data).subscribe({
      next: (res: any) => {
        this.spinner.hide();
        console.log(res);
        this.selectedTCSOption =
          res.AskTcs === 1 ? 'TcsInInvoice' : 'TcsInReceipt';
        this.option194Q = res.AskIndtcs === 1;
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong');
      },
    });
  }
}
