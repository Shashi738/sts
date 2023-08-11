namespace Application
{
    public interface IAuthenticationService
    {
        Task<string> AuthenticateAndGenerateToken(User user);
    }
}
