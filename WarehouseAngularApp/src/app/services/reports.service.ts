import { Host, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

import { Observable, pipe, of } from 'rxjs';
import { HostInfo } from '../services/hostInfo';
import { NewItem } from '../Models/NewItem';


@Injectable({
  providedIn: 'root',
})
export class ReportsService {

  hostInfo : HostInfo = new HostInfo();

  constructor(private http: HttpClient) { }

  getReports(type : string |  undefined): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.reportController + '/getReports?type='+type);
  }

  getReportsSortByDate(type : string |  undefined, orderDate : string | undefined): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.reportController + '/getReportsSortByDate?type='+type + '&orderWay=' + orderDate);
  }

  getReportsSortByType(type : string |  undefined,orderType : string | undefined): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.reportController + '/getReportsSortByType?type='+type + '&orderWay=' + orderType);
  }

  getReportFile(reportId : string | undefined) : Observable<Blob> {   
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.reportController + '/getReportPdf?reportId=' + reportId, { responseType: 'blob' });
}

  private handle(error: any) {
    return of (error.message);
  }
}
