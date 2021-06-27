import { Component, OnInit } from '@angular/core';
import { Item } from '../Models/Item';
import { Warehouse } from '../Models/Warehouse';
import { ShelvesService } from '../services/shelves.service';
import { WarehouseService } from '../services/warehouse.service';
import { AuthService } from '../auth/auth.service';
import { Router } from '@angular/router';
import { ItemsService } from '../services/items.service';
import { ReceiptService } from '../services/receipt.service';
import { Validators, FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { Result } from '../Models/Result';
import { ReportsService } from '../services/reports.service';
import { LoadService } from '../services/load.service';
import { CompaniesService } from '../services/companies.service';


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
  OrderDataSource : Array<Item>;
  expandedElement: Item | null | undefined;
  active = 'Shelves';
  date : Date | undefined;
  email : string = sessionStorage.email;
  warehouseNames : Array<string | undefined>;
  curentWarehouse : string | undefined;
  total :number = 0;
  keyWord : string = '';
  quantity : number = 0;
  company : string | undefined = '';
  companies : any;
  shelves: any;

  
  constructor(private warehouseService : WarehouseService,private shelvesService : ShelvesService, private itemsService : ItemsService, public receiptService : ReceiptService,private companyService : CompaniesService) { 
    this.colums = 0;
    this.colArray = [];
    this.warehouseNames = [];
    this.OrderDataSource = [];
    this.companies = [];
    this.shelves = [];
    
  }

  ngOnInit(): void {
    this.getWarehouses();
    this.getDateAndTime();
    this.getCompanies();
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
    let item = this.dataSource[i].Items[j];
    var found = false;
    var index = null;
    for(var k = 0; k < this.OrderDataSource.length; k++) {
    if (this.OrderDataSource[k].Id === item.Id) {
        found = true;
        index = k;
        break;
    }
  }
    if(found && index !== null)
    {
      this.OrderDataSource[index].Quantity +=1;
      var amount : number = this.OrderDataSource[index].Amount;
      this.total += amount;
    }
    else
    {
      var newItem = Object.assign({},item);
      console.log(newItem,"newItem")
      newItem.Quantity = 1;
      this.OrderDataSource.push(newItem);
      var amount : number = newItem.Amount;
      this.total += amount;
    }

  }

  increaseQuantity(id : string)
  {
    var that = this;
    var index = 0;
    for(var k = 0; k < this.OrderDataSource.length; k++) {
      if (this.OrderDataSource[k].Id === id)  {
         index = k;
         this.itemsService.getQuantityById(id).subscribe((data) => {
          that.quantity = data.quantity;

          console.log( that.quantity);

         if(that.quantity >= (that.OrderDataSource[index].Quantity +1))
         {
           console.log("ovde");
            that.OrderDataSource[index].Quantity +=1;
            var amount : number = that.OrderDataSource[index].Amount;
            that.total += amount;
         }
    
        });
        
      }
    
    }
  }

  decreseQuantity(id : string)
  {
    for(var k = 0; k < this.OrderDataSource.length; k++) {
      if (this.OrderDataSource[k].Id === id) {
        if(this.OrderDataSource[k].Quantity - 1 > 0)
        {
          this.OrderDataSource[k].Quantity -=1;
          var amount : number = this.OrderDataSource[k].Amount;
          this.total -= amount;
          break;
        }
            
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
        var quantity : number = this.OrderDataSource[k].Quantity;
        var amount : number = this.OrderDataSource[k].Amount * quantity;
        this.total -= amount;
        this.OrderDataSource.splice(k, 1);  
      }
    
    }
  }

  save()
  {
    if(this.company == '')
    {
      window.alert("Please select company");
    }
    else if(this.OrderDataSource.length == 0)
    {
      window.alert("Please add items first");
    }
    else
    {
      this.OrderDataSource.forEach(element => {
        element.Warehouse = this.curentWarehouse!;
      });
      this.receiptService.createReceipt(this.OrderDataSource,this.company,this.curentWarehouse).subscribe((data) => {
        window.alert(data.Message);
        if(data.Success)
        {
          this.OrderDataSource = [];
          this.total = 0;
        }
        this.getShelves(this.curentWarehouse);      
      });
    }
  }

  search()
  {
    this.shelvesService.getShelvesWithItemsSearch(this.curentWarehouse,this.keyWord).subscribe((data) => {
      this.dataSource = data;
    });
  }

  clearSearch()
  {
    this.keyWord = '';
    this.shelvesService.getShelvesWithItems(this.curentWarehouse).subscribe((data) => {
      this.dataSource = data;
    });
  }

  updateSearch(e : any) {
    this.keyWord = e.target.value; 
  }
  getCompanies()
  {
    this.companyService.getCompanies().subscribe((data) => {
      this.companies = data;
      });
  }

  takeCompany(event: any)
  {
    this.company = event.target.value;
  }
}