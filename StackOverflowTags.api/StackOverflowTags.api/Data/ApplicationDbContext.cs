using Microsoft.EntityFrameworkCore;
using StackOverflowTags.api.Model;

namespace StackOverflowTags.api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() { }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Tag> Tags { get; set; }
}