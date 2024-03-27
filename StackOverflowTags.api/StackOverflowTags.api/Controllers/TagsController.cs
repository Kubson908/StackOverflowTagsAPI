using Microsoft.AspNetCore.Mvc;
using StackOverflowTags.api.Services;
using System.ComponentModel.DataAnnotations;

namespace StackOverflowTags.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagsController : ControllerBase
{
    private readonly TagsService _tagsService;
    public TagsController(TagsService tagsService)
    {
        _tagsService = tagsService;
    }

    [HttpGet("download-tags")]
    public async Task<IActionResult> DownloadTags([FromQuery] int minTotal = 1000, [FromQuery] int pageSize = 100, [FromQuery][EnumDataType(typeof(StackAPISort))] StackAPISort sort = StackAPISort.popular, [FromQuery][EnumDataType(typeof(Order))] Order order = Order.desc)
    {
        try
        {
            await _tagsService.DownloadTags(minTotal, pageSize, sort.ToString(), order.ToString());
        } catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
        return Ok();
    }

    [HttpGet("get-tags")]
    public async Task<IActionResult> GetTags([FromQuery] int page, [FromQuery] int pageSize, 
        [FromQuery][EnumDataType(typeof(DBSort))] DBSort sort = DBSort.name,
        [FromQuery][EnumDataType(typeof(Order))] Order order = Order.asc)
    {
        if (page <= 0)
        {
            return BadRequest("Page must be greater than 0");
        }
        try
        {
            var tags = await _tagsService.GetTags(page, pageSize, sort, order);
            return Ok(tags);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
