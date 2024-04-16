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
                Text.Color("Could not find item of name " + name, Text.TC.R);
                return null;
            }
        }

        public int determineXPCap()
        {
            int xpCap = 25;
            int total = 0;

            foreach (Stat stat in Game.player.role.roleStats)
            {
                total += stat.value;
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

        public void increaseStat()
        {
            while (true)
            {
                Graphics.PrintTitleCard();
                Text.Color("Which stat would you like to increase?", Text.TC.W);

                DisplayStatOptions();

                string choice = Console.ReadLine().ToLower();

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
            foreach (Stat stat in Game.player.role.roleStats)
            {
                Text.Color($"{num}. {stat.name}: {stat.value}", Text.TC.g);
                num++;
            }
            Text.Print("\n");
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
            Map.Stage beforeStage = Game.map.getStage();

            Game.player.lvl++;
            Game.player.lvlXpCap = determineXPCap();
            Game.player.xp -= 200;
            if (Game.player.xp < 0)
            {
                Game.player.xp = 0;
            }
            Graphics.PrintTitleCard();
            Game.map.checkStage();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("You leveled up to level " + Game.player.lvl + "!");
            Text.Continue();

            increaseStat();

            if (beforeStage != Game.map.getStage())
            {
                Console.WriteLine("\nCongratulations! You entered the " + Game.map.getStage() + " stage!");
                Text.Continue();
            }

            Text.SetTextColor();
            Game.player.health = Game.player.maxHealth;
        }

        public void takeDamage(float damage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            float damageTaken = damage * (1 - (Game.player.AR * 0.01f));
            if (damageTaken < 0)
            {
                Console.WriteLine("You took 0 pts of damage!");
                Text.Continue();
                damageTaken = 0;
            }
            else
            {
                Game.player.health -= damageTaken;
                Console.WriteLine("You took " + damageTaken.ToString("0.00") + " pts of damage!");
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

        public float wepDamageWithWeaponScaling(Item weapon)
        {
            float damage = 0;

            Stat.StatName[] statNames = { Stat.StatName.Intelligence, Stat.StatName.Luck, Stat.StatName.Precision, Stat.StatName.Speed, Stat.StatName.Strength, Stat.StatName.Vigor };

            foreach (Stat.StatName statName in statNames)
            {
                int minStatValue = weapon.minStats[statName];
                int roleStatValue = Game.player.role.getStat(statName).value;

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
                    weapons.Add(item);
                }
                else if (item.use == Item.Use.Wear)
                {
                    armor.Add(item);
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

            Console.WriteLine("Inventory: ");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            string title = string.Format("{0,-35}{1,-35}{2,-35}\n", "Weapons", "Armor", "Heals");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(title);
            Text.SetTextColor();
            string inv = "";
            for (int i = 0; i < sortedList[0].Count; i++) // MADE A SORTED LIST SO I COULD GET THE COUNT OF ALL ITEMS COLLECTIVELY
            {
                try
                {
                    weaponSpot = weapons[i].name.ToLower();
                    if (weapons[i].equipped)
                    {
                        weaponSpot = "EQ >> " + weaponSpot.ToUpper();
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    weaponSpot = " ";
                }
                try
                {
                    armorSpot = armor[i].name.ToLower();
                    if (armor[i].equipped)
                    {
                        armorSpot = "EQ >> " + armorSpot.ToUpper();
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    armorSpot = " ";
                }
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
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
        }

        public void printStats()
        {
            string choice = "";
            string[] leftStats = new string[8];
            string[] rightStats = new string[8];
            while (true)
            {
                Graphics.PrintTitleCard();
                string roleName = Game.player.role.roleName.ToString();
                string healthStr = "HP: " + Game.player.health.ToString("0.0");
                string maxHealthStr = "Max HP: " + Game.player.maxHealth.ToString("0.0");
                string xpStr = "XP: " + Game.player.xp.ToString("0.0");
                string nxtLvl = "XP until next level: " + (determineXPCap() - Game.player.xp).ToString("0.0");
                string lvlStr = "Level: " + Game.player.lvl;
                string distWalkedStr = "Distance Walked: " + Game.player.distWalked.ToString();
                string curStageStr = "Current Stage: " + Game.map.getStage().ToString();
                string armorStr = "Armor Rating: " + Game.player.AR.ToString();
                string faithStr = "Faith: " + Game.player.faith;
                string strength = Game.player.role.getStat(Stat.StatName.Strength).name + ": " + Game.player.role.getStat(Stat.StatName.Strength).value;
                string luck = Game.player.role.getStat(Stat.StatName.Luck).name + ": " + Game.player.role.getStat(Stat.StatName.Luck).value;
                string vigor = Game.player.role.getStat(Stat.StatName.Vigor).name + ": " + Game.player.role.getStat(Stat.StatName.Vigor).value;
                string speed = Game.player.role.getStat(Stat.StatName.Speed).name + ": " + Game.player.role.getStat(Stat.StatName.Speed).value;
                string intelligence = Game.player.role.getStat(Stat.StatName.Intelligence).name + ": " + Game.player.role.getStat(Stat.StatName.Intelligence).value;
                string precision = Game.player.role.getStat(Stat.StatName.Precision).name + ": " + Game.player.role.getStat(Stat.StatName.Precision).value;

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
                rightStats[4] = vigor;
                rightStats[5] = intelligence;
                rightStats[6] = armorStr;
                rightStats[7] = "";


                Console.WriteLine("\n" + Game.player.name + "'s Stats\n~~~~~~~~~~~~~~~~~~\n");

                Text.SetTextColor(Text.TC.E);
                Console.WriteLine("Class: " + roleName);
                for (int i = 0; i < leftStats.Length; i++)
                {
                    leftStats[i] = String.Format("{0,-58}", leftStats[i]);
                    rightStats[i] = String.Format("{0,-20}", rightStats[i]);
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
                    choice = Console.ReadLine().ToLower();

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
