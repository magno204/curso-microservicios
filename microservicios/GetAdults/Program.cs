using Microsoft.EntityFrameworkCore;
using GetAdults.Data;
using GetAdults.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");


builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

async Task<List<Adult>> GetAdults(DataContext context) => await context.Adults.ToListAsync();
app.MapGet("/Adults", async (DataContext context) => await GetAdults(context))
.WithName("GetAdults")
.WithOpenApi();

app.Run();