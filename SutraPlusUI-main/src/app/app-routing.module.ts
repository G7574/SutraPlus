import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DefaultLayoutComponent } from './containers';
import { Page404Component } from './views/pages/page404/page404.component';
import { Page500Component } from './views/pages/page500/page500.component';
// import { LoginComponent } from './views/pages/login/login.component';
import { RegisterComponent } from './views/pages/register/register.component';
import { LoginComponent } from './super-admin/components/login/login.component';
import { AuthGuard } from './core/guards/auth.guard';
import { ForgotPasswordComponent } from './super-admin/components/forgot-password/forgot-password.component';
import { ItemWiseReportViewComponent } from './admin/components/item-wise-report-view/item-wise-report-view.component';
import { PaymentListReportViewComponent } from './admin/components/payment-list-report-view/payment-list-report-view.component';
import { GoodsInvoicePrintViewComponent } from './admin/components/sales/goods-invoice/goods-invoice-print-view/goods-invoice-print-view.component';

const routes: Routes = [
  // {
  //   path: 'authentication',
  //   redirectTo: 'dashboard',
  //   pathMatch: 'full'
  // },
  {
    path: '',
    loadChildren: () => import('./authentication/authentication.module').then(m => m.AuthenticationModule)
  },
  {
    path: '',
    redirectTo: '/startup-page',
    pathMatch: 'full',
  },
  {
    path: '',
    component: DefaultLayoutComponent,
    canActivate: [AuthGuard],
    data: {
      title: 'Home'
    },
    children: [
      // {
      //   path: 'dashboard',
      //   loadChildren: () =>
      //     import('./views/dashboard/dashboard.module').then((m) => m.DashboardModule)
      // },
      // {
      //   path: 'theme',
      //   loadChildren: () =>
      //     import('./views/theme/theme.module').then((m) => m.ThemeModule)
      // },
      // {
      //   path: 'base',
      //   loadChildren: () =>
      //     import('./views/base/base.module').then((m) => m.BaseModule)
      // },
      // {
      //   path: 'buttons',
      //   loadChildren: () =>
      //     import('./views/buttons/buttons.module').then((m) => m.ButtonsModule)
      // },
      // {
      //   path: 'forms',
      //   loadChildren: () =>
      //     import('./views/forms/forms.module').then((m) => m.CoreUIFormsModule)
      // },
      // {
      //   path: 'charts',
      //   loadChildren: () =>
      //     import('./views/charts/charts.module').then((m) => m.ChartsModule)
      // },
      // {
      //   path: 'icons',
      //   loadChildren: () =>
      //     import('./views/icons/icons.module').then((m) => m.IconsModule)
      // },
      // {
      //   path: 'notifications',
      //   loadChildren: () =>
      //     import('./views/notifications/notifications.module').then((m) => m.NotificationsModule)
      // },
      // {
      //   path: 'widgets',
      //   loadChildren: () =>
      //     import('./views/widgets/widgets.module').then((m) => m.WidgetsModule)
      // },
      {
        path: 'pages',
        loadChildren: () =>
          import('./views/pages/pages.module').then((m) => m.PagesModule)
      },

      // main routing
      // {
      //   path: 'super-admin',
      //   loadChildren: () => import('./super-admin/super-admin.module').then(m => m.SuperAdminModule)
      // },
      {
        path: 'admin',
        loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule)
      },
      {
        path: 'user',
        loadChildren: () => import('./user/user.module').then(m => m.UserModule)
      },
      {
        path: 'super-admin',
        loadChildren: () => import('./super-admin/super-admin.module').then(m => m.SuperAdminModule) },
      {
        path: 'profile',
        loadChildren: () =>
          import('./share/profile/profile.module').then(
            (m) => m.ProfileModule
          ),
      },
    ]
  },
  {
    path: '404',
    component: Page404Component,
    data: {
      title: 'Page 404'
    }
  },
  {
    path: '500',
    component: Page500Component,
    data: {
      title: 'Page 500'
    }
  },
  {
    path: 'login',
    component: LoginComponent,
    data: {
      title: 'Login Page'
    }
  },
  {
    path: 'register',
    component: RegisterComponent,
    data: {
      title: 'Register Page'
    }
  },
  {
    path: 'super-admin-login',
    component: LoginComponent,
  },
  {
    path: 'forgot-password',
    component: ForgotPasswordComponent,
  },
  {
    path: 'ReportView',
    component: ItemWiseReportViewComponent,
  },
  {
    path: 'PaymentListReportView',
    component: PaymentListReportViewComponent,
  },
  {
    path: 'GoodsInvoicePrintView',
    component: GoodsInvoicePrintViewComponent,
  },
  { path: 'profile', loadChildren: () => import('./share/profile/profile.module').then(m => m.ProfileModule) },
  // { path: 'super-admin', loadChildren: () => import('./super-admin/super-admin.module').then(m => m.SuperAdminModule) },

  {path: '**', redirectTo: 'dashboard'},


];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      scrollPositionRestoration: 'top',
      anchorScrolling: 'enabled',
      initialNavigation: 'enabledBlocking'
      // relativeLinkResolution: 'legacy'
    })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
