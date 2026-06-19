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
        Task CreatePresentGroup(PresentGroupModel model);
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

        public async Task CreatePresentGroup(PresentGroupModel model)
        {
            await _presentGroupRepository.CreatePresentGroup(model);
        }
    }
}
