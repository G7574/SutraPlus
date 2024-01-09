import { Injectable } from '@angular/core';
import {
  HttpBackend,
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { tap, catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  private loginHttpClient: HttpClient;

  constructor(private http: HttpClient, handler: HttpBackend) {
    this.loginHttpClient = new HttpClient(handler);
  }

  /**
   * Gets data from api as entity
   * @param url api path
   */
  get<T>(url: string): Observable<T> {
    ;
    return this.httpRequest(
      this.http.get(environment.api + url, this.getHttpHeader())
    ) as Observable<T>;
  }

  /**
   * Gets data from api as an entity without authorization
   * @param url api path
   */
  getAnonymous<T>(url: any): Observable<T> {
    var tenantId: any = sessionStorage.getItem('tenantId');
  /*  const headers = new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8; multipart/form-data',
      'Cache-control': 'no-cache',
      'Access-Control-Allow-Credentials': 'true',
      // 'Content-Type': 'multipart/form-data',
      'Accept': 'application/json',
      Expires: '0',
      Pragma: 'no-cache',
      tenantId: atob(tenantId)
    });
    headers.append('Content-Type', 'application/json');
    headers.append('Access-Control-Allow-Origin', '*');
    headers.append('Access-Control-Allow-Methods', '*');
    headers.append('Access-Control-Expose-Headers', '*');
*/
const headers = new HttpHeaders({});
    return this.httpRequest(
      this.http.get(environment.api + url, {
        headers: headers,
      })
    ) as Observable<T>;
  }
  /**
   * Sends payload to api
   * @param url api path
   * @param data payload to send
   */
  post<T, M>(url: string, data: M): Observable<T> {
    const formData: any = new FormData();
    formData.append('RequestData', JSON.stringify(data));
    const body = JSON.stringify(data);
    return this.httpRequest(
      this.http.post(environment.api + url, body, this.getHttpHeader())
    ).pipe(map((response: any) => response)) as Observable<T>;
  }
  /**
   * Sends payload to api without authorization
   * @param url api path
   * @param data payload to send
   */

  postAnonymous<T>(url: string, data: T): Observable<any> {
    var tenantId: any = sessionStorage.getItem('tenantId');
    const headers = new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8; multipart/form-data',
      // 'Content-Type': 'multipart/form-data',
      'Cache-control': 'no-cache',
      'Accept': 'application/json',
      Expires: '0',
      Pragma: 'no-cache',
      ar: '5e3d803410d9620e488249eb',
      tenantId: atob(tenantId)
    });
    headers.append('Content-Type', 'application/json; charset=utf-8');
    headers.append('Access-Control-Allow-Origin', '*');
    headers.append('Access-Control-Allow-Methods', '*');
    headers.append('Access-Control-Expose-Headers', '*');

    return this.httpRequest(
      this.http.post(environment.api + url, JSON.stringify(data), {
        headers: headers,
      })
    ).pipe(map((response: any) => response)) as Observable<any>;
  }

  uploadDocument<T>(url: string, data: T): Observable<any> {
    const formData = new FormData();
    formData.append('RequestData', JSON.stringify(data));
    // formData.append('RequestFile', JSON.stringify(file));
    // const body = JSON.stringify(data);
    const headers = new HttpHeaders()
      .set('Content-Type', 'multipart/form-data')
      .set('Cache-control', 'no-cache')
      .set('Cache-control', 'no-store')
      .set('Expires', '0')
      .set('Pragma', 'no-cache')

      .set('Access-Control-Expose-Headers', '*');
    return this.httpRequest(
      this.http.post(environment.api + url, data, {
        headers: headers,
      })
    ).pipe(map((response: any) => response)) as Observable<any>;
  }

  /**
   * Calls api to delete an entity
   * @param url api path with id
   */
  delete(url: string): Observable<boolean>;
  /**
   * Calls api to delete an entity
   * @param url api path
   * @param id Item id to be deleted
   */
  delete(url: string, id: string): Observable<boolean>;
  delete(url: string, id?: string): Observable<boolean> {
    if (id) {
      url += '/' + id;
    }
    return this.httpRequest(this.http.delete(environment.api + url)).pipe(
      map((response) => response && true)
    ) as Observable<boolean>;
  }

  /**
   * Update using only url
   * @param url api path
   */
  put<T>(url: string): Observable<boolean>;
  /**
   * Sends payload to api to update an entity
   * @param url api path
   * @param data payload to send
   */
  put<T>(url: string, data: T): Observable<boolean>;
  put<T>(url: string, data?: T): Observable<boolean> {
    let preparedData;
    if (data) {
      preparedData = JSON.stringify(data);
    } else {
      preparedData = null;
    }
    return this.httpRequest(
      this.http.put(environment.api + url, preparedData, this.getHttpHeader())
    ).pipe(map((response) => response && true)) as Observable<boolean>;
  }

  /**
   * Sends payload to api to update some properties of entity
   * @param url api path
   * @param data payload to send
   */
  patch<T>(url: string, data: T): Observable<boolean> {
    return this.httpRequest(
      this.http.patch(
        environment.api + url,
        JSON.stringify(data),
        this.getHttpHeader()
      )
    ).pipe(map((response) => response && true)) as Observable<boolean>;
  }

  // TODO: Use http interceptor instead
  private httpRequest(response: Observable<Object>): Observable<Object> {
    return response.pipe(
      tap(() => { }),
      catchError((err: HttpErrorResponse) => {
        // if ((err.status === 400) || (err.status === 404)) {
        //   alert(err.error.message);
        return throwError(err);
        // }
      })
    );
  }

  private getHttpHeader(): { headers: HttpHeaders } {
    return {
      headers: new HttpHeaders()
        .set('Authorization', `Bearer ${sessionStorage.getItem('AccessToken')}`)
        // .set('Content-Type', 'application/json')
        .set('Cache-control', 'no-cache')
        .set('Cache-control', 'no-store')
        .set('Expires', '0')
        .set('Pragma', 'no-cache')

        .set('Access-Control-Expose-Headers', '*'),
    };
  }
  postPDF<T, M>(url: string, data: M): Observable<T> {
    return this.httpRequest(
      this.http.post(
        environment.api + url,
        JSON.stringify(data),
        this.getHttpHeadersForPDF()
      )
    ).pipe(map((response: any) => response)) as Observable<T>;
  }
  private getHttpHeadersForPDF(): { headers: HttpHeaders } {
    return {
      headers: new HttpHeaders()
        // .set(
        //   'Authorization',
        //   `Bearer  ${sessionStorage.getItem('AccessToken')}`
        // )
        .set('Content-Type', 'application/json')
        .set('Cache-control', 'no-cache')
        .set('Cache-control', 'no-store')
        .set('Expires', '0')
        .set('Pragma', 'no-cache')
        .set('responseType', 'blob' as 'json')
        .set('observe', 'response' as 'body')
        .set('Access-Control-Expose-Headers', '*'),
    };
  }
  getDataFromObject(model: any) {
    const params = new URLSearchParams();
    // tslint:disable-next-line:forin
    for (const key in model) {
      params.set(key, model[key]);
    }
    params.toString();
    return params;
  }
}
