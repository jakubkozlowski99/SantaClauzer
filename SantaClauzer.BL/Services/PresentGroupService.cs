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
        Task<PresentGroupModel> GetPresentGroup(int id);
        Task UpdatePresentGroup(int id, PresentGroupModel model);
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
        public async Task<PresentGroupModel> GetPresentGroup(int id)
        {
            return await _presentGroupRepository.GetPresentGroup(id);
        }
        public async Task UpdatePresentGroup(int id, PresentGroupModel model)
        {
            var existingPresentGroup = await _presentGroupRepository.GetPresentGroup(id);
            if (existingPresentGroup != null)
            {
                existingPresentGroup.Name = model.Name;
                existingPresentGroup.Description = model.Description;
                // Update other properties as needed
                await _presentGroupRepository.UpdatePresentGroup(existingPresentGroup);
            }
        }
    }
}
