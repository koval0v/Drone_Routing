import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExperimentPageComponent } from './experiment-page/experiment-page.component';
import { SettingsMapComponent } from './components/settings-map/settings-map.component';
import { ProgressComponent } from './components/progress/progress.component';
import { StatisticsComponent } from './components/statistics/statistics.component';
import { CanActivateProgressPageGuard } from 'src/app/guards/activateProgressPage';
import { CanActivateStatPageGuard } from 'src/app/guards/activateStatPage';

const routes: Routes = [
  {
      path: '',
      component: ExperimentPageComponent,
      children: [
          {
            path: '',
            redirectTo: 'settings',
            pathMatch: 'full'
          },
          {
            path: 'settings',
            component: SettingsMapComponent,
            data: {title: 'Підготовка даних'},
          },
          {
            path: 'progress',
            component: ProgressComponent,
            data: {title: 'Обробка'},
            canActivate: [CanActivateProgressPageGuard],
          },
          {
            path: 'statistics',
            component: StatisticsComponent,
            data: {title: 'Результати експериментів'},
            canActivate: [CanActivateStatPageGuard],
          },
          {
            path: '**',
            redirectTo: 'settings',
          },
      ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExperimentRoutingModule { }
