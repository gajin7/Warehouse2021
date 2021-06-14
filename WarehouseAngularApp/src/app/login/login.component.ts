import { Component} from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';
import { LoginService } from '../auth/login.service';
import { Result } from '../Models/Result';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  isLoginFailed: boolean = false;
  message: string = "Username or password is wrong. Please try again";

    loginForm = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required],
  });
  constructor(public authService: AuthService,public loginService: LoginService,private fb: FormBuilder,public router: Router) { 
    this.isLoginFailed = false;
  }


  login() {
    this.authService.getToken(this.loginForm.value).subscribe((data) => {
      if(this.authService.isLoggedin)
      {
          this.isLoginFailed = false;
          if(localStorage.role == "driver")
          {
              this.router.navigate(['driver-home']);
          }
          else if(localStorage.role == "admin")
          {
              this.router.navigate(['manager-home']);
          }
          else if(localStorage.role == "storekeeper")
          {
              this.router.navigate(['storekeeper-home']);
          }
          else
          {
            this.authService.logout();
            this.isLoginFailed = true;
            window.alert("Login failed, please check your username and password");
          }
      }
      else
      {
        this.isLoginFailed = true;
        window.alert("Login failed, please check your username and password");
      }
  
    });
      
  }

  ResetPassword()
  {
    if(this.loginForm.value['username'] === '')
    {
      window.alert("Please enter username first");
    }
    else
    {
      this.authService.resetPassword(this.loginForm.value['username']).subscribe((data)=>{
        var result = data as Result;
        window.alert(result.Message);
      });
    }
  }


}
