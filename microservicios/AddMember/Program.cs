using AddMember.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/addmember", (string name, string lastname, string birthyear) =>
{
    var connectionString = builder.Configuration["ServiceBus:ConnectionString"] ?? Environment.GetEnvironmentVariable("SERVICEBUS_CONNECTION_STRING");
    var queueName = builder.Configuration["ServiceBus:QueueName"] ?? Environment.GetEnvironmentVariable("SERVICE_BUS_QUEUE_NAME");;
    var serviceBus = new ServiceBus(connectionString, queueName);
    serviceBus.SendMessageAsync(name, lastname, birthyear).GetAwaiter().GetResult();
    return Results.Ok($"Miembro {name} agregado con éxito.");
})
.WithName("AddMember")
.WithOpenApi();

app.Run();