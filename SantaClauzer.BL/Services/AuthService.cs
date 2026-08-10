using Microsoft.Identity.Client;
using SantaClauzer.BL.Repositories;
using SantaClauzer.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaClauzer.BL.Services
{
    public interface IAuthService
    {
        Task<UserModel> GetUserByLogin(string username, string password);
        Task AddRefreshTokenModel(RefreshTokenModel refreshToken);
        Task<RefreshTokenModel> GetRefreshTokenModel(string refreshToken);
    }
    public class AuthService(IAuthRepository authRepository) : IAuthService
    {
        public async Task AddRefreshTokenModel(RefreshTokenModel refreshToken)
        {
            await authRepository.RemoveRefreshTokenByUserID(refreshToken.UserId);
            await authRepository.AddRefreshTokenModel(refreshToken);
        }

        public Task<RefreshTokenModel> GetRefreshTokenModel(string refreshToken)
        {
            return authRepository.GetRefreshTokenModel(refreshToken);
        }

        public Task<UserModel> GetUserByLogin(string username, string password)
        {
            var user = authRepository.GetUserByLogin(username, password);

            return user;
        }
    }
}
