using Microsoft.EntityFrameworkCore;

#nullable disable

namespace LibraryManagementSystem.Models
{
    public partial class Library_Management_SystemContext : DbContext
    {
        public Library_Management_SystemContext()
        {
        }

        public Library_Management_SystemContext(DbContextOptions<Library_Management_SystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<LendRequest> LendRequests { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }

    }
}
