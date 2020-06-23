using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameConfig
{
    public class Pokemon : INotifyPropertyChanged
    {
        private string _name;
        private int _hp;
        private int _level;
        private int _attack;
        private int _defense;
        private int _xp;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Type { get; set; }
        public int BaseHp { get; set; }
        public int HP
        {
            get => _hp;
            set
            {
                _hp = value;
                OnPropertyChanged(nameof(HP));
            }
        }
        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                OnPropertyChanged(nameof(Level));
            }
        }
        public int BaseAttack { get; set; }
        public int Attack
        {
            get => _attack;
            set
            {
                _attack = value;
                OnPropertyChanged(nameof(Attack));
            }
        }
        public int BaseDefense { get; set; }
        public int Defense
        {
            get => _defense;
            set
            {
                _defense = value;
                OnPropertyChanged(nameof(Defense));
            }
        }
        public int XP
        {
            get => _xp;
            set
            {
                _xp = value;
                OnPropertyChanged(nameof(XP));
            }
        }
        public int BaseLevel { get; set; }
        public int EvolutionLevel { get; set; }
        public List<object> Moves { get; set; } = new List<object>();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
