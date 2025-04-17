// Decompiled with JetBrains decompiler
// Type: JT_Database_App.PublicMethods
// Assembly: JT Database App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D649E70A-0AB7-4AF5-A104-CFA0EBCF124B
// Assembly location: C:\Users\Skye\Desktop\JT Database App 23aug21.exe

using JT_Database_App.Properties;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace JT_Database_App
{
  internal class PublicMethods
  {
    public static string BOConnectionString
    {
      get => "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Settings.Default.BOPath;
    }
    public static string IQConnectionString
    {
      get => "DRIVER={DBISAM 4 ODBC Driver};ConnectionType=Local;CatalogName=" + Settings.Default.IQPath;
    }
    public static void SetBoPath()
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Access DB(*.mdb; *.accdb)|*.mdb; *.accdb";
      if(Settings.Default.BOPath != null)
        openFileDialog.InitialDirectory = Path.GetDirectoryName(Settings.Default.BOPath);
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      Settings.Default.BOPath = openFileDialog.FileName;
      Settings.Default.Save();
    }
    public static void SetIQPath()
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (Settings.Default.IQPath != null)
        folderBrowserDialog.SelectedPath = Settings.Default.IQPath;
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      Settings.Default.IQPath = folderBrowserDialog.SelectedPath;
      Settings.Default.Save();
    }

    public static void SetDefaultForm(Form form = null)
    {
      Settings.Default.DefaultForm = form == null ? "" : form.Name;
      Settings.Default.Save();
    }
  }
}
