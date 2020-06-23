using System;
using System.Collections.Generic;
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
    /// Interaction logic for Pokedex.xaml
    /// </summary>
    public partial class Pokedex : Window
    {
        public Pokedex()
        {
            InitializeComponent();
        }
        private void PokedexClose_Click(object sender, RoutedEventArgs e) { Close(); }
    }
}
