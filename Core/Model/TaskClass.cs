
namespace Grip.Core.Model
{
    public class TaskClass
    {
        [AutoIncrement]
        [PrimaryKey]
        [NotNull]
        public int N { get; set; }
        [Indexed(Name = "ListingID", Order = 1, Unique = true)]
        public string Type { get; set; }
        [Indexed(Name = "ListingID", Order = 2, Unique = true)]
        public string Name { get; set; }
        public string Descripton { get; set; }
        public bool IsActive { get; set; }
        public DateTime SaveDate { get; set; }
    }
}
