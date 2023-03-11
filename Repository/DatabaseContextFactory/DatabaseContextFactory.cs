using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using CompanyEmployees.Repository;

namespace CompanyEmployeesAPI.DatabaseContextFactory
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlServer(configuration.GetConnectionString("sqlConnection"));
            return new DatabaseContext(builder.Options);
        }
    }
}
