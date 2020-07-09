using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameConfig
{
    public static class GenFunctions
    {
        private static int[] IVGenerator(BaseStats stats, int level)
        {
            int evSum= stats.HP[1] + stats.Attack[1] + stats.Defense[1] + stats.SpecialAttack[1] + stats.SpecialDefense[1] + stats.Speed[1];
            int[] ivArr = new int[6];
            ivArr[0]= ((stats.HP[0] + level - 10) * 100 / level) - 2 * stats.HP[0] - Convert.ToInt32(Math.Floor(Convert.ToDecimal(evSum / 6)));
            ivArr[1]= ((stats.Attack[0] - 5) * 100 / level) - 2 * stats.Attack[0] - Convert.ToInt32(Math.Floor(Convert.ToDecimal(evSum / 6)));
            ivArr[2]= ((stats.Defense[0] - 5) * 100 / level) - 2 * stats.Defense[0] - Convert.ToInt32(Math.Floor(Convert.ToDecimal(evSum / 6)));
            ivArr[3]= ((stats.SpecialAttack[0] - 5) * 100 / level) - 2 * stats.SpecialAttack[0] - Convert.ToInt32(Math.Floor(Convert.ToDecimal(evSum / 6)));
            ivArr[4]= ((stats.SpecialDefense[0] - 5) * 100 / level) - 2 * stats.SpecialDefense[0] - Convert.ToInt32(Math.Floor(Convert.ToDecimal(evSum / 6)));
            ivArr[5]= ((stats.Speed[0] - 5) * 100 / level) - 2 * stats.Speed[0] - Convert.ToInt32(Math.Floor(Convert.ToDecimal(evSum / 6)));
            return ivArr;
        }
        public static void BattleStatsGenerator(Pokemon pokemon)
        {
            int[] battleStats = IVGenerator(pokemon.Base, pokemon.CurLevel);

            pokemon.Base.HP.Insert(
                3,
                Convert.ToInt32(
                    Math.Floor(
                        ((pokemon.Base.HP[0] + battleStats[0]) * 2 
                        + Math.Floor(Math.Ceiling(Math.Sqrt(pokemon.Base.HP[2])) / 4)) * pokemon.CurLevel / 100
                    ) + pokemon.CurLevel + 10
                )
            );

            pokemon.Base.Attack.Insert(
                3,
                Convert.ToInt32(
                    Math.Floor(
                        ((pokemon.Base.Attack[0] + battleStats[1]) * 2 
                        + Math.Floor(Math.Ceiling(Math.Sqrt(pokemon.Base.Attack[2])) / 4)) * pokemon.CurLevel / 100
                    ) + 5
                )
            );

            pokemon.Base.Defense.Insert(
                3,
                Convert.ToInt32(
                    Math.Floor(
                        ((pokemon.Base.Defense[0] + battleStats[2]) * 2 
                        + Math.Floor(Math.Ceiling(Math.Sqrt(pokemon.Base.Defense[2])) / 4)) * pokemon.CurLevel / 100
                    ) + 5
                )
            );

            pokemon.Base.SpecialAttack.Insert(
                3,
                Convert.ToInt32(
                    Math.Floor(
                        ((pokemon.Base.SpecialAttack[0] + battleStats[3]) * 2 
                        + Math.Floor(Math.Ceiling(Math.Sqrt(pokemon.Base.SpecialAttack[2])) / 4)) * pokemon.CurLevel / 100
                    ) + 5
                )
            );

            pokemon.Base.SpecialDefense.Insert(
                3,
                Convert.ToInt32(
                    Math.Floor(
                        ((pokemon.Base.SpecialDefense[0] + battleStats[4]) * 2 
                        + Math.Floor(Math.Ceiling(Math.Sqrt(pokemon.Base.SpecialDefense[2])) / 4)) * pokemon.CurLevel / 100
                    ) + 5
                )
            );

            pokemon.Base.Speed.Insert(
                3,
                Convert.ToInt32(
                    Math.Floor(
                        ((pokemon.Base.Speed[0] + battleStats[5]) * 2 
                        + Math.Floor(Math.Ceiling(Math.Sqrt(pokemon.Base.Speed[2])) / 4)) * pokemon.CurLevel / 100
                    ) + 5
                )
            );

            pokemon.CurHp = pokemon.Base.HP[3];
            pokemon.MaxHp = pokemon.Base.HP[3];
            pokemon.CurHpPercent = 100;
        }

        public static List<Move> MoveList(string[] type)
        {
            List<Move> moveList = new List<Move>();
            using (StreamReader movesJson = new StreamReader("..\\..\\..\\moves.json"))
            {
                var json = JsonConvert.DeserializeObject<List<Move>>(movesJson.ReadToEnd());
                var moves = json.FindAll(move => type.Contains(move.Type));
                Random rnd = new Random();

                for (int i = 0; i < 4; ++i)
                {
                    int index = rnd.Next(0, moves.Count);
                    Move move = moves[index];
                    if (moveList.Any(m => m.Id == move.Id) || (move.Power == -1 || move.Accuracy == -1))
                    {
                        i -= 1;
                        continue;
                    }
                    moveList.Add(move);
                }
                return moveList;
            }
        }

        public static void PokemonLeveller(Pokemon pokemon)
        {
            int xpRequired = 0;
            switch (pokemon.Growth)
            {
                case "erratic":
                    if (pokemon.CurLevel >= 0 && pokemon.CurLevel + 1 <= 50)
                    {
                        xpRequired = 
                            Convert.ToInt32(
                                Math.Pow(Convert.ToDouble(pokemon.CurLevel + 1), 3) 
                                * (double)pokemon.CurLevel 
                                / 50
                            );
                    }
                    else if (pokemon.CurLevel + 1 >= 50 && pokemon.CurLevel + 1 <= 68)
                    {
                        xpRequired = 
                            Convert.ToInt32(
                                Math.Pow(Convert.ToDouble(pokemon.CurLevel + 1), 3) 
                                * (149 - pokemon.CurLevel) 
                                / 100
                            );
                    }
                    else if (pokemon.CurLevel + 1 >= 68 && pokemon.CurLevel + 1 <= 98)
                    {
                        xpRequired =
                            Convert.ToInt32(
                                Math.Pow(Convert.ToDouble(pokemon.CurLevel + 1), 3)
                                * Math.Floor((1901 - (10 * (double)pokemon.CurLevel)) / 3) 
                                / 500
                            );
                    }
                    else if (pokemon.CurLevel + 1 >= 98 && pokemon.CurLevel + 1 <= 100)
                    {
                        xpRequired = 
                            Convert.ToInt32(
                                Math.Pow(Convert.ToDouble(pokemon.CurLevel + 1), 3) 
                                * (159 - pokemon.CurLevel) 
                                / 100
                            );
                    }
                    break;

                case "fast":
                    xpRequired = Convert.ToInt32(4 * Math.Pow(Convert.ToDouble(pokemon.CurLevel + 1), 3) / 5);
                    break;

                case "medium fast":
                    xpRequired = Convert.ToInt32(Math.Pow(Convert.ToDouble(pokemon.CurLevel + 1), 3));
                    break;

                case "medium slow":
                    xpRequired = Convert.ToInt32(
                        (6 * Math.Pow(Convert.ToDouble(pokemon.CurLevel + 1), 3) / 5)
                        - (15 * Math.Pow(Convert.ToDouble(pokemon.CurLevel + 1), 2))
                        + (100 * (pokemon.CurLevel + 1)) 
                        - 140
                    );
                    break;

                case "slow":
                    xpRequired = Convert.ToInt32(5 * Math.Pow(Convert.ToDouble(pokemon.CurLevel + 1), 3) / 4);
                    break;

                case "fluctuating":
                    if (pokemon.CurLevel >= 0 && pokemon.CurLevel + 1 <= 15)
                    {
                        xpRequired =
                            Convert.ToInt32(
                                Math.Pow(Convert.ToDouble(pokemon.CurLevel + 1), 3)
                                * (Math.Floor(Convert.ToDouble(pokemon.CurLevel + 2) / 3) + 24)
                                / 50
                            );
                    }
                    else if (pokemon.CurLevel >= 15 && pokemon.CurLevel <= 36)
                    {
                        xpRequired =
                            Convert.ToInt32(
                                Math.Pow(Convert.ToDouble(pokemon.CurLevel + 1), 3)
                                * (pokemon.CurLevel + 15)
                                / 50
                            );
                    }
                    else if (pokemon.CurLevel + 1 >= 36 && pokemon.CurLevel + 1 <= 100)
                    {
                        xpRequired =
                            Convert.ToInt32(
                                Math.Pow(Convert.ToDouble(pokemon.CurLevel + 1), 3)
                                * (Math.Floor(Convert.ToDouble((pokemon.CurLevel + 1) / 2)) + 32)
                                / 50
                            );
                    }
                    break;

                default:
                    break;
            }

            if (pokemon.CurXp >= xpRequired)
            {
                pokemon.CurLevel += 1;
                PokemonLeveller(pokemon);
            }
        }
    }
}
