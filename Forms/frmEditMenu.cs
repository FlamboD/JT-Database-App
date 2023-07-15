// Decompiled with JetBrains decompiler
// Type: JT_Database_App.frmEditMenu
// Assembly: JT Database App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D649E70A-0AB7-4AF5-A104-CFA0EBCF124B
// Assembly location: C:\Users\Skye\Desktop\JT Database App 23aug21.exe

using JT_Database_App.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace JT_Database_App
{
  public class frmEditMenu : Form
  {
    public frmMenu _frmMenu;
    private IContainer components;
    private CheckedListBox cblForms;
    private Label label1;
    private Panel panel2;
    private Panel panel3;
    private TableLayoutPanel tableLayoutPanel1;
    private Button button1;
    private Button button2;

    public frmEditMenu()
    {
      this.InitializeComponent();
      bool flag = false;
      for (int index = 0; index < this.cblForms.Items.Count; ++index)
      {
        switch (index)
        {
          case 0:
            flag = (bool) Settings.Default["frmCashier"];
            break;
          case 1:
            flag = (bool) Settings.Default["frmSales"];
            break;
          case 2:
            flag = (bool) Settings.Default["frmPlaced"];
            break;
          case 3:
            flag = (bool) Settings.Default["frmCutter"];
            break;
          case 4:
            flag = (bool) Settings.Default["frmEdger"];
            break;
          case 5:
            flag = (bool) Settings.Default["frmDriller"];
            break;
          case 6:
            flag = (bool) Settings.Default["frmChecker"];
            break;
          case 7:
            flag = (bool) Settings.Default["frmCollected"];
            break;
          case 8:
            flag = (bool) Settings.Default["frmFiled"];
            break;
        }
        this.cblForms.SetItemChecked(index, flag);
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.cblForms.Items.Count; ++index)
      {
        bool itemChecked = this.cblForms.GetItemChecked(index);
        switch (index)
        {
          case 0:
            Settings.Default["frmCashier"] = (object) itemChecked;
            break;
          case 1:
            Settings.Default["frmSales"] = (object) itemChecked;
            break;
          case 2:
            Settings.Default["frmPlaced"] = (object) itemChecked;
            break;
          case 3:
            Settings.Default["frmCutter"] = (object) itemChecked;
            break;
          case 4:
            Settings.Default["frmEdger"] = (object) itemChecked;
            break;
          case 5:
            Settings.Default["frmDriller"] = (object) itemChecked;
            break;
          case 6:
            Settings.Default["frmChecker"] = (object) itemChecked;
            break;
          case 7:
            Settings.Default["frmCollected"] = (object) itemChecked;
            break;
          case 8:
            Settings.Default["frmFiled"] = (object) itemChecked;
            break;
        }
        this.cblForms.SetItemChecked(index, itemChecked);
      }
      Settings.Default.Save();
      this._frmMenu.ToggleButtons();
      this.Close();
    }

    private void button2_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cblForms = new CheckedListBox();
      this.label1 = new Label();
      this.panel2 = new Panel();
      this.panel3 = new Panel();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.button1 = new Button();
      this.button2 = new Button();
      this.panel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.cblForms.CheckOnClick = true;
      this.cblForms.Dock = DockStyle.Fill;
      this.cblForms.FormattingEnabled = true;
      this.cblForms.Items.AddRange(new object[9]
      {
        (object) "Cashier",
        (object) "Sales",
        (object) "Placed",
        (object) "Cutter",
        (object) "Edger",
        (object) "Driller",
        (object) "Checker",
        (object) "Collected",
        (object) "Filed"
      });
      this.cblForms.Location = new Point(0, 0);
      this.cblForms.Name = "cblForms";
      this.cblForms.Size = new Size(495, 316);
      this.cblForms.TabIndex = 0;
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(120, 17);
      this.label1.Name = "label1";
      this.label1.Size = new Size(254, 25);
      this.label1.TabIndex = 1;
      this.label1.Text = "Select buttons to activate";
      this.label1.TextAlign = ContentAlignment.MiddleCenter;
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Dock = DockStyle.Top;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(495, 58);
      this.panel2.TabIndex = 3;
      this.panel3.Controls.Add((Control) this.cblForms);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(0, 58);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(495, 316);
      this.panel3.TabIndex = 4;
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tableLayoutPanel1.Controls.Add((Control) this.button1, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.button2, 1, 0);
      this.tableLayoutPanel1.Dock = DockStyle.Bottom;
      this.tableLayoutPanel1.Location = new Point(0, 374);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
      this.tableLayoutPanel1.Size = new Size(495, 72);
      this.tableLayoutPanel1.TabIndex = 1;
      this.button1.Dock = DockStyle.Fill;
      this.button1.Location = new Point(3, 3);
      this.button1.Name = "button1";
      this.button1.Size = new Size(241, 66);
      this.button1.TabIndex = 0;
      this.button1.Text = "Save";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.button2.Dock = DockStyle.Fill;
      this.button2.Location = new Point(250, 3);
      this.button2.Name = "button2";
      this.button2.Size = new Size(242, 66);
      this.button2.TabIndex = 1;
      this.button2.Text = "Cancel";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.AutoScaleDimensions = new SizeF(12f, 25f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(495, 446);
      this.Controls.Add((Control) this.panel3);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.Name = "frmEditMenu";
      this.Text = "Edit Menu";
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
