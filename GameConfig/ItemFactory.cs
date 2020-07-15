using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameConfig
{
    public static class ItemFactory
    {
        public static Item GenerateItem(int id, int quantity)
        {
            Item item;
            switch (id)
            {
                case 1:
                    item = new Item(
                        id,
                        "PokéBall",
                        "A device for catching wild Pokémon.",
                        quantity,
                        200,
                        0,
                        1.0
                    );
                    return item;

                case 2:
                    item = new Item(
                        id,
                        "GreatBall",
                        "A high-performance Ball with a higher catch rate than a standard Poké Ball.",
                        quantity,
                        600,
                        0,
                        1.5
                    );
                    return item;

                case 3:
                    item = new Item(
                        id,
                        "UltraBall",
                        "An ultra-performance Ball with a higher catch rate than a Great Ball.",
                        quantity,
                        1200,
                        0,
                        2.0
                    );
                    return item;


                case 4:
                    item = new Item(
                        id,
                        "Potion",
                        "Restores the HP of a Pokémon by 20 points.",
                        quantity,
                        300,
                        20,
                        0
                    );
                    return item;

                case 5:
                    item = new Item(
                        id,
                        "SuperPotion",
                        "Restores the HP of a Pokémon by 50 points.",
                        quantity,
                        500,
                        50,
                        0
                    );
                    return item;

                case 6:
                    item = new Item(
                        id,
                        "HyperPotion",
                        "Restores the HP of a Pokémon by 200 points.",
                        quantity,
                        1500,
                        200,
                        0
                    );
                    return item;

                case 7:
                    item = new Item(
                        id,
                        "MaxPotion",
                        "Fully restores the HP of a Pokémon.",
                        quantity,
                        2500,
                        0,
                        0
                    );
                    return item;

                case 8:
                    item = new Item(
                        id,
                        "MasterBall",
                        "With this, you will catch any wild Pokémon without fail.",
                        quantity,
                        10000,
                        0,
                        0
                    );
                    return item;

                default:
                    return null;
            }
        }
    }
}
