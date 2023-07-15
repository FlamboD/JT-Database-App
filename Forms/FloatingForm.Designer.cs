namespace JT_Database_App
{
  partial class FloatingForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.dgvItems = new System.Windows.Forms.DataGridView();
      this.txtCommands = new System.Windows.Forms.TextBox();
      ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
      this.SuspendLayout();
      // 
      // dgvItems
      // 
      this.dgvItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvItems.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dgvItems.Location = new System.Drawing.Point(0, 31);
      this.dgvItems.Margin = new System.Windows.Forms.Padding(0);
      this.dgvItems.MultiSelect = false;
      this.dgvItems.Name = "dgvItems";
      this.dgvItems.RowHeadersWidth = 82;
      this.dgvItems.Size = new System.Drawing.Size(800, 419);
      this.dgvItems.TabIndex = 0;
      // 
      // txtCommands
      // 
      this.txtCommands.Dock = System.Windows.Forms.DockStyle.Top;
      this.txtCommands.Location = new System.Drawing.Point(0, 0);
      this.txtCommands.Name = "txtCommands";
      this.txtCommands.Size = new System.Drawing.Size(800, 31);
      this.txtCommands.TabIndex = 1;
      // 
      // FloatingForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.dgvItems);
      this.Controls.Add(this.txtCommands);
      this.Name = "FloatingForm";
      this.Text = "FloatingForm";
      ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DataGridView dgvItems;
    private System.Windows.Forms.TextBox txtCommands;
  }
}