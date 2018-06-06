import { Component, OnInit, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
   selector: 'vega',
   templateUrl: './vega.component.html',
   styleUrls: ['./vega.component.css']
})
export class VegaComponent implements OnInit {
   public makes: GetMakes[];
   public features: GetFeatures[];
   public models: string[];

   constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
      http.get(baseUrl + 'api/makes')
         .subscribe(result => {
            this.makes = result.json() as GetMakes[];
         }, error => console.error(error));

      http.get(baseUrl + 'api/features')
         .subscribe(result => {
            this.features = result.json() as GetFeatures[];
         }, error => console.error(error));
   }

   ngOnInit(): void {

   }

   public loadModel(models) {

      this.models = models;

   }
}


interface GetFeatures {
   id: string;
   name: string;
}

interface GetMakes {
   id: string;
   name: string;
   models: string[];
}
