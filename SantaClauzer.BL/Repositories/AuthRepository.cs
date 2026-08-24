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
    public interface IAuthRepository
    {
        Task RegisterUser (UserModel user);
        Task<UserModel> GetUserByUserName(string username);
        Task RemoveRefreshTokenByUserID(int userId);
        Task AddRefreshTokenModel(RefreshTokenModel refreshToken);
        Task<RefreshTokenModel> GetRefreshTokenModel(string refreshToken);
    }
    public class AuthRepository(AppDbContext dbContext) : IAuthRepository
    {
        public async Task AddRefreshTokenModel(RefreshTokenModel refreshToken)
        {
            await dbContext.RefreshTokens.AddAsync(refreshToken);
            await dbContext.SaveChangesAsync();
        }

        public async Task RegisterUser(UserModel user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        public Task<RefreshTokenModel> GetRefreshTokenModel(string refreshToken)
        {
            return dbContext.RefreshTokens.Include(rt => rt.User).ThenInclude(rt => rt.UserRoles).ThenInclude(rt => rt.Role).FirstOrDefaultAsync(rt => rt.RefreshToken == refreshToken);
        }

        public async Task<UserModel> GetUserByUserName(string username)
        {
            return await dbContext.Users.Include(ur => ur.UserRoles).ThenInclude(r => r.Role).FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task RemoveRefreshTokenByUserID(int userId)
        {
            var refreshToken = dbContext.RefreshTokens.FirstOrDefault(rt => rt.UserId == userId);
            if (refreshToken != null)
            {
                dbContext.RefreshTokens.Remove(refreshToken);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
