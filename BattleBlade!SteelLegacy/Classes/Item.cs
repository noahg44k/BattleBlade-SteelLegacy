using BattleBlade_SteelLegacy.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BattleBlade_SteelLegacy.Classes
{
    public class Item
    {
        public string name = "";

        private static int prevID = 0;
        public int ID = 0;
        public int stage = 1;
        public int stackHeight = 10;
        public int currentStack = 0;

        public float heal = 0;
        public float discoverChance = 1;

        public Use use;
        public enum Use
        {
            Heal,
            Hold,
            Wear
        }

        public Item()
        {
            //empty constructor for empty instantiation
        }
        public Item(string name, float heal, int stage, float discoverChance = 1)
        {
            this.name = name;
            this.ID = ++prevID;
            this.heal = heal;
            this.use = Use.Heal;
            this.stage = stage;
            this.discoverChance = discoverChance;

            if (ItemManager.items == null)
            {
                ItemManager.items = new List<Item>();
            }

            //Just in case
            ItemManager.RemoveDuplicateItem(name);
            ItemManager.AddNewItemToList(this);
        }
        public Item(string name, int stage, float discoverChance)
        {
            this.name = name;
            this.stage = stage;
            this.discoverChance = discoverChance;
        }

        /// <summary>
        /// Creates a copy of an item
        /// </summary>
        /// <param name="item">Item to copy</param>
        public Item(Item item)
        {
            this.name = item.name;
            this.ID = item.ID;
            this.heal = item.heal;
            this.use = Use.Heal;
            this.stage = item.stage;
            this.discoverChance = item.discoverChance;
        }
    }
}