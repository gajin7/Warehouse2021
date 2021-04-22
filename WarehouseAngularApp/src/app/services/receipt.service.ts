import { Host, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Observable, pipe, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { HostInfo } from '../services/hostInfo';
import { Item } from '../Models/Item';


@Injectable({
  providedIn: 'root',
})


export class ReceiptService {
  isLoggedin = false;
  hostInfo : HostInfo = new HostInfo();

 
  constructor(private http: HttpClient) { }

  createReceipt(items: any, company : string): Observable<any> {
    return this.http.post(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.receiptController + "/createReceipt", `Items=`+ items +`&Company=`+company,this.hostInfo.httpOptionsJson);
  }
  private handle(error: any) {
    return of (error.message);
  }
}
