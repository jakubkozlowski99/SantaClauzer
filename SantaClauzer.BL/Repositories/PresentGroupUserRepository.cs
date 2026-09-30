using SantaClauzer.Database.Data;
using SantaClauzer.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaClauzer.BL.Repositories
{
    public interface IPresentGroupUserRepository
    {
        Task<PresentGroupUserModel> AddPresentGroupUser(PresentGroupUserModel presentGroupUser);
        Task<List<UserModel>> GetUsersInPresentGroup(int presentGroupId);
    }
    public class PresentGroupUserRepository : IPresentGroupUserRepository
    {
        private readonly AppDbContext _appDbContext;
        public PresentGroupUserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<PresentGroupUserModel> AddPresentGroupUser(PresentGroupUserModel presentGroupUser)
        {
            await _appDbContext.PresentGroupUsers.AddAsync(presentGroupUser);
            await _appDbContext.SaveChangesAsync();
            return presentGroupUser;
        }
        public async Task<List<UserModel>> GetUsersInPresentGroup(int presentGroupId)
        {
            var users = await Task.Run(() => _appDbContext.PresentGroupUsers
                .Where(pgu => pgu.PresentGroupId == presentGroupId)
                .Select(pgu => pgu.User)
                .ToList());
            return users;
        }
    }
}
