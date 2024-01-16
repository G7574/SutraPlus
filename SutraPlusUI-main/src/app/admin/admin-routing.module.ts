import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PartyMasterComponent } from './components/party-master/party-master.component';
import { AddPartyMasterComponent } from './components/add-party-master/add-party-master.component';
import { EditPartyMasterComponent } from './components/edit-party-master/edit-party-master.component';
import { OtherAccountComponent } from './components/other-account/other-account.component';
import { AddOtherAccountComponent } from './components/add-other-account/add-other-account.component';
import { UserMasterComponent } from './components/user-master/user-master.component';
import { AddUserMasterComponent } from './components/add-user-master/add-user-master.component';
import { EditUserMasterComponent } from './components/edit-user-master/edit-user-master.component';
import { ProductMasterComponent } from './components/product-master/product-master.component';
import { AddProductComponent } from './components/add-product/add-product.component';
import { EditProductComponent } from './components/edit-product/edit-product.component';
import { EditOtherAccountComponent } from './components/edit-other-account/edit-other-account.component';
// import { GoodsInvoiceDashboardComponent } from './components/Sale/goods-invoice/goods-invoice-dashboard/goods-invoice-dashboard.component';
// import { ViewGoodsInvoiceComponent } from './components/Sale/goods-invoice/view-goods-invoice/view-goods-invoice.component';
// import { AddGoodsInvoiceComponent } from './components/Sale/goods-invoice/add-goods-invoice/add-goods-invoice.component';
// import { GinningInvoiceDashboardComponent } from './components/Sale/ginning-invoice/ginning-invoice-dashboard/ginning-invoice-dashboard.component';
// import { ViewGinningInvoiceComponent } from './components/Sale/ginning-invoice/view-ginning-invoice/view-ginning-invoice.component';
// import { AddGinningInvoiceComponent } from './components/Sale/ginning-invoice/add-ginning-invoice/add-ginning-invoice.component';
// import { ExportInvoiceDashboardComponent } from './components/Sale/export-invoice/export-invoice-dashboard/export-invoice-dashboard.component';
// import { ViewExportInvoiceComponent } from './components/Sale/export-invoice/view-export-invoice/view-export-invoice.component';
// import { AddExportInvoiceComponent } from './components/Sale/export-invoice/add-export-invoice/add-export-invoice.component';
import { GoodsInvoiceDashboardComponent } from './components/sales/goods-invoice/goods-invoice-dashboard/goods-invoice-dashboard.component';
import { AddGoodsInvoiceComponent } from './components/sales/goods-invoice/add-goods-invoice/add-goods-invoice.component';
import { ViewGoodsInvoiceComponent } from './components/sales/goods-invoice/view-goods-invoice/view-goods-invoice.component';
import { GinningInvoiceDashboardComponent } from './components/sales/ginning-invoice/ginning-invoice-dashboard/ginning-invoice-dashboard.component';
import { ViewGinningInvoiceComponent } from './components/sales/ginning-invoice/view-ginning-invoice/view-ginning-invoice.component';
import { AddGinningInvoiceComponent } from './components/sales/ginning-invoice/add-ginning-invoice/add-ginning-invoice.component';
import { ExportInvoiceDashboardComponent } from './components/sales/export-invoice/export-invoice-dashboard/export-invoice-dashboard.component';
import { ViewExportInvoiceComponent } from './components/sales/export-invoice/view-export-invoice/view-export-invoice.component';
import { AddExportInvoiceComponent } from './components/sales/export-invoice/add-export-invoice/add-export-invoice.component';
import { ReportsMainComponent } from './reports-main/reports-main.component';
import { BankJournalEntriesComponent } from './components/VoucherEntries/bank-journal-entries/bank-journal-entries.component';
import { AkadaEntryComponent } from './components/purchase/akada-entry/akada-entry/akada-entry.component';
import { CashEntryComponent } from './components/VoucherEntries/cash-entry/cash-entry.component';
import { ProductionEntryComponent } from './components/sales/ProductionEntry/production-entry/production-entry.component';
import { OptionSettingsComponent } from './components/option-settings/option-settings/option-settings.component';
import { CreateDcComponent } from './components/sales/create-dc/create-dc/create-dc.component';
import { ImportEInvoiceComponent } from './components/sales/import-e-invoice/import-e-invoice.component';
import { ItemWiseReportComponent } from './components/item-wise-report/item-wise-report.component';
import { MonthWiseReportComponent } from './components/month-wise-report/month-wise-report.component';
import { PartyWiseCaseReportComponent } from './components/party-wise-case-report/party-wise-case-report.component';
import { PartyWiseCommHamaliComponent } from './components/party-wise-comm-hamali/party-wise-comm-hamali.component';
import { ListAndRegistersComponent } from './components/list-and-registers/list-and-registers.component';

const routes: Routes = [
  { path: '', component: DashboardComponent },
  // party
  { path: 'party-details', component: PartyMasterComponent },
  { path: 'add-party-details', component: AddPartyMasterComponent },
  { path: 'edit-party-details/:id', component: EditPartyMasterComponent },
  // other account
  { path: 'other-account', component: OtherAccountComponent },
  { path: 'add-other-account', component: AddOtherAccountComponent },
  { path: 'edit-other-account/:id', component: EditOtherAccountComponent },
  // user
  { path: 'create-user', component: UserMasterComponent },
  { path: 'add-user', component: AddUserMasterComponent },
  { path: 'edit-user/:id', component: EditUserMasterComponent },
  // product
  { path: 'product-master', component: ProductMasterComponent },
  { path: 'add-product', component: AddProductComponent },
  { path: 'edit-product/:id', component: EditProductComponent },
  // goods invoice
  { path: 'InvoiceList', component: GoodsInvoiceDashboardComponent },
  { path: 'view-goods-invoice', component: ViewGoodsInvoiceComponent },
  { path: 'Invoice', component: AddGoodsInvoiceComponent },
  // ginning invoice
  //{ path: 'GinningInvoiceList', component: GinningInvoiceDashboardComponent },
  { path: 'view-ginning-invoice', component: ViewGinningInvoiceComponent },
  //{ path: 'Invoice', component: AddGoodsInvoiceComponent },
  // ginning invoice
  // { path: 'export-invoice-dashboard', component: ExportInvoiceDashboardComponent },
  { path: 'view-export-invoice', component: ViewExportInvoiceComponent },
  //{ path: 'add-export-invoice', component: AddExportInvoiceComponent },
  { path: 'reports', component: ReportsMainComponent },
  { path: 'ItemWise', component: ItemWiseReportComponent },
  { path: 'PartyWiseCaseReport', component: PartyWiseCaseReportComponent },
  { path: 'PartyWiseCommHamali', component: PartyWiseCommHamaliComponent },
  { path: 'ListAndRegisters', component: ListAndRegistersComponent },
  { path: 'MonthWise', component: MonthWiseReportComponent },
  { path: 'BankJournal', component: BankJournalEntriesComponent },
  { path: 'akada-entry', component: AkadaEntryComponent },
  { path: 'BankJournal', component: BankJournalEntriesComponent },
  { path: 'CashEntry', component: CashEntryComponent },
  { path: 'Production-Entry', component: ProductionEntryComponent },
  { path: 'create-dc', component: CreateDcComponent },
  { path: 'import-e-invoice', component: ImportEInvoiceComponent },
  { path: 'option-settings', component: OptionSettingsComponent },
   {path:'Import Excel Invoices',component:ImportEInvoiceComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
