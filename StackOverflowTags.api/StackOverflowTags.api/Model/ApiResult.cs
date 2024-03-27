namespace StackOverflowTags.api.Model;

public class ApiResult
{
    private ApiResult(bool success, Error? error = null, List<Tag>? tags = null)
    {
        IsSuccess = success;
        Error = error;
        Tags = tags;
    }

    private ApiResult(bool success, List<Tag>? tags = null)
    {
        IsSuccess = success;
        Tags = tags;
    }

    public bool IsSuccess { get; }
    public Error? Error { get; } = null;
    public List<Tag>? Tags { get; } = null;

    public static ApiResult Success(List<Tag>? tags = null) => new(true, tags);
    public static ApiResult Failure(Error error) => new(false, error);
}
