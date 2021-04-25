import { Component, OnInit } from '@angular/core';
import { LoadService } from '../services/load.service';
import { VehicleService } from '../services/vehicle.service';

@Component({
  selector: 'app-driver-home-page',
  templateUrl: './driver-home-page.component.html',
  styleUrls: ['./driver-home-page.component.css']
})
export class DriverHomePageComponent implements OnInit {
 vehicle : any;
 vehicles: any
 loads: any;
  constructor(private vehicleService : VehicleService, private loadsService : LoadService) {
    this.vehicle = [];
    this.loads = [];
   }

  ngOnInit(): void {
    this.getDriversVehicle();
    this.getFreeVehicles();
    this.getLoads();
  }

  getDriversVehicle()
  {
    this.vehicleService.getDriversVehicle().subscribe((data)=> {
      this.vehicle = data;
      console.log(this.vehicle,"vehicle");
    })
  }

  getFreeVehicles()
  {
    this.vehicleService.getFreeVehicles().subscribe((data) => {
      this.vehicles = data;
      console.log(this.vehicles,"vehicles");
    })
  }

  takeVehicleByDriver(registration : string)
  {
    this.vehicleService.giveVehicleToDriver(registration).subscribe((data) => {
      this.getDriversVehicle();
    })
  }

  freeVehicle(registration: string)
  {
    this.vehicleService.freeVehicle(registration).subscribe((data) => {
      if(data.Success)
      {
        this.vehicle = null;
        window.alert(data.Message);
        this.getFreeVehicles();
        this.getLoads();
      }
    })
  }

  getLoads()
  {
    this.loadsService.getLoads().subscribe((data) => {
      this.loads = data;
    })
  }

  takeLoad(id: string, rampFree: string)
  {
    console.log(id + ' ' + rampFree,"take load");
  }
   

}
