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

    private static string select(string _where = "") => "SELECT * FROM \n(((((sales_counter AS sc \nLEFT JOIN cutter AS c ON sc.invsales = c.invcutter)\nLEFT JOIN drilling AS d ON sc.invsales = d.[inv drill])\nLEFT JOIN edger AS e ON sc.invsales = e.[inv edger])\nLEFT JOIN filed AS f ON sc.invsales = f.inv_filled)\nLEFT JOIN checker AS chk ON sc.invsales = chk.[inv checker])\nLEFT JOIN placed AS p ON sc.invsales = p.invQ_LU\n" + _where;
    private static string selectCount(string _where = "") => "SELECT COUNT(*) FROM \n(((((sales_counter AS sc \nLEFT JOIN cutter AS c ON sc.invsales = c.invcutter)\nLEFT JOIN drilling AS d ON sc.invsales = d.[inv drill])\nLEFT JOIN edger AS e ON sc.invsales = e.[inv edger])\nLEFT JOIN filed AS f ON sc.invsales = f.inv_filled)\nLEFT JOIN checker AS chk ON sc.invsales = chk.[inv checker])\nLEFT JOIN placed AS p ON sc.invsales = p.invQ_LU\n" + _where;

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
        "       sc.[Sales Date] + 1 < NOW() OR\n" +
        " (" + 
        Validation.selectCount("WHERE\n" +
          "   sc.invsales <> @inv AND\n" +
          "   (\n" +
          "       p.InvQ_LU Is Null AND\n" +
          "       sc.Cancelled = NO AND\n" +
          "       sc.rep <> 18 AND\n" +
          "       sc.[Sales Date] + 1 < NOW()\n" +
          "   )"
        ) + ") = 0 \n" +
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
  }
}
