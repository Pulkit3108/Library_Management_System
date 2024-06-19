namespace LibraryManagementSystem.Models
{
    public interface IBookRepo
    {
        Book getBookById(int bookId);
    }
}
