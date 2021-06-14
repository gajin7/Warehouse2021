import { Host, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Observable, pipe, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { User } from './user';
import { HostInfo } from '../services/hostInfo';


@Injectable({
  providedIn: 'root',
})
export class AuthService {
  isLoggedin = false;
  email = "";
  role = "";
  hostInfo : HostInfo = new HostInfo();

  constructor(private http: HttpClient) { }

  getToken(user: User): Observable<any> {
    return this.http.post<any>(this.hostInfo.defaultHostAddress + this.hostInfo.tokenPath, `username=`+ user.username +`&password=`+ user.password + `&grant_type=password`, { 'headers': { 'Content-type': 'x-www-form-urlencoded' } }).pipe(
      map(res => {
        this.isLoggedin = true;

        let jwt = res.access_token;

        let jwtData = jwt.split('.')[1]
        let decodedJwtJsonData = window.atob(jwtData)
        let decodedJwtData = JSON.parse(decodedJwtJsonData)

        this.role = decodedJwtData.role
        this.email = decodedJwtData.email

        console.log('jwtData: ' + jwtData)
        console.log('decodedJwtJsonData: ' + decodedJwtJsonData)
        console.log('decodedJwtData: ' + decodedJwtData)
        console.log('Role ' + this.role)

        localStorage.setItem('jwt', jwt)
        localStorage.setItem('role', this.role);
        localStorage.setItem('email', this.email);
      }),

      catchError(this.handle)
    );
  }

  resetPassword(email : string)
  {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.userController + '/resetPassword?email='+email);
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
