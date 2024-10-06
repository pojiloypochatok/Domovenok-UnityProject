using UnityEngine;
using SQLite;
using System.IO;

[Table("Coins")]
public class Coins{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int Id { get; set; }
    [Column("gold")]
    public int Gold { get; set; }
    [Column("silver")]
    public int Silver { get; set; }
}

public class Energy{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int Id { get; set; }
    [Column("amount")]
    public int Amount { get; set; }
}

[Table("Inventory")]
public class Inventory
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("BrokenLamp")]
    public int BrokenLamp { get; set; } 
    
    [Column("BrokenPicture")]
    public int BrokenPicture { get; set; }
    
    [Column("BrokenPot")]
    public int BrokenPot { get; set; }
    
    [Column("Chocolate")]
    public int Chocolate { get; set; }
}

public class Collections
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("Tree")]
    public int Tree { get; set; } 
    
    [Column("Pazzle")]
    public int Pazzle { get; set; }
    
    [Column("Shirt")]
    public int Shirt { get; set; }
    
    [Column("Leaf")]
    public int Leaf { get; set; }
}


public class DataBaseMain : MonoBehaviour
{
    public readonly string dataPath = Path.Combine(Application.dataPath, "DataBaseStorage", "base.db");

    void Start()
    {
        if (!File.Exists(dataPath)){
            CreateDatabase();
        }
        else{
            ConnectToDatabase();
        }
    }


    private void CreateDatabase()
    {
        using var db = new SQLiteConnection(dataPath);
        Debug.Log("База данных создана успешно.");

        db.CreateTable<Coins>();
        db.CreateTable<Energy>();
        db.CreateTable<Inventory>();
        db.CreateTable<Collections>();

        db.Commit();
        Debug.Log("Таблицы созданы!");
    }

    private void ConnectToDatabase(){
        using var db = new SQLiteConnection(dataPath);
        Debug.Log("Подключение к базе данных установлено.");
    }
}


public static class DatabaseManager
{
    private static SQLiteConnection _connection;

    public static SQLiteConnection GetConnection()
    {
        if (_connection == null)
        {
            string dataPath = Path.Combine(Application.dataPath, "DataBaseStorage", "base.db");
            _connection = new SQLiteConnection(dataPath);
        }

        return _connection;
    }
}


