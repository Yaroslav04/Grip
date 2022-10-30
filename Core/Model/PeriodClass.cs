
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
        public TimeSpan ControlTime { get; set; }
        public TimeSpan StopTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime SaveDate { get; set; }

    }
}
