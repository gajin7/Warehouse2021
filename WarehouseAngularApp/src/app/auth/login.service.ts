import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { User } from './user';


@Injectable({
  providedIn: 'root',
})
export class LoginService {
  isLoggedin = false;

  baseUrl: string = 'http://localhost:64334';

  constructor(private http: HttpClient) { }

  login(user: User): Observable<any> {
    return this.http.post<any>(this.baseUrl+'/api/user/Login?email='+ user.username+'&password='+user.password,null).pipe(
      map(res => {
        if(res.Message === "Success")
        {
          this.isLoggedin = true;
          console.log('role: ' + res.Role)
     
        localStorage.setItem('role', res.Role)
        }
      }),

      catchError(this.handle)
    );
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