using System.ComponentModel.DataAnnotations;

namespace App;

public class Note
{
    public int Id { get; set; }
    public required  string  Name { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
}