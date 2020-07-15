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
        private int _curHp;
        private int _maxHp;
        private int _curHpPercent;
        private int _curXp;
        private string _category;
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

                switch (Growth)
                {
                    case "fluctuating":
                        MAX_XP = 1640000;
                        break;

                    case "slow":
                        MAX_XP = 1250000;
                        break;

                    case "medium slow":
                        MAX_XP = 1059860;
                        break;

                    case "medium fast":
                        MAX_XP = 1000000;
                        break;

                    case "fast":
                        MAX_XP = 800000;
                        break;

                    case "erratic":
                        MAX_XP = 600000;
                        break;

                    default:
                        break;
                }
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

        public int CurHp
        {
            get => _curHp;
            set
            {
                _curHp = value;
                OnPropertyChanged(nameof(CurHp));
            }
        }

        public int MaxHp
        {
            get => _maxHp;
            set
            {
                _maxHp = value;
                OnPropertyChanged(nameof(MaxHp));
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

        public int CurXp
        {
            get => _curXp;
            set
            {
                _curXp = value;
                OnPropertyChanged(nameof(CurXp));
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

        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        public int MAX_XP { get; private set; }

        public Pokemon Clone()
        {
            return new Pokemon()
            {
                Id = this.Id,
                Name = this.Name,
                Type = this.Type,
                Base = this.Base,
                XP = this.XP,
                CatchRate = this.CatchRate,
                Growth = this.Growth,
                EvolutionLevel = this.EvolutionLevel,
                BaseLevel = this.BaseLevel,
                EvolutionId = this.EvolutionId,
                Image = this.Image,
                FindType = this.FindType,
                CurLevel = this.CurLevel,
                CurHp = this.CurHp,
                MaxHp = this.MaxHp,
                CurHpPercent = this.CurHpPercent,
                CurXp = this.CurXp,
                Moves = this.Moves,
                Category = this.Category
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
