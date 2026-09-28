using LinqToDB.Mapping;

namespace Infrastructure.Entities;

public class User
{
    [PrimaryKey] public int Id { get; set; }
    [Column] public string Username { get; set; } = "";
}