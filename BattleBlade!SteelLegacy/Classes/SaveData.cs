using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.Intrinsics.X86;
using BattleBlade_SteelLegacy.Classes;
using static BattleBlade_SteelLegacy.Classes.Role;

namespace BattleBlade_SteelLegacy.Classes
{
    public class Data
    {
        public Dictionary<string, int> inventory { get; set; } = new Dictionary<string, int>();
        public string roleName { get; set; }
        public Dictionary<string, int> stats { get; set; } = new Dictionary<string, int>();
        public string currentWeapon { get; set; }
        public string currentArmor { get; set; }
        //public List<string> stageItemsAvailable { get; set; } = new List<string>();
        public string name { get; set; }
        public float maxHealth { get; set; }
        public float health { get; set; }
        public float dmg { get; set; }
        public float dmgMod { get; set; }
        public int AR { get; set; }
        public long xp { get; set; }
        public int lvl { get; set; }
        public int distWalked { get; set; }
        public int lvlXpCap { get; set; }
        public int clockDay { get; set; }
        public int clockHour { get; set; }
        public int clockMinute { get; set; }
        public Map.Stage stage { get; set; }

    }

    //write all player data;
    public class SaveData
    {
        static JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true, // Adds \n and indentation
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Optional: camelCase naming
        };

        public static void Save()
        {
            var data = new Data();
            
            //data.stageItemsAvailable = new List<string>();
            data.inventory = new Dictionary<string, int>();
            data.stats = new Dictionary<string, int>();

            if (data.inventory != null)
                data.inventory.Clear();
            /*
            if (data.stageItemsAvailable != null)
                data.stageItemsAvailable.Clear();*/
            if (data.stats != null)
                data.stats.Clear();

            for (int i = 0; i < Game.player.inventory.Count; i++)
            {
                data.inventory[Game.player.inventory[i].name] = (Game.player.inventory[i].currentStack);
            }

            /*
            for (int i = 0; i < Game.player.stageItemsAvailable.Count; i++)
            {
                data.stageItemsAvailable.Add(Game.player.stageItemsAvailable[i].name);
            }
            */
            foreach(KeyValuePair<Stat.StatName, int> kvp in Game.player.role.roleStats)
            {
                data.stats[kvp.Key.ToString()] = kvp.Value;
            }

            data.roleName = Game.player.role.roleName.ToString();
            data.currentWeapon = Game.player.currentWeapon.name;
            data.currentArmor = Game.player.currentArmor.name;
            data.name = Game.player.name;
            data.maxHealth = Game.player.maxHealth;
            data.health = Game.player.health;
            data.dmg = Game.player.dmg;
            data.dmgMod = Game.player.dmgMod;
            data.AR = Game.player.AR;
            data.xp = Game.player.xp;
            data.lvl = Game.player.lvl;
            data.distWalked = Game.player.distWalked;
            data.lvlXpCap = Game.player.lvlXpCap;
            data.clockDay = Clock.day;
            data.clockHour = Clock.hour;
            data.clockMinute = Clock.minute;
            data.stage = Game.map.currentStage;


            string fileName = "PlayerData.json";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,fileName);
            string jsonString = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, jsonString);

            Text.Color("Saved player data", Text.TC.E);
        }

        public static void NewSave()
        {
            var data = new Data();
            Game.player.resetPlayer();

            //data.stageItemsAvailable = new List<string>();
            data.inventory = new Dictionary<string, int>();


            for (int i = 0; i < Game.player.inventory.Count; i++)
            {
                data.inventory[Game.player.inventory[i].name] = (Game.player.inventory[i].currentStack);
            }
            /*
            for (int i = 0; i < Game.player.stageItemsAvailable.Count; i++)
            {
                data.stageItemsAvailable.Add(Game.player.stageItemsAvailable[i].name);
            }
            */
            data.currentWeapon = Game.player.currentWeapon.name;
            data.currentArmor = Game.player.currentArmor.name;
            data.name = Game.player.name;
            data.maxHealth = Game.player.maxHealth;
            data.health = Game.player.health;
            data.dmg = Game.player.dmg;
            data.dmgMod = Game.player.dmgMod;
            data.AR = Game.player.AR;
            data.xp = Game.player.xp;
            data.lvl = Game.player.lvl;
            data.distWalked = Game.player.distWalked;
            data.lvlXpCap = Game.player.lvlXpCap;
            data.clockDay = Clock.day;
            data.clockHour = Clock.hour;
            data.clockMinute = Clock.minute;
            data.stage = Game.map.currentStage;

            string fileName = "PlayerData.json";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string jsonString = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, jsonString);

            Text.Color("Saved player data", Text.TC.E);
        }

        public static void Load()
        {
            try
            {
                string fileName = "PlayerData.json";
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                string jsonString = File.ReadAllText(filePath);
                Data data = JsonSerializer.Deserialize<Data>(jsonString)!;

                if (Game.player.inventory != null)
                    Game.player.inventory.Clear();
                if (Game.player.stageItemsAvailable != null)
                    Game.player.stageItemsAvailable.Clear();

                foreach (KeyValuePair<string, int> kvp in data.inventory)
                {
                    //creates a copy of the item from the item manager so that it does not effect the current
                    //stack value of the item permanently
                    Item item = ItemManager.getItem(kvp.Key);
                    item.currentStack = kvp.Value;
                    Game.player.inventory.Add(item);
                    Game.player.pm.getInvItem(item.name).currentStack = kvp.Value;
                }

                Map m = new Map();
                m.setStage(data.stage);
                /*
                for (int i = 0; i < data.stageItemsAvailable.Count; i++)
                {
                    Game.player.stageItemsAvailable.Add(ItemManager.getItem(data.stageItemsAvailable[i]));
                }
                */
                Role.buildRoleIndex();
                Role.assignRole(Role.getRole((RoleName)Enum.Parse(typeof(RoleName), data.roleName)));

                if (Game.player.role.roleStats != null) // IF LIST IS NOT EMPTY
                    Game.player.role.roleStats.Clear(); // CLEAR IT

                foreach (KeyValuePair<string, int> kvp in data.stats)
                {
                    Enum.TryParse(kvp.Key, out Stat.StatName statName);
                    Game.player.role.roleStats[statName] = kvp.Value;
                }

                Game.player.currentWeapon = (Weapon)Game.player.pm.getInvItem(data.currentWeapon);
                Game.player.currentWeapon.equipped = true;
                if(data.currentArmor != "Nakey")
                {
                    Game.player.currentArmor = (Armor)Game.player.pm.getInvItem(data.currentArmor);
                    Game.player.currentArmor.equipped = true;
                }
                Game.player.name = data.name;
                Game.player.maxHealth = data.maxHealth;
                Game.player.health = data.health;
                Game.player.dmg = data.dmg;
                Game.player.dmgMod = data.dmgMod;
                Game.player.AR = data.AR;
                Game.player.xp = data.xp;
                Game.player.lvl = data.lvl;
                Game.player.distWalked = data.distWalked;
                Game.player.lvlXpCap = data.lvlXpCap;
                Game.map.setStage(data.stage);
                Clock.setClock(data.clockDay, data.clockHour, data.clockMinute);
            }
            catch (Exception ex)
            {
                Text.Color($"Error loading player data: {ex.Message}", Text.TC.R);
                Text.Continue();
                Environment.Exit(0);
            }
        }

        public static void Delete()
        {
            File.Delete("PlayerData.json");
        }
    }

    //create load data method
}
