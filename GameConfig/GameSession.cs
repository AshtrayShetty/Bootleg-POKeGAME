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
        private Player _player = new Player();
        private bool _isGameCreated;
        private int _losses;
        private double _winPercentage;
        public Player CurrentPlayer
        {
            get => _player;
            set
            {
                _player = value;
                OnPropertyChanged(nameof(CurrentPlayer));
            }
        }
        public bool IsGameCreated
        {
            get => _isGameCreated;
            set
            {
                _isGameCreated = value;
                OnPropertyChanged(nameof(IsGameCreated));
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
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<GameMessageEventArgs> Event;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void RaiseMessage(string message)
        {
            Event?.Invoke(this, new GameMessageEventArgs(message));
        }
    }
}
