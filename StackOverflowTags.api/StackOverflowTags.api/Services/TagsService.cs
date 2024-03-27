using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackOverflowTags.api.Data;
using StackOverflowTags.api.Model;
using System;
using System.IO.Compression;

namespace StackOverflowTags.api.Services;

public class TagsService(HttpClient httpClient, ApplicationDbContext context, ILogger<TagsService> logger)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<TagsService> _logger = logger;

    public async Task<ApiResult> DownloadTags(int minTagsCount = 1000, int pageSize = 100, string sort = "popular", string order = "desc")
    {
        if (minTagsCount <= 0)
        {
            _logger.LogError("Exception: {Message}", TagsErrors.TagsCount.Message);
            return ApiResult.Failure(TagsErrors.TagsCount);
        }

        if (pageSize <= 0)
        {
            _logger.LogError("Exception: {Message}", TagsErrors.PageSize.Message);
            return ApiResult.Failure(TagsErrors.PageSize);
        }

        await _context.Tags.ExecuteDeleteAsync();

        var totalPages = (int)Math.Ceiling((double)minTagsCount / pageSize);

        for (int page = 1; page <= totalPages; page++)
        {
            var tags = await GetTagsPage(page, pageSize, sort, order);

            await _context.Tags.AddRangeAsync(tags);

            if (tags.Count >= minTagsCount)
            {
                break;
            }
        }
        await _context.SaveChangesAsync();
        await CalculatePercentages();
        return ApiResult.Success();
    }

    private async Task<List<Tag>> GetTagsPage(int page, int pageSize, string sort, string order)
    {

        string apiUrl = $"tags?page={page}&pagesize={pageSize}&order={order}&sort={sort}&site=stackoverflow";

        var response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            if (response.Content.Headers.ContentEncoding.Contains("gzip"))
            {
                using var decompressedStream = new GZipStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress);
                using var reader = new StreamReader(decompressedStream);
                var jsonString = await reader.ReadToEndAsync();
                var items = JsonConvert.DeserializeObject<SOApiResponse>(jsonString)?.Items;
                return items!;
            }
            else
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<SOApiResponse>(jsonString)?.Items;
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
        if (_context.Tags.Count() < 1000) // The condition can be removed if data needs to be fetched upon each application launch
        {
            await DownloadTags();
        }
    }

    public async Task CalculatePercentages()
    {
        int totalTagCount = await _context.Tags.SumAsync(t => t.Count);
        await _context.Tags.ForEachAsync(tag =>
        {
            tag.Population_percentage = (double)tag.Count / (double)totalTagCount * 100;
        });
        await _context.SaveChangesAsync();
    }

    public async Task<ApiResult> GetTags(int page, int pageSize,
        DBSort sort = DBSort.name, Order order = Order.asc)
    {
        if (page <= 0)
        {
            _logger.LogError("Exception: {Message}", TagsErrors.Page.Message);
            return ApiResult.Failure(TagsErrors.Page);
        }

        if (pageSize <= 0)
        {
            _logger.LogError("Exception: {Message}", TagsErrors.PageSize.Message);
            return ApiResult.Failure(TagsErrors.PageSize);
        }

        List<Tag>? tags;
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
        return ApiResult.Success(tags);
    }
}

public enum StackAPISort
{
    popular,
    activity,
    name
}

public enum Order
{
    asc,
    desc,
}

public enum DBSort
{
    name,
    percentage
}
