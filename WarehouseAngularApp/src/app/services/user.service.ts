import { Host, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

import { Observable, pipe, of } from 'rxjs';
import { HostInfo } from './hostInfo';
import { NewItem } from '../Models/NewItem';
import { User } from '../Models/User';
import { ChangePassword } from '../Models/ChangePassword';


@Injectable({
  providedIn: 'root',
})
export class UserService {
  isLoggedin = false;
  hostInfo : HostInfo = new HostInfo();


  constructor(private http: HttpClient) { }

  getAllEmployees(): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.userController + '/GetAllUsers');
  }

  getEmployeeTypes(): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.userController + '/GetEmployeeTypes');
  }

  register(user: User): Observable<any> {
    return this.http.post(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.userController + '/register',user);
  }
  change(user: User): Observable<any> {
    return this.http.post(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.userController + '/ChangeEmployeeData',user);
  }
  remove(email: string): Observable<any> {
    return this.http.delete(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.userController + '/DeleteEmployee?email=' +email);
  }

  search(keyWod: string): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.userController + '/SearchEmployees?keyWord=' +keyWod);
  }

  getProfile(email: string): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.userController + '/GetUserInfo?email=' +email,{ 'headers': { 'Access-Control-Allow-Origin': 'http://localhost:4200' } });
  }

  changePassword(changePassword: ChangePassword): Observable<any> {
    return this.http.post(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.userController + '/ChangePassword',changePassword);
  }
}
