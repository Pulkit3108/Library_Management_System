using System.ComponentModel.DataAnnotations;
#nullable disable

namespace LibraryManagementSystem.Models
{
    public partial class Publisher
    {
        [Key]
        public int PublisherId { get; set; }
        [Required]
        public string PublisherName { get; set; }

    }
}
