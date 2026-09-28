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
    }
}
