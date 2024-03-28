using System.Net;

namespace StackOverflowTags.api.UnitTests;

internal class HttpMessageHandlerMock : HttpMessageHandler
{
    public HttpStatusCode _code;
    public HttpMessageHandlerMock(HttpStatusCode code)
    {
        _code = code;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new HttpResponseMessage()
        {
            StatusCode = _code,
            Content = new StringContent(StaticVars.Response)
        });
    }
}
