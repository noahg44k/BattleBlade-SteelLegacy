using BattleBlade_SteelLegacy.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BattleBlade_SteelLegacy.Classes
{
    public class Item
    {
        public string name = "";
        public Use use;
        public int ID = 0;
        private static int prevID = 0;
        public int AR = 0; //ARMOR RATING
        public int energyConsumption = 1;
        public int stage = 1;
        public int stackHeight = 1;
        public int currentStack = 0;
        public float wepDmg = 0f;
        public float heal = 0;
        public float discoverChance = 1;
        public bool equipped = false;
        public static List<Item> items = new List<Item>();

        public Dictionary<Stat.StatName, int> minStats = new Dictionary<Stat.StatName, int>();
        public Dictionary<Stat.StatName, int> sclStats = new Dictionary<Stat.StatName, int>();

        public enum Use
        {
            Heal,
            Hold,
            Wear
        }

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

        public static Item copyItem(Item item)
        {
            return new Item
            {
                name = item.name,
                heal = item.heal,
                wepDmg = item.wepDmg,
                energyConsumption = item.energyConsumption,
                AR = item.AR,
                ID = item.ID,
                use = item.use,
                equipped = item.equipped,
                stage = item.stage,
                stackHeight = item.stackHeight
            };
        }

        public static Item getItem(string name)
        {
            Item foundItem = items.Find(item => item.name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (foundItem != null)
            {
                return foundItem;
            }
            else
            {
                Text.Color($"Could not find item of name: {name}", Text.TC.R);
                return null;
            }
        }

        public static Item getRandomItem()
        {
            Random rand = new Random();
            return items[rand.Next(0, items.Count)];
        }

        private static void SetDefaultStats(Item newItem)
        {
            newItem.minStats.Add(Stat.StatName.Strength, 1);
            newItem.minStats.Add(Stat.StatName.Luck, 1);
            newItem.minStats.Add(Stat.StatName.Vigor, 1);
            newItem.minStats.Add(Stat.StatName.Speed, 1);
            newItem.minStats.Add(Stat.StatName.Intelligence, 1);
            newItem.minStats.Add(Stat.StatName.Precision, 1);

            newItem.sclStats.Add(Stat.StatName.Strength, 1);
            newItem.sclStats.Add(Stat.StatName.Luck, 1);
            newItem.sclStats.Add(Stat.StatName.Vigor, 1);
            newItem.sclStats.Add(Stat.StatName.Speed, 1);
            newItem.sclStats.Add(Stat.StatName.Intelligence, 1);
            newItem.sclStats.Add(Stat.StatName.Precision, 1);
        }

        private static void RemoveDuplicateItem(string name, List<Item> items)
        {
            foreach (Item item in items.Where(item => item.name == name).ToList())
            {
                items.Remove(item);
                Console.WriteLine("Removed item with duplicate name {0}", name);
            }
        }

        private static void AddNewItem(Item newItem, List<Item> items)
        {
            items.Add(newItem);
            Console.WriteLine("Item added to items with name {0}", newItem.name + " and item ID " + newItem.ID);
        }

        public static Item newWeapon(string name, float dmg, float heal, int stage,
            List<Stat.StatName> minStatNames = null, List<int> minStatValues = null,
            List<Stat.StatName> scaleStatNames = null, List<int> scaleStatValues = null,
            int energyConsumption = 1, float discoverChance = 1)
        {
            Item newWeapon = new Item();
            newWeapon.name = name;
            newWeapon.ID = ++prevID;
            newWeapon.wepDmg = dmg;
            newWeapon.heal = heal;
            newWeapon.use = Use.Hold;
            newWeapon.equipped = false;
            newWeapon.stage = stage;
            newWeapon.energyConsumption = energyConsumption;
            newWeapon.discoverChance = discoverChance;

            SetDefaultStats(newWeapon);

            if (minStatNames != null)
            {
                for (int i = 0; i < minStatNames.Count; i++)
                {
                    newWeapon.minStats[minStatNames[i]] = minStatValues[i];
                }
            }

            if (scaleStatNames != null)
            {
                for (int i = 0; i < scaleStatNames.Count; i++)
                {
                    newWeapon.sclStats[scaleStatNames[i]] = scaleStatValues[i];
                }
            }

            RemoveDuplicateItem(name, items);
            AddNewItem(newWeapon, items);

            return newWeapon;
        }
        public static Item newArmor(string name, int stage, int AR,
            List<Stat.StatName> minStatNames = null, List<int> minStatValues = null,
            List<Stat.StatName> scaleStatNames = null, List<int> scaleStatValues = null,
            int energyConsumption = 1, float discoverChance = 1)
        {
            Item newArmor = new Item();
            newArmor.name = name;
            newArmor.AR = AR;
            newArmor.ID = ++prevID;
            newArmor.use = Use.Wear;
            newArmor.equipped = false;
            newArmor.stage = stage;
            newArmor.energyConsumption = energyConsumption;
            newArmor.discoverChance = discoverChance;

            SetDefaultStats(newArmor);

            if (minStatNames != null)
            {
                for (int i = 0; i < minStatNames.Count; i++)
                {
                    newArmor.minStats[minStatNames[i]] = minStatValues[i];
                }
            }

            if (scaleStatNames != null)
            {
                for (int i = 0; i < scaleStatNames.Count; i++)
                {
                    newArmor.sclStats[scaleStatNames[i]] = scaleStatValues[i];
                }
            }

            RemoveDuplicateItem(name, items);
            AddNewItem(newArmor, items);

            return newArmor;
        }
        public static Item newItem(string name, float dmg, float heal, int stage,
            List<Stat.StatName> minStatNames = null, List<int> minStatValues = null,
            List<Stat.StatName> scaleStatNames = null, List<int> scaleStatValues = null,
            int energyConsumption = 1, float discoverChance = 1)
        {
            Item newItem = new Item();
            newItem.name = name;
            newItem.ID = ++prevID;
            newItem.wepDmg = dmg;
            newItem.heal = heal;
            newItem.use = Use.Heal;
            newItem.equipped = false;
            newItem.stage = stage;
            newItem.energyConsumption = energyConsumption;
            newItem.discoverChance = discoverChance;

            SetDefaultStats(newItem);

            if (minStatNames != null)
            {
                for (int i = 0; i < minStatNames.Count; i++)
                {
                    newItem.minStats[minStatNames[i]] = minStatValues[i];
                }
            }

            if (scaleStatNames != null)
            {
                for (int i = 0; i < scaleStatNames.Count; i++)
                {
                    newItem.sclStats[scaleStatNames[i]] = scaleStatValues[i];
                }
            }

            RemoveDuplicateItem(name, items);
            AddNewItem(newItem, items);

            return newItem;
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

        public void buildItemDex()
        {
            //WEAPONS
            //         name   dmg, heal, stg,               required skill              required skill lvl                  scaling skill         scaling skill amount
            newWeapon("Fist", 2, -1f, 1, (new List<Stat.StatName> { Stat.StatName.Strength }), (new List<int>() { 10 }), new List<Stat.StatName>() { Stat.StatName.Strength }, new List<int>() { 5 }, 1);
            newWeapon("Sword", 8, -3.5f, 1, (new List<Stat.StatName> { Stat.StatName.Strength }), (new List<int>() { 5 }), new List<Stat.StatName>() { Stat.StatName.Strength }, new List<int>() { 5 }, 1);
            newWeapon("Knife", 3, -1, 1, (new List<Stat.StatName> { Stat.StatName.Speed }), (new List<int>() { 3 }), new List<Stat.StatName>() { Stat.StatName.Speed }, new List<int>() { 3 }, 1);
            newWeapon("Dagger", 5f, -1.5f, 1, (new List<Stat.StatName> { Stat.StatName.Speed }), (new List<int>() { 5 }), new List<Stat.StatName>() { Stat.StatName.Speed }, new List<int>() { 5 }, 1, 0.8f);
            newWeapon("Short Bow", 10, -5, 1, (new List<Stat.StatName> { Stat.StatName.Precision, Stat.StatName.Strength }), (new List<int>() { 6, 4 }), new List<Stat.StatName>() { Stat.StatName.Precision, Stat.StatName.Strength }, new List<int>() { 6, 3 }, 1, 0.7f);
            newWeapon("Hatchet", 8f, -2, 1, (new List<Stat.StatName> { Stat.StatName.Strength, Stat.StatName.Speed }), (new List<int>() { 3, 3 }), new List<Stat.StatName>() { Stat.StatName.Strength, Stat.StatName.Speed }, new List<int>() { 3, 3 }, 1, 0.8f);
            newWeapon("Great Sword", 15, -8.5f, 1, (new List<Stat.StatName> { Stat.StatName.Strength }), (new List<int>() { 12 }), new List<Stat.StatName>() { Stat.StatName.Strength }, new List<int>() { 7 }, 2);

            newWeapon("Crossbow", 14, -8, 2, (new List<Stat.StatName> { Stat.StatName.Precision, Stat.StatName.Strength }), (new List<int>() { 6, 5 }), new List<Stat.StatName>() { Stat.StatName.Precision, Stat.StatName.Strength }, new List<int>() { 5, 2 }, 1, 0.85f);
            newWeapon("Spear", 13, -4, 2, (new List<Stat.StatName> { Stat.StatName.Strength, Stat.StatName.Precision }), (new List<int>() { 6, 5 }), new List<Stat.StatName>() { Stat.StatName.Strength, Stat.StatName.Precision }, new List<int>() { 5, 4 }, 1);
            newWeapon("Warhammer", 11.5f, -2, 2, (new List<Stat.StatName> { Stat.StatName.Strength }), (new List<int>() { 5 }), new List<Stat.StatName>() { Stat.StatName.Strength }, new List<int>() { 4 }, 1);
            newWeapon("War Axe", 15.5f, -4, 2, (new List<Stat.StatName> { Stat.StatName.Strength }), (new List<int>() { 12 }), new List<Stat.StatName>() { Stat.StatName.Strength }, new List<int>() { 9 }, 2, 0.8f);
            newWeapon("Club", 12, -5, 2, (new List<Stat.StatName> { Stat.StatName.Strength }), (new List<int>() { 10 }), new List<Stat.StatName>() { Stat.StatName.Strength }, new List<int>() { 9 }, 2, 0.7f);

            newWeapon("The Big One", 120, -40, 3, (new List<Stat.StatName> { Stat.StatName.Strength }), (new List<int>() { 20 }), new List<Stat.StatName>() { Stat.StatName.Strength }, new List<int>() { 8 }, 2, 0.3f);
            newWeapon("Life Staff", 18, 100, 3);
            newWeapon("Death Staff", 200, -100, 3);
            newWeapon("Dragon Club", 212, -60, 3);

            newWeapon("Electric Staff", 30, -6, 4, (new List<Stat.StatName> { Stat.StatName.Intelligence }), (new List<int>() { 10 }), new List<Stat.StatName>() { Stat.StatName.Intelligence }, new List<int>() { 8 });
            newWeapon("Light Staff", 5, -2, 4, (new List<Stat.StatName> { Stat.StatName.Intelligence }), (new List<int>() { 7 }), new List<Stat.StatName>() { Stat.StatName.Intelligence }, new List<int>() { 6 });
            newWeapon("Dark Staff", 20, -2, 4, (new List<Stat.StatName> { Stat.StatName.Intelligence }), (new List<int>() { 7 }), new List<Stat.StatName>() { Stat.StatName.Intelligence }, new List<int>() { 6 });
            newWeapon("Poison Staff", 29, -20, 4, (new List<Stat.StatName> { Stat.StatName.Intelligence }), (new List<int>() { 5 }), new List<Stat.StatName>() { Stat.StatName.Intelligence }, new List<int>() { 5 });
            newWeapon("Trident", 26, -20, 4, (new List<Stat.StatName> { Stat.StatName.Strength, Stat.StatName.Precision }), (new List<int>() { 15, 13 }), new List<Stat.StatName>() { Stat.StatName.Strength, Stat.StatName.Precision }, new List<int>() { 8, 7 }, 2, 0.3f);

            newWeapon("Fire Staff", 72, -10, 5);
            newWeapon("Earth Staff", 66, -15, 5);
            newWeapon("Battle Axe", 55, -5, 5);
            newWeapon("Demon Blade", 88, -40, 5);//RARE
            newWeapon("Great Zweihander", 61, -10, 5);

            newWeapon("Elder Staff", 700, 0, 6);
            newWeapon("Elder Blade", 2500, 0, 6);//LEGENDARY
            newWeapon("Elder Bow", 650, 0, 6);
            newWeapon("Fist", 1, 0, 6);

            //ARMOR
            newArmor("Nakey", 0, 0);

            newArmor("Wolfskin Armor", 15, 1);
            newArmor("Light Armor", 16, 1);
            newArmor("Orcskin Armor", 23, 1);
            newArmor("Ent Armor", 18, 1);

            newArmor("Bone Armor", 31, 2);
            newArmor("Leather Armor", 34, 2);
            newArmor("Night Armor", 33, 2);

            newArmor("Sea Dweller Armor", 39, 3);
            newArmor("Shell Armor", 46, 3);

            newArmor("Hell Armor", 58, 4);
            newArmor("Chain Armor", 65, 4);
            newArmor("Inferno Armor", 97, 4);//LEGENDARY

            newArmor("Steel Armor", 76, 5);
            newArmor("Dragonian Armor", 99, 5);//LEGENDARY
            newArmor("Magic Armor", 84, 5);

            newArmor("Elder Armor", 93, 6);

            //ITEMS
            newItem("Health Potion", 55, 7, 5);
            newItem("Large Health Potion", 100, 7, 3);
            newItem("Wolf Meat", 20, 1, 10);
            newItem("Orc Meat", 15, 1, 10);
            newItem("Spider Meat", 10, 2, 10);
            newItem("Bone Meat", 45, 2, 10);
            newItem("Sea Dweller Meat", 28, 3, 10);
            newItem("Hell Meat", 2, 4, 10);
            newItem("Shell Meat", 29, 3, 10);
            newItem("Ent Meat", 30, 1, 10);
            newItem("Elder Meat", 125, 6, 10);
            newItem("Chain Meal", 10, 4, 10);
            newItem("Steel Meal", 5, 4, 10);
            newItem("Dragon Meat", 120, 5, 10);
            newItem("Hippogriff Meat", 125, 5, 10);
            newItem("Wyrm Meat", 4000, 5, 10);//LEGENDARY
        }
    }
}