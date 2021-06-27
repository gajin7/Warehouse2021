import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root',
  })
  export class RightsService {
   
    constructor() { }
  
      CheckIfUserHasAdminRights(): boolean {
          if(sessionStorage.role == 'admin')
          {
            return true;
          }
          else
          {
            return false;
          }
        }
      }