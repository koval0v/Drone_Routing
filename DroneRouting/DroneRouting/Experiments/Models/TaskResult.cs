using DroneRouting.Algorithms.Models.AlgorithmsTypes;
using DroneRouting.Models.Targets;

namespace DroneRouting.Experiments.Models
{
    public class TaskResult
    {
        public TheNearestNeighbourTypes? NeighbourType { get; set; } = null;
        public TheNearestToTheLineTypes? LineType { get; set; } = null;
        public double RouteDistance { get; set; }
        public double RouteTime { get; set; }
        public List<Target> Route { get; set; } = new();
        public long ExecutionTime { get; set; }
        public bool TheBestByTargets { get; set; } = false;
        public bool TheBestByTargetsAndDistance { get; set; } = false;
        public bool TheWorstByTargets { get; set; } = false;
        public bool TheWorstByTargetsAndDistance { get; set; } = false;
    }
}
