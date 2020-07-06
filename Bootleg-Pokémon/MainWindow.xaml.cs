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
            _gameSession.IsBattle = false;
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
            _gameSession.IsBattle = true;
            _gameSession.EnemyPokemon = _gameSession.AllPokemon.First(p => p.Id == 74);
            _gameSession.GeneratePokemonStats(_gameSession.EnemyPokemon);
            EnemyCorner.Visibility = Visibility.Visible;
            MenuBar.IsEnabled = false;
            FightStatus.Document.Blocks.Add(new Paragraph(new Run($"Brock chose {_gameSession.EnemyPokemon.Name}")));
        }

        private void Pokemon_Choose_Click(object sender, RoutedEventArgs e)
        {
            if (PlayerPokemon.SelectedItem != null && _gameSession.IsBattle)
            {
                _gameSession.CurrentPlayer.ChosenPokemon = PlayerPokemon.SelectedItem as Pokemon;
                _gameSession.CurrentPlayer.ChosenPokemon.Moves = _gameSession.CurrentPlayer.PokemonCollection[PlayerPokemon.SelectedIndex].Moves;
                PlayerCorner.Visibility = Visibility.Visible;
            }
        }
    }
}
