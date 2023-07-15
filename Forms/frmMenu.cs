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
    private Button btnCashier;
    private Button btnCutter;
    private Button btnPlaced;
    private Button btnSales;
    private Button btnEdger;
    private Button btnDriller;
    private Button btnCollected;
    private Button btnChecker;
    private Button btnFiled;
    private TableLayoutPanel tableLayoutPanel1;

    public frmMenu()
    {
      this.InitializeComponent();
      this.ToggleButtons();
    }

    private void btnCashier_Click(object sender, EventArgs e) => ChangeForm.Cashier((Form) this);

    private void btnSales_Click(object sender, EventArgs e) => ChangeForm.Sales((Form) this);

    private void btnPlaced_Click(object sender, EventArgs e) => ChangeForm.Placed((Form) this);

    private void btnCutter_Click(object sender, EventArgs e) => ChangeForm.Cutter((Form) this);

    private void btnEdger_Click(object sender, EventArgs e) => ChangeForm.Edger((Form) this);

    private void btnDriller_Click(object sender, EventArgs e) => ChangeForm.Driller((Form) this);

    private void btnChecker_Click(object sender, EventArgs e) => ChangeForm.Checker((Form) this);

    private void btnCollected_Click(object sender, EventArgs e) => ChangeForm.Collected((Form) this);

    private void btnFiled_Click(object sender, EventArgs e) => ChangeForm.Filed((Form) this);

    private void exitToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

    private void connectToToolStripMenuItem_Click(object sender, EventArgs e) => PublicMethods.SetBoPath();

    public void ToggleButtons()
    {
      this.btnCashier.Enabled = (bool) Settings.Default["frmCashier"];
      this.btnSales.Enabled = (bool) Settings.Default["frmSales"];
      this.btnPlaced.Enabled = (bool) Settings.Default["frmPlaced"];
      this.btnCutter.Enabled = (bool) Settings.Default["frmCutter"];
      this.btnEdger.Enabled = (bool) Settings.Default["frmEdger"];
      this.btnDriller.Enabled = (bool) Settings.Default["frmDriller"];
      this.btnChecker.Enabled = (bool) Settings.Default["frmChecker"];
      this.btnCollected.Enabled = (bool) Settings.Default["frmCollected"];
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
      this.btnCashier = new System.Windows.Forms.Button();
      this.btnCutter = new System.Windows.Forms.Button();
      this.btnPlaced = new System.Windows.Forms.Button();
      this.btnSales = new System.Windows.Forms.Button();
      this.btnEdger = new System.Windows.Forms.Button();
      this.btnDriller = new System.Windows.Forms.Button();
      this.btnCollected = new System.Windows.Forms.Button();
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
      // btnCashier
      // 
      this.btnCashier.Dock = System.Windows.Forms.DockStyle.Fill;
      this.btnCashier.Location = new System.Drawing.Point(0, 0);
      this.btnCashier.Margin = new System.Windows.Forms.Padding(0);
      this.btnCashier.Name = "btnCashier";
      this.btnCashier.Size = new System.Drawing.Size(1774, 152);
      this.btnCashier.TabIndex = 1;
      this.btnCashier.Text = "Cashier";
      this.btnCashier.UseVisualStyleBackColor = true;
      this.btnCashier.Click += new System.EventHandler(this.btnCashier_Click);
      // 
      // btnCutter
      // 
      this.btnCutter.Dock = System.Windows.Forms.DockStyle.Fill;
      this.btnCutter.Location = new System.Drawing.Point(0, 456);
      this.btnCutter.Margin = new System.Windows.Forms.Padding(0);
      this.btnCutter.Name = "btnCutter";
      this.btnCutter.Size = new System.Drawing.Size(1774, 152);
      this.btnCutter.TabIndex = 2;
      this.btnCutter.Text = "Cutter";
      this.btnCutter.UseVisualStyleBackColor = true;
      this.btnCutter.Click += new System.EventHandler(this.btnCutter_Click);
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
      // btnSales
      // 
      this.btnSales.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnSales.Location = new System.Drawing.Point(0, 152);
      this.btnSales.Margin = new System.Windows.Forms.Padding(0);
      this.btnSales.Name = "btnSales";
      this.btnSales.Size = new System.Drawing.Size(1774, 150);
      this.btnSales.TabIndex = 4;
      this.btnSales.Text = "Sales";
      this.btnSales.UseVisualStyleBackColor = true;
      this.btnSales.Click += new System.EventHandler(this.btnSales_Click);
      // 
      // btnEdger
      // 
      this.btnEdger.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnEdger.Location = new System.Drawing.Point(0, 608);
      this.btnEdger.Margin = new System.Windows.Forms.Padding(0);
      this.btnEdger.Name = "btnEdger";
      this.btnEdger.Size = new System.Drawing.Size(1774, 150);
      this.btnEdger.TabIndex = 5;
      this.btnEdger.Text = "Edger";
      this.btnEdger.UseVisualStyleBackColor = true;
      this.btnEdger.Click += new System.EventHandler(this.btnEdger_Click);
      // 
      // btnDriller
      // 
      this.btnDriller.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnDriller.Location = new System.Drawing.Point(0, 760);
      this.btnDriller.Margin = new System.Windows.Forms.Padding(0);
      this.btnDriller.Name = "btnDriller";
      this.btnDriller.Size = new System.Drawing.Size(1774, 150);
      this.btnDriller.TabIndex = 6;
      this.btnDriller.Text = "Driller";
      this.btnDriller.UseVisualStyleBackColor = true;
      this.btnDriller.Click += new System.EventHandler(this.btnDriller_Click);
      // 
      // btnCollected
      // 
      this.btnCollected.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnCollected.Location = new System.Drawing.Point(0, 1064);
      this.btnCollected.Margin = new System.Windows.Forms.Padding(0);
      this.btnCollected.Name = "btnCollected";
      this.btnCollected.Size = new System.Drawing.Size(1774, 150);
      this.btnCollected.TabIndex = 7;
      this.btnCollected.Text = "Collected";
      this.btnCollected.UseVisualStyleBackColor = true;
      this.btnCollected.Click += new System.EventHandler(this.btnCollected_Click);
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
      this.tableLayoutPanel1.Controls.Add(this.btnFiled, 0, 4);
      this.tableLayoutPanel1.Controls.Add(this.btnChecker, 0, 3);
      this.tableLayoutPanel1.Controls.Add(this.btnCollected, 0, 3);
      this.tableLayoutPanel1.Controls.Add(this.btnDriller, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.btnEdger, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.btnSales, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.btnPlaced, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.btnCutter, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.btnCashier, 0, 0);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 40);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 5;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.25113F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.25113F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.25113F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.25113F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.25113F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.25113F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.25113F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.25113F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.990999F));
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
