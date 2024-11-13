// Decompiled with JetBrains decompiler
// Type: JT_Database_App.frmChecker
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
  public class frmChecker : Form
  {
    private string connectionStr;
    private Timer TmrRefresh = new Timer();
    private Timer inputChecker = new Timer();
    private string sInv;
    private int sRep = -1;
    private IContainer components;
    private DataGridView dbRecentlyChecked;
    private DataGridView dbNotChecked;
    private TableLayoutPanel tableLayoutPanel1;
    private Label lblJobNum;
    private Label lblCutter;
    private TextBox txtJobNum;
    private TextBox txtChecker;
    private Label lblTime;
    private cmpJTMenuStrip menuStrip1;
    private TextBox txtTime;
    private ContextMenuStrip contextMenuStrip1;
    private Label lblCheckerName;
    private Task refreshTask = Task.Run(() => { });

    public frmChecker()
    {
      this.connectionStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Settings.Default.BOPath;
      this.InitializeComponent();

      menuStrip1.Form = this;
      menuStrip1.InitializeComponents();
    }

    private void frmChecker_Load(object sender, EventArgs e)
    {
      this.txtJobNum.Focus();
      this.TmrRefresh.Interval = 500;
      this.TmrRefresh.Tick += (EventHandler) ((s, a) => this.refresh());
      this.TmrRefresh.Start();
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
      string selectCommandText = "SELECT\n   sc.Invsales AS Inv,\n   sc.[Customer Name] AS Customer,\n   sc.[sale time] AS [Date],\n   SWITCH(\n       NOT sc.cut, 'NA',\n       cut.[cut date] IS NOT NULL, '☑',\n       true, '☐'\n   ) AS cutnum,\n   SWITCH(\n       NOT sc.edge, 'NA',\n       edge.[edge date] IS NOT NULL, '☑',\n       true, '☐'\n   ) AS edgenum,\n   SWITCH(\n       NOT sc.drill, 'NA',\n       drill.[drill date] IS NOT NULL, '☑',\n       true, '☐'\n   ) AS drillnum\nFROM\n   (((Sales_counter AS sc\n   LEFT JOIN cutter    AS cut      ON sc.Invsales = cut.invcutter)\n   LEFT JOIN edger     AS edge     ON sc.Invsales = edge.[inv edger])\n   LEFT JOIN drilling  AS drill    ON sc.Invsales = drill.[inv drill])\n   LEFT JOIN checker   AS chk      ON sc.Invsales = chk.[inv checker]\nWHERE\n   sc.cancelled = NO AND\n   sc.[sale time] >= NOW() - 365 AND\n   chk.[inv checker] IS NULL AND\n   (\n       sc.Cut = NO OR\n       cut.[cut status] = 'completed'\n   ) AND\n   (\n       sc.Edge = NO OR\n       edge.[inv edger] IS NOT NULL\n   ) AND\n   (\n       sc.Drill = NO OR\n       drill.[inv drill] IS NOT NULL\n   )\nORDER BY\n   sc.urgent,\n   sc.[sales date],\n   sc.[sale time];";
      string cmdText = "SELECT\n   chk.[inv checker] AS Inv, reps.[first name] AS Checker, chk.[check date] AS [Date]\nFROM\n   checker AS chk\n   LEFT JOIN reps ON chk.checker = reps.[employ no]\nWHERE\n   chk.[check date] >=  NOW() - @minutes/(24*60)\nORDER BY chk.[check time];";
      DataTable dataTable1 = new DataTable();
      DataTable dataTable2 = new DataTable();
      try
      {
        using (OleDbConnection oleDbConnection = new OleDbConnection(this.connectionStr))
        {
          using (OleDbDataAdapter oleDbDataAdapter1 = new OleDbDataAdapter(selectCommandText, oleDbConnection))
          {
            using (OleDbCommand selectCommand = new OleDbCommand(cmdText, oleDbConnection))
            {
              using (OleDbDataAdapter oleDbDataAdapter2 = new OleDbDataAdapter(selectCommand))
              {
                oleDbDataAdapter2.SelectCommand.Parameters.AddWithValue("@minutes", (object)Validation.showForMins);
                oleDbConnection.Open();
                oleDbDataAdapter1.Fill(dataTable1);
                oleDbDataAdapter2.Fill(dataTable2);
                dataTable1.Columns.Add(new DataColumn("Cut complete", typeof(string), "cutnum"));
                dataTable1.Columns.Add(new DataColumn("Edge complete", typeof(string), "edgenum"));
                dataTable1.Columns.Add(new DataColumn("Drill complete", typeof(string), "drillnum"));
              }
            }
          }
        }
      }
      catch (OleDbException ex)
      {
        Console.WriteLine(ex.Message);
        frmMenu frmMenu = new frmMenu();
        PublicMethods.SetDefaultForm((Form)frmMenu);
        frmMenu.FormClosed += (FormClosedEventHandler)((s, args) => this.Close());
        frmMenu.Show();
        this.Hide();
        return;
      }
      new Validation().getHours();
      this.Invoke(new Action(() => { 
      
        this.dbNotChecked.RowTemplate.DefaultCellStyle.Font = new Font(FontFamily.GenericSansSerif, 8f);
        this.dbNotChecked.RowTemplate.Height = 25;
        if (!this.compareDataTables(dataTable1, (DataTable)this.dbNotChecked.DataSource))
          this.dbNotChecked.DataSource = (object)dataTable1;
        if (this.dbNotChecked.DataSource != null)
        {
          this.dbNotChecked.Columns["Date"].Visible = false;
          this.dbNotChecked.Columns["cutnum"].Visible = false;
          this.dbNotChecked.Columns["edgenum"].Visible = false;
          this.dbNotChecked.Columns["drillnum"].Visible = false;
          this.dbNotChecked.Columns["Inv"].Width = 80;
          this.dbNotChecked.Columns["Cut complete"].Width = 60;
          this.dbNotChecked.Columns["Edge complete"].Width = 60;
          this.dbNotChecked.Columns["Drill complete"].Width = 60;
        }
        this.dbRecentlyChecked.RowTemplate.DefaultCellStyle.Font = new Font(FontFamily.GenericSansSerif, 10f);
        this.dbRecentlyChecked.RowTemplate.Height = 25;
        if (!this.compareDataTables(dataTable2, (DataTable)this.dbRecentlyChecked.DataSource))
          this.dbRecentlyChecked.DataSource = (object)dataTable2;
        if (this.dbRecentlyChecked.DataSource != null)
        {
          this.dbRecentlyChecked.Columns["Inv"].Width = 150;
          this.dbRecentlyChecked.Columns["Checker"].Width = 130;
          this.dbRecentlyChecked.Columns["Date"].DefaultCellStyle.Format = "dd/MM/yy HH:mm:ss";
        }
      }));
    }

    private void ResetForm()
    {
      this.sInv = (string) null;
      this.sRep = -1;
      Color white = Color.White;
      Color black = Color.Black;
      string str = "N/A";
      this.txtJobNum.Text = "";
      this.txtJobNum.BackColor = white;
      this.txtChecker.Text = "";
      this.txtChecker.BackColor = white;
      this.lblCheckerName.Text = str;
      this.lblCheckerName.ForeColor = black;
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
    
    private void txtChecker_KeyUp(object sender, KeyEventArgs e)
    {
      this.inputChecker.Dispose();
      this.txtChecker.BackColor = Color.White;
      if (this.txtChecker.Text == "")
        return;
      this.inputChecker = new Timer();
      this.inputChecker.Interval = 1000;
      this.inputChecker.Tick += (EventHandler) ((s, a) =>
      {
        this.inputChecker.Dispose();
        this.txtChecker.Invoke(new Action(() => ValidateChecker()));
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
        if (new Validation().invChecker(this.txtJobNum.Text))
        {
          this.sInv = this.txtJobNum.Text;
          this.txtChecker.Focus();
          this.txtJobNum.BackColor = Color.Green;
        }
        else
        {
          this.sInv = (string) null;
          AutoClosingMessageBox.Show(string.Format("Invoice {0} is not in the table or isn't available for checking!", (object) this.txtJobNum.Text), "Error", 3000);
          this.txtJobNum.Text = "";
        }
      }
    }

    private void ValidateChecker()
    {
      if (this.txtChecker.Text.Contains("CANCEL"))
      {
        this.ResetForm();
      }
      else
      {
        int num = new Validation().rep(this.txtChecker.Text);
        if (num != -1)
        {
          this.sRep = num;
          this.txtChecker.BackColor = Color.Green;
          this.SubmitData();
        }
        else
        {
          this.sRep = -1;
          AutoClosingMessageBox.Show(string.Format("Rep {0} is not in the table!", (object) this.txtChecker.Text), "Error", 3000);
          this.txtChecker.Text = "";
          this.txtChecker.Focus();
        }
      }
    }

    private void SubmitData()
    {
      if (this.txtJobNum.Text.Equals("") || this.txtChecker.Text.Equals(""))
      {
        AutoClosingMessageBox.Show("One of the form's fields are empty!", "Error", 3000);
        this.ResetForm();
      }
      else
      {
        string cmdText = "INSERT INTO checker(id, [inv checker], checker, [check date], [check time]) VALUES(@id, @inv, @checker, @date, @time);";
        using (OleDbConnection connection = new OleDbConnection(this.connectionStr))
        {
          using (OleDbCommand oleDbCommand1 = new OleDbCommand(cmdText, connection))
          {
            using (OleDbCommand oleDbCommand2 = new OleDbCommand("SELECT id FROM checker ORDER BY id DESC", connection))
            {
              connection.Open();
              int num = (int) oleDbCommand2.ExecuteScalar() + 1;
              oleDbCommand1.Parameters.AddWithValue("@id", (object) num);
              oleDbCommand1.Parameters.AddWithValue("@inv", (object) this.sInv);
              oleDbCommand1.Parameters.AddWithValue("@checker", (object) this.sRep);
              oleDbCommand1.Parameters.AddWithValue("@date", (object) this.DTC(DateTime.Now));
              oleDbCommand1.Parameters.AddWithValue("@time", (object) this.DTC(DateTime.Now));
              oleDbCommand1.ExecuteNonQuery();
            }
          }
        }
        this.ResetForm();
      }
    }

    private DateTime DTC(DateTime dt) => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);

    private bool compareDataTables(DataTable a, DataTable b)
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

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      this.dbRecentlyChecked = new System.Windows.Forms.DataGridView();
      this.dbNotChecked = new System.Windows.Forms.DataGridView();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.lblJobNum = new System.Windows.Forms.Label();
      this.lblCutter = new System.Windows.Forms.Label();
      this.lblTime = new System.Windows.Forms.Label();
      this.txtJobNum = new System.Windows.Forms.TextBox();
      this.txtChecker = new System.Windows.Forms.TextBox();
      this.txtTime = new System.Windows.Forms.TextBox();
      this.lblCheckerName = new System.Windows.Forms.Label();
      this.menuStrip1 = new JT_Database_App.cmpJTMenuStrip();
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.dbRecentlyChecked)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dbNotChecked)).BeginInit();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // dbRecentlyChecked
      // 
      this.dbRecentlyChecked.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dbRecentlyChecked.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dbRecentlyChecked.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dbRecentlyChecked.DefaultCellStyle = dataGridViewCellStyle2;
      this.dbRecentlyChecked.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.dbRecentlyChecked.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
      this.dbRecentlyChecked.Location = new System.Drawing.Point(0, 105);
      this.dbRecentlyChecked.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.dbRecentlyChecked.Name = "dbRecentlyChecked";
      this.dbRecentlyChecked.ReadOnly = true;
      this.dbRecentlyChecked.RowHeadersWidth = 20;
      this.dbRecentlyChecked.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
      this.dbRecentlyChecked.RowTemplate.Height = 35;
      this.dbRecentlyChecked.Size = new System.Drawing.Size(642, 260);
      this.dbRecentlyChecked.TabIndex = 0;
      // 
      // dbNotChecked
      // 
      this.dbNotChecked.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dbNotChecked.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
      this.dbNotChecked.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle4.Font = new System.Drawing.Font("Matura MT Script Capitals", 5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dbNotChecked.DefaultCellStyle = dataGridViewCellStyle4;
      this.dbNotChecked.Dock = System.Windows.Forms.DockStyle.Right;
      this.dbNotChecked.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
      this.dbNotChecked.Location = new System.Drawing.Point(142, 24);
      this.dbNotChecked.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.dbNotChecked.Name = "dbNotChecked";
      this.dbNotChecked.ReadOnly = true;
      this.dbNotChecked.RowHeadersWidth = 20;
      this.dbNotChecked.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.dbNotChecked.RowTemplate.Height = 30;
      this.dbNotChecked.Size = new System.Drawing.Size(500, 81);
      this.dbNotChecked.TabIndex = 1;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 3;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
      this.tableLayoutPanel1.Controls.Add(this.lblJobNum, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.lblCutter, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.lblTime, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.txtJobNum, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this.txtChecker, 1, 1);
      this.tableLayoutPanel1.Controls.Add(this.txtTime, 1, 2);
      this.tableLayoutPanel1.Controls.Add(this.lblCheckerName, 2, 1);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 3;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(142, 81);
      this.tableLayoutPanel1.TabIndex = 2;
      // 
      // lblJobNum
      // 
      this.lblJobNum.AutoSize = true;
      this.lblJobNum.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblJobNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblJobNum.Location = new System.Drawing.Point(2, 0);
      this.lblJobNum.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.lblJobNum.Name = "lblJobNum";
      this.lblJobNum.Size = new System.Drawing.Size(43, 27);
      this.lblJobNum.TabIndex = 0;
      this.lblJobNum.Text = "Job Num";
      this.lblJobNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // lblCutter
      // 
      this.lblCutter.AutoSize = true;
      this.lblCutter.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblCutter.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblCutter.Location = new System.Drawing.Point(2, 27);
      this.lblCutter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.lblCutter.Name = "lblCutter";
      this.lblCutter.Size = new System.Drawing.Size(43, 27);
      this.lblCutter.TabIndex = 2;
      this.lblCutter.Text = "Checker";
      this.lblCutter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // lblTime
      // 
      this.lblTime.AutoSize = true;
      this.lblTime.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTime.Location = new System.Drawing.Point(2, 54);
      this.lblTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.lblTime.Name = "lblTime";
      this.lblTime.Size = new System.Drawing.Size(43, 27);
      this.lblTime.TabIndex = 4;
      this.lblTime.Text = "Time";
      this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // txtJobNum
      // 
      this.txtJobNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtJobNum.BackColor = System.Drawing.SystemColors.Window;
      this.txtJobNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtJobNum.Location = new System.Drawing.Point(49, 2);
      this.txtJobNum.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.txtJobNum.Name = "txtJobNum";
      this.txtJobNum.Size = new System.Drawing.Size(43, 38);
      this.txtJobNum.TabIndex = 6;
      this.txtJobNum.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtJobNum_KeyUp);
      // 
      // txtChecker
      // 
      this.txtChecker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtChecker.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtChecker.Location = new System.Drawing.Point(49, 29);
      this.txtChecker.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.txtChecker.Name = "txtChecker";
      this.txtChecker.Size = new System.Drawing.Size(43, 38);
      this.txtChecker.TabIndex = 5;
      this.txtChecker.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtChecker_KeyUp);
      // 
      // txtTime
      // 
      this.txtTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.tableLayoutPanel1.SetColumnSpan(this.txtTime, 2);
      this.txtTime.Enabled = false;
      this.txtTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtTime.Location = new System.Drawing.Point(49, 56);
      this.txtTime.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.txtTime.Name = "txtTime";
      this.txtTime.Size = new System.Drawing.Size(91, 31);
      this.txtTime.TabIndex = 14;
      // 
      // lblCheckerName
      // 
      this.lblCheckerName.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.lblCheckerName.AutoSize = true;
      this.lblCheckerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblCheckerName.Location = new System.Drawing.Point(102, 27);
      this.lblCheckerName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.lblCheckerName.Name = "lblCheckerName";
      this.lblCheckerName.Size = new System.Drawing.Size(32, 27);
      this.lblCheckerName.TabIndex = 12;
      this.lblCheckerName.Text = "N/A";
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
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
      // 
      // frmChecker
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(642, 365);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Controls.Add(this.dbNotChecked);
      this.Controls.Add(this.dbRecentlyChecked);
      this.Controls.Add(this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.Name = "frmChecker";
      this.Text = "Checker";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.Load += new System.EventHandler(this.frmChecker_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dbRecentlyChecked)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dbNotChecked)).EndInit();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }
  }
}
