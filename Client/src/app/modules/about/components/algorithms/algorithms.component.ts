import { Component } from '@angular/core';
import { getAlgorithmsDescription } from './getDescription';
import { TheNearestNeighbourTypes } from 'src/app/models/enums/TheNearestNeighbourTypes.model';

@Component({
  selector: 'app-algorithms',
  templateUrl: './algorithms.component.html',
  styleUrls: ['./algorithms.component.css']
})
export class AlgorithmsComponent {
  description: string = "";

  constructor() {
    this.description = getAlgorithmsDescription(TheNearestNeighbourTypes.Standard);
  }

  changeAlgorithm(algorithm: any) {
    this.description = getAlgorithmsDescription(algorithm);
  }
}
