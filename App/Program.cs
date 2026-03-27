using App;
using Microsoft.EntityFrameworkCore;

await using var db = new DataContext();
//await db.Database.EnsureCreatedAsync();
var records =await db.Notes.ToListAsync();
foreach (var record in records)
{
    Console.WriteLine($"{record.Name}, {record.TypeOfSqlite}, {record.TypeInCSharp}");
}