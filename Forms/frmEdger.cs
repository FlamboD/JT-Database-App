// Decompiled with JetBrains decompiler
// Type: JT_Database_App.frmEdger
// Assembly: JT Database App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D649E70A-0AB7-4AF5-A104-CFA0EBCF124B
// Assembly location: C:\Users\Skye\Desktop\JT Database App 23aug21.exe

using JT_Database_App.Properties;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JT_Database_App
{
  public class frmEdger : Form
  {
    private string connectionStr;
    private Timer tRefresh = new Timer();
    private Timer inputChecker = new Timer();
    private string sInv;
    private int sRep = -1;
    private int sStatus = -1;
    private IContainer components;
    private DataGridView dbFinish;
    private DataGridView dbStart;
    private TableLayoutPanel tableLayoutPanel1;
    private TextBox txtJobNum;
    private Label lblStatus;
    private Label lblEdger;
    private TextBox txtEdger;
    private TextBox txtStatus;
    private TextBox txtTime;
    private Label label4;
    private Label label3;
    private Label label2;
    private Label label1;
    private cmpJTMenuStrip menuStrip1;
    private Task refreshTask = Task.Run(() => { });

    public frmEdger()
    {
      this.connectionStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Settings.Default.BOPath;
      this.InitializeComponent();

      menuStrip1.Form = this;
      menuStrip1.InitializeComponents();
    }

    private void frmEdger_Load(object sender, EventArgs e)
    {
      this.txtJobNum.Focus();
      this.ActiveControl = (Control) this.txtJobNum;
      this.tRefresh.Interval = 500;
      this.tRefresh.Tick += (EventHandler) ((s, a) => this.refresh());
      this.tRefresh.Start();
      this.refresh();
    }

    private async void refresh()
    {
      if (!this.Visible) return;

      this.txtTime.Text = DateTime.Now.ToString();

      if (refreshTask.IsCompleted)
      {
        refreshTask = Task.Run(FillTables);
        try
        {
          await Task.Run(refreshTask.Wait);
        }
        catch (AggregateException aex)
        {
          Exception ex = aex.GetBaseException();
          Debug.WriteLine(ex.StackTrace);
          throw ex;
        }
      }
    }

    private void FillTables()
    {
      string selectCommandText = "SELECT\nSales_counter.Invsales,\nSales_counter.[Customer Name],\nSales_counter.urgent,\nFIX(NOW() - sales_counter.[sale time]) AS [Days ago]\nFROM ((Sales_counter LEFT JOIN edger ON Sales_counter.Invsales = edger.[inv edger]) LEFT JOIN filed ON Sales_counter.Invsales = filed.inv_filled) LEFT JOIN placed ON Sales_counter.Invsales = placed.InvQ_LU WHERE ((placed.InvQ_LU IS NOT NULL) AND (Sales_counter.Edge = YES) AND (edger.[edge date] IS NULL) AND (filed.Date_filled IS NULL) AND (Sales_counter.Cancelled = FALSE))ORDER BY\n   Sales_counter.urgent,\n   Sales_counter.Invsales;";
      string cmdText = "SELECT e.[inv edger], e.edger, e.[edge time], e.[edge status], sc.[customer name] FROM (edger AS e LEFT JOIN sales_counter AS sc ON e.[inv edger] = sc.invsales)\nLEFT JOIN filed AS f ON e.[inv edger] = f.inv_filled\nWHERE\n   f.inv_filled IS NULL AND\n   (\n       e.[edge status] <> \"completed\" OR\n       e.[edge date] >= NOW() - @minutes/(24*60)\n   )\nORDER BY e.[edge status] DESC, e.[edge time] ASC;";
      DataTable dataTable1 = new DataTable();
      DataTable dataTable2 = new DataTable();
      try
      {
        using (OleDbConnection oleDbConnection = new OleDbConnection(this.connectionStr))
        {
          oleDbConnection.Open();
          using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(selectCommandText, oleDbConnection))
            oleDbDataAdapter.Fill(dataTable1);
          using (OleDbCommand selectCommand = new OleDbCommand(cmdText, oleDbConnection))
          {
            selectCommand.Parameters.AddWithValue("@minutes", (object) Validation.showForMins);
            using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(selectCommand))
              oleDbDataAdapter.Fill(dataTable2);
          }
        }
      }
      catch (OleDbException ex)
      {
        frmMenu frmMenu = new frmMenu();
        PublicMethods.SetDefaultForm((Form) frmMenu);
        frmMenu.FormClosed += (FormClosedEventHandler) ((s, args) => this.Close());
        frmMenu.Show();
        this.Hide();
        return;
      }
      this.Invoke(new Action(() => {
        this.dbStart.RowTemplate.DefaultCellStyle.Font = new Font(FontFamily.GenericSansSerif, 8f);
        this.dbStart.RowTemplate.Height = 25;
        if (!Validation.CompareDataTables(dataTable1, (DataTable) this.dbStart.DataSource))
          this.dbStart.DataSource = (object) dataTable1;
        if (this.dbStart.DataSource != null)
        {
          this.dbStart.Columns["invsales"].Width = 110;
          this.dbStart.Columns["urgent"].Width = 50;
          this.dbStart.Columns["Days ago"].Width = 50;
          foreach (DataGridViewRow row in (IEnumerable) this.dbStart.Rows)
          {
            if (row.Cells["urgent"] != null && row.Cells["urgent"].Value != null)
            {
              if ((bool) row.Cells["urgent"].Value)
                row.DefaultCellStyle.BackColor = Color.Red;
              else
                row.DefaultCellStyle.BackColor = Color.White;
            }
            if (row.Cells["Days ago"].Value != null && int.Parse(row.Cells["Days ago"].Value.ToString()) > 5)
              row.Cells["Days ago"].Style.BackColor = Color.Orange;
          }
        }
        this.dbFinish.RowTemplate.DefaultCellStyle.Font = new Font(FontFamily.GenericSansSerif, 8f);
        this.dbFinish.RowTemplate.Height = 25;
        if (!Validation.CompareDataTables(dataTable2, (DataTable) this.dbFinish.DataSource))
          this.dbFinish.DataSource = (object) dataTable2;
        this.dbFinish.Columns["edge time"].DefaultCellStyle.Format = "HH:MM:ss";
      }));
    }

    private void ResetForm()
    {
      Color white = Color.White;
      Color black = Color.Black;
      string str = "N/A";
      this.sInv = (string) null;
      this.sRep = -1;
      this.sStatus = -1;
      this.txtJobNum.Text = "";
      this.txtJobNum.BackColor = white;
      this.txtEdger.Text = "";
      this.txtEdger.BackColor = white;
      this.lblEdger.Text = str;
      this.lblEdger.ForeColor = black;
      this.txtStatus.Text = "";
      this.txtStatus.BackColor = white;
      this.lblStatus.Text = str;
      this.lblStatus.ForeColor = black;
      this.txtJobNum.Focus();
    }

    private void txtJobNum_KeyUp(object sender, KeyEventArgs e)
    {
      this.inputChecker.Dispose();
      this.txtJobNum.BackColor = Color.White;
      if (this.txtJobNum.Text == "")
        return;
      this.inputChecker = new Timer();
      this.inputChecker.Interval = 1000;
      this.inputChecker.Tick += (EventHandler) ((s, a) =>
      {
        this.inputChecker.Dispose();
        this.txtJobNum.Invoke(new Action(() => this.ValidateJobNumber()));
      });
      this.inputChecker.Start();
    }

    private void txtEdger_KeyUp(object sender, KeyEventArgs e)
    {
      this.inputChecker.Dispose();
      this.txtEdger.BackColor = Color.White;
      if (this.txtEdger.Text == "")
        return;
      this.inputChecker = new Timer();
      this.inputChecker.Interval = 1000;
      this.inputChecker.Tick += (EventHandler) ((s, a) =>
      {
        this.inputChecker.Dispose();
        this.txtEdger.Invoke(new Action(() => this.ValidateEdger()));
      });
      this.inputChecker.Start();
    }

    private void txtStatus_KeyUp(object sender, KeyEventArgs e)
    {
      this.inputChecker.Dispose();
      this.txtStatus.BackColor = Color.White;
      if (this.txtStatus.Text == "")
        return;
      this.inputChecker = new Timer();
      this.inputChecker.Interval = 1000;
      this.inputChecker.Tick += (EventHandler) ((s, a) =>
      {
        this.inputChecker.Dispose();
        this.txtStatus.Invoke(new Action(() => this.ValidateStatus()));
      });
      this.inputChecker.Start();
    }

    private void ValidateJobNumber()
    {
      if (this.txtJobNum.Text.Contains("CANCEL"))
        this.ResetForm();
      else if (this.txtJobNum.Text.StartsWith("frm"))
      {
        if (ChangeForm.Name((Form) this, this.txtJobNum.Text))
          return;
        AutoClosingMessageBox.Show(string.Format("Form {0} is not isn't available or doesn't exist!", (object) this.txtJobNum.Text), "Error", 3000);
        this.ResetForm();
      }
      else
      {
        this.txtJobNum.Text = this.txtJobNum.Text.ToUpper();
        if (new Validation().invEdger(this.txtJobNum.Text))
        {
          this.sInv = this.txtJobNum.Text;
          this.txtEdger.Focus();
          this.txtJobNum.BackColor = Color.Green;
        }
        else
        {
          this.sInv = (string) null;
          AutoClosingMessageBox.Show(string.Format("Invoice {0} is not in the table or isn't available for edging!", (object) this.txtJobNum.Text), "Error", 3000);
          this.txtJobNum.Text = "";
        }
      }
    }

    private void ValidateEdger()
    {
      if (this.txtEdger.Text.Contains("CANCEL"))
      {
        this.ResetForm();
      }
      else
      {
        int num = new Validation().rep(this.txtEdger.Text);
        if (num != -1)
        {
          this.sRep = num;
          this.txtEdger.BackColor = Color.Green;
          this.txtStatus.Focus();
        }
        else
        {
          this.sRep = -1;
          AutoClosingMessageBox.Show(string.Format("Rep {0} is not in the table!", (object) this.txtEdger.Text), "Error", 3000);
          this.txtEdger.Text = "";
          this.txtEdger.Focus();
        }
      }
    }

    private void ValidateStatus()
    {
      if (this.txtStatus.Text.Contains("CANCEL"))
      {
        this.ResetForm();
      }
      else
      {
        int _status = new Validation().status(this.txtStatus.Text);
        int edgeStatus = new Validation().getEdgeStatus(this.sInv);
        if (_status != -1)
        {
          this.sStatus = _status;
          if (edgeStatus != -1)
          {
            if (_status != edgeStatus)
            {
              this.UpdateData();
            }
            else
            {
              AutoClosingMessageBox.Show(string.Format("Job {0}'s status is already {1}!", (object) this.txtJobNum.Text, (object) Validation.StatusName(_status)), "Error", 3000);
              this.txtStatus.Text = "";
            }
          }
          else
            this.SubmitData();
        }
        else
        {
          this.sStatus = -1;
          AutoClosingMessageBox.Show(string.Format("Status {0} does not exist!", (object) this.txtStatus.Text), "Error", 3000);
          this.txtStatus.Text = "";
        }
      }
    }

    private void SubmitData()
    {
      string cmdText = "INSERT INTO edger VALUES(@id, null, @inv, @edger, @time, @time, @status, @end);";
      using (OleDbConnection connection = new OleDbConnection(this.connectionStr))
      {
        using (OleDbCommand oleDbCommand1 = new OleDbCommand(cmdText, connection))
        {
          using (OleDbCommand oleDbCommand2 = new OleDbCommand("SELECT id FROM edger ORDER BY id DESC", connection))
          {
            connection.Open();
            int num;
            using (OleDbDataReader oleDbDataReader = oleDbCommand2.ExecuteReader())
            {
              oleDbDataReader.Read();
              num = oleDbDataReader.GetInt32(0) + 1;
            }
            oleDbCommand1.Parameters.AddWithValue("@id", (object) num);
            oleDbCommand1.Parameters.AddWithValue("@inv", (object) this.sInv);
            oleDbCommand1.Parameters.AddWithValue("@edger", (object) Validation.RepName(this.sRep));
            oleDbCommand1.Parameters.AddWithValue("@time", (object) this.DTC(DateTime.Now));
            oleDbCommand1.Parameters.AddWithValue("@status", (object) Validation.StatusName(this.sStatus));
            if (this.sStatus != 3)
              oleDbCommand1.Parameters.AddWithValue("@end", (object) DBNull.Value);
            else
              oleDbCommand1.Parameters.AddWithValue("@end", (object) this.DTC(DateTime.Now));
            oleDbCommand1.ExecuteNonQuery();
          }
        }
      }
      this.ResetForm();
    }

    private void UpdateData()
    {
      string cmdText = "UPDATE edger SET [edge status] = @status, [edge fin] = @fin WHERE [inv edger] = @inv";
      using (OleDbConnection connection = new OleDbConnection(this.connectionStr))
      {
        using (OleDbCommand oleDbCommand = new OleDbCommand(cmdText, connection))
        {
          connection.Open();
          oleDbCommand.Parameters.AddWithValue("@status", (object) Validation.StatusName(this.sStatus));
          if (this.sStatus != 3)
            oleDbCommand.Parameters.AddWithValue("@fin", (object) DBNull.Value);
          else
            oleDbCommand.Parameters.AddWithValue("@fin", (object) this.DTC(DateTime.Now));
          oleDbCommand.Parameters.AddWithValue("@inv", (object) this.sInv);
          oleDbCommand.ExecuteNonQuery();
        }
      }
      this.ResetForm();
    }

    private void menuToolStripMenuItem_Click_1(object sender, EventArgs e)
    {
      frmMenu frmMenu = new frmMenu();
      PublicMethods.SetDefaultForm();
      frmMenu.FormClosed += (FormClosedEventHandler) ((s, args) => this.Close());
      frmMenu.Show();
      this.Hide();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

    private void menuToolStripMenuItem_Click(object sender, EventArgs e)
    {
      frmMenu frmMenu = new frmMenu();
      PublicMethods.SetDefaultForm();
      frmMenu.FormClosed += (FormClosedEventHandler) ((s, args) => this.Close());
      frmMenu.Show();
      this.Hide();
    }

    private DateTime DTC(DateTime dt) => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.dbFinish = new System.Windows.Forms.DataGridView();
      this.dbStart = new System.Windows.Forms.DataGridView();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.lblStatus = new System.Windows.Forms.Label();
      this.lblEdger = new System.Windows.Forms.Label();
      this.txtJobNum = new System.Windows.Forms.TextBox();
      this.txtEdger = new System.Windows.Forms.TextBox();
      this.txtStatus = new System.Windows.Forms.TextBox();
      this.txtTime = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.menuStrip1 = new JT_Database_App.cmpJTMenuStrip();
      ((System.ComponentModel.ISupportInitialize)(this.dbFinish)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dbStart)).BeginInit();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // dbFinish
      // 
      this.dbFinish.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dbFinish.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dbFinish.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.dbFinish.Location = new System.Drawing.Point(0, 105);
      this.dbFinish.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.dbFinish.Name = "dbFinish";
      this.dbFinish.RowHeadersWidth = 20;
      this.dbFinish.RowTemplate.Height = 33;
      this.dbFinish.Size = new System.Drawing.Size(642, 260);
      this.dbFinish.TabIndex = 0;
      // 
      // dbStart
      // 
      this.dbStart.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dbStart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dbStart.Dock = System.Windows.Forms.DockStyle.Right;
      this.dbStart.Location = new System.Drawing.Point(142, 24);
      this.dbStart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.dbStart.Name = "dbStart";
      this.dbStart.RowHeadersWidth = 20;
      this.dbStart.RowTemplate.Height = 33;
      this.dbStart.Size = new System.Drawing.Size(500, 81);
      this.dbStart.TabIndex = 1;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 3;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.Controls.Add(this.lblStatus, 2, 2);
      this.tableLayoutPanel1.Controls.Add(this.lblEdger, 2, 1);
      this.tableLayoutPanel1.Controls.Add(this.txtJobNum, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this.txtEdger, 1, 1);
      this.tableLayoutPanel1.Controls.Add(this.txtStatus, 1, 2);
      this.tableLayoutPanel1.Controls.Add(this.txtTime, 1, 3);
      this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
      this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 4;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(142, 81);
      this.tableLayoutPanel1.TabIndex = 2;
      // 
      // lblStatus
      // 
      this.lblStatus.AutoSize = true;
      this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblStatus.Location = new System.Drawing.Point(96, 40);
      this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new System.Drawing.Size(44, 20);
      this.lblStatus.TabIndex = 10;
      this.lblStatus.Text = "N/A";
      this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // lblEdger
      // 
      this.lblEdger.AutoSize = true;
      this.lblEdger.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblEdger.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblEdger.Location = new System.Drawing.Point(96, 20);
      this.lblEdger.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.lblEdger.Name = "lblEdger";
      this.lblEdger.Size = new System.Drawing.Size(44, 20);
      this.lblEdger.TabIndex = 9;
      this.lblEdger.Text = "N/A";
      this.lblEdger.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // txtJobNum
      // 
      this.txtJobNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtJobNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtJobNum.Location = new System.Drawing.Point(49, 2);
      this.txtJobNum.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.txtJobNum.Name = "txtJobNum";
      this.txtJobNum.Size = new System.Drawing.Size(43, 37);
      this.txtJobNum.TabIndex = 2;
      this.txtJobNum.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtJobNum_KeyUp);
      // 
      // txtEdger
      // 
      this.txtEdger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtEdger.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtEdger.Location = new System.Drawing.Point(49, 22);
      this.txtEdger.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.txtEdger.Name = "txtEdger";
      this.txtEdger.Size = new System.Drawing.Size(43, 37);
      this.txtEdger.TabIndex = 8;
      this.txtEdger.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtEdger_KeyUp);
      // 
      // txtStatus
      // 
      this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtStatus.Location = new System.Drawing.Point(49, 42);
      this.txtStatus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.txtStatus.Name = "txtStatus";
      this.txtStatus.Size = new System.Drawing.Size(43, 37);
      this.txtStatus.TabIndex = 7;
      this.txtStatus.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtStatus_KeyUp);
      // 
      // txtTime
      // 
      this.txtTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel1.SetColumnSpan(this.txtTime, 2);
      this.txtTime.Enabled = false;
      this.txtTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtTime.Location = new System.Drawing.Point(49, 62);
      this.txtTime.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.txtTime.Name = "txtTime";
      this.txtTime.Size = new System.Drawing.Size(91, 37);
      this.txtTime.TabIndex = 6;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
      this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label4.Location = new System.Drawing.Point(2, 60);
      this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(43, 21);
      this.label4.TabIndex = 5;
      this.label4.Text = "Time";
      this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label3.Location = new System.Drawing.Point(2, 40);
      this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(43, 20);
      this.label3.TabIndex = 4;
      this.label3.Text = "Status";
      this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(2, 20);
      this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(43, 20);
      this.label2.TabIndex = 3;
      this.label2.Text = "Edger";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(2, 0);
      this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(43, 20);
      this.label1.TabIndex = 1;
      this.label1.Text = "Job Num";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // menuStrip1
      // 
      this.menuStrip1.Form = this;
      this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Padding = new System.Windows.Forms.Padding(3, 1, 0, 1);
      this.menuStrip1.Size = new System.Drawing.Size(642, 24);
      this.menuStrip1.TabIndex = 3;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // frmEdger
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(642, 365);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Controls.Add(this.dbStart);
      this.Controls.Add(this.dbFinish);
      this.Controls.Add(this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.Name = "frmEdger";
      this.Text = "Edger";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.Load += new System.EventHandler(this.frmEdger_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dbFinish)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dbStart)).EndInit();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }
  }
}
