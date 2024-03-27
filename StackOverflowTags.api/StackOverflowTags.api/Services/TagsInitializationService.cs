namespace StackOverflowTags.api.Services;

public class TagsInitializationService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    public TagsInitializationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<TagsService>();
        await service.CheckOnStartup();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
