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
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _gameSession;
            _gameSession.IsGameCreated = false;
            using(StreamReader pokedexJson = new StreamReader("..\\..\\..\\pokedex.json"))
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
                _gameSession.Losses = _gameSession.CurrentPlayer.Fights - _gameSession.CurrentPlayer.Wins;

                if (_gameSession.CurrentPlayer.Fights != 0)
                {
                    _gameSession.WinPercentage = Math.Round(Convert.ToDouble(_gameSession.CurrentPlayer.Wins) * 100.0 / Convert.ToDouble(_gameSession.CurrentPlayer.Fights), 2);
                }
                else { _gameSession.WinPercentage = 0.0; }
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

        private void Brock_Click(object sender, RoutedEventArgs e)
        {
            _gameSession.EnemyPokemon = _gameSession.AllPokemon.First(p => p.Id == 74);
            Random rnd = new Random();
            int rndEvSum = 0;
            int[] evVals=new int[6];
            for(int i=0; i<6; ++i)
            {
                if (rndEvSum > 510) { break; }
                evVals[i] = rnd.Next(0, 256);
                rndEvSum += evVals[i];
            }
            int[] stats = StatGenFunctions.BattleStatsGenerator(evVals, _gameSession.EnemyPokemon);
            _gameSession.EnemyPokemon.Moves = StatGenFunctions.MoveList(_gameSession.EnemyPokemon.Type);
            EnemyCurrentHP.Content = stats[0];
            Slash.Visibility = Visibility.Visible;
            EnemyMaxHP.Content = stats[0];
            MenuBar.IsEnabled = false;
        }
    }
}
