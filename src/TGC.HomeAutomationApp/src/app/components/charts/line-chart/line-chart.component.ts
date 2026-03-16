
import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import ApexCharts from 'apexcharts';
import { ApexOptions, ChartComponent, NgApexchartsModule } from 'ng-apexcharts';
import { Observable, Subject, Subscription } from 'rxjs';
import { SharedModule } from 'src/app/theme/shared/shared.module';

@Component({
  selector: 'app-line-chart',
  standalone: true,
  imports: [NgApexchartsModule, SharedModule],
  providers: [],
  templateUrl: './line-chart.component.html',
  styleUrl: './line-chart.component.scss'
})

export class LineChartComponent implements OnInit, OnDestroy, OnChanges {

  @Input() series$!: Subject<LineChartData[]>;
  @Input() title: string = '';

  @ViewChild('chartRef') chart?: ChartComponent;

  private seriesSub?: Subscription;

  public chartOptions: Partial<ChartOptions> = {
    series: [],
    chart: {
      type: 'area',
      height: 350,
      animations: { enabled: true },
      toolbar: { show: true }
    },
    title: { text: '' },
    stroke: { curve: 'smooth', width: 2 },
    dataLabels: { enabled: false },
    markers: { size: 3 },
    xaxis: {
      type: 'datetime',
      labels: {
        formatter: function (value: string) {
          const date = new Date(value);
          return date.toUTCString(); // e.g., "11/05/2025"
        }
      }
    },
    yaxis: {
      labels: {
        formatter: (val: number) => val.toFixed(0) // 1 decimal place
      }
    },
    tooltip: {
      x: {
        format: 'dd/MM/yy HH:mm'
      },
      y: {
        formatter: (val: number) => val.toFixed(1) // 1 decimal place
      }
    },
    grid: { borderColor: '#eee' },
    legend: { show: true }
  };

  ngOnInit(): void {
    if (this.title) {
      this.chartOptions.title = { text: this.title };
    }

    this.subscribeToSeries();
  }

  ngOnChanges(changes: SimpleChanges): void {

    if (changes['title']) {
      const newTitle = (changes['title'].currentValue ?? '') as string;
      this.chartOptions.title = { text: newTitle };

      // If chart component is ready, apply via updateOptions for instant UI change
      if (this.chart) {
        this.chart.updateOptions({ title: { text: newTitle } }, false, true);
      }
    }

    // If the series$ observable instance itself changes, resubscribe
    if (changes['series$'] && !changes['series$'].firstChange) {
      this.subscribeToSeries();
    }
  }

  private subscribeToSeries(): void {
    this.seriesSub?.unsubscribe();

    if (!this.series$) {
      return;
    }

    this.seriesSub = this.series$.subscribe((incoming: LineChartData[]) => {
      const mapped = this.toApexSeries(incoming);

      if (this.chart) {
        this.chart.updateSeries(mapped, true);
      } else {
        this.chartOptions.series = mapped;
      }
    });
  }

  private toApexSeries(series: LineChartData[]): ApexAxisChartSeries {
    // ApexAxisChartSeries is an array of { name: string; data: [...] }
    // 'data' can be number[] or {x, y}[] depending on your use-case.
    return series.map(s => ({ name: s.name, data: s.data })) as ApexAxisChartSeries;
  }

  ngOnDestroy(): void {
    this.seriesSub?.unsubscribe();
  }
}

export type LineChartData = {
  name:string
  data:any
}

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: any;
  title: ApexTitleSubtitle;
  stroke: any;
  dataLabels?: ApexDataLabels;
  markers?: ApexMarkers;
  xaxis?: ApexXAxis;
  yaxis?: ApexYAxis;
  tooltip?: ApexTooltip;
  grid?: ApexGrid;
  legend?: ApexLegend;
};