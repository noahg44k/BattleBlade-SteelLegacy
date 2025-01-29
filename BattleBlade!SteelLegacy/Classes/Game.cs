using System.Data;
using System.Numerics;
using System.Reflection;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace BattleBlade_SteelLegacy.Classes
{
    class Game
    {
        public static Player player = new Player();
        public static Map map = new Map();
        public static List<Stat.StatName> statNames = new List<Stat.StatName> { 
            Stat.StatName.Strength,
            Stat.StatName.Intelligence,
            Stat.StatName.Luck,
            Stat.StatName.Vigor,
            Stat.StatName.Speed,
            Stat.StatName.Precision,
            Stat.StatName.Faith,
            Stat.StatName.Favor
        };

        static void Main()
        {
            ItemManager.buildItemDex();
            EnemyManager.buildEnemyDex();
            map.checkStage();
            gameLoop();
        }
        public static void gameLoop()
        {
            Intro();
            playerActions();
        }

        public static void Intro()
        {
            Graphics.PrintTitleCard();

            string choice = "";
            while (true)
            {
                Graphics.PrintTitleCard();
                Text.SetTextColor(Text.TC.m);
                Text.Print("                    Welcome to Battle: Blade! Steel Legacy - The Text-Based RPG!\n");
                Text.SetTextColor(Text.TC.W);
                Text.Print("Would you like to...");
                Text.Print("Load previous save");
                Text.Print("Start new save");

                try
                {
                    choice = GetPlayerInput();
                }
                catch
                {
                    Text.InvalidInput();
                }

                if (choice.ToLower().Contains("load") || choice.ToLower().Contains("previous"))
                {
                    try
                    {
                        SaveData.Load();
                        return;
                    }
                    catch (FileNotFoundException)
                    {
                        Text.Color("Save not found!", Text.TC.R);
                        Text.Continue();
                    }
                    playerActions();
                    return;
                }
                else if (choice.ToLower().Contains("start") || choice.ToLower().Contains("new"))
                {
                    SaveData.NewSave();
                    welcomeNewSave();
                    return;
                }
                else
                {
                    Text.InvalidInput();
                }
            }
        }
        public static void welcomeNewSave()
        {
            Graphics.PrintTitleCard();
            Text.Color("                    Welcome to Battle: Blade! The Text-Based RPG!\n", Text.TC.M);
            Text.Color("What is your name?", Text.TC.W);
            string name = Console.ReadLine();
            player.name = name;
            player.currentWeapon = (Weapon)player.pm.getInvItem("fist");

            while (true)
            {
                ChooseClass();
                player.maxHealth = Role.calculateHealth(player.role.getStat(Stat.StatName.Vigor));
                player.health = player.maxHealth;
                Graphics.PrintTitleCard();
                Text.Color("Let's begin!", Text.TC.E);
                Text.Continue();
                return;
            }
        }

        public static void ChooseClass()
        {
            Role.buildRoleIndex();
            Graphics.PrintTitleCard();
            Thread.Sleep(2000);

            while (true)
            {
                Graphics.PrintTitleCard();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"So, {player.name}...");
                Console.WriteLine("What class would you like to be?\n");

                foreach (Role role in Role.roles)
                {
                    PrintRoleInfo(role);
                }

                string choice = GetPlayerInput();

                if (TrySelectRole(choice, Role.RoleName.Thief, "You selected the Thief class!"))
                {
                    return;
                }
                else if (TrySelectRole(choice, Role.RoleName.Knight, "You selected the Knight class!"))
                {
                    return;
                }
                else if (TrySelectRole(choice, Role.RoleName.Sorcerer, "You selected the Sorcerer class!"))
                {
                    return;
                }
                else if (TrySelectRole(choice, Role.RoleName.Archer, "You selected the Archer class!"))
                {
                    return;
                }
                else if (TrySelectRole(choice, Role.RoleName.Barren, "You selected the Barren class!"))
                {
                    return;
                }
                else if (TrySelectRole(choice, Role.RoleName.Blest, "You selected the Blest class!"))
                {
                    return;
                }
                else
                {
                    Text.InvalidInput();
                }
            }
        }
        private static void PrintRoleInfo(Role role)
        {
            Text.Color($"{role.roleName}", Text.TC.C);
            Text.Color(role.desc, Text.TC.c);
            foreach (KeyValuePair<Stat.StatName, int> kvp in role.roleStats)
            {
                Text.Color($"{kvp.Key}: {kvp.Value}", Text.TC.g);
            }
            Text.Color($"Level: {role.roleLevel}", Text.TC.g);
            Text.Color($"God: {role.god}\n", Text.TC.g);
        }
        private static bool TrySelectRole(string choice, Role.RoleName roleName, string successMessage)
        {
            if (choice.Contains(roleName.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                while (true)
                {
                    Graphics.PrintTitleCard();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Are you sure you want to be a {roleName}?");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("y/n");
                    Console.ForegroundColor = ConsoleColor.White;
                    string answer = GetPlayerInput();

                    if (answer.Equals("y", StringComparison.OrdinalIgnoreCase))
                    {
                        Role.assignRole(Role.getRole(roleName));
                        player.role.buildAssignedItems(roleName);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(successMessage);
                        Text.Continue();
                        SaveData.Save();
                        return true;
                    }
                    else if (answer.Equals("n", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                    else
                    {
                        Text.InvalidInput();
                    }
                }
            }
            return false;
        }

        //PrimaryActions Menu
        public static void playerActions()
        {
            string option = "";
            bool canContinue = false;
            while (true)
            {
                SaveData.Save();
                DisplayOptions();

                try
                {
                    option = GetPlayerInput();
                    canContinue = true;
                }
                catch
                {
                    Text.InvalidInput();
                    canContinue = false;
                }

                if (canContinue)
                {
                    ProcessOption(option);
                }
                else
                {
                    Text.InvalidInput();
                }
            }
        }
        public static string GetPlayerInput()
        {
            try
            {
                Text.Print("");
                Text.SetTextColor();
                return Console.ReadLine()?.ToLower();
            }
            catch
            {
                return null;
            }
        }
        private static void DisplayOptions()
        {
            Graphics.PrintTitleCard();
            Text.Color("What would you like to do?\n", Text.TC.W);
            Text.SetTextColor(Text.TC.g);
            Text.Print("Explore");
            Text.Print("Rest");
            Text.Print("Pray");
            Text.Print("Use/Equip Item");
            Text.Print("Inspect Player");
            Text.Print("Quit");
        }
        public static void ProcessOption(string option)
        {
            switch (option)
            {
                case string a when a.Contains("explore") || option == "1":
                    player.am.explore();
                    break;

                case string a when a.Contains("rest") || option == "2":
                    player.am.rest();
                    break;

                case string a when a.Contains("pray") || option == "3":
                    player.am.pray();
                    break;

                case string a when a.Contains("use") || a.Contains("equip") || a.Contains("item") || option == "4":
                    player.am.useItem();
                    break;

                case string a when a.Contains("player") || a.Contains("inspect") || option == "5":
                    player.pm.printStats();
                    break;

                case string a when a.Contains("quit") || option == "6":
                    ProcessQuitOption();
                    break;

                default:
                    Text.InvalidInput();
                    break;
            }
        }
        private static void ProcessQuitOption()
        {
            while (true)
            {
                Graphics.PrintTitleCard();
                Text.Color("Are you sure you want to quit?", Text.TC.W);
                Text.Color("(y/n)", Text.TC.g);
                string choice = GetPlayerInput();

                if (choice == "y")
                {
                    Text.Color($"\nI bid you farewell, {player.name}.", Text.TC.W);
                    Environment.Exit(0);
                }
                else if (choice == "n")
                {
                    break;
                }
                else
                {
                    Text.InvalidInput();
                }
            }
        }
    }
}
