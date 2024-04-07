using DroneRouting.Models.Targets;

namespace DroneRouting.Algorithms.Models.Distances
{
    internal class DistanceMatrixElement
    {
        public Target? Target { get; set; }
        public Dictionary<Target, double> SecondTargetDistance { get; set; } = new Dictionary<Target, double>();
    }
}
