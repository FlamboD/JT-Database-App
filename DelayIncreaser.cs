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
    private int delay;
    private TimeSpan resetInterval;
    private DateTime lastReset;

    public DelayIncreaser(
      int increaseSeconds,
      int initialSeconds,
      TimeSpan resetInterval
      )
    {
      this.increaseAmount = increaseSeconds;
      this.delay = initialSeconds;
      this.resetInterval = resetInterval;
      lastReset = DateTime.Now;
    }

    private void checkReset()
    {
      if (DateTime.Now > lastReset + resetInterval)
      {
        lastReset = DateTime.Now;
        delay = 0;
      }
    }

    public void Delay()
    {
      Thread.Sleep(delay);
      delay += increaseAmount;
      checkReset();
    }
  }
}
