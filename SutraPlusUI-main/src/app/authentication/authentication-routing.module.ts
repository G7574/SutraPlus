import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticationComponent } from './authentication.component';
import { CompaniesComponent } from './components/companies/companies.component';
import { LandingPageComponent } from './components/landing-page/landing-page.component';
import { LoginComponent } from './components/login/login.component';
import { StartupPageComponent } from './components/startup-page/startup-page.component';
import { ForgotPasswordAdminComponent } from './components/forgot-password-admin/forgot-password-admin.component';
import { AddCompanyComponent } from './components/add-company/add-company.component';
import { EditCompanyComponent } from './components/edit-company/edit-company.component';

const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'landing-page', component: LandingPageComponent },
  { path: 'landing-page/:id', component: StartupPageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'companies', component: CompaniesComponent },
  { path: 'forgot-password-admin', component: ForgotPasswordAdminComponent },
  { path: 'new-company', component: AddCompanyComponent },
  { path: 'edit-company/:id', component: EditCompanyComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthenticationRoutingModule { }
