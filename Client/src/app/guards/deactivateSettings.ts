/*import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/internal/Observable';

export interface IDeactivateComponent {
  canExit: () => Observable<boolean> | Promise<boolean> | boolean;
}

export class CanDeactivateSettingsPageService {
  constructor(private router: Router) { }

  canDeactivate(component: IDeactivateComponent, currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot, nextState?: RouterStateSnapshot)  {
      return component.canExit();
  }
}

export const CanDeactivateSettingsGuard: CanDeactivateFn<IDeactivateComponent> = (component: IDeactivateComponent,
  currentRoute: ActivatedRouteSnapshot,
  currentState: RouterStateSnapshot,
  nextState?: RouterStateSnapshot) => {
  return inject(CanDeactivateSettingsPageService).canDeactivate(component, currentRoute, currentState, nextState);
}*/
