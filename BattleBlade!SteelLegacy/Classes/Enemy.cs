using BattleBlade_SteelLegacy.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BattleBlade_SteelLegacy.Classes
{
    public class Enemy
    {
        public string name = "";
        public float health = 100;
        public float dmg = 0;
        public int ID = 0;
        public long XP = 0;
        public int stage = 0;
        private static int prevID = 0;

        /// <summary>
        /// Creates a copy of an already existing enemy
        /// </summary>
        /// <param name="other">Enemy to copy</param>
        public Enemy(Enemy other)
        {
            this.name = other.name;
            this.health = EnemyManager.genRandomHealth(other.health);
            this.dmg = other.dmg;
            this.ID = other.ID;
            this.XP = EnemyManager.genRandomHealth(other.XP);
            this.stage = other.stage;
        }

        public Enemy(string name, float health, float dmg, long XP, int stage)
        {
            this.name = name;
            this.health = health;
            this.dmg = dmg;
            this.ID = ++prevID;
            this.XP = XP;
            this.stage = stage;

            if (EnemyManager.enemies == null)
            {
                EnemyManager.enemies = new List<Enemy>();
            }
            EnemyManager.enemies.Add(this);
        }
    }
}
