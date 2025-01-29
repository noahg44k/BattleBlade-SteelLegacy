using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBlade_SteelLegacy.Classes
{
    internal class EnemyManager
    {
        public static List<Enemy> stageEnemies = new List<Enemy>();
        public static Enemy currentEnemy = new Enemy("Noah", 9001, 9002, 74, 0);
        public static List<Enemy> enemies = new List<Enemy>();
        public static bool playerCompletedAction = true;

        public static void AddAllEnemiesInStage(int stageNum)
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

        /// <summary>
        /// Used for lazy random health
        /// </summary>
        /// <param name="hp">Monster's current health</param>
        /// <returns></returns>
        public static int genRandomHealth(float hp)
        {
            int health = 1;

            Random r = new Random();
            health = r.Next((int)(hp - (hp * 0.25)), (int)(hp + (hp * 0.25)));
            return health;
        }
        /// <summary>
        /// Used for lazy random xp
        /// </summary>
        /// <param name="xp">Monster's current xp</param>
        /// <returns></returns>
        public static int genRandomXP(float xp)
        {
            int XP = 1;

            Random r = new Random();
            XP = r.Next((int)(xp - (xp * 0.25)), (int)(xp + (xp * 0.25)));
            return XP;
        }

        public static void buildEnemyDex()
        {
            new Enemy(name: "Dryad", health: 10, dmg: 2, XP: 4, stage: 1);
            new Enemy("Gimn", 12, 3, 6, 1); //crazy aggressive, muscular, hairy goblin
            new Enemy("Tol", 8, 3.5f, 7, 1);
            new Enemy("Ent", 25, 9.5f, 19, 1);
            new Enemy("Gyor", 45, 12.2f, 49, 1); // big beasty guy : every word they say starts with G
            new Enemy("Whisp", 5, 2, 4, 1);

            new Enemy("Spider", 12, 7, 28, 2);
            new Enemy("Ghoul", 28, 14, 56, 2);
            new Enemy("Ghost", 1, 12.3f, 49, 2);
            new Enemy("Banshee", 68, 15.2f, 60, 2);
            new Enemy("Skeleton", 50, 11, 51, 2);
            new Enemy("Lich", 55, 20.8f, 100, 2);

            new Enemy("Drake", 150, 40, 200, 3);
            new Enemy("Wyvern", 235, 65, 400, 3);
            new Enemy("Dragon", 500, 85, 500, 3);
            new Enemy("Hippogriff", 200, 35, 370, 3);
            new Enemy("Wyrm", 2000, 0, 2500, 3); // LEGENDARY

            new Enemy("Fisk", 67, 27, 400, 4);
            new Enemy("Torkle", 84, 34, 600, 4);
            new Enemy("Eel", 104, 46f, 800, 4);
            new Enemy("Siren", 120, 58f, 1200, 4);
            new Enemy("Kraken", 210, 75f, 1500, 4);

            new Enemy("Oxmen", 220, 60, 2028, 5);
            new Enemy("Inferno Golem", 500, 125.6f, 10000, 5); //LEGENDARY
            new Enemy("Demon", 80, 85, 2580, 5);
            new Enemy("Steel Golem", 405, 109.7f, 5122, 5);

            new Enemy("Elder Shaantii", 750, 138, 621000, 6);
            new Enemy("Elder Rakkek", 695, 145, 604650, 6);
            new Enemy("Elder Vizarum", 1005, 300, 1809000, 6);
        }

        /// <summary>
        /// Get the enemy object NOT a copy
        /// </summary>
        /// <param name="name">Enemy name</param>
        /// <returns></returns>
        public static Enemy getEnemy(string name)
        {
            foreach (Enemy enemy in stageEnemies)
            {
                if (enemy.name.ToLower() == name.ToLower())
                {
                    return enemy;
                }
            }
            Text.Error("Could not find enemy with name: " + name);
            return null;
        }

        public static Enemy getEnemy(int ID)
        {
            foreach (Enemy enemy in stageEnemies)
            {
                if (enemy.ID == ID)
                {
                    return enemy;
                }
            }
            Text.Error("Could not find enemy with ID: " + ID);
            return null;
        }

        /// <summary>
        /// Get the enemy object copy
        /// </summary>
        /// <param name="name">Enemy name</param>
        /// <returns></returns>
        public static Enemy getEnemyCopy(string name)
        {
            foreach (Enemy enemy in stageEnemies)
            {
                if (enemy.name.ToLower() == name.ToLower())
                {
                    //creates a new enemy COPY that has all the values of the real enemy
                    Enemy enemyCopy = new Enemy(getEnemy(enemy.name));
                    return enemyCopy;
                }
            }
            Text.Error("Could not find enemy with name: " + name);
            return null;
        }

        static void RandomEncounterDialog()
        {
            Random rand = new Random();
            int num = rand.Next(0, 8);
            string article = "";

            //used for getting the proper article in case of an Ent or Ogre or something
            switch (currentEnemy.name.ToLower()[0])
            {
                case 'a':
                case 'e': 
                case 'i':
                case 'o': 
                case 'u':
                    article = "an";
                    break;
                default:
                    article = "a";
                    break;
            }
            
            //Random dialogue for enemy encounter
            switch (num)
            {
                case 0:
                    Text.Color($"Oh no! You've encountered {article} {currentEnemy.name}!", Text.TC.C);
                    Text.Continue();
                    break;
                case 1:
                    Text.Color($"Look out, {article} {currentEnemy.name}!", Text.TC.C);
                    Text.Continue();
                    break;
                case 2:
                    Text.Color($"Beware, {article} {currentEnemy.name} approaches.", Text.TC.C);
                    Text.Continue();
                    break;
                case 3:
                    Text.Color($"AAAAAAHHH, {article.ToUpper()} {currentEnemy.name.ToUpper()}!", Text.TC.C);
                    Text.Continue();
                    break;
                case 4:
                    Text.Color($"By the holy light, {article} {currentEnemy.name}!", Text.TC.C);
                    Text.Continue();
                    break;
                case 5:
                    Text.Color($"Blessed heavens, {article} {currentEnemy.name}!", Text.TC.C);
                    Text.Continue();
                    break;
                case 6:
                    Text.Color($"By the gods, {article} {currentEnemy.name}!", Text.TC.C);
                    Text.Continue();
                    break;
                case 7:
                    Text.Color($"Hark, {article} {currentEnemy.name}!", Text.TC.C);
                    Text.Continue();
                    break;
                default:
                    Text.Color($"What dark forces stir in this land?! Here cometh {article} {currentEnemy.name}!", Text.TC.C);
                    Text.Continue();
                    break;
            }
        }

        public static void encounterEnemy()
        {
            Random rand = new Random();
            
            string choice = "";
            bool keepFighting = true;
            Graphics.PrintTitleCard();

            //generates a random index inside the stage enemies list
            int index = rand.Next(0, stageEnemies.Count());
            //sets the current enemy to a COPY of the enemy at the specified index
            currentEnemy = getEnemyCopy(stageEnemies.ElementAt<Enemy>(index).name);
            RandomEncounterDialog();

            //LOGIC FOR ACTUAL FIGHT vvvv
            while (currentEnemy.health > 0 && keepFighting)
            {
                try
                {
                    //DISPLAY STUFF vvvv
                    SaveData.Save();
                    Graphics.PrintTitleCard();
                    Text.Color(Game.player.name, Text.TC.G);
                    Text.Color($"HP: {Game.player.health.ToString("0.00")}\n", Text.TC.g);
                    Text.Color(currentEnemy.name, Text.TC.M);
                    Text.Color($"Enemy HP: {currentEnemy.health.ToString("0.00")}", Text.TC.m);
                    Text.Color($"Enemy dmg: {currentEnemy.dmg.ToString("0.00")}", Text.TC.m);
                    Text.Color("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~", Text.TC.W);

                    //WHAT WOULD U LIKE TO DO vvvv
                    Text.Color("What would you like to do?\n", Text.TC.W);

                    Text.SetTextColor(Text.TC.g);
                    Text.Print("Attack");
                    Text.Print("Use/Equip Item");
                    Text.Print("Run away");
                    choice = Game.GetPlayerInput();

                    //ATTACK vvvv
                    if (choice.Contains("attack") || choice.Contains("1"))
                    {
                        AttackController.NewPlayerAttack();
                        Clock.increaseTime(1);
                    }
                    //ITEM vvvv
                    else if (choice.Contains("item") || choice.Contains("2"))
                    {
                        Game.player.am.useItem();
                        Clock.increaseTime(1);
                    }
                    //RUN vvvv
                    else if (choice.Contains("run") || choice.Contains("3") || choice.Contains("exit"))
                    {
                        Graphics.PrintTitleCard();
                        Text.Color("You ran away! Like a coward!");
                        Text.Continue();
                        keepFighting = false;
                        playerCompletedAction = false;
                        return;
                    }
                    else
                    {
                        // I dont think this will ever fire -- nvm it fired 1/27/2025
                        Text.InvalidInput();
                        playerCompletedAction = false;
                        choice = "";
                    }
                }
                //INVALID INPUT vvvv
                catch
                {
                    Text.InvalidInput();
                    playerCompletedAction = false;
                    choice = "";
                }

                //ENEMY ATTACK vvvv
                if (playerCompletedAction && currentEnemy.health > 0)
                {
                    Text.Instructions("Attacking");
                    AttackController.EnemyAttack();
                    Clock.increaseTime(1);
                }
            }

            //ENEMY IS SLAIN vvvv
            Text.Color("\n\nWell done! You slayed the " + currentEnemy.name + "!", Text.TC.C);
            Text.Color("You gained " + currentEnemy.XP + "xp points!", Text.TC.C);
            Text.Continue();

            Game.player.pm.addXP(currentEnemy.XP);

            //FINDING LOOT AFTER ENEMY IS KILLED vvvv
            int chance = rand.Next(0, 4);
            if (chance == 3)
            {
                int itemType = rand.Next(0, Game.player.stageItemsAvailable.Count);
                Game.player.am.pickUp(Game.player.stageItemsAvailable.ElementAt(itemType));
                Text.Continue();
            }
        }
    }
}