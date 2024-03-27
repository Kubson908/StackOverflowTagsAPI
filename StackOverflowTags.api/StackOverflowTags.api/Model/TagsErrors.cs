namespace StackOverflowTags.api.Model;

public static class TagsErrors
{
    public static readonly Error TagsCount = new("Tags.TagsCount", "Tags count should be greater than 0");
    public static readonly Error Page = new("Tags.Page", "Page number should be greater than 0");
    public static readonly Error PageSize = new("Tags.PageSize", "Page size should be greater than 0");
}
