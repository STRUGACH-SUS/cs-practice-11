using Microsoft.EntityFrameworkCore;

namespace App.Tests;

public class CRUDTests
{
    [Theory]
    [InlineData("Name","TEXT","string")]
    [InlineData("1","1","1")]
    [InlineData("","","")]
    public void Create_PassValid_Success(string name, string typeOfSqlite, string typeInCSharp)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        CRUD.Create(name, typeOfSqlite, typeInCSharp).Wait();
        var result = db.Notes.Select(x => x.Name).Contains(name);
        //Assert
        db.Database.EnsureDeleted();
        Assert.True(result);
    }
    
    [Fact]
    public void Create_PassNull_Fail()
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        //Assert
        db.Database.EnsureDeleted();
        Assert.Throws<AggregateException>(() => CRUD.Create(null!, null!, null!).Wait());
    }
    
    [Theory]
    [InlineData("Id")]
    [InlineData("4")]
    [InlineData("")]
    public void Raed_PassValid_Success(string search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        db.Notes.Add(new Note
        {
            Name = search,
            TypeInCSharp = "",
            TypeOfSqlite = ""
        });
        db.SaveChangesAsync().Wait();
        var result = CRUD.Read(search).Result.Select(x => x.Name).Contains(search);
        //Assert
        db.Database.EnsureDeleted();
        Assert.True(result);
    }
    
    [Theory]
    [InlineData("Value")]
    [InlineData("5")]
    [InlineData("")]
    public void Read_PassError_Fail(string search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var result = CRUD.Read(search).Result.Select(x => x.Name).Contains(search);
        //Assert
        db.Database.EnsureDeleted();
        Assert.False(result);
    }

    [Fact]
    public void Read_PassNull_Fail()
    {
        
    }
    
    [Theory]
    [InlineData("Rank")]
    [InlineData("6")]
    [InlineData("")]
    public void Update_PassValid_Success(string changes)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var record = new Note
        {
            Name = "",
            TypeInCSharp = "",
            TypeOfSqlite = ""
        };
        db.Notes.Add(record);
        CRUD.Update(record,changes, "", "").Wait();
        var result = db.Notes.Select(x => x.Name).Contains(changes);
        //Assert
        db.Database.EnsureDeleted();
        Assert.True(result);
    }

    [Theory]
    [InlineData("Rank")]
    [InlineData("6")]
    [InlineData("")]
    public void Update_PassError_Fail(string changes)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var record = new Note
        {
            Name = "",
            TypeInCSharp = "",
            TypeOfSqlite = ""
        };
        CRUD.Update(record,changes, "", "").Wait();
        var result = record.Id == 0;
        //Assert
        db.Database.EnsureDeleted();
        Assert.False(result);
    }
    
    [Fact]
    public void Update_PassNull_Fail()
    {
        var db = new DataContext();
        db.Database.EnsureCreated();
        var record = new Note
        {
            Name = "1",
            TypeInCSharp = "1",
            TypeOfSqlite = "1"
        };
        //Assert
        db.Database.EnsureDeleted();
        Assert.Throws<AggregateException>(() => CRUD.Update(record, null!, null!, null!).Wait());
    }
    
    [Theory]
    [InlineData("Level")]
    [InlineData("7")]
    [InlineData("")]
    public void Delete_PassValid_Success(string search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var record = new Note
        {
            Name = search,
            TypeInCSharp = "",
            TypeOfSqlite = ""
        };
        db.Notes.Add(record);
        db.SaveChanges();
        CRUD.Delete(record).Wait();
        var result = db.Notes.Select(x => x.Name).Contains(search);
        //Assert
        db.Database.EnsureDeleted();
        Assert.False(result);
    }
    
    [Theory]
    [InlineData("Level")]
    [InlineData("7")]
    [InlineData("")]
    public void Delete_PassError_Fail(string search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var record = new Note
        {
            Name = search,
            TypeInCSharp = "",
            TypeOfSqlite = ""
        };
        //Assert
        Assert.Throws<AggregateException>(()=>CRUD.Delete(record).Wait());
    }
}