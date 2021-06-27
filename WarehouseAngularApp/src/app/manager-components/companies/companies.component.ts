import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { Company } from 'src/app/Models/Company';
import { Result } from 'src/app/Models/Result';
import { CompaniesService } from 'src/app/services/companies.service';

@Component({
  selector: 'app-companies',
  templateUrl: './companies.component.html',
  styleUrls: ['./companies.component.css']
})
export class CompaniesComponent implements OnInit {
  CompanySubmitType = "";
  allCompaniesKeyWord : string = '';
  companies: Array<Company>

  Company = this.fb.group({
    Name: ['', Validators.required],
    PIB: ['', Validators.required],
    Address: ['', Validators.required],
    Deposit: ['',Validators.required],
    AccountNo: ['', Validators.required],
  });
  
  constructor( private fb: FormBuilder,private companyService : CompaniesService,) 
  {     
    this.companies = [];
  }

  ngOnInit(): void {
    this.getAllComapnies();
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

}
