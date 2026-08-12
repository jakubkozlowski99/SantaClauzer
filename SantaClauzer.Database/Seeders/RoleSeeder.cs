using SantaClauzer.Database.Data;
using SantaClauzer.Model.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SantaClauzer.Database.Seeders
{
    public class RoleSeeder : ISeeder
    {
        private readonly AppDbContext _dbContext;
        public RoleSeeder(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<SeedResult> SeedAsync()
        {
            if (!_dbContext.Database.CanConnect())
            {
                return new SeedResult { Success = false, Message = "Database is not available" };
            }

            if (!_dbContext.Roles.Any())
            {
                var roles = new List<RoleModel>
                {
                    new RoleModel { Name = "Admin" },
                    new RoleModel { Name = "User" }
                };
                _dbContext.Roles.AddRange(roles);
                await _dbContext.SaveChangesAsync();
                return new SeedResult { Success = true, Message = "Seeded roles" };
            }

            return new SeedResult { Success = true, Message = "Roles already exist" };
        }
    }
}
