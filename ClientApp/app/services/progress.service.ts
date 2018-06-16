import { Injectable } from '@angular/core';
import { BrowserXhr } from '@angular/http';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class ProgressService {
   uploadProgress: Subject<any> = new Subject();
   downloadProgress: Subject<any> = new Subject();
}

@Injectable()
export class BrowserXhrWithProgress extends BrowserXhr {
   constructor(private service: ProgressService) {
      super();
   }

   build(): XMLHttpRequest {
      var xhr: XMLHttpRequest = super.build();

      xhr.onprogress = (event) => {
         this.service.downloadProgress.next(this.createProgress(event));
      };

      xhr.upload.onprogress = (event) => {
         this.service.uploadProgress.next(this.createProgress(event));
      };

      return xhr;
   }

   private createProgress(event) {
      return {
         total: event.total,
         percentage: Math.round(event.loaded / event.total * 100)
      };
   }
}