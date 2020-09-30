import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IProduct } from 'src/app/shared/models/product';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss'],
})
export class ProductDetailsComponent implements OnInit {
  product: IProduct;

  constructor(private shopServices: ShopService, private activateRouter: ActivatedRoute, private bcServices: BreadcrumbService) {
    this.bcServices.set('@productDetails', ' ');
  }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    this.shopServices.getProduct(+this.activateRouter.snapshot.paramMap.get('id'))
      .subscribe(product => {
        this.product = product;
        this.bcServices.set('@productDetails', product.name);
      }, error => {
        console.log(error);
      });
  }
}
