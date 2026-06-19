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
    }
}
