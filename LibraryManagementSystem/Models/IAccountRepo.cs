namespace LibraryManagementSystem.Models
{
    public interface IAccountRepo
    {
        Account getUserByName(string userName);
    }
}
