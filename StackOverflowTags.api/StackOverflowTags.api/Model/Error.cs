namespace StackOverflowTags.api.Model;

public sealed class Error(string Code, string Message)
{
    public string Code { get; } = Code;
    public string Message { get; } = Message;
}
