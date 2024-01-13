import { Component, Input } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';

import { ClassToggleService, HeaderComponent } from '@coreui/angular';

@Component({
  selector: 'app-default-header',
  templateUrl: './default-header.component.html',
})
export class DefaultHeaderComponent extends HeaderComponent {

  @Input() sidebarId: string = "sidebar";

  public newMessages = new Array(4)
  public newTasks = new Array(5)
  public newNotifications = new Array(5)

  financialYear!: string | null;
  companyName!: string | null;
  userDetails: any;
  userName!: string

  constructor(
    private classToggler: ClassToggleService,
    private router: Router
    ) {
    super();
  }

  ngOnInit(): void {
    // this.currentYear = new Date().getFullYear();
    this.financialYear = sessionStorage.getItem('financialYear')
    this.companyName = sessionStorage.getItem('companyName')

    this.userDetails = sessionStorage.getItem('userDetails')
    this.userDetails = JSON.parse(this.userDetails);
    this.userName = this.userDetails?.result?.UserEmailId;
  }

  changePassword(): void {
    this.router.navigate(['profile/change-password']);
  }

  logOut(): void {

    sessionStorage.removeItem('userDetails');
    sessionStorage.removeItem('financialYear');
    sessionStorage.removeItem('companyName')

  //  sessionStorage.clear();

    this.router.navigate(['/login']).then(
      () => console.log('Navigation to login succeeded'),
      (error) => console.error('Navigation to login failed:', error),
    );

  }

  navigateToProfile(): void{
    this.router.navigate(['profile/user-profile']);
  }

}
