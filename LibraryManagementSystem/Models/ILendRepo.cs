namespace LibraryManagementSystem.Models
{
    public interface ILendRepo
    {
        LendRequest getLendRequestById(int lendId);
    }
}
