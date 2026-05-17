namespace App.Tests;

public class CRUDTests
{
    [Theory]
    [InlineData("Name","TEXT","string")]
    [InlineData("","","")]// тут надо null вместо этой хуйни
    [InlineData("1","2","3")]
    public void Create_PassValid_Success(string name, string typeOfSqlite, string typeInCSharp)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        CRUD.Create(name, typeOfSqlite, typeInCSharp).Wait();
        var result = db.Notes.Select(x => x.Name).Contains(name);
        //Assert
        Assert.True(result); //нужна очистка бд коректная
    }
    
    [Theory]
    [InlineData("Id")]
    [InlineData("")]// тут надо null вместо этой хуйни
    [InlineData("4")]
    public void Raed_PassValid_Success(string search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        CRUD.Create(search, "", "").Wait(); // тут не должно быть метода создания так как если он упадет то упадет все
        var result = CRUD.Read(search).Result.Select(x => x.Name).Contains(search);
        //Assert
        Assert.True(result);//нужна очистка бд коректная
    }
    
    [Theory]
    [InlineData("Value")]
    [InlineData("5")]//добавить null если потребуется
    public void Read_PassError_Fail(string search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var result = CRUD.Read(search).Result.Select(x => x.Name).Contains(search);
        //Assert
        Assert.False(result);//нужна очистка бд коректная
    }
    
    [Theory]
    [InlineData("Rank")]
    [InlineData("6")]//добавить null если потребуется
    public void Update_PassValid_Success(string changes) // проверка не существующей записи надо сделать
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var record = CRUD.Create("", "", "").Result;// тут не должно быть метода создания так как если он упадет то упадет все
        CRUD.Update(record,changes, "", "").Wait();
        var result = db.Notes.Select(x => x.Name).Contains(changes);
        //Assert
        Assert.True(result);//нужна очистка бд коректная
    }
    
    [Theory]
    [InlineData("Level")]
    [InlineData("7")]//добавить null если потребуется
    public void Delete_PassValid_Success(string search)// также проверка на удаление не существующей записи нужно сделать
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var record = CRUD.Create(search, "", "").Result;// тут не должно быть метода создания так как если он упадет то упадет все
        CRUD.Delete(record).Wait();
        var result = db.Notes.Select(x => x.Name).Contains(search);
        //Assert
        Assert.False(result);//нужна очистка бд коректная
    }
}