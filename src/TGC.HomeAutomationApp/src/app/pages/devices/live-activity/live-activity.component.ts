import { Component } from "@angular/core";
import { MeasureClient, MeasureRequest } from "src/app/generated/autogenerated-client";
import { SignalRService } from "src/app/services/SignalRService.service";
import { SharedModule } from "src/app/theme/shared/shared.module";

class RawMeasureTableRecord extends MeasureRequest {
  isNew: boolean = true;
  received: string = new Date().toLocaleTimeString("da-dk");

  constructor(measureRequest: MeasureRequest) {
    super();
    this.dataValue = measureRequest.dataValue;
    this.macAddress = measureRequest.macAddress;
    this.type = measureRequest.type;
  }
}

@Component({
  selector: 'app-live-activity',
  standalone: true,
  imports: [SharedModule],
  providers: [MeasureClient],
  templateUrl: './live-activity.component.html',
  styleUrls: ['./live-activity.component.scss']
})
export default class LiveActivityComponent {

  public rawMeasures:RawMeasureTableRecord[];
  receivedMessage: string = "";

  constructor(private signalRService: SignalRService) {
    this.rawMeasures = [];
  }

  ngOnInit(): void {
    this.signalRService.startConnection().subscribe(() => {
      this.signalRService.receiveMessage().subscribe((message) => {
        this.receivedMessage = message;

        const parsed = JSON.parse(message);
        const deserialized = MeasureRequest.fromJS(parsed);

        var deserializedMessage = new RawMeasureTableRecord(deserialized);

        this.rawMeasures.unshift(deserializedMessage);

        setTimeout(() => {
          deserializedMessage.isNew = false;
        }, 5000);
      });
    });
  }

  sendMessage(): void {
    let timestamp = new Date();
    this.signalRService.sendMessage(timestamp.toTimeString());
  }
}
