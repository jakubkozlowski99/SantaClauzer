using Microsoft.EntityFrameworkCore;
using SantaClauzer.Database.Data;
using SantaClauzer.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaClauzer.BL.Repositories
{
    public interface IRoleRepository
    {
        Task<RoleModel> GetRoleByName(string roleName);
    }

    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _appDbContext;

        public RoleRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<RoleModel> GetRoleByName(string roleName)
        {
            return await _appDbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }
    }
}
