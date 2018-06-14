import { Component, OnInit } from '@angular/core';

import { Vehicle, KeyValuePair } from '../../models/vehicle';
import { VehicleService } from '../../services/vehicle.service';

@Component({
   selector: 'app-vehicle-list',
   templateUrl: './vehicle-list.component.html'
})
export class VehicleListComponent implements OnInit {
   vehicles: Vehicle[];
   makes: KeyValuePair[];
   filter: any = {};

   constructor(private vehicleService: VehicleService) { }

   ngOnInit() {
      this.vehicleService.getMakes()
         .subscribe(makes => this.makes = makes);

      this.populateVehicles();
   }

   private populateVehicles() {
      this.vehicleService.getVehicles(this.filter)
         .subscribe(vehicles => this.vehicles = vehicles);
   }

   onFilterChange() {
      this.populateVehicles();
   }

   resetFilter() {
      this.filter = {};
      this.onFilterChange();
   }
}
