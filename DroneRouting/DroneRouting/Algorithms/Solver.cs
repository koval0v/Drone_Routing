using DroneRouting.Algorithms.Models.AlgorithmsTypes;
using DroneRouting.Algorithms.Models.Distances;
using DroneRouting.Experiments.Models;
using DroneRouting.Models.Targets;
using DroneRouting.Models.Topsis;
using System.Diagnostics;

namespace DroneRouting.Algorithms
{
    public class Solver
    {
        private Vehicle Start { get; set; }
        private Vehicle End { get; set; }
        private List<ExamTarget> Targets { get; set; }
        private List<DistanceMatrixElement> TargetsDistancesMatrix { get; set; }
        private List<DistanceToTheLineElement> TargetsDistancesToTheLine { get; set; } = new();

        public Solver(Vehicle startVehiclePosition, Vehicle endVehiclePosition, List<ExamTarget> examinationTargets)
        {
            Start = startVehiclePosition;
            End = endVehiclePosition;
            Targets = examinationTargets;

            TargetsDistancesMatrix = new();
            TargetsDistancesToTheLine = new();

            foreach (var target in new List<Target>(Targets) { Start, End })
            {
                TargetsDistancesMatrix.Add(new DistanceMatrixElement() { Target = target });
                TargetsDistancesToTheLine.Add(new DistanceToTheLineElement() { Target = target });
            }

            GenerateDistancesMatrix();
            GenerateDistancesToLineRow();
        }

        public ExperimentResult BuildRouteWithAllAlgorithms(double timeResource, double averageSpeed)
        {
            // automapper + delegate?
            ExperimentResult result = new();

            (TheNearestNeighbourTypes type1, double distance1, List<Target> resultRoute1, long time1) = (0, 0, null, 0);
            (type1, distance1, resultRoute1, time1) = BuildRouteAsTheNearestNeighbour(timeResource, averageSpeed);
            result.AlgorithmsResults.Add(new TaskResult()
            {
                NeighbourType = type1,
                RouteDistance = distance1,
                RouteTime = (distance1 / averageSpeed) * 60,
                Route = resultRoute1,
                ExecutionTime = time1
            });;
            (type1, distance1, resultRoute1, time1) = BuildRouteAsTheNearestNeighbourInTheArea(timeResource, averageSpeed);
            result.AlgorithmsResults.Add(new TaskResult()
            {
                NeighbourType = type1,
                RouteDistance = distance1,
                RouteTime = (distance1 / averageSpeed) * 60,
                Route = resultRoute1,
                ExecutionTime = time1
            });
            (type1, distance1, resultRoute1, time1) = BuildRouteAsTheParallelNearestNeighbour(timeResource, averageSpeed);
            result.AlgorithmsResults.Add(new TaskResult()
            {
                NeighbourType = type1,
                RouteDistance = distance1,
                RouteTime = (distance1 / averageSpeed) * 60,
                Route = resultRoute1,
                ExecutionTime = time1
            });

            (TheNearestToTheLineTypes type2, double distance2, List<Target> resultRoute2, long time2) = (0, 0, null, 0);
            (type2, distance2, resultRoute2, time2) = BuildRouteAsTheNearestToTheLine(timeResource, averageSpeed);
            result.AlgorithmsResults.Add(new TaskResult()
            {
                LineType = type2,
                RouteDistance = distance2,
                RouteTime = (distance2 / averageSpeed) * 60,
                Route = resultRoute2,
                ExecutionTime = time2
            });
            (type2, distance2, resultRoute2, time2) = BuildRouteAsTheNearestToTheLine(timeResource, averageSpeed, TheNearestToTheLineTypes.OrderingByDistances);
            result.AlgorithmsResults.Add(new TaskResult()
            {
                LineType = type2,
                RouteDistance = distance2,
                RouteTime = (distance2 / averageSpeed) * 60,
                Route = resultRoute2,
                ExecutionTime = time2
            });

            return result;
        }

        public (TheNearestToTheLineTypes type, double distance, List<Target> route, long executionTime) BuildRouteAsTheNearestToTheLine
            (double timeResource, double averageSpeed, TheNearestToTheLineTypes type = TheNearestToTheLineTypes.OrderingByX)
        {
            TargetsDistancesToTheLine.ForEach(x => x.IsAdded = false);
            double lMax = timeResource * averageSpeed;
            List<Target> route = new List<Target>() { Start, End };
            bool isEnded = false;
            double checkDistance = 0;

            Stopwatch watch = new();
            watch = Stopwatch.StartNew();
            while (!isEnded)
            {
                Target theNearestLinePoint = TargetsDistancesToTheLine.Where(x => x.Distance != 0 && !x.IsAdded)
                    .OrderBy(x => x.Distance).FirstOrDefault().Target;

                if (type == TheNearestToTheLineTypes.OrderingByDistances)
                {
                    if (route.Count != 2)
                    {
                        Dictionary<Target, double> matrixRow = TargetsDistancesMatrix.FirstOrDefault(x => x.Target == theNearestLinePoint)
                            .SecondTargetDistance;
                        Target theNearestPointToThePoint = matrixRow.AsEnumerable().OrderBy(x => x.Value).FirstOrDefault(x => x.Value != 0 &&
                            route.Exists(y => y == x.Key)).Key;

                        if (theNearestPointToThePoint == Start)
                        {
                            route.Insert(1, theNearestLinePoint);
                        }
                        else
                        {
                            route.Insert(route.IndexOf(theNearestPointToThePoint), theNearestLinePoint);
                        }
                    }
                    else
                    {
                        route.Insert(1, theNearestLinePoint);
                    }
                }
                else
                {
                    if (route.Count != 2)
                    {
                        Target rightTargetsOfThePoint = route.OrderBy(x => x.X).FirstOrDefault(x => x != Start && x != End && x.X > theNearestLinePoint.X);
                        if (rightTargetsOfThePoint != null)
                        {
                            route.Insert(route.IndexOf(rightTargetsOfThePoint), theNearestLinePoint);
                        }
                        else
                        {
                            route.Insert(route.Count - 1, theNearestLinePoint);
                        }
                    }
                    else
                    {
                        route.Insert(1, theNearestLinePoint);
                    }
                }

                TargetsDistancesToTheLine.FirstOrDefault(x => x.Target == theNearestLinePoint).IsAdded = true;

                checkDistance = CountDistanceOfTheRoute(route);
                if (checkDistance > lMax)
                {
                    route.Remove(theNearestLinePoint);
                }

                if (TargetsDistancesToTheLine.Where(x => x.Distance != 0).All(x => x.IsAdded))
                {
                    isEnded = true;
                }
            }
            watch.Stop();

            double finalDistance = CountDistanceOfTheRoute(route);

            if (route.Count != route.Distinct().Count())
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("BuildRouteAsTheNearestToTheLine");
                Console.BackgroundColor = ConsoleColor.White;
            }

            return (type, finalDistance, route, watch.ElapsedMilliseconds);
        }

        public (TheNearestNeighbourTypes type, double distance, List<Target> route, long executionTime) BuildRouteAsTheNearestNeighbour
            (double timeResource, double averageSpeed, TheNearestNeighbourTypes type = TheNearestNeighbourTypes.Standard)
        {
            ResetVisitedTargets();

            double lMax = timeResource * averageSpeed;
            List<Target> route = new List<Target>() { Start };
            double l = 0, distanceToEndVehicle = 0;
            Target currentPosition = Start;
            Target theBestNearest = null!;
            bool isEnded = false;

            MarkTargetAsVisited(Start);

            Stopwatch watch = new();
            watch = Stopwatch.StartNew();
            while (!isEnded)
            {
                Dictionary<Target, double> matrixRow = TargetsDistancesMatrix
                    .FirstOrDefault(x => x.Target == currentPosition).SecondTargetDistance;

                var searchingList = matrixRow.Where(x => !x.Key.isVisited);

                if (type == TheNearestNeighbourTypes.InTheArea)
                {
                    searchingList = searchingList.Where(x => !x.Key.IsExcluded);
                }

                double theNearestNeighbourDistance = Math.Round(searchingList
                        .Select(o => o.Value).Where(x => x != 0)
                        .OrderBy(x => x).FirstOrDefault(), 2);

                List<Target> theNearestNeighbours = searchingList
                    .Where(x => Math.Round(x.Value, 2) == theNearestNeighbourDistance)
                    .Select(x => x.Key).ToList();

                if (theNearestNeighbours.Count > 1)
                {
                    List<Target> allAlternatives = new List<Target>(Targets) { Start, End };
                    theBestNearest = SortByTopsis(theNearestNeighbours, allAlternatives);
                }
                else
                {
                    theBestNearest = theNearestNeighbours.FirstOrDefault();
                }

                if (route.Count > 1)
                {
                    l = CountDistanceOfTheRoute(route);
                }

                if (theBestNearest == End)
                {
                    /*Dictionary<Target, double> matrixRowForEnd = TargetsDistancesMatrix.FirstOrDefault(x => x.Target == End).SecondTargetDistance;*/

            double nextTheNearestNeighbourDistance = Math.Round(searchingList.Where(x => x.Key != End)
                       .Select(o => o.Value).Where(x => x != 0)
                       .OrderBy(x => x).FirstOrDefault(), 2);
                    Target nextTheNearestNeighbour = searchingList
                       .Where(x => Math.Round(x.Value, 2) == nextTheNearestNeighbourDistance)
                       .Select(x => x.Key).ToList().FirstOrDefault();

                    if (nextTheNearestNeighbour is not null && nextTheNearestNeighbour != route[route.Count - 1])
                    {
                        Dictionary<Target, double> matrixRowForTheNextNearest = TargetsDistancesMatrix
                            .FirstOrDefault(x => x.Target == nextTheNearestNeighbour).SecondTargetDistance;
                        double nextTheNearestNeighbourDistanceToEndVehicle = matrixRowForTheNextNearest.FirstOrDefault(o => o.Key == End).Value;

                        if (l + nextTheNearestNeighbourDistance + nextTheNearestNeighbourDistanceToEndVehicle <= lMax)
                        {
                            theBestNearest = nextTheNearestNeighbour;
                        }
                    }
                }

                Dictionary<Target, double> matrixRowForTheNearest = TargetsDistancesMatrix
                    .FirstOrDefault(x => x.Target == theBestNearest).SecondTargetDistance;

                distanceToEndVehicle = matrixRowForTheNearest.FirstOrDefault(o => o.Key == End).Value;

                if (l + theNearestNeighbourDistance + distanceToEndVehicle <= lMax)
                {
                    MarkTargetAsVisited(currentPosition);
                    route.Add(theBestNearest);
                    currentPosition = theBestNearest;
                }
                else
                {
                    MarkTargetAsVisited(End);
                    route.Add(End);
                    currentPosition = End;
                }

                if (currentPosition == End)
                {
                    isEnded = true;
                }

                bool allIsVisited = TargetsDistancesMatrix.SelectMany(x => x.SecondTargetDistance.Keys).All(x => x.isVisited);

                if (allIsVisited)
                {
                    isEnded = true;
                }
            }
            watch.Stop();

            double finalDistance = CountDistanceOfTheRoute(route);

            if (route.Count != route.Distinct().Count())
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("BuildRouteAsTheNearestNeighbour");
                Console.BackgroundColor = ConsoleColor.White;
            }

            return (type, finalDistance, route, watch.ElapsedMilliseconds);
        }

        public (TheNearestNeighbourTypes type, double distance, List<Target> route, long executionTime)BuildRouteAsTheNearestNeighbourInTheArea
            (double timeResource, double averageSpeed)
        {
            ResetVisitedTargets();

            (TheNearestNeighbourTypes type, double distance, List<Target> route, long executionTime) result = (0, 0, new(), 0);
            (TheNearestNeighbourTypes type, double distance, List<Target> route, long executionTime) newResult = new();

            Stopwatch watch = new();
            watch = Stopwatch.StartNew();
            (int lineX, int lineY, int lineA) = CountLineParameters(Start, End);
            foreach (var target in TargetsDistancesToTheLine)
            {
                target.Distance = CountDistanceFromPointToLine(lineX, lineY, lineA, target.Target.X, target.Target.Y);
            }

            double distanceToTheNearest = TargetsDistancesToTheLine.Where(x => x.Distance != 0).OrderBy(x => x.Distance).FirstOrDefault().Distance;
            double maxDelta = TargetsDistancesToTheLine.Where(x => x.Distance != 0).OrderByDescending(x => x.Distance).FirstOrDefault().Distance;
            double deltaPercent = distanceToTheNearest / maxDelta + 0.05;

            do
            {
                double delta = maxDelta * deltaPercent;

                foreach (var target in TargetsDistancesMatrix.Select(x => x.Target))
                {
                    double distanceToTheLine = TargetsDistancesToTheLine.Where(x => x.Target.Equals(target)).FirstOrDefault().Distance;

                    if (distanceToTheLine <= delta)
                    {
                        target.IsExcluded = false;
                    }
                    else
                    {
                        target.IsExcluded = true;
                    }
                }

                newResult = BuildRouteAsTheNearestNeighbour(timeResource, averageSpeed,
                    TheNearestNeighbourTypes.InTheArea);

                if (newResult.route.Count < result.route.Count)
                {
                    return result;
                }

                if (newResult.route.Count == result.route.Count &&
                    newResult.distance > result.distance)
                {
                    return result;
                }

                deltaPercent += 0.05;
                result = newResult;

            } while (newResult.route.Count >= result.route.Count && deltaPercent <= 1.0);
            watch.Stop();
            newResult.executionTime = watch.ElapsedMilliseconds;
            return newResult;
        }

        public (TheNearestNeighbourTypes type, double distance, List<Target> route, long executionTime) BuildRouteAsTheParallelNearestNeighbour
            (double timeResource, double averageSpeed)
        {
            ResetVisitedTargets();

            double lMax = timeResource * averageSpeed;
            List<Target> route = new List<Target>() { Start, End };

            Target currentPosition = null!;
            Target theBestNearest = null!;
            int iterationNumber = 1;
            bool isEnded = false;

            Target theLastAddedFromEnd = End, theLastAddedFromStart = Start;

            MarkTargetAsVisited(Start);
            MarkTargetAsVisited(End);

            Stopwatch watch = new();
            watch = Stopwatch.StartNew();
            while (!isEnded)
            {
                if (iterationNumber % 2 == 0)
                {
                    currentPosition = theLastAddedFromEnd;
                }
                else
                {
                    currentPosition = theLastAddedFromStart;
                }

                Dictionary<Target, double> matrixRow = TargetsDistancesMatrix.FirstOrDefault(x => x.Target == currentPosition).SecondTargetDistance;

                var searchingList = matrixRow.Where(x => !x.Key.isVisited);

                double theNearestNeighbourDistance = Math.Round(searchingList
                        .Select(o => o.Value).Where(x => x != 0)
                        .OrderBy(x => x).FirstOrDefault(), 2);

                List<Target> theNearestNeighbours = searchingList.Where(x => Math.Round(x.Value, 2) == theNearestNeighbourDistance)
                    .Select(x => x.Key).ToList();

                if (theNearestNeighbours.Count > 1)
                {
                    List<Target> allAlternatives = new List<Target>(Targets) { Start, End };
                    theBestNearest = SortByTopsis(theNearestNeighbours, allAlternatives);
                }
                else
                {
                    theBestNearest = theNearestNeighbours.FirstOrDefault();
                }

                if (currentPosition == End)
                {
                    route.Insert(route.IndexOf(theLastAddedFromEnd), theBestNearest);
                    theLastAddedFromEnd = theBestNearest;
                }
                else
                {
                    route.Insert(route.IndexOf(theLastAddedFromStart) + 1, theBestNearest);
                    theLastAddedFromStart = theBestNearest;
                }

                MarkTargetAsVisited(theBestNearest);

                double checkDistance = CountDistanceOfTheRoute(route);

                if (checkDistance >= lMax)
                {
                    route.Remove(theBestNearest);
                    isEnded = true;
                }

                if (route.Count == Targets.Count + 2)
                {
                    isEnded = true;
                }

                iterationNumber++;
            }
            watch.Stop();

            double finalDistance = CountDistanceOfTheRoute(route);

            if (route.Count != route.Distinct().Count())
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("BuildRouteAsTheParallelNearestNeighbour");
                Console.BackgroundColor = ConsoleColor.White;
            }

            return (TheNearestNeighbourTypes.Parallel, finalDistance, route, watch.ElapsedMilliseconds);
        }

        private (int A, int B, int C) CountLineParameters(Target StartPoint, Target EndPoint)
        {
            int lineX = EndPoint.Y - StartPoint.Y;
            int lineY = -(EndPoint.X - StartPoint.X);
            int lineA = -StartPoint.X * (EndPoint.Y - StartPoint.Y) + StartPoint.Y * (EndPoint.X - StartPoint.X);
            return (lineX, lineY, lineA);
        }

        private double CountDistanceFromPointToLine(int lineX, int lineY, int lineA, int pointX, int pointY)
        {
            return Math.Round(Math.Abs(lineX * pointX + lineY * pointY + lineA) / Math.Sqrt(Math.Pow(lineX, 2)
                + Math.Pow(lineY, 2)), 3);
        }

        private Target SortByTopsis(List<Target> alternatives, List<Target> allTargets)
        {
            double xWeight = 0.5, yWeight = 0.5;

            List<Alternative> topsisAlternatives = new();

            double allTargetsAverageX = Math.Sqrt(allTargets.Sum(x => x.X * x.X));
            double allTargetsAverageY = Math.Sqrt(allTargets.Sum(x => x.Y * x.Y));

            foreach (var target in allTargets)
            {
                topsisAlternatives.Add(new Alternative(target)
                {
                    XValue = target.X / allTargetsAverageX,
                    YValue = target.Y / allTargetsAverageY
                });
            }

            foreach (var alternative in topsisAlternatives)
            {
                alternative.XValue *= xWeight;
                alternative.YValue *= yWeight;
            }

            double xPIS = topsisAlternatives.OrderBy(x => x.XValue).LastOrDefault().XValue;
            double yPIS = topsisAlternatives.OrderBy(x => x.YValue).LastOrDefault().YValue;
            double xNIS = topsisAlternatives.OrderBy(x => x.XValue).FirstOrDefault().XValue;
            double yNIS = topsisAlternatives.OrderBy(x => x.YValue).FirstOrDefault().YValue;

            foreach (var alternative in topsisAlternatives)
            {
                alternative.PISDistance = DistanceBetweenPoints(alternative.XValue, alternative.YValue, xPIS, yPIS);
                alternative.NISDistance = DistanceBetweenPoints(alternative.XValue, alternative.YValue, xNIS, yNIS);
            }

            foreach (var alternative in topsisAlternatives)
            {
                alternative.PISProximity = alternative.NISDistance / (alternative.NISDistance + alternative.PISDistance);
            }

            return topsisAlternatives.Where(x => alternatives.Any(y => y == x.Target)).OrderByDescending(x => x.PISProximity).LastOrDefault().Target;
        }

        private double DistanceBetweenPoints(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        private void GenerateDistancesMatrix()
        {
            foreach (var target in TargetsDistancesMatrix)
            {
                foreach (var t in Targets)
                {
                    target.SecondTargetDistance.Add(t, DistanceBetweenPoints(target.Target.X, target.Target.Y, t.X, t.Y));
                }
                target.SecondTargetDistance.Add(Start, DistanceBetweenPoints(target.Target.X, target.Target.Y, Start.X, Start.Y));
                target.SecondTargetDistance.Add(End, DistanceBetweenPoints(target.Target.X, target.Target.Y, End.X, End.Y));
            }
        }

        private void GenerateDistancesToLineRow()
        {
            (int lineX, int lineY, int lineA) = CountLineParameters(Start, End);

            foreach (var target in TargetsDistancesToTheLine)
            {
                target.Distance = CountDistanceFromPointToLine(lineX, lineY, lineA, target.Target.X, target.Target.Y);
            }
        }

        private void ResetVisitedTargets()
        {
            foreach (var target in TargetsDistancesMatrix)
            {
                foreach (var key in target.SecondTargetDistance.Keys)
                {
                    key.isVisited = false;
                }
            }
        }

        private void MarkTargetAsVisited(Target currentPosition)
        {
            foreach (var target in TargetsDistancesMatrix)
            {
                foreach (var key in target.SecondTargetDistance.Keys)
                {
                    if (key == currentPosition)
                    {
                        key.isVisited = true;
                    }
                }
            }
        }

        private double CountDistanceOfTheRoute(List<Target> route)
        {
            double distance = 0;
            for (int i = 0; i < route.Count - 1; i++)
            {
                Dictionary<Target, double> targetRow = TargetsDistancesMatrix.Where(x => x.Target == route[i])
                    .Select(x => x.SecondTargetDistance).FirstOrDefault();
                distance += targetRow.Where(x => x.Key == route[i + 1]).FirstOrDefault().Value;
            }
            return distance;
        }
    }
}