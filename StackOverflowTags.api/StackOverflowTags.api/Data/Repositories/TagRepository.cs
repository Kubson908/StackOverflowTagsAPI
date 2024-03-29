using Microsoft.EntityFrameworkCore;
using StackOverflowTags.api.Data.Interfaces;
using StackOverflowTags.api.Model;
using StackOverflowTags.api.Services;

namespace StackOverflowTags.api.Data.Repositories;

public class TagRepository(ApplicationDbContext context) : ITagRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddRangeAsync(List<Tag> tags)
    {
        await _context.Tags.AddRangeAsync(tags);
    }

    public async Task CalculatePercentagesAsync()
    {
        int totalTagCount = await _context.Tags.SumAsync(t => t.Count);
        await _context.Tags.ForEachAsync(tag =>
        {
            tag.Population_percentage = (double)tag.Count / (double)totalTagCount * 100;
        });
        await _context.SaveChangesAsync();
    }

    public int Count()
    {
        return _context.Tags.Count();
    }

    public async Task ExecuteDeleteAsync()
    {
        await _context.Tags.ExecuteDeleteAsync();
    }

    public async Task<List<Tag>> GetPageAsync(int page, int pageSize, DBSort sort, Order order)
    {
        List<Tag> tags;
        if (order == Order.asc)
        {
            tags = await (sort == DBSort.name ?
            _context.Tags.OrderBy(t => t.Name) : _context.Tags.OrderBy(t => t.Population_percentage))
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        else
        {
            tags = await (sort == DBSort.name ?
            _context.Tags.OrderByDescending(t => t.Name) :
             _context.Tags.OrderByDescending(t => t.Population_percentage))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }
        return tags;
    }

    public async Task<Tag?> GetTagByIdAsync(int id)
    {
        Tag? tag = await _context.Tags.FindAsync(id);
        return tag;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
