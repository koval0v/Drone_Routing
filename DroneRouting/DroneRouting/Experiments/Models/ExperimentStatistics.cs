using DroneRouting.Algorithms.Models.AlgorithmsTypes;

namespace DroneRouting.Experiments.Models
{
    public class ExperimentStatistics
    {
        public List<ExperimentResult> TotalResults { get; set; } = new();
        public List<DynamicTaskResult> DynamicResults { get; set; } = new();
        public double AverageDistance { get; set; }
        public double TheBestResultDistance { get; set; }
        public List<AlgorithmDeviations> Deviations { get; set; } = new();
        public List<AlgorithmDeviations> SquareDeviations { get; set; } = new();
    }
}
