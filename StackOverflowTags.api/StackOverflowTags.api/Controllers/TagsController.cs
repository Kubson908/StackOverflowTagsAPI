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
    public async Task<IResult> DownloadTags([FromQuery] int minTotal = 1000, [FromQuery] int pageSize = 100, [FromQuery][EnumDataType(typeof(StackAPISort))] StackAPISort sort = StackAPISort.popular, [FromQuery][EnumDataType(typeof(Order))] Order order = Order.desc)
    {
        var result = await _tagsService.DownloadTags(minTotal, pageSize, sort.ToString(), order.ToString());
        if (result.IsSuccess)
            return Results.Ok();

        return Results.BadRequest(result.Error);
    }

    [HttpGet("get-tags")]
    public async Task<IResult> GetTags([FromQuery] int page, [FromQuery] int pageSize, 
        [FromQuery][EnumDataType(typeof(DBSort))] DBSort sort = DBSort.name,
        [FromQuery][EnumDataType(typeof(Order))] Order order = Order.asc)
    {
        var result = await _tagsService.GetTags(page, pageSize, sort, order);
        if (result.IsSuccess)
            return Results.Ok(result.Tags);

        return Results.BadRequest(result.Error);
    }
}
