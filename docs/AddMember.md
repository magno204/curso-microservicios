# Documentación de AddMember

Primero, crea la infraestructura que necesitas, ejecuta el script **cloudDeployment** dentro de la carpeta **infrastructure**.

```bash
bash cloudDeployment.sh
```

Agrega los siguientes paquetes de Nuget en tu proyecto **AddMember** y después ejecútalo.

```bash
dotnet add package Azure.Messaging.ServiceBus

dotnet run
```

Verifica que puedes acceder a tu interfaz de swagger y además al método que **weatherforecast**.

## Código del proyecto

Crea una carpeta llamada **Data**, después crea un archivo **ServiceBus.cs** y agrega:

```csharp
using Azure.Messaging.ServiceBus;
using System.Text;
using System.Text.Json;

namespace AddMember.Data
{
    public class ServiceBus
    {
        private readonly string _connectionString;
        private readonly string _queueName;

        public ServiceBus(string connectionString, string queueName)
        {
            _connectionString = connectionString;
            _queueName = queueName;
        }

        public async Task SendMessageAsync(string name, string lastname, string birthyear)
        {
            var client = new ServiceBusClient(_connectionString);
            var sender = client.CreateSender(_queueName);

            var messageBody = ($"Name: {name}, Lastname: {lastname}, Birthyear: {birthyear}");
            var jsonMessage = JsonSerializer.Serialize(messageBody);
            var serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage));

            await sender.SendMessageAsync(serviceBusMessage);
        }
    }
}
```

Agrega lo siguiente en tu **appsettings.json**, agrega tu cadena de conexión.

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ServiceBus": {
    "ConnectionString": "Endpoint=sb://<your-service-bus-namespace>.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;Shared",
    "QueueName": "pickage"
  },
  "AllowedHosts": "*"
}
```

Reemplaza el contenido de tu archivo **Program.cs**.

```csharp
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
    var connectionString = builder.Configuration["ServiceBus:ConnectionString"];
    var queueName = builder.Configuration["ServiceBus:QueueName"];
    var serviceBus = new ServiceBus(connectionString, queueName);
    serviceBus.SendMessageAsync(name, lastname, birthyear).GetAwaiter().GetResult();
    return Results.Ok($"Miembro {name} agregado con éxito.");
})
.WithName("AddMember")
.WithOpenApi();

app.Run();
```

Y por último puedes hacer la consulta usando tu archivo http, ya sabes, Swagger no siempre estará ahí.

```http
@AddMember_HostAddress = http://localhost:5236

POST {{AddMember_HostAddress}}/addmember?name=Miranda&lastname=Espinoza&birthyear=2017
Content-Type: application/json
Accept: application/json
```

