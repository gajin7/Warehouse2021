import { Host, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

import { Observable, pipe, of } from 'rxjs';
import { HostInfo } from '../services/hostInfo';
import { NewItem } from '../Models/NewItem';
import { TakeLoadByDriverParams } from '../Models/TakeLoadByDriverParams';


@Injectable({
  providedIn: 'root',
})
export class LoadService {

  hostInfo : HostInfo = new HostInfo();

  constructor(private http: HttpClient) { }

  getUnloadedLoads(warehouse : string | undefined): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.loadController + '/getUnloadedLoads?warehouseId=' + warehouse,{ 'headers': { 'Access-Control-Allow-Origin': 'http://localhost:4200' } });
  }
  getLoadedLoads(warehouse : string | undefined): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.loadController + '/getLoadedLoads?warehouseId=' + warehouse);
  }
  getLoadingLoads(warehouse : string | undefined): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.loadController + '/getLoadingLoads?warehouseId=' + warehouse);
  }
  getLoadsById(loadId : string | undefined): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.loadController + '/getLoadsById?loadId=' + loadId);
  }
  takeLoadByDriver(params : TakeLoadByDriverParams): Observable<any> {
    return this.http.post(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.loadController + '/takeLoadByDriver', params);
  }

  finishLoading(params : TakeLoadByDriverParams): Observable<any> {
    return this.http.post(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.loadController + '/finishLoading', params);
  }

  getDriversLoad(email : string | undefined): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.loadController + '/getTakenLoadForDriver?email=' + email);
  }

  private handle(error: any) {
    return of (error.message);
  }
}
