import { AlgorithmDeviations } from "./AlgorithmDeviations.model";
import { DynamicTaskResult } from "./DynamicTaskResult.model";
import { ExperimentResult } from "./ExperimentResult.model";

export interface ExperimentStatistics {
  averageSpeed : number;
  timeResource : number;
  totalResults : ExperimentResult[];
  dynamicResults : DynamicTaskResult[];
  averageTargetsCount : number;
  targetsCount : number;
  averageDistance : number;
  theBestResultDistance : number;
  deviations : AlgorithmDeviations[];
  squareDeviations : AlgorithmDeviations[];
  minExecutionTime : number;
  minExecutionTimeMethodName : string | undefined;
  avgExecutionTime : number;
  avgExecutionTimeMethodName : string | undefined;
  maxExecutionTime : number;
  maxExecutionTimeMethodName : string | undefined;
}
