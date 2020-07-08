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
        private int _losses;
        private double _winPercentage;
        private Pokemon _chosenPokemon;

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

        public int Losses
        {
            get => _losses;
            set
            {
                _losses = value;
                OnPropertyChanged(nameof(Losses));
            }
        }
        public double WinPercentage
        {
            get => _winPercentage;
            set
            {
                _winPercentage = value;
                OnPropertyChanged(nameof(WinPercentage));
            }
        }

        public Pokemon ChosenPokemon
        {
            get => _chosenPokemon;
            set
            {
                _chosenPokemon = value;
                OnPropertyChanged(nameof(ChosenPokemon));
            }
        }
        public ObservableCollection<Pokemon> PokemonCollection { get; set; } = new ObservableCollection<Pokemon>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
