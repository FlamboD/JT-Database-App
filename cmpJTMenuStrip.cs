using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JT_Database_App
{
  internal class cmpJTMenuStrip : MenuStrip 
  {
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem databaseToolStripMenuItem;
    private ToolStripMenuItem menuToolStripMenuItem;
    private ToolStripMenuItem manualInputToolStripMenuItem;
    private ToolStripMenuItem exitToolStripMenuItem;
    private ToolStripMenuItem connectToJtJobsBofficeToolStripMenuItem;
    private Form form;

    public Form Form { get => form; set => form = value; }

    private void Initialize()
    {
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.manualInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.databaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.connectToJtJobsBofficeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
    }

    public cmpJTMenuStrip(Form form) : base()
    {
      this.Form = form;;
      Initialize();
    }

    public cmpJTMenuStrip() : base()
    {
      Initialize();
    }

    private void menuToolStripMenuItem_Click(object sender, EventArgs e)
    {
      frmMenu frmMenu = new frmMenu();
      PublicMethods.SetDefaultForm();
      frmMenu.FormClosed += (FormClosedEventHandler)((s, args) => this.Form.Close());
      frmMenu.Show();
      this.Form.Hide();
    }

    private void editMenuToolStripMenuItem_Click(object sender, EventArgs e)
    {
      frmEditMenu frmEditMenu = new frmEditMenu();
      frmEditMenu._frmMenu = (frmMenu)this.Form;
      frmEditMenu.MdiParent = this.Form.MdiParent;
      int num = (int)frmEditMenu.ShowDialog();
    }

    private void fileToolStripMenuItem_Click(object sender, EventArgs e)
    {
      manualInputToolStripMenuItem.Checked = AppSettings.ManualInput;
    }

    private void manualInputToolStripMenuItem_CheckChanged(object sender, EventArgs e)
    {
      AppSettings.ManualInput = ((ToolStripMenuItem)sender).Checked;
    }
    private void exitToolStripMenuItem_Click(object sender, EventArgs e) => this.Form.Close();

    private void connectToJtJobsBofficeToolStripMenuItem_Click(object sender, EventArgs e) => PublicMethods.SetBoPath();

    public void InitializeComponents()
    {
      this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.databaseToolStripMenuItem});

      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem,
            this.manualInputToolStripMenuItem,
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(71, 44);
      this.fileToolStripMenuItem.Text = "File";
      this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
      // 
      // menuToolStripMenuItem
      // 
      this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
      if (this.Form is frmMenu)
      {
        this.menuToolStripMenuItem.Size = new System.Drawing.Size(257, 44);
        this.menuToolStripMenuItem.Text = "Edit Menu";
        this.menuToolStripMenuItem.Click += new System.EventHandler(this.editMenuToolStripMenuItem_Click);
      }
      else
      {
        this.menuToolStripMenuItem.Size = new System.Drawing.Size(210, 44);
        this.menuToolStripMenuItem.Text = "Menu";
        this.menuToolStripMenuItem.Click += new System.EventHandler(this.menuToolStripMenuItem_Click);
      }
      //
      // manualInputToolStripMenuItem
      //
      this.manualInputToolStripMenuItem.Name = "manualInput.ToolStripMenuItem";
      this.manualInputToolStripMenuItem.Size = new System.Drawing.Size(210, 44);
      this.manualInputToolStripMenuItem.Text = "Manual Input";
      this.manualInputToolStripMenuItem.CheckOnClick = true;
      this.manualInputToolStripMenuItem.CheckedChanged += new System.EventHandler(this.manualInputToolStripMenuItem_CheckChanged);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(210, 44);
      this.exitToolStripMenuItem.Text = "Exit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // databaseToolStripMenuItem
      // 
      this.databaseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToJtJobsBofficeToolStripMenuItem,
      });
      this.databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
      this.databaseToolStripMenuItem.Size = new System.Drawing.Size(132, 44);
      this.databaseToolStripMenuItem.Text = "Database";
      // 
      // connectToJtJobsBofficeToolStripMenuItem
      // 
      this.connectToJtJobsBofficeToolStripMenuItem.Name = "connectToJtJobsBofficeToolStripMenuItem";
      this.connectToJtJobsBofficeToolStripMenuItem.Size = new System.Drawing.Size(410, 44);
      this.connectToJtJobsBofficeToolStripMenuItem.Text = "Connect to JtJobsBoffice";
      this.connectToJtJobsBofficeToolStripMenuItem.Click += new System.EventHandler(this.connectToJtJobsBofficeToolStripMenuItem_Click);
    }
  }
}
