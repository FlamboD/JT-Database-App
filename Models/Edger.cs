using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT_Database_App.Models
{
  internal class Edger
  {
    public static ObservableCollection<Edger> ToBeEdged = new ObservableCollection<Edger>();
    public static ObservableCollection<Edger> RecentlyEdged = new ObservableCollection<Edger>();
    public int Id { get; set; }
    public Rep Rep { get; set; }
    public DateTime Time { get; set; }
    public SalesCounter Sales { get; set; }
  }
}
