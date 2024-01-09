import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SuperAdminComponent } from './super-admin.component';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { CustomerComponent } from './components/customer/customer.component';
import { AddCustomerComponent } from './components/add-customer/add-customer.component';
import { EditCustomerComponent } from './components/edit-customer/edit-customer.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { EmailConfigComponent } from './components/email-config/email-config.component';

const routes: Routes = [
  // { path: '', component: SuperAdminComponent },
  { path: '', component: DashboardComponent },

  { path: 'super-admin-login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent },
  // customer
  { path: 'customer', component: CustomerComponent },
  { path: 'add-customer', component: AddCustomerComponent },
  { path: 'edit-customer/:id', component: EditCustomerComponent },
  // forgot password
  { path: 'forgot-password', component: ForgotPasswordComponent },
  // email configuration
  { path: 'email-config', component: EmailConfigComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SuperAdminRoutingModule { }
