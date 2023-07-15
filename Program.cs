// Decompiled with JetBrains decompiler
// Type: JT_Database_App.Program
// Assembly: JT Database App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D649E70A-0AB7-4AF5-A104-CFA0EBCF124B
// Assembly location: C:\Users\Skye\Desktop\JT Database App 23aug21.exe

using JT_Database_App.Forms;
using JT_Database_App.Properties;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace JT_Database_App
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form)new frmSales());
      return;
      switch (Settings.Default.DefaultForm)
      {
        case "frmCashier":
          Application.Run((Form)new frmCashier());
          break;
        case "frmChecker":
          Application.Run((Form) new frmChecker());
          break;
        case "frmCollected":
          break;
        case "frmCutter":
          Application.Run((Form) new frmCutter());
          break;
        case "frmDriller":
          Application.Run((Form) new frmDriller());
          break;
        case "frmEdger":
          Application.Run((Form) new frmEdger());
          break;
        case "frmFiled":
          Application.Run((Form) new frmFiler());
          break;
        case "frmPlaced":
          Application.Run((Form) new frmPlaced());
          break;
        case "frmSales":
          break;
        default:
          Application.Run((Form) new frmMenu());
          break;
      }
    }
  }
}
