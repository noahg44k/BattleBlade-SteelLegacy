using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBlade_SteelLegacy.Classes
{
    public class Armor : Item
    {
        public int AR = 0; //ARMOR RATING
        public bool equipped = false;
        public int durability = 100;
        private static int prevID = 0;

        public Dictionary<Stat.StatName, int> minStats = new Dictionary<Stat.StatName, int>();
        public Dictionary<Stat.StatName, int> sclStats = new Dictionary<Stat.StatName, int>();

        public Armor(string name, int AR, int stage,
            List<Stat.StatName> minStatNames = null, List<int> minStatValues = null,
            List<Stat.StatName> scaleStatNames = null, List<int> scaleStatValues = null, 
            float discoverChance = 1) : base(name, stage, discoverChance)
        {
            this.ID = ++prevID;
            this.AR = AR;
            this.use = Use.Wear;
            this.equipped = false;
            this.stage = stage;
            this.discoverChance = discoverChance;
            this.stackHeight = 1;

            ItemManager.SetDefaultStats(this);

            if (minStatNames != null)
            {
                for (int i = 0; i < minStatNames.Count; i++)
                {
                    this.minStats[minStatNames[i]] = minStatValues[i];
                }
            }

            if (scaleStatNames != null)
            {
                for (int i = 0; i < scaleStatNames.Count; i++)
                {
                    this.sclStats[scaleStatNames[i]] = scaleStatValues[i];
                }
            }

            if (ItemManager.items == null)
            {
                ItemManager.items = new List<Item>();
            }

            //Ensuring no duplicates
            ItemManager.RemoveDuplicateItem(name);
            ItemManager.AddNewItemToList(this);
        }

        /// <summary>
        /// Creates a copy of an armor
        /// </summary>
        /// <param name="armor">Armor to copy</param>
        public Armor(Armor armor) : base(armor) // <-- Add this
        {
            this.name = armor.name;
            this.ID = armor.ID;
            this.use = Use.Wear;
            this.stage = armor.stage;
            this.discoverChance = armor.discoverChance;
            this.AR = armor.AR;
            this.equipped = armor.equipped;
            this.stackHeight = 1;

            // Copy the dictionaries to avoid reference sharing
            this.minStats = new Dictionary<Stat.StatName, int>(armor.minStats);
            this.sclStats = new Dictionary<Stat.StatName, int>(armor.sclStats);
        }

        public bool CheckCanEquip(Player player)
        {
            foreach (KeyValuePair<Stat.StatName, int> kvpStat in this.minStats)
            {
                if (minStats[kvpStat.Key] > player.role.roleStats[kvpStat.Key])
                {
                    Text.Error("Player cannot equip this item!");
                    Text.Error($"{kvpStat.Key} is too low!");
                    Text.Continue();
                    return false;
                }
            }
            return true;
        }
    }
}
