using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameConfig
{
    public class Move : INotifyPropertyChanged
    {
        public int Accuracy { get; set; }
        public string Category { get; set; }
        public string Ename { get; set; }
        public int Id { get; set; }
        public int Power { get; set; }
        public int PP { get; set; }
        public string Type { get; set; }
        public bool IsSelected { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
