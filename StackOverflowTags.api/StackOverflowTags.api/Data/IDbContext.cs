using Microsoft.EntityFrameworkCore;
using StackOverflowTags.api.Model;

namespace StackOverflowTags.api.Data;

public interface IDbContext
{
    public DbSet<Tag> Tags { get; set; }
}
