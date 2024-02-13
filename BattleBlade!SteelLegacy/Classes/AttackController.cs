using BattleBlade_SteelLegacy.Classes;

namespace BattleBlade_SteelLegacy.Classes
{
    public class AttackController
    {
        public static bool attackDodged = false;
        public static bool attackBlocked = true;
        public static float dmgTaken = 0;

        public enum MoveChoice
        {
            Empty,
            Attack,
            Dodge,
            Block,
            Run
        }

        private static float DetermineAtkDmg(float wepDmg)
        {
            Random r = new Random();
            return r.Next((int)(wepDmg - (wepDmg * 0.25f)), (int)(wepDmg + (wepDmg * 0.25f))) + 1;
        }

        private static void PrintTitleAndWeapon()
        {
            Graphics.PrintTitleCard();
            Console.WriteLine($"You are attacking with {Game.player.currentWeapon.name}");
            Game.player.pm.numOfAttacks++;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Type 'cancel' to cancel");
            Console.WriteLine("Press Enter to continue");
            Text.SetTextColor(Text.TC.W);
        }

        private static void HandlePlayerAttack(float damageMultiplier)
        {
            Graphics.PrintTitleCard();
            Game.player.dmg = DetermineAtkDmg(Game.player.currentWeapon.wepDmg + Game.player.pm.wepDamageWithWeaponScaling(Game.player.currentWeapon)) * damageMultiplier;
            Console.WriteLine($"You attacked with {Game.player.currentWeapon.name}!");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"You dealt {Game.player.dmg.ToString("0.00")} damage!");
            Text.SetTextColor(Text.TC.W);
            Enemy.currentEnemy.health -= Game.player.dmg;
            Enemy.completedAction = true;
        }

        public static void PlayerAttack()
        {
            while (true)
            {
                try
                {
                    PrintTitleAndWeapon();
                    string choice = Console.ReadLine().ToLower();

                    if (Game.player.pm.numOfAttacks == 4)
                    {
                        Game.player.pm.numOfAttacks = 0;
                        Game.player.am.restLevel = 0;
                    }

                    if (choice.Contains("cancel"))
                    {
                        Enemy.completedAction = false;
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(choice))
                    {
                        Random rand = new Random();
                        int critChance = rand.Next(1, 16);

                        if (critChance == 4)
                        {
                            HandlePlayerAttack(5);
                        }
                        else
                        {
                            HandlePlayerAttack(1);
                        }

                        return;
                    }
                }
                catch
                {
                    Text.InvalidInput();
                }
            }
        }

        public static void EnemyAttack()
        {
            Console.WriteLine($"\n{Enemy.currentEnemy.name} attacked!");

            if (attackDodged)
            {
                Text.Color("But you dodged it!", Text.TC.B);
                Text.Continue();
            }
            else if (attackBlocked)
            {
                Text.Color("But you blocked it!", Text.TC.B);
                dmgTaken = DetermineAtkDmg(Enemy.currentEnemy.dmg) / 2;
                Game.player.pm.takeDamage(dmgTaken);
            }
            else
            {
                dmgTaken = DetermineAtkDmg(Enemy.currentEnemy.dmg);
                Game.player.pm.takeDamage(dmgTaken);
            }
        }

        private static void Attack()
        {
            Random rand = new Random();
            int critChance = rand.Next(1, 16);

            if (critChance == 4)
            {
                Graphics.PrintTitleCard();
                Game.player.dmg = DetermineAtkDmg(Game.player.currentWeapon.wepDmg + Game.player.pm.wepDamageWithWeaponScaling(Game.player.currentWeapon)) * 5;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("CRITICAL HIT");
                Text.SetTextColor(Text.TC.W);
                Console.WriteLine($"You attacked with {Game.player.currentWeapon.name}!");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"You dealt {Game.player.dmg.ToString("0.00")} damage!");
                Text.SetTextColor(Text.TC.W);
                Enemy.currentEnemy.health -= Game.player.dmg;
                Enemy.completedAction = true;
            }
            else
            {
                Graphics.PrintTitleCard();
                Game.player.dmg = DetermineAtkDmg(Game.player.currentWeapon.wepDmg + Game.player.pm.wepDamageWithWeaponScaling(Game.player.currentWeapon));
                Console.WriteLine($"You attacked with {Game.player.currentWeapon.name}!");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"You dealt {Game.player.dmg.ToString("0.00")} damage!");
                Text.SetTextColor(Text.TC.W);
                Enemy.currentEnemy.health -= Game.player.dmg;
                Enemy.completedAction = true;
            }
        }

        private static void Dodge()
        {
            Random rand = new Random();
            int dodge = rand.Next(1, 4);
            attackDodged = dodge == 3;
        }

        private static void Block()
        {
            attackBlocked = dmgTaken > Game.player.AR * 2;
        }

        public static void NewPlayerAttack()
        {
            int maxEnergy = 2;
            int energy = maxEnergy;
            attackBlocked = false;
            attackDodged = false;

            while (true)
            {
                try
                {
                    PrintTitleAndWeapon();
                    Text.Color("What actions would you like to do?");

                    if (Game.player.currentWeapon.energyConsumption == 1)
                    {
                        Text.SetTextColor(Text.TC.Y);
                        Console.WriteLine("1. Attack & Prepare Dodge");
                        Console.WriteLine("2. Attack & Prepare Block");
                        Console.WriteLine("3. Cancel");
                        string action = Game.GetPlayerInput();
                        Text.Color("Use numbers: 1, 2 etc...", Text.TC.g);

                        if (action.Contains("1"))
                        {
                            Attack();
                            energy -= Game.player.currentWeapon.energyConsumption;
                            Dodge();
                            energy--;
                            return;
                        }
                        else if (action.Contains("2"))
                        {
                            Attack();
                            energy -= Game.player.currentWeapon.energyConsumption;
                            Block();
                            energy--;
                            return;
                        }
                        else if (action.Contains("3"))
                        {
                            Enemy.completedAction = false;
                            return;
                        }
                    }
                    else
                    {
                        Text.SetTextColor(Text.TC.Y);
                        Console.WriteLine("1. Attack");
                        Console.WriteLine("2. Cancel");
                        string action = Game.GetPlayerInput();
                        Text.Color("Use numbers: 1, 2 etc...", Text.TC.g);

                        if (action.Contains("1"))
                        {
                            Attack();
                            energy -= Game.player.currentWeapon.energyConsumption;
                            return;
                        }
                        else if (action.Contains("2"))
                        {
                            return;
                        }
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
