
namespace Grip.Core.Model
{
    public class ObjectClass
    {
        [AutoIncrement]
        [PrimaryKey]
        [NotNull]
        public int N { get; set; }
        public int TaskId { get; set; }
        public int PeriodId { get; set; }
        public TimeSpan NotificationTime { get; set; }
        public int Status { get; set; }
        public DateTime SaveDate { get; set; }

    }
}
