import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExperimentPageComponent } from './experiment-page/experiment-page.component';
import { SettingsMapComponent } from './components/settings-map/settings-map.component';
import { ProgressComponent } from './components/progress/progress.component';
import { StatisticsComponent } from './components/statistics/statistics.component';
import { ExperimentRoutingModule } from './experiment-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import { StatisticsTabsComponent } from './components/statistics/statistics-tabs/statistics-tabs/statistics-tabs.component';
import { PieChartComponent } from './components/diagrams/pie-chart/pie-chart.component';
import { NgApexchartsModule } from 'ng-apexcharts';
import { ColumnsComponent } from './components/diagrams/columns/columns.component';
import { RangebarMapComponent } from './components/diagrams/rangebar-map/rangebar-map.component';

@NgModule({
  declarations: [
    ExperimentPageComponent,
    SettingsMapComponent,
    ProgressComponent,
    StatisticsComponent,
    StatisticsTabsComponent,
    PieChartComponent,
    ColumnsComponent,
    RangebarMapComponent
  ],
  imports: [
    CommonModule,
    ExperimentRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgApexchartsModule
  ]
})
export class ExperimentModule { }
