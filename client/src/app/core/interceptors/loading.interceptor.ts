import { HttpInterceptor } from '@angular/common/http';
import { HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { BusyService } from '../services/busy.service';
import { Injectable } from '@angular/core';
import { finalize, delay } from 'rxjs/operators';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  constructor(private busyService: BusyService) {}
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (req.method === 'POST' && req.url.includes('orders'))
    {
        return next.handle(req);
    }

    if (!req.url.includes('emailexists')) {
      this.busyService.busy();
    }
    return next.handle(req).pipe(
      delay(300),
      finalize(() => {
        this.busyService.idle();
      })
    );
  }
}
