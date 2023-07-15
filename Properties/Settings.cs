// Decompiled with JetBrains decompiler
// Type: JT_Database_App.Properties.Settings
// Assembly: JT Database App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D649E70A-0AB7-4AF5-A104-CFA0EBCF124B
// Assembly location: C:\Users\Skye\Desktop\JT Database App 23aug21.exe

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace JT_Database_App.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.8.1.0")]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default => Settings.defaultInstance;

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    [DefaultSettingValue("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\Aneesa\\Documents\\JtJobSystem\\JtJobsBoffice.accdb")]
    public string JtJobsBofficeConnectionString => (string) this[nameof (JtJobsBofficeConnectionString)];

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("")]
    public string BOPath
    {
      get => (string) this[nameof (BOPath)];
      set => this[nameof (BOPath)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("")]
    public string IQPath
    {
      get => (string)this[nameof(IQPath)];
      set => this[nameof(IQPath)] = (object)value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("")]
    public string DefaultForm
    {
      get => (string) this[nameof (DefaultForm)];
      set => this[nameof (DefaultForm)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("True")]
    public bool frmCashier
    {
      get => (bool) this[nameof (frmCashier)];
      set => this[nameof (frmCashier)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    public bool frmSales
    {
      get => (bool) this[nameof (frmSales)];
      set => this[nameof (frmSales)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("True")]
    public bool frmPlaced
    {
      get => (bool) this[nameof (frmPlaced)];
      set => this[nameof (frmPlaced)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("True")]
    public bool frmCutter
    {
      get => (bool) this[nameof (frmCutter)];
      set => this[nameof (frmCutter)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("True")]
    public bool frmEdger
    {
      get => (bool) this[nameof (frmEdger)];
      set => this[nameof (frmEdger)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("True")]
    public bool frmDriller
    {
      get => (bool) this[nameof (frmDriller)];
      set => this[nameof (frmDriller)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("True")]
    public bool frmChecker
    {
      get => (bool) this[nameof (frmChecker)];
      set => this[nameof (frmChecker)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    public bool frmCollected
    {
      get => (bool) this[nameof (frmCollected)];
      set => this[nameof (frmCollected)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("True")]
    public bool frmFiled
    {
      get => (bool) this[nameof (frmFiled)];
      set => this[nameof (frmFiled)] = (object) value;
    }
  }
}
