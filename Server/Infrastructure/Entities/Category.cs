using LinqToDB.Mapping;

namespace Infrastructure.Entities;

public class Category
{
    [PrimaryKey, Identity] public int Id { get; set; }
    [Column] public string Name { get; set; } = "";
}