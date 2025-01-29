using BattleBlade_SteelLegacy.Classes;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static BattleBlade_SteelLegacy.Classes.Stat;
using static BattleBlade_SteelLegacy.Classes.Text;
using static System.Net.Mime.MediaTypeNames;

namespace BattleBlade_SteelLegacy.Classes
{
    public class Role
    {
        public string desc = "";
        public string god = "";
        public int roleLevel = 0;
        public static List<Role> roles = new List<Role>();
        public Dictionary<Stat.StatName, int> roleStats = new Dictionary<Stat.StatName, int>();
        public RoleName roleName;

        public enum RoleName
        {
            Thief,
            Knight,
            Sorcerer,
            Archer,
            Barren,
            Blest
        }
        public static void newRole(RoleName roleName, string god, string desc, List<int> statValues)
        {
            Role role = new Role();

            role.roleName = roleName;
            role.god = god;
            role.desc = desc;

            for (int i = 0; i < statValues.Count; i++)
            {
                role.roleStats[Game.statNames[i]] = statValues[i];
            }

            int total = 0;
            foreach (KeyValuePair<Stat.StatName, int> kvp in role.roleStats)
            {
                total += kvp.Value;
            }
            role.roleLevel = total;
            roles.Add(role);
        }

        public void buildAssignedItems(RoleName roleName)
        {
            switch (roleName)
            {
                case RoleName.Knight:
                    SetDefaultItem("sword");
                    break;
                case RoleName.Thief:
                    SetDefaultItem("dagger");
                    break;
                case RoleName.Sorcerer:
                    SetDefaultItem("light staff");
                    break;
                case RoleName.Archer:
                    SetDefaultItem("short bow");
                    break;
                case RoleName.Blest:
                    SetDefaultItem("hatchet");
                    break;
                case RoleName.Barren:
                    SetDefaultItem("fist");
                    break;
                // Add more cases for other roles as needed
                default:
                    SetDefaultItem("fist");
                    break;
            }
        }
        public void SetDefaultItem(string name)
        {
            Game.player.classDefaultWeapon = (Weapon)ItemManager.getItem(name);
            Game.player.am.adminPickUp(Game.player.classDefaultWeapon);
            Game.player.am.Equip(Game.player.classDefaultWeapon);
        }

        public static void assignRole(Role role)
        {
            Game.player.role = role;
        }
        public static Role getRole(RoleName roleName)
        {
            foreach (Role role in roles)
            {
                if (roleName == role.roleName)
                {
                    return role;
                }
            }
            Text.InvalidInput("ERROR Could not find role of name " + roleName);
            return null;
        }
        public static void buildRoleIndex()
        {
            newRole(RoleName.Thief, "Godess Vica", "A swift and cunning assassin type class, " +
                "proficient with daggers and knives. \nGodess Vica watches " +
                "over all thieves.",//      s  i  l  v   s  p  f  v
                statValues: new List<int> { 4, 8, 7, 4, 11, 7, 0, 5 });
            newRole(RoleName.Knight, "God Beram", "A strong and sturdy breed, the knights " +
                "protect and serve their king, Napir. \nProficient with mighty" +
                " swords and hammers. God Beram watches over all knights.", 
                statValues: new List<int> { 8, 3, 4, 8, 2, 5, 0, 5 });
            newRole(RoleName.Sorcerer, "Godess Kakia", "The mages of this land, sorcerers are " +
                "highly intelligent, yet lack the physical \nprowess needed " +
                "to wield a sword. Godess Kakia watches over all sorcerers.",
                statValues: new List<int> { 2, 11, 6, 4, 5, 9, 3, 5 });
            newRole(RoleName.Archer, "God Coses", "Precise and swift, these kind are deadly from " +
                "long range. Proficient with bows and \ncrossbows. God Coses" +
                " watches over all archers.",
                statValues: new List<int> { 3, 7, 6, 4, 7, 11, 0, 5 });
            newRole(RoleName.Barren, "God Bast", "A shell of who they once were. They were not" +
                " fit to live this world, yet they refuse \nto give up. God Bast" +
                " watches over all barren.",
                statValues: new List<int> { 1, 1, 0, 1, 1, 1, 0, 0 });
            newRole(RoleName.Blest, "God Osper", "Not particularly skillful in any area, these" +
                " kind survive on pure fortune alone. \nGod Osper watches over " +
                "all blest.",
                statValues: new List<int> { 4, 4, 12, 4, 4, 4, 7, 12 });
        }

        public static float calculateHealth(int lvl)
        {
            float healthMultiplier = 1.1f;
            float baseHealth = 68.302f;
            float newHealth;

            newHealth = baseHealth * (float)(Math.Pow(healthMultiplier, lvl));
            return newHealth;
        }
        public void raiseStat(Stat.StatName name)
        {
            foreach (KeyValuePair < Stat.StatName, int> kvp in roleStats)
            {
                if (kvp.Key == name)
                {
                    int value = kvp.Value + 1;
                    roleStats[name] = value;

                    Text.Color(name + " was increased by 1!", Text.TC.Y);
                    Text.Continue();

                    switch (name)
                    {
                        case Stat.StatName.Strength:
                            // make strength change something
                            return;
                        case Stat.StatName.Luck:
                            // make item discovery increase
                            return;
                        case Stat.StatName.Vigor:
                            Game.player.maxHealth = calculateHealth(Game.player.role.getStat(Stat.StatName.Vigor));
                            return;
                        case Stat.StatName.Speed:
                            // make speed change something
                            return;
                        case Stat.StatName.Intelligence:
                            // make intelligence change something
                            return;
                        case Stat.StatName.Precision:
                            // make precision change something
                            return;
                        default:
                            Text.Color("ERROR Could not find stat of name " + name, Text.TC.R);
                            Text.Continue();
                            return;
                    }
                }
            }
            Text.Color("ERROR Could not find stat of name " + name, Text.TC.R);
            Text.Continue();
            return;
        }
        public void lowerStat(Stat.StatName name)
        {
            foreach (KeyValuePair< Stat.StatName, int> kvp in roleStats)
            {
                if (kvp.Key == name)
                {
                    int value = kvp.Value - 1;
                    roleStats[name] = value;

                    Text.Color(name + " was decreased by 1!", Text.TC.Y);
                    Text.Continue();
                    return;
                }
            }
            Text.Color("ERROR Could not find stat of name " + name, Text.TC.R);
            Text.Continue();
        }
        public void setStat(Stat.StatName name, int value)
        {
            foreach (KeyValuePair<Stat.StatName, int> kvp in roleStats)
            {
                if (kvp.Key == name)
                {
                    roleStats[name] = value;
                    return;
                }
            }
            Text.Color("ERROR Could not find stat of name " + name, Text.TC.R);
            Text.Continue();
        }
        /// <summary>
        /// Returns stat value
        /// </summary>
        /// <param name="name">name of stat to check value of</param>
        /// <returns></returns>
        public int getStat(Stat.StatName name)
        {
            foreach (KeyValuePair<Stat.StatName, int> kvp in roleStats)
            {
                if (kvp.Key == name)
                {
                    return roleStats[name];
                }
            }
            Text.Color("ERROR Could not find stat of name " + name, Text.TC.R);
            Text.Continue();
            return 0;
        }
    }
}
