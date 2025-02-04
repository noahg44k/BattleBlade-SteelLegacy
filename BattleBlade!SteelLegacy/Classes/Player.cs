﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBlade_SteelLegacy.Classes
{
    public class Player
    {
        public List<Item> inventory = new List<Item>();
        public Weapon currentWeapon = new Weapon("Fist", 1, 0, 0);
        public Armor currentArmor = new Armor("Nakey", 0, 0);
        public List<Item> stageItemsAvailable = new List<Item>();
        public Role role = new Role();
        public Weapon? classDefaultWeapon = null;
        public string name = "";
        public float maxHealth = 75f;
        public float health = 75f;
        public float dmg = 1f;
        public float dmgMod = 1;
        public int AR = 0; //ARMOR RATING
        public long xp = 0;
        public int lvl = 1;
        public int distWalked = 0;
        public int luckWalkCounter = 0;
        public int lvlXpCap = 25;
        public PlayerManager pm = new PlayerManager();
        public ActionManager am = new ActionManager();

        public void resetPlayer()
        {
            name = "";
            maxHealth = 75f;
            health = 75f;
            dmg = 1f;
            dmgMod = 1;
            AR = 0; //ARMOR RATING
            xp = 0;
            lvlXpCap = 25;
            lvl = 1;
            distWalked = 0;
            Clock.resetClock();
            inventory.Clear();
            inventory.Add(ItemManager.getItem("fist"));
            currentWeapon = (Weapon)pm.getInvItem("fist");
            currentArmor = (Armor)ItemManager.getItem("nakey");
        }
    }
}
