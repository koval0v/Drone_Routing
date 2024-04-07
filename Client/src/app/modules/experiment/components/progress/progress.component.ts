import { Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Settings } from 'src/app/models/Settings.model';

@Component({
  selector: 'app-progress',
  templateUrl: './progress.component.html',
  styleUrls: ['./progress.component.css']
})
export class ProgressComponent {
  settings : Settings = new Settings();

  constructor(private router : Router) {}
}
