using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT_Database_App.Models
{
  internal class SalesCounter
  {
    public int? Id { get; set; }
    public string Invoice { get; set; }
    public DateTime Time { get; set; } = DateTime.Now;
    public bool Urgent { get; set; } = false;
    public bool Cut { get; set; } = false;
    public bool Edge { get; set; } = false;
    public bool Drill { get; set; } = false;
    public bool Timber { get; set; } = false;
    public bool Other { get; set; } = false;
    public bool Delivery { get; set; } = false;
    public bool Cancelled { get; set; } = false;
    public bool OnHold { get; set; } = false;
    public Rep Rep { get; set; }

    public virtual Placer Placer { get; set; }
    public virtual Cutter Cutter { get; set; }
    public virtual Driller Driller { get; set; }
    public virtual Edger Edger { get; set; }
    public virtual Checker Checker { get; set; }
  }
}
