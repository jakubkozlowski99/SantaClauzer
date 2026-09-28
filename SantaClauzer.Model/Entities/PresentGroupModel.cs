using System;
using System.Collections.Generic;

namespace SantaClauzer.Model.Entities
{
    public class PresentGroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Creator FK and navigation
        public int CreatorId { get; set; }
        public UserModel Creator { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Budget { get; set; }

        // Wish lists contained in the group
        public ICollection<WishListModel> WishLists { get; set; } = new List<WishListModel>();

        // Many-to-many membership relation: users in this present group
        public ICollection<PresentGroupUserModel> PresentGroupUsers { get; set; } = new List<PresentGroupUserModel>();
    }
}
