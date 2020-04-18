import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { Observable } from 'rxjs';
import { IBasket, IBasketItem } from 'src/app/models/basket';

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss'],
})
export class BasketSummaryComponent implements OnInit {
  // child element emit value to parent element
  @Output() decrement: EventEmitter<IBasketItem> = new EventEmitter<
    IBasketItem
  >();
  @Output() increment: EventEmitter<IBasketItem> = new EventEmitter<
    IBasketItem
  >();
  @Output() remove: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();


  // this is element get from parent check-out-review
  @Input() isBasket = true;

  basket$: Observable<IBasket>;

  constructor(private basketService: BasketService) {}

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
  }

  decrementItemQuantity(item: IBasketItem) {
    this.decrement.emit(item);
  }

  incrementItemQuantity(item: IBasketItem) {
    this.increment.emit(item);
  }

  removeBasketItem(item: IBasketItem) {
    this.remove.emit(item);
  }
}
