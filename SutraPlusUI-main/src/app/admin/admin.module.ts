import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

import {
  AccordionModule,
  CardModule,
  DropdownModule,
  FooterModule,
  FormModule,
  NavModule,
  NavbarModule,
  OffcanvasModule,
  TableModule,
  UtilitiesModule,
  SharedModule,
  ModalModule,
  TooltipModule,
} from '@coreui/angular';
import { ButtonModule } from '@coreui/angular';
import { GridModule } from '@coreui/angular';
import { PartyMasterComponent } from './components/party-master/party-master.component';
import { AddPartyMasterComponent } from './components/add-party-master/add-party-master.component';
import { EditPartyMasterComponent } from './components/edit-party-master/edit-party-master.component';
import { IconModule } from '@coreui/icons-angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { OtherAccountComponent } from './components/other-account/other-account.component';
import { AddOtherAccountComponent } from './components/add-other-account/add-other-account.component';
import { UserMasterComponent } from './components/user-master/user-master.component';
import { AddUserMasterComponent } from './components/add-user-master/add-user-master.component';
import { EditUserMasterComponent } from './components/edit-user-master/edit-user-master.component';
import { ProductMasterComponent } from './components/product-master/product-master.component';
import { EditOtherAccountComponent } from './components/edit-other-account/edit-other-account.component';
import { AddProductComponent } from './components/add-product/add-product.component';
import { EditProductComponent } from './components/edit-product/edit-product.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgSelectModule } from '@ng-select/ng-select';
import { GoodsInvoiceDashboardComponent } from './components/sales/goods-invoice/goods-invoice-dashboard/goods-invoice-dashboard.component';
import { AddGoodsInvoiceComponent } from './components/sales/goods-invoice/add-goods-invoice/add-goods-invoice.component';
import { ViewGoodsInvoiceComponent } from './components/sales/goods-invoice/view-goods-invoice/view-goods-invoice.component';
import { GinningInvoiceDashboardComponent } from './components/sales/ginning-invoice/ginning-invoice-dashboard/ginning-invoice-dashboard.component';
import { AddGinningInvoiceComponent } from './components/sales/ginning-invoice/add-ginning-invoice/add-ginning-invoice.component';
import { ViewGinningInvoiceComponent } from './components/sales/ginning-invoice/view-ginning-invoice/view-ginning-invoice.component';
import { ExportInvoiceDashboardComponent } from './components/sales/export-invoice/export-invoice-dashboard/export-invoice-dashboard.component';
import { AddExportInvoiceComponent } from './components/sales/export-invoice/add-export-invoice/add-export-invoice.component';
import { ViewExportInvoiceComponent } from './components/sales/export-invoice/view-export-invoice/view-export-invoice.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { DecimalpointPipe } from '../pipe/decimalpoint.pipe';
import { MatDialogModule } from '@angular/material/dialog';
import { ReportsMainComponent } from './reports-main/reports-main.component';
import { MatTableModule } from '@angular/material/table';
import { ExportAsService } from 'ngx-export-as';
import { AkadaEntryComponent } from './components/purchase/akada-entry/akada-entry/akada-entry.component';

import { BankJournalEntriesComponent } from './components/VoucherEntries/bank-journal-entries/bank-journal-entries.component';
import { CashEntryComponent } from './components/VoucherEntries/cash-entry/cash-entry.component';
import { ProductionEntryComponent } from './components/sales/ProductionEntry/production-entry/production-entry.component';
import { OptionSettingsComponent } from './components/option-settings/option-settings/option-settings.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CreateDcComponent } from './components/sales/create-dc/create-dc/create-dc.component';
import { ImportEInvoiceComponent } from './components/sales/import-e-invoice/import-e-invoice.component';
import { ItemWiseReportComponent } from './components/item-wise-report/item-wise-report.component';
import { DxReportDesignerModule, DxReportViewerModule } from 'devexpress-reporting-angular';
import { MonthWiseReportComponent } from './components/month-wise-report/month-wise-report.component';
import { PartyWiseCaseReportComponent } from './components/party-wise-case-report/party-wise-case-report.component';
import { PartyWiseCommHamaliComponent } from './components/party-wise-comm-hamali/party-wise-comm-hamali.component';
import { ListAndRegistersComponent } from './components/list-and-registers/list-and-registers.component';
import { PrintPartyReportComponent } from './components/print-party-report/print-party-report.component';
import { PaymentListComponent } from './components/payment-list/payment-list.component';
import { DxDataGridModule } from 'devextreme-angular';
import { TDSReportComponent } from './components/tdsreport/tdsreport.component';
import { ItemWiseReportViewComponent } from './components/item-wise-report-view/item-wise-report-view.component';
import { CommonReportGeneratorComponent } from './components/common-report-generator/common-report-generator.component';
import { SelectBankDailogComponent } from './components/select-bank-dailog/select-bank-dailog.component';
import { PaymentListReportViewComponent } from './components/payment-list-report-view/payment-list-report-view.component';

@NgModule({
  declarations: [
    AdminComponent,
    DashboardComponent,
    PartyMasterComponent,
    AddPartyMasterComponent,
    EditPartyMasterComponent,
    OtherAccountComponent,
    AddOtherAccountComponent,
    UserMasterComponent,
    AddUserMasterComponent,
    EditUserMasterComponent,
    ProductMasterComponent,
    EditOtherAccountComponent,
    AddProductComponent,
    EditProductComponent,
    GoodsInvoiceDashboardComponent,
    AddGoodsInvoiceComponent,
    ViewGoodsInvoiceComponent,
    GinningInvoiceDashboardComponent,
    AddGinningInvoiceComponent,
    ViewGinningInvoiceComponent,
    ExportInvoiceDashboardComponent,
    AddExportInvoiceComponent,
    ViewExportInvoiceComponent,
    DecimalpointPipe,
    ReportsMainComponent,
    AkadaEntryComponent,
    BankJournalEntriesComponent,
    CashEntryComponent,
    ProductionEntryComponent,
    OptionSettingsComponent,
    CreateDcComponent,
    ImportEInvoiceComponent,
    ItemWiseReportComponent,
    CommonReportGeneratorComponent,
    ItemWiseReportViewComponent,
    MonthWiseReportComponent,
    PartyWiseCaseReportComponent,
    PartyWiseCommHamaliComponent,
    ListAndRegistersComponent,
    PrintPartyReportComponent,
    PaymentListComponent,
    TDSReportComponent,
    ItemWiseReportViewComponent,
    CommonReportGeneratorComponent,
    SelectBankDailogComponent,
    PaymentListReportViewComponent,
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    GridModule,
    CardModule,
    ButtonModule,
    CardModule,
    ButtonModule,
    GridModule,
    IconModule,
    FormModule,
    ReactiveFormsModule,
    NavbarModule,
    OffcanvasModule,
    NavModule,
    DropdownModule,
    FooterModule,
    TableModule,
    TooltipModule,
    UtilitiesModule,
    NgxSpinnerModule,
    NgMultiSelectDropDownModule,
    NgxPaginationModule,
    AccordionModule,
    SharedModule,
    ModalModule,
    NgSelectModule,
    FormsModule,
    MatAutocompleteModule,
    MatDialogModule,
    MatTableModule,
    MatFormFieldModule,
    DxReportViewerModule,
    DxDataGridModule,

  ],
  providers: [DatePipe, ExportAsService],
})
export class AdminModule { }
