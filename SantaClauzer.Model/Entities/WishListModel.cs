using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaClauzer.Model.Entities
{
    public class WishListModel
    {
        public required int Id { get; set; }
        public int UserId { get; set; }
        public int PresentGroupId { get; set; }
        public string Content { get; set; } = string.Empty;

    }
}
