import { Component, EventEmitter, Input, Output, viewChild } from "@angular/core";
import { MatDatepickerInputEvent } from "@angular/material/datepicker";
import { MatTimepicker } from "@angular/material/timepicker";
import { SharedModule } from "src/app/theme/shared/shared.module";

@Component({
  selector: 'app-date-time',
  imports: [SharedModule],
  providers: [],
  standalone: true,
  templateUrl: './date-time.component.html',
  styleUrl: './date-time.component.scss'
})
export class DateTimeComponent {
  @Input() selectedDate: Date = new Date();
  @Output() selectedDateChange = new EventEmitter<Date>();
  @Output() onChange = new EventEmitter<Date>();

  public dateString:string = "";
  public timeString:string = "";

  private timeDate:Date = new Date();
  private dateDate:Date = new Date();

  myComponentRef = viewChild.required(MatTimepicker);
  timeChanged = false;

  ngOnInit(): void {
    this.myComponentRef().selected.subscribe((value) => {
      this.timeChanged = true;
      this.timeDate = value.value
      this.updateDate();
    });
  }

  public getFormattedSelectedDate():string{
    return this.selectedDate.toLocaleString();
  }

  public onDateChange(event: MatDatepickerInputEvent<Date>):void {
    this.dateDate = event.value || new Date();
    this.updateDate();
  }

  private updateDate(){
    this.selectedDate = this.combineDateAndTime(this.dateDate, this.timeDate)
    this.selectedDateChange.emit(this.selectedDate);
    this.onChange.emit(this.selectedDate);
  }


  private combineDateAndTime(datePart: Date, timePart: Date): Date {
    const year = datePart.getFullYear();
    const month = datePart.getMonth(); // 0-based
    const day = datePart.getDate();

    const hours = timePart.getHours();

    const minutes = timePart.getMinutes();
    const seconds = timePart.getSeconds();
    const milliseconds = timePart.getMilliseconds();

    return new Date(year, month, day, hours, minutes, seconds, milliseconds);
  }

}