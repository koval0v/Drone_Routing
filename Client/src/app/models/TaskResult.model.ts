import { Target } from "./Target.model";
import { TheNearestNeighbourTypes } from "./enums/TheNearestNeighbourTypes.model";
import { TheNearestToTheLineTypes } from "./enums/TheNearestToTheLineTypes.model";

export interface TaskResult {
  neighbourType? : TheNearestNeighbourTypes;
  lineType? : TheNearestToTheLineTypes;
  routeDistance : number;
  routeTime : number;
  route : Target[];
  executionTime : number;
  theBestByTargets : boolean;
  theBestByTargetsAndDistance : boolean;
  theWorstByTargets : boolean;
  theWorstByTargetsAndDistance : boolean;
}
