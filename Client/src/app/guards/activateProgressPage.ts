import { Injectable, OnInit, inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot} from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class CanActivateProgressPageService {

  constructor(private router: Router) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean  {
    console.log(this.router.getCurrentNavigation()?.extras.state);
    if (this.router.getCurrentNavigation()?.extras.state != undefined) {
      return true;
    } else {
      this.router.navigateByUrl("/");
      return false;
    }
  }
}

export const CanActivateProgressPageGuard: CanActivateFn = (next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean => {
  return inject(CanActivateProgressPageService).canActivate(next, state);
}
