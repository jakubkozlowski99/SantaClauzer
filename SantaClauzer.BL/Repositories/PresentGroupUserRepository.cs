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
    public interface IPresentGroupUserRepository
    {
        Task<PresentGroupUserModel> AddPresentGroupUser(PresentGroupUserModel presentGroupUser);
        Task<List<UserModel>> GetUsersInPresentGroup(int presentGroupId);
        Task<bool> CheckIfUserInPresentGroup(int presentGroupId, int userId);
        Task<List<PresentGroupUserModel>> GetActiveInvitationsForUser(int userId);
        Task<PresentGroupUserModel?> AcceptInvitation(int presentGroupId, int userId);
        Task<bool> RemovePresentGroupUser(int presentGroupId, int userId);
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
            // server-side duplicate-checking: if a PresentGroupUser with same group and user already exists,
            // return the existing record instead of creating a duplicate.
            var existing = await _appDbContext.PresentGroupUsers
                .FirstOrDefaultAsync(pgu => pgu.PresentGroupId == presentGroupUser.PresentGroupId
                                         && pgu.UserId == presentGroupUser.UserId);

            if (existing != null)
            {
                // return existing to avoid duplicate records
                return existing;
            }

            await _appDbContext.PresentGroupUsers.AddAsync(presentGroupUser);
            await _appDbContext.SaveChangesAsync();
            return presentGroupUser;
        }
        public async Task<List<UserModel>> GetUsersInPresentGroup(int presentGroupId)
        {
            var users = await _appDbContext.PresentGroupUsers
                .Where(pgu => pgu.PresentGroupId == presentGroupId && pgu.InvitationAccepted == true)
                .Select(pgu => pgu.User)
                .ToListAsync();
            return users;
        }

        public async Task<bool> CheckIfUserInPresentGroup(int userId, int presentGroupId)
        {
            var exists = await _appDbContext.PresentGroupUsers
                .AnyAsync(pgu => pgu.PresentGroupId == presentGroupId && pgu.UserId == userId);
            return exists;
        }

        public async Task<List<PresentGroupUserModel>> GetActiveInvitationsForUser(int userId)
        {
            var invitations = await _appDbContext.PresentGroupUsers
                .Where(pgu => pgu.UserId == userId && pgu.InvitationAccepted == false)
                .Include(pgu => pgu.PresentGroup)
                .ThenInclude(pg => pg.Creator) // include creator if useful
                .ToListAsync();
            return invitations;
        }

        public async Task<PresentGroupUserModel?> AcceptInvitation(int presentGroupId, int userId)
        {
            var pgu = await _appDbContext.PresentGroupUsers
                .FirstOrDefaultAsync(p => p.PresentGroupId == presentGroupId && p.UserId == userId);

            if (pgu == null)
                return null;

            if (pgu.InvitationAccepted)
                return pgu;

            pgu.InvitationAccepted = true;
            _appDbContext.PresentGroupUsers.Update(pgu);
            await _appDbContext.SaveChangesAsync();
            return pgu;
        }

        public async Task<bool> RemovePresentGroupUser(int presentGroupId, int userId)
        {
            var pgu = await _appDbContext.PresentGroupUsers
                .FirstOrDefaultAsync(p => p.PresentGroupId == presentGroupId && p.UserId == userId);

            if (pgu == null)
                return false;

            _appDbContext.PresentGroupUsers.Remove(pgu);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
