using System.Collections.Generic;

namespace SantaClauzer.Model.Entities
{
    public class RoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // navigation: many users can have this role (via join table)
        public ICollection<UserRoleModel> UserRoles { get; set; } = new List<UserRoleModel>();
    }
}
