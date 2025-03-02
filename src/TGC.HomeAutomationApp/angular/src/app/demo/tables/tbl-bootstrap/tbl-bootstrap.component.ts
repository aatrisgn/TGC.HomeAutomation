// angular import
import { Component } from '@angular/core';

// project import
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ProductSaleComponent } from '../product-sale/product-sale.component';

@Component({
  selector: 'app-tbl-bootstrap',
  standalone: true,
  imports: [SharedModule, ProductSaleComponent],
  templateUrl: './tbl-bootstrap.component.html',
  styleUrls: ['./tbl-bootstrap.component.scss']
})
export default class TblBootstrapComponent {}
