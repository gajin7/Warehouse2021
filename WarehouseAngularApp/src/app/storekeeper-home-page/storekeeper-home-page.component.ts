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
import { Validators, FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { Result } from '../Models/Result';
import { ReportsService } from '../services/reports.service';

@Component({
  selector: 'app-storekeeper-home-page',
  templateUrl: './storekeeper-home-page.component.html',
  styleUrls: ['./storekeeper-home-page.component.css'],
})


export class StorekeeperHomePageComponent implements OnInit {

  NewItem = this.fb.group({
    Name: ['', Validators.required],
    Quantity: ['', Validators.required],
    Amount: ['', Validators.required],
    Type: ['',Validators.required],
    Warehouse: ['', Validators.required],
    ShelfId: ['',Validators.required]
  });

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
  company : string | undefined = '';
  itemTypes : any;
  companies : any;
  shelves: any;
  reports : any;
  columnsToDisplay = ['date', 'type'];
  
 
  constructor(private warehouseService : WarehouseService,private shelvesService : ShelvesService, private authService: 
    AuthService,public router: Router, private itemsService : ItemsService, public receiptService : ReceiptService,
    private fb: FormBuilder, private reportService : ReportsService) { 
    this.colums = 0;
    this.colArray = [];
    this.warehouseNames = [];
    this.OrderDataSource = [];
    this.allItems = [];
    this.itemTypes = [];
    this.companies = [];
    this.shelves = [];
    this.reports = [];
    
  }

  ngOnInit(): void {
    this.getWarehouses();
    this.getDateAndTime();
    this.getItemTypes();
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
      this.OrderDataSource = [];
      this.total = 0;
      this.company = '';
      });
    }
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

  ShowNewItem() {
    const nId = "newItemSectionHidden"; 
    var item = document.getElementById(nId)!;
    item.style.visibility = 'visible';

    var btn = document.getElementById('newItemBtn')!;
    btn.style.visibility = 'hidden';

    const tableid = "tableAllItems";
    var table = document.getElementById(tableid)!;
    table.style.width = "800px";
    table.style.minWidth = "800px";
    table.style.maxWidth = "800px";
    
  }

 HideNewItem() {
    const nId = "newItemSectionHidden"; 
    var item = document.getElementById(nId)!;
    item.style.visibility = 'hidden';

    var btn = document.getElementById('newItemBtn')!;
    btn.style.visibility = 'visible';

    const tableid = "tableAllItems";
    var table = document.getElementById(tableid)!;
    table.style.width = "1400px";
    table.style.minWidth = "1400px";
    table.style.maxWidth = "1400px";
    
  }

  getItemTypes()
  {
    this.itemsService.getItemTypes().subscribe((data) => {
      this.itemTypes = data;
      });
  }

  getCompanies()
  {
    this.itemsService.getCompanies().subscribe((data) => {
      this.companies = data;
      console.log(this.companies,"companies");
      });
  }

  SaveItem()
  {
    
    this.itemsService.addItem(this.NewItem.value).subscribe((data) =>{
      var result = data as Result;
      window.alert(result.Message);
      
      if(result.Success)
      {
        this.itemsService.getAllItems().subscribe();
        for(var name in this.NewItem.controls) {
            
          (<FormControl>this.NewItem.controls[name]).setValue('');
          this.NewItem.controls[name].setErrors(null);
        }
      }
    });

  }

  getShelvesForNewItem() : void
  {
    this.shelvesService.getShelves(this.NewItem.value.Warehouse).subscribe((data) => {
      this.shelves = data;
    });
  }
  takeCompany(event: any)
  {
    this.company = event.target.value;
  }

  getReports()
  {
    this.reportService.getReports('').subscribe((data) => {
      this.reports = data;
      console.log(data,"reports");
    });
  }

  SortByDate()
  {
    console.log("SortByDate");
  }

  SortByType()
  {
    console.log("SortByType");
  }
  
}