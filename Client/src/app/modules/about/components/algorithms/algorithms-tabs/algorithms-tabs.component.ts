import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { AlgorithmTypes } from 'src/app/models/enums/AlgorithmTypes.model';
import { TheNearestNeighbourTypes } from 'src/app/models/enums/TheNearestNeighbourTypes.model';
import { TheNearestToTheLineTypes } from 'src/app/models/enums/TheNearestToTheLineTypes.model';

@Component({
  selector: 'app-algorithms-tabs',
  templateUrl: './algorithms-tabs.component.html',
  styleUrls: ['./algorithms-tabs.component.css']
})
export class AlgorithmsTabsComponent {
  type : AlgorithmTypes | undefined;
  TheNearestNeighbourTypes = TheNearestNeighbourTypes;
  TheNearestToTheLineTypes = TheNearestToTheLineTypes;
  @Output() changeSectionEvent = new EventEmitter<AlgorithmTypes>();

  constructor(public router: Router) {
    this.type = TheNearestNeighbourTypes.Standard;
  }

  changeSection(algorithmType: AlgorithmTypes | undefined){
    this.type = algorithmType;
    this.changeSectionEvent.emit(algorithmType);
  }
}
