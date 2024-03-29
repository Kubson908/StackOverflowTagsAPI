using System.Net;

namespace StackOverflowTags.api.IntegrationTests;

public class TagsControllerTests : IClassFixture<AppFactory>
{
    private readonly HttpClient _httpClient;
    public TagsControllerTests(AppFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }

    [Fact]
    public async Task DownloadTags_InvalidTagsCount_ShouldReturnBadRequest()
    {
        // Arrange
        int minTotal = 0, pageSize = 10;

        // Act
        var response = await _httpClient.GetAsync($"/api/tags/download-tags?minTotal={minTotal}&pageSize={pageSize}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DownloadTags_InvalidPageSize_ShouldReturnBadRequest()
    {
        // Arrange
        int minTotal = 100, pageSize = 0;

        // Act
        var response = await _httpClient.GetAsync($"/api/tags/download-tags?minTotal={minTotal}&pageSize={pageSize}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DownloadTags_ValidParameters_ShouldReturnOk()
    {
        // Arrange
        int minTotal = 100, pageSize = 10;

        // Act
        var response = await _httpClient.GetAsync($"/api/tags/download-tags?minTotal={minTotal}&pageSize={pageSize}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetTags_InvalidPage_ShouldReturnBadRequest()
    {
        // Arrange
        int page = 0, pageSize = 10;

        // Act
        var response = await _httpClient.GetAsync($"/api/tags/get-tags?page={page}&pageSize={pageSize}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetTags_InvalidPageSize_ShouldReturnBadRequest()
    {
        // Arrange
        int page = 1, pageSize = 0;

        // Act
        var response = await _httpClient.GetAsync($"/api/tags/get-tags?page={page}&pageSize={pageSize}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetTags_ValidParameters_ShouldReturnOk()
    {
        // Arrange
        int page = 1, pageSize = 10;

        // Act
        var response = await _httpClient.GetAsync($"/api/tags/get-tags?page={page}&pageSize={pageSize}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
