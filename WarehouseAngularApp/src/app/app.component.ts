import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'WarehouseAngularApp';
  date : Date | undefined;


  ngOnInit(): void {
    this.getDateAndTime();

  }
  constructor(public authService: AuthService,public router: Router) { 
  }

  getRole () { 
    return localStorage.role
  }

  getEmail () { 
    return localStorage.email
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

