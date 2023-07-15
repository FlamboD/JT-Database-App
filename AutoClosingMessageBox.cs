// Decompiled with JetBrains decompiler
// Type: JT_Database_App.AutoClosingMessageBox
// Assembly: JT Database App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D649E70A-0AB7-4AF5-A104-CFA0EBCF124B
// Assembly location: C:\Users\Skye\Desktop\JT Database App 23aug21.exe

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace JT_Database_App
{
  public class AutoClosingMessageBox
  {
    private System.Threading.Timer _timeoutTimer;
    private string _caption;
    private const int WM_CLOSE = 16;

    private AutoClosingMessageBox(string text, string caption, int timeout)
    {
      this._caption = caption;
      this._timeoutTimer = new System.Threading.Timer(new TimerCallback(this.OnTimerElapsed), (object) null, timeout, -1);
      using (this._timeoutTimer)
      {
        int num = (int) MessageBox.Show(text, caption);
      }
    }

    public static void Show(string text, string caption = "ERROR", int timeout = 5000)
    {
      AutoClosingMessageBox closingMessageBox = new AutoClosingMessageBox(text, caption, timeout);
    }

    private void OnTimerElapsed(object state)
    {
      IntPtr window = AutoClosingMessageBox.FindWindow("#32770", this._caption);
      if (window != IntPtr.Zero)
        AutoClosingMessageBox.SendMessage(window, 16U, IntPtr.Zero, IntPtr.Zero);
      this._timeoutTimer.Dispose();
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(
      IntPtr hWnd,
      uint Msg,
      IntPtr wParam,
      IntPtr lParam);
  }
}
