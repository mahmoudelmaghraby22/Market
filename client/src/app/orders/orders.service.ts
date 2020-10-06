import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  baseUrl = environment.ApiUrl;

  constructor(private http: HttpClient) { }

  getOrderDetailed(id: number) {
    return this.http.get(this.baseUrl + 'orders/' + id);
  }

  getOrdersForUser() {
    return this.http.get(this.baseUrl + 'orders');
  }
}
