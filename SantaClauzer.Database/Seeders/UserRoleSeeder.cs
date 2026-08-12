using SantaClauzer.Database.Data;
using SantaClauzer.Model.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SantaClauzer.Database.Seeders
{
    public class UserRoleSeeder : ISeeder
    {
        private readonly AppDbContext _dbContext;
        public UserRoleSeeder(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SeedResult> SeedAsync()
        {
            if (!_dbContext.Database.CanConnect())
            {
                return new SeedResult { Success = false, Message = "Database is not available" };
            }

            if (!_dbContext.UserRoles.Any())
            {
                var user1 = _dbContext.Users.FirstOrDefault(u => u.UserName == "Admin");
                var user2 = _dbContext.Users.FirstOrDefault(u => u.UserName == "User");

                if (user1 == null || user2 == null)
                {
                    return new SeedResult { Success = false, Message = "Required users not found" };
                }

                var roleAdmin = _dbContext.Roles.FirstOrDefault(r => r.Name == "Admin");
                var roleUser = _dbContext.Roles.FirstOrDefault(r => r.Name == "User");

                if (roleAdmin == null || roleUser == null)
                {
                    return new SeedResult { Success = false, Message = "Required roles not found" };
                }

                var userRoles = new List<UserRoleModel>
                {
                    new UserRoleModel { UserId = user1.Id, RoleId = roleAdmin.Id },
                    new UserRoleModel { UserId = user1.Id, RoleId = roleUser.Id },
                    new UserRoleModel { UserId = user2.Id, RoleId = roleUser.Id }
                };
                _dbContext.UserRoles.AddRange(userRoles);
                await _dbContext.SaveChangesAsync();
                return new SeedResult { Success = true, Message = "Seeded user roles" };
            }

            return new SeedResult { Success = true, Message = "User roles already exist" };
        }
    }
}
