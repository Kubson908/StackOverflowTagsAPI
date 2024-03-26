using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackOverflowTags.api.Data;
using StackOverflowTags.api.Model;
using System.IO.Compression;

namespace StackOverflowTags.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagsController : ControllerBase
{
    private ApplicationDbContext _context;
    private IConfiguration _configuration;
    private HttpClient _client;
    public TagsController(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _client = new HttpClient();
        _context = context;
    }

    [HttpGet("download-tags")]
    public async Task<IActionResult> DownloadTags()
    {
        string? apiUrl = _configuration.GetValue<string>("ApiURL");

        try
        {
            var response = await _client.GetAsync(apiUrl);

            if (response.Content.Headers.ContentEncoding.Contains("gzip"))
            {
                using var decompressedStream = new GZipStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress);
                using var reader = new StreamReader(decompressedStream);
                var jsonString = await reader.ReadToEndAsync();
                var items = JsonConvert.DeserializeObject<ApiResponse>(jsonString)?.items;
                if (items is not null)
                {
                    await _context.Tags.ExecuteDeleteAsync();
                    await _context.Tags.AddRangeAsync(items);
                    await _context.SaveChangesAsync();
                }
                return Ok(items);
            } 
            else
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<ApiResponse>(jsonString)?.items;
                return Ok(items);
            }

        } catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("get-tags")]
    public async Task<IActionResult> GetTags()
    {
        var tags = await _context.Tags.Take(100).ToListAsync();
        return Ok(tags);
    }
}
