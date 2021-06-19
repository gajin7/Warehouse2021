import { Host, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

import { Observable, pipe, of } from 'rxjs';
import { HostInfo } from '../services/hostInfo';
import { NewItem } from '../Models/NewItem';
import { Vehicle } from '../Models/Vehicle';


@Injectable({
  providedIn: 'root',
})
export class VehicleService {

  hostInfo : HostInfo = new HostInfo();

  constructor(private http: HttpClient) { }

  getDriversVehicle(): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.vehicleController + '/getDriversVehicle?driversEmail='+localStorage.email);
  }

  getFreeVehicles(): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.vehicleController + '/getFreeVehicles');
  }

  getAllVehicles(): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.vehicleController + '/getAllVehicles');
  }

  giveVehicleToDriver(registration : string |  undefined): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.vehicleController + '/giveVehicleToDriver?registration='+registration +'&driversEmail='+localStorage.email);
  }

  freeVehicle(registration : string |  undefined): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.vehicleController + '/freeVehicle?registration='+registration);
  }

  addVehicle(vehicle: Vehicle): Observable<any> {
    return this.http.post(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.vehicleController + '/addVehicle',vehicle);
  }

  changeVehicle(vehicle: Vehicle): Observable<any> {
    return this.http.post(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.vehicleController + '/changeVehicle',vehicle);
  }

  removeVehicle(registration: string | undefined): Observable<any> {
    return this.http.delete(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.vehicleController + '/removeVehicle?registration=' +registration);
  }


  
  private handle(error: any) {
    return of (error.message);
  }
}
