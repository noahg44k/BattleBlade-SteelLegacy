using BattleBlade_SteelLegacy.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BattleBlade_SteelLegacy.Classes
{
    internal class PrayController
    {
        public void Pray(Player p)
        {
            while (true)
            {
                Graphics.PrintTitleCard();
                Text.SetTextColor(Text.TC.Y);
                Console.WriteLine("PRAY TO YOUR GOD: " + Game.player.role.god);
                Console.WriteLine("\n\n\n\n\n\n\n\n\n");
                Text.SetTextColor(Text.TC.g);
                Console.WriteLine("Faith: " + Game.player.faith);
                Text.SetTextColor(Text.TC.Y);
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("What would you like to pray for?");
                Console.WriteLine("1. Favor");
                Console.WriteLine("2. Luck");
                Console.WriteLine("3. Weapon Imbuement");
                Console.WriteLine("4. Quit");
                Text.SetTextColor(Text.TC.g);
                Console.WriteLine("Praying increases faith, which is used to cast blessings.");
                Console.WriteLine("");

                string input = Console.ReadLine().ToLower();
                int inputValue;

                if (int.TryParse(input, out inputValue))
                {
                    switch (inputValue)
                    {
                        case 1:
                            favor(p);
                            break;
                        case 2:
                            luck(p);
                            break;
                        case 3:
                            weaponImbuement(p);
                            break;
                        case 4:
                            return;
                    }
                }
                else
                {
                    Text.InvalidInput();
                }
            }
        }

        private void favor(Player p) // FAVOR IS THE MAGIC ABILITY TO CAST HEALING SPELLS
        {
            string sacrificeItems = "";
            while (true)
            {
                Graphics.PrintTitleCard();
                Text.Color("What would you like to sacrifice to increase your favor?", Text.TC.g);
                string title = string.Format("{0,-35}{1,-35}{2,-35}\n", "Sacrificable Items", "", "");
                List<Item> sacrificeItemsList = new List<Item>(); // SO I CAN USE FOR INSTEAD FOR FOREACH - BASICALLY TO AVOID A HEADACHE OF LOGIC

                foreach (Item item in p.inventory)
                {
                    if (item.use == Item.Use.Heal)
                    {
                        sacrificeItemsList.Add(item);
                    }
                }

                string item1 = "";
                string item2 = "";
                string item3 = "";

                for (int i = 0; i < sacrificeItemsList.Count; i += 3)
                {
                    try
                    {
                        item1 = sacrificeItemsList[i].name;
                    }
                    catch
                    {
                        item1 = "";
                    }
                    try
                    {
                        item2 = sacrificeItemsList[i + 1].name;
                    }
                    catch
                    {
                        item2 = "";
                    }
                    try
                    {
                        item3 = sacrificeItemsList[i + 2].name;
                    }
                    catch
                    {
                        item3 = "";
                    }
                    sacrificeItems += string.Format("{0,-35}{1,-35}{2,-35}\n", item1, item2, item3);
                }

                Console.WriteLine("\n\n\n");
                Text.Color(title, Text.TC.Y);
                Text.Color(sacrificeItems, Text.TC.E);

                Text.Continue();
                return;
            }
        }
        private void luck(Player p) // LUCK IS THE CHANCE TO ENCOUNTER ITEMS ** YET TO BE IMPLEMENTED
        {
            while (true)
            {
                Graphics.PrintTitleCard();
                Random r = new Random();
                double luckChance = r.NextDouble() + (Convert.ToDouble(p.faith)) / 50;
                if (p.luckWalkCounter <= 0)
                {
                    if (luckChance >= 0.90)
                    {
                        p.luckWalkCounter = 50;
                        p.role.raiseStat(Stat.StatName.Luck);
                        break;
                    }
                    else
                    {
                        p.luckWalkCounter = 50;
                        Text.Color(p.role.god + " did not look upon you favorably. Your luck was not increased.", Text.TC.b);
                        Text.Continue();
                        break;
                    }
                }
                else
                {
                    Text.Color("You must walk " + p.luckWalkCounter + "m until you can try and level up your luck!", Text.TC.R);
                    Text.Continue();
                    break;
                }
            }
        }
        private void weaponImbuement(Player p) // WEAPON IMBUEMENT WILL ADD SPECIAL MAGICAL ABILITIES TO WEAPONS
        {

        }
    }
}
