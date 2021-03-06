import { Injectable } from '@angular/core';
import {
  CanActivate, Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanActivateChild,
} from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class LoginGuard implements CanActivate, CanActivateChild {
  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {    
    if (sessionStorage.role === 'admin') {
      this.router.navigate(['manager-home']);
      return false;
    }
    if (sessionStorage.role === 'driver') {
      this.router.navigate(['driver-home']);
      return false;
    }
    if (sessionStorage.role === 'storekeeper') {
      this.router.navigate(['storekeeper-home']);
      return false;
    }
    // not logged in so redirect to login page
    else {
      
      return true;
    }
  }

  

  canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    return this.canActivate(route, state);
  }

}
