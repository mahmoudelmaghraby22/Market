import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { OrderDetailedComponent } from './order-detailed/order-detailed.component';
import { OrdersComponent } from './orders.component';
import { BrowserModule } from '@angular/platform-browser';

const routes: Routes = [
  {path: '', component: OrdersComponent},
  {path: ':id', component: OrderDetailedComponent, data: {breadcrumb: {alias: 'OrderDetailed'}}}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ], exports: [RouterModule]
})
export class OrderRoutingModule { }
