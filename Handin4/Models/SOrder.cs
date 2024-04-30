using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Handin4.Models
{
    public class SOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderId { get; set; }
        public long SupermarketId { get; set; }
        public long ScheduleId { get; set; }
        public string? Date { get; set; }
        public string? Location { get; set; }
        public Supermarket? Supermarket { get; set; }
        public Schedule? Schedule { get; set; }
    }
}
