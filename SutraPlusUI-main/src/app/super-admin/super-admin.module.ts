import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ButtonModule,
  CardModule,
  FormModule,
  GridModule,
  NavbarModule,
  OffcanvasModule,
  NavModule,
  DropdownModule,
  FooterModule,
  TableModule,
  UtilitiesModule,
  ModalModule,
} from '@coreui/angular';
import { SuperAdminRoutingModule } from './super-admin-routing.module';
import { SuperAdminComponent } from './super-admin.component';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { IconModule } from '@coreui/icons-angular';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { CustomerComponent } from './components/customer/customer.component';
import { AddCustomerComponent } from './components/add-customer/add-customer.component';
import { EditCustomerComponent } from './components/edit-customer/edit-customer.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { ForgotPasswordComponent } from '../super-admin/components/forgot-password/forgot-password.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { EmailConfigComponent } from './components/email-config/email-config.component';

@NgModule({
  declarations: [
    SuperAdminComponent,
    LoginComponent,
    DashboardComponent,
    CustomerComponent,
    AddCustomerComponent,
    EditCustomerComponent,
    ForgotPasswordComponent,
    EmailConfigComponent,
  ],
  imports: [
    CommonModule,
    SuperAdminRoutingModule,
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
    UtilitiesModule,
    ModalModule,
    NgxSpinnerModule,
    NgMultiSelectDropDownModule,
    NgxPaginationModule,
  ],
})
export class SuperAdminModule {}
