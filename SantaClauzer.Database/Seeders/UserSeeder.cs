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

        public UserSeeder(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SeedResult> SeedAsync()
        {
            if (!_dbContext.Database.CanConnect())
            {
                return new SeedResult { Success = false, Message = "Database is not available" };
            }

            if (!_dbContext.Users.Any())
            {
                var users = new List<UserModel>
                {
                    new UserModel { UserName = "Admin", Password = "Admin" },
                    new UserModel { UserName = "User", Password = "User" },
                    new UserModel { UserName = "UserNoRole", Password = "UserNoRole" }
                };
                _dbContext.Users.AddRange(users);
                await _dbContext.SaveChangesAsync();
                return new SeedResult { Success = true, Message = "Seeded users" };
            }

            return new SeedResult { Success = true, Message = "Users already exist" };
        }
    }
}
