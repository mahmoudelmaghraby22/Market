import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Market';

  constructor(private basketServices: BasketService){}

  ngOnInit(): void {
    const basketId = localStorage.getItem('basket_id');
    if (basketId) {
      this.basketServices.getBasket(basketId).subscribe(() => {
        console.log('Insialised basket');
      }, error => {
        console.log(error);
      })
    }
  }
}
