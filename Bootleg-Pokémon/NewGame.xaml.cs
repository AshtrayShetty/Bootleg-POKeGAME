using GameConfig;
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
using System.Windows.Shapes;

namespace Bootleg_Pokémon
{
    /// <summary>
    /// Interaction logic for NewGame.xaml
    /// </summary>
    public partial class NewGame : Window
    {
        public GameSession Session => DataContext as GameSession;
        public NewGame()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if(CharName.Text.Trim().Length !=0 || SaveFile.Text.Trim().Length != 0)
            {
                Session.CurrentPlayer.Name = CharName.Text;
                string path = $"..\\..\\..\\SaveFiles\\{SaveFile.Text}.txt";
                try
                {
                    if (File.Exists(path)) { File.Delete(path); }

                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine($"Name: {CharName.Text}");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(Directory.GetCurrentDirectory());
                    throw;
                }
                Close();
                Session.IsGameCreated = true;
                Session.Losses = 0;
                Session.WinPercentage = 0.0;
            }
        }
    }
}
