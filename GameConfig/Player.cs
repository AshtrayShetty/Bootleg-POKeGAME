using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameConfig
{
    public class Player
    {
        public String Name { get; set; }
        public int Money { get; set; }
        public int Fight { get; set; }
        public int Wins { get; set; }
        public ObservableCollection<object> Pokemon { get; set; }

    }
}
