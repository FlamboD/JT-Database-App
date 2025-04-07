using JT_Database_App.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JT_Database_App.Forms
{
  public partial class frmBase : Form
  {
    internal cmpJTMenuStrip menuStrip;
    internal string connectionStr;
    public frmBase()
    {
      this.connectionStr = PublicMethods.BOConnectionString; // "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Settings.Default.BOPath;
      InitializeComponent();

      this.menuStrip.Form = this;
      this.menuStrip.InitializeComponents();
    }

    protected virtual void DoLoad(object sender, EventArgs e) { }

    protected async Task<DataTable> QueryJT(string cmd)
    {
      DataTable dataTable = new DataTable();
      using (OleDbConnection conn = new OleDbConnection(this.connectionStr))
      {
        await conn.OpenAsync();
        using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd, conn))
        {
          adapter.Fill(dataTable);
          return dataTable;
        }
      }
    }
    protected async Task<DataTable> QueryJT(OleDbCommand cmd)
    {
      DataTable dataTable = new DataTable();
      using (OleDbConnection conn = new OleDbConnection(this.connectionStr))
      {
        cmd.Connection = conn;
        await conn.OpenAsync();
        using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
        {
          adapter.Fill(dataTable);
          return dataTable;
        }
      }
    }
  }
}
