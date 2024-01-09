import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DataService } from '../../core/services/data.service'
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationServiceService {

  constructor(
    private dataService: DataService
  ) { }

  // login api call
  login(UserDetails: any): Observable<any> {
    return this.dataService.postAnonymous('UserSecurity/Login', UserDetails);
  }

  getYearList(customerId: any): Observable<any> {
    return this.dataService.getAnonymous('Customer/Get/' + customerId);
  }

  getCompanyList(sessionId: any): Observable<any> {
    return this.dataService.getAnonymous('Company/GetCompanyList?UserSessionId=' + sessionId);
  }

  getCompany(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/List', data);
  }

  sendPassword(data: any): Observable<any> {
    return this.dataService.postAnonymous('Security/Forgot', data);
  }

  createCompany(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/Add', data);
  }

  updateCompany(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/Update', data);
  }

  saveEInvoice(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/SaveEInvoice', data);
  }

  getEInvoiceByCompanyId(id: any): Observable<any> {
    // return this.dataService.getAnonymous('Company/GetCompanyEInvoice/' + id);
    return this.dataService.postAnonymous('Company/GetCompanyEInvoice', id);
  }

  // saveEInvoice(data: any): Observable<any

}
