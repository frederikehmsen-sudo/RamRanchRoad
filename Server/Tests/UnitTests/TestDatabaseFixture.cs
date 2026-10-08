using Infrastructure;
using Infrastructure.Entities;
using LinqToDB;

namespace Service.UnitTests;

public class TestDatabaseFixture
{
    public RamRanchDatabase CreateConnection()
    {
        var options = new DataOptions<RamRanchDatabase>(
            new DataOptions().UseSQLite("Data Source=:memory:"));
        var db = new RamRanchDatabase(options);

        db.CreateTable<User>();
        db.CreateTable<Category>();
        db.CreateTable<Listing>();
        db.CreateTable<Order>();

        db.Insert(new User { Username = "ordinaryUser" }); // Id 1 (the buyer)
        db.Insert(new User { Username = "exampleUser" }); // Id 2 (the vendor)
        db.Insert(new Category { Name = "Drugs" }); // Id 1
        return db;
    }
}