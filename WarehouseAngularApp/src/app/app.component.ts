import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'WarehouseAngularApp';
  date : Date | undefined;


  ngOnInit(): void {
    this.getDateAndTime();

  }

  async  ngOnDestroy() {
    this.authService.logout();
  }
  constructor(public authService: AuthService,public router: Router) { 
  }

  getRole () { 
    return sessionStorage.role
  }

  getEmail () { 
    return sessionStorage.email
  }

  getDateAndTime() : void
  {
    this.date = new Date();
   }

   logOut()
   {
     this.authService.logout();
     this.router.navigate(['']);
   }

   OpenMyProfile()
  {
    this.router.navigate(['my-profile']);
  }
}

