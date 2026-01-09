using Ecommerce.API.Data;
using Ecommerce.API.Service;
using Ecommerce.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using dotenv.net;

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
    });

//DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseNpgsql(connectionString)
);

builder.Services.AddControllers();

//Controllers
builder.Services.AddControllers();

//Aplication Service
builder.Services.AddScoped<IPersonaService, PersonaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();


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
app.Run();
