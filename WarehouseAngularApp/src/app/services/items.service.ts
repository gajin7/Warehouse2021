import { Host, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

import { Observable, pipe, of } from 'rxjs';
import { HostInfo } from '../services/hostInfo';
import { NewItem } from '../Models/NewItem';


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

  getItemTypes()
  {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.itemsController + '/getItemTypes');
  }

  addItem(item : NewItem)
  {
    return this.http.post(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.itemsController + '/addItem',item);
  }

  change(item : NewItem)
  {
    return this.http.post(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.itemsController + '/changeItem',item);
  }

  remove(id : string)
  {
    return this.http.delete(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.itemsController + '/removeItem?id='+id);
  }

  private handle(error: any) {
    return of (error.message);
  }
}
