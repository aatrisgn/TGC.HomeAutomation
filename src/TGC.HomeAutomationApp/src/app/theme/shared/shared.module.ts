// Angular Import
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatTimepickerModule} from '@angular/material/timepicker';

// project import
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { CardComponent } from './components/card/card.component';
import { MatSelectModule } from '@angular/material/select';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule } from '@angular/material/core';

// third party
import { NgScrollbarModule } from 'ngx-scrollbar';
import 'hammerjs';
import 'mousetrap';

// bootstrap import
import { NgbDropdownModule, NgbNavModule, NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    CardComponent,
    BreadcrumbComponent,
    NgbDropdownModule,
    NgbNavModule,
    NgbModule,
    NgScrollbarModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatTimepickerModule,
    MatInputModule,
    MatSelectModule
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    CardComponent,
    BreadcrumbComponent,
    NgbModule,
    NgbDropdownModule,
    NgbNavModule,
    NgScrollbarModule,
    MatDatepickerModule,
    MatTimepickerModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule
  ],
  declarations: []
})
export class SharedModule {}
