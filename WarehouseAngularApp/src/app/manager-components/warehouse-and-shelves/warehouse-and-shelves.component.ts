import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { Result } from 'src/app/Models/Result';
import { User } from 'src/app/Models/User';
import { Warehouse } from 'src/app/Models/Warehouse';
import { ShelvesService } from 'src/app/services/shelves.service';
import { UserService } from 'src/app/services/user.service';
import { WarehouseService } from 'src/app/services/warehouse.service';

@Component({
  selector: 'app-warehouse-and-shelves',
  templateUrl: './warehouse-and-shelves.component.html',
  styleUrls: ['./warehouse-and-shelves.component.css']
})
export class WarehouseAndShelvesComponent implements OnInit {
  employees = Array<User>();
  WarehouseSubmitType = "";
  shelves: any;
  warehouseNames : Array<string | undefined>;
  warehouses: Array<Warehouse>;
  dataSource : any;

  NewWarehouse = this.fb.group({
    WarehouseId: ['', Validators.required],
    Address: ['', Validators.required],
    StorekeeperName: ['', Validators.required],
    StorekeeperEmail: ['', Validators.required],
    StorekeeperId: ['', Validators.required]
  });

  NewShelf = this.fb.group({
    WarehouseId: ['', Validators.required],
    Name: ['', Validators.required],
  });
  constructor(private userService: UserService,private fb: FormBuilder, private shelvesService : ShelvesService, private warehouseService : WarehouseService)
  {
    this.warehouseNames = [];
    this.warehouses = [];
  }
  
  ngOnInit(): void {
    this.getAllWarehouses();
    this.getWarehouses();
    this.GetEmployees();
  }

  GetEmployees()
  {
    this.userService.getAllEmployees().subscribe((data)=> {
      this.employees = data;
    });
  }

  getWarehouses() : void {
    this.warehouseService.getWarehouses().subscribe((data) => {
      let array = data as Array<Warehouse>;
      this.warehouseNames = [];
      array.forEach(element => {
        this.warehouseNames.push(element.WarehouseId);
      });
    });
  }

  getAllWarehouses() : void {
    this.warehouseService.getWarehouses().subscribe((data) => {
      let array = data as Array<Warehouse>;
      this.warehouses = [];
      array.forEach(element => {
        this.warehouses.push(element);
      });
    });
  }

  ShowNewWarehouseDrawer()
  {
    const nId = "newWarehouseSectionHidden"; 
    var item = document.getElementById(nId)!;
    item.style.visibility = 'visible';

    var btn = document.getElementById('thNewWarehouse')!;
    btn.style.visibility = 'hidden';

    if(this.WarehouseSubmitType === "change")
    {
      var warehouseName = document.getElementById("warehouseName")!;
      warehouseName.setAttribute('readonly', 'readonly');

      const nId = "tableAllItemsShelfs"; 
      var item = document.getElementById(nId)!;
      item.style.visibility = 'visible';
      
    }
  }

  HideNewWarehouseDrawer()
  {
    const nId = "newWarehouseSectionHidden"; 
    var item = document.getElementById(nId)!;
    item.style.visibility = 'hidden';

    const nId2 = "tableAllItemsShelfs"; 
    var item = document.getElementById(nId2)!;
    item.style.visibility = 'hidden';

    var btn = document.getElementById('thNewWarehouse')!;
    btn.style.visibility = 'visible';

    for(var name in this.NewWarehouse.controls) {
            
        (<FormControl>this.NewWarehouse.controls[name]).setValue('');
        this.NewWarehouse.controls[name].setErrors(null);
    }

    var warehouseName = document.getElementById("warehouseName")!;
    warehouseName.removeAttribute('readonly');

     this.WarehouseSubmitType = '';
  }

  ChangeWarehouse(item : Warehouse)
  {
    this.NewWarehouse.setValue(item);
    this.NewWarehouse.value["StorekeeperEmail"] = item.StorekeeperEmail;
    console.log( this.NewWarehouse.value);
    this.WarehouseSubmitType = "change";
    this.ShowNewWarehouseDrawer();
    this.getShelves(item.WarehouseId);
  }

  SaveWarehouse()
  {
    if(this.WarehouseSubmitType === "change")
    {
      this.warehouseService.changeWarehouses(this.NewWarehouse.value).subscribe((data) =>{
        var result = data as Result;
        window.alert(result.Message);
        this.HideNewWarehouseDrawer();
        this.getAllWarehouses();
        
      });
    }
    else
    {
    this.warehouseService.addWarehouses(this.NewWarehouse.value).subscribe((data) =>{
      var result = data as Result;
      window.alert(result.Message);
      
      if(result.Success)
      {
        this.getAllWarehouses();
        for(var name in this.NewWarehouse.controls) {
            
          (<FormControl>this.NewWarehouse.controls[name]).setValue('');
          this.NewWarehouse.controls[name].setErrors(null);
        }
      }
      this.HideNewWarehouseDrawer();
    });
    }
  }

  getShelves(warehouseId: string | undefined) : void
  {
    
    this.shelvesService.getShelvesWithItems(warehouseId).subscribe((data) => {
      this.dataSource = data;
    });
  }

  ShowNewShelfForm()
  {
    const nId = "newShelfForm"; 
    var item = document.getElementById(nId)!;
    item.style.visibility = 'visible';
    this.NewShelf.controls['WarehouseId'].setValue(this.NewWarehouse.value['WarehouseId']);
  }

  HideNewShelfForm()
  {
    const nId = "newShelfForm"; 
    var item = document.getElementById(nId)!;
    item.style.visibility = 'hidden';
  }

  SaveNewShelf()
  {
    this.shelvesService.addShelves(this.NewShelf.value['WarehouseId'],this.NewShelf.value['Name']).subscribe(data =>
      {
        var result = data as Result;
        window.alert(result.Message);

        this.NewShelf.controls['Name'].setValue('');
        this.getShelves(this.NewWarehouse.value['WarehouseId']);
      });
  }

  RemoveShelf(shelfId : string)
  {
    this.shelvesService.removeShelves(this.NewWarehouse.value['WarehouseId'],shelfId).subscribe(data =>
      {
        var result = data as Result;
        window.alert(result.Message);

        this.getShelves(this.NewWarehouse.value['WarehouseId']);
      });
  }

}
