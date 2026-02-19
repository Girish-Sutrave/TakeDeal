using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TakeDeal.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }


        public required string Name { get; set; }

        public required string PasswordHash  { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
        [Phone]
        [StringLength(10,MinimumLength =10)]
        public required string Phone { get; set; }

        public DateTime CreatedDate { get; set; }=DateTime.UtcNow;

    }
}
