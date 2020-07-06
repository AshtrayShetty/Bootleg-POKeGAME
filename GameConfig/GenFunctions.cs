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
            int[] ivArr = new int[7];
            ivArr[0]= ((stats.HP[0] - level - 10) * 100 / level) - 2 * stats.HP[0] - Convert.ToInt32(Math.Floor(Convert.ToDecimal(evSum / 6)));
            ivArr[1]= ((stats.Attack[0] - 5) * 100 / level) - 2 * stats.Attack[0] - Convert.ToInt32(Math.Floor(Convert.ToDecimal(evSum / 6)));
            ivArr[2]= ((stats.Defense[0] - 5) * 100 / level) - 2 * stats.Defense[0] - Convert.ToInt32(Math.Floor(Convert.ToDecimal(evSum / 6)));
            ivArr[3]= ((stats.SpecialAttack[0] - 5) * 100 / level) - 2 * stats.SpecialAttack[0] - Convert.ToInt32(Math.Floor(Convert.ToDecimal(evSum / 6)));
            ivArr[4]= ((stats.SpecialDefense[0] - 5) * 100 / level) - 2 * stats.SpecialDefense[0] - Convert.ToInt32(Math.Floor(Convert.ToDecimal(evSum / 6)));
            ivArr[5]= ((stats.Speed[0] - 5) * 100 / level) - 2 * stats.Speed[0] - Convert.ToInt32(Math.Floor(Convert.ToDecimal(evSum / 6)));
            ivArr[6] = level;
            return ivArr;
        }
        public static int[] BattleStatsGenerator(int[] battleEv, Pokemon pokemon)
        {
            Random rnd = new Random();
            int level = rnd.Next(pokemon.BaseLevel, pokemon.EvolutionLevel);
            int[] battleStats = IVGenerator(pokemon.Base, level);
            battleStats[0]= Convert.ToInt32(
                    Math.Floor(
                        ((pokemon.Base.HP[0] + battleStats[0]) * 2 + Math.Floor(Math.Ceiling(Math.Sqrt(battleEv[0])) / 4)) * level / 100
                    ) + level + 10
                );
            battleStats[1] = Convert.ToInt32(
                    Math.Floor(
                        ((pokemon.Base.Attack[0] + battleStats[1]) * 2 + Math.Floor(Math.Ceiling(Math.Sqrt(battleEv[1])) / 4)) * level / 100
                    ) + 5
                );
            battleStats[2] = Convert.ToInt32(
                    Math.Floor(
                        ((pokemon.Base.Defense[0] + battleStats[2]) * 2 + Math.Floor(Math.Ceiling(Math.Sqrt(battleEv[2])) / 4)) * level / 100
                    ) + 5
                );
            battleStats[3] = Convert.ToInt32(
                    Math.Floor(
                        ((pokemon.Base.SpecialAttack[0] + battleStats[3]) * 2 + Math.Floor(Math.Ceiling(Math.Sqrt(battleEv[3])) / 4)) * level / 100
                    ) + 5
                );
            battleStats[4] = Convert.ToInt32(
                    Math.Floor(
                        ((pokemon.Base.SpecialDefense[0] + battleStats[4]) * 2 + Math.Floor(Math.Ceiling(Math.Sqrt(battleEv[4])) / 4)) * level / 100
                    ) + 5
                );
            battleStats[5] = Convert.ToInt32(
                    Math.Floor(
                        ((pokemon.Base.Speed[0] + battleStats[5]) * 2 + Math.Floor(Math.Ceiling(Math.Sqrt(battleEv[5])) / 4)) * level / 100
                    ) + 5
                );
            return battleStats;
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
                    if (moveList.Any(m => m.Id == move.Id))
                    {
                        i -= 1;
                        continue;
                    }
                    moveList.Add(move);
                }
                return moveList;
            }
        }
    }
}
