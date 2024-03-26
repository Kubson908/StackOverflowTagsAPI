using Microsoft.EntityFrameworkCore;
using StackOverflowTags.api.Data;

namespace StackOverflowTags.api.Extensions;

public static class MigrationHelper
{
    public static void AddMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
}