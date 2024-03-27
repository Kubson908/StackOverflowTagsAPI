using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackOverflowTags.api.Data;
using StackOverflowTags.api.Services;

namespace StackOverflowTags.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagsController : ControllerBase
{
    private ApplicationDbContext _context;
    private readonly TagsService _tagsService;
    public TagsController(ApplicationDbContext context, TagsService tagsService)
    {
        _context = context;
        _tagsService = tagsService;
    }

    [HttpGet("download-tags")]
    public async Task<IActionResult> DownloadTags([FromQuery] int minTotal = 1000, [FromQuery] int pageSize = 100, [FromQuery] string order = "asc")
    {
        try
        {
            await _tagsService.DownloadTags(minTotal, pageSize, order);
        } catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
        return Ok();
    }

    [HttpGet("get-tags")]
    public async Task<IActionResult> GetTags()
    {
        var tags = await _context.Tags.Take(100).ToListAsync();
        return Ok(tags);
    }
}
