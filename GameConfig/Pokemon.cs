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
        private int _id;
        private string _name;
        private string[] _type;
        private BaseStats _base;
        private int _xp;
        private int _catchRate;
        private string _growth;
        private int _evolutionLevel;
        private int _baseLevel;
        private int[] _evolutionId;
        private string _image;
        private string _findType;
        private int _curLevel;
        private int _curHpPercent;
        private List<Move> _moves;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string[] Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }
        public BaseStats Base
        {
            get => _base;
            set
            {
                _base = value;
                OnPropertyChanged(nameof(BaseStats));
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
        public int CatchRate
        {
            get => _catchRate;
            set
            {
                _catchRate = value;
                OnPropertyChanged(nameof(CatchRate));
            }
        }
        public string Growth
        {
            get => _growth;
            set
            {
                _growth = value;
                OnPropertyChanged(nameof(Growth));
            }
        }
        public int EvolutionLevel
        {
            get => _evolutionLevel;
            set
            {
                _evolutionLevel = value;
                OnPropertyChanged(nameof(EvolutionLevel));
            }
        }
        public int BaseLevel
        {
            get => _baseLevel;
            set
            {
                _baseLevel = value;
                OnPropertyChanged(nameof(BaseLevel));
            }
        }
        public int[] EvolutionId
        {
            get => _evolutionId;
            set
            {
                _evolutionId = value;
                OnPropertyChanged(nameof(EvolutionId));
            }
        }
        public string Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }
        public string FindType
        {
            get => _findType;
            set
            {
                _findType = value;
                OnPropertyChanged(nameof(FindType));
            }
        }
        public int CurLevel
        {
            get => _curLevel;
            set
            {
                _curLevel = value;
                OnPropertyChanged(nameof(CurLevel));
            }
        }
        public int CurHpPercent
        {
            get => _curHpPercent;
            set
            {
                _curHpPercent = value;
                OnPropertyChanged(nameof(CurHpPercent));
            }
        }
        public List<Move> Moves
        {
            get => _moves;
            set
            {
                _moves = value;
                OnPropertyChanged(nameof(Moves));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
