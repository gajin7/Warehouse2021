import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Company } from '../Models/Company';
import { Item } from '../Models/Item';
import { Ramp } from '../Models/Ramp';
import { Result } from '../Models/Result';
import { User } from '../Models/User';
import { Vehicle } from '../Models/Vehicle';
import { Warehouse } from '../Models/Warehouse';
import { CompaniesService } from '../services/companies.service';
import { ItemsService } from '../services/items.service';
import { LoadService } from '../services/load.service';
import { RampService } from '../services/ramps.service';
import { ReceiptService } from '../services/receipt.service';
import { ReportsService } from '../services/reports.service';
import { ShelvesService } from '../services/shelves.service';
import { UserService } from '../services/user.service';
import { VehicleService } from '../services/vehicle.service';
import { WarehouseService } from '../services/warehouse.service';

@Component({
  selector: 'app-manager-home-page',
  templateUrl: './manager-home-page.component.html',
  styleUrls: ['./manager-home-page.component.css']
})
export class ManagerHomePageComponent implements OnInit {

  active = 'Employees';
  employees = Array<User>();
  employeeTypes : any;
  submitType = "";
  ItemSubmitType = "";
  CompanySubmitType = "";
  RampSubmitType = "";
  WarehouseSubmitType = "";
  VehicleSubmitType = "";
  allItems : any;
  allItemsKeyWord : string = '';
  allCompaniesKeyWord : string = '';
  allIUsersKeyWord : string = '';
  shelves: any;
  itemTypes: any;
  warehouseNames : Array<string | undefined>;
  warehouses: Array<Warehouse>;
  dataSource : any;
  companies: Array<Company>
  loads: any;
  loadTypes = ['Unloaded','Loaded','Loading'];
  currentLoadType : string | undefined = 'Unloaded';
  reports : any;
  rampWarehouse = '';
  ramps: Array<Ramp>;
  loadKeyWord : string = '';
  allVehicles : Array<Vehicle>;
  orderDate = 'asc';
  orderType = 'desc';

  NewEmployee = this.fb.group({
    FirstName: ['', Validators.required],
    LastName: ['', Validators.required],
    Email: ['', Validators.required],
    Type: ['',Validators.required],
    Password: ['', Validators.required],
  });

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
  
  NewItem = this.fb.group({
    Id: ['', Validators.required],
    Name: ['', Validators.required],
    Quantity: ['', Validators.required],
    Amount: ['', Validators.required],
    Type: ['',Validators.required],
    Warehouse: ['', Validators.required],
    ShelfId: ['',Validators.required]
  });

  Company = this.fb.group({
    Name: ['', Validators.required],
    PIB: ['', Validators.required],
    Address: ['', Validators.required],
    Deposit: ['',Validators.required],
    AccountNo: ['', Validators.required],
  });

  Ramp = this.fb.group({
    Id: ['', Validators.required],
    Free: ['', Validators.required],
    Load: ['', Validators.required],
    WarehouseId: ['', Validators.required],
  });
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
  constructor(private userService: UserService, private fb: FormBuilder, private itemsService : ItemsService, 
    private shelvesService : ShelvesService, private warehouseService : WarehouseService,public router: Router, 
    private companyService : CompaniesService,private loadService : LoadService,private reportService : ReportsService,
    private rampService : RampService, private receiptService : ReceiptService, private vehicleService : VehicleService) {
    this.warehouseNames = [];
    this.warehouses = [];
    this.companies = [];
    this.reports = [];
    this.ramps = [];
    this.allVehicles = [];
   }

  ngOnInit(): void {
    this.GetEmployees();
    this.GetEmployeeTypes();
    this.getWarehouses();
    this.getItemTypes();
  }

  GetEmployees()
  {
    this.userService.getAllEmployees().subscribe((data)=> {
      this.employees = data;
    });
  }
  ShowNewEmployee() {
    const nId = "newEmployeeSectionHidden"; 
    var item = document.getElementById(nId)!;
    item.style.visibility = 'visible';

    var btn = document.getElementById('newEmployeeBtn')!;
    btn.style.visibility = 'hidden';

    const tableid = "tableAllEmployees";
    var table = document.getElementById(tableid)!;
    table.style.width = "50%";
    table.style.minWidth = "50%";
    table.style.maxWidth = "50%";

    this.GetEmployeeTypes();
    
    if(this.submitType === "change")
    {
      var password = document.getElementById("EmployeePassword")!;
      password.style.visibility = "hidden";

      var email = document.getElementById("email")!;
      email.setAttribute('readonly', 'readonly');
    }
    
  }

  getShelvesForNewItem() : void
  {
    this.shelvesService.getShelves(this.NewItem.value.Warehouse).subscribe((data) => {
      this.shelves = data;
    });
  }

  HideNewEmployee() {
    const nId = "newEmployeeSectionHidden"; 
    var item = document.getElementById(nId)!;
    item.style.visibility = 'hidden';

    var btn = document.getElementById('newEmployeeBtn')!;
    btn.style.visibility = 'visible';

    const tableid = "tableAllEmployees";
    var table = document.getElementById(tableid)!;
    table.style.width = "100%";
    table.style.minWidth = "100%";
    table.style.maxWidth = "100%";

    var password = document.getElementById('EmployeePassword')!;
    password.style.visibility = 'visible';

    var email = document.getElementById("email")!;
    email.removeAttribute('readonly');

      for(var name in this.NewEmployee.controls) {
            
        (<FormControl>this.NewEmployee.controls[name]).setValue('');
        this.NewEmployee.controls[name].setErrors(null);
      }

      this.submitType = '';
      this.GetEmployees();
    
  }

  GetEmployeeTypes()
  {
    this.userService.getEmployeeTypes().subscribe(data => {
        this.employeeTypes = data;
    });
  }

  SubmitUser()
  {
    if(this.submitType === "change")
    {
      this.userService.change(this.NewEmployee.value).subscribe((data) =>{
        var result = data as Result;
        window.alert(result.Message);
        this.GetEmployees();
        this.HideNewEmployee();
        
        
    });
    }
    else
    {
      this.userService.register(this.NewEmployee.value).subscribe((data) =>{
        var result = data as Result;
        window.alert(result.Message);
        this.GetEmployees();
        this.HideNewEmployee();
        
    });

    
    }

    this.submitType = "";
  }

  ChangeUser(user : User)
  {
    user.Password = "";
    this.NewEmployee.setValue(user);
    this.submitType = "change";
    this.ShowNewEmployee();
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

  allUsersSearch()
  {
    this.userService.search(this.allIUsersKeyWord).subscribe((data) => {
      this.employees = data;
    });
  }

  clearAllUsersSearch()
  {
    this.allIUsersKeyWord = '';
    this.GetEmployees();
  }

  updateAllUsersSearch(e : any) {
    this.allIUsersKeyWord = e.target.value; 
  }

  AllUsersSearchEnter() {
    if(this.allIUsersKeyWord.length >=3)
    {
      this.allUsersSearch();
    }
  }

  RemoveUser(email : string)
  {
    if(window.confirm("Are you sure want to delete " + email + " user?"))
    {
      this.userService.remove(email.toString()).subscribe((data) =>{
        var result = data as Result;
        window.alert(result.Message);
        this.GetEmployees();
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

  getWarehouses() : void {
    this.warehouseService.getWarehouses().subscribe((data) => {
      let array = data as Array<Warehouse>;
      this.warehouseNames = [];
      array.forEach(element => {
        this.warehouseNames.push(element.WarehouseId);
      });
      this.rampWarehouse = this.warehouseNames[0]!;
    });
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
    this.NewItem.setValue(item);
    this.ItemSubmitType = "change";
    this.ShowNewItem();
  }

  RemoveItem(id :string, name:string)
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

    const tableid = "warehouses";
    var table = document.getElementById(tableid)!;
   // table.style.width = "50%";
  //  table.style.minWidth = "50%";
   // table.style.maxWidth = "50%";
    
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

    const tableid = "warehouses";
    var table = document.getElementById(tableid)!;
    //table.style.width = "90%";
  //  table.style.minWidth = "90%";
   // table.style.maxWidth = "90%";

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

  getAllComapnies() : void {
    this.companyService.getCompanies().subscribe((data) => {
      let array = data as Array<Company>;
      this.companies = [];
      array.forEach(element => {
        this.companies.push(element);
      });
    });
  }

  getUnloadedLoads()
  {
    this.loadService.getUnloadedLoads('all').subscribe((data) => {
      this.loads = data;
    })
  }

  getLoadedLoads()
  {
    this.loadService.getLoadedLoads('all').subscribe((data) => {
      this.loads = data;
    })
  }

  getLoadingLoads()
  {
    this.loadService.getLoadingLoads('all').subscribe((data) => {
      this.loads = data;
    })
  }

  getLoad()
  {
    if(this.currentLoadType === 'Unloaded')
    {
      this.getUnloadedLoads();
    }
    if(this.currentLoadType === 'Loaded')
    {
      this.getLoadedLoads();
    }
    if(this.currentLoadType === 'Loading')
    {
      this.getLoadingLoads();
    }
  }

  changeLoadType (event: any) {
   
    this.currentLoadType = event.target.value;
    this.getLoad();
    
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
    this.reportService.getReportsSortByDate('',this.orderDate).subscribe((data) => {
      this.reports = data;
      if(this.orderDate === 'asc')
      {
        this.orderDate = 'desc';
      }
      else
      {
        this.orderDate = 'asc';
      }
    });
  }

  SortByType()
  {
    this.reportService.getReportsSortByType('',this.orderType).subscribe((data) => {
      this.reports = data;
      if(this.orderType === 'asc')
      {
        this.orderType = 'desc';
      }
      else
      {
        this.orderType = 'asc';
      }
    });
  }

  SaveCompany()
  {
    if(this.CompanySubmitType === "change")
    {
      this.companyService.changeCompany(this.Company.value).subscribe((data) =>{
        var result = data as Result;
        window.alert(result.Message);
        this.HideNewCompany();
        this.getAllComapnies();
        
      });
    }
    else
    {
    this.companyService.addCompany(this.Company.value).subscribe((data) =>{
      var result = data as Result;
      window.alert(result.Message);
      
      if(result.Success)
      {
        this.getAllComapnies();
        for(var name in this.Company.controls) {
            
          (<FormControl>this.Company.controls[name]).setValue('');
          this.Company.controls[name].setErrors(null);
        }
      }
    });
    }
  }

  ShowNewCompany() {
    const nId = "newCompanySectionHidden"; 
    var item = document.getElementById(nId)!;
    item.style.visibility = 'visible';

    var btn = document.getElementById('newCompanyBtn')!;
    btn.style.visibility = 'hidden';

    const tableid = "tableAllCompanies";
    var table = document.getElementById(tableid)!;
    table.style.width = "50%";
    table.style.minWidth = "50%";
    table.style.maxWidth = "50%";
    
    if(this.CompanySubmitType === "change")
    {
      //
    }
  }

  HideNewCompany() {
    const nId = "newCompanySectionHidden"; 
    var item = document.getElementById(nId)!;
    item.style.visibility = 'hidden';

    var btn = document.getElementById('newCompanyBtn')!;
    btn.style.visibility = 'visible';

    const tableid = "tableAllCompanies";
    var table = document.getElementById(tableid)!;
    table.style.width = "100%";
    table.style.minWidth = "100%";
    table.style.maxWidth = "100%";

      for(var name in this.Company.controls) {
            
        (<FormControl>this.Company.controls[name]).setValue('');
        this.Company.controls[name].setErrors(null);
      }

      this.CompanySubmitType = '';
    
  }

  ChangeCompany(company : Company)
  {
    this.Company.setValue(company);
    this.CompanySubmitType = "change";
    this.ShowNewCompany();
  }

  RemoveCompany(pib : string | undefined, name : string| undefined)
  {
    if(window.confirm('Are you sure that you want to remove company ' + name + '?'))
    {
      this.companyService.deleteCompany(pib).subscribe(data => {
          var result = data as Result;
          window.alert(result.Message);
          this.getAllComapnies();
      });
    }
  }

  allCompaniesSearch()
  {
    this.companyService.getAllCompaniesSearch(this.allCompaniesKeyWord).subscribe((data) => {
      this.companies = data;
    });
  }

  clearAllCompaniesSearch()
  {
    this.allCompaniesKeyWord = '';
    this.getAllComapnies();
  }

  updateAllCompaniesSearch(e : any) {
    this.allCompaniesKeyWord = e.target.value; 
  }

  AllCompaniesSearchEnter() {
    if(this.allCompaniesKeyWord.length >=3)
    {
      this.companyService.getAllCompaniesSearch(this.allCompaniesKeyWord).subscribe((data) => {
      this.companies = data;
      });
    }
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

  
  GetReceiptPdf(receiptId : string): void {
    this.receiptService.getReceiptFile(receiptId)
        .subscribe(x => {
            // It is necessary to create a new blob object with mime-type explicitly set
            // otherwise only Chrome works like it should
            var newBlob = new Blob([x], { type: "application/pdf" });

            // IE doesn't allow using a blob object directly as link href
            // instead it is necessary to use msSaveOrOpenBlob
            if (window.navigator && window.navigator.msSaveOrOpenBlob) {
                window.navigator.msSaveOrOpenBlob(newBlob);
                return;
            }

            // For other browsers: 
            // Create a link pointing to the ObjectURL containing the blob.
            const data = window.URL.createObjectURL(newBlob);

            var link = document.createElement('a');
            link.href = data;
            link.download = "Receipt" + receiptId + ".pdf";
            // this is necessary as link.click() does not work on the latest firefox
            link.dispatchEvent(new MouseEvent('click', { bubbles: true, cancelable: true, view: window }));

            setTimeout(function () {
                // For Firefox it is necessary to delay revoking the ObjectURL
                window.URL.revokeObjectURL(data);
                link.remove();
            }, 100);

        });
}
GetReportPdf(reportId : string | undefined): void {
  this.reportService.getReportFile(reportId)
      .subscribe(x => {
          // It is necessary to create a new blob object with mime-type explicitly set
          // otherwise only Chrome works like it should
          var newBlob = new Blob([x], { type: "application/pdf" });

          // IE doesn't allow using a blob object directly as link href
          // instead it is necessary to use msSaveOrOpenBlob
          if (window.navigator && window.navigator.msSaveOrOpenBlob) {
              window.navigator.msSaveOrOpenBlob(newBlob);
              return;
          }

          // For other browsers: 
          // Create a link pointing to the ObjectURL containing the blob.
          const data = window.URL.createObjectURL(newBlob);

          var link = document.createElement('a');
          link.href = data;
          link.download = "Report" + reportId + ".pdf";
          // this is necessary as link.click() does not work on the latest firefox
          link.dispatchEvent(new MouseEvent('click', { bubbles: true, cancelable: true, view: window }));

          setTimeout(function () {
              // For Firefox it is necessary to delay revoking the ObjectURL
              window.URL.revokeObjectURL(data);
              link.remove();
          }, 100);

      });
}
LoadSearch()
{
  this.loadService.getLoadsById(this.loadKeyWord).subscribe((data) => {
    this.loads = data;
  });
}

clearLoadSearch()
{
  this.loadKeyWord = '';
  this.getLoad()
}

updateLoadSearch(e : any) {
  this.loadKeyWord = e.target.value; 
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
