using System.ComponentModel.DataAnnotations;

#nullable disable

namespace LibraryManagementSystem.Models
{
    public partial class Author
    {
        [Key]
        public int AuthorId { get; set; }
        [Required]
        public string AuthorName { get; set; }

    }
}
