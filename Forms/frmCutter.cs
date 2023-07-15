// Decompiled with JetBrains decompiler
// Type: JT_Database_App.frmCutter
// Assembly: JT Database App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D649E70A-0AB7-4AF5-A104-CFA0EBCF124B
// Assembly location: C:\Users\Skye\Desktop\JT Database App 23aug21.exe

using JT_Database_App.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JT_Database_App
{
  public class frmCutter : Form
  {
    private string connectionStr;
    private Timer TmrRefresh = new Timer();
    private Timer inputChecker = new Timer();
    private int curStatus = -1;
    private string sInv;
    private int sRep = -1;
    private int sMachine = -1;
    private int sStatus = -1;
    private IContainer components;
    private DataGridView dbRecentlyCut;
    private DataGridView dbNotCut;
    private TableLayoutPanel tableLayoutPanel1;
    private Label lblJobNum;
    private Label lblCutter;
    private Label lblMachine;
    private TextBox txtJobNum;
    private TextBox txtCutter;
    private Label lblTime;
    private TextBox txtMachine;
    private cmpJTMenuStrip menuStrip1;
    private Label lblCutterName;
    private Label lblMachineName;
    private TextBox txtTime;
    private Label lblStatus;
    private TextBox txtStatus;
    private TableLayoutPanel tableLayoutPanel2;
    private TableLayoutPanel tableLayoutPanel3;
    private Label lblStatusName;
    private DataGridViewCellStyle dataGridViewCellStyleDefault;
    private DataGridViewCellStyle dataGridViewCellStyleDateFormat;
    private DataGridViewCellStyle dataGridViewCellStyleRed;
    private DataGridViewCellStyle dataGridViewCellStyleLightGrey;
    private DataGridViewCellStyle dataGridViewCellStyleOrange;
    private DataGridViewCellStyle dataGridViewCellStyleYellow;
    private Task refreshTask = Task.Run(() => { });

    public frmCutter()
    {
      this.connectionStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Settings.Default.BOPath;
      this.InitializeComponent();

      this.menuStrip1.Form = this;
      this.menuStrip1.InitializeComponents();
      this.CustomSuspended();
    }

    private void frmCutter_Load(object sender, EventArgs e)
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
        } catch(AggregateException aex)
        {
          Exception ex = aex.GetBaseException();
          Debug.WriteLine(ex.StackTrace);
          throw ex;
        }
      }
    }

    private async Task FillTables()
    {
      // Debug.WriteLine("ASYNC TASK STARTED");
      string notCutCmdText = "" +
        "SELECT\n" +
        " sc.Invsales, sc.[Customer name], sc.timber, sc.urgent, FIX(NOW() - sc.[sale time]) AS [Days ago], sc.[sale time], c.[cut status]\n" +
        "FROM\n" +
        " (\n" +
        "   (\n" +
        "     (\n" +
        "       Sales_counter AS sc\n" +
        "       LEFT JOIN cutter AS c ON sc.Invsales = c.invcutter\n" +
        "     )\n" +
        "     LEFT JOIN filed AS f ON sc.Invsales = f.inv_filled\n" +
        "   )\n" +
        "   LEFT JOIN placed AS p ON sc.Invsales = p.InvQ_LU\n" +
        " )\n" +
        "WHERE\n" +
        " sc.Cut = Yes AND\n" +
        " p.InvQ_LU Is Not Null AND\n" +
        " (\n" +
        "   (\n" +
        "     (sc.Cut = Yes) AND\n" +
        "     (sc.Timber = Yes) AND\n" +
        "     (sc.Urgent = Yes) AND\n" +
        "     (f.inv_filled Is Null) AND\n" +
        "     (p.InvQ_LU Is Not Null) AND\n" +
        "     (sc.Cancelled = No) AND\n" +
        "     (c.[cut date] Is Null)\n" +
        "   ) OR\n" +
        "   (\n" +
        "     (sc.Cut = Yes) AND\n" +
        "     (f.inv_filled Is Null) AND\n" +
        "     (p.InvQ_LU Is Not Null) AND\n" +
        "     (sc.Cancelled = No) AND\n" +
        "     (c.[cut date] Is Null)\n" +
        "   )\n" +
        " ) OR\n" +
        " c.[cut status] = 'paused'\n" +
        "ORDER BY\n" +
        " sc.urgent, c.[cut status] DESC, sc.timber, sc.[sales date], sc.[sale time];";
      string cmdText = "SELECT    c.invcutter, c.cutter, c.[cut time], c.[cut status],    SWITCH(       m.machine IS NULL, c.machine,       TRUE, m.machine    ) as machine,    sc.[customer name] FROM(cutter AS c    LEFT JOIN sales_counter AS sc ON c.invcutter = sc.invsales)    LEFT OUTER JOIN machines AS m ON c.machine = CStr(m.ID) WHERE    c.[cut status] <> 'completed' OR   c.[cut date] > NOW() - @minutes/(24*60) ORDER BY c.[cut status] = 'completed', c.[cut time]";
      DataTable dataTable1 = new DataTable();
      DataTable dataTable2 = new DataTable();
      {
        try
        {
          using (OleDbConnection oleDbConnection = new OleDbConnection(this.connectionStr))
          {
            await oleDbConnection.OpenAsync();
            using (OleDbDataAdapter oleDbDataAdapter1 = new OleDbDataAdapter(notCutCmdText, oleDbConnection))
            {
              oleDbDataAdapter1.Fill(dataTable1);
              using (OleDbCommand selectCommand = new OleDbCommand(cmdText, oleDbConnection))
              using (OleDbDataAdapter oleDbDataAdapter2 = new OleDbDataAdapter(selectCommand))
              {
                oleDbDataAdapter2.SelectCommand.Parameters.AddWithValue("@minutes", (object)Validation.showForMins);
                oleDbDataAdapter2.Fill(dataTable2);
              }
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
        Dictionary<string, int> hours = new Validation().getHours();
        this.Invoke(new Action(() =>
        {
          this.dbNotCut.RowTemplate.DefaultCellStyle.Font = new Font(FontFamily.GenericSansSerif, 8f);
          this.dbNotCut.RowTemplate.Height = 25;
          if (!Validation.CompareDataTables(dataTable1, (DataTable)this.dbNotCut.DataSource))
            this.dbNotCut.DataSource = (object)dataTable1;
          if (this.dbNotCut.DataSource != null)
          {
            this.dbNotCut.Columns["invsales"].Width = 110;
            this.dbNotCut.Columns["timber"].Width = 50;
            this.dbNotCut.Columns["urgent"].Width = 50;
            this.dbNotCut.Columns["Days ago"].Width = 50;
            this.dbNotCut.Columns["sale time"].Visible = false;
            this.dbNotCut.Columns["cut status"].Visible = false;
            foreach (DataGridViewRow row in (IEnumerable)this.dbNotCut.Rows)
            {
              row.DefaultCellStyle = dataGridViewCellStyleDefault;
              if (row.Cells["timber"] != null && row.Cells["timber"].Value != null)
              {
                if ((bool)row.Cells["urgent"].Value)
                  row.DefaultCellStyle = dataGridViewCellStyleRed;
                else if ((bool)row.Cells["timber"].Value && (DateTime)row.Cells["sale time"].Value > DateTime.Now.AddHours((double)-hours["timber"]) || !(bool)row.Cells["timber"].Value && (DateTime)row.Cells["sale time"].Value > DateTime.Now.AddHours((double)-hours["boards"]))
                  row.DefaultCellStyle.BackColor = Color.LightGray;
              }
              if (!(row.Cells["cut status"].Value is DBNull))
                row.DefaultCellStyle = dataGridViewCellStyleYellow;
              if (row.Cells["Days ago"].Value != null && int.Parse(row.Cells["Days ago"].Value.ToString()) > 5)
                row.Cells["Days ago"].Style = dataGridViewCellStyleOrange;
            }
          }
          this.dbRecentlyCut.RowTemplate.DefaultCellStyle = dataGridViewCellStyleDefault;
          this.dbRecentlyCut.RowTemplate.Height = 25;
          if (!Validation.CompareDataTables(dataTable2, (DataTable)this.dbRecentlyCut.DataSource))
            this.dbRecentlyCut.DataSource = (object)dataTable2;
          if (this.dbRecentlyCut.DataSource != null)
          {
            this.dbRecentlyCut.Columns["invcutter"].Width = 150;
            this.dbRecentlyCut.Columns["cut status"].Width = 130;
            this.dbRecentlyCut.Columns["machine"].Width = 150;
            this.dbRecentlyCut.Columns["cut time"].DefaultCellStyle = dataGridViewCellStyleDateFormat;
          }
        }));
      };
    }

    private void ResetForm()
    {
      this.curStatus = -1;
      this.sInv = (string) null;
      this.sRep = -1;
      this.sStatus = -1;
      this.sMachine = -1;
      Color white = Color.White;
      Color black = Color.Black;
      string str = "N/A";
      this.txtJobNum.Text = "";
      this.txtJobNum.BackColor = white;
      this.txtCutter.Text = "";
      this.txtCutter.BackColor = white;
      this.lblCutterName.Text = str;
      this.lblCutterName.ForeColor = black;
      this.txtMachine.Text = "";
      this.txtMachine.BackColor = white;
      this.lblMachineName.Text = str;
      this.lblMachineName.ForeColor = black;
      this.txtStatus.Text = "";
      this.txtStatus.BackColor = white;
      this.lblStatusName.Text = str;
      this.lblStatusName.ForeColor = black;
      this.txtJobNum.Focus();
    }

    private void input_KeyUp(object sender, KeyEventArgs e)
    {
      if (!(sender is TextBox)) return;
      TextBox _sender = (TextBox)sender;

      this.inputChecker.Dispose();
      _sender.BackColor = Color.White;
      if (_sender.Text == "") return;

      Action action = () =>
      {
        if (this.txtJobNum.Text.EndsWith("CANCEL"))
        {
          this.ResetForm();
          return;
        }
        if (_sender.Text.ToLower().StartsWith("frm"))
        {
          if (ChangeForm.Name((Form)this, _sender.Text))
            return;
          AutoClosingMessageBox.Show(string.Format("Form {0} is not isn't available or doesn't exist!", (object)this.txtJobNum.Text), "Error", 3000);
          this.ResetForm();
          return;
        }

        if (_sender == txtJobNum)
        {
          this.inputChecker.Dispose();
          this.txtJobNum.Invoke(new Action(() => this.ValidateJobNumber()));
          return;
        };
        if (_sender == txtCutter)
        {
          this.inputChecker.Dispose();
          this.txtCutter.Invoke(new Action(() => this.ValidateCutter()));
          return;
        };
        if (_sender == txtMachine)
        {
          this.inputChecker.Dispose();
          this.txtMachine.Invoke(new Action(() => this.ValidateMachine()));
          return;
        };
        if (_sender == txtStatus)
        {
          this.inputChecker.Dispose();
          this.txtStatus.Invoke(new Action(() => this.ValidateStatus()));
          return;
        };
      };

      if(!AppSettings.ManualInput)
      {
        this.inputChecker = new Timer();
        this.inputChecker.Interval = 100;
        this.inputChecker.Tick += (EventHandler)((s, a) =>
        {
          action();
        });
        this.inputChecker.Start();
      } else if(e.KeyCode == Keys.Return)
      {
        action();
      }
    }

    private void ValidateJobNumber()
    {
      this.txtJobNum.Text = this.txtJobNum.Text.ToUpper();
      if (new Validation().invCutter(this.txtJobNum.Text))
      {
        FloatingForm frmf = new FloatingForm();
        frmf.Show();
        // frmf.FormClosing += (a, b) => { ResetForm(); };
        // frmf.Focus();

        using (OdbcConnection IQConn = new OdbcConnection(PublicMethods.IQConnectionString))
        using (OdbcCommand odbcCommandIQ = new OdbcCommand("SELECT stk.descript, COUNT(stk.descript), 'Not started' AS status from stoctran as st LEFT JOIN stock as stk ON st.code = stk.code WHERE st.reference = ? GROUP BY stk.descript", IQConn))
        {
          odbcCommandIQ.Parameters.AddWithValue("@inv", (object)this.txtJobNum.Text);
          IQConn.Open();
          frmf.setData(odbcCommandIQ);

          OdbcDataReader reader = odbcCommandIQ.ExecuteReader();
          bool res = reader.Read();
          Type type;
          while(res)
          {
            Debug.WriteLine($"{reader.GetString(0)} | {reader.GetInt32(1)}");
            res = reader.Read();
          };
          Debug.WriteLine("");
        }

        bool _btmp = frmf.TopMost;
        frmf.TopMost = true;
        // frmf.TopMost = _btmp;
        this.curStatus = new Validation().getCutStatus(this.txtJobNum.Text);
        this.sInv = this.txtJobNum.Text;
        this.txtCutter.Focus();
        this.txtJobNum.BackColor = Color.Green;
      }
      else
      {
        this.curStatus = -1;
        this.sInv = (string) null;
        AutoClosingMessageBox.Show(string.Format("Invoice {0} is not in the table or isn't available for cutting!", (object) this.txtJobNum.Text), "Error", 3000);
        this.txtJobNum.Text = "";
      }
    }

    private void ValidateCutter()
    {
      int num = new Validation().rep(this.txtCutter.Text);
      if (num != -1)
      {
        this.sRep = num;
        this.txtMachine.Focus();
        this.txtCutter.BackColor = Color.Green;
      }
      else
      {
        this.sRep = -1;
        AutoClosingMessageBox.Show(string.Format("Cutter {0} is not in the table!", (object) this.txtCutter.Text), "Error", 3000);
        this.txtCutter.Text = "";
      }
    }

    private void ValidateMachine()
    {
      int num = new Validation().cutMachine(this.txtMachine.Text);
      if (num != -1)
      {
        this.sMachine = num;
        this.txtStatus.Focus();
        this.txtMachine.BackColor = Color.Green;
      }
      else
      {
        this.sMachine = -1;
        AutoClosingMessageBox.Show(string.Format("Machine {0} does not exist!", (object) this.txtMachine.Text), "Error", 3000);
        this.txtMachine.Text = "";
      }
    }

    private void ValidateStatus()
    {
      int _status = new Validation().status(this.txtStatus.Text);
      if (_status != -1)
      {
        this.sStatus = _status;
        if (this.curStatus != -1)
        {
          if (_status != this.curStatus)
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

    private void SubmitData()
    {
      if (this.txtJobNum.Text.Equals("") || this.txtCutter.Text.Equals("") || this.txtMachine.Text.Equals("") || this.txtStatus.Text.Equals(""))
      {
        AutoClosingMessageBox.Show("One of the form's fields are empty!", "Error", 3000);
        this.ResetForm();
      }
      else
      {
        string cmdText = "INSERT INTO cutter VALUES(@id, null, @inv, @cutter, @date, @time, @status, @end, @machine);";
        using (OleDbConnection connection = new OleDbConnection(this.connectionStr))
        {
          using (OleDbCommand oleDbCommand1 = new OleDbCommand(cmdText, connection))
          {
            using (OleDbCommand oleDbCommand2 = new OleDbCommand("SELECT id FROM cutter ORDER BY id DESC", connection))
            {
              connection.Open();
              int num = (int) oleDbCommand2.ExecuteScalar() + 1;
              oleDbCommand1.Parameters.AddWithValue("@id", (object) num);
              oleDbCommand1.Parameters.AddWithValue("@inv", (object) this.sInv);
              oleDbCommand1.Parameters.AddWithValue("@cutter", (object) Validation.RepName(this.sRep));
              oleDbCommand1.Parameters.AddWithValue("@date", (object) this.DTC(DateTime.Now));
              oleDbCommand1.Parameters.AddWithValue("@time", (object) this.DTC(DateTime.Now));
              oleDbCommand1.Parameters.AddWithValue("@status", (object) Validation.StatusName(this.sStatus));
              if (Validation.StatusName(this.sStatus).Equals("completed"))
                oleDbCommand1.Parameters.AddWithValue("@end", (object) this.DTC(DateTime.Now));
              else
                oleDbCommand1.Parameters.AddWithValue("@end", (object) DBNull.Value);
              oleDbCommand1.Parameters.AddWithValue("@machine", (object) this.sMachine);
              oleDbCommand1.ExecuteNonQuery();
            }
          }
        }
        this.ResetForm();
      }
    }

    private void UpdateData()
    {
      string cmdText = "UPDATE cutter SET [cut status] = @status, [fin cut] = @fin WHERE invcutter = @inv";
      using (OleDbConnection connection = new OleDbConnection(this.connectionStr))
      {
        using (OleDbCommand oleDbCommand = new OleDbCommand(cmdText, connection))
        {
          connection.Open();
          oleDbCommand.Parameters.AddWithValue("@status", (object) Validation.StatusName(this.sStatus));
          if (Validation.StatusName(this.sStatus).Equals("completed"))
            oleDbCommand.Parameters.AddWithValue("@fin", (object) this.DTC(DateTime.Now));
          else
            oleDbCommand.Parameters.AddWithValue("@fin", (object) DBNull.Value);
          oleDbCommand.Parameters.AddWithValue("@inv", (object) this.sInv);
          oleDbCommand.ExecuteNonQuery();
        }
      }
      this.ResetForm();
    }

    private DateTime DTC(DateTime dt) => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void CustomInitialize()
    {
      //this.menuStrip1 = new cmpJTMenuStrip(this);
    }

    private void CustomSuspended()
    {
      this.dataGridViewCellStyleDefault = new DataGridViewCellStyle(this.dbNotCut.RowsDefaultCellStyle);
      this.dataGridViewCellStyleDefault.Font = new Font(FontFamily.GenericSansSerif, 10f);

      this.dataGridViewCellStyleDateFormat = new DataGridViewCellStyle(dataGridViewCellStyleDefault);
      this.dataGridViewCellStyleDateFormat.Format = "dd/MM/yy HH:mm:ss";

      this.dataGridViewCellStyleRed = new DataGridViewCellStyle(dataGridViewCellStyleDefault);
      this.dataGridViewCellStyleRed.BackColor = Color.Red;

      this.dataGridViewCellStyleLightGrey = new DataGridViewCellStyle(dataGridViewCellStyleDefault);
      this.dataGridViewCellStyleLightGrey.BackColor = Color.LightGray;

      this.dataGridViewCellStyleOrange = new DataGridViewCellStyle(dataGridViewCellStyleDefault);
      this.dataGridViewCellStyleOrange.BackColor = Color.Orange;

      this.dataGridViewCellStyleYellow = new DataGridViewCellStyle(dataGridViewCellStyleDefault);
      this.dataGridViewCellStyleYellow.BackColor = Color.FromArgb(255, 255, 200);

      //menuStrip1.InitializeComponents();
    }

    private void InitializeComponent()
    {
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      this.menuStrip1 = new JT_Database_App.cmpJTMenuStrip();
      this.dbRecentlyCut = new System.Windows.Forms.DataGridView();
      this.dbNotCut = new System.Windows.Forms.DataGridView();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.lblJobNum = new System.Windows.Forms.Label();
      this.lblCutter = new System.Windows.Forms.Label();
      this.lblMachine = new System.Windows.Forms.Label();
      this.lblStatus = new System.Windows.Forms.Label();
      this.lblTime = new System.Windows.Forms.Label();
      this.txtJobNum = new System.Windows.Forms.TextBox();
      this.txtCutter = new System.Windows.Forms.TextBox();
      this.txtMachine = new System.Windows.Forms.TextBox();
      this.txtStatus = new System.Windows.Forms.TextBox();
      this.txtTime = new System.Windows.Forms.TextBox();
      this.lblCutterName = new System.Windows.Forms.Label();
      this.lblMachineName = new System.Windows.Forms.Label();
      this.lblStatusName = new System.Windows.Forms.Label();
      this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
      this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
      ((System.ComponentModel.ISupportInitialize)(this.dbRecentlyCut)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dbNotCut)).BeginInit();
      this.tableLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      this.tableLayoutPanel3.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStrip1
      // 
      this.menuStrip1.Form = null;
      this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
      this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(1284, 48);
      this.menuStrip1.TabIndex = 8;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // dbRecentlyCut
      // 
      this.dbRecentlyCut.AllowUserToAddRows = false;
      this.dbRecentlyCut.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dbRecentlyCut.BorderStyle = System.Windows.Forms.BorderStyle.None;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dbRecentlyCut.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dbRecentlyCut.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dbRecentlyCut.DefaultCellStyle = dataGridViewCellStyle2;
      this.dbRecentlyCut.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dbRecentlyCut.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
      this.dbRecentlyCut.Location = new System.Drawing.Point(0, 441);
      this.dbRecentlyCut.Margin = new System.Windows.Forms.Padding(0);
      this.dbRecentlyCut.Name = "dbRecentlyCut";
      this.dbRecentlyCut.ReadOnly = true;
      this.dbRecentlyCut.RowHeadersWidth = 20;
      this.dbRecentlyCut.RowTemplate.Height = 35;
      this.dbRecentlyCut.Size = new System.Drawing.Size(1284, 261);
      this.dbRecentlyCut.TabIndex = 0;
      // 
      // dbNotCut
      // 
      this.dbNotCut.AllowUserToAddRows = false;
      this.dbNotCut.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dbNotCut.BorderStyle = System.Windows.Forms.BorderStyle.None;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dbNotCut.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
      this.dbNotCut.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle4.Font = new System.Drawing.Font("Matura MT Script Capitals", 5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dbNotCut.DefaultCellStyle = dataGridViewCellStyle4;
      this.dbNotCut.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dbNotCut.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
      this.dbNotCut.Location = new System.Drawing.Point(642, 0);
      this.dbNotCut.Margin = new System.Windows.Forms.Padding(0);
      this.dbNotCut.Name = "dbNotCut";
      this.dbNotCut.ReadOnly = true;
      this.dbNotCut.RowHeadersWidth = 20;
      this.dbNotCut.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.dbNotCut.RowTemplate.Height = 30;
      this.dbNotCut.Size = new System.Drawing.Size(642, 391);
      this.dbNotCut.TabIndex = 1;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 3;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
      this.tableLayoutPanel1.Controls.Add(this.lblJobNum, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.lblCutter, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.lblMachine, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.lblStatus, 0, 3);
      this.tableLayoutPanel1.Controls.Add(this.lblTime, 0, 4);
      this.tableLayoutPanel1.Controls.Add(this.txtJobNum, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this.txtCutter, 1, 1);
      this.tableLayoutPanel1.Controls.Add(this.txtMachine, 1, 2);
      this.tableLayoutPanel1.Controls.Add(this.txtStatus, 1, 3);
      this.tableLayoutPanel1.Controls.Add(this.txtTime, 1, 4);
      this.tableLayoutPanel1.Controls.Add(this.lblCutterName, 2, 1);
      this.tableLayoutPanel1.Controls.Add(this.lblMachineName, 2, 2);
      this.tableLayoutPanel1.Controls.Add(this.lblStatusName, 2, 3);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 5;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(642, 391);
      this.tableLayoutPanel1.TabIndex = 2;
      // 
      // lblJobNum
      // 
      this.lblJobNum.AutoSize = true;
      this.lblJobNum.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblJobNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblJobNum.Location = new System.Drawing.Point(4, 0);
      this.lblJobNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lblJobNum.Name = "lblJobNum";
      this.lblJobNum.Size = new System.Drawing.Size(205, 78);
      this.lblJobNum.TabIndex = 0;
      this.lblJobNum.Text = "Job Num";
      this.lblJobNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // lblCutter
      // 
      this.lblCutter.AutoSize = true;
      this.lblCutter.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblCutter.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblCutter.Location = new System.Drawing.Point(4, 78);
      this.lblCutter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lblCutter.Name = "lblCutter";
      this.lblCutter.Size = new System.Drawing.Size(205, 78);
      this.lblCutter.TabIndex = 2;
      this.lblCutter.Text = "Cutter";
      this.lblCutter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // lblMachine
      // 
      this.lblMachine.AutoSize = true;
      this.lblMachine.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblMachine.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblMachine.Location = new System.Drawing.Point(4, 156);
      this.lblMachine.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lblMachine.Name = "lblMachine";
      this.lblMachine.Size = new System.Drawing.Size(205, 78);
      this.lblMachine.TabIndex = 1;
      this.lblMachine.Text = "Machine";
      this.lblMachine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // lblStatus
      // 
      this.lblStatus.AutoSize = true;
      this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblStatus.Location = new System.Drawing.Point(4, 234);
      this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new System.Drawing.Size(205, 78);
      this.lblStatus.TabIndex = 15;
      this.lblStatus.Text = "Status";
      this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // lblTime
      // 
      this.lblTime.AutoSize = true;
      this.lblTime.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTime.Location = new System.Drawing.Point(4, 312);
      this.lblTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lblTime.Name = "lblTime";
      this.lblTime.Size = new System.Drawing.Size(205, 79);
      this.lblTime.TabIndex = 4;
      this.lblTime.Text = "Time";
      this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // txtJobNum
      // 
      this.txtJobNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtJobNum.BackColor = System.Drawing.SystemColors.Window;
      this.txtJobNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtJobNum.Location = new System.Drawing.Point(217, 5);
      this.txtJobNum.Margin = new System.Windows.Forms.Padding(4);
      this.txtJobNum.Name = "txtJobNum";
      this.txtJobNum.Size = new System.Drawing.Size(206, 68);
      this.txtJobNum.TabIndex = 5;
      this.txtJobNum.KeyUp += new System.Windows.Forms.KeyEventHandler(this.input_KeyUp);
      // 
      // txtCutter
      // 
      this.txtCutter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtCutter.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtCutter.Location = new System.Drawing.Point(217, 83);
      this.txtCutter.Margin = new System.Windows.Forms.Padding(4);
      this.txtCutter.Name = "txtCutter";
      this.txtCutter.Size = new System.Drawing.Size(206, 68);
      this.txtCutter.TabIndex = 6;
      this.txtCutter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.input_KeyUp);
      // 
      // txtMachine
      // 
      this.txtMachine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtMachine.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtMachine.Location = new System.Drawing.Point(217, 161);
      this.txtMachine.Margin = new System.Windows.Forms.Padding(4);
      this.txtMachine.Name = "txtMachine";
      this.txtMachine.Size = new System.Drawing.Size(206, 68);
      this.txtMachine.TabIndex = 7;
      this.txtMachine.KeyUp += new System.Windows.Forms.KeyEventHandler(this.input_KeyUp);
      // 
      // txtStatus
      // 
      this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtStatus.Location = new System.Drawing.Point(217, 239);
      this.txtStatus.Margin = new System.Windows.Forms.Padding(4);
      this.txtStatus.Name = "txtStatus";
      this.txtStatus.Size = new System.Drawing.Size(206, 68);
      this.txtStatus.TabIndex = 16;
      this.txtStatus.KeyUp += new System.Windows.Forms.KeyEventHandler(this.input_KeyUp);
      // 
      // txtTime
      // 
      this.txtTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.tableLayoutPanel1.SetColumnSpan(this.txtTime, 2);
      this.txtTime.Enabled = false;
      this.txtTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtTime.Location = new System.Drawing.Point(217, 321);
      this.txtTime.Margin = new System.Windows.Forms.Padding(4);
      this.txtTime.Name = "txtTime";
      this.txtTime.Size = new System.Drawing.Size(421, 61);
      this.txtTime.TabIndex = 14;
      // 
      // lblCutterName
      // 
      this.lblCutterName.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.lblCutterName.AutoSize = true;
      this.lblCutterName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblCutterName.Location = new System.Drawing.Point(491, 94);
      this.lblCutterName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lblCutterName.Name = "lblCutterName";
      this.lblCutterName.Size = new System.Drawing.Size(87, 46);
      this.lblCutterName.TabIndex = 12;
      this.lblCutterName.Text = "N/A";
      // 
      // lblMachineName
      // 
      this.lblMachineName.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.lblMachineName.AutoSize = true;
      this.lblMachineName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblMachineName.Location = new System.Drawing.Point(491, 172);
      this.lblMachineName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lblMachineName.Name = "lblMachineName";
      this.lblMachineName.Size = new System.Drawing.Size(87, 46);
      this.lblMachineName.TabIndex = 13;
      this.lblMachineName.Text = "N/A";
      // 
      // lblStatusName
      // 
      this.lblStatusName.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.lblStatusName.AutoSize = true;
      this.lblStatusName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblStatusName.Location = new System.Drawing.Point(491, 250);
      this.lblStatusName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lblStatusName.Name = "lblStatusName";
      this.lblStatusName.Size = new System.Drawing.Size(87, 46);
      this.lblStatusName.TabIndex = 17;
      this.lblStatusName.Text = "N/A";
      // 
      // tableLayoutPanel2
      // 
      this.tableLayoutPanel2.ColumnCount = 1;
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel2.Controls.Add(this.dbRecentlyCut, 0, 2);
      this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
      this.tableLayoutPanel2.Controls.Add(this.menuStrip1, 0, 0);
      this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 3;
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
      this.tableLayoutPanel2.Size = new System.Drawing.Size(1284, 702);
      this.tableLayoutPanel2.TabIndex = 3;
      // 
      // tableLayoutPanel3
      // 
      this.tableLayoutPanel3.ColumnCount = 2;
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 0);
      this.tableLayoutPanel3.Controls.Add(this.dbNotCut, 1, 0);
      this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 50);
      this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
      this.tableLayoutPanel3.Name = "tableLayoutPanel3";
      this.tableLayoutPanel3.RowCount = 1;
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel3.Size = new System.Drawing.Size(1284, 391);
      this.tableLayoutPanel3.TabIndex = 0;
      // 
      // frmCutter
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1284, 702);
      this.Controls.Add(this.tableLayoutPanel2);
      this.MainMenuStrip = this.menuStrip1;
      this.Margin = new System.Windows.Forms.Padding(4);
      this.Name = "frmCutter";
      this.Text = "Cutter";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.Load += new System.EventHandler(this.frmCutter_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dbRecentlyCut)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dbNotCut)).EndInit();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      this.tableLayoutPanel3.ResumeLayout(false);
      this.ResumeLayout(false);

    }
  }
}
