// Decompiled with JetBrains decompiler
// Type: JT_Database_App.frmCheckAdmin
// Assembly: JT Database App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D649E70A-0AB7-4AF5-A104-CFA0EBCF124B
// Assembly location: C:\Users\Skye\Desktop\JT Database App 23aug21.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace JT_Database_App
{
  public class frmCheckAdmin : Form
  {
    private IContainer components;
    private TextBox textBox1;
    private Button btnSubmit;
    private Label label1;
    private TableLayoutPanel tableLayoutPanel1;
    private Button btnCancel;
    private Panel panel1;

    public frmCheckAdmin() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.btnSubmit = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.btnCancel = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      this.tableLayoutPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // textBox1
      // 
      this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBox1.Location = new System.Drawing.Point(20, 15);
      this.textBox1.Margin = new System.Windows.Forms.Padding(5);
      this.textBox1.MaxLength = 10;
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(760, 49);
      this.textBox1.TabIndex = 0;
      // 
      // btnSubmit
      // 
      this.btnSubmit.Dock = System.Windows.Forms.DockStyle.Fill;
      this.btnSubmit.Location = new System.Drawing.Point(3, 3);
      this.btnSubmit.Name = "btnSubmit";
      this.btnSubmit.Size = new System.Drawing.Size(394, 94);
      this.btnSubmit.TabIndex = 1;
      this.btnSubmit.Text = "Submit";
      this.btnSubmit.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Dock = System.Windows.Forms.DockStyle.Top;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(0, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(321, 51);
      this.label1.TabIndex = 2;
      this.label1.Text = "Enter password";
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.Controls.Add(this.btnCancel, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this.btnSubmit, 0, 0);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 129);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 100);
      this.tableLayoutPanel1.TabIndex = 3;
      // 
      // btnCancel
      // 
      this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.btnCancel.Location = new System.Drawing.Point(403, 3);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(394, 94);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.textBox1);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(0, 51);
      this.panel1.Name = "panel1";
      this.panel1.Padding = new System.Windows.Forms.Padding(20, 15, 20, 20);
      this.panel1.Size = new System.Drawing.Size(800, 78);
      this.panel1.TabIndex = 4;
      // 
      // frmCheckAdmin
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 229);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Controls.Add(this.label1);
      this.Name = "frmCheckAdmin";
      this.Text = "Security";
      this.tableLayoutPanel1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }
  }
}
