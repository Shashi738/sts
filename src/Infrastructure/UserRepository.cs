using Application;

namespace Infrastructure
{
    public class UserRepository : IUserRepository
    {
        public async Task<bool> FindUserAsync(User user)
        {
            return Task.FromResult(true).Result;
        }
    }
}
