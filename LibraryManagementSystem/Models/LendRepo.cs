using System.Linq;
namespace LibraryManagementSystem.Models
{
    public class LendRepo : ILendRepo
    {
        private readonly Library_Management_SystemContext _context; 
        public LendRepo(Library_Management_SystemContext context)
        {
            _context = context;
        }
        public LendRequest getLendRequestById(int lendId)
        {
            return _context.LendRequests.FirstOrDefault(s => s.LendId == lendId);
        }
    }
}
