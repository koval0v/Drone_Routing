using DroneRouting.Models.Targets;
using DroneRouting.Tasks.Models;

namespace DroneRouting.Tasks
{
    public static class TasksGenerator
    {
        public static List<ExperimentTask> GenerateTasksSet(TasksSettings settings)
        {
            List<ExperimentTask> resultSet = new();
            for (int i = 0; i < settings.TasksQuantity; i++)
            {
                Vehicle startVehicle = null!;
                Vehicle endVehicle = null!;
                List<ExamTarget> targets = new();
                for (int j = 0; j < settings.TargetsQuantity; j++)
                {
                    startVehicle = GenerateVehicle(settings.StartBase, "Start");
                    endVehicle = GenerateVehicle(settings.EndBase, "End");
                    targets = GenerateTargets(settings.TargetsQuantity, settings.General, startVehicle, endVehicle, settings.DistributionIsUniform);
                }
                resultSet.Add(new ExperimentTask()
                {
                    TimeResource = settings.TimeResource,
                    AverageSpeed = settings.AverageSpeed,
                    Targets = targets,
                    StartBase = startVehicle,
                    EndBase = endVehicle
                });
            }
            return resultSet;
        }

        private static Vehicle GenerateVehicle(Coordinates coordinates, string name)
        {
            Random r = new();
            return new Vehicle(r.Next(coordinates.XRange.StartCoordinate, coordinates.XRange.EndCoordinate),
                        r.Next(coordinates.YRange.StartCoordinate, coordinates.YRange.EndCoordinate), name);
        }

        private static List<ExamTarget> GenerateTargets(double quantity, Coordinates general,
            Vehicle startBase, Vehicle endBase, bool isUniformDistribution)
        {
            Random r;
            List<ExamTarget> targets = new();
            int rowNumber = 0;
            int columnNumber = 0;
            for (int k = 0; k < quantity; k++)
            {
                bool isReservedPlace;
                ExamTarget examTarget;
                do
                {
                    r = new();
                    if (!isUniformDistribution) {
                        examTarget = new ExamTarget(r.Next(general.XRange.StartCoordinate, general.XRange.EndCoordinate),
                            r.Next(general.YRange.StartCoordinate, general.YRange.EndCoordinate), $"Target #{k}");
                    } else {
                        (int a, int b) = FindNumberMultipliers((int)quantity);
                        Console.WriteLine($"a -> {a}, b -> {b}");

                        int xTargetRange = (general.XRange.EndCoordinate - general.XRange.StartCoordinate) / a;
                        int yTargetRange = (general.YRange.EndCoordinate - general.YRange.StartCoordinate) / b;

                        int squareXStartCoordinate = general.XRange.StartCoordinate + xTargetRange * rowNumber;
                        int squareXEndCoordinate = general.XRange.StartCoordinate + xTargetRange * (rowNumber + 1);
                        int squareYStartCoordinate = general.YRange.StartCoordinate + yTargetRange * columnNumber;
                        int squareYEndCoordinate = general.YRange.StartCoordinate + yTargetRange * (columnNumber + 1);

                        rowNumber++;
                        if (rowNumber == a)
                        {
                            rowNumber = 0;
                            columnNumber++;
                        }

                        Console.WriteLine($"Target #{k} xЄ({squareXStartCoordinate},{squareXEndCoordinate})," +
                            $"yЄ({squareYStartCoordinate},{squareYEndCoordinate})");
                        examTarget = new ExamTarget(r.Next(squareXStartCoordinate, squareXEndCoordinate),
                           r.Next(squareYStartCoordinate, squareYEndCoordinate), $"Target #{k}");
                    }
                    isReservedPlace = targets.Any(x => x == examTarget) || examTarget.X < startBase.X || examTarget.X > endBase.X ||
                        (examTarget.X == startBase.X && examTarget.Y == startBase.Y) || (examTarget.X == endBase.X && examTarget.Y == endBase.Y);
                }
                while (isReservedPlace);
                targets.Add(examTarget);
            }
            return targets;
        }

        private static (int a, int b) FindNumberMultipliers(int n)
        {
            int sqrt = (int)Math.Round(Math.Sqrt(n));
            int factor1 = sqrt;
            int factor2 = n / factor1;

            while (factor1 * factor2 != n)
            {
                factor1++;
                factor2 = n / factor1;
            }

            return (factor1, factor2);
        }
    }
}
