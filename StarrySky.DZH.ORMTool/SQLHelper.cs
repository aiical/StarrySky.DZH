using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;

/// <summary>
/// ADO.NET帮助类
/// </summary>
public class SQLHelper
{
    public SQLHelper()
    {
        //
        // TODO
        //
    }
    protected static OleDbConnection Connection()
    {
        OleDbConnection conn = new OleDbConnection();
        conn.ConnectionString = "Provider=OraOLEDB.Oracle.1;Password=ws005;Persist Security Info=True;User ID=ws005;Data Source=AVS_SFIS11G_DEV.WORLD";
        return conn;
    }
    public static string GetData(string sql)
    {
        OleDbConnection conn = Connection();
        conn.Open();
        OleDbDataAdapter da = new OleDbDataAdapter(sql, conn);
        DataTable dt = new DataTable();
        da.Fill(dt);
        conn.Close();
        DataRow dr = dt.Rows[0];
        return dr[0].ToString();
    }
    public static DataTable GetDataTable(string sql)
    {
        OleDbConnection conn = Connection();
        conn.Open();
        OleDbDataAdapter da = new OleDbDataAdapter(sql, conn);
        DataTable dt = new DataTable();
        da.Fill(dt);
        conn.Close();
        return dt;
    }
    public static int ExecSql(string sql)
    {
        OleDbConnection conn = Connection();
        conn.Open();
        OleDbCommand comm = new OleDbCommand(sql, conn);
        int result = comm.ExecuteNonQuery();
        conn.Close();
        return result;
    }   
}