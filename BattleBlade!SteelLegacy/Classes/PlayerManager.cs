using BattleBlade_SteelLegacy.Classes;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BattleBlade_SteelLegacy.Classes
{
    public class PlayerManager
    {
        public Item getInvItem(string name)
        {
            Item item = Game.player.inventory.FirstOrDefault(item => item.name.Equals(name, StringComparison.OrdinalIgnoreCase));
            
            if (item != null)
            {
                return item;
            }
            else
            {
                return null;
            }
        }

        public int determineXPCap()
        {
            int xpCap = 25;
            int total = 0;

            foreach (KeyValuePair<Stat.StatName, int> kvp in Game.player.role.roleStats)
            {
                total += kvp.Value;
            }
            Game.player.role.roleLevel = total;
            xpCap += 5 * Game.player.role.roleLevel;
            return xpCap;
        }
        public void addXP(long newXP)
        {
            Game.player.xp += newXP;

            Game.player.lvlXpCap = determineXPCap();

            while (Game.player.xp >= Game.player.lvlXpCap)
            {
                lvlUp();
            }
        }
        /// <summary>
        /// Increase the player's favor by 1
        /// 
        /// Used in the Pray Controller class
        /// </summary>
        /// <param name="p">Player</param>
        public void addFavor(Player p)
        {
            //raise the favor stat but do not announce it
            if (Game.player.role.getStat(Stat.StatName.Favor) < 20)
            {
                p.role.raiseStat(Stat.StatName.Favor, false);
                Text.Timed("Your favor with " + p.role.god + " has increased by one!", 10, Text.TC.M);
                Text.Continue();
            }
        }

        public void increaseStat()
        {
            while (true)
            {
                Graphics.PrintTitleCard();
                Text.Color("Which stat would you like to increase?", Text.TC.W);

                DisplayStatOptions();

                string choice = Game.GetPlayerInput();

                if (choice.Contains("1") || choice.Contains("str"))
                {
                    increaseStatWithConfirmation(Stat.StatName.Strength);
                    return;
                }

                else if (choice.Contains("2") || choice.Contains("luck"))
                {
                    DisplayAndContinue("Cannot upgrade luck. Pray to your god to increase luck.");
                }

                else if (choice.Contains("3") || choice.Contains("vig"))
                {
                    increaseStatWithConfirmation(Stat.StatName.Vigor);
                    return;
                }    

                else if (choice.Contains("4") || choice.Contains("spee"))
                {
                    increaseStatWithConfirmation(Stat.StatName.Speed);
                    return;
                }

                else if (choice.Contains("5") || choice.Contains("inte"))
                {
                    increaseStatWithConfirmation(Stat.StatName.Intelligence);
                    return;
                }    

                else if (choice.Contains("6") || choice.Contains("prec"))
                {
                    increaseStatWithConfirmation(Stat.StatName.Precision);
                    return;
                }
            }
        }
        private void DisplayStatOptions()
        {
            int num = 1;
            foreach (KeyValuePair<Stat.StatName, int> kvp in Game.player.role.roleStats)
            {
                Text.Color($"{num}. {kvp.Key}: {kvp.Value}", Text.TC.g);
                num++;
            }
        }
        private void increaseStatWithConfirmation(Stat.StatName statName)
        {
            while (true)
            {
                Graphics.PrintTitleCard();
                Text.Color($"You want to increase {statName}?", Text.TC.W);
                Text.Color("y/n", Text.TC.g);
                string confirm = Game.GetPlayerInput();

                if (confirm.Contains("y"))
                {
                    Game.player.role.raiseStat(statName);
                    return;
                }
                else if (confirm.Contains("n"))
                {
                    
                }
                else
                {
                    Text.InvalidInput();
                }
            }
        }

        private void DisplayAndContinue(string message)
        {
            while (true)
            {
                Graphics.PrintTitleCard();
                Text.Color(message, Text.TC.W);
                Text.Continue();
                break;
            }
        }

        public void lvlUp()
        {
            Game.player.lvl++;
            Game.player.lvlXpCap = determineXPCap();
            Game.player.xp -= 200;
            if (Game.player.xp < 0)
            {
                Game.player.xp = 0;
            }
            Graphics.PrintTitleCard();
            Text.Color($"You leveled up to level {Game.player.lvl}!", Text.TC.y);
            Text.Continue();

            increaseStat();

            Map.Stage beforeStage = Game.map.getStage();
            Game.map.checkStage();

            Text.SetTextColor();
            Game.player.health = Game.player.maxHealth;
            SaveData.Save();
        }
        public void takeDamage(float damage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            float damageTaken = damage * (1 - (Game.player.AR * 0.01f));
            if (damageTaken < 0)
            {
                Text.Color("You took 0 pts of damage!");
                Text.Continue();
                damageTaken = 0;
            }
            else
            {
                Game.player.health -= damageTaken;
                Text.Color($"You took {Math.Round(damageTaken, 2)} pts of damage!");
                Text.Continue();
                damageTaken = 0;
            }

            if (Game.player.health <= 0)
            {
                Graphics.death();
            }
        }
        public void heal(float heal)
        {
            Game.player.health += heal;
            if (Game.player.health > Game.player.maxHealth)
                Game.player.health = Game.player.maxHealth;
            Console.WriteLine("You healed {0}", heal + " health pts!");
        }

        //ATTACK VARS
        //USED FOR "TIRING YOU OUT"
        //INCREASES THE AMOUNT OF REST YOU CAN HAVE IF YOU ATTACK FOUR TIMES
        public int numOfAttacks = 0;

        public float calculateDamage(int lvl, int minStat, int scaleStat)
        {
            //TAKE MINIMUM STAT LEVEL AND EACH LEVEL ABOVE THAT, USE THE FORMULA
            float dmgMultiplier = 1.1f;
            float newDmg;

            int difference = lvl - minStat;
            // SCALES DAMAGE UP EXPONENTIALLY FOR EACH LEVEL ABOVE
            // MINIMUM * SCALE VALUE FOR THAT STAT
            newDmg = (float)(Math.Pow(dmgMultiplier, difference)) * (1 + (scaleStat * 0.5f)) - 1;
            return newDmg;
        }
        public float wepDamageWithWeaponScaling(Weapon weapon)
        {
            float damage = 0;

            Stat.StatName[] statNames = { Stat.StatName.Intelligence, Stat.StatName.Luck, Stat.StatName.Precision, Stat.StatName.Speed, Stat.StatName.Strength, Stat.StatName.Vigor };

            foreach (Stat.StatName statName in statNames)
            {
                int minStatValue = weapon.minStats[statName];
                int roleStatValue = Game.player.role.getStat(statName);

                if (minStatValue > roleStatValue || minStatValue == 1)
                {
                    // do nothing
                    damage += 0;
                }
                else if (minStatValue < roleStatValue)
                {
                    damage += calculateDamage(roleStatValue, minStatValue, weapon.sclStats[statName]);
                }
            }

            return damage;
        }

        public void printInventory()
        {
            List<Item> weapons = new List<Item>();
            List<Item> armor = new List<Item>();
            List<Item> heals = new List<Item>();

            string weaponSpot = "";
            string armorSpot = "";
            string healsSpot = "";

            List<List<Item>> listsList = new List<List<Item>>();

            foreach (Item item in Game.player.inventory)
            {
                if (item.use == Item.Use.Hold)
                {
                    weapons.Add((Weapon)item);
                }
                else if (item.use == Item.Use.Wear)
                {
                    armor.Add((Armor)item);
                }
                else if (item.use == Item.Use.Heal)
                {
                    heals.Add(item);
                }
            }

            listsList.Add(weapons);
            listsList.Add(armor);
            listsList.Add(heals);

            var sortedList = listsList.OrderByDescending(l => l.Count()).ToList();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Inventory: ");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            string title = string.Format("{0,-35}{1,-35}{2,-35}\n", "Weapons", "Armor", "Heals");
            Text.Color(title, Text.TC.g);
            Text.SetTextColor();
            string inv = "";
            for (int i = 0; i < sortedList[0].Count; i++) // MADE A SORTED LIST SO I COULD GET THE COUNT OF ALL ITEMS COLLECTIVELY
            {

                //Weapon
                try
                {
                    // Cast to Weapon and get name and equipped status
                    Weapon weapon = weapons[i] as Weapon;
                    if (weapon != null)
                    {
                        weaponSpot = weapon.name.ToLower();
                        if (weapon.equipped)
                        {
                            weaponSpot = "EQ >> " + weaponSpot.ToUpper();
                        }
                    }
                    else
                    {
                        weaponSpot = " ";
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    weaponSpot = " ";
                }

                //Armor
                try
                {
                    // Cast to Weapon and get name and equipped status
                    Armor arm = armor[i] as Armor;
                    if (arm != null)
                    {
                        armorSpot = arm.name.ToLower();
                        if (arm.equipped)
                        {
                            armorSpot = "EQ >> " + armorSpot.ToUpper();
                        }
                    }
                    else
                    {
                        armorSpot = " ";
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    armorSpot = " ";
                }

                //Consumables
                try
                {
                    healsSpot = heals[i].name.ToLower() + " x" + heals[i].currentStack;
                }
                catch (ArgumentOutOfRangeException)
                {
                    healsSpot = " ";
                }

                inv += string.Format("{0,-35}{1,-35}{2,-35}\n", weaponSpot, armorSpot, healsSpot);
            }

            Console.WriteLine(inv);
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }
        public void printStats()
        {
            string format = "{0, -25}";
            string choice = "";
            string[] leftStats = new string[8];
            string[] rightStats = new string[8];
            while (true)
            {
                Graphics.PrintTitleCard();
                string roleName = Game.player.role.roleName.ToString();
                string healthStr = String.Format(format, "HP: ") + Math.Round(Game.player.health, 2);
                string maxHealthStr = String.Format(format, "Max HP: ") + Math.Round(Game.player.maxHealth, 2);
                string xpStr = String.Format(format, "XP: ") + Game.player.xp.ToString("0.0");
                string nxtLvl = String.Format(format, "XP until next level: ") + (determineXPCap() - Game.player.xp).ToString("0.0");
                string lvlStr = String.Format(format, "Level: ") + Game.player.lvl;
                string distWalkedStr = String.Format(format, "Distance Walked: ") + Game.player.distWalked.ToString();
                string curStageStr = String.Format(format, "Current Stage: ") + Game.map.getStage().ToString();
                string armorStr = String.Format(format, "Armor Rating: ") + Game.player.AR.ToString();
                string faithStr = String.Format(format, "Faith: ") + Game.player.role.getStat(Stat.StatName.Faith);
                string favorStr = String.Format(format, "Favor: ") + Game.player.role.getStat(Stat.StatName.Favor);
                string strength = String.Format(format, "Strength: ") + Game.player.role.getStat(Stat.StatName.Strength);
                string luck = String.Format(format, "Luck: ") + Game.player.role.getStat(Stat.StatName.Luck);
                string vigor = String.Format(format, "Vigor: ") + Game.player.role.getStat(Stat.StatName.Vigor);
                string speed = String.Format(format, "Speed: ") + Game.player.role.getStat(Stat.StatName.Speed);
                string intelligence = String.Format(format, "Intelligence: ") + Game.player.role.getStat(Stat.StatName.Intelligence);
                string precision = String.Format(format, "Precision: ") + Game.player.role.getStat(Stat.StatName.Precision);

                leftStats[0] = healthStr;
                leftStats[1] = xpStr;
                leftStats[2] = lvlStr;
                leftStats[3] = distWalkedStr;
                leftStats[4] = luck;
                leftStats[5] = speed;
                leftStats[6] = precision;
                leftStats[7] = strength;

                rightStats[0] = maxHealthStr;
                rightStats[1] = nxtLvl;
                rightStats[2] = curStageStr;
                rightStats[3] = faithStr;
                rightStats[4] = favorStr;
                rightStats[5] = vigor;
                rightStats[6] = intelligence;
                rightStats[7] = armorStr;


                Console.WriteLine("\n" + Game.player.name + "'s Stats\n~~~~~~~~~~~~~~~~~~\n");

                Text.SetTextColor(Text.TC.E);
                Console.WriteLine("Class: " + roleName);
                for (int i = 0; i < leftStats.Length; i++)
                {
                    leftStats[i] = String.Format("{0,-50}", leftStats[i]);
                    rightStats[i] = rightStats[i];
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(leftStats[i] + rightStats[i]);
                }
                Console.WriteLine("\n");
                Text.SetTextColor(Text.TC.W);

                printInventory();

                Console.WriteLine("What would you like to do?\n");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Change Name");
                Console.WriteLine("Drop Item");
                Console.WriteLine("Use/Equip Item");
                Console.WriteLine("Inspect Item");
                Console.WriteLine("Leave");
                Text.SetTextColor(Text.TC.W);
                try
                {
                    choice = Game.GetPlayerInput();

                    if (choice.Contains("name") || choice.Contains("1"))
                    {
                        Game.player.am.changeName();
                    }
                    else if (choice.Contains("drop") || choice.Contains("2"))
                    {
                        Game.player.am.dropItem();
                    }
                    else if (choice.Contains("use") || choice.Contains("3") || choice.Contains("equip"))
                    {
                        Game.player.am.useItem();
                    }
                    else if (choice.Contains("inspect") || choice.Contains("4"))
                    {
                        Game.player.am.inspectItem();
                    }
                    else if (choice.Contains("leave") || choice.Contains("5") || choice.Contains("exit"))
                    {
                        return;
                    }
                    else
                    {
                        Text.InvalidInput();
                    }
                }
                catch
                {
                    Text.InvalidInput();
                }
            }
        }
    }

}
