import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DriverGuard } from './auth/driver.guard';
import { LoginGuard } from './auth/login.guard';
import { ManagerGuard } from './auth/manager.guard';
import { ProfileGuard } from './auth/profile.guard';
import { StorekeeperGuard } from './auth/storekeeper.guard';
import { DefaultHomepageComponent } from './default-homepage/default-homepage.component';
import { DriverHomePageComponent } from './driver-home-page/driver-home-page.component';
import { LoginComponent } from './login/login.component';
import { ManagerHomePageComponent } from './manager-home-page/manager-home-page.component';
import { MyProfilePageComponent } from './my-profile-page/my-profile-page.component';
import { StorekeeperHomePageComponent } from './storekeeper-home-page/storekeeper-home-page.component';

const routes: Routes = [
  {
    path: '',
    component: DefaultHomepageComponent,
    canActivate: [LoginGuard]
  },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [LoginGuard]
  },
  {
    path: 'driver-home',
    component: DriverHomePageComponent,
    canActivate: [DriverGuard]
  },
  {
    path: 'manager-home',
    component: ManagerHomePageComponent,
    canActivate: [ManagerGuard]
  },
  {
    path: 'storekeeper-home',
    component: StorekeeperHomePageComponent,
    canActivate: [StorekeeperGuard]
  },
  {
    path: 'my-profile',
    component: MyProfilePageComponent,
    canActivate: [ProfileGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
