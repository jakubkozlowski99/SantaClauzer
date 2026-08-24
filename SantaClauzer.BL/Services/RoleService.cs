using SantaClauzer.BL.Repositories;
using SantaClauzer.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaClauzer.BL.Services
{
    public interface IRoleService
    {
        Task<RoleModel> GetRoleByName(string roleName);
    }
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RoleModel> GetRoleByName(string roleName)
        {
            var role = await _roleRepository.GetRoleByName(roleName);
            if (role == null)
            {
                throw new InvalidOperationException($"Role '{roleName}' does not exist.");
            }
            return role;
        }
    }
}
