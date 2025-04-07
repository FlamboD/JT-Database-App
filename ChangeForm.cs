// Decompiled with JetBrains decompiler
// Type: JT_Database_App.ChangeForm
// Assembly: JT Database App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D649E70A-0AB7-4AF5-A104-CFA0EBCF124B
// Assembly location: C:\Users\Skye\Desktop\JT Database App 23aug21.exe

using JT_Database_App.Properties;
using System.Diagnostics;
using System.Windows.Forms;

namespace JT_Database_App
{
  internal class ChangeForm
  {
    public static bool Sales(Form current) => (bool) Settings.Default["frmSales"];

    public static bool Placed(Form current)
    {
      if (!(bool) Settings.Default["frmPlaced"])
        return false;
      ChangeForm.OpenForm(current, (Form) new frmPlaced());
      return true;
    }

    public static bool Checker(Form current)
    {
      if (!(bool) Settings.Default["frmChecker"])
        return false;
      ChangeForm.OpenForm(current, (Form) new frmChecker());
      return true;
    }

    public static bool Collected(Form current) => (bool) Settings.Default["frmCollected"];

    public static bool Filed(Form current)
    {
      if (!(bool) Settings.Default["frmFiled"])
        return false;
      ChangeForm.OpenForm(current, (Form) new frmFiler());
      return true;
    }

    private static void OpenForm(Form current, Form form)
    {
      PublicMethods.SetDefaultForm(form);
      form.FormClosed += (FormClosedEventHandler) ((s, args) => current.Close());
      form.Show();
      current.Hide();
    }

    public static bool Name(Form current, string frmName)
    {
      switch (frmName)
      {
        case "frmChecker":
          return ChangeForm.Checker(current);
        case "frmCollected":
          return ChangeForm.Collected(current);
        case "frmFiled":
          return ChangeForm.Filed(current);
        case "frmPlaced":
          return ChangeForm.Placed(current);
        case "frmSales":
          return ChangeForm.Sales(current);
        default:
          return false;
      }
    }
  }
}
