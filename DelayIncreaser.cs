using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace JT_Database_App
{
  class DelayIncreaser
  {
    private int increaseAmount;
    private TimeSpan delay;
    private TimeSpan resetInterval;
    private DateTime lastReset;

    public DelayIncreaser(
      int increaseSeconds,
      int initialSeconds,
      TimeSpan resetInterval
      )
    {
      this.increaseAmount = increaseSeconds;
      this.delay = TimeSpan.FromSeconds(initialSeconds);
      this.resetInterval = resetInterval;
      lastReset = DateTime.Now;
    }

    private void checkReset()
    {
      if (DateTime.Now > lastReset + resetInterval)
      {
        lastReset = DateTime.Now;
        delay = TimeSpan.Zero;
      }
    }

    public void Delay()
    {
      Thread.Sleep(delay);
      delay.Add(TimeSpan.FromSeconds(increaseAmount));
      checkReset();
    }
  }
}
