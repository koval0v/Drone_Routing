using DroneRouting.Models.Targets;

namespace DroneRouting.Algorithms.Models.Distances
{
    internal class DistanceToTheLineElement
    {
        public Target Target { get; set; } = null!;
        public double Distance { get; set; }
        public bool IsAdded { get; set; } = false;
    }
}
