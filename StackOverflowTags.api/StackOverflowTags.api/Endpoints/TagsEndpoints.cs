using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StackOverflowTags.api.Model;
using StackOverflowTags.api.Services;
using System.ComponentModel.DataAnnotations;

namespace StackOverflowTags.api.Endpoints;

/// <summary>
/// List of tag endpoints
/// </summary>
public static class TagsEndpoints
{
    public static void MapTagsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/tags");
        group.MapGet("", GetTags);
        group.MapGet("{id}", GetTagById);
        group.MapGet("download-tags", DownloadTags);
    }

    /// <summary>
    /// Gets a page of tags with pagination and optional sorting.
    /// </summary>
    /// <param name="tagsService"></param>
    /// <param name="page">Number of the page to get</param>
    /// <param name="pageSize">The number of tags per page</param>
    /// <param name="sort">The field to sort by</param>
    /// <param name="order">Order of sorting</param>
    /// <returns>A page of tags based on the specified parameters.</returns>
    public static async Task<Results<Ok<List<Tag>>, BadRequest<Error>>> GetTags(
        TagsService tagsService, 
        int page, int pageSize, 
        [FromQuery][EnumDataType(typeof(DBSort))] DBSort sort = DBSort.name,
        [FromQuery][EnumDataType(typeof(Order))] Order order = Order.asc)
    {
        var result = await tagsService.GetTagsAsync(page, pageSize, sort, order);
        if (result.IsSuccess)
            return TypedResults.Ok(result.Tags);

        return TypedResults.BadRequest(result.Error);
    }

    /// <summary>
    /// Gets a tag by id
    /// </summary>
    /// <param name="tagsService"></param>
    /// <param name="id">The unique id of the tag to get.</param>
    public static async Task<Results<Ok<List<Tag>>, NotFound<Error>>> GetTagById(
        TagsService tagsService,
        [FromRoute] int id)
    {
        ApiResult result = await tagsService.GetTagByIdAsync(id);
        if (result.IsSuccess)
            return TypedResults.Ok(result.Tags);
        return TypedResults.NotFound(result.Error);
    }

    /// <summary>
    /// Forces tags to be re-downloaded to the database
    /// </summary>
    /// <param name="tagsService"></param>
    /// <param name="minTotal">Minimal total number of tags to be downloaded</param>
    /// <param name="pageSize">Number of tags on one page in Stack Overflow API pagination</param>
    /// <param name="sort">Field to sort by</param>
    /// <param name="order">Order of sorting</param>
    public static async Task<Results<Ok, BadRequest<Error>>> DownloadTags(
        TagsService tagsService,
        [FromQuery] int minTotal = 1000,
        [FromQuery] int pageSize = 100,
        [FromQuery][EnumDataType(typeof(StackAPISort))] StackAPISort sort = StackAPISort.popular,
        [FromQuery][EnumDataType(typeof(Order))] Order order = Order.desc)
    {
        var result = await tagsService.DownloadTags(minTotal, pageSize, sort.ToString(), order.ToString());
        if (result.IsSuccess)
            return TypedResults.Ok();

        return TypedResults.BadRequest(result.Error);
    }
}
