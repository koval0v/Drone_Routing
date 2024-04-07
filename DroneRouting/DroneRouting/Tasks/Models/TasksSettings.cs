namespace DroneRouting.Tasks.Models
{
    public class TasksSettings
    {
        public int TasksQuantity { get; set; }
        public int TargetsQuantity { get; set; }
        public double TimeResource { get; set; }
        public double AverageSpeed { get; set; }
        public bool DistributionIsUniform { get; set; }
        public Coordinates General { get; set; }
        public Coordinates StartBase { get; set; }
        public Coordinates EndBase { get; set; }
    }
}
