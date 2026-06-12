using Microsoft.EntityFrameworkCore;

namespace App.Tests;

public class CRUDTests
{
    [Theory]
    [InlineData("Name")]
    [InlineData("1")]
    [InlineData("")]
    public void Create_PassValid_Success(string name)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreatedAsync();
        CRUD.Create(name);
        var result = db.Notes.Select(x => x.Name).Contains(name);
        //Assert
        db.Database.EnsureDeletedAsync();
        Assert.True(result);
    }
    
    [Fact]
    public void Create_PassNull_Fail()
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreatedAsync();
        //Assert
        db.Database.EnsureDeletedAsync();
        Assert.Throws<AggregateException>(() => CRUD.Create(null!).Result);
    }
    
    [Theory]
    [InlineData("Id")]
    [InlineData("4")]
    [InlineData("")]
    public void Raed_PassValid_Success(string search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreatedAsync();
        db.Notes.Add(new Note
        {
            Name = search,
            CreatedAt = DateTimeOffset.Now
        });
        db.SaveChangesAsync();
        var result = CRUD.Read(search).Result.Select(x => x.Name).Contains(search);
        //Assert
        db.Database.EnsureDeletedAsync();
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
        db.Database.EnsureCreatedAsync();
        var result = CRUD.Read(search).Result.Select(x => x.Name).Contains(search);
        //Assert
        db.Database.EnsureDeletedAsync();
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
        db.Database.EnsureCreatedAsync();
        var record = new Note
        {
            Name = "",
            CreatedAt = DateTimeOffset.Now
        };
        db.Notes.Add(record);
        CRUD.Update(record,changes);
        var result = db.Notes.Select(x => x.Name).Contains(changes);
        //Assert
        db.Database.EnsureDeletedAsync();
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
        db.Database.EnsureCreatedAsync();
        var record = new Note
        {
            Name = "",
            CreatedAt = DateTimeOffset.Now
        };
        CRUD.Update(record,changes);
        var result = record.Id == 0;
        //Assert
        db.Database.EnsureDeletedAsync();
        Assert.False(result);
    }
    
    [Fact]
    public async void Update_PassNull_Fail()
    {
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var record = new Note
        {
            Name = "",
            CreatedAt = DateTimeOffset.Now
        };
        //Assert
        await db.Database.EnsureDeletedAsync();
        await Assert.ThrowsAsync<DbUpdateException>(()=>CRUD.Update(record,null!));
    }
    
    [Theory]
    [InlineData("Level")]
    [InlineData("7")]
    [InlineData("")]
    public void Delete_PassValid_Success(string search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreatedAsync();
        var record = new Note
        {
            Name = search,
            CreatedAt = DateTimeOffset.Now
        };
        db.Notes.Add(record);
        db.SaveChanges();
        CRUD.Delete(record);
        var result = db.Notes.Select(x => x.Name).Contains(search);
        //Assert
        db.Database.EnsureDeletedAsync();
        Assert.False(result);
    }
    
    [Theory]
    [InlineData("Level")]
    [InlineData("7")]
    [InlineData("")]
    public async void Delete_PassError_Fail(string search)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var record = new Note
        {
            Name = search,
            CreatedAt = DateTimeOffset.Now
        };
        //Assert
        await db.Database.EnsureDeletedAsync();
        await Assert.ThrowsAsync<InvalidOperationException>(()=>CRUD.Delete(record));
    }
}