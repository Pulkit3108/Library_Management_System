using System.ComponentModel.DataAnnotations;

#nullable disable

namespace LibraryManagementSystem.Models
{
    public partial class Account
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserPassword { get; set; }
        [Required]
        public string UserRole { get; set; }
    }
}
