using SantaClauzer.BL.Repositories;
using SantaClauzer.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaClauzer.BL.Services
{
    public interface IUserRoleService
    {
        Task AddUserRole(UserRoleModel userRole);
    }
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task AddUserRole(UserRoleModel userRole)
        {
            if (await _userRoleRepository.CheckIfUserRoleExists(userRole.UserId, userRole.RoleId))
            {
                throw new InvalidOperationException("The user already has this role.");
            }

            await _userRoleRepository.AddUserRole(userRole);
        }
    }
}
