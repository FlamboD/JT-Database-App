using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT_Database_App.Models
{
  internal class Checker
  {
    public static ObservableCollection<Checker> ToBeChecked = new ObservableCollection<Checker>();
    public static ObservableCollection<Checker> RecentlyChecked = new ObservableCollection<Checker>();
    public int Id { get; set; }
    public Rep Rep { get; set; }
    public DateTime Time { get; set; }
    public SalesCounter Sales { get; set; }
  }
}
