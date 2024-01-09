import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthenticationRoutingModule } from './authentication-routing.module';
import { AuthenticationComponent } from './authentication.component';
import { LoginComponent } from './components/login/login.component';
import { StartupPageComponent } from './components/startup-page/startup-page.component';
import { CompaniesComponent } from './components/companies/companies.component';

import { ButtonModule, CardModule, FormModule, GridModule, NavbarModule, OffcanvasModule, NavModule, DropdownModule, FooterModule, TableModule, UtilitiesModule, TooltipModule } from '@coreui/angular';
import { IconModule } from '@coreui/icons-angular';
import { ReactiveFormsModule } from '@angular/forms';
import { LandingPageComponent } from './components/landing-page/landing-page.component';

import { NgxSpinnerModule } from "ngx-spinner";
import { ForgotPasswordAdminComponent } from './components/forgot-password-admin/forgot-password-admin.component';
import { AddCompanyComponent } from './components/add-company/add-company.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { EditCompanyComponent } from './components/edit-company/edit-company.component';

@NgModule({
  declarations: [
    AuthenticationComponent,
    LoginComponent,
    StartupPageComponent,
    CompaniesComponent,
    LandingPageComponent,
    ForgotPasswordAdminComponent,
    AddCompanyComponent,
    EditCompanyComponent
  ],
  imports: [
    CommonModule,
    AuthenticationRoutingModule,
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
    NgxPaginationModule
  ],
})
export class AuthenticationModule { }
