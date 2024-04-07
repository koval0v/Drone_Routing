import { TheNearestNeighbourTypes } from "./enums/TheNearestNeighbourTypes.model";
import { TheNearestToTheLineTypes } from "./enums/TheNearestToTheLineTypes.model";

export interface DynamicTaskResult {
  neighbourType? : TheNearestNeighbourTypes;
  lineType? : TheNearestToTheLineTypes;
  mapTargetsCount : number;
  routeTargetsCount : number;
}
