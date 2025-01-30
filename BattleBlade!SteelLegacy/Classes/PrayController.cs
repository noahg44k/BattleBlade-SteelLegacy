using BattleBlade_SteelLegacy.Classes;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Numerics;
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
                Console.WriteLine("PRAY TO YOUR GOD: " + p.role.god);
                Console.WriteLine("\n\n\n\n\n\n\n\n\n");
                Text.SetTextColor(Text.TC.g);
                Console.WriteLine("Faith: " + p.role.getStat(Stat.StatName.Faith));
                Console.WriteLine("Favor: " + p.role.getStat(Stat.StatName.Favor));
                Text.SetTextColor(Text.TC.Y);
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("What would you like to pray for?");
                Console.WriteLine("Favor");
                Console.WriteLine("Luck");
                Console.WriteLine("Weapon Imbuement");
                Console.WriteLine("Quit");
                Text.Color("Praying increases faith, which is used to cast blessings.", Text.TC.g);

                string option = Game.GetPlayerInput();

                switch (option)
                {
                    case string a when a.Contains("fav") || option == "1":
                        favor(p);
                        break;

                    case string a when a.Contains("lu") || option == "2":
                        luck(p);
                        break;

                    case string a when a.Contains("wea") || option == "3":
                        weaponImbuement(p);
                        break;

                    case string a when a.Contains("quit") || option == "6":
                        return;

                    default:
                        Text.InvalidInput();
                        break;
                }
            }
        }

        private void favor(Player p) // FAVOR IS THE MAGIC ABILITY TO CAST HEALING SPELLS
        {
            string sacrificeItems = "";
            Item selectedItem = null;
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
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Text.Instructions("Press Enter to go back");

                string input = Game.GetPlayerInput();
                foreach(Item item in sacrificeItemsList)
                {
                    if(item.name.ToLower() == input)
                    {
                        selectedItem = item;
                    }
                }
                if(selectedItem != null)
                {
                    if(Game.player.role.getStat(Stat.StatName.Favor) < 20)
                    {
                        p.inventory.Remove(selectedItem);
                        p.pm.addFavor(p);
                        return;
                    }
                    else
                    {
                        Text.Error("Your Favor is at its maximum!");
                        Text.Continue();
                        return;
                    }
                }
                if(input == String.Empty)
                {
                    return;
                }
                Text.Error("Invalid input!");
                Text.Error(input + " is not a valid item!");
                Text.Continue();
            }
        }
        private void luck(Player p) // LUCK IS THE CHANCE TO ENCOUNTER ITEMS ** YET TO BE IMPLEMENTED
        {
            while (true)
            {
                Graphics.PrintTitleCard();
                Random r = new Random();
                double luckChance = r.NextDouble() + Convert.ToDouble(p.role.getStat(Stat.StatName.Favor)) / 60;
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
            Text.Color("This doesn't work yet! :D", Text.TC.C);
            Text.Continue();
        }
    }
}
