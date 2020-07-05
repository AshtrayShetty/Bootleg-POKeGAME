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
        private int[] _hp;
        private int[] _attack;
        private int[] _defense;
        private int[] _specialAttack;
        private int[] _specialDefense;
        private int[] _speed;
        public int[] HP
        {
            get => _hp;
            set
            {
                _hp = value;
                OnPropertyChanged(nameof(HP));
            }
        }
        public int[] Attack
        {
            get => _attack;
            set
            {
                _attack = value;
                OnPropertyChanged(nameof(Attack));
            }
        }
        public int[] Defense
        {
            get => _defense;
            set
            {
                _defense = value;
                OnPropertyChanged(nameof(Defense));
            }
        }
        public int[] SpecialAttack
        {
            get => _specialAttack;
            set
            {
                _specialAttack = value;
                OnPropertyChanged(nameof(SpecialAttack));
            }
        }
        public int[] SpecialDefense
        {
            get => _specialDefense;
            set
            {
                _specialDefense = value;
                OnPropertyChanged(nameof(SpecialDefense));
            }
        }
        public int[] Speed
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
