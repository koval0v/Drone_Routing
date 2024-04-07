import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-about-page',
  templateUrl: './about-page.component.html',
  styleUrls: ['./about-page.component.css']
})
export class AboutPageComponent {
  stageTitle : string = 'Інформаційна система з підтримки процесу дослідження алгоритмів маршрутизації дронів';

  constructor(public router: Router) {}

  openExperimentPage() {
    this.router.navigateByUrl("/experiment");
  }

  changeInfoSection(url: string) {
    this.router.navigateByUrl(`/about/${url}`);
  }
}
