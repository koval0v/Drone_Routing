using DroneRouting.Algorithms;
using DroneRouting.Algorithms.Models.AlgorithmsTypes;
using DroneRouting.Experiments.Models;
using DroneRouting.Tasks;
using DroneRouting.Tasks.Models;

namespace DroneRouting.Experiments
{
    public class ExperimentsGenerator
    {
        private TasksSettings TasksSettings { get; set; }

        public ExperimentsGenerator(TasksSettings settings)
        {
            TasksSettings = settings;
        }

        public async Task<ExperimentStatistics> AnalyseExperimentResults()
        {
            List<ExperimentResult> results = LauchExperiments();
            ExperimentStatistics statistics = new() { TotalResults = results };

            List<double> tasksAverageDistances = new();
            List<(double distance, double targetsCount)> tasksBetterDistances = new();
            foreach (var experiment in results.Select(x => x.AlgorithmsResults))
            {
                tasksAverageDistances.Add(experiment.Select(x => x.RouteDistance).Average());
                TaskResult betterResult = experiment.FirstOrDefault(x => x.TheBestByTargetsAndDistance);
                tasksBetterDistances.Add((betterResult.RouteDistance, betterResult.Route.Count));
            }

            statistics.AverageDistance = tasksAverageDistances.Average();
            statistics.TheBestResultDistance = tasksBetterDistances.OrderBy(x => x.distance).ThenByDescending(x => x.targetsCount).FirstOrDefault().distance;

            statistics.Deviations.Add(CountDeviationsByAlgorithmType(statistics, TheNearestNeighbourTypes.Standard));
            statistics.Deviations.Add(CountDeviationsByAlgorithmType(statistics, TheNearestNeighbourTypes.InTheArea));
            statistics.Deviations.Add(CountDeviationsByAlgorithmType(statistics, TheNearestNeighbourTypes.Parallel));
            statistics.Deviations.Add(CountDeviationsByAlgorithmType(statistics, null, TheNearestToTheLineTypes.OrderingByX));
            statistics.Deviations.Add(CountDeviationsByAlgorithmType(statistics, null, TheNearestToTheLineTypes.OrderingByDistances));

            statistics.SquareDeviations.Add(CountDeviationsByAlgorithmType(statistics, TheNearestNeighbourTypes.Standard, null, true));
            statistics.SquareDeviations.Add(CountDeviationsByAlgorithmType(statistics, TheNearestNeighbourTypes.InTheArea, null, true));
            statistics.SquareDeviations.Add(CountDeviationsByAlgorithmType(statistics, TheNearestNeighbourTypes.Parallel, null, true));
            statistics.SquareDeviations.Add(CountDeviationsByAlgorithmType(statistics, null, TheNearestToTheLineTypes.OrderingByX, true));
            statistics.SquareDeviations.Add(CountDeviationsByAlgorithmType(statistics, null, TheNearestToTheLineTypes.OrderingByDistances, true));

            statistics.DynamicResults = LauchDynamicExperiment();

            return statistics;
        }

        private AlgorithmDeviations CountDeviationsByAlgorithmType(ExperimentStatistics statistics, TheNearestNeighbourTypes? neighbourType = null,
            TheNearestToTheLineTypes? lineType = null, bool isSquare = false)
        {
            AlgorithmDeviations deviations = new() { NeighbourType = neighbourType, LineType = lineType };
            List<double> tasksDistancesForMethod = new();
            foreach (var experiment in statistics.TotalResults.Select(x => x.AlgorithmsResults))
            {
                if (neighbourType != null)
                {
                    tasksDistancesForMethod.Add(experiment.FirstOrDefault(x => x.NeighbourType == neighbourType).RouteDistance);
                }
                if (lineType != null)
                {
                    tasksDistancesForMethod.Add(experiment.FirstOrDefault(x => x.LineType == lineType).RouteDistance);
                }
            }

            if (isSquare)
            {
                deviations.CommonAverageDeviation = Math.Sqrt(tasksDistancesForMethod.Average(x =>
                    Math.Pow(x - statistics.AverageDistance, 2)));
                deviations.TheBestResultDeviation = Math.Sqrt(tasksDistancesForMethod.Average(x =>
                    Math.Pow(x - statistics.TheBestResultDistance, 2)));
            }
            else
            {
                deviations.CommonAverageDeviation = tasksDistancesForMethod.Average(x => Math.Abs(x - statistics.AverageDistance));
                deviations.TheBestResultDeviation = tasksDistancesForMethod.Average(x => Math.Abs(x - statistics.TheBestResultDistance));
            }
            

            return deviations;
        }

        private List<ExperimentResult> LauchExperiments()
        {
            List<ExperimentResult> results = new();

            List<ExperimentTask> tasks = TasksGenerator.GenerateTasksSet(TasksSettings);

            foreach (var task in tasks)
            {
                Solver solver = new Solver(task.StartBase, task.EndBase, task.Targets);
                ExperimentResult experimentResults = solver.BuildRouteWithAllAlgorithms(task.TimeResource, task.AverageSpeed);
                List<TaskResult> maxTargetsResults = experimentResults.AlgorithmsResults.Where(x => x.Route.Count ==
                    experimentResults.AlgorithmsResults.Max(x => x.Route.Count)).ToList();
                List<TaskResult> minTargetsResults = experimentResults.AlgorithmsResults.Where(x => x.Route.Count ==
                    experimentResults.AlgorithmsResults.Min(x => x.Route.Count)).ToList();
                foreach (var targetResult in maxTargetsResults)
                {
                    targetResult.TheBestByTargets = true;
                }
                foreach (var targetResult in minTargetsResults)
                {
                    targetResult.TheWorstByTargets = true;
                }
                if (maxTargetsResults.Count > 1)
                {
                    List<TaskResult> minDistanceResults = maxTargetsResults.Where(x => x.RouteDistance ==
                        maxTargetsResults.Min(x => x.RouteDistance)).ToList();
                    foreach (var distanceResult in minDistanceResults)
                    {
                        distanceResult.TheBestByTargetsAndDistance = true;
                    }
                }
                else
                {
                    maxTargetsResults.FirstOrDefault().TheBestByTargetsAndDistance = true;
                }
                if (minTargetsResults.Count > 1)
                {
                    List<TaskResult> maxDistanceResults = minTargetsResults.Where(x => x.RouteDistance ==
                        minTargetsResults.Max(x => x.RouteDistance)).ToList();
                    foreach (var distanceResult in maxDistanceResults)
                    {
                        distanceResult.TheWorstByTargetsAndDistance = true;
                    }
                }
                else
                {
                    minTargetsResults.FirstOrDefault().TheWorstByTargetsAndDistance = true;
                }
                results.Add(experimentResults);
            }

            return results;
        }

        private List<DynamicTaskResult> LauchDynamicExperiment()
        {
            List<DynamicTaskResult> results = new();
            TasksSettings.TasksQuantity = 1;
            ExperimentTask task = TasksGenerator.GenerateTasksSet(TasksSettings).FirstOrDefault();

            for (int i = 1; i <= task.Targets.Count; i++)
            {
                Solver solver = new Solver(task.StartBase, task.EndBase, task.Targets.Take(i).ToList());
                ExperimentResult experimentResults = solver.BuildRouteWithAllAlgorithms(task.TimeResource, task.AverageSpeed);
                foreach (var result in experimentResults.AlgorithmsResults)
                {
                    results.Add(new DynamicTaskResult() { LineType = result.LineType, NeighbourType = result.NeighbourType,
                        MapTargetsCount = i, RouteTargetsCount = result.Route.Count});
                }
            }

            return results;
        }
    }
}
