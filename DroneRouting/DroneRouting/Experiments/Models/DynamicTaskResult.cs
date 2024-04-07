using DroneRouting.Algorithms.Models.AlgorithmsTypes;

namespace DroneRouting.Experiments.Models
{
    public class DynamicTaskResult
    {
        public TheNearestNeighbourTypes? NeighbourType { get; set; } = null;
        public TheNearestToTheLineTypes? LineType { get; set; } = null;
        public int MapTargetsCount { get; set; }
        public int RouteTargetsCount { get; set; }
    }
}
