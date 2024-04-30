using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Handin4.Models
{
    public class Batch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long BatchId { get; set; }
        public long ScheduleId { get; set; }
        public long OrderId { get; set; }
        public long Quantity { get; set; }

        [MaxLength(16)] // DD-MM-YYYY HH:MM
        public string? StartTime { get; set; }

        [MaxLength(16)] // DD-MM-YYYY HH:MM
        public string? TargetFinishTime { get; set; }

        [MaxLength(16)] // DD-MM-YYYY HH:MM
        public string? ActualFinishTime { get; set; }
        public long RecipeId { get; set; }
        public Schedule? Schedule { get; set; }
        public SOrder? SOrder { get; set; }
    }
}
