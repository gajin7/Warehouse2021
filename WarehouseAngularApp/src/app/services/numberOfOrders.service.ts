import { Host, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

import { Observable, pipe, of } from 'rxjs';
import { HostInfo } from './hostInfo';
import { NewItem } from '../Models/NewItem';
import { Company } from '../Models/Company';


@Injectable({
  providedIn: 'root',
})
export class NumberOfOrdersService {
  isLoggedin = false;
  hostInfo : HostInfo = new HostInfo();


  constructor(private http: HttpClient) { }

  getNumberOfOrdersToday()
  {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.numberOfOrdersController + '/getOrdersNumberForToday',{ 'headers': { 'Access-Control-Allow-Origin': 'http://localhost:4200' } });
  }

  getNumberOfOrdersAllTime()
  {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.numberOfOrdersController + '/getOrdersNumberAllTime',{ 'headers': { 'Access-Control-Allow-Origin': 'http://localhost:4200' } });
  }

   private handle(error: any) {
    return of (error.message);
  }
}
