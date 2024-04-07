using DroneRouting.Algorithms.Models.AlgorithmsTypes;

namespace DroneRouting.Experiments.Models
{
    public class AlgorithmDeviations
    {
        public TheNearestNeighbourTypes? NeighbourType { get; set; } = null;
        public TheNearestToTheLineTypes? LineType { get; set; } = null;
        public double TheBestResultDeviation { get; set; }
        public double CommonAverageDeviation { get; set; }
    }
}
