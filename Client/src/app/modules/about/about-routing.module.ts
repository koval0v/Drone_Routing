import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AboutPageComponent } from './about-page/about-page.component';
import { GeneralComponent } from './components/general/general.component';
import { AlgorithmsComponent } from './components/algorithms/algorithms.component';
import { StructureComponent } from './components/structure/structure.component';

const routes: Routes = [
  {
      path: '',
      component: AboutPageComponent,
      children: [
          {
            path: '',
            redirectTo: 'general',
            pathMatch: 'full'
          },
          {
            path: 'general',
            component: GeneralComponent,
          },
          {
            path: 'algorithms',
            component: AlgorithmsComponent,
          },
          {
            path: 'structure',
            component: StructureComponent,
          },
          {
            path: '**',
            redirectTo: 'general',
          },
      ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AboutRoutingModule { }
