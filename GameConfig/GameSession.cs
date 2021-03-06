﻿using System;
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
        private TypeDamage _typeDamage = new TypeDamage();
        private bool _isGameCreated;
        private string _pokedexImage;

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
                if (rndEvSum > 510) { evVals[i] = 0; continue; }
                evVals[i] = rnd.Next(0, 256);
                rndEvSum += evVals[i];
            }

            pokemon.Base.HP.Insert(2, evVals[0]);
            pokemon.Base.Attack.Insert(2, evVals[1]);
            pokemon.Base.Defense.Insert(2, evVals[2]);
            pokemon.Base.SpecialAttack.Insert(2, evVals[3]);
            pokemon.Base.SpecialDefense.Insert(2, evVals[4]);
            pokemon.Base.Speed.Insert(2, evVals[5]);

            GenFunctions.BattleStatsGenerator(pokemon);
            pokemon.Moves = GenFunctions.MoveList(pokemon.Type);
        }

        public void MoveOutcome(Move move, Pokemon attackingPokemon, Pokemon defendingPokemon)
        {
            double chanceHit = (double)move.Accuracy / (double)defendingPokemon.Base.Speed[3];
            Random rnd = new Random();
            double chanceMiss = rnd.NextDouble();
            BaseStats PokemonStats = attackingPokemon.Base;

            if (IsBattle)
            {
                RaiseMessage($"{attackingPokemon.Name} used {move.Ename}");

                if (chanceHit >= 1 || chanceHit > chanceMiss)
                {
                    double attDefRatio = 
                        move.Category.Equals("Physical") ? 
                            ((double)PokemonStats.Attack[3] / (double)PokemonStats.Defense[3]) : 
                            ((double)PokemonStats.SpecialAttack[3] / (double)PokemonStats.SpecialDefense[3]);

                    double probCritHit = (double)PokemonStats.Speed[0] / 256;
                    double critical = rnd.NextDouble() < probCritHit ? 2 : 1;
                    double STAB = defendingPokemon.Type.Any(m => m == move.Type) ? 1.5 : 1;
                    double modifier = critical * (double)rnd.Next(217, 255) * STAB * _typeDamage.TypeMultiplier(attackingPokemon.Type, defendingPokemon.Type) / 255;
                    int damage = Convert.ToInt32(((2 * attackingPokemon.CurLevel / 5 + 2) * move.Power * attDefRatio / 50 + 2) * modifier);
                    defendingPokemon.CurHp -= damage;
                    defendingPokemon.CurHpPercent = defendingPokemon.CurHp * 100 / defendingPokemon.MaxHp;

                    if (critical == 2) { RaiseMessage("It's a critical hit!!!"); }
                    RaiseMessage($"{defendingPokemon.Name} took {damage} points of damage");
                }
                else { RaiseMessage("The move completely missed its target\n"); }
            }
        }

        public bool TryToFlee()
		{
            Random rnd = new Random();
            int chance = 100;
            if (EnemyPokemon.CurLevel >= CurrentPlayer.ChosenPokemon.CurLevel)
            {
                // 80% chance to flee at the same level, 20% when the enemy is 10 levels (or more) above the player pokemon
                int diff = Math.Min(10, EnemyPokemon.CurLevel - CurrentPlayer.ChosenPokemon.CurLevel);
                chance = -6 * diff + 80;
            }
            if (rnd.Next(1, 101) <= chance)
            {
                CurrentPlayer.Fights += 1;
                RaiseMessage($"You fled successfully!");
                CurrentPlayer.Losses += 1;
                EnemyPokemon.CurHp = 0;
                CurrentPlayer.WinPercentage = CurrentPlayer.Wins * 100 / (double) CurrentPlayer.Fights;

                GenFunctions.PokemonLeveller(CurrentPlayer.PokemonCollection.First(p => p.Id == CurrentPlayer.ChosenPokemon.Id));
                return true;
            }
            else
            {
                RaiseMessage($"You did not manage to get away!");
                return false;
            }
        }

        public void PlayerWon()
        {
            Random rnd = new Random();
            EnemyPokemon.CurHp = 0;
            EnemyPokemon.CurHpPercent = 0;
            CurrentPlayer.Fights += 1;
            RaiseMessage($"\nOpponent {EnemyPokemon.Name} has fainted");
            RaiseMessage($"You won the battle");

            int moneyEarned = rnd.Next(10, 150);
            CurrentPlayer.Money += moneyEarned;
            RaiseMessage($"\nYou earned {moneyEarned}¥");

            if (CurrentPlayer.ChosenPokemon != null && EnemyPokemon.Category != null)
            {
                double trainer = EnemyPokemon.Category.Equals("Trainer") ? 1.5 : 1;
                double wild = EnemyPokemon.Category.Equals("Wild") ? 1 : 1.5;

                int xpEarned = Convert.ToInt32(trainer * EnemyPokemon.XP * EnemyPokemon.CurLevel * wild / 7);
                Pokemon rewardedPokemon = CurrentPlayer.PokemonCollection.First(p => p.Id == CurrentPlayer.ChosenPokemon.Id);

                if (rewardedPokemon.CurXp + xpEarned <= rewardedPokemon.MAX_XP)
                {
                    CurrentPlayer.PokemonCollection.First(p => p.Id == CurrentPlayer.ChosenPokemon.Id).CurXp += xpEarned;
                    RaiseMessage($"\nYour {CurrentPlayer.ChosenPokemon.Name} earned {xpEarned}XP");
                    if (rewardedPokemon.CurLevel < CurrentPlayer.PokemonCollection.First(p => p.Id == CurrentPlayer.ChosenPokemon.Id).CurLevel)
                    {
                        rewardedPokemon = CurrentPlayer.PokemonCollection.First(p => p.Id == CurrentPlayer.ChosenPokemon.Id);
                        RaiseMessage($"\nYour {rewardedPokemon.Name} grew to level {rewardedPokemon.CurLevel}");
                    }
                }
            }

            CurrentPlayer.Wins += 1;
            CurrentPlayer.WinPercentage = Math.Round((double)CurrentPlayer.Wins * 100 / (double)CurrentPlayer.Fights, 2);

            GenFunctions.PokemonLeveller(CurrentPlayer.PokemonCollection.First(p => p.Id == CurrentPlayer.ChosenPokemon.Id));
        }

        public void OpponentWon()
        {
            Random rnd = new Random();
            CurrentPlayer.ChosenPokemon.CurHp = 0;
            CurrentPlayer.ChosenPokemon.CurHpPercent = 0;
            CurrentPlayer.Fights += 1;
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
            CurrentPlayer.WinPercentage = CurrentPlayer.Wins * 100 / (double)CurrentPlayer.Fights;

            if (EnemyPokemon.Category.Equals("Trainer")) 
            {
                CurrentPlayer.Money /= 2;
                RaiseMessage($"You lost {CurrentPlayer.Money}¥"); 
            }

            GenFunctions.PokemonLeveller(CurrentPlayer.PokemonCollection.First(p => p.Id == CurrentPlayer.ChosenPokemon.Id));

        }

        public void ItemEffect(string name)
        {
            Item item = CurrentPlayer.Inventory.FirstOrDefault(i => i.Name.Equals(name));

            if (item != null && IsBattle)
            {
                if (EnemyPokemon.Category.Equals("Wild"))
                {
                    int a = Convert.ToInt32(((3 * EnemyPokemon.MaxHp) - (2 * EnemyPokemon.CurHp)) * EnemyPokemon.CatchRate * item.CatchRate / (3 * EnemyPokemon.MaxHp));

                    if (a > 0 || item.ID == 8)
                    {
                        if (a > 0)
                        {
                            int b = 1048560 / Convert.ToInt32(Math.Sqrt(Math.Sqrt(16711680 / a)));
                            Random rnd = new Random();
                            for (int i = 0; i < 4; i++)
                            {
                                if (b <= rnd.Next(0, 65536)) 
                                { 
                                    RaiseMessage($"The {EnemyPokemon.Name} managed to break out!!!");
                                    CurrentPlayer.RemoveFromInventory(item.ID);
                                    return;
                                }
                            }
                        }
                        RaiseMessage($"You managed to catch the {EnemyPokemon.Name}");
                        CurrentPlayer.RemoveFromInventory(item.ID);

                        Pokemon captured = EnemyPokemon.Clone();
                        
                        captured.Category = null;
                        PlayerWon();
                        CurrentPlayer.PokemonCollection.Add(captured);
                    }
                }
                else if (EnemyPokemon.Category.Equals("Trainer") && item.Heal == 0)
                {
                    RaiseMessage("You can't capture a trainer's pokémon");
                }

                if ((item.Heal > 0 || item.ID == 7) && CurrentPlayer.ChosenPokemon != null)
                {
                    int pointsRestored = item.Heal;
                    CurrentPlayer.ChosenPokemon.CurHp += item.Heal;
                    CurrentPlayer.ChosenPokemon.CurHpPercent = CurrentPlayer.ChosenPokemon.CurHp * 100 / CurrentPlayer.ChosenPokemon.MaxHp;

                    if (CurrentPlayer.ChosenPokemon.CurHp > CurrentPlayer.ChosenPokemon.MaxHp || item.ID == 7)
                    {
                        pointsRestored += CurrentPlayer.ChosenPokemon.MaxHp - CurrentPlayer.ChosenPokemon.CurHp;
                        CurrentPlayer.ChosenPokemon.CurHp = CurrentPlayer.ChosenPokemon.MaxHp;
                        CurrentPlayer.ChosenPokemon.CurHpPercent = 100;
                    }
                    RaiseMessage($"Your {CurrentPlayer.ChosenPokemon.Name} restored {pointsRestored} heath points");
                    CurrentPlayer.RemoveFromInventory(item.ID);
                }
            }
        }

        public bool IsBattle => EnemyPokemon != null && EnemyPokemon.Name != null && EnemyPokemon.CurHp > 0 && CurrentPlayer.ChosenPokemon != null && CurrentPlayer.ChosenPokemon.CurHp > 0;

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
