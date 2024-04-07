using DroneRouting.Algorithms;
using DroneRouting.Algorithms.Models.AlgorithmsTypes;
using DroneRouting.Experiments;
using DroneRouting.Experiments.Models;
using DroneRouting.Models.Targets;
using DroneRouting.Tasks;
using DroneRouting.Tasks.Models;
using System.Text.Json;

namespace DroneRoutingConsole
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            //List<ExperimentTask> tasks = TasksGenerator.GenerateTasksSet(new TasksSettings()
            //{
            //    AverageSpeed = 10,
            //    TimeResource = 1,
            //    TargetsQuantity = 5,
            //    TasksQuantity = 8,
            //    DistributionIsUniform = true,
            //    General = new Coordinates()
            //    {
            //        XRange = new DroneRouting.Tasks.Models.Range()
            //        {
            //            StartCoordinate = 5,
            //            EndCoordinate = 25
            //        },
            //        YRange = new DroneRouting.Tasks.Models.Range()
            //        {
            //            StartCoordinate = 0,
            //            EndCoordinate = 20
            //        },
            //    },
            //    StartBase = new Coordinates()
            //    {
            //        XRange = new DroneRouting.Tasks.Models.Range()
            //        {
            //            StartCoordinate = 7,
            //            EndCoordinate = 11
            //        },
            //        YRange = new DroneRouting.Tasks.Models.Range()
            //        {
            //            StartCoordinate = 3,
            //            EndCoordinate = 16
            //        },
            //    },
            //    EndBase = new Coordinates()
            //    {
            //        XRange = new DroneRouting.Tasks.Models.Range()
            //        {
            //            StartCoordinate = 14,
            //            EndCoordinate = 19
            //        },
            //        YRange = new DroneRouting.Tasks.Models.Range()
            //        {
            //            StartCoordinate = 7,
            //            EndCoordinate = 17
            //        },
            //    },
            //});

            var taskSettings = new TasksSettings()
            {
                AverageSpeed = 10,
                TimeResource = 1,
                TargetsQuantity = 5,
                TasksQuantity = 8,
                DistributionIsUniform = true,
                General = new Coordinates()
                {
                    XRange = new DroneRouting.Tasks.Models.Range()
                    {
                        StartCoordinate = 5,
                        EndCoordinate = 25
                    },
                    YRange = new DroneRouting.Tasks.Models.Range()
                    {
                        StartCoordinate = 0,
                        EndCoordinate = 20
                    },
                },
                StartBase = new Coordinates()
                {
                    XRange = new DroneRouting.Tasks.Models.Range()
                    {
                        StartCoordinate = 7,
                        EndCoordinate = 11
                    },
                    YRange = new DroneRouting.Tasks.Models.Range()
                    {
                        StartCoordinate = 3,
                        EndCoordinate = 16
                    },
                },
                EndBase = new Coordinates()
                {
                    XRange = new DroneRouting.Tasks.Models.Range()
                    {
                        StartCoordinate = 14,
                        EndCoordinate = 19
                    },
                    YRange = new DroneRouting.Tasks.Models.Range()
                    {
                        StartCoordinate = 7,
                        EndCoordinate = 17
                    },
                },
            };

            string strJson = JsonSerializer.Serialize<TasksSettings>(taskSettings);
            Console.WriteLine(strJson);

            ExperimentStatistics statistics = await new ExperimentsGenerator(taskSettings).AnalyseExperimentResults();

            Console.WriteLine("=========");

            Console.WriteLine(statistics);

            int neighbourIsBest = 0, lineIsBest = 0, neighbourLineIsBest = 0, neighbourAreaIsBest = 0, neighbourParallelIsBest = 0;
            double timeResource = 0, averageSpeed = 10;
            Vehicle startVehicle, endVehicle;
            List<ExamTarget> targets = new();

            //string[] lines = File.ReadAllLines(@"C:\Users\UserPc\Desktop\4course\8sem\Diploma\Application\DroneRouting\input-data.txt");

            //int i = 1;
            //int targetsCount = 0;
            //int tasksAdded = 0;

            //while (tasksAdded != 100)
            //{
            //    targets = new();
            //    targetsCount = Convert.ToInt32(lines[i].Split(' ')[0].ToString());
            //    i++;
            //    startVehicle = new Vehicle(Convert.ToInt32(lines[i].Split(' ')[0].ToString()), Convert.ToInt32(lines[i].Split(' ')[1].ToString()), "Start");
            //    i++;
            //    endVehicle = new Vehicle(Convert.ToInt32(lines[i].Split(' ')[0].ToString()), Convert.ToInt32(lines[i].Split(' ')[1].ToString()), "End");
            //    i++;
            //    timeResource = Convert.ToInt32(lines[i].Split(' ')[0].ToString());

            //    Console.WriteLine($"Start vehicle: ({startVehicle.X}, {startVehicle.Y})");
            //    Console.WriteLine($"End vehicle: ({endVehicle.X}, {endVehicle.Y})");
            //    Console.WriteLine($"Time resource: {timeResource} -> Distance resource {timeResource * averageSpeed}");
            //    tasksAdded++;

            //    i++;
            //    int targetsRead = 0;
            //    while (targetsRead != targetsCount)
            //    {
            //        targets.Add(new ExamTarget(Convert.ToInt32(lines[i].Split(' ')[0].ToString()),
            //            Convert.ToInt32(lines[i].Split(' ')[1].ToString()), $"Target #{targetsRead + 1}"));
            //        i++;
            //        targetsRead++;
            //    }

            //    Console.WriteLine("Targets:");
            //    foreach (var tar in targets)
            //    {
            //        Console.WriteLine($"Target: ({tar.X}, {tar.Y})");
            //    }

            //    i ++;

            int targetsCount = 0;
            int tasksAdded = 0;

            int taskNum = 1;

            //foreach (var task in tasks)
            //{
            //    Console.WriteLine($"TASK #{taskNum}");
            //    targets = new();
            //    targetsCount = task.Targets.Count;
            //    startVehicle = task.StartBase;
            //    endVehicle = task.EndBase;
            //    timeResource = task.TimeResource;
            //    averageSpeed = task.AverageSpeed;

            //    Console.WriteLine($"Start vehicle: ({startVehicle.X}, {startVehicle.Y})");
            //    Console.WriteLine($"End vehicle: ({endVehicle.X}, {endVehicle.Y})");
            //    Console.WriteLine($"Time resource: {timeResource} -> Distance resource {timeResource * averageSpeed}");
            //    tasksAdded++;

            //    foreach (var t in task.Targets)
            //    {
            //        targets.Add(t);
            //    }

            //    Console.WriteLine("Targets:");
            //    foreach (var tar in targets)
            //    {
            //        Console.WriteLine($"Target: ({tar.X}, {tar.Y})");
            //    }

            //    Solver solver = new Solver(startVehicle, endVehicle, targets);

            //    // the nearest neighbour
            //    (var type1, double TheNearestNeighbour, List<Target> result1) = solver.BuildRouteAsTheNearestNeighbour(timeResource, averageSpeed);

            //    Console.WriteLine($"-------------- THE NEAREST NEIGHBOUR -> {type1} -------------");
            //    Console.WriteLine(String.Format("Route distance: {0:0.00} km", TheNearestNeighbour));
            //    Console.WriteLine(String.Format("Route time: {0:0.00} min", (TheNearestNeighbour / averageSpeed) * 60));
            //    Console.WriteLine($"Targets: {result1.Count() - 2}");
            //    Console.WriteLine("--------------------------------------------------");

            //    if (Math.Round(TheNearestNeighbour, 2) > Math.Round(timeResource * 60, 2))
            //    {
            //        Console.BackgroundColor = ConsoleColor.Yellow;
            //        Console.WriteLine($"{Math.Round(TheNearestNeighbour, 2)} > {Math.Round(timeResource * 60, 2)}");
            //        Console.WriteLine("ERROR");
            //        Console.BackgroundColor = ConsoleColor.Black;
            //    }

            //    Console.WriteLine(result1.Count() - 2);

            //    foreach (var t in result1)
            //    {
            //        Console.WriteLine($"{t.X} {t.Y}");
            //    }

            //    // the nearest line target (x order)
            //    (var type2, double TheNearestLineTargetsWithX, List<Target> result2) = solver.BuildRouteAsTheNearestToTheLine(timeResource,
            //        averageSpeed, TheNearestToTheLineTypes.OrderingByX);

            //    Console.WriteLine($"------ THE NEAREST TARGET TO LINE (X order) -> {type2} ------");
            //    Console.WriteLine(String.Format("Route distance: {0:0.00} km", TheNearestLineTargetsWithX));
            //    Console.WriteLine(String.Format("Route time: {0:0.00} min", (TheNearestLineTargetsWithX / averageSpeed) * 60));
            //    Console.WriteLine($"Targets: {result2.Count() - 2}");
            //    Console.WriteLine("--------------------------------------------------");

            //    if (Math.Round(TheNearestLineTargetsWithX, 2) > Math.Round(timeResource * 60, 2))
            //    {
            //        Console.BackgroundColor = ConsoleColor.Yellow;
            //        Console.WriteLine($"{Math.Round(TheNearestLineTargetsWithX, 2)} > {Math.Round(timeResource * 60, 2)}");
            //        Console.WriteLine("ERROR");
            //        Console.BackgroundColor = ConsoleColor.Black;
            //    }

            //    Console.WriteLine(result2.Count() - 2);

            //    foreach (var t in result2)
            //    {
            //        Console.WriteLine($"{t.X} {t.Y}");
            //    }


            //    // the nearest line target (neighbour order)
            //    (var type3, double TheNearestLineTargetsWithMatrix, List<Target> result3) = solver.BuildRouteAsTheNearestToTheLine(timeResource,
            //        averageSpeed, TheNearestToTheLineTypes.OrderingByDistances);

            //    Console.WriteLine($"------ THE NEAREST TARGET TO LINE (Matrix order) -> {type3} ------");
            //    Console.WriteLine(String.Format("Route distance: {0:0.00} km", TheNearestLineTargetsWithMatrix));
            //    Console.WriteLine(String.Format("Route time: {0:0.00} min", (TheNearestLineTargetsWithMatrix / averageSpeed) * 60));
            //    Console.WriteLine($"Targets: {result3.Count() - 2}");
            //    Console.WriteLine("--------------------------------------------------");

            //    if (TheNearestLineTargetsWithMatrix > timeResource * 60)
            //    {
            //        Console.BackgroundColor = ConsoleColor.Blue;
            //        Console.WriteLine("ERROR");
            //        Console.BackgroundColor = ConsoleColor.Black;
            //    }

            //    Console.WriteLine(result3.Count() - 2);

            //    foreach (var t in result3)
            //    {
            //        Console.WriteLine($"{t.X} {t.Y}");
            //    }

            //    // the nearest neighbour with limited area
            //    (var type4, double TheNearestNeighbourArea, List<Target> result4) = solver.BuildRouteAsTheNearestNeighbourInTheArea(timeResource, averageSpeed);

            //    Console.WriteLine($"-------------- THE NEAREST NEIGHBOUR (AREA) -> {type4} -------------");
            //    Console.WriteLine(String.Format("Route distance: {0:0.00} km", TheNearestNeighbourArea));
            //    Console.WriteLine(String.Format("Route time: {0:0.00} min", (TheNearestNeighbourArea / averageSpeed) * 60));
            //    Console.WriteLine($"Targets: {result4.Count() - 2}");
            //    Console.WriteLine("--------------------------------------------------");

            //    if (Math.Round(TheNearestNeighbourArea, 2) > Math.Round(timeResource * 60, 2))
            //    {
            //        Console.BackgroundColor = ConsoleColor.Yellow;
            //        Console.WriteLine($"{Math.Round(TheNearestNeighbourArea, 2)} > {Math.Round(timeResource * 60, 2)}");
            //        Console.WriteLine("ERROR");
            //        Console.BackgroundColor = ConsoleColor.Black;
            //    }

            //    Console.WriteLine(result4.Count() - 2);

            //    foreach (var t in result4)
            //    {
            //        Console.WriteLine($"{t.X} {t.Y}");
            //    }

            //    // the nearest neighbour parallel
            //    (var type5, double TheNearestNeighbourParallel, List<Target> result5) = solver.BuildRouteAsTheParallelNearestNeighbour(timeResource, averageSpeed);

            //    Console.WriteLine($"-------------- THE NEAREST NEIGHBOUR (AREA) -> {type5} -------------");
            //    Console.WriteLine(String.Format("Route distance: {0:0.00} km", TheNearestNeighbourParallel));
            //    Console.WriteLine(String.Format("Route time: {0:0.00} min", (TheNearestNeighbourParallel / averageSpeed) * 60));
            //    Console.WriteLine($"Targets: {result5.Count() - 2}");
            //    Console.WriteLine("--------------------------------------------------");

            //    if (Math.Round(TheNearestNeighbourParallel, 2) > Math.Round(timeResource * 60, 2))
            //    {
            //        Console.BackgroundColor = ConsoleColor.Yellow;
            //        Console.WriteLine($"{Math.Round(TheNearestNeighbourParallel, 2)} > {Math.Round(timeResource * 60, 2)}");
            //        Console.WriteLine("ERROR");
            //        Console.BackgroundColor = ConsoleColor.Black;
            //    }

            //    Console.WriteLine(result5.Count() - 2);

            //    foreach (var t in result5)
            //    {
            //        Console.WriteLine($"{t.X} {t.Y}");
            //    }

            //    int theBestResult = (new List<int>() { result1.Count(), result2.Count(), result3.Count(),
            //        result4.Count(), result5.Count() }).OrderBy(x => x).LastOrDefault();
            //    IEnumerable<List<Target>> theBestResultLists = new List<List<Target>>() { result1, result2, result3, result4, result5 }
            //        .Where(x => x.Count() == theBestResult);

            //    Console.WriteLine(theBestResultLists.Count());

            //    if (theBestResultLists.Count() == 1)
            //    {
            //        if (theBestResult == result1.Count())
            //        {
            //            neighbourIsBest += 1;
            //        }
            //        if (theBestResult == result2.Count())
            //        {
            //            lineIsBest += 1;
            //        }
            //        if (theBestResult == result3.Count())
            //        {
            //            neighbourLineIsBest += 1;
            //        }
            //        if (theBestResult == result4.Count())
            //        {
            //            neighbourAreaIsBest += 1;
            //        }
            //        if (theBestResult == result5.Count())
            //        {
            //            neighbourParallelIsBest += 1;
            //        }
            //    }
            //    else
            //    {
            //        double theBestResultTime = (new List<double>() { TheNearestNeighbour, TheNearestLineTargetsWithX, TheNearestLineTargetsWithMatrix,
            //            TheNearestNeighbourArea, TheNearestNeighbourParallel }).OrderBy(x => x).LastOrDefault();
            //        if (theBestResultTime == TheNearestNeighbour)
            //        {
            //            neighbourIsBest += 1;
            //        }
            //        if (theBestResultTime == TheNearestLineTargetsWithX)
            //        {
            //            lineIsBest += 1;
            //        }
            //        if (theBestResultTime == TheNearestLineTargetsWithMatrix)
            //        {
            //            neighbourLineIsBest += 1;
            //        }
            //        if (theBestResultTime == TheNearestNeighbourArea)
            //        {
            //            neighbourAreaIsBest += 1;
            //        }
            //        if (theBestResultTime == TheNearestNeighbourParallel)
            //        {
            //            neighbourParallelIsBest += 1;
            //        }
            //        taskNum++;
            //    }
            //}

            //Console.WriteLine($"!=!=!=!=!  RESULTS  !=!=!=!=!");
            //Console.WriteLine($"TheNearestNeighbour: {neighbourIsBest}");
            //Console.WriteLine($"TheNearestNeighbourArea: {neighbourAreaIsBest}");
            //Console.WriteLine($"TheNearestNeighbourParallel: {neighbourParallelIsBest}");
            //Console.WriteLine($"TheNearestLineWithX: {lineIsBest}");
            //Console.WriteLine($"TheNearestLineTargetsWithMatrix: {neighbourLineIsBest}");
        }
    }
}