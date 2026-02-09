using AyudaIguales.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container - MODIFICADO: Añadir opciones JSON aquí
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        // Permite convertir strings a enums automáticamente
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost/ayuda_iguales/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddScoped<ICentroService, CentroService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAyudaService, AyudaService>();
builder.Services.AddScoped<IValoracionService, ValoracionService>();
builder.Services.AddScoped<IRespuestaService, RespuestaService>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// AÑADIR SESIÓN antes de Authorization
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();