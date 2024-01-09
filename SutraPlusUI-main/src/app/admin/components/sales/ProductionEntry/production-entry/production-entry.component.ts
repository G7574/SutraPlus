import { Component } from '@angular/core';
import { ProductionMaster } from '../../models/production-master.model';
import { AdminServicesService } from 'src/app/admin/services/admin-services.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-production-entry',
  templateUrl: './production-entry.component.html',
  styleUrls: ['./production-entry.component.scss'],
})
export class ProductionEntryComponent {
  globalCompanyId!: number;
  productionList: ProductionMaster[] = [];
  addEntryModel: ProductionMaster = new ProductionMaster();
  ngOnInit() { }

  constructor(
    private adminService: AdminServicesService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
  ) {
    this.spinner.show();
    this.globalCompanyId = Number(sessionStorage.getItem('companyID'));
    this.getList();
    this.spinner.hide();
  }

  getList() {
    let CompanyDetails = {
      CompanyId: this.globalCompanyId,
    };
    this.adminService.getAllProductionEntries(CompanyDetails).subscribe({
      next: (res: any) => {
        // console.log("get all ", res);
        if (res && res.ProductionEntries) {
          for (let data of res.ProductionEntries) {
            data.ledgerName = `${data.ledgerName} - ${data.place}`;
          }
          this.productionList = res.ProductionEntries;
        }
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong');
      },
    });
  }

  addEntry() {
    this.addEntryModel.Companyid = this.globalCompanyId;
    // console.log("before saving entry", this.addEntryModel);
    this.adminService.addProductionEntry(this.addEntryModel).subscribe({
      next: (res: any) => {
        // console.log("add entry ", res);
        if (res) {
          this.getList();
          // Clear the form or reset to default values
          this.addEntryModel = new ProductionMaster();
        }
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Something went wrong');
      },
    });
  }

  formatDate(dateInput: string | Date | null): string | null {
    if (dateInput === null) {
      return null; // or return a default value or throw an error
    }

    const date = dateInput instanceof Date ? dateInput : new Date(dateInput);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // January is 0!
    const year = date.getFullYear();

    return `${day}-${month}-${year}`;
  }

  clearForm() {
    this.addEntryModel = new ProductionMaster();
  }
}
