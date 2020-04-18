import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { IDeliveryMethod } from '../models/deliveryMethod';
import { map } from 'rxjs/operators';
import { IOrderToCreate } from '../models/order';

@Injectable({
  providedIn: 'root',
})
export class CheckoutService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getDeliveryMethods() {
    return this.http.get(this.baseUrl + 'orders/deliveryMethods').pipe(
      map((dm: IDeliveryMethod[]) => {
        // sorted in highest price first
        return dm.sort((a, b) => b.price - a.price);
      })
    );
  }

  // create order
  createOrder(order: IOrderToCreate){
    return this.http.post(this.baseUrl + 'orders', order);
  }
}
