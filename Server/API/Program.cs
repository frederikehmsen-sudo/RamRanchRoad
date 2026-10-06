using Api;
using DefaultNamespace;
using Infrastructure;
using Infrastructure.Entities;
using LinqToDB;
using Service.Services;

var builder = WebApplication.CreateBuilder(args);

var options = new DataOptions<RamRanchDatabase>(new DataOptions().UseSQLite("Data Source=db.db"));
builder.Services.AddScoped<RamRanchDatabase>(_ => new RamRanchDatabase(options));
builder.Services.AddScoped<OrderService>();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddScoped<RamRanchSeeder>();
builder.Services.AddScoped<ListingService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddControllers();
builder.Services.AddOpenApiDocument();
builder.Services.AddCors();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<RamRanchSeeder>().Seed();
}

app.UseExceptionHandler();
app.UseCors(config => config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().SetIsOriginAllowed(_ => true));
app.MapControllers();
app.UseOpenApi();
app.UseSwaggerUi();
app.Run();
