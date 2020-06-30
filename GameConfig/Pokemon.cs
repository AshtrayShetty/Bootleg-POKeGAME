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
        private string _growth;
        private int _evolutionLevel;
        private int _baseLevel;
        private int[] _evolutionId;
        private string _image;
        private string _findType;
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
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
