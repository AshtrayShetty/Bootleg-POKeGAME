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

        public void MoveOutcome(Move move, Pokemon attackingPokemon, Pokemon defendingPokemon)
        {
            double chanceHit = (double)move.Accuracy / (double)defendingPokemon.Base.Speed[2];
            Random rnd = new Random();
            double chanceMiss = rnd.NextDouble();
            BaseStats PokemonStats = attackingPokemon.Base;

            if (IsBattle)
            {
                if (chanceHit >= 1 || chanceHit > chanceMiss)
                {
                    double attDefRatio = move.Category == "Physical" ? ((double)PokemonStats.Attack[2] / (double)PokemonStats.Defense[2]) : ((double)PokemonStats.SpecialAttack[2] / (double)PokemonStats.SpecialDefense[2]);
                    double probCritHit = (double)PokemonStats.Speed[0] / 256;
                    double critical = rnd.NextDouble() < probCritHit ? 2 : 1;
                    double STAB = defendingPokemon.Type.Any(m => m == move.Type) ? 1.5 : 1;
                    double modifier = critical * (double)rnd.Next(217, 255) * STAB / 255 ;
                    int damage = Convert.ToInt32(((2 * attackingPokemon.CurLevel / 5 + 2) * move.Power * attDefRatio / 50 + 2) * modifier);
                    defendingPokemon.CurHp -= damage;
                    defendingPokemon.CurHpPercent = defendingPokemon.CurHp * 100 / defendingPokemon.MaxHp;

                    RaiseMessage($"{attackingPokemon.Name} used {move.Ename}");
                    if (critical == 2) { RaiseMessage("It's a critical hit!!!"); }
                    RaiseMessage($"{defendingPokemon.Name} took {damage} points of damage");
                }
                else { RaiseMessage("The move completely missed its target\n"); }
            }
        }

        public void PlayerWon()
        {
            Random rnd = new Random();
            EnemyPokemon.CurHp = 0;
            EnemyPokemon.CurHpPercent = 0;
            RaiseMessage($"\nOpponent {EnemyPokemon.Name} has fainted");
            RaiseMessage($"You won the battle");

            int moneyEarned = rnd.Next(10, 150);
            CurrentPlayer.Money += moneyEarned;
            RaiseMessage($"\nYou earned {moneyEarned}¥");

            double term = Math.Pow(
                (2 * Convert.ToDouble(EnemyPokemon.CurLevel) + 10) / (Convert.ToDouble(EnemyPokemon.CurLevel + CurrentPlayer.ChosenPokemon.CurLevel) + 10),
                2.5
            );

            int xpEarned = (EnemyPokemon.XP * EnemyPokemon.CurLevel / 5) * Convert.ToInt32(term) + 1;
            CurrentPlayer.PokemonCollection.First(p => p.Id == CurrentPlayer.ChosenPokemon.Id).XP += xpEarned;
            RaiseMessage($"\nYour {CurrentPlayer.ChosenPokemon.Name} earned {xpEarned}XP");

            CurrentPlayer.Wins += 1;
            CurrentPlayer.WinPercentage = Math.Round((double)CurrentPlayer.Wins * 100 / (double)CurrentPlayer.Fights, 2);
        }

        public void OpponentWon()
        {
            Random rnd = new Random();
            CurrentPlayer.ChosenPokemon.CurHp = 0;
            CurrentPlayer.ChosenPokemon.CurHpPercent = 0;
            RaiseMessage($"\nYour {CurrentPlayer.ChosenPokemon.Name} has fainted");
            RaiseMessage($"You lost the battle");

            CurrentPlayer.ChosenPokemon.Base.HP[1] += EnemyPokemon.Base.HP[1];
            CurrentPlayer.ChosenPokemon.Base.Attack[1] += EnemyPokemon.Base.Attack[1];
            CurrentPlayer.ChosenPokemon.Base.Defense[1] += EnemyPokemon.Base.Defense[1];
            CurrentPlayer.ChosenPokemon.Base.SpecialAttack[1] += EnemyPokemon.Base.SpecialAttack[1];
            CurrentPlayer.ChosenPokemon.Base.SpecialDefense[1] += EnemyPokemon.Base.SpecialDefense[1];
            CurrentPlayer.ChosenPokemon.Base.Speed[1] += EnemyPokemon.Base.Speed[1];

            CurrentPlayer.PokemonCollection.First(p => p.Id == CurrentPlayer.ChosenPokemon.Id).Base = CurrentPlayer.ChosenPokemon.Base;
            CurrentPlayer.Losses += 1;
            CurrentPlayer.WinPercentage = CurrentPlayer.Wins * 100 / CurrentPlayer.Fights;
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
