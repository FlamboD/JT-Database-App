// Decompiled with JetBrains decompiler
// Type: JT_Database_App.frmMenu
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
  public class frmMenu : Form
  {
    private IContainer components;
    private cmpJTMenuStrip menuStrip1;
    private Button btnPlaced;
    private Button btnChecker;
    private Button btnFiled;
    private TableLayoutPanel tableLayoutPanel1;

    public frmMenu()
    {
      this.InitializeComponent();
      this.ToggleButtons();
    }

    private void btnPlaced_Click(object sender, EventArgs e) => ChangeForm.Placed((Form) this);

    private void btnChecker_Click(object sender, EventArgs e) => ChangeForm.Checker((Form) this);

    private void btnFiled_Click(object sender, EventArgs e) => ChangeForm.Filed((Form) this);

    private void exitToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

    private void connectToToolStripMenuItem_Click(object sender, EventArgs e) => PublicMethods.SetBoPath();

    public void ToggleButtons()
    {
      this.btnPlaced.Enabled = (bool) Settings.Default["frmPlaced"];
      this.btnChecker.Enabled = (bool) Settings.Default["frmChecker"];
      this.btnFiled.Enabled = (bool) Settings.Default["frmFiled"];
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.menuStrip1 = new cmpJTMenuStrip(this);
      this.btnPlaced = new System.Windows.Forms.Button();
      this.btnChecker = new System.Windows.Forms.Button();
      this.btnFiled = new System.Windows.Forms.Button();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.menuStrip1.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStrip1
      // 
      this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
      this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(1774, 40);
      this.menuStrip1.TabIndex = 0;
      this.menuStrip1.Text = "menuStrip1";
      this.menuStrip1.InitializeComponents();
      // 
      // btnPlaced
      // 
      this.btnPlaced.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnPlaced.Location = new System.Drawing.Point(0, 304);
      this.btnPlaced.Margin = new System.Windows.Forms.Padding(0);
      this.btnPlaced.Name = "btnPlaced";
      this.btnPlaced.Size = new System.Drawing.Size(1774, 150);
      this.btnPlaced.TabIndex = 3;
      this.btnPlaced.Text = "Placed";
      this.btnPlaced.UseVisualStyleBackColor = true;
      this.btnPlaced.Click += new System.EventHandler(this.btnPlaced_Click);
      // 
      // btnChecker
      // 
      this.btnChecker.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnChecker.Location = new System.Drawing.Point(0, 912);
      this.btnChecker.Margin = new System.Windows.Forms.Padding(0);
      this.btnChecker.Name = "btnChecker";
      this.btnChecker.Size = new System.Drawing.Size(1774, 150);
      this.btnChecker.TabIndex = 8;
      this.btnChecker.Text = "Checker";
      this.btnChecker.UseVisualStyleBackColor = true;
      this.btnChecker.Click += new System.EventHandler(this.btnChecker_Click);
      // 
      // btnFiled
      // 
      this.btnFiled.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnFiled.Location = new System.Drawing.Point(0, 1216);
      this.btnFiled.Margin = new System.Windows.Forms.Padding(0);
      this.btnFiled.Name = "btnFiled";
      this.btnFiled.Size = new System.Drawing.Size(1774, 135);
      this.btnFiled.TabIndex = 9;
      this.btnFiled.Text = "Filed";
      this.btnFiled.UseVisualStyleBackColor = true;
      this.btnFiled.Click += new System.EventHandler(this.btnFiled_Click);
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.btnFiled, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.btnChecker, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.btnPlaced, 0, 0);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 40);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 3;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(1774, 1359);
      this.tableLayoutPanel1.TabIndex = 19;
      // 
      // frmMenu
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1774, 1399);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Controls.Add(this.menuStrip1);
      this.Name = "frmMenu";
      this.Text = "Menu";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }
  }
}
