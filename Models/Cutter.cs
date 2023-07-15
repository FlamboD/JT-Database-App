using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT_Database_App.Models
{
  internal class Cutter
  {
    public static ObservableCollection<Cutter> ToBeCut = new ObservableCollection<Cutter>();
    public static ObservableCollection<Cutter> RecentlyCut = new ObservableCollection<Cutter>();
    public int Id { get; set; }
    public Rep Rep { get; set; }
    public DateTime Time { get; set; }
    public SalesCounter Sales { get; set; }
  }
}
