import { NgModule } from '@angular/core';
import { OrdersComponent } from './orders.component';
import { OrderDetailedComponent } from './order-detailed/order-detailed.component';
import { OrderRoutingModule } from './order-routing.module';
import { CommonModule } from '@angular/common';



@NgModule({
  declarations: [OrdersComponent, OrderDetailedComponent],
  imports: [
    CommonModule,
    OrderRoutingModule
  ]
})
export class OrdersModule { }
