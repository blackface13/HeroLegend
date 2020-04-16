using UnityEngine;


using System.IO;

public class SQLiteCore
{
    //public static string connection = "URI=file:" + Application.dataPath + "/Datas/DB.db";
    //public static IDbConnection dbcon = new SqliteConnection(connection);
    //public IDbCommand dbcmd;
    //public SQLiteCore()
    //{
    //    dbcon = new SqliteConnection(connection);
    //    if (dbcon.State != System.Data.ConnectionState.Open)
    //        dbcon.Open();
                             
    //}
    //public IDataReader gettest()
    //{
    //    dbcmd = dbcon.CreateCommand();
    //    string query =
    //      "select * from test";
    //    dbcmd.CommandText = query;
    //    IDataReader reader = dbcmd.ExecuteReader();
    //    return reader;
    //}
    //public IDataReader _getEnemy()//Lấy dữ liệu Enemy đưa vào map
    //{
    //    try
    //    {
    //        dbcmd = dbcon.CreateCommand();
    //        dbcmd.CommandText = "select * from DBEnemy";
    //        IDataReader reader = dbcmd.ExecuteReader();
    //        dbcmd.Dispose();//Phải có đóng hàm sau khi truy vấn xong
    //        return reader;
    //    }
    //    catch
    //    {
    //        dbcmd.Dispose();//Phải có đóng hàm sau khi truy vấn xong
    //        return null;
    //    }
    //}
    //public IDataReader _getHero()//Lấy dữ liệu Hero
    //{
    //    try
    //    {
    //        dbcmd = dbcon.CreateCommand();
    //        dbcmd.CommandText = "select * from DBHero";
    //        IDataReader reader = dbcmd.ExecuteReader();
    //        dbcmd.Dispose();//Phải có đóng hàm sau khi truy vấn xong
    //        return reader;
    //    }
    //    catch
    //    {
    //        dbcmd.Dispose();//Phải có đóng hàm sau khi truy vấn xong
    //        return null;
    //    }
    //}
    //public IDataReader _getItem_Gem()//Lấy dữ liệu Item gem
    //{
    //    try
    //    {
    //        dbcmd = dbcon.CreateCommand();
    //        dbcmd.CommandText = "select * from Item_Gem";
    //        IDataReader reader = dbcmd.ExecuteReader();
    //        dbcmd.Dispose();//Phải có đóng hàm sau khi truy vấn xong
    //        return reader;
    //    }
    //    catch
    //    {
    //        dbcmd.Dispose();//Phải có đóng hàm sau khi truy vấn xong
    //        return null;
    //    }
    //}
}
