import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { Item } from '../Models/Item';
import { Warehouse } from '../Models/Warehouse';
import { ShelvesService } from '../services/shelves.service';
import { WarehouseService } from '../services/warehouse.service';
import { DatePipe } from '@angular/common'
import { ThrowStmt } from '@angular/compiler';


@Component({
  selector: 'app-storekeeper-home-page',
  templateUrl: './storekeeper-home-page.component.html',
  styleUrls: ['./storekeeper-home-page.component.css'],
})
export class StorekeeperHomePageComponent implements OnInit {
  colArray: Array<Warehouse>;
  colums : number;
  warehousesTableShow : boolean = true; 
  dataSource : any;
  OrderDataSource : Array<any>;
  expandedElement: Item | null | undefined;
  active = 'Shelves';
  date : Date | undefined;
  role : string = localStorage.role;
  email : string = localStorage.email;
  warehouseNames : Array<string | undefined>;
  curentWarehouse : string | undefined;
 
  constructor(private warehouseService : WarehouseService,private shelvesService : ShelvesService) { 
    this.colums = 0;
    this.colArray = [];
    this.warehouseNames = [];
    this.OrderDataSource = [];
    
  }

  ngOnInit(): void {
    this.getWarehouses();
    this.getDateAndTime();
  }

  getDateAndTime() : void
  {
    this.date = new Date();
   }

  getWarehouses() : void {
    this.warehouseService.getWarehouses().subscribe((data) => {
      let array = data as Array<Warehouse>;
      this.colums = array.length;
      array.forEach(element => {
        this.colArray.push(element);
        this.warehouseNames.push(element.WarehouseId);
      });
    });
  }

  openWarehouse(warehouseId : string | undefined) : void{
    this.curentWarehouse = warehouseId;
    this.getShelves(warehouseId);
    this.warehousesTableShow = false;
  }

  getShelves(warehouseId: string | undefined) : void
  {
    this.shelvesService.getShelvesWithItems(warehouseId).subscribe((data) => {
      this.dataSource = data;
    });
  }

  changeWarehouse (event: any) {
    this.OrderDataSource = [];
    this.openWarehouse(event.target.value);
  }

  addToOrder(i: number, j :number)
  {
    let dataSourceCopy = [...this.dataSource];
    var item = dataSourceCopy[i].Items[j];
    var found = false;
    var index = null;
    for(var k = 0; k < this.OrderDataSource.length; k++) {
    if (this.OrderDataSource[k].Id == item.Id) {
        found = true;
        index = k;
        break;
    }
  }
    if(found && index !== null)
    {
      this.OrderDataSource[index].Quantity +=1;
    }
    else
    {
      item.Quantity = 1;
      this.OrderDataSource.push(item);
    }

  }

  increaseQuantity(id : string)
  {
    for(var k = 0; k < this.OrderDataSource.length; k++) {
      if (this.OrderDataSource[k].Id === id) {
        this.OrderDataSource[k].Quantity +=1;
          break;
      }
    
    }
  }

  decreseQuantity(id : string)
  {
    for(var k = 0; k < this.OrderDataSource.length; k++) {
      if (this.OrderDataSource[k].Id === id) {
        if(this.OrderDataSource[k].Quantity - 1 > 0)
            this.OrderDataSource[k].Quantity -=1;
        else
        {
          this.removeItem(id);
        }
          break;
      }
    
    }
  }

  removeItem(id : string)
  {
    for(var k = 0; k < this.OrderDataSource.length; k++) {
      if (this.OrderDataSource[k].Id === id) {
          this.OrderDataSource.splice(k, 1);  
      }
    
    }
  }

}