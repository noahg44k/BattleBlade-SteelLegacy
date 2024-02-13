using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BattleBlade_SteelLegacy.Classes
{
    public class ActionManager
    {

        public void useItem()
        {
            while (true)
            {
                Graphics.PrintTitleCard();
                Console.WriteLine("What item would you like to use?");
                Game.player.pm.printInventory();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Press Enter to go back");
                Text.SetTextColor();
                string choice = Console.ReadLine();

                if (choice.ToLower() == "" || choice == "nothing" || choice == "back" || choice == "exit" || choice == null)
                {
                    Enemy.completedAction = false;
                    Graphics.PrintTitleCard();
                    return;
                }
                else
                {
                    foreach (Item item in Game.player.inventory)
                    {
                        if (choice.ToLower() == item.name.ToLower())
                        {
                            if (item.use == Item.Use.Heal)
                            {
                                Graphics.PrintTitleCard();
                                Game.player.pm.heal(item.heal);
                                item.currentStack -= 1;
                                if (item.currentStack == 0)
                                    Game.player.inventory.Remove(item);
                                Enemy.completedAction = true;
                                Text.Continue();
                                return;
                            }
                            else if (item.use == Item.Use.Hold)
                            {
                                if (!item.equipped)
                                {
                                    Graphics.PrintTitleCard();
                                    Game.player.currentWeapon.equipped = false;
                                    Game.player.currentWeapon = item;
                                    Game.player.currentWeapon.equipped = true;
                                    Console.WriteLine("You equipped {0}", item.name);
                                    Enemy.completedAction = false;
                                    Text.Continue();
                                    return;
                                }
                                else
                                {
                                    Graphics.PrintTitleCard();
                                    Game.player.currentWeapon.equipped = false;
                                    Game.player.currentWeapon = Game.player.pm.getInvItem("fist");
                                    Game.player.currentWeapon.equipped = true;
                                    Console.WriteLine("You unequipped {0}", item.name);
                                    Enemy.completedAction = false;
                                    Text.Continue();
                                    return;
                                }
                            }
                            else if (item.use == Item.Use.Wear)
                            {
                                if (!item.equipped)
                                {
                                    Graphics.PrintTitleCard();
                                    Game.player.currentArmor.equipped = false;
                                    Game.player.currentArmor = item;
                                    Game.player.currentArmor.equipped = true;
                                    Game.player.AR = Game.player.currentArmor.AR;
                                    Console.WriteLine("You equipped {0}", item.name);
                                    Enemy.completedAction = false;
                                    Text.Continue();
                                    return;
                                }
                                else if (item.equipped)
                                {
                                    Graphics.PrintTitleCard();
                                    Game.player.currentArmor.equipped = false;
                                    Game.player.currentArmor = Item.getItem("nakey");
                                    Game.player.AR = 0;
                                    Console.WriteLine("You unequipped {0}", item.name);
                                    Enemy.completedAction = false;
                                    Text.Continue();
                                    return;
                                }
                            }
                            else
                            {
                                Graphics.PrintTitleCard();
                                Enemy.completedAction = false;
                                Text.InvalidInput(choice + " could not be used!\nPlease type in the name of a useable item...");
                                break;
                            }
                        }
                    }
                    Graphics.PrintTitleCard();
                    Enemy.completedAction = false;
                    Text.InvalidInput("You don't have " + choice + "!\nType in an item name...");
                }
            }
        }

        public void adminPickUp(Item item) //SAME LOGIC AS PICKUP EXCEPT WITHOUT THE CONSOLE OUTPUT
        {
            if (item.currentStack < item.stackHeight)
            {
                item.currentStack += 1;
                if (!Game.player.inventory.Contains(item))
                {
                    Game.player.inventory.Add(item);
                }
            }
            else
            {
                //DO NOTHING
            }
        }

        public void pickUp(Item item)
        {
            Graphics.PrintTitleCard();
            if (item.currentStack < item.stackHeight)
            {
                item.currentStack += 1;
                if (!Game.player.inventory.Contains(item))
                {
                    Game.player.inventory.Add(item);
                }
                Console.WriteLine("You picked up {0}", item.name + "!");
            }
            else
            {
                Console.WriteLine("You encountered a " + item.name + ", but you have the maximum amount of that item already.");
            }
        }
        public void pray()
        {
            PrayController pc = new PrayController();
            pc.Pray(Game.player);
        }

        //REST VARS
        int restLimit = 8;
        public int restLevel = 0;

        public void rest()
        {
            Random rand = new Random();
            int num = rand.Next(1, 4);
            string level = "";

            Graphics.PrintTitleCard();
            restLevel += num;
            if (restLevel <= restLimit)
            {
                Console.WriteLine("You rested for {0}", num + " hours.\n");
                Clock.increaseTime(num, 0);
                num = rand.Next(0, 4);
                switch (num)
                {
                    case 0:
                        level = "tired";
                        Console.WriteLine("You gained 0 health points.");
                        break;
                    case 1:
                        level = "the same as you did before";
                        int healNum = rand.Next(5, 11);
                        Game.player.pm.heal(healNum);
                        break;
                    case 2:
                        level = "refreshed";
                        healNum = rand.Next(12, 18);
                        Game.player.pm.heal(healNum);
                        break;
                    case 3:
                        level = "amazing";
                        healNum = rand.Next(19, 25);
                        Game.player.pm.heal(healNum);
                        break;
                }

                if (Game.player.health >= Game.player.maxHealth)
                {
                    Game.player.health = Game.player.maxHealth;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~~~~");
                    Console.WriteLine("|You are fully healed!|");
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~\n");
                    Text.SetTextColor();
                }
                Console.WriteLine("You feel {0}", level + ".");
                Text.Continue();
            }
            else
            {
                Console.WriteLine("You aren't tired anymore!");
                Console.WriteLine("You have to tire yourself out first.");
                Text.Continue();
            }

        }

        public void explore()
        {
            Random rand = new Random();
            int num = rand.Next(0, 6);
            int previousDist = 0;
            int walkTime;

            Graphics.PrintTitleCard();
            Console.WriteLine("                                            (  N  )");
            Console.WriteLine("What direction would you like to travel in? (W + E)");
            Console.WriteLine("                                            (  S  )");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("(North, West, Forward, Left, etc.)");
            Text.SetTextColor();
            string direction = Console.ReadLine();
            switch (num)
            {
                case 0: // ENCOUNTER ENEMY
                    Enemy.currentEnemy.encounterEnemy();
                    Console.ResetColor();
                    break;
                case 1: // LONG WALK AND REST
                    Text.SetTextColor();
                    int dist = rand.Next(4, 15);
                    Console.WriteLine("After a long voyage of {0}", dist + " miles, you lay on the floor and take a rest.");
                    Game.player.distWalked += dist;

                    if (Game.player.distWalked >= previousDist + 8)
                    {
                        previousDist = Game.player.distWalked;
                        restLevel = 0;
                    }

                    walkTime = dist * 16;
                    Clock.increaseTime(walkTime);
                    Game.player.luckWalkCounter -= dist;
                    Text.Continue();
                    rest();
                    break;
                case 2: // ENCOUNTER ITEM
                    Graphics.PrintTitleCard();

                    int itemType = rand.Next(0, Game.player.stageItemsAvailable.Count());

                    Console.WriteLine("What luck! You encountered an item!");
                    Text.Continue();

                    pickUp(Game.player.stageItemsAvailable.ElementAt(itemType));
                    Text.Continue();
                    break;
                case 3: // ENCOUNTER ENEMY
                    Enemy.currentEnemy.encounterEnemy();
                    Console.ResetColor();
                    break;
                case 4: // LONG WALK
                    Graphics.PrintTitleCard();
                    dist = rand.Next(4, 15);
                    Console.WriteLine("After a long voyage of {0}", dist + " miles, you decide you can continue.");
                    Game.player.distWalked += dist;

                    if (Game.player.distWalked >= previousDist + 8)
                    {
                        previousDist = Game.player.distWalked;
                        restLevel = 0;
                    }

                    walkTime = dist * 16;
                    Clock.increaseTime(walkTime);
                    Game.player.luckWalkCounter -= dist;
                    Text.Continue();
                    break;
                case 5: // LONG WALK
                    Graphics.PrintTitleCard();
                    dist = rand.Next(4, 15);
                    Console.WriteLine("After a long voyage of {0}", dist + " miles, you decide you can continue.");
                    Game.player.distWalked += dist;

                    if (Game.player.distWalked >= previousDist + 8)
                    {
                        previousDist = Game.player.distWalked;
                        restLevel = 0;
                    }

                    walkTime = dist * 16;
                    Clock.increaseTime(walkTime);
                    Game.player.luckWalkCounter -= dist;
                    Text.Continue();
                    break;
            }
        }

        public void changeName()
        {
            while (true)
            {
                Graphics.PrintTitleCard();
                Console.WriteLine("What would you like to change your name to?");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Press Enter to cancel");
                Text.SetTextColor();
                string name = Console.ReadLine();
                if (name == "" || name == null)
                {
                    return;
                }
                else
                {
                    while (true)
                    {
                        Console.WriteLine("Are you sure you want to change your name to " + name + "?");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("(y/n)");
                        Text.SetTextColor();
                        string confirm = Console.ReadLine().ToLower();
                        if (confirm == "y")
                        {
                            Game.player.name = name;
                            return;
                        }
                        else if (confirm == "n")
                        {
                            return;
                        }
                        else
                        {
                            Text.InvalidInput();
                        }
                    }
                }
            }
        }

        public void inspectItem()
        {
            string name = "";
            while (true)
            {
                try
                {
                    Graphics.PrintTitleCard();
                    Console.WriteLine("What item would you like to inspect?\n");
                    Game.player.pm.printInventory();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Press Enter to cancel");
                    Text.SetTextColor();
                    name = Console.ReadLine();

                    if (name == "" || name == null)
                    {
                        return;
                    }
                    else
                    {
                        try
                        {
                            foreach (Item item in Game.player.inventory)
                            {
                                if (item.name.ToLower() == name.ToLower())
                                {
                                    if (item.use == Item.Use.Hold)
                                    {
                                        Graphics.PrintTitleCard();
                                        Console.WriteLine(item.name);
                                        Console.WriteLine("Equipped: " + item.equipped);
                                        Console.WriteLine("Energy Consumption: " + item.energyConsumption);
                                        Console.WriteLine("Base Damage: " + item.wepDmg);
                                        Console.WriteLine("Damage with Skill Proficiencies: " + (item.wepDmg + Game.player.pm.wepDamageWithWeaponScaling(item)).ToString());
                                        Console.Write("Current weapon damage: ");
                                        Text.SetTextColor(Text.TC.C);
                                        Console.WriteLine((Game.player.currentWeapon.wepDmg + Game.player.pm.wepDamageWithWeaponScaling(Game.player.currentWeapon)).ToString());
                                        Text.SetTextColor();
                                        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

                                        try
                                        {
                                            //MIN STATS
                                            foreach (KeyValuePair<Stat.StatName, int> kvp in item.minStats)
                                            {
                                                if (Game.player.role.getStat(kvp.Key).value >= kvp.Value)
                                                {
                                                    Text.SetTextColor(Text.TC.E);
                                                }
                                                else
                                                {
                                                    Text.SetTextColor();
                                                }
                                                Console.WriteLine(String.Format("{0,-25}", "Minimum " + kvp.Key + ": ") + String.Format("{0,10}", kvp.Value));
                                                Text.SetTextColor();
                                            }

                                            Console.Write("\n");

                                            //SCALE STATS
                                            foreach (KeyValuePair<Stat.StatName, int> kvp in item.sclStats)
                                            {
                                                Console.WriteLine(String.Format("{0,-25}", kvp.Key + " Scaling: ") + String.Format("{0,10}", kvp.Value));
                                            }
                                        }
                                        catch
                                        {
                                            Console.WriteLine("smt wrong with the dictionary");
                                        }
                                        Text.Continue();
                                    }
                                    else if (item.use == Item.Use.Wear)
                                    {
                                        Graphics.PrintTitleCard();
                                        Console.WriteLine(item.name);
                                        Console.WriteLine("Equipped: " + item.equipped);
                                        Console.WriteLine("Armor Rating: " + item.AR);
                                        Console.Write("Current Armor Rating: ");
                                        Text.SetTextColor(Text.TC.C);
                                        Console.WriteLine(Game.player.currentArmor.AR.ToString());
                                        Text.SetTextColor();
                                        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                                        //MIN STATS
                                        foreach (KeyValuePair<Stat.StatName, int> kvp in item.minStats)
                                        {
                                            if (Game.player.role.getStat(kvp.Key).value >= kvp.Value)
                                            {
                                                Text.SetTextColor(Text.TC.E);
                                            }
                                            else
                                            {
                                                Text.SetTextColor();
                                            }
                                            Console.WriteLine(String.Format("{0,-25}", "Minimum " + kvp.Key + ": ") + String.Format("{0,10}", kvp.Value));
                                            Text.SetTextColor();
                                        }

                                        Console.Write("\n");

                                        //SCALE STATS
                                        foreach (KeyValuePair<Stat.StatName, int> kvp in item.sclStats)
                                        {
                                            Console.WriteLine(String.Format("{0,-25}", kvp.Key + " Scaling: ") + String.Format("{0,10}", kvp.Value));
                                        }
                                        Text.Continue();
                                    }
                                    else if (item.use == Item.Use.Heal)
                                    {
                                        Graphics.PrintTitleCard();
                                        Console.WriteLine(item.name);
                                        Console.WriteLine("Heal: " + item.heal);
                                        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                                        //MIN STATS
                                        foreach (KeyValuePair<Stat.StatName, int> kvp in item.minStats)
                                        {
                                            if (Game.player.role.getStat(kvp.Key).value >= kvp.Value)
                                            {
                                                Text.SetTextColor(Text.TC.E);
                                            }
                                            else
                                            {
                                                Text.SetTextColor();
                                            }
                                            Console.WriteLine(String.Format("{0,-25}", "Minimum " + kvp.Key + ": ") + String.Format("{0,10}", kvp.Value));
                                            Text.SetTextColor();
                                        }

                                        Console.Write("\n");

                                        //SCALE STATS
                                        foreach (KeyValuePair<Stat.StatName, int> kvp in item.sclStats)
                                        {
                                            Console.WriteLine(String.Format("{0,-25}", kvp.Key + " Scaling: ") + String.Format("{0,10}", kvp.Value));
                                        }
                                        Text.Continue();
                                    }
                                }
                            }
                        }
                        catch
                        {
                            Text.InvalidInput(name + " is not a valid name");
                        }
                    }
                }
                catch
                {
                    Text.InvalidInput("It not work idk");
                }
            }
        }

        public void dropItem()
        {
            string name = "";
            while (true)
            {
                try
                {
                    Graphics.PrintTitleCard();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("What item would you like to drop?\n");
                    Text.SetTextColor();
                    Game.player.pm.printInventory();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Press Enter to cancel");
                    Text.SetTextColor();
                    name = Console.ReadLine();

                    if (name == "" || name == null || name == " ")
                    {
                        return;
                    }

                    foreach (Item item in Game.player.inventory)
                    {
                        if (name.ToLower() == item.name.ToLower())
                        {
                            if (item.name.ToLower() == "fist")
                            {
                                Text.InvalidInput("Cannot drop fists! They are attached to your arms...");
                            }
                            else
                            {
                                if (item == Game.player.currentWeapon)
                                {
                                    Game.player.currentWeapon = Item.getItem("fist");
                                }
                                item.currentStack -= 1;
                                if (item.currentStack == 0)
                                    Game.player.inventory.Remove(item);
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("You dropped " + name + "!");
                                Text.Continue();
                                return;
                            }
                        }
                    }
                }
                catch
                {
                    Text.InvalidInput("Could not find item with name " + name + "!");
                }
            }

        }
    }
}
