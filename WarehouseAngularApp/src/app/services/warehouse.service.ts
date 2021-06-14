import { Host, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Observable, pipe, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { HostInfo } from '../services/hostInfo';
import { Warehouse } from '../Models/Warehouse';


@Injectable({
  providedIn: 'root',
})
export class WarehouseService {
  isLoggedin = false;
  hostInfo : HostInfo = new HostInfo();


  constructor(private http: HttpClient) { }

  getWarehouses(): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.warehouseController + '/getAllWarehouses');
  }
  addWarehouses(warehouse : Warehouse): Observable<any> {
    return this.http.post(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.warehouseController + '/AddWarehouses',warehouse);
  }
  changeWarehouses(warehouse : Warehouse): Observable<any> {
    return this.http.post(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.warehouseController + '/ChangeWarehouses',warehouse);
  }

  logout(): void {
    this.isLoggedin = false;
    localStorage.removeItem('jwt');
    localStorage.removeItem('role');
  }

  private handle(error: any) {
    return of (error.message);
  }
}
