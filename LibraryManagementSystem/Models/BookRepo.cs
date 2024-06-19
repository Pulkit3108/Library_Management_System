using System.Linq;
namespace LibraryManagementSystem.Models
{
    public class BookRepo : IBookRepo
    {
        private readonly Library_Management_SystemContext _context; 
        public BookRepo(Library_Management_SystemContext context)
        {
            _context = context;
        }
        public Book getBookById(int bookId)
        {
            return _context.Books.FirstOrDefault(b => b.BookId == bookId);
        }
    }
}
