using Microsoft.EntityFrameworkCore;
using StackOverflowTags.api.Data;
using StackOverflowTags.api.Extensions;
using StackOverflowTags.api.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); ;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDB"));
});

builder.Services.AddScoped<TagsService>();

builder.Services.AddHttpClient<TagsService>(client =>
{
    client.BaseAddress = new Uri("https://api.stackexchange.com/2.3/");
});

builder.Services.AddHostedService<TagsInitializationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.AddMigrations();
}

/*app.UseHttpsRedirection();

app.UseAuthorization();*/

app.MapControllers();

app.Run();
