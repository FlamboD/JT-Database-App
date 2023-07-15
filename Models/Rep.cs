using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT_Database_App.Models
{
  internal class Rep
  {
    public static ObservableCollection<Rep> Reps { get; } = new ObservableCollection<Rep>();
    public int Id { get; set; }
    public int EmployeeNumber { get; set; }
    public string FirstName { get; set; }
    public int? Code { get; set; }
    public char Department { get; set; }
    public bool Active { get; set; } = true;
    public bool Admin { get; set; } = false;
    public bool Cashier { get; set; } = false;
    public bool Sales { get; set; } = false;
    public bool Workshop { get; set; } = false;
    public bool Delivery { get; set; } = false;
  }
}
