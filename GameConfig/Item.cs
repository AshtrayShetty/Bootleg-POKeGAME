using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GameConfig
{
    public class Item : INotifyPropertyChanged 
    {
        private int _quantity;
        private int _price;

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public int Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public Item(int id, string name, string desc, int quantity, int price)
        {
            ID = id;
            Name = name;
            Description = desc;
            Quantity = quantity;
            Price = price;
        }

        public Item() { }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
