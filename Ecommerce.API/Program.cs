using Ecommerce.API.Data;
using Ecommerce.API.Service;
using Ecommerce.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using dotenv.net;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
//.Env
DotEnv.Load();
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

var builder = WebApplication.CreateBuilder(args);

//Los campos null NO se serialicen y Swagger deje de enviarlos automáticamente
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

        //ignora los ciclos de los objetos
        opt.JsonSerializerOptions.ReferenceHandler = 
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

//DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseNpgsql(connectionString)
);

//Controllers
builder.Services.AddControllers();

//Aplication Service
builder.Services.AddScoped<IPersonaService, PersonaService>();
builder.Services.AddScoped<ICliente, ClienteService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ICategoria, CategoriaService>();
builder.Services.AddScoped<IPedido, PedidoService>();
builder.Services.AddScoped<IMovimientoInventario, MovimientoInventarioService>();


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseCors(options => // esto ayuda a conectar el front con la API
{
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});
app.Run();
