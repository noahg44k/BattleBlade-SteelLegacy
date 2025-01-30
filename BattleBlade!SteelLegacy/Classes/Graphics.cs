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
        public static void PrintCompass()
        {
            string top =    ("_____");
            string n =      ("/  N  \\");
            string middle = ("|W  +  E|");
            string s =      ("\\  S  /");
            string bottom = ("-----");

            string compass = string.Format("{0,22}\n{1,23}\n{2,24}\n{3,23}\n{4,22}\n", top, n, middle, s, bottom);
            foreach(char c in compass)
            {
                Text.SetTextColor();
                if(c == 'N' ||  c == 'W' || c == 'E' || c == 'S')
                {
                    Text.SetTextColor(Text.TC.R);
                }
                Console.Write(c);
            }
        }
        public static void WelcomeBackMessage()
        {
            PrintTitleCard();

            Text.Timed("Welcome back", 40);
            Text.Timed(Game.player.name, 160, Text.TC.E);
            Text.Continue();
        }
        public static void death()
        {
            SaveData.Delete();

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
                    Game.player.resetPlayer();
                    Game.gameLoop();
                }
                else if (choice == "n")
                {
                    Console.WriteLine("I bid you farewell, {0}", Game.player.name + ".");
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
