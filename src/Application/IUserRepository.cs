namespace Application
{
    public interface IUserRepository
    {
        Task<bool> FindUserAsync(User user);
    }
}
