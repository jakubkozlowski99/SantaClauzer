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
        Task<List<UserModel>> GetUsersInPresentGroup(int presentGroupId);
        Task<bool> CheckIfUserInPresentGroup(int userId, int presentGroupId);
        Task<List<PresentGroupUserModel>> GetActiveInvitationsForUser(int userId);
        Task<PresentGroupUserModel?> AcceptInvitation(int presentGroupId, int userId);
        Task<bool> RemovePresentGroupUser(int presentGroupId, int userId);
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
        public async Task<List<UserModel>> GetUsersInPresentGroup(int presentGroupId)
        {
            return await _presentGroupUserRepository.GetUsersInPresentGroup(presentGroupId);
        }

        public async Task<bool> CheckIfUserInPresentGroup(int userId, int presentGroupId)
        {
            return await _presentGroupUserRepository.CheckIfUserInPresentGroup(userId, presentGroupId);
        }

        public async Task<List<PresentGroupUserModel>> GetActiveInvitationsForUser(int userId)
        {
            return await _presentGroupUserRepository.GetActiveInvitationsForUser(userId);
        }

        public async Task<PresentGroupUserModel?> AcceptInvitation(int presentGroupId, int userId)
        {
            return await _presentGroupUserRepository.AcceptInvitation(presentGroupId, userId);
        }

        public async Task<bool> RemovePresentGroupUser(int presentGroupId, int userId)
        {
            return await _presentGroupUserRepository.RemovePresentGroupUser(presentGroupId, userId);
        }
    }
}
