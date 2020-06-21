using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameConfig
{
    public class Player : INotifyPropertyChanged
    {
        private string _name;
        private int _money;
        private int _fights;
        private int _wins;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public int Money
        {
            get => _money;
            set
            {
                _money = value;
                OnPropertyChanged(nameof(Money));
            }
        }
        public int Fights
        {
            get => _fights;
            set
            {
                _fights = value;
                OnPropertyChanged(nameof(Fights));
            }
        }
        public int Wins
        {
            get => _wins;
            set
            {
                _wins = value;
                OnPropertyChanged(nameof(Wins));
            }
        }
        public ObservableCollection<object> Pokemon { get; set; }
        public Player(string name, int money, int fights, int wins)
        {
            Name = name;
            Money = money;
            Fights = fights;
            Wins = wins;
            Pokemon = new ObservableCollection<object>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
