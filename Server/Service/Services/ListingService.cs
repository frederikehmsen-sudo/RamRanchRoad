using Infrastructure;
using Infrastructure.Entities;

namespace Service.Services;

public class ListingService(RamRanchDatabase db)
{
    public List<Listing> GetListings()
    {
        return db.Listings.ToList();
    }
}