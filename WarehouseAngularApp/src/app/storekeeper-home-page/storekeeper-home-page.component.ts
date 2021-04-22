import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { Item } from '../Models/Item';
import { Warehouse } from '../Models/Warehouse';
import { ShelvesService } from '../services/shelves.service';
import { WarehouseService } from '../services/warehouse.service';
import { AuthService } from '../auth/auth.service';
import { Router } from '@angular/router';
import { ItemsService } from '../services/items.service';
import { ReceiptService } from '../services/receipt.service';

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
  role : string = localStorage.role;
  email : string = localStorage.email;
  warehouseNames : Array<string | undefined>;
  curentWarehouse : string | undefined;
  total :number = 0;
  message: string = '';
  keyWord : string = '';
  allItemsKeyWord : string = '';
  quantity : number = 0;
  allItems : any;
  newItemSectionHidden: any;
  company : string = "test company";
  
 
  constructor(private warehouseService : WarehouseService,private shelvesService : ShelvesService, private authService: 
    AuthService,public router: Router, private itemsService : ItemsService, public receiptService : ReceiptService) { 
    this.colums = 0;
    this.colArray = [];
    this.warehouseNames = [];
    this.OrderDataSource = [];
    this.allItems = [];
    
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
    let item = this.dataSource[i].Items[j];
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
      var amount : number = this.OrderDataSource[index].Amount;
      this.total += amount;
    }
    else
    {
      item.Quantity = 1;
      this.OrderDataSource.push(item);
      var amount : number = item.Amount;
      this.total += amount;
      this.shelvesService.getShelvesWithItems(this.curentWarehouse).subscribe((data) => {
        this.dataSource = data;
      });
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

  logOut()
  {
    this.authService.logout();
    this.router.navigate(['']);
  }

  save()
  {
    this.receiptService.createReceipt(this.OrderDataSource,this.company).subscribe((data) => {
      console.log(data);
      this.message = "Order saved. Please go to report for more details";
    this.OrderDataSource = [];
    this.total = 0;
    setTimeout(()=> this.message = '',3000); 
    });
    
  }

  closeAlert() {
    this.message = '';
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

  getAllItems()
  {
    this.itemsService.getAllItems().subscribe((data) => {
      this.allItems = data;
    });
  }

  allItemsSearch()
  {
    this.itemsService.getAlltemsSearch(this.curentWarehouse,this.allItemsKeyWord).subscribe((data) => {
      this.allItems = data;
    });
  }

  clearAllItemsSearch()
  {
    this.allItemsKeyWord = '';
    this.itemsService.getAllItems().subscribe((data) => {
      this.allItems = data;
    });
  }

  updateAllItemsSearch(e : any) {
    this.allItemsKeyWord = e.target.value; 
  }

  AllItemsSearchEnter() {
    if(this.allItemsKeyWord.length >=3)
    {
      this.itemsService.getAlltemsSearch(this.curentWarehouse,this.allItemsKeyWord).subscribe((data) => {
      this.allItems = data;
      });
    }
  }

  showHide() {
    const nId = "newItemSectionHidden"; 
    var item = document.getElementById(nId)!;
    item.style.visibility = 'visible';
    
  }
}