// Decompiled with JetBrains decompiler
// Type: JT_Database_App.frmPlaced
// Assembly: JT Database App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D649E70A-0AB7-4AF5-A104-CFA0EBCF124B
// Assembly location: C:\Users\Skye\Desktop\JT Database App 23aug21.exe

using JT_Database_App.Properties;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JT_Database_App
{
  public class frmPlaced : Form
  {
    private string connectionStr;
    private Timer tRefresh = new Timer();
    private Timer inputChecker = new Timer();
    private string sInv;
    private IContainer components;
    private DataGridView dbRecentlyPegged;
    private DataGridView dbNotPlaced;
    private TableLayoutPanel tableLayoutPanel1;
    private TableLayoutPanel tableLayoutPanel2;
    private TableLayoutPanel tableLayoutPanel3;
    private Label label1;
    private Label label2;
    private cmpJTMenuStrip menuStrip1;
    private TextBox txtJobNum;
    private TextBox txtTime;
    private Task refreshTask = Task.Run(() => { });

    private DelayIncreaser delay = new DelayIncreaser(5, 0, TimeSpan.FromMinutes(60));

    private delegate void BlankDelegate();
    bool closed = false;

    public frmPlaced()
    {
      this.connectionStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Settings.Default.BOPath;
      //this.connectionStr = "DSN=JTBOffice";
      this.InitializeComponent();

      this.menuStrip1.Form = this;
      this.menuStrip1.InitializeComponents();

      //this.FormClosed += (FormClosedEventHandler)((s, args) => Application.Exit());
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      this.txtJobNum.Focus();
      this.WindowState = FormWindowState.Maximized;
      this.ActiveControl = (Control)this.txtJobNum;
      this.tRefresh.Interval = 500;
      this.tRefresh.Tick += (EventHandler)((s, a) => this.refresh());
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
      string selectCommandText1 = "" +
        "SELECT Sales_counter.[Sales Date], Sales_counter.Invsales, Sales_counter.[Customer Name]\n" +
        "FROM\n" +
        "   (\n" +
        "     Sales_counter\n" +
        "     LEFT JOIN placed ON Sales_counter.Invsales = placed.InvQ_LU\n" +
        "   )\n" +
        "WHERE\n" +
        "   (\n" +
        "       Sales_counter.[Customer Name] Is Not Null AND\n" +
        "       placed.InvQ_LU Is Null AND\n" +
        "       Sales_counter.Cancelled = No AND\n" +
        "       Sales_counter.rep <> 18\n" +
        "   )\n" +
        "ORDER BY Sales_counter.Invsales;";
      string selectCommandText2 = "" +
        "SELECT TOP 10\n" +
        "   placed.InvQ_LU, Sales_counter.[Customer Name], placed.[peg date]\n" +
        "FROM Sales_counter\n" +
        "   LEFT JOIN placed ON Sales_counter.Invsales = placed.InvQ_LU\n" +
        "WHERE\n" +
        // "   placed.[peg date] >= NOW() - @minutes/(24*60) AND\n" +
        "   Sales_counter.Cancelled = No\n" +
        "ORDER BY\n" +
        "   placed.[peg time] DESC;";
      DataTable dataTable1 = new DataTable();
      DataTable dataTable2 = new DataTable();
      try
      {
        using (OleDbConnection selectConnection = new OleDbConnection(this.connectionStr))
        {
          using (OleDbDataAdapter oleDbDataAdapter1 = new OleDbDataAdapter(selectCommandText1, selectConnection))
          {
            using (OleDbDataAdapter oleDbDataAdapter2 = new OleDbDataAdapter(selectCommandText2, selectConnection))
            {
              selectConnection.Open();
              oleDbDataAdapter1.Fill(dataTable1);
              //oleDbDataAdapter2.SelectCommand.Parameters.AddWithValue("@minutes", (object)Validation.showForMins);
              oleDbDataAdapter2.Fill(dataTable2);
            }
          }
        }
      }
      catch (Exception ex)// when (ex is OleDbException || ex is InvalidOperationException)
      {
        Debug.WriteLine(ex.Message);
        ChangeForm.Menu(this);
        //frmMenu frmMenu = new frmMenu();
        //(new frmMenu()).Show();
        //this.Close();
        return;
      }

      this.Invoke(new Action(() =>
      {
        this.dbNotPlaced.RowTemplate.DefaultCellStyle.Font = new Font(FontFamily.GenericSansSerif, 16f);
        this.dbNotPlaced.RowTemplate.Height = 25;
        if (!Validation.CompareDataTables(dataTable1, (DataTable)this.dbNotPlaced.DataSource))
          this.dbNotPlaced.DataSource = (object)dataTable1;
        if (this.dbNotPlaced.DataSource != null)
        {
          this.dbNotPlaced.Columns["sales date"].DefaultCellStyle.Format = "dd/MM/yy HH:mm:ss";
          this.dbNotPlaced.Columns["sales date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
          this.dbNotPlaced.Columns["invsales"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }
        this.dbRecentlyPegged.RowTemplate.DefaultCellStyle.Font = new Font(FontFamily.GenericSansSerif, 16f);
        this.dbRecentlyPegged.RowTemplate.Height = 25;
        if (!Validation.CompareDataTables(dataTable2, (DataTable)this.dbRecentlyPegged.DataSource))
          this.dbRecentlyPegged.DataSource = (object)dataTable2;
        if (this.dbRecentlyPegged.DataSource != null)
        {
          this.dbRecentlyPegged.Columns["invq_lu"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
          this.dbRecentlyPegged.Columns["peg date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }
      }));
    }

    private void ResetForm()
    {
      this.sInv = (string)null;
      this.txtJobNum.BackColor = Color.White;
      this.txtJobNum.Text = "";
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
      this.inputChecker.Tick += (EventHandler)((s, a) =>
     {
       this.inputChecker.Dispose();
       this.txtJobNum.Invoke(new Action(() => this.ValidateJobNumber()));
     });
      this.inputChecker.Start();
    }

    private void ValidateJobNumber()
    {
      if (this.txtJobNum.Text.Contains("CANCEL"))
        this.ResetForm();
      else if (this.txtJobNum.Text.StartsWith("frm"))
      {
        if (ChangeForm.Name((Form)this, this.txtJobNum.Text))
          return;
        AutoClosingMessageBox.Show(string.Format("Form {0} is not isn't available or doesn't exist!", (object)this.txtJobNum.Text), "Error", 3000);
        this.ResetForm();
      }
      else
      {
        this.txtJobNum.Text = this.txtJobNum.Text.ToUpper();
        if (new Validation().invPlaced(this.txtJobNum.Text))
        {
          if (new Validation().invPlacedCooldown(this.txtJobNum.Text))
          {
            this.sInv = this.txtJobNum.Text;
            this.txtJobNum.BackColor = Color.Green;
            this.insertRecord();
          }
          else
          {
            this.sInv = (string)null;
            AutoClosingMessageBox.Show(string.Format("Invoice {0} has only been placed today, do older jobs first!", (object)this.txtJobNum.Text), "Error", 3000);
            this.txtJobNum.Text = "";
          }
        }
        else
        {
          this.sInv = (string)null;
          AutoClosingMessageBox.Show(string.Format("Invoice {0} is not in the table or isn't available for placing!", (object)this.txtJobNum.Text), "Error", 3000);
          this.txtJobNum.Text = "";
        }
      }
    }

    private void insertRecord()
    {
      string cmdText = "INSERT INTO placed VALUES(@id, NULL, @now, @now, @inv, FALSE)";
      using (OleDbConnection connection = new OleDbConnection(this.connectionStr))
      {
        using (OleDbCommand oleDbCommand1 = new OleDbCommand(cmdText, connection))
        {
          using (OleDbCommand oleDbCommand2 = new OleDbCommand("SELECT id FROM placed ORDER BY id DESC", connection))
          {
            connection.Open();
            int num;
            using (OleDbDataReader oleDbDataReader = oleDbCommand2.ExecuteReader())
            {
              oleDbDataReader.Read();
              num = oleDbDataReader.GetInt32(0) + 1;
            }
            oleDbCommand1.Parameters.AddWithValue("@id", (object)num);
            oleDbCommand1.Parameters.AddWithValue("@now", (object)this.DTC(DateTime.Now));
            oleDbCommand1.Parameters.AddWithValue("@inv", (object)this.sInv);
            oleDbCommand1.ExecuteNonQuery();
          }
        }
      }
      this.ResetForm();
    }

    private void submit(object sender, EventArgs e)
    {
      this.txtJobNum.Text.ToUpper();
      if (null == null)
        AutoClosingMessageBox.Show("That invoice number does not exist in the table!", "Error", 6000);
      this.txtJobNum.Text = "";
      this.ActiveControl = (Control)this.txtJobNum;
    }

    private void refresh_click(object sender, EventArgs e) => this.refresh();

    private void menuToolStripMenuItem_Click(object sender, EventArgs e)
    {
      frmMenu frmMenu = new frmMenu();
      PublicMethods.SetDefaultForm();
      frmMenu.FormClosed += (FormClosedEventHandler)((s, args) => this.Close());
      frmMenu.Show();
      this.Hide();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

    private void connectToToolStripMenuItem_Click(object sender, EventArgs e) => PublicMethods.SetBoPath();

    private DateTime DTC(DateTime dt) => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      this.dbRecentlyPegged = new System.Windows.Forms.DataGridView();
      this.dbNotPlaced = new System.Windows.Forms.DataGridView();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
      this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
      this.txtTime = new System.Windows.Forms.TextBox();
      this.txtJobNum = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.menuStrip1 = new JT_Database_App.cmpJTMenuStrip();
      ((System.ComponentModel.ISupportInitialize)(this.dbRecentlyPegged)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dbNotPlaced)).BeginInit();
      this.tableLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      this.tableLayoutPanel3.SuspendLayout();
      this.SuspendLayout();
      // 
      // dbRecentlyPegged
      // 
      this.dbRecentlyPegged.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dbRecentlyPegged.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
      this.dbRecentlyPegged.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dbRecentlyPegged.DefaultCellStyle = dataGridViewCellStyle1;
      this.dbRecentlyPegged.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dbRecentlyPegged.Location = new System.Drawing.Point(0, 446);
      this.dbRecentlyPegged.Margin = new System.Windows.Forms.Padding(0);
      this.dbRecentlyPegged.Name = "dbRecentlyPegged";
      this.dbRecentlyPegged.RowHeadersWidth = 20;
      this.dbRecentlyPegged.RowTemplate.Height = 33;
      this.dbRecentlyPegged.Size = new System.Drawing.Size(887, 281);
      this.dbRecentlyPegged.TabIndex = 6;
      // 
      // dbNotPlaced
      // 
      this.dbNotPlaced.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle2.Format = "g";
      dataGridViewCellStyle2.NullValue = null;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dbNotPlaced.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
      this.dbNotPlaced.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dbNotPlaced.DefaultCellStyle = dataGridViewCellStyle3;
      this.dbNotPlaced.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dbNotPlaced.Location = new System.Drawing.Point(443, 0);
      this.dbNotPlaced.Margin = new System.Windows.Forms.Padding(0);
      this.dbNotPlaced.Name = "dbNotPlaced";
      this.dbNotPlaced.RowHeadersWidth = 20;
      this.dbNotPlaced.RowTemplate.Height = 33;
      this.dbNotPlaced.Size = new System.Drawing.Size(444, 420);
      this.dbNotPlaced.TabIndex = 7;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.dbRecentlyPegged, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.menuStrip1, 0, 0);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 3;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(887, 727);
      this.tableLayoutPanel1.TabIndex = 14;
      // 
      // tableLayoutPanel2
      // 
      this.tableLayoutPanel2.ColumnCount = 2;
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel2.Controls.Add(this.dbNotPlaced, 1, 0);
      this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
      this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 26);
      this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 1;
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel2.Size = new System.Drawing.Size(887, 420);
      this.tableLayoutPanel2.TabIndex = 7;
      // 
      // tableLayoutPanel3
      // 
      this.tableLayoutPanel3.ColumnCount = 2;
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel3.Controls.Add(this.txtTime, 1, 1);
      this.tableLayoutPanel3.Controls.Add(this.txtJobNum, 1, 0);
      this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
      this.tableLayoutPanel3.Controls.Add(this.label2, 0, 1);
      this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
      this.tableLayoutPanel3.Name = "tableLayoutPanel3";
      this.tableLayoutPanel3.RowCount = 2;
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
      this.tableLayoutPanel3.Size = new System.Drawing.Size(443, 420);
      this.tableLayoutPanel3.TabIndex = 8;
      // 
      // txtTime
      // 
      this.txtTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.txtTime.Enabled = false;
      this.txtTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtTime.Location = new System.Drawing.Point(223, 300);
      this.txtTime.Margin = new System.Windows.Forms.Padding(2);
      this.txtTime.Name = "txtTime";
      this.txtTime.Size = new System.Drawing.Size(218, 30);
      this.txtTime.TabIndex = 2;
      // 
      // txtJobNum
      // 
      this.txtJobNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtJobNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtJobNum.Location = new System.Drawing.Point(223, 86);
      this.txtJobNum.Margin = new System.Windows.Forms.Padding(2);
      this.txtJobNum.Name = "txtJobNum";
      this.txtJobNum.Size = new System.Drawing.Size(218, 37);
      this.txtJobNum.TabIndex = 1;
      this.txtJobNum.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtJobNum_KeyUp);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(2, 0);
      this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(217, 210);
      this.label1.TabIndex = 0;
      this.label1.Text = "Job Num";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(2, 210);
      this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(217, 210);
      this.label2.TabIndex = 1;
      this.label2.Text = "Time";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // menuStrip1
      // 
      this.menuStrip1.Form = null;
      this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Padding = new System.Windows.Forms.Padding(3, 1, 0, 1);
      this.menuStrip1.Size = new System.Drawing.Size(887, 24);
      this.menuStrip1.TabIndex = 8;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // frmPlaced
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(887, 727);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.MainMenuStrip = this.menuStrip1;
      this.Margin = new System.Windows.Forms.Padding(2);
      this.Name = "frmPlaced";
      this.Text = "Placed";
      this.Load += new System.EventHandler(this.Form1_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dbRecentlyPegged)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dbNotPlaced)).EndInit();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel3.ResumeLayout(false);
      this.tableLayoutPanel3.PerformLayout();
      this.ResumeLayout(false);

    }
  }
}
