// Decompiled with JetBrains decompiler
// Type: JT_Database_App.frmDriller
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
  public class frmDriller : Form
  {
    private string connectionStr;
    private Timer tRefresh = new Timer();
    private Timer inputChecker = new Timer();
    private string sInv;
    private int sRep = -1;
    private int sStatus = -1;
    private IContainer components;
    private TextBox txtJobNum;
    private TextBox txtDriller;
    private cmpJTMenuStrip menuStrip1;
    private Label lblDriller;
    private TextBox txtTime;
    private TableLayoutPanel tableLayoutPanel1;
    private Label label4;
    private Label label2;
    private Label label1;
    private DataGridView dbFinish;
    private DataGridView dbStart;
    private Task refreshTask = Task.Run(() => { });

    public frmDriller()
    {
      this.connectionStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Settings.Default.BOPath;
      this.InitializeComponent();

      menuStrip1.Form = this;
      menuStrip1.InitializeComponents();
    }

    private void frmDriller_Load(object sender, EventArgs e)
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
      string selectCommandText = "SELECT\nSales_counter.Invsales,\nSales_counter.[Customer Name],\nSales_counter.urgent,\nFIX(NOW() - sales_counter.[sale time]) AS [Days ago]\nFROM((Sales_counter LEFT JOIN drilling ON Sales_counter.Invsales = drilling.[inv drill]) LEFT JOIN filed ON Sales_counter.Invsales = filed.inv_filled) LEFT JOIN placed ON Sales_counter.Invsales = placed.InvQ_LU WHERE((placed.InvQ_LU IS NOT NULL) AND (Sales_counter.Drill = YES) AND (drilling.[drill date] IS NULL) AND (filed.Date_filled IS NULL) AND (Sales_counter.Cancelled = NO)) ORDER BY Sales_counter.Invsales;";
      string cmdText = "SELECT drilling.[inv drill], Sales_counter.[Customer Name], drilling.[drill time] FROM Sales_counter RIGHT JOIN drilling ON Sales_counter.Invsales = drilling.[inv drill] WHERE((drilling.[drill time] >= NOW() - @minutes/(24*60)) AND (Sales_counter.Cancelled = NO)) ORDER BY drilling.[drill time] DESC;";
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
            selectCommand.Parameters.AddWithValue("@minutes", (object)Validation.showForMins);
            using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(selectCommand))
              oleDbDataAdapter.Fill(dataTable2);
          }
        }
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
      this.Invoke(new Action(() => { 
        this.dbStart.RowTemplate.DefaultCellStyle.Font = new Font(FontFamily.GenericSansSerif, 8f);
        this.dbStart.RowTemplate.Height = 25;
        if (!Validation.CompareDataTables(dataTable1, (DataTable)this.dbStart.DataSource))
          this.dbStart.DataSource = (object)dataTable1;
        if (this.dbStart.DataSource != null)
        {
          this.dbStart.Columns["invsales"].Width = 110;
          this.dbStart.Columns["urgent"].Width = 50;
          this.dbStart.Columns["Days ago"].Width = 50;
          foreach (DataGridViewRow row in (IEnumerable)this.dbStart.Rows)
          {
            if (row.Cells["urgent"] != null && row.Cells["urgent"].Value != null)
            {
              if ((bool)row.Cells["urgent"].Value)
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
        if (!Validation.CompareDataTables(dataTable2, (DataTable)this.dbFinish.DataSource))
          this.dbFinish.DataSource = (object)dataTable2;
        this.dbFinish.Columns["drill time"].DefaultCellStyle.Format = "HH:MM:ss";
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
      this.txtDriller.Text = "";
      this.txtDriller.BackColor = white;
      this.lblDriller.Text = str;
      this.lblDriller.ForeColor = black;
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

    private void txtDriller_KeyUp(object sender, KeyEventArgs e)
    {
      this.inputChecker.Dispose();
      this.txtDriller.BackColor = Color.White;
      if (this.txtDriller.Text == "")
        return;
      this.inputChecker = new Timer();
      this.inputChecker.Interval = 1000;
      this.inputChecker.Tick += (EventHandler) ((s, a) =>
      {
        this.inputChecker.Dispose();
        this.txtDriller.Invoke(new Action(() => this.ValidateDriller()));
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
        if (new Validation().invDriller(this.txtJobNum.Text))
        {
          this.sInv = this.txtJobNum.Text;
          this.txtDriller.Focus();
          this.txtJobNum.BackColor = Color.Green;
        }
        else
        {
          this.sInv = (string) null;
          AutoClosingMessageBox.Show(string.Format("Invoice {0} is not in the table or isn't available for drilling!", (object) this.txtJobNum.Text), "Error", 3000);
          this.txtJobNum.Text = "";
        }
      }
    }

    private void ValidateDriller()
    {
      if (this.txtDriller.Text.Contains("CANCEL"))
      {
        this.ResetForm();
      }
      else
      {
        int num = new Validation().rep(this.txtDriller.Text);
        if (num != -1)
        {
          this.sRep = num;
          this.txtDriller.BackColor = Color.Green;
          this.SubmitData();
        }
        else
        {
          this.sRep = -1;
          AutoClosingMessageBox.Show(string.Format("Rep {0} is not in the table!", (object) this.txtDriller.Text), "Error", 3000);
          this.txtDriller.Text = "";
          this.txtDriller.Focus();
        }
      }
    }

    private void SubmitData()
    {
      string cmdText = "INSERT INTO drilling VALUES(@id, null, @inv, @driller, @time, @time, @time);";
      using (OleDbConnection connection = new OleDbConnection(this.connectionStr))
      {
        using (OleDbCommand oleDbCommand1 = new OleDbCommand(cmdText, connection))
        {
          using (OleDbCommand oleDbCommand2 = new OleDbCommand("SELECT id FROM drilling ORDER BY id DESC", connection))
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
            oleDbCommand1.Parameters.AddWithValue("@driller", (object) Validation.RepName(this.sRep));
            oleDbCommand1.Parameters.AddWithValue("@time", (object) this.DTC(DateTime.Now));
            oleDbCommand1.ExecuteNonQuery();
          }
        }
      }
      this.ResetForm();
    }

    private void menuToolStripMenuItem_Click(object sender, EventArgs e)
    {
      frmMenu frmMenu = new frmMenu();
      PublicMethods.SetDefaultForm();
      frmMenu.FormClosed += (FormClosedEventHandler) ((s, args) => this.Close());
      frmMenu.Show();
      this.Hide();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

    private void connectToolStripMenuItem_Click(object sender, EventArgs e)
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
      this.txtJobNum = new System.Windows.Forms.TextBox();
      this.txtDriller = new System.Windows.Forms.TextBox();
      this.menuStrip1 = new JT_Database_App.cmpJTMenuStrip();
      this.lblDriller = new System.Windows.Forms.Label();
      this.txtTime = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.dbFinish = new System.Windows.Forms.DataGridView();
      this.dbStart = new System.Windows.Forms.DataGridView();
      this.tableLayoutPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dbFinish)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dbStart)).BeginInit();
      this.SuspendLayout();
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
      // txtDriller
      // 
      this.txtDriller.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtDriller.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtDriller.Location = new System.Drawing.Point(49, 37);
      this.txtDriller.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.txtDriller.Name = "txtDriller";
      this.txtDriller.Size = new System.Drawing.Size(43, 37);
      this.txtDriller.TabIndex = 8;
      this.txtDriller.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtDriller_KeyUp);
      // 
      // menuStrip1
      // 
      this.menuStrip1.Form = this;
      this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Padding = new System.Windows.Forms.Padding(3, 1, 0, 1);
      this.menuStrip1.Size = new System.Drawing.Size(142, 24);
      this.menuStrip1.TabIndex = 7;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // lblDriller
      // 
      this.lblDriller.AutoSize = true;
      this.lblDriller.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblDriller.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblDriller.Location = new System.Drawing.Point(96, 35);
      this.lblDriller.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.lblDriller.Name = "lblDriller";
      this.lblDriller.Size = new System.Drawing.Size(44, 35);
      this.lblDriller.TabIndex = 9;
      this.lblDriller.Text = "N/A";
      this.lblDriller.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // txtTime
      // 
      this.txtTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel1.SetColumnSpan(this.txtTime, 2);
      this.txtTime.Enabled = false;
      this.txtTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtTime.Location = new System.Drawing.Point(49, 72);
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
      this.label4.Location = new System.Drawing.Point(2, 70);
      this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(43, 35);
      this.label4.TabIndex = 5;
      this.label4.Text = "Time";
      this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(2, 35);
      this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(43, 35);
      this.label2.TabIndex = 3;
      this.label2.Text = "Driller";
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
      this.label1.Size = new System.Drawing.Size(43, 35);
      this.label1.TabIndex = 1;
      this.label1.Text = "Job Num";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 3;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.Controls.Add(this.lblDriller, 2, 1);
      this.tableLayoutPanel1.Controls.Add(this.txtJobNum, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this.txtDriller, 1, 1);
      this.tableLayoutPanel1.Controls.Add(this.txtTime, 1, 2);
      this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 3;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(142, 105);
      this.tableLayoutPanel1.TabIndex = 6;
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
      this.dbFinish.TabIndex = 4;
      // 
      // dbStart
      // 
      this.dbStart.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dbStart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dbStart.Dock = System.Windows.Forms.DockStyle.Right;
      this.dbStart.Location = new System.Drawing.Point(142, 0);
      this.dbStart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.dbStart.Name = "dbStart";
      this.dbStart.RowHeadersWidth = 20;
      this.dbStart.RowTemplate.Height = 33;
      this.dbStart.Size = new System.Drawing.Size(500, 105);
      this.dbStart.TabIndex = 5;
      // 
      // frmDriller
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(642, 365);
      this.Controls.Add(this.menuStrip1);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Controls.Add(this.dbStart);
      this.Controls.Add(this.dbFinish);
      this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.Name = "frmDriller";
      this.Text = "Driller";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.Load += new System.EventHandler(this.frmDriller_Load);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dbFinish)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dbStart)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }
  }
}
