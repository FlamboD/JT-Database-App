using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace JT_Database_App.Models
{
  internal class Placer
  {
    public static ObservableCollection<Placer> ToBePlaced = new ObservableCollection<Placer>();
    public static ObservableCollection<Placer> RecentlyPlaced = new ObservableCollection<Placer>();
    public int Id { get; set; }
    public Rep Rep { get; set; }
    public DateTime Time { get; set; }
    public SalesCounter Sales { get; set; }
  }
}
