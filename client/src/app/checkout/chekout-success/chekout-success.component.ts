import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IOrder } from 'src/app/models/order';

@Component({
  selector: 'app-chekout-success',
  templateUrl: './chekout-success.component.html',
  styleUrls: ['./chekout-success.component.scss'],
})
export class ChekoutSuccessComponent implements OnInit {
  order: IOrder;

  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    const state = navigation && navigation.extras && navigation.extras.state;
    if (state) {
      this.order = state as IOrder;
    }
  }

  ngOnInit(): void {}
}
