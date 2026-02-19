using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakeDeal.Models
{
    public class Listing
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public required string Title { get; set; }

        [Required]
        [MaxLength(2000)]
        public required string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        public decimal Price { get; set; }

        [Required]
        public string City { get; set; } = string.Empty;

        // 🔗 Category relation
        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // 🔗 User relation (who posted ad)
        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

        // 🖼️ Images support (one listing → many images)
        public ICollection<ListingImage>? Images { get; set; }

        // 📊 Status
        public bool IsActive { get; set; } = true;
        public bool IsSold { get; set; } = false;

        // ⏱️ Dates
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
    }
}
