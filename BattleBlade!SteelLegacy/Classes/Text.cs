using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBlade_SteelLegacy.Classes
{
    internal class Text
    {
        public enum TC
        {
            W,
            G,
            g,
            M,
            m,
            Y,
            y,
            E,
            e,
            R,
            r,
            B,
            b,
            C,
            c
        }
        public static void Continue()
        {
            Instructions("Press any key to continue");
            Console.ReadKey();
        }
        public static void InvalidInput(string msg = "")
        {
            //set text color red
            Timed("Invalid Input!", TC.R);
            if (msg != "")
            {
                Timed(msg, TC.R);
            }
            Continue();
        }
        public static void Instructions(string text)
        {
            //set text color dark gray
            SetTextColor(TC.g);
            Console.WriteLine(text);
        }
        public static void Error(string text, TC color = TC.R)
        {
            SetTextColor(color);
            Console.WriteLine(text);
        }
        public static void Timed(string text, TC color = TC.W, int speed = 10)
        {
            SetTextColor(color);

            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(speed);
            }
            Console.Write("\n");
        }
        public static void Timed(string text, int speed = 10, TC color = TC.W)
        {
            SetTextColor(color);
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(speed);
            }
            Console.Write("\n");
        }
        public static void Color(string text, TC color = TC.W)
        {
            SetTextColor(color);
            Console.WriteLine(text);
        }
        public static void Print(string text)
        {
            Console.WriteLine(text);
        }
        public static void SetTextColor(TC color = TC.W)
        {
            switch (color)
            {
                case TC.W:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case TC.G:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case TC.g:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case TC.M:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case TC.m:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case TC.Y:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case TC.y:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case TC.E:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case TC.e:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case TC.R:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case TC.r:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case TC.B:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case TC.b:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case TC.C:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case TC.c:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
            }
        }
    }
}
