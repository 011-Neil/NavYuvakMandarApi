using NavYuvakMandarApi.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace NavYuvakMandarApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly navYuvakMandarDBContext _dbContext;

        public  UserRepository(navYuvakMandarDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteUser(int userId)
        {
            var existingUser = await _dbContext.RegisterUser.FindAsync(userId);
            if (existingUser != null)
            {
                _dbContext.RegisterUser.Remove(existingUser); // Mark the user for deletion
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetUsers()
        {
            return await _dbContext.RegisterUser.ToListAsync();
        }

        public async Task<bool> IsUsernameTaken(string username)
        {
            return await _dbContext.RegisterUser.AnyAsync(u => u.username == username);
        }
    

        public async Task RegisterUser(User user)
        {
            _dbContext.RegisterUser.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> AuthenticateUser(string username, string password)
        {
            var user = await _dbContext.RegisterUser.FirstOrDefaultAsync(u => u.username == username);
            if (user != null && VerifyPassword(password, user.password, user.salt))
            {
                return user;
            }
            return null;
        }


        public async Task UpdateUserDetails(User user)
        {
            var existingUser = await _dbContext.RegisterUser.FindAsync(user.user_id);
            if (existingUser != null)
            {
                existingUser.first_name = user.first_name;
                existingUser.last_name = user.last_name;
                // Update other user details as needed

                await _dbContext.SaveChangesAsync();
            }
        }





        private bool VerifyPassword(string password, string passwordHash, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            string hashedPassword = HashPassword(password, saltBytes);

            return passwordHash == hashedPassword;
        }

        private string HashPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

       
    }
}
