using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaClauzer.Model.Entities
{
    public class PresentGroupUserModel
    {
        public int Id { get; set; }

        // FK to PresentGroup
        public int PresentGroupId { get; set; }
        public PresentGroupModel PresentGroup { get; set; } = null!;

        // FK to User
        public int UserId { get; set; }
        public UserModel User { get; set; } = null!;

        public bool InvitationAccepted { get; set; } = false;
    }
}
