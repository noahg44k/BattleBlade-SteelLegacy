using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static BattleBlade_SteelLegacy.Classes.Item;

namespace BattleBlade_SteelLegacy.Classes
{
    public static class ItemManager
    {
        public static List<Item> items = new List<Item>();
        private static int prevID = 0;
        /*
        public int minStr = 1;
        public int minLuc = 1;
        public int minVig = 1;
        public int minSpd = 1;
        public int minInl = 1;
        public int minPrc = 1;

        public int sclStr = 1;
        public int sclLuc = 1;
        public int sclVig = 1;
        public int sclSpd = 1;
        public int sclInl = 1;
        public int sclPrc = 1;
        */

        /// <summary>
        /// Returns a COPY of the passed item
        /// </summary>
        /// <param name="name">Item name</param>
        /// <returns></returns>
        public static Item getItem(string name)
        {
            Item? foundItem = items.Find(item => item.name.Equals(name, StringComparison.OrdinalIgnoreCase));
            Item copy = new Item();
            if(foundItem == null)
            {
                Text.Error($"Could not find item of name: {name}");
                Text.Continue();
                return null;
            }
            switch (foundItem.use)
            {
                case Use.Heal:
                    copy = new Item(foundItem);
                    break;
                case Use.Wear:
                    copy = new Armor((Armor)foundItem);
                    break;
                case Use.Hold:
                    copy = new Weapon((Weapon)foundItem);
                    break;
            }
            return copy;
        }
        /// <summary>
        /// Returns the passed item
        /// </summary>
        /// <param name="name">Item name</param>
        /// <returns></returns>
        public static Item GetRealItem(string name)
        {
            Item? foundItem = items.Find(item => item.name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (foundItem != null)
            {
                return foundItem;
            }
            else
            {
                Text.Error($"Could not find item of name: {name}");
                Text.Continue();
                return null;
            }
        }
        public static Item getRandomItem()
        {
            Random rand = new Random();
            return items[rand.Next(0, items.Count)];
        }

        public static void SetDefaultStats(Weapon newWeapon)
        {
            newWeapon.minStats.Add(Stat.StatName.Strength, 1);
            newWeapon.minStats.Add(Stat.StatName.Luck, 1);
            newWeapon.minStats.Add(Stat.StatName.Vigor, 1);
            newWeapon.minStats.Add(Stat.StatName.Speed, 1);
            newWeapon.minStats.Add(Stat.StatName.Intelligence, 1);
            newWeapon.minStats.Add(Stat.StatName.Precision, 1);

            newWeapon.sclStats.Add(Stat.StatName.Strength, 1);
            newWeapon.sclStats.Add(Stat.StatName.Luck, 1);
            newWeapon.sclStats.Add(Stat.StatName.Vigor, 1);
            newWeapon.sclStats.Add(Stat.StatName.Speed, 1);
            newWeapon.sclStats.Add(Stat.StatName.Intelligence, 1);
            newWeapon.sclStats.Add(Stat.StatName.Precision, 1);
        }
        public static void SetDefaultStats(Armor newArmor)
        {
            newArmor.minStats.Add(Stat.StatName.Strength, 1);
            newArmor.minStats.Add(Stat.StatName.Luck, 1);
            newArmor.minStats.Add(Stat.StatName.Vigor, 1);
            newArmor.minStats.Add(Stat.StatName.Speed, 1);
            newArmor.minStats.Add(Stat.StatName.Intelligence, 1);
            newArmor.minStats.Add(Stat.StatName.Precision, 1);

            newArmor.sclStats.Add(Stat.StatName.Strength, 1);
            newArmor.sclStats.Add(Stat.StatName.Luck, 1);
            newArmor.sclStats.Add(Stat.StatName.Vigor, 1);
            newArmor.sclStats.Add(Stat.StatName.Speed, 1);
            newArmor.sclStats.Add(Stat.StatName.Intelligence, 1);
            newArmor.sclStats.Add(Stat.StatName.Precision, 1);
        }
    
        /// <summary>
        /// Removes all duplicates of passed item name, if there are none then nothing happens
        /// </summary>
        /// <param name="name">Name of item to remove duplicates for</param>
        public static void RemoveDuplicateItem(string name)
        {
            foreach (Item item in items.Where(item => item.name == name).ToList())
            {
                items.Remove(item);
                //Console.WriteLine("Removed item with duplicate name {0}", name);
            }
        }

        /// <summary>
        /// Adds passed item to items list
        /// </summary>
        /// <param name="name">Item to add to items list</param>
        public static void AddNewItemToList(Item newItem)
        {
            items.Add(newItem);
            //Console.WriteLine("Item added to items with name {0}", newItem.name + " and item ID " + newItem.ID);
        }
        public static void AddAllItemsInStage(int stageNum)
        {
            Game.player.stageItemsAvailable.AddRange(items.FindAll(item => item.stage == stageNum));
        }

        /*
        public static void BuildItemDexFromJson(string jsonFilePath)
        {
            string json = File.ReadAllText(jsonFilePath);
            List<Item> itemsFromJson = JsonConvert.DeserializeObject<List<Item>>(json);

            foreach (var item in itemsFromJson)
            {
                items.Add(item);
                Console.WriteLine("Item added to items with name {0}", item.name + " and item ID " + item.ID);
            }
        }*/

        public static void buildItemDex()
        {
            buildWeaponDex();
            buildArmorDex();
            buildConsumableDex();
        }
        public static void buildWeaponDex()
        {
            //WEAPONS
            new Weapon(
                name: "Fist",
                dmg: 2, 
                heal: -1f,
                stage: 1,
                minStatNames: new List<Stat.StatName> { Stat.StatName.Strength },
                minStatValues: new List<int> { 10 },
                scaleStatNames: new List<Stat.StatName> { Stat.StatName.Strength },
                scaleStatValues: new List<int> { 5 },
                energyConsumption: 1,
                discoverChance: 1);// -- not yet implemented

            new Weapon(
                name: "Sword",
                dmg: 8,
                heal: -3.5f,
                stage: 1,
                minStatNames: new List<Stat.StatName> { Stat.StatName.Strength },
                minStatValues: new List<int>() { 5 },
                scaleStatNames: new List<Stat.StatName>() { Stat.StatName.Strength },
                scaleStatValues: new List<int>() { 5 },
                energyConsumption: 1);

            new Weapon("Knife",
                3, -1, 1,
                (new List<Stat.StatName> { Stat.StatName.Speed }),
                (new List<int>() { 3 }),
                new List<Stat.StatName>() { Stat.StatName.Speed },
                new List<int>() { 3 },
                1);

            new Weapon("Dagger", 5f, -1.5f, 1,
                (new List<Stat.StatName> { Stat.StatName.Speed }),
                (new List<int>() { 5 }),
                new List<Stat.StatName>() { Stat.StatName.Speed },
                new List<int>() { 5 },
                1,
                0.8f);

            new Weapon("Short Bow", 10, -5, 1,
                (new List<Stat.StatName> { Stat.StatName.Precision, Stat.StatName.Strength }),
                (new List<int>() { 6, 4 }),
                new List<Stat.StatName>() { Stat.StatName.Precision, Stat.StatName.Strength },
                new List<int>() { 6, 3 },
                1,
                0.7f);

            new Weapon("Hatchet",
                8f, -2, 1,
                (new List<Stat.StatName> { Stat.StatName.Strength, Stat.StatName.Speed }),
                (new List<int>() { 3, 3 }),
                new List<Stat.StatName>() { Stat.StatName.Strength, Stat.StatName.Speed },
                new List<int>() { 3, 3 },
                1,
                0.8f);

            new Weapon("Great Sword",
                15, -8.5f, 1,
                (new List<Stat.StatName> { Stat.StatName.Strength }),
                (new List<int>() { 12 }),
                new List<Stat.StatName>() { Stat.StatName.Strength },
                new List<int>() { 7 },
                2);


            //STAGE 2
            new Weapon("Crossbow",
                14, -8, 2,
                (new List<Stat.StatName> { Stat.StatName.Precision, Stat.StatName.Strength }),
                (new List<int>() { 6, 5 }),
                new List<Stat.StatName>() { Stat.StatName.Precision, Stat.StatName.Strength },
                new List<int>() { 5, 2 },
                1,
                0.85f);

            new Weapon("Spear",
                13, -4, 2,
                (new List<Stat.StatName> { Stat.StatName.Strength, Stat.StatName.Precision }),
                (new List<int>() { 6, 5 }),
                new List<Stat.StatName>() { Stat.StatName.Strength, Stat.StatName.Precision },
                new List<int>() { 5, 4 },
                1);

            new Weapon("Warhammer",
                11.5f, -2, 2,
                (new List<Stat.StatName> { Stat.StatName.Strength }),
                (new List<int>() { 5 }),
                new List<Stat.StatName>() { Stat.StatName.Strength },
                new List<int>() { 4 },
                1);

            new Weapon("War Axe",
                15.5f, -4, 2,
                (new List<Stat.StatName> { Stat.StatName.Strength }),
                (new List<int>() { 12 }),
                new List<Stat.StatName>() { Stat.StatName.Strength },
                new List<int>() { 9 },
                2,
                0.8f);

            new Weapon("Club",
                12, -5, 2,
                (new List<Stat.StatName> { Stat.StatName.Strength }),
                (new List<int>() { 10 }),
                new List<Stat.StatName>() { Stat.StatName.Strength },
                new List<int>() { 9 },
                2,
                0.7f);


            //STAGE 3
            new Weapon("The Big One",
                120, -40, 3,
                (new List<Stat.StatName> { Stat.StatName.Strength }),
                (new List<int>() { 20 }),
                new List<Stat.StatName>() { Stat.StatName.Strength },
                new List<int>() { 8 },
                2,
                0.3f);

            new Weapon("Life Staff",
                18, 100, 3);

            new Weapon("Death Staff",
                200, -100, 3);

            new Weapon("Dragon Club",
                212, -60, 3);


            //STAGE 4
            new Weapon("Electric Staff",
                30, -6, 4,
                (new List<Stat.StatName> { Stat.StatName.Intelligence }),
                (new List<int>() { 10 }),
                new List<Stat.StatName>() { Stat.StatName.Intelligence },
                new List<int>() { 8 });

            new Weapon("Light Staff",
                5, -2, 4,
                (new List<Stat.StatName> { Stat.StatName.Intelligence }),
                (new List<int>() { 7 }),
                new List<Stat.StatName>() { Stat.StatName.Intelligence },
                new List<int>() { 6 });

            new Weapon("Dark Staff",
                20, -2, 4,
                (new List<Stat.StatName> { Stat.StatName.Intelligence }),
                (new List<int>() { 7 }),
                new List<Stat.StatName>() { Stat.StatName.Intelligence },
                new List<int>() { 6 });

            new Weapon("Poison Staff",
                29, -20, 4,
                (new List<Stat.StatName> { Stat.StatName.Intelligence }),
                (new List<int>() { 5 }),
                new List<Stat.StatName>() { Stat.StatName.Intelligence },
                new List<int>() { 5 });

            new Weapon("Trident",
                26, -20, 4,
                (new List<Stat.StatName> { Stat.StatName.Strength, Stat.StatName.Precision }),
                (new List<int>() { 15, 13 }),
                new List<Stat.StatName>() { Stat.StatName.Strength, Stat.StatName.Precision },
                new List<int>() { 8, 7 },
                2,
                0.3f);


            //STAGE 5
            new Weapon("Fire Staff", 72, -10, 5);
            new Weapon("Earth Staff", 66, -15, 5);
            new Weapon("Battle Axe", 55, -5, 5);
            new Weapon("Demon Blade", 88, -40, 5);//RARE
            new Weapon("Great Zweihander", 61, -10, 5);


            //STAGE 6
            new Weapon("Elder Staff", 700, 0, 6);
            new Weapon("Elder Blade", 2500, 0, 6);//LEGENDARY
            new Weapon("Elder Bow", 650, 0, 6);
            new Weapon("Fist", 1, 0, 6);

            /*
             * NAME
             * DMG, HEAL, STAGE
             * REQUIRED SKILL
             * REQUIRED SKILL LVL
             * SCALING SKILL
             * SCALING SKILL AMOUNT
             * ENERGY CONSUMPTION
             * DISCOVER CHANCE
             */
        }
        public static void buildArmorDex()
        {
            /*
             *name
             *stage 
             *AR
             *minStatNames
             *minStatValuesList<Stat.StatName> 
             *scaleStatNames = null, List<int> 
             *scaleStatValues
             *energyConsumption 
             *discoverChance
             */

            //ARMOR
            new Armor(
                name: "Nakey", 
                stage: 0, 
                AR: 0);

            new Armor("Wolfskin Armor", 15, 1);
            new Armor("Light Armor", 16, 1);
            new Armor("Orcskin Armor", 23, 1);
            new Armor("Ent Armor", 18, 1);

            new Armor("Bone Armor", 31, 2);
            new Armor("Leather Armor", 34, 2);
            new Armor("Night Armor", 33, 2);

            new Armor("Sea Dweller Armor", 39, 3);
            new Armor("Shell Armor", 46, 3);

            new Armor("Hell Armor", 58, 4);
            new Armor("Chain Armor", 65, 4);
            new Armor("Inferno Armor", 97, 4);//LEGENDARY

            new Armor("Steel Armor", 76, 5);
            new Armor("Dragonian Armor", 99, 5);//LEGENDARY
            new Armor("Magic Armor", 84, 5);

            new Armor("Elder Armor", 93, 6);
            /*
             *name
             *stage 
             *AR
             *minStatNames
             *minStatValuesList<Stat.StatName> 
             *scaleStatNames = null, List<int> 
             *scaleStatValues
             *energyConsumption 
             *discoverChance
             */
        }
        public static void buildConsumableDex()
        {
            /*
             * name
             * heal
             * stage
             * float discoverChance
             */
            new Item("Health Potion", 55, 7, 5);
            new Item("Large Health Potion", 100, 7, 3);
            new Item("Wolf Meat", 20, 1, 10);
            new Item("Orc Meat", 15, 1, 10);
            new Item("Spider Meat", 10, 2, 10);
            new Item("Bone Meat", 45, 2, 10);
            new Item("Sea Dweller Meat", 28, 3, 10);
            new Item("Hell Meat", 2, 4, 10);
            new Item("Shell Meat", 29, 3, 10);
            new Item("Ent Meat", 30, 1, 10);
            new Item("Elder Meat", 125, 6, 10);
            new Item("Chain Meal", 10, 4, 10);
            new Item("Steel Meal", 5, 4, 10);
            new Item("Dragon Meat", 120, 5, 10);
            new Item("Hippogriff Meat", 125, 5, 10);
            new Item("Wyrm Meat", 4000, 5, 10);//LEGENDARY
        }
    }
}