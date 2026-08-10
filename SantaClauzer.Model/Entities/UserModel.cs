using System.Collections.Generic;

namespace SantaClauzer.Model.Entities
{
    public class UserModel
    {
        public int Id { get; set; }

        // username and password storage
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // navigation: user's refresh tokens
        public ICollection<RefreshTokenModel> RefreshTokens { get; set; } = new List<RefreshTokenModel>();

        // navigation: roles assigned to the user via join table
        public ICollection<UserRoleModel> UserRoles { get; set; } = new List<UserRoleModel>();
        public ICollection<PresentGroupModel> PresentGroups { get; set; } = new List<PresentGroupModel>();
    }
}
