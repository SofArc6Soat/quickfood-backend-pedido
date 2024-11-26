using Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace Api.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class DatabaseMigratorBase
    {
        public static void MigrateDatabase(ApplicationDbContext context)
        {
            if (context.Database.IsInMemory())
            {
                return;
            }

            var migrator = context.GetService<IMigrator>();

            migrator.Migrate();
        }
    }
}