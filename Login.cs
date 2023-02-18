using System.ComponentModel.DataAnnotations;

namespace MK_Core_MVC.Models
{
    public class Login
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? password { get; set; }
    }
}
