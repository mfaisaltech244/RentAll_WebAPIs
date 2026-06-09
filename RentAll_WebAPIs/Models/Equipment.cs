using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentAll_WebAPIs.Models
{

    public class Equipment
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }= string.Empty;

        [Required]
        public string Description { get; set; }= string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        [Range(1, 999999)]
        public decimal DailyRate { get; set; }

        [Required]
        [Column(TypeName = "TEXT")]  
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(OwnerId))]
        public User Owner { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
