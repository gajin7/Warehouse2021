import { Host, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Observable, pipe, of } from 'rxjs';
import { HostInfo } from '../services/hostInfo';


@Injectable({
  providedIn: 'root',
})
export class ShelvesService {
  isLoggedin = false;
  hostInfo : HostInfo = new HostInfo();


  constructor(private http: HttpClient) { }

  getShelvesWithItems(warehouseId : string |  undefined): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.shelvesController + '/getShelvesInWarehouse?warehouseId='+warehouseId);
  }

  getShelves(warehouseId : string |  undefined): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.shelvesController + '/getShelves?warehouseId='+warehouseId);
  }

  getShelvesWithItemsSearch(warehouseId : string |  undefined, keyWord: string | undefined): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix +
       this.hostInfo.shelvesController + '/getShelvesInWarehouseSearch?warehouseId='+warehouseId
       +'&keyWord=' + keyWord);
  }
  private handle(error: any) {
    return of (error.message);
  }
}
