// Decompiled with JetBrains decompiler
// Type: JT_Database_App.Validation
// Assembly: JT Database App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D649E70A-0AB7-4AF5-A104-CFA0EBCF124B
// Assembly location: C:\Users\Skye\Desktop\JT Database App 23aug21.exe

using JT_Database_App.Properties;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Diagnostics;

namespace JT_Database_App
{
  internal class Validation
  {
    public static int showForMins = 5;
    private static string BOConnectionStr { get => "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Settings.Default.BOPath; }

    //public Validation() => Validation.BOConnectionStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Settings.Default.BOPath;

    private static string select(string _where = "") => "SELECT * FROM \n(((((sales_counter AS sc \nLEFT JOIN cutter AS c ON sc.invsales = c.invcutter)\nLEFT JOIN drilling AS d ON sc.invsales = d.[inv drill])\nLEFT JOIN edger AS e ON sc.invsales = e.[inv edger])\nLEFT JOIN filed AS f ON sc.invsales = f.inv_filled)\nLEFT JOIN checker AS chk ON sc.invsales = chk.[inv checker])\nLEFT JOIN placed AS p ON sc.invsales = p.invQ_LU\n" + _where;

    public Dictionary<string, int> getHours()
    {
      Dictionary<string, int> hours = new Dictionary<string, int>();
      string cmdText = "SELECT boards, timber FROM delay_hours";
      try
      {
        using (OleDbConnection connection = new OleDbConnection(Validation.BOConnectionStr))
        {
          using (OleDbCommand oleDbCommand = new OleDbCommand(cmdText, connection))
          {
            connection.Open();
            using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
            {
              oleDbDataReader.Read();
              hours.Add("boards", oleDbDataReader.GetInt32(oleDbDataReader.GetOrdinal("boards")));
              hours.Add("timber", oleDbDataReader.GetInt32(oleDbDataReader.GetOrdinal("timber")));
              return hours;
            }
          }
        }
      }
      catch (OleDbException ex)
      {
        AutoClosingMessageBox.Show(ex.Message);
        return new Dictionary<string, int>()
        {
          {
            "boards",
            -1
          },
          {
            "timber",
            -1
          }
        };
      }
    }

    public int rep(string _repId)
    {
      string cmdText = "SELECT [employ no] FROM reps WHERE [first name] + ' ' + [employ no] = @rep";
      using (OleDbConnection connection = new OleDbConnection(Validation.BOConnectionStr))
      {
        connection.Open();
        using (OleDbCommand oleDbCommand = new OleDbCommand(cmdText, connection))
        {
          oleDbCommand.Parameters.AddWithValue("@rep", (object)_repId);
          using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
            return oleDbDataReader.Read() ? int.Parse(oleDbDataReader.GetString(0)) : -1;
        }
      }
    }

    public static string RepName(int _id)
    {
      string cmdText = "SELECT [first name] FROM reps WHERE [employ no] = @id";
      using (OleDbConnection connection = new OleDbConnection(Validation.BOConnectionStr))
      {
        connection.Open();
        using (OleDbCommand oleDbCommand = new OleDbCommand(cmdText, connection))
        {
          oleDbCommand.Parameters.AddWithValue("@id", (object)_id);
          return oleDbCommand.ExecuteScalar().ToString();
        }
      }
    }

    public int cutMachine(string _machineName)
    {
      string cmdText = "SELECT * FROM machines WHERE machine = @machine;";
      using (OleDbConnection connection = new OleDbConnection(Validation.BOConnectionStr))
      {
        using (OleDbCommand oleDbCommand = new OleDbCommand(cmdText, connection))
        {
          connection.Open();
          oleDbCommand.Parameters.AddWithValue("@machine", (object)_machineName);
          using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
            return oleDbDataReader.Read() ? oleDbDataReader.GetInt32(oleDbDataReader.GetOrdinal("ID")) : -1;
        }
      }
    }

    public int status(string _status)
    {
      string cmdText = "SELECT * FROM status WHERE status = @status;";
      using (OleDbConnection connection = new OleDbConnection(Validation.BOConnectionStr))
      {
        using (OleDbCommand oleDbCommand = new OleDbCommand(cmdText, connection))
        {
          connection.Open();
          oleDbCommand.Parameters.AddWithValue("@status", (object)_status);
          using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
            return oleDbDataReader.Read() ? oleDbDataReader.GetInt32(oleDbDataReader.GetOrdinal("ID")) : -1;
        }
      }
    }

    public static string StatusName(int _status)
    {
      string cmdText = "SELECT status FROM status WHERE ID = @status";
      using (OleDbConnection connection = new OleDbConnection(Validation.BOConnectionStr))
      {
        using (OleDbCommand oleDbCommand = new OleDbCommand(cmdText, connection))
        {
          connection.Open();
          oleDbCommand.Parameters.AddWithValue("@status", (object)_status);
          return (string)oleDbCommand.ExecuteScalar();
        }
      }
    }

    public int getCutStatus(string _inv)
    {
      string cmdText = "SELECT [cut status] FROM cutter WHERE invcutter = @inv";
      using (OleDbConnection connection = new OleDbConnection(Validation.BOConnectionStr))
      {
        using (OleDbCommand oleDbCommand = new OleDbCommand(cmdText, connection))
        {
          connection.Open();
          oleDbCommand.Parameters.AddWithValue("@inv", (object)_inv);
          return oleDbCommand.ExecuteScalar() != null ? this.status((string)oleDbCommand.ExecuteScalar()) : -1;
        }
      }
    }

    public int getEdgeStatus(string _inv)
    {
      string cmdText = "SELECT [edge status] FROM edger WHERE [inv edger] = @inv";
      using (OleDbConnection connection = new OleDbConnection(Validation.BOConnectionStr))
      {
        using (OleDbCommand oleDbCommand = new OleDbCommand(cmdText, connection))
        {
          connection.Open();
          oleDbCommand.Parameters.AddWithValue("@inv", (object)_inv);
          return oleDbCommand.ExecuteScalar() != null ? this.status((string)oleDbCommand.ExecuteScalar()) : -1;
        }
      }
    }

    public bool invCashier(string _invNumber)
    {
      using (OleDbConnection BOConnection = new OleDbConnection(Validation.BOConnectionStr))
      using (OdbcConnection IQConnection = new OdbcConnection(PublicMethods.IQConnectionString))
      using (OleDbCommand oleDbCommandBO = new OleDbCommand("SELECT COUNT(*) FROM sales_counter WHERE invsales = @inv", BOConnection))
      using (OdbcCommand odbcCommandIQ = new OdbcCommand("SELECT COUNT(*) FROM INVOICES WHERE document = ?", IQConnection))
      {
        BOConnection.Open();
        IQConnection.Open();
        oleDbCommandBO.Parameters.AddWithValue("@inv", (object)_invNumber);
        odbcCommandIQ.Parameters.AddWithValue("@inv", (object)_invNumber);
        return (
          (int.Parse(oleDbCommandBO.ExecuteScalar().ToString()) == 0) &&
          (int.Parse(odbcCommandIQ.ExecuteScalar().ToString()) != 0)
        );
      }
    }

    public string getCustomerName(string _inv)
    {
      using (OdbcConnection IQConnection = new OdbcConnection(PublicMethods.IQConnectionString))
      using (OdbcCommand odbcCommandIQ = new OdbcCommand("SELECT name FROM INVOICES WHERE document = ?", IQConnection))
      {
        IQConnection.Open();
        odbcCommandIQ.Parameters.AddWithValue("@inv", _inv);
        object _ = odbcCommandIQ.ExecuteScalar();
        if (_ is string)
          return (string)_;
        else
          return (string)null;
      }
    }

    public string getCustomerCell(string _inv)
    {
      using (OdbcConnection IQConnection = new OdbcConnection(PublicMethods.IQConnectionString))
      using (OdbcCommand odbcCommandIQ = new OdbcCommand("SELECT phone FROM INVOICES WHERE document = ?", IQConnection))
      {
        IQConnection.Open();
        odbcCommandIQ.Parameters.AddWithValue("@inv", _inv);
        object _ = odbcCommandIQ.ExecuteScalar();
        if (_ is string)
          return (string)_;
        else
          return (string)null;
      }
    }

    public bool invPlaced(string _invNumber)
    {
      string _where = "WHERE\n" +
        "   sc.invsales = @inv AND\n" +
        "   (\n" +
        "       sc.[Customer Name] Is Not Null AND\n" +
        "       p.InvQ_LU Is Null AND\n" +
        "       sc.Cancelled = NO AND\n" +
        "       sc.rep <> 18\n" +
        "   );";
      using (OleDbConnection connection = new OleDbConnection(Validation.BOConnectionStr))
      {
        using (OleDbCommand oleDbCommand = new OleDbCommand(Validation.select(_where), connection))
        {
          connection.Open();
          oleDbCommand.Parameters.AddWithValue("@inv", (object)_invNumber);
          return oleDbCommand.ExecuteScalar() != null;
        }
      }
    }



    public bool invPlacedCooldown(string _invNumber)
    {
      // All conditions need to be true to be good
      // If Placed today and no other jobs OR if placed before today
      string _where = "WHERE\n" +
        "   sc.invsales = @inv AND\n" +
        "   (\n" +
        "       p.[peg date] + 1 < NOW() OR\n" +
        "       (SELECT COUNT(*) FROM placed AS psq WHERE psq.[peg date] + 1 > NOW()) = 0\n" +
        "   );";
      using (OleDbConnection connection = new OleDbConnection(Validation.BOConnectionStr))
      {
        using (OleDbCommand oleDbCommand = new OleDbCommand(Validation.select(_where), connection))
        {
          connection.Open();
          oleDbCommand.Parameters.AddWithValue("@inv", (object)_invNumber);
          return oleDbCommand.ExecuteScalar() != null;
        }
      }
    }

    public bool invCutter(string _invNumber)
    {
      Debug.WriteLine($"INV = {_invNumber}");
      Dictionary<string, int> hours = this.getHours();
      // string _where = "WHERE sc.invsales = @inv AND \nsc.cut = YES AND \nsc.[sale time] > NOW() - 365 AND \n(\n   c.[cut date] IS NULL OR\n   c.[cut date] >= NOW() - " + Validation.showForMins.ToString() + "/(24*60) OR\n   c.[cut status] <> \"completed\"\n) AND \n( \n   sc.urgent = YES OR \n   ( \n       sc.timber = YES AND \n       sc.[sale time] < NOW() - @timber/24 \n   ) OR \n   ( \n       sc.timber = NO AND \n       sc.[sale time] < NOW() - @boards/24 \n   )\n)";
      string _where = "WHERE sc.invsales = @inv AND \n" +
                "sc.cut = YES AND \n" +
                "InvQ_LU IS NOT NULL AND\n" +
                " \n" +
                "(\n" +
                "   c.[cut date] IS NULL OR\n" +
                "   c.[cut date] >= NOW() - " + Validation.showForMins.ToString() + "/(24*60) OR\n" +
                "   c.[cut status] <> \"completed\"\n" +
                ") AND \n" +
                "( \n" +
                "   sc.urgent = YES OR \n" +
                "   ( \n" +
                "       sc.timber = YES AND \n" +
                "       sc.[sale time] < NOW() - @timber/24 \n" +
                "   ) OR \n" +
                "   ( \n" +
                "       sc.timber = NO AND \n" +
                "       sc.[sale time] < NOW() - @boards/24 \n" +
                "   )\n" +
                ") AND \n" +
                " (\n" +
                "   (\n" +
                "     (sc.Cut = Yes) AND\n" +
                "     (f.inv_filled Is Null) AND\n" +
                "     (p.InvQ_LU Is Not Null) AND\n" +
                "     (sc.Cancelled = No) AND\n" +
                "     (c.[cut date] Is Null)\n" +
                "   )\n" +
                " )";
      using (OleDbConnection connection = new OleDbConnection(Validation.BOConnectionStr))
      {
        using (OleDbCommand oleDbCommand = new OleDbCommand(Validation.select(_where), connection))
        {
          connection.Open();
          oleDbCommand.Parameters.AddWithValue("@inv", (object)_invNumber);
          oleDbCommand.Parameters.AddWithValue("@timber", (object)hours["timber"]);
          oleDbCommand.Parameters.AddWithValue("@boards", (object)hours["boards"]);

          return oleDbCommand.ExecuteScalar() != null;
        }
      }
    }

    public bool invEdger(string _invNumber)
    {
      string _where = "WHERE sc.invsales = @inv AND (\n   p.InvQ_LU IS NOT NULL AND\n   sc.Edge = YES AND\n   e.[edge status] IS NULL OR\n   (\n       e.[edge status] <> \"completed\"\n   ) AND\n   f.inv_filled IS NULL AND\n   sc.Cancelled = FALSE\n);";
      using (OleDbConnection connection = new OleDbConnection(Validation.BOConnectionStr))
      {
        using (OleDbCommand oleDbCommand = new OleDbCommand(Validation.select(_where), connection))
        {
          connection.Open();
          oleDbCommand.Parameters.AddWithValue("@inv", (object)_invNumber);
          return oleDbCommand.ExecuteScalar() != null;
        }
      }
    }

    public bool invDriller(string _invNumber)
    {
      string _where = "WHERE\n   sc.invsales = @inv AND\n   (\n      p.InvQ_LU IS NOT NULL AND\n      sc.Drill = YES AND\n      d.[drill date] IS NULL AND\n      f.Date_filled IS NULL AND\n      sc.Cancelled = NO\n   );";
      using (OleDbConnection connection = new OleDbConnection(Validation.BOConnectionStr))
      {
        using (OleDbCommand oleDbCommand = new OleDbCommand(Validation.select(_where), connection))
        {
          connection.Open();
          oleDbCommand.Parameters.AddWithValue("@inv", (object)_invNumber);
          return oleDbCommand.ExecuteScalar() != null;
        }
      }
    }

    public bool invChecker(string _invNumber)
    {
      string _where = "WHERE \n   sc.invsales = @inv AND \n   sc.[sale time] >= NOW() - 365 AND \n   chk.[inv checker] IS NULL AND \n   ( \n       sc.Cut = NO OR \n       c.[cut status] = \"completed\" \n   ) AND \n   ( \n       sc.Edge = NO OR \n       e.[inv edger] IS NOT NULL \n   ) AND \n   ( \n       sc.Drill = NO OR \n       d.[inv drill] IS NOT NULL \n   ) \n";
      using (OleDbConnection connection = new OleDbConnection(Validation.BOConnectionStr))
      {
        using (OleDbCommand oleDbCommand = new OleDbCommand(Validation.select(_where), connection))
        {
          connection.Open();
          oleDbCommand.Parameters.AddWithValue("@inv", (object)_invNumber);
          return oleDbCommand.ExecuteScalar() != null;
        }
      }
    }

    public bool invFiler(string _invNumber)
    {
      string _where = "WHERE \n   sc.invsales = @inv AND \n   sc.[sale time] >= NOW() - 365 AND\n   chk.[inv checker] IS NOT NULL AND\n   f.inv_filled IS NULL";
      using (OleDbConnection connection = new OleDbConnection(Validation.BOConnectionStr))
      {
        using (OleDbCommand oleDbCommand = new OleDbCommand(Validation.select(_where), connection))
        {
          connection.Open();
          oleDbCommand.Parameters.AddWithValue("@inv", (object)_invNumber);
          return oleDbCommand.ExecuteScalar() != null;
        }
      }
    }

    public static bool CompareDataTables(DataTable a, DataTable b)
    {
      if (a == null || b == null || a.Rows.Count != b.Rows.Count)
        return false;
      for (int index1 = 0; index1 < a.Rows.Count; ++index1)
      {
        object[] itemArray1 = a.Rows[index1].ItemArray;
        object[] itemArray2 = b.Rows[index1].ItemArray;
        if (itemArray1.Length != itemArray2.Length)
          return false;
        for (int index2 = 0; index2 < itemArray1.Length; ++index2)
        {
          if (!a.Rows[index1].ItemArray[index2].ToString().Equals(b.Rows[index1].ItemArray[index2].ToString()))
            return false;
        }
      }
      return true;
    }

    public static string[] GetOdbcDriverNames()
    {
      string[] odbcDriverNames = null;
      using (RegistryKey localMachineHive = Registry.LocalMachine)
      using (RegistryKey odbcDriversKey = localMachineHive.OpenSubKey(@"SOFTWARE\ODBC\ODBCINST.INI\ODBC Drivers"))
      {
        if (odbcDriversKey != null)
        {
          odbcDriverNames = odbcDriversKey.GetValueNames();
        }
      }

      return odbcDriverNames;
    }

    public static int getItemStatus(string invoice, string itemId)
    {
      string sql = @"SELECT TOP 1 status FROM ItemStatus WHERE invoice = @inv AND itemId = @itemId";
      using (OleDbConnection connection = new OleDbConnection(Validation.BOConnectionStr))
      using (OleDbCommand oleDbCommand = new OleDbCommand(sql, connection))
      {
        connection.Open();
        oleDbCommand.Parameters.AddWithValue("@inv", invoice);
        oleDbCommand.Parameters.AddWithValue("@itemId", itemId);
        return (int)(oleDbCommand.ExecuteScalar() ?? -1);
      }
    }
  }
}
