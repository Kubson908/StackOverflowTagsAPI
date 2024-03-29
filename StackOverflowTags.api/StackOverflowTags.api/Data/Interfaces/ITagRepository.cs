using StackOverflowTags.api.Model;
using StackOverflowTags.api.Services;

namespace StackOverflowTags.api.Data.Interfaces;

public interface ITagRepository
{
    Task AddRangeAsync(List<Tag> tags);
    int Count();
    Task ExecuteDeleteAsync();
    Task CalculatePercentagesAsync();
    Task<List<Tag>> GetPageAsync(int page, int pageSize, DBSort sort, Order order);
    Task<Tag?> GetTagByIdAsync(int id);
    Task SaveChangesAsync();
}
