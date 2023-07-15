namespace JT_Database_App.Forms
{
  partial class frmSales
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
      this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
      this.dgvItems = new System.Windows.Forms.DataGridView();
      this.layoutJobInfo = new System.Windows.Forms.TableLayoutPanel();
      this.lblInv = new System.Windows.Forms.Label();
      this.cbxInv = new System.Windows.Forms.ComboBox();
      this.lblRep = new System.Windows.Forms.Label();
      this.txtRep = new System.Windows.Forms.TextBox();
      this.layoutCustomerInfo = new System.Windows.Forms.TableLayoutPanel();
      this.lblCusName = new System.Windows.Forms.Label();
      this.txtCusName = new System.Windows.Forms.TextBox();
      this.lblCusCell = new System.Windows.Forms.Label();
      this.txtCusCell = new System.Windows.Forms.TextBox();
      this.layoutMain.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
      this.layoutJobInfo.SuspendLayout();
      this.layoutCustomerInfo.SuspendLayout();
      this.SuspendLayout();
      // 
      // layoutMain
      // 
      this.layoutMain.ColumnCount = 2;
      this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.layoutMain.Controls.Add(this.dgvItems, 1, 0);
      this.layoutMain.Controls.Add(this.layoutJobInfo, 0, 0);
      this.layoutMain.Controls.Add(this.layoutCustomerInfo, 0, 1);
      this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.layoutMain.Location = new System.Drawing.Point(0, 48);
      this.layoutMain.Name = "layoutMain";
      this.layoutMain.RowCount = 3;
      this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.layoutMain.Size = new System.Drawing.Size(800, 402);
      this.layoutMain.TabIndex = 4;
      // 
      // dgvItems
      // 
      this.dgvItems.AllowUserToAddRows = false;
      this.dgvItems.AllowUserToDeleteRows = false;
      this.dgvItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dgvItems.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
      this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvItems.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dgvItems.Location = new System.Drawing.Point(403, 3);
      this.dgvItems.Name = "dgvItems";
      this.dgvItems.RowHeadersWidth = 82;
      this.layoutMain.SetRowSpan(this.dgvItems, 2);
      this.dgvItems.RowTemplate.Height = 33;
      this.dgvItems.Size = new System.Drawing.Size(394, 262);
      this.dgvItems.TabIndex = 0;
      // 
      // layoutJobInfo
      // 
      this.layoutJobInfo.ColumnCount = 2;
      this.layoutJobInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
      this.layoutJobInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
      this.layoutJobInfo.Controls.Add(this.lblInv, 0, 0);
      this.layoutJobInfo.Controls.Add(this.cbxInv, 1, 0);
      this.layoutJobInfo.Controls.Add(this.lblRep, 0, 1);
      this.layoutJobInfo.Controls.Add(this.txtRep, 1, 1);
      this.layoutJobInfo.Dock = System.Windows.Forms.DockStyle.Fill;
      this.layoutJobInfo.Location = new System.Drawing.Point(3, 3);
      this.layoutJobInfo.Name = "layoutJobInfo";
      this.layoutJobInfo.RowCount = 2;
      this.layoutJobInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.layoutJobInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.layoutJobInfo.Size = new System.Drawing.Size(394, 128);
      this.layoutJobInfo.TabIndex = 1;
      // 
      // lblInv
      // 
      this.lblInv.AutoSize = true;
      this.lblInv.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblInv.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblInv.Location = new System.Drawing.Point(4, 0);
      this.lblInv.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lblInv.Name = "lblInv";
      this.lblInv.Size = new System.Drawing.Size(110, 65);
      this.lblInv.TabIndex = 4;
      this.lblInv.Text = "Inv";
      this.lblInv.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // cbxInv
      // 
      this.cbxInv.AllowDrop = true;
      this.cbxInv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.cbxInv.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cbxInv.FormattingEnabled = true;
      this.cbxInv.Location = new System.Drawing.Point(121, 3);
      this.cbxInv.Name = "cbxInv";
      this.cbxInv.Size = new System.Drawing.Size(270, 69);
      this.cbxInv.TabIndex = 8;
      // 
      // lblRep
      // 
      this.lblRep.AutoSize = true;
      this.lblRep.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblRep.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblRep.Location = new System.Drawing.Point(4, 65);
      this.lblRep.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lblRep.Name = "lblRep";
      this.lblRep.Size = new System.Drawing.Size(110, 66);
      this.lblRep.TabIndex = 3;
      this.lblRep.Text = "Rep";
      this.lblRep.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // txtRep
      // 
      this.txtRep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtRep.BackColor = System.Drawing.SystemColors.Window;
      this.txtRep.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtRep.Location = new System.Drawing.Point(122, 69);
      this.txtRep.Margin = new System.Windows.Forms.Padding(4);
      this.txtRep.Name = "txtRep";
      this.txtRep.Size = new System.Drawing.Size(268, 68);
      this.txtRep.TabIndex = 7;
      // 
      // layoutCustomerInfo
      // 
      this.layoutCustomerInfo.ColumnCount = 2;
      this.layoutCustomerInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
      this.layoutCustomerInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
      this.layoutCustomerInfo.Controls.Add(this.lblCusName, 0, 0);
      this.layoutCustomerInfo.Controls.Add(this.txtCusName, 1, 0);
      this.layoutCustomerInfo.Controls.Add(this.lblCusCell, 0, 1);
      this.layoutCustomerInfo.Controls.Add(this.txtCusCell, 1, 1);
      this.layoutCustomerInfo.Dock = System.Windows.Forms.DockStyle.Fill;
      this.layoutCustomerInfo.Location = new System.Drawing.Point(3, 137);
      this.layoutCustomerInfo.Name = "layoutCustomerInfo";
      this.layoutCustomerInfo.RowCount = 2;
      this.layoutCustomerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.layoutCustomerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.layoutCustomerInfo.Size = new System.Drawing.Size(394, 128);
      this.layoutCustomerInfo.TabIndex = 2;
      // 
      // lblCusName
      // 
      this.lblCusName.AutoSize = true;
      this.lblCusName.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblCusName.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblCusName.Location = new System.Drawing.Point(3, 0);
      this.lblCusName.Name = "lblCusName";
      this.lblCusName.Size = new System.Drawing.Size(151, 64);
      this.lblCusName.TabIndex = 0;
      this.lblCusName.Text = "Customer Name";
      this.lblCusName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // txtCusName
      // 
      this.txtCusName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtCusName.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtCusName.Location = new System.Drawing.Point(160, 3);
      this.txtCusName.Name = "txtCusName";
      this.txtCusName.Size = new System.Drawing.Size(231, 68);
      this.txtCusName.TabIndex = 1;
      // 
      // lblCusCell
      // 
      this.lblCusCell.AutoSize = true;
      this.lblCusCell.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblCusCell.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblCusCell.Location = new System.Drawing.Point(3, 64);
      this.lblCusCell.Name = "lblCusCell";
      this.lblCusCell.Size = new System.Drawing.Size(151, 64);
      this.lblCusCell.TabIndex = 2;
      this.lblCusCell.Text = "Customer Cell";
      this.lblCusCell.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // txtCusCell
      // 
      this.txtCusCell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtCusCell.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtCusCell.Location = new System.Drawing.Point(160, 67);
      this.txtCusCell.Name = "txtCusCell";
      this.txtCusCell.Size = new System.Drawing.Size(231, 68);
      this.txtCusCell.TabIndex = 3;
      // 
      // frmSales
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.layoutMain);
      this.Name = "frmSales";
      this.Text = "frmSales";
      this.Controls.SetChildIndex(this.layoutMain, 0);
      this.layoutMain.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
      this.layoutJobInfo.ResumeLayout(false);
      this.layoutJobInfo.PerformLayout();
      this.layoutCustomerInfo.ResumeLayout(false);
      this.layoutCustomerInfo.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel layoutMain;
    private System.Windows.Forms.DataGridView dgvItems;
    private System.Windows.Forms.TableLayoutPanel layoutJobInfo;
    private System.Windows.Forms.Label lblInv;
    private System.Windows.Forms.Label lblRep;
    private System.Windows.Forms.TextBox txtRep;
    private System.Windows.Forms.ComboBox cbxInv;
    private System.Windows.Forms.TableLayoutPanel layoutCustomerInfo;
    private System.Windows.Forms.Label lblCusName;
    private System.Windows.Forms.TextBox txtCusName;
    private System.Windows.Forms.Label lblCusCell;
    private System.Windows.Forms.TextBox txtCusCell;
  }
}