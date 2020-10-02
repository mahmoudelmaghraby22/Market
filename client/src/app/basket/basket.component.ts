import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IBasket, IBasketItem } from '../shared/models/basket';
import { BasketService } from './basket.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent implements OnInit {
  basket$: Observable<IBasket>;

  constructor(private basketServices: BasketService) { }

  ngOnInit(): void {
    this.basket$ = this.basketServices.basket$;
  }

  removeBasketItem(item: IBasketItem) {
    this.basketServices.removeItemFormBasket(item);
  }

  incrementItemQuantity(item: IBasketItem) {
    this.basketServices.incrementItemQuantity(item);
  }

  decrementItemQuantity(item: IBasketItem) {
    this.basketServices.decrementItemQuantity(item);
  }

}
