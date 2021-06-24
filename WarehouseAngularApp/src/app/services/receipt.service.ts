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

  createReceipt(items: Item[], company : string | undefined, warehouse : string | undefined): Observable<any> {
      console.log(company,"company");
    const myPostBody = { Items: items, Company: company,  StorekeeperEmail: sessionStorage.email, WarehouseId : warehouse}
    return this.http.post(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.receiptController + "/createReceipt", myPostBody,this.hostInfo.httpOptionsJson);
  }

  getReceiptFile(receiptid : string | undefined) : Observable<Blob> {   
        return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.receiptController + '/getReceiptPdf?receiptId=' + receiptid, { responseType: 'blob' });
    }
  private handle(error: any) {
    return of (error.message);
  }
}
