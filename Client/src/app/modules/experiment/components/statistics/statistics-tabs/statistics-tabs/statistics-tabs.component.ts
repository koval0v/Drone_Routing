import { Component, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';
import { AlgorithmTypes } from 'src/app/models/enums/AlgorithmTypes.model';
import { TheNearestNeighbourTypes } from 'src/app/models/enums/TheNearestNeighbourTypes.model';
import { TheNearestToTheLineTypes } from 'src/app/models/enums/TheNearestToTheLineTypes.model';

@Component({
  selector: 'app-statistics-tabs',
  templateUrl: './statistics-tabs.component.html',
  styleUrls: ['./statistics-tabs.component.css']
})
export class StatisticsTabsComponent {
  type : AlgorithmTypes | undefined;
  TheNearestNeighbourTypes = TheNearestNeighbourTypes;
  TheNearestToTheLineTypes = TheNearestToTheLineTypes;
  @Output() changeSectionEvent = new EventEmitter<AlgorithmTypes>();

  constructor(public router: Router) { }

  changeSection(algorithmType: AlgorithmTypes | undefined){
    this.type = algorithmType;
    this.changeSectionEvent.emit(algorithmType);
  }
}
