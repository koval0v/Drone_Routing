import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AboutRoutingModule } from './about-routing.module';
import { AboutPageComponent } from './about-page/about-page.component';
import { GeneralComponent } from './components/general/general.component';
import { AlgorithmsComponent } from './components/algorithms/algorithms.component';
import { StructureComponent } from './components/structure/structure.component';
import { StatisticsTabsComponent } from '../experiment/components/statistics/statistics-tabs/statistics-tabs/statistics-tabs.component';
import { AlgorithmsTabsComponent } from './components/algorithms/algorithms-tabs/algorithms-tabs.component';

@NgModule({
  declarations: [
    AboutPageComponent,
    GeneralComponent,
    AlgorithmsComponent,
    StructureComponent,
    AlgorithmsTabsComponent
  ],
  imports: [
    CommonModule,
    AboutRoutingModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class AboutModule { }
