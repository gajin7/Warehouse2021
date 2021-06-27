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
import { ReportsComponent } from './ReusableComponents/reports/reports.component';
import { LoadsComponent } from './ReusableComponents/loads/loads.component';
import { ItemsComponent } from './ReusableComponents/items/items.component';
import { CompaniesComponent } from './manager-components/companies/companies.component';
import { EmployeesComponent } from './manager-components/employees/employees.component';
import { RampsComponent } from './manager-components/ramps/ramps.component';
import { VehiclesComponent } from './manager-components/vehicles/vehicles.component';
import { WarehouseAndShelvesComponent } from './manager-components/warehouse-and-shelves/warehouse-and-shelves.component';
 
@NgModule({
  declarations: [
    AppComponent,
    DefaultHomepageComponent,
    LoginComponent,
    DriverHomePageComponent,
    StorekeeperHomePageComponent,
    ManagerHomePageComponent,
    MyProfilePageComponent,
    ReportsComponent,
    LoadsComponent,
    ItemsComponent,
    CompaniesComponent,
    EmployeesComponent,
    RampsComponent,
    VehiclesComponent,
    WarehouseAndShelvesComponent
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
