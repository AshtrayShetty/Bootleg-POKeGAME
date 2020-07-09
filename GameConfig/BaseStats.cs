using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameConfig
{
    public class BaseStats : INotifyPropertyChanged 
    {
        private List<int> _hp;
        private List<int> _attack;
        private List<int> _defense;
        private List<int> _specialAttack;
        private List<int> _specialDefense;
        private List<int> _speed;

        public List<int> HP
        {
            get => _hp;
            set
            {
                _hp = value;
                OnPropertyChanged(nameof(HP));
            }
        }

        public List<int> Attack
        {
            get => _attack;
            set
            {
                _attack = value;
                OnPropertyChanged(nameof(Attack));
            }
        }

        public List<int> Defense
        {
            get => _defense;
            set
            {
                _defense = value;
                OnPropertyChanged(nameof(Defense));
            }
        }

        public List<int> SpecialAttack
        {
            get => _specialAttack;
            set
            {
                _specialAttack = value;
                OnPropertyChanged(nameof(SpecialAttack));
            }
        }

        public List<int> SpecialDefense
        {
            get => _specialDefense;
            set
            {
                _specialDefense = value;
                OnPropertyChanged(nameof(SpecialDefense));
            }
        }

        public List<int> Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                OnPropertyChanged(nameof(Speed));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
