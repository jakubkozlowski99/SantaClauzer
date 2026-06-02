using SantaClauzer.BL.Repositories;
using SantaClauzer.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaClauzer.BL.Services
{
    public interface IPresentGroupService
    {
        Task<List<PresentGroupModel>> GetPresentGroups();
    }

    public class PresentGroupService : IPresentGroupService
    {
        private readonly IPresentGroupRepository _presentGroupRepository;

        public PresentGroupService(IPresentGroupRepository presentGroupRepository)
        {
            _presentGroupRepository = presentGroupRepository;
        }

        public async Task<List<PresentGroupModel>> GetPresentGroups()
        {
            return await _presentGroupRepository.GetPresentGroups();
        }
    }
}
