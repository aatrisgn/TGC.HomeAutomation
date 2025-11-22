import { Component, EventEmitter, Inject, Input, Output } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogActions, MatDialogContent } from '@angular/material/dialog'

@Component({
  selector: 'app-dynamic-popup',
  imports: [],
  templateUrl: './dynamic-popup.component.html',
  styleUrl: './dynamic-popup.component.scss',
  standalone: true
})
export class DynamicPopupComponent {
  @Output() close = new EventEmitter<void>();

  closePopup() {
	  this.close.emit();
  }
}
