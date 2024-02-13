using BattleBlade_SteelLegacy.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BattleBlade_SteelLegacy.Classes.Text;

namespace BattleBlade_SteelLegacy.Classes
{
    public class Clock
    {
        //8PM to 6AM IS DARK
        //DURING DARK YOU'RE MORE LIKELY TO ENCOUNTER MONSTERS, DAY UR MORE LIKE TO ENCOUNTER ITEMS
        public static int hour = 0;
        public static int minute = 0;
        public static int day = 0;

        public static void resetClock()
        {
            Clock.day = 0;
            Clock.hour = 0;
            Clock.minute = 0;
        }

        public static void setClock(int day, int hour, int minute)
        {
            Clock.day = day;
            Clock.hour = hour;
            Clock.minute = minute;
        }

        public static void increaseTime(int hour, int minute)
        {
            Clock.hour += hour;
            Clock.minute += minute;
            while (Clock.minute >= 60)
            {
                Clock.minute -= 60;
                Clock.hour++;
            }
            while (Clock.hour >= 24)
            {
                Clock.hour -= 24;
                Clock.day++;
            }
        }
        public static void increaseTime(int minute)
        {
            Clock.minute += minute;
            while (Clock.minute >= 60)
            {
                Clock.minute -= 60;
                Clock.hour++;
            }
            while (Clock.hour >= 24)
            {
                Clock.hour -= 24;
                Clock.day++;
            }
        }

        public static void printTime()
        {
            Text.SetTextColor(TC.M);
            string hour = string.Format("{0,0:D2}", Clock.hour);
            string minute = string.Format("{0,0:D2}", Clock.minute);
            string time = string.Format("| {0}:{1}   {2} |", hour, minute, Clock.day);
            int barLen = time.Length;
            string bar = "";
            for (int i = 0; i < barLen; i++)
            {
                bar += "~";
            }
            Console.WriteLine(time);
            Console.WriteLine(bar);
        }
    }
}
