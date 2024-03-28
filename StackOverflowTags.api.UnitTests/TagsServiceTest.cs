using Microsoft.Extensions.Logging;
using Moq;
using StackOverflowTags.api.Data;
using StackOverflowTags.api.Model;
using StackOverflowTags.api.Services;
using System.Net;

namespace StackOverflowTags.api.UnitTests;

public class TagsServiceTest
{
    private readonly HttpClient _httpClient;
    private readonly Mock<ApplicationDbContext> _contextMock;
    private readonly Mock<ILogger<TagsService>> _loggerMock;
    private readonly TagsService _tagsService;
    private readonly List<Tag> _sampleTags = [
        new Tag {Name = "action-menu", Count = 28, Is_required = false, Has_synonyms = false, Is_moderator_only = false, Population_percentage = 0.043},
        new Tag {Name = "actionmenuview", Count = 5, Is_required = false, Has_synonyms = false, Is_moderator_only = false, Population_percentage = 0.011},
        new Tag {Name = "actionmethod", Count = 135, Is_required = false, Has_synonyms = false, Is_moderator_only = false, Population_percentage = 0.145},
        new Tag {Name = "actionmode", Count = 58, Is_required = false, Has_synonyms = false, Is_moderator_only = false, Population_percentage = 0.088},
        new Tag {Name = "action-open-document-tree", Count = 10, Is_required = false, Has_synonyms = false, Is_moderator_only = false, Population_percentage = 0.020},
    ];

    public TagsServiceTest()
    {
        _httpClient = new HttpClient(new HttpMessageHandlerMock(HttpStatusCode.OK));
        var mockSet = MockHelper.GetMockDbSet<Tag>(_sampleTags.AsQueryable());
        _contextMock = new Mock<ApplicationDbContext>();
        _loggerMock = new Mock<ILogger<TagsService>>();
        _tagsService = new TagsService(_httpClient, _contextMock.Object, _loggerMock.Object);
    }

    /*private DbSet<Tag> MockDbSet(List<Tag> tags)
    {
        var queryable = tags.AsQueryable();
        var mockDbSet = new Mock<DbSet<Tag>>();
        mockDbSet.As<IQueryable<Tag>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockDbSet.As<IQueryable<Tag>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockDbSet.As<IQueryable<Tag>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockDbSet.As<IQueryable<Tag>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        return mockDbSet.Object;
    }*/

    [Fact]
    public async Task DownloadTags_MinTagsCountIsInvalid_ShouldReturnFailureResult()
    {
        // Arrange
        int minTagsCount = 0;

        // Act
        var result = await _tagsService.DownloadTags(minTagsCount);

        // Assert
        Assert.True(!result.IsSuccess && result.Error == TagsErrors.TagsCount);
    }

    [Fact]
    public async Task DownloadTags_PageSizeIsInvalid_ShouldReturnFailureResult()
    {
        // Arrange
        int pageSize = 0;

        // Act
        var result = await _tagsService.DownloadTags(pageSize: pageSize);

        // Assert
        Assert.True(!result.IsSuccess && result.Error == TagsErrors.PageSize);
    }

    [Fact]
    public async Task GetTags_PageNumberInvalid_ShouldReturnFailureResult()
    {
        // Arrange
        int page = 0;

        // Act
        var result = await _tagsService.GetTagsAsync(page, 10);

        // Assert
        Assert.True(!result.IsSuccess && result.Error == TagsErrors.Page);
    }

    [Fact]
    public async Task GetTags_PageSizeInvalid_ShouldReturnFailureResult()
    {
        // Arrange
        int pageSize = 0;

        // Act
        var result = await _tagsService.GetTagsAsync(1, pageSize);

        // Assert
        Assert.True(!result.IsSuccess && result.Error == TagsErrors.PageSize);
    }

    /*[Fact]
    public async Task GetTags_ValidParameters_OrderByNameAsc_SchouldReturnSuccessResult()
    {
        // Arrange
        int page = 1;
        int pageSize = 5;

        // Act
        var result = await _tagsService.GetTagsAsync(page, pageSize);

        // Assert
        Assert.True(result.IsSuccess);
    }*/

    /*[Fact]
    public async Task DownloadTags_ValidParameters_ShouldReturnSuccessResult()
    {
        // Arrange

        // Act
        var result = await _tagsService.DownloadTags(5, 5);

        // Assert
        Assert.True(result.IsSuccess);
    }*/


}