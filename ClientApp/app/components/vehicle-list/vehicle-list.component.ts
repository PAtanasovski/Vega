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
   query: any = {
      pageSize: 3
   };
   columns = [
      { title: 'Id' },
      { title: 'Make', key: 'make', isSortable: true },
      { title: 'Model', key: 'model', isSortable: true },
      { title: 'Contact Name', key: 'contactName', isSortable: true },
      {}
   ];

   constructor(private vehicleService: VehicleService) { }

   ngOnInit() {
      this.vehicleService.getMakes()
         .subscribe(makes => this.makes = makes);

      this.populateVehicles();
   }

   private populateVehicles() {
      this.vehicleService.getVehicles(this.query)
         .subscribe(vehicles => this.vehicles = vehicles);
   }

   onFilterChange() {
      this.populateVehicles();
   }

   resetFilter() {
      this.query = {};
      this.onFilterChange();
   }

   sortBy(columnName) {
      if (this.query.sortBy === columnName) {
         this.query.isSortAscending = !this.query.isSortAscending;
      } else {
         this.query.sortBy = columnName;
         this.query.isSortAscending = true;
      }

      this.populateVehicles();
   }

   onPageChange(page) {
      this.query.page = page;
      this.populateVehicles();
   }
}
