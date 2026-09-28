using Infrastructure;
using Infrastructure.Entities;
using LinqToDB;

var builder = WebApplication.CreateBuilder(args);

var options = new DataOptions<RamRanchDatabase>(new DataOptions().UseSQLite("Data Source=db.db"));
builder.Services.AddScoped<RamRanchDatabase>(_ => new RamRanchDatabase(options));

builder.Services.AddScoped<RamRanchDatabase>();
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RamRanchDatabase>();
    db.CreateTable<User>();
    db.CreateTable<Listing>();
    db.CreateTable<Order>();
    db.CreateTable<Category>();

    var ordinaryUser = db.InsertWithInt32Identity(new User() { Username = "ordinaryUser" });
    var exampleUser = db.InsertWithInt32Identity(new User() { Username = "exampleUser" });

    db.Insert(new Listing
    {
        VendorId = exampleUser,
        CategoryId = 1,
        Title = "Fake Coke, 1g",
        Description = "Definitely not baking soda",
        Price = 20m,
        Stock = 50
    });
}

app.MapControllers();
app.Run();
