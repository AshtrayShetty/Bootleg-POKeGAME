using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameConfig
{
    public class GameSession:INotifyPropertyChanged
    {
        private Player _player = new Player("Ash Ketchum", 0, 0, 1);
        public Player CurrentPlayer
        {
            get => _player;
            set
            {
                _player = value;
                OnPropertyChanged(nameof(CurrentPlayer));
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
