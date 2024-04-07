import { AboutModule } from './modules/about/about.module';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [{
    path: '',
    redirectTo: 'about',
    pathMatch: 'full',
  },
  {
    path: 'about',
    loadChildren: () => import('./modules/about/about-routing.module').then((m) => m.AboutRoutingModule),
  },
  {
    path: 'experiment',
    loadChildren: () => import('./modules/experiment/experiment-routing.module').then((m) => m.ExperimentRoutingModule),
  },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
