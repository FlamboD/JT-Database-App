using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JT_Database_App.Forms
{
  public partial class frmSales : frmBase
  {
    private string invoice;
    private bool blockDB = true;
    public frmSales()
    {
      InitializeComponent();
      Load += (s, e) => { getSalesInvoices(); };
      cbxInv.DataSourceChanged += (s, e) => { blockDB = true; };
      cbxInv.LostFocus += (s, e) => {
        if (((List<string>)cbxInv.DataSource).Contains(cbxInv.Text.ToUpper().Trim())) {
          cbxInv.Text = cbxInv.Text.ToUpper().Trim();
        } else {
          cbxInv.Text = "";
        }; 
      };
      cbxInv.SelectedValueChanged += (s, e) => { 
        invoice = cbxInv.Text; 
        populateJobInfo();
        blockDB = false;
      };
    }
    private async void populateJobInfo()
    {
      if (blockDB) return;
      string cmdSale = "SELECT invsales, [Customer Name], [Customer Cell] FROM sales_counter WHERE invsales = @inv;";
      DataTable dataTable = new DataTable();
      try
      {
        OleDbCommand cmd = new OleDbCommand(cmdSale);
        cmd.Parameters.AddWithValue("@inv", invoice);
        dataTable = await QueryJT(cmd);
        Debug.WriteLine(cmdSale);
        Debug.WriteLine(dataTable.Rows.Count);
        this.Invoke(new Action(() =>
        {
          txtCusName.Text = dataTable.Rows[0]["Customer Name"].ToString();
          txtCusCell.Text = dataTable.Rows[0]["Customer Cell"].ToString();
        }));
      } catch (OleDbException ex)
      {
        Debug.WriteLine(ex);
      }
      fillItems();
    }
    private async void getSalesInvoices()
    {
      string cmdSales = "" +
        "SELECT \r\n" +
        "    invsales\r\n" +
        "FROM sales_counter\r\n" +
        "WHERE\r\n" +
        "    (\n" +
        "       urgent = NO AND\n" +
        "       cut = NO AND\n" +
        "       edge = NO AND\n" +
        "       drill = NO AND\n" +
        "       timber = NO AND\n" +
        "       other = NO AND\n" +
        "       delivery = NO AND\n" +
        "       cancelled = NO\n" +
        "   ) AND [Sales Date] >= DATEADD(\"YYYY\", -2, NOW);";
      DataTable dataTable = new DataTable();
      {
        try
        {
          dataTable = await QueryJT(cmdSales);
        }
        catch (OleDbException ex)
        {
          frmMenu frmMenu = new frmMenu();
          PublicMethods.SetDefaultForm((Form)frmMenu);
          frmMenu.FormClosed += (FormClosedEventHandler)((s, args) => this.Close());
          frmMenu.Show();
          this.Hide();
          return;
        }
        this.Invoke(new Action(() =>
        {
          List<string> invs = new List<string>();
          foreach (DataRow row in dataTable.Rows)
          {
            invs.Add(row[0].ToString());
          }
          this.cbxInv.DataSource = invs;
          cbxInv.SelectedText = string.Empty;
          cbxInv.SelectedItem = null;
          cbxInv.SelectedIndex = -1;
          cbxInv.Text = string.Empty;
        }));
      };
    }
    private async void fillItems()
    {
      if (invoice == null) return;
      string cmdSales = "" +
        "SELECT\n" +
        " DESCRIPT AS Name,\n" +
        " 0 AS Qty,\n" +
        " QUANTITY AS Total,\n" +
        " false AS Cut,\n" +
        " false AS Edge,\n" +
        " false AS Drill\n" +
        "FROM stoctran AS st\n" +
        " LEFT JOIN stock as stk ON st.code = stk.code\n" +
        "WHERE st.reference = ? AND (" +
        " DEPARTMENT IN (\'D001\', \'D002\', \'D003\', \'D004\', \'D005\', \'D010\')" +
        ");";

      DataTable dataTable = new DataTable();
      {
        using (OdbcConnection IQConn = new OdbcConnection(PublicMethods.IQConnectionString))
        using (OdbcCommand odbcCommandIQ = new OdbcCommand(cmdSales, IQConn))
        {
          odbcCommandIQ.Parameters.AddWithValue("@inv", invoice);
          IQConn.Open();
          using (OdbcDataAdapter adapter = new OdbcDataAdapter(odbcCommandIQ))
          {
            adapter.Fill(dataTable);
            dgvItems.DataSource = dataTable;
            dgvItems.Columns
              .Cast<DataGridViewColumn>()
              .Where(_ => _.ValueType != typeof(bool) && _.HeaderText != "Qty")
              .ToList().ForEach(_ =>
              {
                _.ReadOnly = true;
              });
          }/*
            // frmf.setData(odbcCommandIQ);

            OdbcDataReader reader = odbcCommandIQ.ExecuteReader();
          bool res = reader.Read();
          Type type;
          while (res)
          {
            Debug.WriteLine($"{reader.GetString(0)} | {reader.GetInt32(1)}");
            res = reader.Read();
          };
          Debug.WriteLine("");*/
        }
      };
    }
  }

}
