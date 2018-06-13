import * as Raven from 'raven-js';
import { NgModule, ErrorHandler } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { ToastyModule } from 'ng2-toasty';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';
import { VehicleFormComponent } from './components/vehicle-form/vehicle-form.component';
import { VehicleService } from './services/vehicle.service';
import { AppErrorHandler } from './app.error-handler';

Raven.config('https://4faafe3bc3354f96a36035993ca0120c@sentry.io/1225799').install();

@NgModule({
   declarations: [
      AppComponent,
      NavMenuComponent,
      CounterComponent,
      FetchDataComponent,
      HomeComponent,
      VehicleFormComponent
   ],
   imports: [
      CommonModule,
      HttpModule,
      FormsModule,
      ToastyModule.forRoot(),
      RouterModule.forRoot([
         { path: '', redirectTo: 'home', pathMatch: 'full' },
         { path: 'vehicles/new', component: VehicleFormComponent },
         { path: 'vehicles/:id', component: VehicleFormComponent },
         { path: 'home', component: HomeComponent },
         { path: 'counter', component: CounterComponent },
         { path: 'fetch-data', component: FetchDataComponent },
         { path: '**', redirectTo: 'home' }
      ])
   ],
   providers: [
      { provide: ErrorHandler, useClass: AppErrorHandler },
      VehicleService
   ]
})
export class AppModuleShared {
}
