using DroneRouting.Models.Targets;

namespace DroneRouting.Tasks.Models
{
    public class ExperimentTask
    {
        public double TimeResource { get; set; }
        public double AverageSpeed { get; set; }
        public List<ExamTarget> Targets { get; set; }
        public Vehicle StartBase { get; set; }
        public Vehicle EndBase { get; set; }
    }
}
