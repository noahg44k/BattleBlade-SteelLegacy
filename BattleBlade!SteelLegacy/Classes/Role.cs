using BattleBlade_SteelLegacy.Classes;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
        public List<Stat> roleStats = new List<Stat>();
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

        public static void newRole(RoleName roleName, string god, string desc, int str, int luck, int vig, int spd, int inl, int prc)
        {
            Role role = new Role();

            role.roleName = roleName;
            role.god = god;
            role.desc = desc;

            role.roleStats.AddRange(new[]
            {
                new Stat().newStat(Stat.StatName.Strength, str),
                new Stat().newStat(Stat.StatName.Luck, luck),
                new Stat().newStat(Stat.StatName.Vigor, vig),
                new Stat().newStat(Stat.StatName.Speed, spd),
                new Stat().newStat(Stat.StatName.Intelligence, inl),
                new Stat().newStat(Stat.StatName.Precision, prc)
            });

            int total = 0;
            foreach (Stat stat in role.roleStats)
            {
                total += stat.value;
            }
            role.roleLevel = total;
            roles.Add(role);
        }

        public void buildAssignedItems(RoleName roleName)
        {
            switch (roleName)
            {
                case RoleName.Knight:
                    Game.player.am.adminPickUp(Item.getItem("sword"));
                    break;
                case RoleName.Thief:
                    Game.player.am.adminPickUp(Item.getItem("dagger"));
                    break;
                case RoleName.Sorcerer:
                    Game.player.am.adminPickUp(Item.getItem("light staff"));
                    break;
                case RoleName.Archer:
                    Game.player.am.adminPickUp(Item.getItem("short bow"));
                    break;
                case RoleName.Blest:
                    Game.player.am.adminPickUp(Item.getItem("hatchet"));
                    break;
                // Add more cases for other roles as needed
                default:
                    break;
            }
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
                "over all thieves.", 4, 8, 5, 11, 6, 7);
            newRole(RoleName.Knight, "God Beram", "A strong and sturdy breed, the knights " +
                "protect and serve their king, Napir. \nProficient with mighty" +
                " swords and hammers. God Beram watches over all knights."
                , 11, 6, 9, 4, 3, 5);
            newRole(RoleName.Sorcerer, "Godess Kakia", "The mages of this land, sorcerers are " +
                "highly intelligent, yet lack the physical \nprowess needed " +
                "to wield a sword. Godess Kakia watches over all sorcerers."
                , 2, 5, 4, 6, 11, 9);
            newRole(RoleName.Archer, "God Coses", "Precise and swift, these kind are deadly from " +
                "long range. Proficient with bows and \ncrossbows. God Coses" +
                " watches over all archers.", 7, 3, 4, 9, 7, 11);
            newRole(RoleName.Barren, "God Bast", "A shell of who they once were. They were not" +
                " fit to live this world, yet they refuse \nto give up. God Bast" +
                " watches over all barren.", 1, 1, 1, 1, 1, 1);
            newRole(RoleName.Blest, "God Osper", "Not particularly skillful in any area, these" +
                " kind survive on pure fortune alone. \nGod Osper watches over " +
                "all blest.", 4, 12, 4, 4, 4, 4);
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
            foreach (Stat stat in roleStats)
            {
                if (stat.name == name)
                {
                    stat.value++;

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
                            Game.player.maxHealth = calculateHealth(Game.player.role.getStat(Stat.StatName.Vigor).value);
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
            foreach (Stat stat in roleStats)
            {
                if (stat.name == name)
                {
                    stat.value--;

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
            foreach (Stat stat in roleStats)
            {
                if (stat.name == name)
                {
                    stat.value = value;
                    return;
                }
            }
            Text.Color("ERROR Could not find stat of name " + name, Text.TC.R);
            Text.Continue();
        }

        public Stat getStat(Stat.StatName name)
        {
            foreach (Stat stat in roleStats)
            {
                if (stat.name == name)
                {
                    return stat;
                }
            }
            Text.Color("ERROR Could not find stat of name " + name, Text.TC.R);
            Text.Continue();
            return null;
        }
    }
}
