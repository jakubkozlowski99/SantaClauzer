using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaClauzer.Model.Entities
{
    public class PresentGroupModel
    {
        public required int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CreatorId { get; set; }
        public List<int> MemberIds { get; set; } = new List<int>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Budget { get; set; }

    }
}
