using System.Linq;
namespace LibraryManagementSystem.Models
{
    public class AccountRepo : IAccountRepo
    {
        private readonly Library_Management_SystemContext _context;
        public AccountRepo(Library_Management_SystemContext context)
        {
            _context = context;
        }
        public Account getUserByName(string userName)
        {
            return _context.Accounts.FirstOrDefault(u => u.UserName == userName);
        }

    }
}
