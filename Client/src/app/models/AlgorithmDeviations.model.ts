import { TheNearestNeighbourTypes } from "./enums/TheNearestNeighbourTypes.model";
import { TheNearestToTheLineTypes } from "./enums/TheNearestToTheLineTypes.model";

export interface AlgorithmDeviations {
  neighbourType : TheNearestNeighbourTypes;
  lineType : TheNearestToTheLineTypes;
  theBestResultDeviation : number;
  commonAverageDeviation : number;
}
