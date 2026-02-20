using TakeDeal.Models;

namespace TakeDeal.DTOs
{
    public class ListingDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;


    }
}
