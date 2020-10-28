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
using System.Xml.Schema;

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
            this.btn_save.IsEnabled = false;
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
            this.btn_save.IsEnabled = true;
        }
        
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Save Files|*.dat|All Files|*.*";

            if (openFile.ShowDialog() == true)
            {
                string fileContent = File.ReadAllText(openFile.FileName);
                fileContent = Cryptography.DecryptStringAES(fileContent, "PokémonYeeeehaaaa");
                var saveObject = JsonConvert.DeserializeObject<SaveObject>(fileContent);

                // Init Game from Save
                _gameSession.CurrentPlayer = saveObject.CurrentPlayer;
                _gameSession.IsGameCreated = true;
                this.btn_save.IsEnabled = true;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Save Files|*.dat";

            if (saveFile.ShowDialog() == true)
            {
                SaveObject saveObject = new SaveObject()
                {
                    CurrentPlayer = _gameSession.CurrentPlayer
                };

                string saveObjectJson = JsonConvert.SerializeObject(saveObject);
                saveObjectJson = Cryptography.EncryptStringAES(saveObjectJson, "PokémonYeeeehaaaa");

                File.WriteAllText(saveFile.FileName, saveObjectJson);
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
                    Pokemon pokemon = _gameSession.AllPokemon.First(p => p.Id == _ids[i]).Clone();
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

        private void GenerateTrainerBattle(List<int> ids, List<int> levels, string trainer)
        {
            for (int i = 0; i < ids.Count; i++)
            {
                _ids.Add(ids[i]); _levels.Add(levels[i]);
            }
            InitializeOpponent("Trainer");
            _trainer = trainer;

            if (_gameSession.EnemyPokemon.Name != null)
            {
                FightStatus.Document.Blocks.Add(new Paragraph(new Run($"Red chose {_gameSession.EnemyPokemon.Name}")));
            }

            SetHpColor(_gameSession.EnemyPokemon, false);
        }

        private void Red_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 4 }, new List<int>() { 5 }, ""); }

        private void Blue_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 7 }, new List<int>() { 5 }, ""); }

        private void Green_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 1 }, new List<int>() { 5 }, ""); }

        private void Yellow_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 25 }, new List<int>() { 5 }, ""); }

        private void Gold_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 152 }, new List<int>() { 10 }, ""); }

        private void Silver_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 155 }, new List<int>() { 10 }, ""); }

        private void Crystal_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 158 }, new List<int>() { 10 }, ""); }

        private void Ruby_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 256 }, new List<int>() { 25 }, ""); }
        
        private void Sapphire_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 259 }, new List<int>() { 25 }, ""); }
        
        private void Emerald_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 253 }, new List<int>() { 25 }, ""); }
        
        private void Oak_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 128, 103, 59, 9, 130 }, new List<int>() { 66, 67, 68, 69, 70 }, ""); }
        
        private void Elm_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 152, 155, 158 }, new List<int>() { 66, 66, 66 }, ""); }
        
        private void Birch_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 252, 255, 258 }, new List<int>() { 66, 66, 66 }, ""); }
        
        private void Jesse_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 24 }, new List<int>() { 67 }, ""); }
        
        private void James_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 110 }, new List<int>() { 67 }, ""); }

        private void Giovanni_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 51, 34, 31, 464, 150 }, new List<int>() { 68, 68, 68, 68, 70 }, ""); }

        private void Brock_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 74, 95 }, new List<int>() { 12, 14 }, "Brock"); }

        private void Misty_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 120, 121 }, new List<int>() { 18, 21 }, "Misty"); }

        private void Surge_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 100, 25, 26 }, new List<int>() { 21, 18, 24 }, "Surge"); }

        private void Erika_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 71, 114, 45 }, new List<int>() { 29, 24, 29 }, "Erika"); }

        private void Janine_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 169, 110, 110, 168, 49 }, new List<int>() { 36, 36, 36, 33, 39 }, "Janine"); }

        private void Sabrina_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 64, 122, 49, 65 }, new List<int>() { 38, 37, 38, 43 }, "Sabrina"); }

        private void Blaine_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 58, 77, 78, 59 }, new List<int>() { 42, 40, 42, 47 }, "Blaine"); }

        private void Gary_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 18, 65, 112, 103, 130, 6 }, new List<int>() { 61, 59, 61, 61, 63, 65 }, "Gary"); }

        private void Elite_Four_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 178, 103, 80, 124, 178 }, new List<int>() { 40, 42, 42, 41, 40 }, "Will"); }

        private void Falkner_Click(object sender, RoutedEventArgs e) { GenerateTrainerBattle(new List<int>() { 16, 17 }, new List<int>() { 7, 9 }, "Falkner"); }

        private void Pokemon_Choose_Click(object sender, RoutedEventArgs e)
        {
            if(PlayerPokemon.SelectedItem != null)
            {
                Player curPlayer = _gameSession.CurrentPlayer;
                curPlayer.ChosenPokemon = PlayerPokemon.SelectedItem as Pokemon;

                if (Evolve.IsChecked)
                {
                    if(curPlayer.ChosenPokemon.CurLevel > curPlayer.ChosenPokemon.EvolutionLevel)
                    {
                        Pokemon toEvolve = curPlayer.PokemonCollection[PlayerPokemon.SelectedIndex];
                        Pokemon evolved = _gameSession.AllPokemon.First(p => p.Id == toEvolve.EvolutionId[rnd.Next(0, toEvolve.EvolutionId.Length)]);
                        MessageBox.Show($"Your {toEvolve.Name} evolved to a {evolved.Name}!!");
                        toEvolve.Id = evolved.Id;
                        toEvolve.Name = evolved.Name;
                        toEvolve.Image = evolved.Image;
                        toEvolve.FindType = evolved.FindType;
                        toEvolve.Type = evolved.Type;
                        toEvolve.BaseLevel = evolved.BaseLevel;
                        toEvolve.EvolutionLevel = evolved.EvolutionLevel;
                        toEvolve.EvolutionId = evolved.EvolutionId;
                        toEvolve.CatchRate = evolved.CatchRate;
                        toEvolve.Base.HP.Insert(0, evolved.Base.HP[0]);
                        toEvolve.Base.Attack.Insert(0, evolved.Base.Attack[0]);
                        toEvolve.Base.Defense.Insert(0, evolved.Base.Defense[0]);
                        toEvolve.Base.SpecialAttack.Insert(0, evolved.Base.SpecialAttack[0]);
                        toEvolve.Base.SpecialDefense.Insert(0, evolved.Base.SpecialDefense[0]);
                        toEvolve.Base.Speed.Insert(0, evolved.Base.Speed[0]);
                    }
                    else if (curPlayer.ChosenPokemon.EvolutionLevel == 100)
                    {
                        MessageBox.Show("This Pokémon cannot evolve");
                    }
                    else
                    {
                        MessageBox.Show($"Your {curPlayer.ChosenPokemon.Name} still needs {curPlayer.ChosenPokemon.EvolutionLevel - curPlayer.ChosenPokemon.CurLevel} level(s) to evolve.");
                    }
                }
                else if (Release.IsChecked)
                {
                    if(curPlayer.PokemonCollection.Count > 1) 
                    { 
                        curPlayer.PokemonCollection.RemoveAt(PlayerPokemon.SelectedIndex);
                        MessageBox.Show($"You let your {curPlayer.ChosenPokemon.Name} into the wild");
                    }
                    else { MessageBox.Show("You need to have at least one Pokémon"); }

                    curPlayer.ChosenPokemon = null;
                    Release.IsChecked = false;
                    return;
                }

                if (_gameSession.IsBattle && EnemyCorner.Visibility == Visibility.Visible)
                {
                    SetHpColor(_gameSession.CurrentPlayer.ChosenPokemon, false);
                    curPlayer.ChosenPokemon.Moves = curPlayer.PokemonCollection[PlayerPokemon.SelectedIndex].Moves;
                    PlayerCorner.Visibility = Visibility.Visible;
                    FightStatus.Document.Blocks.Add(new Paragraph(new Run($"You chose {curPlayer.ChosenPokemon.Name}")));                    
                }
                else
                {
                    MessageBox.Show($"{curPlayer.ChosenPokemon.Name}: Level {curPlayer.ChosenPokemon.CurLevel}");
                    curPlayer.ChosenPokemon = null;
                }
            }
        }

        public Move PlayerMoveSelected => _gameSession.CurrentPlayer.ChosenPokemon.Moves.FirstOrDefault(m => m.IsSelected);

        private void Move_Click(object sender, RoutedEventArgs e)
        {
            bool triedToFlee = false;
            if(_gameSession.IsBattle && _gameSession.CurrentPlayer.TryToRun && _gameSession.EnemyPokemon.Category.Equals("Wild"))
			{
                if(_gameSession.TryToFlee())
				{
                    _ids.Clear(); _levels.Clear(); _enemyPokemons.Clear();
                    EndFight.Visibility = Visibility.Visible;
                }
                triedToFlee = true;
			}

            if (_gameSession.IsBattle && (PlayerMoveSelected != null || triedToFlee))
            {
                if(!triedToFlee)
				{
                    _gameSession.MoveOutcome(PlayerMoveSelected, _gameSession.CurrentPlayer.ChosenPokemon, _gameSession.EnemyPokemon);

                    if (_gameSession.EnemyPokemon.CurHp <= 0)
                    {
                        SetHpColor(_gameSession.EnemyPokemon, true);
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
                                    if (!_gameSession.CurrentPlayer.BadgeCollection.Any(b => b.Equals("Surge")))
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

                                    case "Falkner":
                                    if (!_gameSession.CurrentPlayer.BadgeCollection.Any(b => b.Equals("Falkner")))
                                    {
                                        MessageBox.Show("You earned the ZephyrBadge");
                                        _gameSession.CurrentPlayer.BadgeCollection.Add("Falkner");
                                        Bugsy.IsEnabled = true;
                                    }
                                    return;

                                    case "Will":
                                    MessageBox.Show("You beat Will!! Your Next Opponent is Koga");
                                    EndFight.Visibility = Visibility.Hidden;
                                    EnemyCorner.Visibility = Visibility.Hidden;
                                    PlayerCorner.Visibility = Visibility.Hidden;
                                    GenerateTrainerBattle(new List<int>() { 168, 205, 89, 49, 169 }, new List<int>() { 41, 43, 43, 42, 40 }, "Koga");
                                    return;

                                    case "Koga":
                                    MessageBox.Show("You beat Koga!! Your Next Opponent is Bruno");
                                    EndFight.Visibility = Visibility.Hidden;
                                    EnemyCorner.Visibility = Visibility.Hidden;
                                    PlayerCorner.Visibility = Visibility.Hidden;
                                    GenerateTrainerBattle(new List<int>() { 237, 107, 106, 95, 68 }, new List<int>() { 43, 43, 43, 44, 44 }, "Bruno");
                                    return;

                                    case "Bruno":
                                    MessageBox.Show("You beat Bruno!! Your Next Opponent is Karen");
                                    EndFight.Visibility = Visibility.Hidden;
                                    EnemyCorner.Visibility = Visibility.Hidden;
                                    PlayerCorner.Visibility = Visibility.Hidden;
                                    GenerateTrainerBattle(new List<int>() { 197, 45, 198, 94, 229 }, new List<int>() { 49, 45, 41, 45, 44 }, "Karen");
                                    return;

                                    case "Karen":
                                    MessageBox.Show("You have proven your worth against the G/S/C Elite Four. It's time for you to fight the G/S/C Elite Four Champion: Lance!");
                                    EndFight.Visibility = Visibility.Hidden;
                                    EnemyCorner.Visibility = Visibility.Hidden;
                                    PlayerCorner.Visibility = Visibility.Hidden;
                                    GenerateTrainerBattle(new List<int>() { 117, 142, 130, 6, 149 }, new List<int>() { 43, 49, 52, 50, 55 }, "Lance");
                                    return;

                                    case "Lance":
                                    if (!_gameSession.CurrentPlayer.BadgeCollection.Any(b => b.Equals("Lance")))
                                    {
                                        MessageBox.Show("You beat the G/S/C Elite Champion!! You received a Master Ball");
                                        EndFight.Visibility = Visibility.Visible;
                                        _gameSession.CurrentPlayer.AddItemInventory(ItemFactory.GenerateItem(8, 1));
                                        MessageBox.Show("A Master Ball can catch any wild pokémon with any HP. Use it wisely");
                                    }
                                    return;

                                    default:
                                    return;

                                }

                            }
                            return;
                        }

                        InitializeOpponent(_gameSession.EnemyPokemon.Category);
                        return;
                    }
                }

                _gameSession.MoveOutcome(_gameSession.EnemyPokemon.Moves[rnd.Next(0, 4)], _gameSession.EnemyPokemon, _gameSession.CurrentPlayer.ChosenPokemon);

                if (_gameSession.CurrentPlayer.ChosenPokemon.CurHp <= 0)
                {
                    SetHpColor(_gameSession.CurrentPlayer.ChosenPokemon, true);
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
            SetHpColor(_gameSession.EnemyPokemon, false);
            SetHpColor(_gameSession.CurrentPlayer.ChosenPokemon, false);
            FightStatus.Document.Blocks.Add(new Paragraph(new Run(e.Message)));
            FightStatus.ScrollToEnd();
        }

        private void WildPokemon_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            int id = 0;
            Pokemon pokemon;
            IEnumerable<Pokemon> pokemonList;
            int avgpartylvl = 0;
            double prob = rnd.NextDouble();

            //Gathering average level of the parties level so we can more accurately have wild pokemon appear.
            foreach (var poke in _gameSession.CurrentPlayer.PokemonCollection)
            {
                poke.CurLevel += avgpartylvl;
            }
            avgpartylvl /= _gameSession.CurrentPlayer.PokemonCollection.Count;
            pokemonList = _gameSession.AllPokemon.Where(p => Math.Abs(avgpartylvl - p.BaseLevel) < 20);
            index = rnd.Next(1, pokemonList.Count() -1);
            pokemon = pokemonList.ElementAt(index);

            int level;

            int maxlevel = pokemon.EvolutionLevel.CompareTo(avgpartylvl + 20);
            int minLevel = Math.Min(pokemon.BaseLevel, maxlevel);
            if (maxlevel == 1)
            {
                maxlevel = avgpartylvl + 20;
            }
            else
            {
                maxlevel = pokemon.EvolutionLevel;
            }

            if (
                (pokemon.FindType.Equals("basic") && prob >= ((double)10 / 187.5)) ||
                (pokemon.FindType.Equals("rare") && prob >= (6.75 / 187.5) && prob < ((double)10 / 187.5)) ||
                (pokemon.FindType.Equals("legendary") && prob >= (3.33 / 187.5) && prob < (6.75 / 187.5)) ||
                (pokemon.FindType.Equals("ultra beast") && prob >= (1.25 / 187.5) && prob < (3.33 / 187.5))
            )
            {
                level  = rnd.Next(minLevel, maxlevel);
            }
            else
            {
                List<Pokemon> backup = _gameSession.AllPokemon.FindAll(p => p.FindType.Equals("basic"));
                pokemon = backup[rnd.Next(0, backup.Count)];
                level = rnd.Next(minLevel, maxlevel);
            }

            _ids.Add(pokemon.Id); _levels.Add(level);
            _trainer = "";
            InitializeOpponent("Wild");
            SetHpColor(_gameSession.EnemyPokemon, false);
            FightStatus.Document.Blocks.Add(new Paragraph(new Run($"A wild {_gameSession.EnemyPokemon.Name} appeared!!")));
        }

        private void Evolve_Checked(object sender, RoutedEventArgs e)
        {
            if (Release.IsChecked)
            {
                MessageBox.Show("Turn off Release mode to evolve Pokémon");
                Evolve.IsChecked = false;
            }
            else
            {
                MessageBox.Show("Evolution Mode On. Choose Pokémon to evolve.");
            }
        }

        private void Evolve_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!Release.IsChecked)
            {
                MessageBox.Show("Evolution Mode Cancelled.");
            }
        }

        private void Release_Checked(object sender, RoutedEventArgs e)
        {
            if (!Evolve.IsChecked)
            {
                MessageBox.Show("Choose a Pokémon to let go");
            }
            else
            {
                Release.IsChecked = false;
                MessageBox.Show("Turn off Evolution mode to release a Pokémon");
            }
        }

        private void SetHpColor(Pokemon pkm, bool reset)
        {
            if (reset)
            {
                if (pkm == _gameSession.EnemyPokemon)
                    eProgressBar.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 170, 255));
                else if (pkm == _gameSession.CurrentPlayer.ChosenPokemon)
                    pProgressBar.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 170, 255));
            }
            else if (pkm.CurHpPercent > 50)
            {
                if (pkm == _gameSession.EnemyPokemon)
                    eProgressBar.Foreground = new SolidColorBrush(Color.FromArgb(255, (byte)(2 * (170 - pkm.CurHpPercent * 1.70)), 170, 0));
                else if (pkm == _gameSession.CurrentPlayer.ChosenPokemon)
                    pProgressBar.Foreground = new SolidColorBrush(Color.FromArgb(255, (byte)(2 * (170 - pkm.CurHpPercent * 1.70)), 170, 0));                
            }
            else if (pkm.CurHpPercent <= 50)
            {
                if (pkm == _gameSession.EnemyPokemon)
                {
                    eProgressBar.Foreground = new SolidColorBrush(Color.FromArgb(255, 170, (byte)(2 * pkm.CurHpPercent * 1.70), 0));
                }
                else if (pkm == _gameSession.CurrentPlayer.ChosenPokemon)
                {
                    pProgressBar.Foreground = new SolidColorBrush(Color.FromArgb(255, 170, (byte)(2 * pkm.CurHpPercent * 1.70), 0));
                }
            }
        }

    }
}
