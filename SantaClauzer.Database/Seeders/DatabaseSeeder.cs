using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SantaClauzer.Database.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SantaClauzer.Database.Seeders
{
    public interface IDatabaseSeeder
    {
        Task<Dictionary<string, SeedResult>> SeedAsync();
    }

    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly AppDbContext _dbContext;
        private readonly IEnumerable<ISeeder> _seeders;
        private readonly ILogger<DatabaseSeeder> _logger;

        public DatabaseSeeder(AppDbContext dbContext, IEnumerable<ISeeder> seeders, ILogger<DatabaseSeeder> logger)
        {
            _dbContext = dbContext;
            _seeders = seeders;
            _logger = logger;
        }

        public async Task<Dictionary<string, SeedResult>> SeedAsync()
        {
            var results = new Dictionary<string, SeedResult>();

            try
            {
                if (!_dbContext.Database.CanConnect())
                {
                    _logger.LogWarning("Database not available, skipping seeding.");
                    return results;
                }

                _logger.LogInformation("Applying pending migrations (if any)");
                await _dbContext.Database.MigrateAsync();

                foreach (var seeder in _seeders)
                {
                    var name = seeder.GetType().Name;
                    try
                    {
                        _logger.LogInformation("Running seeder {Seeder}", name);
                        var res = await seeder.SeedAsync();
                        results[name] = res ?? new SeedResult { Success = false, Message = "Null result" };
                        if (!results[name].Success)
                        {
                            _logger.LogError("Seeder {Seeder} failed: {Message}", name, results[name].Message);
                        }
                        else
                        {
                            _logger.LogInformation("Seeder {Seeder} succeeded: {Message}", name, results[name].Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Seeder {Seeder} threw an exception", name);
                        results[name] = new SeedResult { Success = false, Message = ex.Message };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Database seeding encountered a fatal error");
                throw; // rethrow so caller is aware
            }

            return results;
        }
    }
}
