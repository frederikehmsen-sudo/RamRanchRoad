using Infrastructure;
using Infrastructure.Entities;
using LinqToDB;

namespace DefaultNamespace;

public class RamRanchSeeder(RamRanchDatabase db)
{
    public void Seed()
    {
        db.CreateTable<User>(tableOptions: TableOptions.CreateIfNotExists);
        db.CreateTable<Category>(tableOptions: TableOptions.CreateIfNotExists);
        db.CreateTable<Listing>(tableOptions: TableOptions.CreateIfNotExists);
        db.CreateTable<Order>(tableOptions: TableOptions.CreateIfNotExists);


        if (db.Users.Count() == 0)
        {
            db.Insert(new User { Username = "ordinaryUser" });
            db.Insert(new User { Username = "exampleUser" });
        }

        if (db.Categories.Count() == 0)
        {
            db.Insert(new Category { Name = "Drugs" });
            db.Insert(new Category { Name = "Weaponry" });
            db.Insert(new Category { Name = "Stolen Artifacts" });
        }

        if (db.Listings.Count() == 0)
        {
            db.Insert(new Listing
            {
                VendorId = 2,
                CategoryId = 1,
                Title = "Fake Coke, 1g",
                Description = "Definitely not baking soda",
                Price = 20m,
                Stock = 50
            });
        }
    }
}