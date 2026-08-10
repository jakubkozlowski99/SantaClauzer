using System;
using System.Collections.Generic;

namespace SantaClauzer.Model.Entities
{
    public class PresentGroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // make nullable to accept existing orphans until you reconcile them
        public int? CreatorId { get; set; }
        public UserModel? Creator { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Budget { get; set; }

        public ICollection<WishListModel> WishLists { get; set; } = new List<WishListModel>();
    }
}
