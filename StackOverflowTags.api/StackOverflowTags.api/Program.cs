using Microsoft.EntityFrameworkCore;
using StackOverflowTags.api.Data;
using StackOverflowTags.api.Extensions;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDB"));
});

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
