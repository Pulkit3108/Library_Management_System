using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace LibraryManagementSystem.Models
{
    public partial class Book
    {
        [Key]
        public int BookId { get; set; }
        [Required]
        public string BookTitle { get; set; }
        [Required]
        public int NumberOfCopies { get; set; }
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public Author Author { get; set; }
        public int PublisherId { get; set; }
        [ForeignKey("PublisherId")]
        public Publisher Publisher { get; set; }
        [Required]
        public string BookCategory { get; set; }
        [Required]
        public string BookImageUrl { get; set; }
        [Required]
        public int IssuedBooks { get; set; } = 0;
        [Required]
        public bool IsAvailable { get; set; } = true;

    }
}
