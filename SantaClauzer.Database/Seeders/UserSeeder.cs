using Microsoft.AspNetCore.Identity;
using SantaClauzer.Database.Data;
using SantaClauzer.Model.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SantaClauzer.Database.Seeders
{
    public class UserSeeder : ISeeder
    {
        private readonly AppDbContext _dbContext;
        private readonly PasswordHasher<UserModel> _hasher = new PasswordHasher<UserModel>();

        public UserSeeder(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SeedResult> SeedAsync()
        {
            if (!_dbContext.Database.CanConnect())
                return new SeedResult { Success = false, Message = "Database is not available" };

            if (!_dbContext.Users.Any())
            {
                var adminUser = new UserModel { UserName = "Admin", Email = "" };
                var normalUser = new UserModel { UserName = "User", Email = "" };

                adminUser.PasswordHash = _hasher.HashPassword(adminUser, "Admin");
                normalUser.PasswordHash = _hasher.HashPassword(normalUser, "User");

                var users = new List<UserModel>
                {
                    adminUser,
                    normalUser
                };

                _dbContext.Users.AddRange(users);
                await _dbContext.SaveChangesAsync();
                return new SeedResult { Success = true, Message = "Seeded users" };
            }

            return new SeedResult { Success = true, Message = "Users already exist" };
        }
    }
}
