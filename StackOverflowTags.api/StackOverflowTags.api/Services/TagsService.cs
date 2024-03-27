using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackOverflowTags.api.Data;
using StackOverflowTags.api.Model;
using System.IO.Compression;

namespace StackOverflowTags.api.Services;

public class TagsService
{
    private readonly HttpClient _httpClient;
    private readonly ApplicationDbContext _context;

    public TagsService (HttpClient httpClient, ApplicationDbContext context)
    {
        _httpClient = httpClient;
        _context = context;
    }

    public async Task DownloadTags(int minTagsCount = 1000, int pageSize = 100, string order = "asc")
    {
        if (minTagsCount <= 0 || pageSize <= 0)
        {
            throw new ArgumentException("Minimum tags count and page size must be greater than zero.");
        }
        
        await _context.Tags.ExecuteDeleteAsync();

        var totalPages = (int)Math.Ceiling((double)minTagsCount / pageSize);
        
        for (int page = 1; page <= totalPages; page++)
        {
            var tags = await GetTagsPage(page, pageSize, order);

            await _context.Tags.AddRangeAsync(tags);

            if (tags.Count >= minTagsCount)
            {
                break;
            }
        }
        await _context.SaveChangesAsync();
        return;
    }

    private async Task<List<Tag>> GetTagsPage(int page, int pageSize, string order)
    {
        string apiUrl = $"tags?page={page}&pagesize={pageSize}&order={order}&sort=name&site=stackoverflow";

        var response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            if (response.Content.Headers.ContentEncoding.Contains("gzip"))
            {
                using var decompressedStream = new GZipStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress);
                using var reader = new StreamReader(decompressedStream);
                var jsonString = await reader.ReadToEndAsync();
                var items = JsonConvert.DeserializeObject<ApiResponse>(jsonString)?.Items;
                return items!;
            }
            else
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<ApiResponse>(jsonString)?.Items;
                return items!;
            }
        }
        else
        {
            throw new Exception($"Failed to get tags. Status code: {response.StatusCode}");
        }
    }

    public async Task CheckOnStartup()
    {
        if (_context.Tags.Count() < 1000)
        {
            await DownloadTags();
        }
    }
}
