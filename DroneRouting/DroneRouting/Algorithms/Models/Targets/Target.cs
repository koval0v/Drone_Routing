namespace DroneRouting.Models.Targets
{
    public abstract class Target // об'єкт мапи
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public bool isVisited { get; set; } = false;
        public bool IsExcluded { get; set; } = false;

        public Target(int x, int y, string name)
        {
            X = x;
            Y = y;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Target target = (Target)obj;
            return (X == target.X && Y == target.Y);
        }

        public override int GetHashCode()
        {
            return 7 * X + 3 * Y;
        }

        public static bool operator ==(Target target1, Target target2)
        {
            if (target1 is null)
            {
                return target2 is null;
            }

            return target1.Equals(target2);
        }

        public static bool operator !=(Target target1, Target target2)
        {
            return !(target1 == target2);
        }
    }
}
