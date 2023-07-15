using JT_Database_App.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JT_Database_App.Forms
{
  public partial class frmBase : Form
  {
    internal cmpJTMenuStrip menuStrip;
    internal string connectionStr;
    public frmBase()
    {
      this.connectionStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Settings.Default.BOPath;
      InitializeComponent();

      this.menuStrip.Form = this;
      this.menuStrip.InitializeComponents();
    }

    protected virtual void DoLoad(object sender, EventArgs e) { }
  }
}
