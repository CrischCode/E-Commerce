using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using E_Commerce_Frontend;
using E_Commerce_Frontend.Services;
using E_Commerce_Frontend.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configuramos el HttpClient para que siempre apunte a tu API de C#
builder.Services.AddScoped(sp => new HttpClient { 
    BaseAddress = new Uri("https://localhost:7253/") 
});

//serices
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IPersonaService, PersonaService>();
builder.Services.AddScoped<IPedidos, PedidoService>();
builder.Services.AddScoped<ICarrito, CarritoService>();
builder.Services.AddScoped<ICatalogo, CatalogoService>();


await builder.Build().RunAsync();
