
namespace Grip.Core.Model
{
    public class PeriodClass
    {
        [AutoIncrement]
        [PrimaryKey]
        [NotNull]
        public int N { get; set; }
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Period { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan StopTime { get; set; }
        public int Pause { get; set; }
        public bool IsActive { get; set; }
        public bool IsNotify { get; set; }
        public bool IsVisible { get; set; }
        public bool IsAutoDayEnd { get; set; }
        public DateTime SaveDate { get; set; }

    }
}
