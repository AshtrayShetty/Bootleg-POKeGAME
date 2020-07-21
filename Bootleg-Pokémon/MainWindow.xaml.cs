using GameConfig;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bootleg_Pokémon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private readonly GameSession _gameSession = new GameSession();
        private Random rnd = new Random();
        private List<Pokemon> _enemyPokemons = new List<Pokemon>();
        private List<int> _ids = new List<int>();
        private List<int> _levels = new List<int>();
        private string _trainer = "";

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _gameSession;
            _gameSession.IsGameCreated = false;
            _gameSession.Event += RaiseBattleMessages;

            using (StreamReader pokedexJson = new StreamReader("..\\..\\..\\pokedex.json"))
            {
                var json = pokedexJson.ReadToEnd();
                _gameSession.AllPokemon = JsonConvert.DeserializeObject<List<Pokemon>>(json);
                // MessageBox.Show(_gameSession.AllPokemon.Count().ToString());
            }
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            NewGame newGame = new NewGame();
            newGame.DataContext = _gameSession;
            newGame.Owner = this;
            newGame.Show();
        }
        
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {
                string[] data = File.ReadAllLines(openFile.FileName);
                _gameSession.CurrentPlayer.Name = data[0].Substring(6);
                _gameSession.CurrentPlayer.Fights = int.Parse(data[1].Substring(data[1].IndexOf(':') + 2));
                _gameSession.CurrentPlayer.Wins = int.Parse(data[2].Substring(data[2].IndexOf(':') + 2));
                _gameSession.IsGameCreated = true;
                _gameSession.CurrentPlayer.Losses = _gameSession.CurrentPlayer.Fights - _gameSession.CurrentPlayer.Wins;

                if (_gameSession.CurrentPlayer.Fights != 0)
                {
                    _gameSession.CurrentPlayer.WinPercentage = Math.Round(Convert.ToDouble(_gameSession.CurrentPlayer.Wins) * 100.0 / Convert.ToDouble(_gameSession.CurrentPlayer.Fights), 2);
                }
                else { _gameSession.CurrentPlayer.WinPercentage = 0.0; }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            if (saveFile.ShowDialog() == true)
            {
                using(StreamWriter sw = File.CreateText(saveFile.FileName))
                {
                    sw.WriteLine($"Name: {_gameSession.CurrentPlayer.Name}");
                    sw.WriteLine($"Fights: {_gameSession.CurrentPlayer.Fights}");
                    sw.WriteLine($"Wins: {_gameSession.CurrentPlayer.Wins}");
                    sw.WriteLine($"Catches: {_gameSession.CurrentPlayer.PokemonCollection.Count}");
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Save_Click(sender, e);
            Close();
        }

        private void Pokedex_Click(object sender, RoutedEventArgs e)
        {
            Pokedex pokedex = new Pokedex();
            pokedex.DataContext = _gameSession;
            pokedex.Owner = this;
            pokedex.Show();
        }

        private void InitializeOpponent(string category)
        {
            if (_gameSession.CurrentPlayer.Money == 0 && category.Equals("Trainer"))
            {
                MessageBox.Show("You have no money to challenge a trainer");
                MenuBar.IsEnabled = true;
                return;
            }

            /*_gameSession.EnemyPokemon = _gameSession.AllPokemon.First(p => p.Id == id);
            _gameSession.EnemyPokemon.CurLevel = level;*/

            if(_enemyPokemons.Count == 0){
                for (int i = 0; i < _ids.Count; ++i)
                {
                    Pokemon pokemon = _gameSession.AllPokemon.First(p => p.Id == _ids[i]);
                    pokemon.Category = category;
                    pokemon.CurLevel = _levels[i];
                    _enemyPokemons.Add(pokemon);
                }
            }

            _gameSession.EnemyPokemon = _enemyPokemons[rnd.Next(0, _enemyPokemons.Count)];
            _gameSession.GeneratePokemonStats(_gameSession.EnemyPokemon);
            EnemyCorner.Visibility = Visibility.Visible;
            MenuBar.IsEnabled = false;
        }

        private void Brock_Click(object sender, RoutedEventArgs e)
        {
            _ids.Add(74); _ids.Add(95);
            _levels.Add(12); _levels.Add(14);
            InitializeOpponent("Trainer");
            _trainer = "Brock";

            if (_gameSession.EnemyPokemon.Name != null)
            {
                FightStatus.Document.Blocks.Add(new Paragraph(new Run($"Brock chose {_gameSession.EnemyPokemon.Name}")));
            }
        }

        private void Misty_Click(object sender, RoutedEventArgs e)
        {
            _ids.Add(120); _ids.Add(121);
            _levels.Add(18); _levels.Add(21);
            InitializeOpponent("Trainer");
            _trainer = "Misty";

            if (_gameSession.EnemyPokemon.Name != null)
            {
                FightStatus.Document.Blocks.Add(new Paragraph(new Run($"Misty chose {_gameSession.EnemyPokemon.Name}")));
            }
        }

        private void Surge_Click(object sender, RoutedEventArgs e)
        {
            _ids.Add(100); _ids.Add(25); _ids.Add(26);
            _levels.Add(21); _levels.Add(18); _levels.Add(24);
            InitializeOpponent("Trainer");
            _trainer = "Surge";

            if (_gameSession.EnemyPokemon.Name != null)
            {
                FightStatus.Document.Blocks.Add(new Paragraph(new Run($"Lt. Surge chose {_gameSession.EnemyPokemon.Name}")));   
            }
        }

        private void Erika_Click(object sender, RoutedEventArgs e)
        {
            _ids.Add(71); _ids.Add(114); _ids.Add(45);
            _levels.Add(29); _levels.Add(24); _levels.Add(29);
            InitializeOpponent("Trainer");
            _trainer = "Erika";

            if (_gameSession.EnemyPokemon.Name != null)
            {
                FightStatus.Document.Blocks.Add(new Paragraph(new Run($"Erika chose {_gameSession.EnemyPokemon.Name}")));
            }
        }

        private void Janine_Click(object sender, RoutedEventArgs e)
        {
            _ids.Add(169); _ids.Add(110); _ids.Add(110); _ids.Add(168); _ids.Add(49);
            _levels.Add(36); _levels.Add(36); _levels.Add(36); _levels.Add(33); _levels.Add(39);
            InitializeOpponent("Trainer");
            _trainer = "Janine";

            if (_gameSession.EnemyPokemon.Name != null)
            {
                FightStatus.Document.Blocks.Add(new Paragraph(new Run($"Janine chose {_gameSession.EnemyPokemon.Name}")));
            }
        }

        private void Sabrina_Click(object sender, RoutedEventArgs e)
        {
            _ids.Add(64); _ids.Add(122); _ids.Add(49); _ids.Add(65);
            _levels.Add(38); _levels.Add(37); _levels.Add(38); _levels.Add(43);
            InitializeOpponent("Trainer");
            _trainer = "Sabrina";

            if (_gameSession.EnemyPokemon.Name != null)
            {
                FightStatus.Document.Blocks.Add(new Paragraph(new Run($"Sabrina chose {_gameSession.EnemyPokemon.Name}")));
            }
        }

        private void Blaine_Click(object sender, RoutedEventArgs e)
        {
            _ids.Add(58); _ids.Add(77); _ids.Add(78); _ids.Add(59);
            _levels.Add(42); _levels.Add(40); _levels.Add(42); _levels.Add(47);
            InitializeOpponent("Trainer");
            _trainer = "Blaine";

            if (_gameSession.EnemyPokemon.Name != null)
            {
                FightStatus.Document.Blocks.Add(new Paragraph(new Run($"Blaine chose {_gameSession.EnemyPokemon.Name}")));
            }
        }

        private void Gary_Click(object sender, RoutedEventArgs e)
        {
            _ids.Add(18); _ids.Add(65); _ids.Add(112); _ids.Add(103); _ids.Add(130); _ids.Add(6);
            _levels.Add(61); _levels.Add(59); _levels.Add(61); _levels.Add(61); _levels.Add(63); _levels.Add(65);
            InitializeOpponent("Trainer");
            _trainer = "Gary";

            if (_gameSession.EnemyPokemon.Name != null)
            {
                FightStatus.Document.Blocks.Add(new Paragraph(new Run($"Blaine chose {_gameSession.EnemyPokemon.Name}")));
            }
        }

        private void Elite_Four_Click(object sender, RoutedEventArgs e)
        {
            _ids.Add(178); _ids.Add(103); _ids.Add(80); _ids.Add(124); _ids.Add(178);
            _levels.Add(40); _levels.Add(42); _levels.Add(42); _levels.Add(41); _levels.Add(40);
            InitializeOpponent("Trainer");
            _trainer = "Will";

            if (_gameSession.EnemyPokemon.Name != null)
            {
                FightStatus.Document.Blocks.Add(new Paragraph(new Run($"Will chose {_gameSession.EnemyPokemon.Name}")));
            }
        }

        private void Pokemon_Choose_Click(object sender, RoutedEventArgs e)
        {
            if(PlayerPokemon.SelectedItem != null)
            {
                _gameSession.CurrentPlayer.ChosenPokemon = PlayerPokemon.SelectedItem as Pokemon;

                if (_gameSession.IsBattle && EnemyCorner.Visibility == Visibility.Visible)
                {
                    _gameSession.CurrentPlayer.ChosenPokemon.Moves = _gameSession.CurrentPlayer.PokemonCollection[PlayerPokemon.SelectedIndex].Moves;
                    PlayerCorner.Visibility = Visibility.Visible;
                    _gameSession.CurrentPlayer.Fights += 1;
                    FightStatus.Document.Blocks.Add(new Paragraph(new Run($"You chose {_gameSession.CurrentPlayer.ChosenPokemon.Name}")));
                
                }
                else
                {
                    MessageBox.Show($"{_gameSession.CurrentPlayer.ChosenPokemon.Name}: Level {_gameSession.CurrentPlayer.ChosenPokemon.CurLevel}");
                    _gameSession.CurrentPlayer.ChosenPokemon = null;
                }
            }
        }

        public Move PlayerMoveSelected => _gameSession.CurrentPlayer.ChosenPokemon.Moves.FirstOrDefault(m => m.IsSelected);

        private void Move_Click(object sender, RoutedEventArgs e)
        {
            if (_gameSession.IsBattle && PlayerMoveSelected != null)
            {
                _gameSession.MoveOutcome(PlayerMoveSelected, _gameSession.CurrentPlayer.ChosenPokemon, _gameSession.EnemyPokemon);

                if (_gameSession.EnemyPokemon.CurHp <= 0)
                {
                    _gameSession.PlayerWon();
                    Pokemon remove = _enemyPokemons.First(p => p.Id == _gameSession.EnemyPokemon.Id && p.CurLevel == _gameSession.EnemyPokemon.CurLevel);
                    _enemyPokemons.Remove(remove);

                    if (_enemyPokemons.Count == 0)
                    {
                        EndFight.Visibility = Visibility.Visible;
                        _ids.Clear(); _levels.Clear(); _enemyPokemons.Clear();
                        if (_gameSession.EnemyPokemon.Category.Equals("Trainer"))
                        {
                            switch (_trainer)
                            {
                                case "Brock":
                                    if (!_gameSession.CurrentPlayer.BadgeCollection.Any(b => b.Equals("Brock")))
                                    {
                                        MessageBox.Show("You earned the BoulderBadge");
                                        _gameSession.CurrentPlayer.BadgeCollection.Add("Brock");
                                        Misty.IsEnabled = true;
                                    }
                                    return;

                                case "Misty":
                                    if (!_gameSession.CurrentPlayer.BadgeCollection.Any(b => b.Equals("Misty")))
                                    {
                                        MessageBox.Show("You earned the CascadeBadge");
                                        _gameSession.CurrentPlayer.BadgeCollection.Add("Misty");
                                        Surge.IsEnabled = true;
                                    }
                                    return;

                                case "Surge":
                                    if(!_gameSession.CurrentPlayer.BadgeCollection.Any(b => b.Equals("Surge")))
                                    {
                                        MessageBox.Show("You earned the ThunderBadge");
                                        _gameSession.CurrentPlayer.BadgeCollection.Add("Surge");
                                        Erika.IsEnabled = true;
                                    }
                                    return;

                                case "Erika":
                                    if (!_gameSession.CurrentPlayer.BadgeCollection.Any(b => b.Equals("Erika")))
                                    {
                                        MessageBox.Show("You earned the RainbowBadge");
                                        _gameSession.CurrentPlayer.BadgeCollection.Add("Erika");
                                        Janine.IsEnabled = true;
                                    }
                                    return;

                                case "Janine":
                                    if (!_gameSession.CurrentPlayer.BadgeCollection.Any(b => b.Equals("Janine")))
                                    {
                                        MessageBox.Show("You earned the SoulBadge");
                                        _gameSession.CurrentPlayer.BadgeCollection.Add("Janine");
                                        Sabrina.IsEnabled = true;
                                    }
                                    return;

                                case "Sabrina":
                                    if (!_gameSession.CurrentPlayer.BadgeCollection.Any(b => b.Equals("Sabrina")))
                                    {
                                        MessageBox.Show("You earned the MarshBadge");
                                        _gameSession.CurrentPlayer.BadgeCollection.Add("Sabrina");
                                        Blaine.IsEnabled = true;
                                    }
                                    return;

                                case "Blaine":
                                    if (!_gameSession.CurrentPlayer.BadgeCollection.Any(b => b.Equals("Blaine")))
                                    {
                                        MessageBox.Show("You earned the VolcanoBadge");
                                        _gameSession.CurrentPlayer.BadgeCollection.Add("Blaine");
                                        Gary.IsEnabled = true;
                                    }
                                    return;

                                case "Gary":
                                    if (!_gameSession.CurrentPlayer.BadgeCollection.Any(b => b.Equals("Gary")))
                                    {
                                        MessageBox.Show("You earned the EarthBadge");
                                        MessageBox.Show("Congratulations!!! You can now challenge the elite four");
                                        _gameSession.CurrentPlayer.BadgeCollection.Add("Gary");
                                        EliteFour.IsEnabled = true; 
                                    }
                                    return;

                                case "Will":
                                    MessageBox.Show("You beat Will!! Your Next Opponent is Koga");
                                    EndFight.Visibility = Visibility.Hidden;
                                    EnemyCorner.Visibility = Visibility.Hidden;
                                    PlayerCorner.Visibility = Visibility.Hidden;
                                    _ids.Add(168); _ids.Add(205); _ids.Add(89); _ids.Add(49); _ids.Add(169);
                                    _levels.Add(41); _levels.Add(43); _levels.Add(43); _levels.Add(42); _levels.Add(40);
                                    InitializeOpponent("Trainer");
                                    _trainer = "Koga";
                                    return;

                                case "Koga":
                                    MessageBox.Show("You beat Koga!! Your Next Opponent is Bruno");
                                    EndFight.Visibility = Visibility.Hidden;
                                    EnemyCorner.Visibility = Visibility.Hidden;
                                    PlayerCorner.Visibility = Visibility.Hidden;
                                    _ids.Add(237); _ids.Add(107); _ids.Add(106); _ids.Add(95); _ids.Add(68);
                                    _levels.Add(43); _levels.Add(43); _levels.Add(43); _levels.Add(44); _levels.Add(44);
                                    InitializeOpponent("Trainer");
                                    _trainer = "Bruno";
                                    return;

                                case "Bruno":
                                    MessageBox.Show("You beat Bruno!! Your Next Opponent is Karen");
                                    EndFight.Visibility = Visibility.Hidden;
                                    EnemyCorner.Visibility = Visibility.Hidden;
                                    PlayerCorner.Visibility = Visibility.Hidden;
                                    _ids.Add(197); _ids.Add(45); _ids.Add(198); _ids.Add(94); _ids.Add(229);
                                    _levels.Add(49); _levels.Add(45); _levels.Add(41); _levels.Add(45); _levels.Add(44);
                                    InitializeOpponent("Trainer");
                                    _trainer = "Karen";
                                    return;

                                case "Karen":
                                    MessageBox.Show("You have proven your worth against the G/S/C Elite Four. It's time for you to fight the G/S/C Elite Four Champion: Lance!");
                                    EndFight.Visibility = Visibility.Hidden;
                                    EnemyCorner.Visibility = Visibility.Hidden;
                                    PlayerCorner.Visibility = Visibility.Hidden;
                                    _ids.Add(117); _ids.Add(142); _ids.Add(130);_ids.Add(6); _ids.Add(149);
                                    _levels.Add(43); _levels.Add(49); _levels.Add(52); _levels.Add(50); _levels.Add(55);
                                    InitializeOpponent("Trainer");
                                    _trainer = "Lance";
                                    return;

                                default:
                                    return;

                            }

                        }

                    }

                    InitializeOpponent(_gameSession.EnemyPokemon.Category);
                    return;
                }

                _gameSession.MoveOutcome(_gameSession.EnemyPokemon.Moves[rnd.Next(0, 4)], _gameSession.EnemyPokemon, _gameSession.CurrentPlayer.ChosenPokemon);

                if (_gameSession.CurrentPlayer.ChosenPokemon.CurHp <= 0)
                {
                    _gameSession.OpponentWon();
                    _ids.Clear(); _levels.Clear(); _enemyPokemons.Clear();
                    EndFight.Visibility = Visibility.Visible;
                    return;
                }
            }
        }

        private void End_Fight_Click(object sender, RoutedEventArgs e)
        {
            foreach (Pokemon playerPokemon in _gameSession.CurrentPlayer.PokemonCollection)
            {
                if (playerPokemon.CurHp == playerPokemon.MaxHp) { continue; }
                GenFunctions.BattleStatsGenerator(playerPokemon);
            }

            PlayerCorner.Visibility = Visibility.Hidden;
            EnemyCorner.Visibility = Visibility.Hidden;
            EndFight.Visibility = Visibility.Hidden;
            _gameSession.CurrentPlayer.ChosenPokemon = null;
            _trainer = "";
            MenuBar.IsEnabled = true;
            FightStatus.Document.Blocks.Clear();
        }

        private void Buy_Items_Click(object sender, RoutedEventArgs e)
        {
            BuyItems buyItems = new BuyItems();
            buyItems.Owner = this;
            buyItems.DataContext = _gameSession;
            buyItems.Show();
        }

        private void Item_Use_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (EnemyCorner.Visibility == Visibility.Visible) { _gameSession.ItemEffect(btn.Content.ToString()); }
            if (_gameSession.EnemyPokemon.CurHp == 0) { 
                EndFight.Visibility = Visibility.Visible;
                _ids.Clear(); _levels.Clear(); _enemyPokemons.Clear();
            }
        }

        private void RaiseBattleMessages(object sender, GameMessageEventArgs e)
        {
            FightStatus.Document.Blocks.Add(new Paragraph(new Run(e.Message)));
            FightStatus.ScrollToEnd();
        }

        private void WildPokemon_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            Pokemon pokemon;
            double prob = rnd.NextDouble();
            id = rnd.Next(1, _gameSession.AllPokemon.Count + 1);
            pokemon = _gameSession.AllPokemon.First(p => p.Id == id);
            int level;

            if (
                (pokemon.FindType.Equals("basic") && prob >= ((double)10 / 187.5)) ||
                (pokemon.FindType.Equals("rare") && prob >= (6.75 / 187.5) && prob < ((double)10 / 187.5)) ||
                (pokemon.FindType.Equals("legendary") && prob >= (3.33 / 187.5) && prob < (6.75 / 187.5)) ||
                (pokemon.FindType.Equals("ultra beast") && prob >= (1.25 / 187.5) && prob < (3.33 / 187.5))
            )
            {
                level = rnd.Next(pokemon.BaseLevel, pokemon.EvolutionLevel);
            }
            else
            {
                List<Pokemon> backup = _gameSession.AllPokemon.FindAll(p => p.FindType.Equals("basic"));
                pokemon = backup[rnd.Next(0, backup.Count)];
                id = pokemon.Id;
                level = rnd.Next(pokemon.BaseLevel, pokemon.EvolutionLevel);
            }

            _ids.Add(id); _levels.Add(level);
            _trainer = "";
            InitializeOpponent("Wild");
            FightStatus.Document.Blocks.Add(new Paragraph(new Run($"A wild {_gameSession.EnemyPokemon.Name} appeared!!")));
        }
    }
}
