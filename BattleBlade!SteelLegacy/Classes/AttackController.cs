using BattleBlade_SteelLegacy.Classes;

namespace BattleBlade_SteelLegacy.Classes
{
    public class AttackController
    {
        public static bool attackDodged = false;
        public static bool attackBlocked = true;
        public static bool attemptedBlock = false;
        public static bool attemptedDodge = false;
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
            //damage is randomized to be slightly above weapon damage and slightly below weapon damage, then adds 1 to ensure no 0's
            float dmg = r.Next((int)(wepDmg - (wepDmg * 0.33f)), (int)(wepDmg + (wepDmg * 0.33f))) + 1;
            return Math.Max(0, dmg); // Ensure no negative values
        }

        private static void PrintTitleAndWeapon()
        {
            Graphics.PrintTitleCard();
            Text.Instructions($"You are attacking with {Game.player.currentWeapon.name}\n");
            Game.player.pm.numOfAttacks++;
        }

        public static void EnemyAttack()
        {
            Console.WriteLine($"\n{EnemyManager.currentEnemy.name} attacked!");

            if (attemptedDodge && attackDodged)
            {
                Text.Color("But you dodged it!", Text.TC.B);
                Text.Continue();
            }
            else if (attemptedDodge && !attackDodged)
            {
                Text.Color("But your dodge failed!", Text.TC.B);
                dmgTaken = DetermineAtkDmg(EnemyManager.currentEnemy.dmg);
                Game.player.pm.takeDamage(dmgTaken);
            }
            else if (attemptedBlock && attackBlocked)
            {
                Text.Color("But you blocked it!", Text.TC.B);
                dmgTaken = DetermineAtkDmg(EnemyManager.currentEnemy.dmg) / 2;
                Game.player.pm.takeDamage(dmgTaken);
            }
            else if (attemptedBlock && !attackBlocked)
            {
                Text.Color("But your block failed!", Text.TC.B);
                dmgTaken = DetermineAtkDmg(EnemyManager.currentEnemy.dmg);
                Game.player.pm.takeDamage(dmgTaken);
            }
            else
            {
                dmgTaken = DetermineAtkDmg(EnemyManager.currentEnemy.dmg);
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
                EnemyManager.currentEnemy.health -= Game.player.dmg;
                EnemyManager.playerCompletedAction = true;
                return;
            }
            else
            {
                Graphics.PrintTitleCard();
                //sets damage to weaponDamage PLUS weaponDamageWithScaling
                Game.player.dmg = DetermineAtkDmg(Game.player.currentWeapon.wepDmg + Game.player.pm.wepDamageWithWeaponScaling(Game.player.currentWeapon));
                Console.WriteLine($"You attacked with {Game.player.currentWeapon.name}!");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"You dealt {Game.player.dmg.ToString("0.00")} damage!");
                Text.SetTextColor(Text.TC.W);
                EnemyManager.currentEnemy.health -= Game.player.dmg;
                EnemyManager.playerCompletedAction = true;
                return;
            }
        }

        private static void Dodge()
        {
            //sets the boolean attackDodged to true only if the random number is 3.
            Random rand = new Random();
            int dodge = rand.Next(1, 4);
            attemptedDodge = true;
            attackDodged = dodge == 3;
        }

        private static void Block()
        {
            //sets blocked to true 2/3 times
            Random rand = new Random();
            int block = rand.Next(1, 4);
            attemptedBlock = true;
            if (block != 1)
                attackBlocked = true;
        }

        public static void NewPlayerAttack()
        {
            int maxEnergy = 2;
            int energy = maxEnergy;
            attackBlocked = false;
            attackDodged = false;
            attemptedBlock = false;
            attemptedDodge = false;

            while (true)
            {
                try
                {
                    PrintTitleAndWeapon();
                    Text.Color("What actions would you like to do?\n");

                    if (Game.player.currentWeapon.energyConsumption == 1)
                    {
                        Text.SetTextColor(Text.TC.Y);
                        Console.WriteLine("1. Attack & Prepare Dodge");
                        Console.WriteLine("2. Attack & Prepare Block");
                        Console.WriteLine("3. Cancel");
                        Text.Color("Use numbers: 1, 2 etc...", Text.TC.g);

                        string option = Game.GetPlayerInput();

                        switch (option)
                        {
                            case string a when a.Contains("dod") || option == "1":
                                Attack();
                                energy -= Game.player.currentWeapon.energyConsumption;
                                Dodge();
                                energy--;
                                return;

                            case string a when a.Contains("bl") || option == "2":
                                Attack();
                                energy -= Game.player.currentWeapon.energyConsumption;
                                Block();
                                energy--;
                                return;

                            case string a when a.Contains("can") || option == "3":

                                EnemyManager.playerCompletedAction = false;
                                return;

                            default:
                                Text.InvalidInput();
                                break;
                        }
                    }
                    else
                    {
                        Text.SetTextColor(Text.TC.Y);
                        Console.WriteLine("1. Attack");
                        Console.WriteLine("2. Cancel");
                        Text.Color("Use numbers: 1, 2 etc...", Text.TC.g);
                        string action = Game.GetPlayerInput();

                        string option = Game.GetPlayerInput();

                        switch (option)
                        {
                            case string a when a.Contains("dod") || option == "1":
                                Attack();
                                energy -= Game.player.currentWeapon.energyConsumption;
                                return;

                            case string a when a.Contains("can") || option == "2":

                                EnemyManager.playerCompletedAction = false;
                                return;

                            default:
                                Text.InvalidInput();
                                break;
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
