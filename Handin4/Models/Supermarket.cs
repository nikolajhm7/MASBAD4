using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Handin4.Models
{
    public class Supermarket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }

        // Migration 3
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
