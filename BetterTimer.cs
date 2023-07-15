// Decompiled with JetBrains decompiler
// Type: JT_Database_App.BetterTimer
// Assembly: JT Database App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D649E70A-0AB7-4AF5-A104-CFA0EBCF124B
// Assembly location: C:\Users\Skye\Desktop\JT Database App 23aug21.exe

using System.ComponentModel;
using System.Windows.Forms;

namespace JT_Database_App
{
  public class BetterTimer : Timer
  {
    private bool _Enabled;

    public BetterTimer() => base.Enabled = true;

    public BetterTimer(IContainer container)
      : base(container)
    {
      base.Enabled = true;
    }

    public override bool Enabled
    {
      get => this._Enabled;
      set => this._Enabled = value;
    }
  }
}
