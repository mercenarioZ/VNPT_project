using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc_app.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? UserName { get; set; }

        [Required]
        [MinLength(6)]
        public string? Password { get; set; }
    }
}
