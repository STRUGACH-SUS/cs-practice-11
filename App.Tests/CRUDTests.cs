namespace App.Tests;

public class CRUDTests
{
    [Theory]
    [InlineData("Name","TEXT","string")]
    [InlineData("","","")]
    [InlineData("1","2","3")]
    public void Create_PassValid_Success(string name, string typeOfSqlite, string typeInCSharp)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        CRUD.Create(name, typeOfSqlite, typeInCSharp);
        var result = db.Notes.Select(x => x.Name).Contains(name);
        //Assert
        Assert.True(result);
    }
    
    [Theory]
    [InlineData("Id")]
    [InlineData("")]
    [InlineData("4")]
    public void Raed_PassValid_Success(string search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        CRUD.Create(search, "", "");
        var result = CRUD.Read(search).Result.Select(x => x.Name).Contains(search);
        //Assert
        Assert.True(result);
    }
    
    [Theory]
    [InlineData("Value")]
    [InlineData("5")]
    public void Read_PassError_Fail(string search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var result = CRUD.Read(search).Result.Select(x => x.Name).Contains(search);
        //Assert
        Assert.False(result);
    }
    
    [Theory]
    [InlineData("Rank")]
    [InlineData("6")]
    public void Update_PassValid_Success(string changes)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var record = CRUD.Create("", "", "").Result;
        CRUD.Update(record,changes, "", "");
        var result = db.Notes.Select(x => x.Name).Contains(changes);
        //Assert
        Assert.True(result);
    }
    
    [Theory]
    [InlineData("Level")]
    [InlineData("7")]
    public void Delete_PassValid_Success(string search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var record = CRUD.Create(search, "", "").Result;
        CRUD.Delete(record);
        var result = db.Notes.Select(x => x.Name).Contains(search);
        //Assert
        Assert.False(result);
    }
}