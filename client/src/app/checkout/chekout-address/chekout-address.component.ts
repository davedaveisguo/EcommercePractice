import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { AccountService } from 'src/app/account/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-chekout-address',
  templateUrl: './chekout-address.component.html',
  styleUrls: ['./chekout-address.component.scss'],
})
export class ChekoutAddressComponent implements OnInit {
  @Input() checkoutForm: FormGroup;

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {}

  saveUserAddress() {
    this.accountService
      .updateUserAddress(this.checkoutForm.get('addressForm').value)
      .subscribe(
        () => {
          this.toastr.success('Address saved');
        },
        (error) => {
          this.toastr.error(error.message);
          console.log(error);
        }
      );
  }
}
