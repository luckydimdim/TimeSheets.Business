using System; 
namespace Cmas.BusinessLayers.TimeSheets.Entities
{
    /// <summary>
    /// Период дат
    /// </summary>
    public class DateRange
    {
        readonly DateTime min;
        readonly DateTime max;

        public DateRange(DateTime min, DateTime max)
        {
            this.min = min;
            this.max = max;
        }

        public bool IsOverlapped(DateRange other)
        {
            return Min.CompareTo(other.Max) < 0 && other.Min.CompareTo(Max) < 0;
        }

        public DateTime Min { get { return min; } }
        public DateTime Max { get { return max; } }

        public override string ToString()
        {
            var localMin = min.ToLocalTime();
            var localMax = max.ToLocalTime();
            return $"{localMin} - {localMax}";
        }
    }
}
