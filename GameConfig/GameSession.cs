using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameConfig
{
    public class GameSession : INotifyPropertyChanged
    {
        private Player _player = new Player();
        private Pokemon _enemy = new Pokemon();
        private bool _isGameCreated;
        private int _losses;
        private double _winPercentage;
        private string _pokedexImage;
        private bool _isBattle;

        public Player CurrentPlayer
        {
            get => _player;
            set
            {
                _player = value;
                OnPropertyChanged(nameof(CurrentPlayer));
            }
        }

        public Pokemon EnemyPokemon
        {
            get => _enemy;
            set
            {
                _enemy = value;
                OnPropertyChanged(nameof(EnemyPokemon));
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

        public bool IsBattle
        {
            get => _isBattle;
            set
            {
                _isBattle = value;
                OnPropertyChanged(nameof(IsBattle));
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

        public List<Pokemon> AllPokemon { get; set; }

        public string PokedexImage
        {
            get => _pokedexImage;
            set
            {
                _pokedexImage = value;
                OnPropertyChanged(nameof(PokedexImage));
            }
        }

        public void GeneratePokemonStats(Pokemon pokemon)
        {
            Random rnd = new Random();
            int rndEvSum = 0;
            int[] evVals = new int[6];
            for (int i = 0; i < 6; ++i)
            {
                if (rndEvSum > 510) { break; }
                evVals[i] = rnd.Next(0, 256);
                rndEvSum += evVals[i];
            }
            int[] stats = GenFunctions.BattleStatsGenerator(evVals, pokemon);
            pokemon.CurLevel = stats[6];
            pokemon.Moves = GenFunctions.MoveList(pokemon.Type);
            pokemon.MaxHp = stats[0];
            pokemon.CurHp = stats[0];
            pokemon.CurHpPercent = (Convert.ToInt32(pokemon.CurHp) / Convert.ToInt32(pokemon.MaxHp)) * 100;
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
