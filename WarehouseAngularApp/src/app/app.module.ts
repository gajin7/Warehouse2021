import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DefaultHomepageComponent } from './default-homepage/default-homepage.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { LoginComponent } from './login/login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtInterceptor } from './auth/jwt-interceptor';
import { AuthService } from './auth/auth.service';
import { DriverHomePageComponent } from './driver-home-page/driver-home-page.component';
import { StorekeeperHomePageComponent } from './storekeeper-home-page/storekeeper-home-page.component';
import { ManagerHomePageComponent } from './manager-home-page/manager-home-page.component';
import { MyProfilePageComponent } from './my-profile-page/my-profile-page.component';
import { QrCodeModule } from 'ng-qrcode';
 
@NgModule({
  declarations: [
    AppComponent,
    DefaultHomepageComponent,
    LoginComponent,
    DriverHomePageComponent,
    StorekeeperHomePageComponent,
    ManagerHomePageComponent,
    MyProfilePageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    ReactiveFormsModule,
    HttpClientModule,
    QrCodeModule
  ],
  providers: [
    AuthService,{ provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
