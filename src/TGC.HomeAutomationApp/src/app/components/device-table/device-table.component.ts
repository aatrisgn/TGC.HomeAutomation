import { Component } from '@angular/core';
import { ApiKeyRequest, DeviceClient, DeviceResponse, MeasureClient } from 'src/app/generated/autogenerated-client';
import { SharedModule } from 'src/app/theme/shared/shared.module';

@Component({
  selector: 'app-device-table',
  imports: [SharedModule],
  providers: [DeviceClient, MeasureClient],
  standalone: true,
  templateUrl: './device-table.component.html',
  styleUrl: './device-table.component.scss'
})
export class DeviceTableComponent {

  deviceData: DeviceResponse[] = [];

  constructor(private deviceClient:DeviceClient, private measureClient:MeasureClient){

  }

  ngOnInit() {
    this.loadDevices();
  }

  loadDevices(){
    this.deviceClient.getAllDevices().subscribe(data => {
      this.deviceData = data;
    })
  }

  generateNewApiKey(id:string){

    var apiKeyRequest = new ApiKeyRequest();
    apiKeyRequest.expirationDate = new Date();
    apiKeyRequest.name = "SomeName"

    this.deviceClient.updateApiKey(id, apiKeyRequest).subscribe(response => {
      console.log(response)
    })
  }

  getLatestOrderedMeasuresForDevice(id:string){
    this.measureClient.getLatestActivityByDeviceId(id).subscribe(response => {
      console.log(response);
    })
  }

  deleteDevice(id:string){
    console.log(id)
    this.deviceClient.deleteDevice(id).subscribe(data => {
      this.loadDevices();
    })
  }
}
