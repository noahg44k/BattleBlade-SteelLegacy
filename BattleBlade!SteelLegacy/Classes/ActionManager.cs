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
        //logic for useItem separated by item type
        void UseHealItem(Item item)
        {
            Graphics.PrintTitleCard();
            Game.player.pm.heal(item.heal);
            item.currentStack -= 1;
            if (item.currentStack == 0)
                Game.player.inventory.Remove(item);
            EnemyManager.playerCompletedAction = true;
            Text.Continue();
        }
        void UseWeapon(Item item)
        {
            Weapon weapon = item as Weapon; // Cast to Weapon
            if (weapon != null)
            {
                if (!weapon.equipped)
                {
                    //if stats are too low to equip weapon, do not continue
                    if (!weapon.CheckCanEquip(Game.player))
                    {
                        return;
                    }

                    Graphics.PrintTitleCard();
                    Unequip(Game.player.currentWeapon);
                    Equip(weapon);
                    EnemyManager.playerCompletedAction = false;
                }
                else
                {
                    Graphics.PrintTitleCard();
                    Unequip(Game.player.currentWeapon);
                    Equip((Weapon)Game.player.pm.getInvItem("fist"), false);
                    EnemyManager.playerCompletedAction = false;
                }
                Text.Continue();
            }
        }
        void UseArmor(Item item)
        {
            Armor armor = item as Armor; // Cast to Armor
            if (armor != null)
            {
                if (!armor.equipped)
                {
                    //if stats are too low to equip armor, do not continue
                    if (!armor.CheckCanEquip(Game.player))
                    {
                        return;
                    }
                    Graphics.PrintTitleCard();
                    Unequip(Game.player.currentArmor);
                    Equip(armor);
                    EnemyManager.playerCompletedAction = false;
                }
                else
                {
                    Graphics.PrintTitleCard();
                    Unequip(Game.player.currentArmor);
                    Equip((Armor)ItemManager.getItem("nakey"), false);
                    EnemyManager.playerCompletedAction = false;
                }
                Text.Continue();
            }
        }

        public void useItem()
        {
            while (true)
            {
                bool foundItem = false;
                Graphics.PrintTitleCard();
                Console.WriteLine("What item would you like to use?");
                Game.player.pm.printInventory();
                Text.Instructions("Press Enter to go back");
                
                string choice = Game.GetPlayerInput();

                if (choice.ToLower() == "" || choice == "nothing" || choice == "back" || choice == "exit" || choice == null)
                {
                    EnemyManager.playerCompletedAction = false;
                    Graphics.PrintTitleCard();
                    return;
                }
                foreach (Item item in Game.player.inventory)
                {

                    if (choice.ToLower() == item.name.ToLower())
                    {
                        foundItem = true;
                        //if unequipping fist while current weapon is fist
                        if(choice == "fist" && Game.player.currentWeapon.name.ToLower() == "fist")
                        {
                            Text.Error("Can't unequip fists, they are attached to your arms...");
                            Text.Continue();
                            break;
                        }
                        if (item.use == Item.Use.Heal)
                        {
                            UseHealItem(item);
                            return;
                        }
                        else if (item.use == Item.Use.Hold)
                        {
                            UseWeapon(item);
                            return;
                        }
                        else if (item.use == Item.Use.Wear) // Handle Armor
                        {
                            UseArmor(item);
                            return;
                        }
                        else
                        {
                            Graphics.PrintTitleCard();
                            EnemyManager.playerCompletedAction = false;
                            Text.InvalidInput(choice + " could not be used!\nPlease type in the name of a useable item...");
                            break;
                        }
                    }
                }
                if (!foundItem)
                {
                    Graphics.PrintTitleCard();
                    EnemyManager.playerCompletedAction = false;
                    Text.InvalidInput("You don't have " + choice + "!\nType in an item name...");
                }
            }
        }

        public void Equip(Armor armor, bool announce = true)
        {
            armor.equipped = true;
            Game.player.currentArmor = armor;
            Game.player.AR = Game.player.currentArmor.AR;
            if (announce)
                Text.Instructions($"You equipped {armor.name}");
        }
        public void Equip(Weapon weapon, bool announce = true)
        {
            weapon.equipped = true;
            Game.player.currentWeapon = weapon;
            //do not announce if weapon to equip is fist
            if (announce && weapon.name.ToLower() != "fist")
                Text.Instructions($"You equipped {weapon.name}");
        }
        public void Unequip(Armor armor, bool announce = true)
        {
            armor.equipped = false;
            Game.player.AR = 0;
            //do not announce unequipping nakey
            if (announce && armor.name.ToLower() != "nakey")
                Text.Instructions($"You unequipped {armor.name}");
        }
        public void Unequip(Weapon weapon, bool announce = true)
        {
            weapon.equipped = false;
            //do not announce unequipping fist
            if (announce && weapon.name.ToLower() != "fist")
                Text.Instructions($"You unequipped {weapon.name}");
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
            if(Game.player.pm.getInvItem(item.name) == null)
            {
                item.currentStack += 1;
                Game.player.inventory.Add(item);
                Text.Color($"You picked up {item.name}!", Text.TC.Y);
                SaveData.Save();
            }
            else
            {
                if (Game.player.pm.getInvItem(item.name).currentStack < item.stackHeight)
                {
                    item.currentStack += 1;
                    Game.player.inventory.Add(item);
                    Text.Color($"You picked up {item.name}!", Text.TC.Y);
                }
                else
                {
                    Text.Color($"You encountered a {item.name}, but you have the maximum amount of that item already.", Text.TC.y);
                }
                SaveData.Save();
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
        int previousDist = 0;

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
        /// <summary>
        /// A random chance to take a rest while walking or to continue walking
        /// </summary>
        /// <param name="pd">previousDistance</param>
        /// <param name="walkTime"></param>
        public void exploreRest(int pd, int walkTime)
        {
            Random rand = new Random();
            int num = rand.Next(0, 3);

            switch (num)
            {
                case 0:
                    Text.SetTextColor();
                    Graphics.PrintTitleCard();
                    int dist = rand.Next(5, 9);
                    Console.WriteLine("After a long voyage of {0}", dist + " miles, you can no longer continue. You lie on the floor and rest.");
                    Game.player.distWalked += dist;

                    if (Game.player.distWalked >= pd + 8)
                    {
                        pd = Game.player.distWalked;
                        restLevel = 0;
                    }

                    walkTime = dist * 16;
                    Clock.increaseTime(walkTime);
                    Game.player.luckWalkCounter -= dist;
                    Text.Continue();
                    rest();
                    break;
                case 1:
                    Graphics.PrintTitleCard();
                    dist = rand.Next(1, 5);
                    Console.WriteLine("After a short trek of {0}", dist + " miles, you decide you can continue.");
                    Game.player.distWalked += dist;

                    if (Game.player.distWalked >= pd + 8)
                    {
                        pd = Game.player.distWalked;
                        restLevel = 0;
                    }

                    walkTime = dist * 16;
                    Clock.increaseTime(walkTime);
                    Game.player.luckWalkCounter -= dist;
                    Text.Continue();
                    break;
                case 2:
                    Graphics.PrintTitleCard();
                    dist = rand.Next(5, 9);
                    Console.WriteLine("After a long journey of {0}", dist + " miles, you muster the strength to continue.");
                    Game.player.distWalked += dist;

                    if (Game.player.distWalked >= pd + 8)
                    {
                        pd = Game.player.distWalked;
                        restLevel = 0;
                    }

                    walkTime = dist * 16;
                    Clock.increaseTime(walkTime);
                    Game.player.luckWalkCounter -= dist;
                    Text.Continue();
                    break;
                case 3:
                    break;
            }
        }
        public void explore()
        {
            Random rand = new Random();
            int num = rand.Next(0, 6);
            int walkTime = 0;

            Graphics.PrintTitleCard();
            Console.WriteLine("                                            (  N  )");
            Console.WriteLine("What direction would you like to travel in? (W + E)");
            Console.WriteLine("                                            (  S  )");
            Text.Color("(North, West, Forward, Left, etc.)", Text.TC.g);

            Text.Print("");
            Text.SetTextColor();
            string direction = Console.ReadLine();

            Graphics.PrintTitleCard();
            Text.Timed("You traveled " + direction + "!", 25, Text.TC.B);
            Text.Continue();

            switch (num)
            {
                case 0:
                case 1: // ENCOUNTER ENEMY
                    EnemyManager.encounterEnemy();
                    Console.ResetColor();
                    break;
                case 2: // ENCOUNTER ITEM
                    Graphics.PrintTitleCard();

                    int itemType = rand.Next(0, Game.player.stageItemsAvailable.Count());

                    Console.WriteLine("What luck! You encountered an item!");
                    Text.Continue();

                    pickUp(Game.player.stageItemsAvailable.ElementAt(itemType));
                    Text.Continue();
                    break;
                case 3:
                case 4:
                case 5: // WALK AND MAYBE REST
                    exploreRest(previousDist, walkTime);
                    break;
            }
        }

        public void changeName()
        {
            while (true)
            {
                Graphics.PrintTitleCard();
                Console.WriteLine("What would you like to change your name to?");
                Text.Instructions("Press Enter to cancel");

                Text.Print("");
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
                        string confirm = Game.GetPlayerInput();
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

        //the logic for inspect item but for specific item types
        void InspectWeapon(Item item)
        {
            Weapon weapon = item as Weapon;
            if (weapon != null)
            {
                Graphics.PrintTitleCard();
                Console.WriteLine(item.name);
                Console.WriteLine("Equipped: " + weapon.equipped);
                Console.WriteLine("Energy Consumption: " + weapon.energyConsumption);
                Console.WriteLine("Base Damage: " + weapon.wepDmg);
                Console.WriteLine("Damage with Skill Proficiencies: " + (weapon.wepDmg + Game.player.pm.wepDamageWithWeaponScaling(weapon)).ToString());
                Console.Write("Current weapon damage: ");
                Text.SetTextColor(Text.TC.C);
                Console.WriteLine((Game.player.currentWeapon.wepDmg + Game.player.pm.wepDamageWithWeaponScaling(Game.player.currentWeapon)).ToString());
                Text.SetTextColor();
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

                //MIN STATS
                foreach (KeyValuePair<Stat.StatName, int> kvp in weapon.minStats)
                {
                    if (Game.player.role.getStat(kvp.Key) >= kvp.Value)
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
                foreach (KeyValuePair<Stat.StatName, int> kvp in weapon.sclStats)
                {
                    Console.WriteLine(String.Format("{0,-25}", kvp.Key + " Scaling: ") + String.Format("{0,10}", kvp.Value));
                }
                Text.Continue();
            }
        }
        void InspectArmor(Item item)
        {
            Armor armor = item as Armor;
            if (armor != null)
            {
                Graphics.PrintTitleCard();
                Console.WriteLine(armor.name);
                Console.WriteLine("Equipped: " + armor.equipped);
                Console.WriteLine("Armor Rating: " + armor.AR);
                Console.Write("Current Armor Rating: ");
                Text.SetTextColor(Text.TC.C);
                Console.WriteLine(Game.player.currentArmor.AR.ToString());
                Text.SetTextColor();
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                //MIN STATS
                foreach (KeyValuePair<Stat.StatName, int> kvp in armor.minStats)
                {
                    if (Game.player.role.getStat(kvp.Key) >= kvp.Value)
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
                foreach (KeyValuePair<Stat.StatName, int> kvp in armor.sclStats)
                {
                    Console.WriteLine(String.Format("{0,-25}", kvp.Key + " Scaling: ") + String.Format("{0,10}", kvp.Value));
                }
                Text.Continue();
            }
        }
        public void inspectItem()
        {
            string name = "";
            while (true)
            {
                bool foundItem = false;
                Graphics.PrintTitleCard();
                Text.Print("What item would you like to inspect?\n");
                Game.player.pm.printInventory();
                Text.Instructions("Press Enter to cancel");
                Text.SetTextColor();
                name = Game.GetPlayerInput();

                if (name == "" || name == null)
                {
                    return;
                }
                foreach (Item item in Game.player.inventory)
                {
                    if (item.name.ToLower() == name)
                    {
                        foundItem = true;

                        // Weapons
                        if (item.use == Item.Use.Hold)
                        {
                            InspectWeapon(item);
                        }

                        // Armor
                        else if (item.use == Item.Use.Wear)
                        {
                            InspectArmor(item);
                        }

                        // Consumables
                        else if (item.use == Item.Use.Heal)
                        {
                            Graphics.PrintTitleCard();
                            Console.WriteLine(item.name);
                            Console.WriteLine("Heal: " + item.heal);
                            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

                            Console.Write("\n");
                            Text.Continue();
                        }
                    }
                }
                if (!foundItem)
                {
                    Text.InvalidInput();
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
                    Text.Color("What item would you like to drop?\n", Text.TC.R);
                    Game.player.pm.printInventory();
                    Text.Instructions("Press Enter to cancel");
                    name = Game.GetPlayerInput();

                    if (name == "" || name == null || name == " ")
                    {
                        return;
                    }

                    foreach (Item item in Game.player.inventory)
                    {
                        if (name.ToLower() == item.name.ToLower())
                        {
                            if (name.ToLower() == "fist")
                            {
                                Text.InvalidInput("Cannot drop fists! They are attached to your arms...");
                            }
                            else
                            {
                                if (item == Game.player.currentWeapon)
                                {
                                    Game.player.currentWeapon = (Weapon)Game.player.pm.getInvItem("fist");
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
