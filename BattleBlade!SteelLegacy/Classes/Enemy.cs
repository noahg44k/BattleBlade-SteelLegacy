using BattleBlade_SteelLegacy.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBlade_SteelLegacy.Classes
{
    public class Enemy
    {
        public string name = "";
        public float health = 100;
        public float dmg = 0;
        public int ID = 0;
        public long XP = 0;
        int prevID = 0;
        public static List<Enemy> stageEnemies = new List<Enemy>();
        public static Enemy currentEnemy = new Enemy();
        public static bool completedAction = true;
        public int stage = 0;

        public static List<Enemy> enemies = new List<Enemy>();

        public Enemy copyEnemy(Enemy enemy)
        {
            Enemy copy = new Enemy();
            copy.name = enemy.name;
            copy.health = genRandomHealth((int)enemy.health - 5, (int)enemy.health + 5);
            copy.dmg = enemy.dmg;
            copy.XP = genRandomXP((int)(enemy.XP - (enemy.XP * 0.25f)), (int)(enemy.XP + (enemy.XP * 0.25f)));
            copy.ID = enemy.ID;

            return copy;
        }

        public void newEnemy(string name, float health, float damage, int XP, int stage)
        {
            Enemy newEnemy = new Enemy();

            newEnemy.name = name;
            newEnemy.health = health;
            newEnemy.dmg = damage;
            newEnemy.XP = XP;
            newEnemy.stage = stage;
            newEnemy.ID = prevID + 1;
            prevID = newEnemy.ID;

            enemies.Add(newEnemy);
            Text.Color("Successfully added enemy with name " + newEnemy.name + " and ID " + newEnemy.ID);
        }

        public void AddAllEnemiesInStage(int stageNum)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.stage == stageNum)
                {
                    stageEnemies.Add(enemy);
                }
            }
        }

        private int genRandomHealth(int min, int max)
        {
            int health = 1;

            Random r = new Random();
            health = r.Next(min, max + 1);
            return health;
        }

        private int genRandomXP(int min, int max)
        {
            int XP = 1;

            Random r = new Random();
            XP = r.Next(min, max + 1);
            return XP;
        }

        public void buildEnemyDex()
        {
            newEnemy("Dryad", 10, 2, 4, 1);
            newEnemy("Gimn", 12, 3, 6, 1); //crazy aggressive, muscular, hairy goblin
            newEnemy("Tol", 8, 3.5f, 7, 1);
            newEnemy("Ent", 25, 9.5f, 19, 1);
            newEnemy("Gyor", 45, 12.2f, 49, 1); // big beasty guy : every word they say starts with G
            newEnemy("Whisp", 5, 2, 4, 1);

            newEnemy("Spider", 12, 7, 28, 2);
            newEnemy("Ghoul", 28, 14, 56, 2);
            newEnemy("Ghost", 1, 12.3f, 49, 2);
            newEnemy("Banshee", 68, 15.2f, 60, 2);
            newEnemy("Skeleton", 50, 11, 51, 2);
            newEnemy("Lich", 55, 20.8f, 100, 2);

            newEnemy("Drake", 150, 40, 200, 3);
            newEnemy("Wyvern", 235, 65, 400, 3);
            newEnemy("Dragon", 500, 85, 500, 3);
            newEnemy("Hippogriff", 200, 35, 370, 3);
            newEnemy("Wyrm", 2000, 0, 2500, 3); // LEGENDARY

            newEnemy("Fisk", 67, 27, 400, 4);
            newEnemy("Torkle", 84, 34, 600, 4);
            newEnemy("Eel", 104, 46f, 800, 4);
            newEnemy("Siren", 120, 58f, 1200, 4);
            newEnemy("Kraken", 210, 75f, 1500, 4);

            newEnemy("Oxmen", 220, 60, 2028, 5);
            newEnemy("Inferno Golem", 500, 125.6f, 10000, 5); //LEGENDARY
            newEnemy("Demon", 80, 85, 2580, 5);
            newEnemy("Steel Golem", 405, 109.7f, 5122, 5);

            newEnemy("Elder Shaantii", 750, 138, 621000, 6);
            newEnemy("Elder Rakkek", 695, 145, 604650, 6);
            newEnemy("Elder Vizarum", 1005, 300, 1809000, 6);
        }

        public Enemy getEnemy(string name)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.name.ToLower() == name.ToLower())
                    return enemy;
            }
            Text.Error("Could not find enemy with name: " + name);
            return null;
        }

        public Enemy getRandomEnemy(int ID)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.ID == ID)
                    return enemy;
            }
            Text.Error("Could not find enemy with ID: " + ID);
            return null;
        }

        public void encounterEnemy()
        {
            Random rand = new Random();
            int num = rand.Next(0, 4);
            string choice = "";
            bool keepFighting = true;
            Graphics.PrintTitleCard();
            Game.map.checkStage();

            int enemyType = rand.Next(0, stageEnemies.Count());

            currentEnemy = copyEnemy(stageEnemies.ElementAt(enemyType));

            //ROLLING RANDOM ENCOUNTER DIALOGUE
            if (num == 0)
            {
                Text.Color($"Oh no! You've encountered a {currentEnemy.name}!", Text.TC.C);
                Text.Continue();
            }
            else if (num == 1)
            {
                Text.Color($"Look out! A {currentEnemy.name}!", Text.TC.C);
                Text.Continue();
            }
            else if (num == 2)
            {
                Text.Color($"A {currentEnemy.name} approaches.", Text.TC.C);
                Text.Continue();
            }
            else if (num == 3)
            {
                Text.Color($"AAAAAA! A {currentEnemy.name}!", Text.TC.C);
                Text.Continue();
            }
            else if (num == 4)
            {
                Text.Color($"Zoinks! A {currentEnemy.name}!", Text.TC.C);
                Text.Continue();
            }
            else if (num == 5)
            {
                Text.Color($"Oh great heavens! A {currentEnemy.name}!", Text.TC.C);
                Text.Continue();
            }
            else if (num == 6)
            {
                Text.Color($"Good golly! A {currentEnemy.name}!", Text.TC.C);
                Text.Continue();
            }
            else if (num == 7)
            {
                Text.Color($"Hark! A {currentEnemy.name}!", Text.TC.C);
                Text.Continue();
            }
            //ROLLING RANDOM ENCOUNTER DIALOGUE ^^^^

            //LOGIC FOR ACTUAL FIGHT vvvv
            while (currentEnemy.health > 0 && keepFighting)
            {
                while (true)
                {
                    try
                    {
                        //DISPLAY STUFF vvvv
                        SaveData.Save();
                        Graphics.PrintTitleCard();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(Game.player.name);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("HP: {0}\n", Game.player.health.ToString("0.00"));
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(currentEnemy.name);
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine("Enemy HP: {0}", currentEnemy.health.ToString("0.00"));
                        Console.WriteLine("Enemy Damage: {0}", currentEnemy.dmg.ToString("0.00"));
                        Text.SetTextColor(Text.TC.W);
                        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

                        //WHAT WOULD U LIKE TO DO vvvv
                        Console.WriteLine("\nWhat would you like to do?\n");

                        Text.SetTextColor(Text.TC.g);
                        Console.WriteLine("Attack");
                        Console.WriteLine("Use/Equip Item");
                        Console.WriteLine("Run away");
                        choice = Game.GetPlayerInput();

                        //ATTACK vvvv
                        if (choice.Contains("attack") || choice.Contains("1"))
                        {
                            AttackController.NewPlayerAttack();
                            Clock.increaseTime(1);
                            break;
                        }
                        //ITEM vvvv
                        else if (choice.Contains("item") || choice.Contains("2"))
                        {
                            Game.player.am.useItem();
                            Clock.increaseTime(1);
                            break;
                        }
                        //RUN vvvv
                        else if (choice.Contains("run") || choice.Contains("3") || choice.Contains("exit"))
                        {
                            Graphics.PrintTitleCard();
                            Console.WriteLine("You ran away! Like a coward!");
                            Text.Continue();
                            keepFighting = false;
                            completedAction = false;
                            return;
                        }
                        else
                        {

                        }
                    }
                    //INVALID INPUT vvvv
                    catch
                    {
                        Text.InvalidInput();
                        completedAction = false;
                        choice = "";
                    }
                }

                //ENEMY ATTACK vvvv
                if (completedAction && currentEnemy.health > 0)
                {
                    AttackController.EnemyAttack();
                    Clock.increaseTime(1);
                }
            }

            //ENEMY IS SLAIN vvvv
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n\nWell done! You slayed the " + currentEnemy.name + "!");
            Console.WriteLine("You gained " + currentEnemy.XP + "xp points!");
            Text.Continue();

            Game.player.pm.addXP(currentEnemy.XP);

            //FINDING LOOT AFTER ENEMY IS KILLED vvvv
            int chance = rand.Next(0, 8);
            if (chance == 7)
            {
                int itemType = rand.Next(0, Game.player.stageItemsAvailable.Count);
                Game.player.am.pickUp(Game.player.stageItemsAvailable.ElementAt(itemType));
                Text.Continue();
            }
        }
    }
}
