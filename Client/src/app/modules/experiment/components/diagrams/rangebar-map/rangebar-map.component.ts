import { Component, Input, ViewChild } from '@angular/core';
import { ChartComponent } from 'ng-apexcharts';
import { ChartOptionsColumn } from '../columns/columns.component';
import { Settings } from 'src/app/models/Settings.model';

@Component({
  selector: 'app-rangebar-map',
  templateUrl: './rangebar-map.component.html',
  styleUrls: ['./rangebar-map.component.css']
})
export class RangebarMapComponent {
  @ViewChild("chart") chart: ChartComponent | undefined;
  @Input() chartOptions: Partial<ChartOptionsColumn> | undefined;

  constructor() { }
}
