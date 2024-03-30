using Microsoft.EntityFrameworkCore;
using Serilog;
using StackOverflowTags.api.Endpoints;
using StackOverflowTags.api.Data;
using StackOverflowTags.api.Data.Interfaces;
using StackOverflowTags.api.Data.Repositories;
using StackOverflowTags.api.Exceptions;
using StackOverflowTags.api.Extensions;
using StackOverflowTags.api.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); ;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var docFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, docFile));
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDB"));
});

builder.Services.AddScoped<TagsService>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

builder.Services.AddHttpClient<TagsService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("ApiURL").Value!);
});

builder.Services.AddHostedService<TagsInitializationService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.AddMigrations();
}

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.MapTagsEndpoints();

app.Run();
