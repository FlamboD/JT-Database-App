using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JT_Database_App
{
  public partial class FloatingForm : Form
  {
    private Timer inputChecker = new Timer();
    public FloatingForm()
    {
      InitializeComponent();
      txtCommands.KeyUp += command_KeyUp;
      dgvItems.DataSourceChanged += onDataSourceChanged;
      this.Focus();
      txtCommands.Focus();
    }

    public void setData(OdbcCommand command)
    {
      DataTable dt = new DataTable();
      using(OdbcDataAdapter adapter = new OdbcDataAdapter(command))
      {
        adapter.Fill(dt);
      }
      dgvItems.DataSource = dt;
    }

    private void onDataSourceChanged(object sender, EventArgs e)
    {
      dgvItems.ClearSelection();
      if (dgvItems.Rows.Count > 0) dgvItems.Rows[0].Selected = true;
    }

    public void command_KeyUp(object sender, KeyEventArgs e)
    {
      if (!txtCommands.Text.ToLower().StartsWith("cmd")) return;
      Action action = () =>
      {
        string command = txtCommands.Text;
        txtCommands.Text = "";
        // DataGridViewSelectedRowCollection rows = dgvItems.SelectedRows;
        if(dgvItems.SelectedRows.Count == 0)
        {
          dgvItems.Rows[0].Selected = true;
          return;
        }
        DataGridViewRow row = dgvItems.SelectedRows[0];
        // DataGridViewRow row = rows[0];
        switch(command.ToLower())
        {

          case "cmddown":
            {
              dgvItems.ClearSelection();
              int index = Math.Max(0, Math.Min(row.Index + 1, dgvItems.Rows.Count - 2));
              dgvItems.Rows[index].Selected = true;
              break;
            }
          case "cmdup":
            {
              dgvItems.ClearSelection();
              int index = Math.Min(dgvItems.Rows.Count, Math.Max(row.Index - 1, 0));
              dgvItems.Rows[index].Selected = true;
              break;
            }
          case "cmdstarted":
            {
              row.Cells["status"].Value = "Started";
              break;
            }
          case "cmdpaused":
            {
              row.Cells["status"].Value = "Paused";
              break;
            }
          case "cmdcomplete":
            {
              row.Cells["status"].Value = "Complete";
              break;
            }
          case "cmdclose":
            {
              Close();
              break;
            }
        }
      };

      if (!AppSettings.ManualInput)
      {
        this.inputChecker = new Timer();
        this.inputChecker.Interval = 100;
        this.inputChecker.Tick += (EventHandler)((s, a) =>
        {
          action();
        });
        this.inputChecker.Start();
      }
      else if (e.KeyCode == Keys.Return)
      {
        action();
      }
    }
  }
}
