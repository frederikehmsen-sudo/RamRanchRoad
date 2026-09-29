using System.Security.AccessControl;
using Facet;
using Infrastructure.Entities;

namespace Service.DTOs;

[Facet(typeof(Listing), nameof(Listing.Vendor), nameof(Listing.Category))]
public partial class ListingResponse
{
    public string VendorName { get; set; } = "";
    public string CategoryName { get; set; } = "";
}