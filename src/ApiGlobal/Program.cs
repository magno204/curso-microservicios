using Microsoft.EntityFrameworkCore;
using ApiGlobal.Data;
using ApiGlobal.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

async Task<List<Adult>> GetAdults(DataContext context) => await context.Adults.ToListAsync();
app.MapGet("/Adults", async (DataContext context) => await GetAdults(context))
.WithName("GetAdults")
.WithOpenApi();

async Task<List<Child>> GetChildren(DataContext context) => await context.Children.ToListAsync();
app.MapGet("/Children", async (DataContext context) => await GetChildren(context))
.WithName("GetChildren")
.WithOpenApi();

app.MapGet("/Adults/{id}", async (DataContext context, int id) => await context.Adults.FindAsync(id))
.WithName("GetAdultById")
.WithOpenApi();

app.MapGet("/Children/{id}", async (DataContext context, int id) => await context.Children.FindAsync(id))
.WithName("GetChildById")
.WithOpenApi();

app.MapPost("Add/Adults",async(DataContext context, Adult item) =>
{
    context.Adults.Add(item);
    await context.SaveChangesAsync();
    return Results.Ok(await GetAdults(context));
})
.WithName("AddAdult")
.WithOpenApi();

app.MapPost("Add/Children",async(DataContext context, Child item) =>
{
    context.Children.Add(item);
    await context.SaveChangesAsync();
    return Results.Ok(await GetChildren(context));
})
.WithName("AddChild")
.WithOpenApi();

app.Run();

