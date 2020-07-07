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
            GenFunctions.BattleStatsGenerator(evVals, pokemon);
            pokemon.Moves = GenFunctions.MoveList(pokemon.Type);
            pokemon.MaxHp = pokemon.Base.HP[2];
            pokemon.CurHp = pokemon.MaxHp;
            pokemon.CurHpPercent = pokemon.CurHp * 100 / pokemon.MaxHp;
        }

        public void MoveOutcome(Move move)
        {
            double chanceHit = move.Accuracy / EnemyPokemon.Base.Speed[2];
            Random rnd = new Random();
            double chanceMiss = rnd.NextDouble();
            BaseStats PlayerPokemonStats = CurrentPlayer.ChosenPokemon.Base;

            if (chanceHit >= 1 || chanceHit > chanceMiss)
            {
                double attDefRatio = move.Category == "Physical" ? ((double)PlayerPokemonStats.Attack[2] / (double)PlayerPokemonStats.Defense[2]) : ((double)PlayerPokemonStats.SpecialAttack[2] / (double)PlayerPokemonStats.SpecialDefense[2]);
                double probCritHit = (double)PlayerPokemonStats.Speed[0] / 256;
                double critical = rnd.NextDouble() < probCritHit ? 2 : 1;
                double STAB = EnemyPokemon.Type.Any(m => m == move.Type) ? 1.5 : 1;
                double modifier = critical * (double)rnd.Next(217, 255) / 255 * STAB;
                int damage = Convert.ToInt32(((2 * CurrentPlayer.ChosenPokemon.CurLevel / 5 + 2) * move.Power * attDefRatio / 50 + 2) * modifier);
                EnemyPokemon.CurHp -= damage;
                EnemyPokemon.CurHpPercent = EnemyPokemon.CurHp * 100 / EnemyPokemon.MaxHp;

                /*RaiseMessage($"attDefRatio: {attDefRatio}, probCritHit: {probCritHit}, critical: {critical}, STAB: {STAB}");
                RaiseMessage($"modifier: {modifier}, damage: {damage}");*/

                RaiseMessage($"The {EnemyPokemon.Name} took {damage} points of damage");

                if (EnemyPokemon.CurHp <= 0)
                {
                    EnemyPokemon.CurHp = 0;
                    EnemyPokemon.CurHpPercent = 0;
                    RaiseMessage($"You defeated {EnemyPokemon.Name}");
                    CurrentPlayer.Money += rnd.Next(10, 150);

                    double term = Math.Pow(
                        (2 * Convert.ToDouble(EnemyPokemon.CurLevel) + 10) / (Convert.ToDouble(EnemyPokemon.CurLevel + CurrentPlayer.ChosenPokemon.CurLevel) + 10), 
                        2.5
                    );

                    int xpEarned = (EnemyPokemon.XP * EnemyPokemon.CurLevel / 5) * Convert.ToInt32(term) + 1;
                    CurrentPlayer.PokemonCollection.First(p => p.Id == CurrentPlayer.ChosenPokemon.Id).XP += xpEarned;
                    
                }
            }
            else { RaiseMessage("The move completely missed its target\n"); }


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
