import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from '../auth/login.service';
import { ChangePassword } from '../Models/ChangePassword';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-my-profile-page',
  templateUrl: './my-profile-page.component.html',
  styleUrls: ['./my-profile-page.component.css']
})
export class MyProfilePageComponent implements OnInit {


  FirstName : string | undefined;
  LastName : string | undefined;
  Email : string | undefined;
  Type : string | undefined;
  OldPassword : string | undefined = '';
  NewPassword : string | undefined = '';
  ChangePasswordVisibility : boolean = false;
  constructor(private fb: FormBuilder,private userService : UserService, private router : Router, private loginService : LoginService) { }

  ngOnInit(): void {
    this.userService.getProfile(localStorage.email).subscribe(data =>
      {
        this.FirstName = data.FirstName;
        this.LastName = data.LastName;
        this.Email = data.Email;
        this.Type = data.Type;

      });
  }

  updateOldPassword(pass : any)
  {
    this.OldPassword = pass.target.value;
  }

  updateNewPassword(pass : any)
  {
    this.NewPassword = pass.target.value;
  }


  ChangePassword()
  {
    var changePass = new ChangePassword();
    changePass.Email = this.Email;
    changePass.OldPassword = this.OldPassword;
    changePass.NewPassword = this.NewPassword;

    this.userService.changePassword(changePass).subscribe(data =>
      {
        if(data.Success)
        {

        window.alert(data.Message + " You will be logged out. To continue, please login with new password");
        this.NewPassword = '';
        this.OldPassword = '';
        this.loginService.logout();
        this.router.navigate(['']);
        }
        else
        {
          window.alert(data.Message);
        }
      });

  }

  ShowChangePassword()
  {
    if(this.ChangePasswordVisibility)
    {
      this.ChangePasswordVisibility = false;
      console.log(this.ChangePasswordVisibility,"change");
    }
    else
    {
    this.ChangePasswordVisibility = true;
    }
  }
}
