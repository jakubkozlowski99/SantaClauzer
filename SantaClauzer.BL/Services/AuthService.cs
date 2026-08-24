using Microsoft.AspNetCore.Identity;
using SantaClauzer.BL.Repositories;
using SantaClauzer.Model.Entities;
using System.Threading.Tasks;

namespace SantaClauzer.BL.Services
{
    public interface IAuthService
    {
        Task AddRefreshTokenModel(RefreshTokenModel refreshToken);
        Task<bool> CheckIfUserExists(string username);
        Task<RefreshTokenModel> GetRefreshTokenModel(string refreshToken);
        Task<UserModel> GetUserByUserName(string username, string password);
        Task RegisterUser (UserModel user, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IRoleService _roleService;
        private readonly IUserRoleService _userRoleService;
        private readonly PasswordHasher<UserModel> _passwordHasher = new PasswordHasher<UserModel>();

        public AuthService(IAuthRepository authRepository, IRoleService roleService, IUserRoleService userRoleService)
        {
            _authRepository = authRepository;
            _roleService = roleService;
            _userRoleService = userRoleService;
        }

        public async Task AddRefreshTokenModel(RefreshTokenModel refreshToken)
        {
            await _authRepository.RemoveRefreshTokenByUserID(refreshToken.UserId);
            await _authRepository.AddRefreshTokenModel(refreshToken);
        }

        public async Task<bool> CheckIfUserExists(string username)
        {
            var user = await _authRepository.GetUserByUserName(username);
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public Task<RefreshTokenModel> GetRefreshTokenModel(string refreshToken)
        {
            return _authRepository.GetRefreshTokenModel(refreshToken);
        }

        public async Task<UserModel> GetUserByUserName(string username, string password)
        {
            var user = await _authRepository.GetUserByUserName(username);
            if (user == null) return null;

            var verification = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (verification == PasswordVerificationResult.Success || verification == PasswordVerificationResult.SuccessRehashNeeded)
                return user;

            return null;
        }

        public async Task RegisterUser(UserModel user, string password)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, password);

            await _authRepository.RegisterUser(user);

            // Assign default role to the user
            var defaultRole = await _roleService.GetRoleByName("User");
            if (defaultRole != null)
            {
                var userRole = new UserRoleModel
                {
                    UserId = user.Id,
                    RoleId = defaultRole.Id
                };
                await _userRoleService.AddUserRole(userRole);
            }
        }
    }
}
