# Documentación para GetAddults

Agrega los siguientes paquetes de Nuget en tu proyecto **AddAdult** y después ejecútalo.

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

dotnet run
```

## Código del proyecto

Reemplaza el contenido de **appsettings.json** con lo siguiente:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:amines..."
  },
  "AllowedHosts": "*"
}
```

Reemplaza el contenido de **Program.cs**

```csharp
using Microsoft.EntityFrameworkCore;
using GetAdults.Data;
using GetAdults.Models;

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

app.Run();
```

Agrega una carpeta llamada **Data** y crea un archivo llamado **DataContext.cs**, agrega lo siguiente.

```csharp
using Microsoft.EntityFrameworkCore;
using GetAdults.Models;

namespace GetAdults.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Adult> Adults { get; set; }
    }
}
```

Agrega una carpeta llamada **Models** y crea un arhivo llamado **Adult.cs**, agrega lo siguiente.

```csharp
namespace GetAdults.Models
{
    public class Adult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public int BirthYear { get; set; }
        public string ImageUrl { get; set; }
    }
}
```

Recuerda que puedes usar el archivo **.http** para probasr tus peticiones.

```http
@GetAdults_HostAddress = http://localhost:5079
@Docker_HostAddress = http://localhost:8080

GET {{GetAdults_HostAddress}}/Adults/
Accept: application/json

###
GET {{Docker_HostAddress}}/Adults/
Accept: application/json
```
