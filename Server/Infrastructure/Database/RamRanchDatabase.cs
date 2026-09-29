using Infrastructure.Entities;
using LinqToDB;
using LinqToDB.Data;

namespace Infrastructure;


public class RamRanchDatabase : DataConnection
{
    public RamRanchDatabase(DataOptions<RamRanchDatabase> options) : base(options.Options)
    {
    }
    public ITable<Listing> Listings => this.GetTable<Listing>();
    public ITable<User> Users => this.GetTable<User>();
    public ITable<Order> Orders => this.GetTable<Order>();
    public ITable<Category> Categories => this.GetTable<Category>();
}