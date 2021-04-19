import { Host, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Observable, pipe, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { HostInfo } from '../services/hostInfo';


@Injectable({
  providedIn: 'root',
})
export class ItemsService {
  isLoggedin = false;
  hostInfo : HostInfo = new HostInfo();


  constructor(private http: HttpClient) { }

  getQuantityById(id : string |  undefined): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.itemsController + '/getQuantity?id='+id);
  }

  getAllItems(): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.itemsController + '/getAllItems');
  }

  getAlltemsSearch(warehouseId : string |  undefined, keyWord: string | undefined): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix +
       this.hostInfo.itemsController + '/getAllItemsSearch?warehouseId='+warehouseId
       +'&keyWord=' + keyWord);
  }

  private handle(error: any) {
    return of (error.message);
  }
}
