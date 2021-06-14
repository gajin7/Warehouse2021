import { Host, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

import { Observable, pipe, of } from 'rxjs';
import { HostInfo } from '../services/hostInfo';
import { NewItem } from '../Models/NewItem';
import { Company } from '../Models/Company';
import { Ramp } from '../Models/Ramp';


@Injectable({
  providedIn: 'root',
})
export class RampService {
  isLoggedin = false;
  hostInfo : HostInfo = new HostInfo();


  constructor(private http: HttpClient) { }

  getRamps(warehouseId : string | undefined)
  {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.rampController + '/getRamps?warehouseId=' + warehouseId,{ 'headers': { 'Access-Control-Allow-Origin': 'http://localhost:4200' } });
  }

  addRamp(ramp : Ramp)
  {
    return this.http.post(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.rampController + '/addRamp',ramp,{ 'headers': { 'Access-Control-Allow-Origin': 'http://localhost:4200' } });
  }

  changeRamp(ramp : Ramp)
  {
    return this.http.post(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.rampController + '/changeRamp',ramp,{ 'headers': { 'Access-Control-Allow-Origin': 'http://localhost:4200' } });
  }

  deleteRamp(id : string | undefined)
  {
    return this.http.delete(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.rampController + '/removeRamp?rampId=' + id,{ 'headers': { 'Access-Control-Allow-Origin': 'http://localhost:4200' } });
  }
  private handle(error: any) {
    return of (error.message);
  }
}
