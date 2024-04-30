using System.ComponentModel.DataAnnotations;

namespace Handin4.Models
{
    public class Packet
    {
        [Key]
        public long TrackId { get; set; }
        public long OrderId { get; set; }
        public long DriverId { get; set; }
        public SOrder? SOrder { get; set; }
        public Driver? Driver { get; set; }
    }
}
