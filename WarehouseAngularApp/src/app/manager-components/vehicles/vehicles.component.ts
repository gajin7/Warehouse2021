import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { Result } from 'src/app/Models/Result';
import { Vehicle } from 'src/app/Models/Vehicle';
import { VehicleService } from 'src/app/services/vehicle.service';

@Component({
  selector: 'app-vehicles',
  templateUrl: './vehicles.component.html',
  styleUrls: ['./vehicles.component.css']
})
export class VehiclesComponent implements OnInit {

  VehicleSubmitType = "";
  allVehicles : Array<Vehicle>;

  NewVehicle = this.fb.group({
    Registration: ['', Validators.required],
    Brand: ['', Validators.required],
    Type: ['', Validators.required],
    LoadCapacity: ['', Validators.required],
    ProductionYear: ['', Validators.required],
    Free: ['', Validators.required],
    Mileage: ['', Validators.required],
    DriverId: ['', Validators.required],
    DriverName: ['', Validators.required],
  });
  constructor(private vehicleService : VehicleService,private fb: FormBuilder) 
  {
    this.allVehicles = [];
  }

  ngOnInit(): void {
    this.getAllVehicles();
  }

  getAllVehicles() : void {
    this.vehicleService.getAllVehicles().subscribe((data) => {
      let array = data as Array<Vehicle>;
      this.allVehicles = [];
      array.forEach(element => {
        this.allVehicles.push(element);
      });
    });
  }
  
  ShowNewVehicle() {
    const nId = "newVehicleSectionHidden"; 
    var item = document.getElementById(nId)!;
    item.style.visibility = 'visible';
  
    var btn = document.getElementById('newVehicleBtn')!;
    btn.style.visibility = 'hidden';
  
    const tableid = "tableAllVehicles";
    var table = document.getElementById(tableid)!;
    table.style.width = "50%";
    table.style.minWidth = "50%";
    table.style.maxWidth = "50%";
    
    if(this.VehicleSubmitType === "change")
    {
      var id = document.getElementById("registration")!;
      id.setAttribute('readonly', 'readonly');
    }
  }
  
  HideNewVehicle() {
    const nId = "newVehicleSectionHidden"; 
    var item = document.getElementById(nId)!;
    item.style.visibility = 'hidden';
  
    var btn = document.getElementById('newVehicleBtn')!;
    btn.style.visibility = 'visible';
  
    var email = document.getElementById("registration")!;
    email.removeAttribute('readonly');
  
  
    const tableid = "tableAllVehicles";
    var table = document.getElementById(tableid)!;
    table.style.width = "100%";
    table.style.minWidth = "100%";
    table.style.maxWidth = "100%";
  
      for(var name in this.NewVehicle.controls) {
            
        (<FormControl>this.NewVehicle.controls[name]).setValue('');
        this.NewVehicle.controls[name].setErrors(null);
      }
  
      this.VehicleSubmitType = '';
    
  }
  
  ChangeVehicle(vehicle : Vehicle)
    {
      this.NewVehicle.setValue(vehicle);
      this.VehicleSubmitType = "change";
      this.ShowNewVehicle();
    }
  
    SaveVehicle()
    {
      if(this.VehicleSubmitType === "change")
      {
        this.vehicleService.changeVehicle(this.NewVehicle.value).subscribe((data: Result) =>{
          var result = data as Result;
          window.alert(result.Message);
          this.HideNewVehicle();
          this.getAllVehicles();
          
        });
      }
      else
      {
        this.NewVehicle.controls['Free'].setValue('true');
      this.vehicleService.addVehicle(this.NewVehicle.value).subscribe((data) =>{
        var result = data as Result;
        window.alert(result.Message);
        
        if(result.Success)
        {
          this.getAllVehicles();
          for(var name in this.NewVehicle.controls) {
              
            (<FormControl>this.NewVehicle.controls[name]).setValue('');
            this.NewVehicle.controls[name].setErrors(null);
          }
          this.HideNewVehicle();
        }
      });
      
      }
    }
  
    RemoveVehicle(registration : string | undefined)
    {
      if(window.confirm("Are you sure want to delete " + registration + " vehicle?"))
      {
        this.vehicleService.removeVehicle(registration).subscribe((data) =>{
          var result = data as Result;
          window.alert(result.Message);
          this.getAllVehicles();
        });
      }
    }
  
    freeVehicle(registration: string | undefined)
    {
      if(window.confirm("Are you sure want to free " + registration + " vehicle?"))
      {
           this.vehicleService.freeVehicle(registration).subscribe((data) => {
            window.alert(data.Message);
            this.getAllVehicles();
           });
        }
    }
  

}
