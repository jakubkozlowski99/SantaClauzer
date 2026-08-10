using System;

namespace SantaClauzer.Model.Entities
{
    public class WishListModel
    {
        public int Id { get; set; }

        // owner and group FKs
        public int UserId { get; set; }
        public UserModel? User { get; set; }

        public int PresentGroupId { get; set; }
        public PresentGroupModel? PresentGroup { get; set; }

        // content and optional metadata
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
