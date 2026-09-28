using LinqToDB.Mapping;

namespace Infrastructure.Entities;

public class Category
{
    [PrimaryKey] public int Id { get; set; }
    [Column] public string Name { get; set; } = "";
}