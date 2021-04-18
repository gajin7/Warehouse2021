import { Host, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Observable, pipe, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
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

  logout(): void {
    this.isLoggedin = false;
    localStorage.removeItem('jwt');
    localStorage.removeItem('role');
  }

  private handle(error: any) {
    return of (error.message);
  }
}
