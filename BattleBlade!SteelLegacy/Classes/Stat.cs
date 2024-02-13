using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBlade_SteelLegacy.Classes
{
    public class Stat
    {
        public int value = 0;
        public static List<Stat> stats = new List<Stat>();
        public static Stat statContr = new Stat();
        public StatName name = new StatName();
        public enum StatName
        {
            Strength,
            Luck,
            Vigor,
            Speed,
            Intelligence,
            Precision
        }

        public Stat newStat(StatName name, int value)
        {
            Stat stat = new Stat();
            stat.name = name;
            stat.value = value;

            return stat;
        }

    }
}
