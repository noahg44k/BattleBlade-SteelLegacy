using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BattleBlade_SteelLegacy.Classes
{
    internal class Graphics
    {
        public static void PrintTitleCard(Text.TC color = Text.TC.M)
        {
            Console.Clear();
            Text.Color(
                "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n" +
                "             ______   ______   ______  ______  __       ______ \n" +
                "            /\\  == \\ /\\  __ \\ /\\__  _\\/\\__  _\\/\\ \\     /\\  ___\\   \n" +
                "            \\ \\  __< \\ \\  __ \\\\/_/\\ \\/\\/_/\\ \\/\\ \\ \\____\\ \\  __\\        \n" +
                "             \\ \\_____\\\\ \\_\\ \\_\\  \\ \\_\\   \\ \\_\\ \\ \\_____\\\\ \\_____\\      \n" +
                "              \\/_____/ \\/_/\\/_/   \\/_/    \\/_/  \\/_____/ \\/_____/      \n" +
                "                                                           \n" +
                "                         ______   __       ______   _____    ______    \n" +
                "                        /\\  == \\ /\\ \\     /\\  __ \\ /\\  __-. /\\  ___\\   \n" +
                "                        \\ \\  __< \\ \\ \\____\\ \\  __ \\\\ \\ \\/\\ \\\\ \\  __\\   \n" +
                "                         \\ \\_____\\\\ \\_____\\\\ \\_\\ \\_\\\\ \\____- \\ \\_____\\ \n" +
                "                          \\/_____/ \\/_____/ \\/_/\\/_/ \\/____/  \\/_____/ \n" +
                "                                                           \n" +
                "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
                , color);
            Clock.printTime();
            Text.SetTextColor(Text.TC.W);
        }
        public static void death()
        {
            while (true)
            {
                PrintTitleCard();

                Text.SetTextColor(Text.TC.R);
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("|          You Died        |");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Text.SetTextColor(Text.TC.R);

                Console.WriteLine("Try again?");
                Text.SetTextColor(Text.TC.g);
                Console.WriteLine("(y/n)");
                Text.SetTextColor();
                string choice = Game.GetPlayerInput();
                if (choice == "y")
                {
                    SaveData.Delete();
                    Game.player.resetPlayer();
                    Game.gameLoop();
                }
                else if (choice == "n")
                {
                    Console.WriteLine("I bid you farewell, {0}", Game.player.name + ".");
                    SaveData.Delete();
                    Environment.Exit(0);
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
