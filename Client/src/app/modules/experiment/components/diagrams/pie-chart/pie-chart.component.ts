import { Component, Input, ViewChild } from '@angular/core';
import { ApexDataLabels, ApexFill, ApexLegend, ApexStroke, ApexTitleSubtitle, ChartComponent } from "ng-apexcharts";

import {
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexChart
} from "ng-apexcharts";

export type ChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: any;
  legend: ApexLegend;
  colors: string[];
  stroke: ApexStroke;
  dataLabels: ApexDataLabels;
  title: ApexTitleSubtitle;
};

@Component({
  selector: 'app-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.css']
})
export class PieChartComponent {
  @ViewChild("chart") chart: ChartComponent | undefined;
  @Input() chartOptions: Partial<ChartOptions> | undefined;

  constructor() { }
}
