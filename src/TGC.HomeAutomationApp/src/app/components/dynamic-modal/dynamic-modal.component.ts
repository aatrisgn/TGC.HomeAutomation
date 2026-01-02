import { Component, EventEmitter, Input, Output } from '@angular/core';
import { SharedModule } from 'src/app/theme/shared/shared.module';

@Component({
  selector: 'app-dynamic-modal',
  templateUrl: './dynamic-modal.component.html',
  styleUrls: ['./dynamic-modal.component.scss'],
  imports: [SharedModule],
  standalone: true
})
export class DynamicModalComponent {
  @Input() isVisible: boolean = false; // Controls modal visibility
  @Input() title: string = ""; // Controls modal visibility
  @Output() isVisibleChange = new EventEmitter<boolean>(); // Two-way binding support
  @Output() closeEvent = new EventEmitter<void>(); // Emits when modal is closed

  // Default close logic
  onClose() {
    console.log(this.isVisible);
    this.isVisible = false;
    this.closeEvent.emit();
  }
}