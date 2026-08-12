using System.Threading.Tasks;

namespace SantaClauzer.Database.Seeders
{
    public interface ISeeder
    {
        Task<SeedResult> SeedAsync();
    }
}
