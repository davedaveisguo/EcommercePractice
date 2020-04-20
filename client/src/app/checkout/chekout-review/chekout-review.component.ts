import { Component, OnInit, Input } from '@angular/core';
import { Observable } from 'rxjs';
import { IBasket } from 'src/app/models/basket';
import { BasketService } from 'src/app/basket/basket.service';
import { ToastrService } from 'ngx-toastr';
import { CdkStepper } from '@angular/cdk/stepper';

@Component({
  selector: 'app-chekout-review',
  templateUrl: './chekout-review.component.html',
  styleUrls: ['./chekout-review.component.scss'],
})
export class ChekoutReviewComponent implements OnInit {

  @Input() appStepper: CdkStepper;
  basket$: Observable<IBasket>;

  constructor(
    private basketService: BasketService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
  }

  createPaymentIntent() {
    return this.basketService.createPaymentIntent().subscribe(
      (response: any) => {
        // move to next stepper
        this.appStepper.next();
      },
      (error) => {
        console.log(error);
        this.toastr.error(error.message);
      }
    );
  }
}
