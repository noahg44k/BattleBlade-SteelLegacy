using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBlade_SteelLegacy.Classes
{
    public class Weapon : Item
    {
        public float wepDmg = 0f;
        public bool equipped = false;
        public int durability = 100;
        public int energyConsumption = 1;
        private static int prevID = 0;

        public Dictionary<Stat.StatName, int> minStats = new Dictionary<Stat.StatName, int>();
        public Dictionary<Stat.StatName, int> sclStats = new Dictionary<Stat.StatName, int>();

        public Weapon(string name, float dmg, float heal, int stage,
            List<Stat.StatName> minStatNames = null, List<int> minStatValues = null,
            List<Stat.StatName> scaleStatNames = null, List<int> scaleStatValues = null,
            int energyConsumption = 1, float discoverChance = 1) : base(name, heal, stage, discoverChance)
        {
            this.ID = ++prevID;
            this.wepDmg = dmg;
            this.use = Use.Hold;
            this.equipped = false;
            this.stage = stage;
            this.energyConsumption = energyConsumption;
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
        /// Creates a copy of a weapon
        /// </summary>
        /// <param name="weapon">Weapon to copy</param>
        public Weapon(Weapon weapon) : base(weapon)
        {
            this.name = weapon.name;
            this.ID = weapon.ID;
            this.heal = weapon.heal;
            this.use = Use.Hold;
            this.stage = weapon.stage;
            this.discoverChance = weapon.discoverChance;
            this.wepDmg = weapon.wepDmg;
            this.equipped = weapon.equipped;
            this.energyConsumption = weapon.energyConsumption;
            this.stackHeight = 1;

            this.minStats = new Dictionary<Stat.StatName, int>(weapon.minStats);
            this.sclStats = new Dictionary<Stat.StatName, int>(weapon.sclStats);
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
