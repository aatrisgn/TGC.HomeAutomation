import { Component } from '@angular/core';
import { ApexOptions, NgApexchartsModule } from 'ng-apexcharts';
import { ChartDB } from 'src/app/fack-db/chartData';
import { SharedModule } from 'src/app/theme/shared/shared.module';

@Component({
  selector: 'app-temperature-summary',
  standalone: true,
  imports: [NgApexchartsModule, SharedModule],
  providers: [],
  templateUrl: './temperature-summary.component.html',
  styleUrl: './temperature-summary.component.scss'
})

export class TemperatureSummaryComponent {
  area1CAC: ApexOptions;
  chartDB: any;

  constructor(){

    this.area1CAC = TemperatureSummaryComponent.localData;
  }

  static localData:any = {
    chart: {
      height: 350,
      type: 'area'
    },
    dataLabels: {
      enabled: false
    },
    stroke: {
      curve: 'smooth'
    },
    colors: ['#FFB64D', '#FF5370'],
    series: [
      {
        name: 'series1',
        data: [31, 40, 28, 51, 42, 109, 100]
      },
      {
        name: 'series2',
        data: [11, 32, 45, 32, 34, 52, 41]
      }
    ],
    xaxis: {
      type: 'datetime',
      categories: [
        '2018-09-19T00:00:00',
        '2018-09-19T01:30:00',
        '2018-09-19T02:30:00',
        '2018-09-19T03:30:00',
        '2018-09-19T04:30:00',
        '2018-09-19T05:30:00',
        '2018-09-19T06:30:00'
      ]
    },
    tooltip: {
      x: {
        format: 'dd/MM/yy HH:mm'
      }
    }
  };
}
