using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using JT_Database_App.Properties;

namespace JT_Database_App
{
  internal class DatabaseController
  {
    private string connStrAcc, connStrIQ;
    public DatabaseController()
    {
      this.connStrAcc = "" +
        "Provider=Microsoft.ACE.OLEDB.12.0;" +
        "Data Source=" + Settings.Default.BOPath;
      this.connStrIQ = "" +
                "DRIVER={DBISAM 4 ODBC Driver};" +
                "ConnectionType=Local;" +
                "CatalogName=" + Settings.Default.IQPath;
    }

    public DataTable QueryAccess(OleDbCommand command)
    {
      return QueryDatabase(command, connStrAcc);
    }

    public int UpdateAccess(OleDbCommand command)
    {
      using (OleDbConnection conn = new OleDbConnection(connStrAcc))
      {
        command.Connection = conn;
        return command.ExecuteNonQuery();
      }
    }

    public DataTable QueryIQ(OleDbCommand command)
    {
      return QueryDatabase(command, connStrIQ);
    }

    private DataTable QueryDatabase(OleDbCommand command, string connectionString)
    {
      DataTable dataTable = new DataTable();
      using(OleDbConnection conn = new OleDbConnection(connectionString))
      {
        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(command))
        {
          dataAdapter.Fill(dataTable);
        }
      }
      return dataTable;
    }
  }
}
