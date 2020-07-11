using GameConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for BuyItems.xaml
    /// </summary>
    public partial class BuyItems : Window
    {
        public GameSession Session => DataContext as GameSession;

        Regex regExp = new Regex(@"\D");
        Item item = new Item();

        public BuyItems()
        {
            InitializeComponent();

            InventoryItems.Items.Insert(0, "Poké Ball");
            InventoryItems.Items.Insert(1, "Great Ball");
            InventoryItems.Items.Insert(2, "Ultra Ball");

            InventoryItems.Items.Insert(3, "Potion");
            InventoryItems.Items.Insert(4, "Super Potion");
            InventoryItems.Items.Insert(5, "Hyper Potion");
            InventoryItems.Items.Insert(6, "Max Potion");
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        { 
            if (InventoryItems.SelectedIndex < 0)
            {
                MessageBox.Show("Select an item to buy");
            }
            else
            {
                if (Quantity.Text.Equals("") || Quantity.Text.Equals("0") || regExp.IsMatch(Quantity.Text))
                {
                    MessageBox.Show("Not a valid quantity");
                }
                else
                {
                    item = ItemFactory.GenerateItem(InventoryItems.SelectedIndex + 1, Convert.ToInt32(Quantity.Text));

                    if (item.Quantity * item.Price > Session.CurrentPlayer.Money) 
                    {
                        MessageBox.Show("Not enough money to buy item");
                    }
                    else
                    {
                        Session.CurrentPlayer.AddItemInventory(item);
                        Session.CurrentPlayer.Money -= (item.Quantity * item.Price);
                    }
                }
            }
        }

        private void Price_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (InventoryItems.SelectedIndex >= 0 && !regExp.IsMatch(e.Text))
            {
                if ((Quantity.Text + e.Text).Length > 2)
                {
                    MessageBox.Show("Quantity can't exceed 1000");
                    Quantity.Text = Quantity.Text.Substring(0, 2);
                    return;
                }

                item.Quantity = Convert.ToInt32(Quantity.Text + e.Text);
                Price.Content = $"{item.Price * item.Quantity}¥";
            }
            else
            {
                item.Quantity = 0;
                Price.Content = "0¥";
            }
            
        }

        private void Description_Display(object sender, SelectionChangedEventArgs e)
        {
            if (InventoryItems.SelectedIndex >= 0)
            {
                item = ItemFactory.GenerateItem(InventoryItems.SelectedIndex + 1, 0);
                Description.Content = item.Description;
            }
            else { Description.Content = ""; }
        }
    }
}
