using SantaClauzer.BL.Repositories;
using SantaClauzer.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaClauzer.BL.Services
{
    public interface IPresentGroupUserService
    {
        Task<PresentGroupUserModel> AddPresentGroupUser(PresentGroupUserModel presentGroupUser);
    }
    public class PresentGroupUserService : IPresentGroupUserService
    {
        private readonly IPresentGroupUserRepository _presentGroupUserRepository;
        public PresentGroupUserService(IPresentGroupUserRepository presentGroupUserRepository)
        {
            _presentGroupUserRepository = presentGroupUserRepository;
        }
        public async Task<PresentGroupUserModel> AddPresentGroupUser(PresentGroupUserModel presentGroupUser)
        {
            return await _presentGroupUserRepository.AddPresentGroupUser(presentGroupUser);
        }
    }
}
