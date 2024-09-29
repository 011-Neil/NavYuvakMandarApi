using NavYuvakMandarApi.Models;


namespace NavYuvakMandarApi.Repositories
{
    public interface IUserRepository
    {
        Task<bool> IsUsernameTaken(string username);
        Task RegisterUser(User user);
        Task<List<User>> GetUsers();

        Task<User> AuthenticateUser(string username, string password);
        Task UpdateUserDetails(User user);
        Task DeleteUser(int userId);

    }
}
