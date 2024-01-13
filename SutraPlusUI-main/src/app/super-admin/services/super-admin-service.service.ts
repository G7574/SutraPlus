import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DataService } from '../../core/services/data.service'

@Injectable({
  providedIn: 'root'
})
export class SuperAdminServiceService {

  constructor(
    private dataService: DataService
  ) { }

  login(UserDetails: any): Observable<any> {
    return this.dataService.postAnonymous('SuperAdminSecurity/Login', UserDetails);
  }

  getEmailConfig() {
    return this.dataService.getAnonymous('SuperAdminSecurity/GetEmailConfig');
  }

  getUser(data: any) {
    return this.dataService.postAnonymous('UserSecurity/GetUserData',data);
  }

  updateUser(data: any) {
    return this.dataService.postAnonymous('UserSecurity/UpdateUserData',data);
  }

  saveEmailConfig(emailConfig: any) {
    return this.dataService.postAnonymous('SuperAdminSecurity/SaveEmailConfig', emailConfig);
  }

  getCustomerList(data: any): Observable<any> {
    return this.dataService.postAnonymous('Customer/List', data);
  }

  getFinancialYears(): Observable<any> {
    return this.dataService.getAnonymous('Common/GetFinancialYear');
  }

  sendPassword(data: any): Observable<any> {
    return this.dataService.postAnonymous('SuperAdmin/Forgot', data);
  }

  getCustomerByID(id: any): Observable<any> {
    return this.dataService.getAnonymous('Customer/GetSingle/' + id);
  }

  getStates(): Observable<any> {
    return this.dataService.getAnonymous('Common/Get')
  }

  createCustomer(data: any): Observable<any> {
    return this.dataService.postAnonymous('Customer/Add', data);
  }

  editCustomer(data: any): Observable<any> {
    return this.dataService.postAnonymous('Customer/Update', data);
  }

  sendOtp(data: any): Observable<any> {
    return this.dataService.postAnonymous('Customer/GenerateOTP', data);
  }

  activeDeactiveCustomer(status: boolean, id: any): Observable<any> {
    if (status == true) {
      return this.dataService.postAnonymous('Customer/Delete', id);
    } else {
      return this.dataService.postAnonymous('Customer/Activate', id);
    }
  }

  getSingleCompany(data: any): Observable<any> {
    return this.dataService.postAnonymous(`Company/Get/${data}`, '');
  }

  updateCompany(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/GetSingle', data);
  }
}
