import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Settings } from 'src/app/models/Settings.model';
import { ExperimentsService } from 'src/app/services/experiments.service';
import { SettingsMapComponent } from '../components/settings-map/settings-map.component';
import { ProgressComponent } from '../components/progress/progress.component';
import { StatisticsComponent } from '../components/statistics/statistics.component';
import { ExperimentStatistics } from 'src/app/models/ExperimentStatistics.model';
import { TheNearestToTheLineTypes } from 'src/app/models/enums/TheNearestToTheLineTypes.model';
import { TheNearestNeighbourTypes } from 'src/app/models/enums/TheNearestNeighbourTypes.model';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-experiment-page',
  templateUrl: './experiment-page.component.html',
  styleUrls: ['./experiment-page.component.css']
})
export class ExperimentPageComponent implements OnInit {
  stageTitle : string = '';
  experimentIsProcessed: boolean = false;
  experimentIsProcessing: boolean = false;
  settings : Settings = new Settings();
  experimentStatistics : ExperimentStatistics = {} as ExperimentStatistics;
  mapComponent: SettingsMapComponent | undefined;
  /*@ViewChild('settingsForm', { static: true }) ngForm: NgForm = {} as NgForm;*/

  constructor(private route: ActivatedRoute, private router: Router,
    private experimentsService: ExperimentsService) {}

  ngOnInit() {
    this.route.url.subscribe(() => {
      this.stageTitle = this.route.snapshot.firstChild?.data['title'];
    });
  }

  redirectToMainPage() {
    this.router.navigateByUrl("/");
  }

  launchExperiment() {
    /*this.router.navigateByUrl("/experiment/progress", { state: { from: 'isFromStartPage' } });*/
    this.router.navigate(['/experiment/progress'], { state: { message: 'isFromStartPage' } });
    this.stageTitle = 'Обробка';
    this.experimentIsProcessing = true;
    this.experimentsService.launch(this.settings).subscribe(res => {
      this.experimentStatistics = res;
      console.log(res);
      this.experimentStatistics.averageSpeed = this.settings.averageSpeed;
      this.experimentStatistics.timeResource = this.settings.timeResource;
      this.experimentStatistics.targetsCount = this.settings.targetsQuantity;
      let targetsSum : number[] = [];
      let executionTimeSum : number[] = [];
      this.experimentStatistics.maxExecutionTime = 0;
      this.experimentStatistics.minExecutionTime = 100;
      this.experimentStatistics.totalResults.forEach((item) => {
        item.algorithmsResults.forEach((innerItem) => {
          targetsSum.push(innerItem.route.length);
          executionTimeSum.push(innerItem.executionTime)
          if (innerItem.executionTime > this.experimentStatistics.maxExecutionTime) {
            this.experimentStatistics.maxExecutionTime = innerItem.executionTime;
            if (innerItem.lineType) {
              this.experimentStatistics.maxExecutionTimeMethodName =
                Object.values(TheNearestToTheLineTypes)[+innerItem.lineType.toString()];
            }
            if (innerItem.neighbourType) {
              this.experimentStatistics.maxExecutionTimeMethodName =
                Object.values(TheNearestNeighbourTypes)[+innerItem.neighbourType.toString()];
            }
          }
          if (innerItem.executionTime <= this.experimentStatistics.minExecutionTime && innerItem.executionTime >= 0.01) {
            this.experimentStatistics.minExecutionTime = innerItem.executionTime;
            if (innerItem.lineType) {
              this.experimentStatistics.minExecutionTimeMethodName =
                Object.values(TheNearestToTheLineTypes)[+innerItem.lineType.toString()];
            }
            if (innerItem.neighbourType) {
              this.experimentStatistics.minExecutionTimeMethodName =
                Object.values(TheNearestNeighbourTypes)[+innerItem.neighbourType.toString()];
            }
          }
        });
      });
      this.experimentStatistics.averageTargetsCount = this.average(targetsSum) - 2;
      this.experimentStatistics.avgExecutionTime = this.average(executionTimeSum);
      const avgExecutionTimeResult = this.experimentStatistics.totalResults.flatMap(x => x.algorithmsResults)
        .find(i => Math.round(i.executionTime) >= this.experimentStatistics.avgExecutionTime);
      if (avgExecutionTimeResult && avgExecutionTimeResult.lineType) {
        this.experimentStatistics.avgExecutionTimeMethodName =
          Object.values(TheNearestToTheLineTypes)[+avgExecutionTimeResult.lineType.toString()];
      }
      if (avgExecutionTimeResult && avgExecutionTimeResult.neighbourType) {
        this.experimentStatistics.avgExecutionTimeMethodName =
          Object.values(TheNearestNeighbourTypes)[+avgExecutionTimeResult.neighbourType.toString()];
      }
      this.router.navigate(['/experiment/statistics'], { state: { message: 'isFromProgressPage' } });
      this.stageTitle = 'Результати експериментів';
      this.experimentIsProcessed = true;
    });
  }

  newExperiment() {
    this.router.navigate(['/']);
    this.experimentIsProcessing = false;
    this.experimentIsProcessed = false;
  }

  onOutletLoaded(component: SettingsMapComponent | ProgressComponent | StatisticsComponent) {
    if (component instanceof StatisticsComponent) {
      component.statistics = this.experimentStatistics;
    } else {
      component.settings = this.settings;
      this.mapComponent = component as SettingsMapComponent;
      /*this.ngForm.form.valueChanges.subscribe(x => {
        this.changeMapChart(component as SettingsMapComponent, x);
      })*/
    }
  }

  changeMapChart(form: NgForm) {
    if (this.mapComponent != undefined) {
      this.mapComponent.changeMapChart(this.settings, form.valid);
    }
  }

  onTasksChanged(input: number, val: EventTarget | null) {
    this.settings.tasksQuantity = this.removeLeadingZero(input);
    if (val) {
      (val as HTMLInputElement).value = this.removeLeadingZero(input).toString();
    }
  }

  onTargetsChanged(input: number, val: EventTarget | null) {
    this.settings.targetsQuantity = this.removeLeadingZero(input);
    if (val) {
      (val as HTMLInputElement).value = this.removeLeadingZero(input).toString();
    }
  }

  onTimeChanged(input: number, val: EventTarget | null) {
    this.settings.timeResource = this.removeLeadingZero(input);
    if (val) {
      (val as HTMLInputElement).value = this.removeLeadingZero(input).toString();
    }
  }

  onSpeedChanged(input: number, val: EventTarget | null) {
    this.settings.averageSpeed = this.removeLeadingZero(input);
    if (val) {
      (val as HTMLInputElement).value = this.removeLeadingZero(input).toString();
    }
  }

  removeLeadingZero(input: number): number {
    const stringWithoutLeadingZeros = input.toString().replace(/^0+/, '');
    const result = stringWithoutLeadingZeros === '' ? 0 : Number(stringWithoutLeadingZeros);
    return result;
  }

  average = (numbers: number[]) => this.sum(numbers) / numbers.length;

  sum = (numbers: number[]) => numbers.reduce((total, aNumber) => total + aNumber, 0);
}
