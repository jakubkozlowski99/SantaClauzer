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
    public interface IPresentGroupRepository
    {
        Task<List<PresentGroupModel>> GetPresentGroups();
        Task<PresentGroupModel> CreatePresentGroup(PresentGroupModel model);
        Task<PresentGroupModel> GetPresentGroup(int id);
        Task<PresentGroupModel> UpdatePresentGroup(PresentGroupModel model);
        Task DeletePresentGroup(PresentGroupModel model);
    }

    public class PresentGroupRepository : IPresentGroupRepository
    {
        private readonly AppDbContext _appDbContext;

        public PresentGroupRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<PresentGroupModel>> GetPresentGroups()
        {
            return await _appDbContext.PresentGroups.ToListAsync();
        }

        public async Task<PresentGroupModel> CreatePresentGroup(PresentGroupModel model)
        {
            await _appDbContext.PresentGroups.AddAsync(model);
            await _appDbContext.SaveChangesAsync();
            return model;
        }

        public async Task<PresentGroupModel> GetPresentGroup(int id)
        {
            return await _appDbContext.PresentGroups.FindAsync(id);
        }
        
        public async Task<PresentGroupModel> UpdatePresentGroup(PresentGroupModel model)
        {
            _appDbContext.PresentGroups.Update(model);
            await _appDbContext.SaveChangesAsync();
            return model;
        }
        public async Task DeletePresentGroup(PresentGroupModel model)
        {
            _appDbContext.PresentGroups.Remove(model);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
