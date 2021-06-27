import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { Item } from 'src/app/Models/Item';
import { Result } from 'src/app/Models/Result';
import { Warehouse } from 'src/app/Models/Warehouse';
import { ItemsService } from 'src/app/services/items.service';
import { RightsService } from 'src/app/services/rights.service';
import { ShelvesService } from 'src/app/services/shelves.service';
import { WarehouseService } from 'src/app/services/warehouse.service';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.css']
})
export class ItemsComponent implements OnInit {

  shelves: any;
  allItems : any;
  allItemsKeyWord : string = '';
  ItemSubmitType = "";
  itemTypes: any;
  warehouseNames : Array<string | undefined>;
  NewItem = this.fb.group({
    Id: ['', Validators.required],
    Name: ['', Validators.required],
    Quantity: ['', Validators.required],
    Amount: ['', Validators.required],
    Type: ['',Validators.required],
    Warehouse: ['', Validators.required],
    ShelfId: ['',Validators.required]
  });



  constructor(private fb: FormBuilder, private itemsService : ItemsService, private shelvesService : ShelvesService, 
    private warehouseService : WarehouseService, private rightsService : RightsService)
  {
    this.warehouseNames = [];
  }

  ngOnInit(): void {
    this.getAllItems();
    this.getItemTypes();
    this.getWarehouseNames();
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


  getShelvesForNewItem() : void
  {
    this.shelvesService.getShelves(this.NewItem.value.Warehouse).subscribe((data) => {
      this.shelves = data;
    });
  }

  getAllItems()
  {
    this.itemsService.getAllItems().subscribe((data) => {
      this.allItems = data;
    });
  }

  allItemsSearch()
  {
    this.itemsService.getAlltemsSearch('',this.allItemsKeyWord).subscribe((data) => {
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
      this.itemsService.getAlltemsSearch('',this.allItemsKeyWord).subscribe((data) => {
      this.allItems = data;
      });
    }
  }

  SaveItem()
  {
    if(this.ItemSubmitType === "change")
    {
      this.itemsService.change(this.NewItem.value).subscribe((data) =>{
        var result = data as Result;
        window.alert(result.Message);
        this.HideNewItem();
        this.getAllItems();
        
      });
    }
    else
    {
    this.itemsService.addItem(this.NewItem.value).subscribe((data) =>{
      var result = data as Result;
      window.alert(result.Message);
      
      if(result.Success)
      {
        this.getAllItems();
        for(var name in this.NewItem.controls) {
            
          (<FormControl>this.NewItem.controls[name]).setValue('');
          this.NewItem.controls[name].setErrors(null);
        }
      }
    });
    }
  }

  getItemTypes()
  {
    this.itemsService.getItemTypes().subscribe((data) => {
      this.itemTypes = data;
      });
  }

  ChangeItem(item : Item)
  {
    if(this.rightsService.CheckIfUserHasAdminRights())
    {
    this.NewItem.setValue(item);
    this.ItemSubmitType = "change";
    this.ShowNewItem();
    }
  }

  RemoveItem(id :string, name:string)
  {
    if(this.rightsService.CheckIfUserHasAdminRights())
    {
    if(window.confirm("Are you sure want to delete " + name + " item? All reports related to this item will be deleted and that can couse problems in the system"))
    {
      this.itemsService.remove(id.toString()).subscribe((data) =>{
        var result = data as Result;
        window.alert(result.Message);
        this.getAllItems();
      });
    }
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
    table.style.width = "50%";
    table.style.minWidth = "50%";
    table.style.maxWidth = "50%";
    
    if(this.ItemSubmitType === "change")
    {
      this.getShelvesForNewItem();
    }
  }

  HideNewItem() {
    const nId = "newItemSectionHidden"; 
    var item = document.getElementById(nId)!;
    item.style.visibility = 'hidden';

    var btn = document.getElementById('newItemBtn')!;
    btn.style.visibility = 'visible';

    const tableid = "tableAllItems";
    var table = document.getElementById(tableid)!;
    table.style.width = "95%";
    table.style.minWidth = "95%";
    table.style.maxWidth = "95%";

      for(var name in this.NewItem.controls) {
            
        (<FormControl>this.NewItem.controls[name]).setValue('');
        this.NewItem.controls[name].setErrors(null);
      }

      this.ItemSubmitType = '';
    
  }

}
