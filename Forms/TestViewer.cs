// Decompiled with JetBrains decompiler
// Type: JT_Database_App.TestViewer
// Assembly: JT Database App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D649E70A-0AB7-4AF5-A104-CFA0EBCF124B
// Assembly location: C:\Users\Skye\Desktop\JT Database App 23aug21.exe

using JT_Database_App.Properties;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace JT_Database_App
{
  public class TestViewer : Form
  {
    private IContainer components;
    private DataGridView TestData;

    public TestViewer()
    {
      this.InitializeComponent();
      DataTable dataTable = new DataTable();
      string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Settings.Default.BOPath;
      string cmdText = "SELECT * FROM (((sales_counter AS sc LEFT JOIN cutter c ON sc.invsales = c.invcutter) LEFT JOIN drilling d ON sc.invsales = d.[inv drill]) LEFT JOIN edger e ON sc.invsales = e.[inv edger]) LEFT JOIN filed f ON sc.invsales = f.inv_filled WHERE invsales = @inv";
      string str = "INV55907";
      using (OleDbConnection connection = new OleDbConnection(connectionString))
      {
        connection.Open();
        OleDbCommand selectCommand = new OleDbCommand(cmdText, connection);
        selectCommand.Parameters.AddWithValue("@inv", (object) str);
        using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(selectCommand))
          oleDbDataAdapter.Fill(dataTable);
      }
      this.TestData.DataSource = (object) dataTable;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.TestData = new DataGridView();
      ((ISupportInitialize) this.TestData).BeginInit();
      this.SuspendLayout();
      this.TestData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.TestData.Dock = DockStyle.Fill;
      this.TestData.Location = new Point(0, 0);
      this.TestData.Name = "TestData";
      this.TestData.RowHeadersWidth = 82;
      this.TestData.RowTemplate.Height = 33;
      this.TestData.Size = new Size(800, 450);
      this.TestData.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(12f, 25f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(800, 450);
      this.Controls.Add((Control) this.TestData);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Name = nameof (TestViewer);
      this.Text = nameof (TestViewer);
      this.WindowState = FormWindowState.Maximized;
      ((ISupportInitialize) this.TestData).EndInit();
      this.ResumeLayout(false);
    }
  }
}
