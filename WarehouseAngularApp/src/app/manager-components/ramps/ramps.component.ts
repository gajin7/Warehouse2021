import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { Ramp } from 'src/app/Models/Ramp';
import { Result } from 'src/app/Models/Result';
import { Warehouse } from 'src/app/Models/Warehouse';
import { RampService } from 'src/app/services/ramps.service';
import { WarehouseService } from 'src/app/services/warehouse.service';

@Component({
  selector: 'app-ramps',
  templateUrl: './ramps.component.html',
  styleUrls: ['./ramps.component.css']
})
export class RampsComponent implements OnInit {

  RampSubmitType = "";
  rampWarehouse = '';
  ramps: Array<Ramp>;
  warehouseNames : Array<string | undefined>;
  Ramp = this.fb.group({
    Id: ['', Validators.required],
    Free: ['', Validators.required],
    Load: ['', Validators.required],
    WarehouseId: ['', Validators.required],
  });
  constructor(private rampService : RampService,private fb: FormBuilder, private warehouseService : WarehouseService)
  {
    this.ramps = [];
    this.warehouseNames = [];
  }

  ngOnInit(): void {
    this.getAllRamps();
    this.getWarehouseNames();
    this.getWarehouses();
  }

  getWarehouseNames() : void {
    this.warehouseService.getWarehouses().subscribe((data) => {
      let array = data as Array<Warehouse>;
      this.warehouseNames = [];
      array.forEach(element => {
        this.warehouseNames.push(element.WarehouseId);
      });
    });
  }

  changeRampWarehouse (event: any) {
    this.rampWarehouse = event.target.value;
    this.getAllRamps();
  }

  getAllRamps() : void {
    this.rampService.getRamps(this.rampWarehouse).subscribe((data) => {
      let array = data as Array<Ramp>;
      this.ramps = [];
      array.forEach(element => {
        this.ramps.push(element);
      });
    });
  }

  SaveRamp()
  {
    this.Ramp.controls['WarehouseId'].setValue(this.rampWarehouse);
    if(this.RampSubmitType === "change")
    {
      this.rampService.changeRamp(this.Ramp.value).subscribe((data) =>{
        var result = data as Result;
        window.alert(result.Message);
        this.HideNewRamp();
        this.getAllRamps();
        
      });
    }
    else
    {
    this.rampService.addRamp(this.Ramp.value).subscribe((data) =>{
      var result = data as Result;
      window.alert(result.Message);
      
      if(result.Success)
      {
        this.getAllRamps();
        for(var name in this.Ramp.controls) {
            
          (<FormControl>this.Ramp.controls[name]).setValue('');
          this.Ramp.controls[name].setErrors(null);
        }
        this.HideNewRamp();
      }
    });
    
    }
  }

  ShowNewRamp() {
    this.Ramp.controls['WarehouseId'].setValue(this.rampWarehouse);
    const nId = "newRampSectionHidden"; 
    var item = document.getElementById(nId)!;
    item.style.visibility = 'visible';

    var btn = document.getElementById('newRampBtn')!;
    btn.style.visibility = 'hidden';

    const tableid = "tableAllRamps";
    var table = document.getElementById(tableid)!;
    table.style.width = "50%";
    table.style.minWidth = "50%";
    table.style.maxWidth = "50%";
    
    if(this.RampSubmitType === "change")
    {
      var id = document.getElementById("rampId")!;
      id.setAttribute('readonly', 'readonly');
    }
  }

  HideNewRamp() {
    const nId = "newRampSectionHidden"; 
    var item = document.getElementById(nId)!;
    item.style.visibility = 'hidden';

    var btn = document.getElementById('newRampBtn')!;
    btn.style.visibility = 'visible';

    const tableid = "tableAllRamps";
    var table = document.getElementById(tableid)!;
    table.style.width = "100%";
    table.style.minWidth = "100%";
    table.style.maxWidth = "100%";

      for(var name in this.Ramp.controls) {
            
        (<FormControl>this.Ramp.controls[name]).setValue('');
        this.Ramp.controls[name].setErrors(null);
      }

      this.RampSubmitType = '';

    var id = document.getElementById("rampId")!;
    id.removeAttribute('readonly');
    
  }

  ChangeRamp(ramp : Ramp)
  {
    this.Ramp.setValue(ramp);
    this.RampSubmitType = "change";
    this.ShowNewRamp();
  }

  RemoveRamp(id : string| undefined)
  {
    if(window.confirm('Are you sure that you want to remove ramp ' + id + '?'))
    {
      this.rampService.deleteRamp(id).subscribe(data => {
        var result = data as Result;
          window.alert(result.Message);
         this.getAllRamps();
      });
    }
  }

  getWarehouses() : void {
    this.warehouseService.getWarehouses().subscribe((data) => {
      let array = data as Array<Warehouse>;
      this.rampWarehouse = array[0].WarehouseId!;
    });
  }

}
