using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StackOverflowTags.api.Model;
using StackOverflowTags.api.Services;
using System.ComponentModel.DataAnnotations;

namespace StackOverflowTags.api.Endpoints;

public static class TagsEndpoints
{

    public static void MapTagsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/tags");
        group.MapGet("", GetTags);
        group.MapGet("download-tags", DownloadTags);
        group.MapGet("{id}", GetTagById);
    }

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

    public static async Task<Results<Ok<List<Tag>>, NotFound<Error>>> GetTagById(
        TagsService tagsService,
        [FromRoute] int id)
    {
        ApiResult result = await tagsService.GetTagByIdAsync(id);
        if (result.IsSuccess)
            return TypedResults.Ok(result.Tags);
        return TypedResults.NotFound(result.Error);
    }
}
