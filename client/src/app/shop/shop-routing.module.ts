import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ShopComponent } from './shop.component';
import { ProductDetailsComponent } from './product-details/product-details.component';
// lazy loading step 1
const routes: Routes = [
  { path: '', component: ShopComponent },
  { path: ':id', component: ProductDetailsComponent },
];

@NgModule({
  declarations: [],
  // lazy loading step 2  RouterModule.forChild(routes)
  imports: [CommonModule, RouterModule.forChild(routes)],
  // lazy loading 3, export routerModule then import in shop.module
  exports: [ RouterModule ]
})
export class ShopRoutingModule {}
