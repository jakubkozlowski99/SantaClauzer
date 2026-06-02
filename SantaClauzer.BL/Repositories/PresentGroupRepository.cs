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
    }
}
