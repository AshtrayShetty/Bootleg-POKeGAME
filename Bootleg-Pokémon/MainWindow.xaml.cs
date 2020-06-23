using GameConfig;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
                _gameSession.CurrentPlayer.Fights = Int32.Parse(data[1].Substring(data[1].IndexOf(':') + 2));
                _gameSession.CurrentPlayer.Wins = Int32.Parse(data[2].Substring(data[2].IndexOf(':') + 2));
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
                    sw.WriteLine($"Catches: {_gameSession.CurrentPlayer.Pokemon.Count}");
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e) { Close(); }
    }
}
