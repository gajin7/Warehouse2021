import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { Result } from 'src/app/Models/Result';
import { User } from 'src/app/Models/User';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css']
})
export class EmployeesComponent implements OnInit {
  employees = Array<User>();
  employeeTypes : any;
  submitType = "";
  allIUsersKeyWord : string = '';
  
  NewEmployee = this.fb.group({
    FirstName: ['', Validators.required],
    LastName: ['', Validators.required],
    Email: ['', Validators.required],
    Type: ['',Validators.required],
    Password: ['', Validators.required],
  });

  constructor(private userService: UserService, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.GetEmployees();
    this.GetEmployeeTypes();
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


}
