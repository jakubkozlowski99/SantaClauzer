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
    public interface IUserRoleRepository
    {
        Task AddUserRole(UserRoleModel userRole);
        Task<bool> CheckIfUserRoleExists(int userId, int roleId);
    }

    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRoleRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddUserRole(UserRoleModel userRole)
        {
            await _appDbContext.UserRoles.AddAsync(userRole);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> CheckIfUserRoleExists(int userId, int roleId)
        {
            return await _appDbContext.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        }
    }
}
