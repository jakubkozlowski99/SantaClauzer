using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaClauzer.Model.Entities
{
    public class UserRoleModel
    {
        public int Id { get; set; }

        // FKs
        public int UserId { get; set; }
        public int RoleId { get; set; }

        // navigation
        public UserModel User { get; set; }
        public RoleModel Role { get; set; }
    }
}
