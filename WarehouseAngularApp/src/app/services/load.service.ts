import { Host, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

import { Observable, pipe, of } from 'rxjs';
import { HostInfo } from '../services/hostInfo';
import { NewItem } from '../Models/NewItem';


@Injectable({
  providedIn: 'root',
})
export class LoadService {

  hostInfo : HostInfo = new HostInfo();

  constructor(private http: HttpClient) { }

  getLoads(): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.loadController + '/getUnloadedLoads');
  }

  private handle(error: any) {
    return of (error.message);
  }
}
