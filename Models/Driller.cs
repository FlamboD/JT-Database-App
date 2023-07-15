using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT_Database_App.Models
{
  internal class Driller
  {
    public static ObservableCollection<Driller> ToBeDrilled = new ObservableCollection<Driller>();
    public static ObservableCollection<Driller> RecentlyDrilled = new ObservableCollection<Driller>();
    public int Id { get; set; }
    public Rep Rep { get; set; }
    public DateTime Time { get; set; }
    public SalesCounter Sales { get; set; }
  }
}
