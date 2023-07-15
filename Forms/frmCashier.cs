// Decompiled with JetBrains decompiler
// Type: JT_Database_App.frmCashier
// Assembly: JT Database App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D649E70A-0AB7-4AF5-A104-CFA0EBCF124B
// Assembly location: C:\Users\Skye\Desktop\JT Database App 23aug21.exe

using JT_Database_App.Forms;
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
using System.Linq;
using System.Windows.Forms;

namespace JT_Database_App
{
  public class frmCashier : frmBase
  {
    private Timer TmrRefresh = new Timer();
    private string sInv, sCustomerName, sCustomerCell;
    private IContainer components;
    private TableLayoutPanel tableLayoutPanel1;
    private Label lblJobNum;
    private TextBox txtJobNum;
    private Label lblTime;
    private TextBox txtTime;
    private DataGridView dgvLastScanned;
    private Button btnSubmit;

    public frmCashier()
    {
      InitializeComponent();
      Load += (s, e) => fillLastScanned();
    }

    private async void fillLastScanned()
    {
      string cmdLastScanned = "" +
        "SELECT TOP 10\n" +
        " invsales, [sales date]\n" +
        " FROM sales_counter\n" +
        " ORDER BY [sales date] DESC;";
      DataTable dataTable = new DataTable();
      {
        try
        {
          using (OleDbConnection oleDbConnection = new OleDbConnection(this.connectionStr))
          {
            await oleDbConnection.OpenAsync();
            using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(cmdLastScanned, oleDbConnection))
            {
              oleDbDataAdapter.Fill(dataTable);
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
          this.dgvLastScanned.DataSource = dataTable;
        }));
      };
    }

    protected override void DoLoad(object sender, EventArgs e)
    {
      if (!Validation.GetOdbcDriverNames().Contains("DBISAM 4 ODBC Driver"))
      {
        MessageBox.Show("DBISAM 4 ODBC Driver not installed", "You can download the driver from https://www.elevatesoft.com/download?category=dbisam under the DBISAM-ODBC section");

        frmMenu frmMenu = new frmMenu();
        PublicMethods.SetDefaultForm();
        frmMenu.FormClosed += (FormClosedEventHandler)((s, args) => this.Close());
        frmMenu.Show();
        this.Hide();
      }

      this.txtJobNum.Focus();
      this.TmrRefresh.Interval = 500;
      this.TmrRefresh.Tick += (EventHandler) ((s, a) => this.refresh());
      this.TmrRefresh.Start();
      this.refresh();
    }

    private void refresh()
    {
      if (!this.Visible) return;
      this.txtTime.Text = DateTime.Now.ToString();
    }

    private void ResetForm()
    {
      this.sInv = (string) null;
      this.sCustomerName = (string)null;
      this.sCustomerCell = (string)null;
      Color white = Color.White;
      this.txtJobNum.Text = "";
      this.txtJobNum.BackColor = white;
      this.txtJobNum.Focus();
      this.fillLastScanned();
    }

    private void ValidateJobNumber()
    {
      if (this.txtJobNum.Text.StartsWith("frm"))
      {
        if (ChangeForm.Name((Form) this, this.txtJobNum.Text))
          return;
        AutoClosingMessageBox.Show(string.Format("Form {0} is not isn't available or doesn't exist!", (object) this.txtJobNum.Text), "Error", 3000);
        this.ResetForm();
      }
      else
      {
        this.txtJobNum.Text = this.txtJobNum.Text.ToUpper();
        if (new Validation().invCashier(this.txtJobNum.Text))
        {
          this.sInv = this.txtJobNum.Text;
          this.txtJobNum.BackColor = Color.Green;
          this.sCustomerName = new Validation().getCustomerName(sInv);
          this.sCustomerCell = new Validation().getCustomerCell(sInv);
          Debug.WriteLine(sCustomerName);
          Debug.WriteLine(sCustomerCell);
          AutoClosingMessageBox.Show(string.Format("Invoice {0} has been added!", (object)this.txtJobNum.Text), "SUCCESS", 3000);
          SubmitData();
        }
        else
        {
          this.sInv = (string) null;
          AutoClosingMessageBox.Show(string.Format("Invoice {0} is invalid!", (object) this.txtJobNum.Text), "Error", 3000);
          this.txtJobNum.Text = "";
        }
      }
    }

    private void SubmitData()
    {
      if (this.txtJobNum.Text.Equals(""))
      {
        AutoClosingMessageBox.Show("One of the form's fields are empty!", "Error", 3000);
        this.ResetForm();
      }
      else
      {
        string cmdText = "INSERT INTO sales_counter(id, rep, invsales, [sales date], [sale time], [customer name], [customer cell]) VALUES(@id, 18, @inv, @date, @time, @name, @cell);";
        using (OleDbConnection connection = new OleDbConnection(PublicMethods.BOConnectionString))
        {
          using (OleDbCommand oleDbCommand1 = new OleDbCommand(cmdText, connection))
          {
            using (OleDbCommand oleDbCommand2 = new OleDbCommand("SELECT MAX(id) FROM sales_counter", connection))
            {
              connection.Open();
              int num = (int) oleDbCommand2.ExecuteScalar() + 1;
              Debug.WriteLine(num);
              oleDbCommand1.Parameters.AddWithValue("@id", (object) num);
              oleDbCommand1.Parameters.AddWithValue("@inv", (object) this.sInv);
              oleDbCommand1.Parameters.AddWithValue("@date", (object) this.DTC(DateTime.Now));
              oleDbCommand1.Parameters.AddWithValue("@time", (object) this.DTC(DateTime.Now));
              oleDbCommand1.Parameters.AddWithValue("@name", (object) this.sCustomerName ?? DBNull.Value);
              oleDbCommand1.Parameters.AddWithValue("@cell", (object) this.sCustomerCell ?? DBNull.Value);
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

    private void btnSubmit_Click(object sender, EventArgs e)
    {
      ValidateJobNumber();
    }

    private void CustomInitialize()
    {
      // this.menuStrip1 = new cmpJTMenuStrip(this);
    }

    private void CustomSuspended()
    {
      // menuStrip1.InitializeComponents();
    }

    private void InitializeComponent()
    {
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.lblJobNum = new System.Windows.Forms.Label();
      this.lblTime = new System.Windows.Forms.Label();
      this.txtJobNum = new System.Windows.Forms.TextBox();
      this.txtTime = new System.Windows.Forms.TextBox();
      this.btnSubmit = new System.Windows.Forms.Button();
      this.dgvLastScanned = new System.Windows.Forms.DataGridView();
      this.tableLayoutPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgvLastScanned)).BeginInit();
      this.SuspendLayout();
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 3;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
      this.tableLayoutPanel1.Controls.Add(this.lblJobNum, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.lblTime, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.txtJobNum, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this.txtTime, 1, 1);
      this.tableLayoutPanel1.Controls.Add(this.btnSubmit, 2, 0);
      this.tableLayoutPanel1.Controls.Add(this.dgvLastScanned, 2, 1);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 38);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 2;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(1284, 664);
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
      this.lblJobNum.Size = new System.Drawing.Size(419, 332);
      this.lblJobNum.TabIndex = 0;
      this.lblJobNum.Text = "Job Num";
      this.lblJobNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // lblTime
      // 
      this.lblTime.AutoSize = true;
      this.lblTime.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTime.Location = new System.Drawing.Point(4, 332);
      this.lblTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lblTime.Name = "lblTime";
      this.lblTime.Size = new System.Drawing.Size(419, 332);
      this.lblTime.TabIndex = 4;
      this.lblTime.Text = "Time";
      this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // txtJobNum
      // 
      this.txtJobNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtJobNum.BackColor = System.Drawing.SystemColors.Window;
      this.txtJobNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtJobNum.Location = new System.Drawing.Point(431, 132);
      this.txtJobNum.Margin = new System.Windows.Forms.Padding(4);
      this.txtJobNum.Name = "txtJobNum";
      this.txtJobNum.Size = new System.Drawing.Size(420, 68);
      this.txtJobNum.TabIndex = 6;
      // 
      // txtTime
      // 
      this.txtTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.txtTime.Enabled = false;
      this.txtTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtTime.Location = new System.Drawing.Point(431, 467);
      this.txtTime.Margin = new System.Windows.Forms.Padding(4);
      this.txtTime.Name = "txtTime";
      this.txtTime.Size = new System.Drawing.Size(420, 61);
      this.txtTime.TabIndex = 14;
      // 
      // btnSubmit
      // 
      this.btnSubmit.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.btnSubmit.AutoSize = true;
      this.btnSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnSubmit.Location = new System.Drawing.Point(859, 46);
      this.btnSubmit.Margin = new System.Windows.Forms.Padding(4);
      this.btnSubmit.Name = "btnSubmit";
      this.btnSubmit.Padding = new System.Windows.Forms.Padding(50);
      this.btnSubmit.Size = new System.Drawing.Size(421, 240);
      this.btnSubmit.TabIndex = 15;
      this.btnSubmit.Text = "Submit";
      this.btnSubmit.UseVisualStyleBackColor = true;
      this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
      // 
      // dgvLastScanned
      // 
      this.dgvLastScanned.AllowUserToAddRows = false;
      this.dgvLastScanned.AllowUserToDeleteRows = false;
      this.dgvLastScanned.AllowUserToResizeColumns = false;
      this.dgvLastScanned.AllowUserToResizeRows = false;
      this.dgvLastScanned.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dgvLastScanned.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvLastScanned.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dgvLastScanned.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
      this.dgvLastScanned.Enabled = false;
      this.dgvLastScanned.Location = new System.Drawing.Point(858, 335);
      this.dgvLastScanned.MultiSelect = false;
      this.dgvLastScanned.Name = "dgvLastScanned";
      this.dgvLastScanned.ReadOnly = true;
      this.dgvLastScanned.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
      this.dgvLastScanned.RowTemplate.Height = 33;
      this.dgvLastScanned.Size = new System.Drawing.Size(423, 326);
      this.dgvLastScanned.TabIndex = 16;
      // 
      // frmCashier
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1284, 702);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Margin = new System.Windows.Forms.Padding(4);
      this.Name = "frmCashier";
      this.Text = "Cashier";
      this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgvLastScanned)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }
  }
}
