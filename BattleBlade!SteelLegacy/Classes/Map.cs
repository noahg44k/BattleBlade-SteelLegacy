using BattleBlade_SteelLegacy.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBlade_SteelLegacy.Classes
{

    public class Map
    {
        public Stage currentStage = Stage.first;
        public enum Stage
        {
            first = 1,
            second = 2,
            third = 3,
            fourth = 4,
            fifth = 5,
            last = 6
        }

        public void setStage(Stage newStage)
        {
            currentStage = newStage;
        }
        public void nextStage()
        {
            setStage(getNextStage());
            Text.Color("Congratulations! You progressed to the " + currentStage + " stage!", Text.TC.Y);
            Text.Continue();
        }
        public Stage getStage()
        {
            return currentStage;
        }
        public Stage getNextStage()
        {
            return getStage() + 1;
        }
        public void changeStage()
        {
            while (true)
            {
                Graphics.PrintTitleCard();
                if (getStage() != Stage.last)
                {
                    Text.Color($"Would you like to progress to the {currentStage} stage?", Text.TC.c);
                    Text.Instructions("y or n");
                    string choice = Game.GetPlayerInput();

                    if (choice.Contains("y"))
                    {
                        nextStage();
                        return;
                    }
                    else if (choice.Contains("n"))
                    {
                        return;
                    }
                    else
                    {
                        Text.InvalidInput();
                    }
                }
                else
                {

                }
            }
        }
        public void checkStage()
        {
            switch (Game.player.lvl)
            {
                case 1:
                    setStage(Stage.first);
                    break;
                case 2:
                case 3:
                    changeStage();
                    break;
                case 4:
                    changeStage();
                    break;
                case 5:
                    changeStage();
                    break;
                case 6:
                    changeStage();
                    break;
                case 7:
                    changeStage();
                    break;
                case 8:
                    changeStage();
                    break;
                case 9:
                    changeStage();
                    break;
                case 10:
                    changeStage();
                    break;
                case 11:
                    changeStage();
                    break;
                case 12:
                    changeStage();
                    break;
                case 13:
                    changeStage();
                    break;
                case 14:
                    changeStage();
                    break;
                case 15:
                    changeStage();
                    break;
                case 16:
                    changeStage();
                    break;
                case 17:
                    changeStage();
                    break;
                case 18:
                    changeStage();
                    break;
                default:
                    setStage(Stage.last);
                    break;
            }
            if (currentStage == Stage.first)
            {
                //SETTING STAGE ENEMIES
                EnemyManager.stageEnemies.Clear();
                EnemyManager.AddAllEnemiesInStage(1);

                //SETTING STAGE ITEMS
                Game.player.stageItemsAvailable.Clear();
                ItemManager.AddAllItemsInStage(1);
                Game.player.stageItemsAvailable.Add(ItemManager.getItem("health potion"));
            }
            else if (currentStage == Stage.second)
            {
                EnemyManager.stageEnemies.Clear();
                EnemyManager.AddAllEnemiesInStage(2);

                Game.player.stageItemsAvailable.Clear();
                ItemManager.AddAllItemsInStage(2);
                ItemManager.AddAllItemsInStage(7);
            }
            else if (currentStage == Stage.third)
            {
                EnemyManager.stageEnemies.Clear();
                EnemyManager.AddAllEnemiesInStage(3);

                Game.player.stageItemsAvailable.Clear();
                ItemManager.AddAllItemsInStage(3);
                ItemManager.AddAllItemsInStage(7);
            }
            else if (currentStage == Stage.fourth)
            {
                EnemyManager.stageEnemies.Clear();
                EnemyManager.AddAllEnemiesInStage(4);

                Game.player.stageItemsAvailable.Clear();
                ItemManager.AddAllItemsInStage(4);
                ItemManager.AddAllItemsInStage(7);
            }
            else if (currentStage == Stage.fifth)
            {
                EnemyManager.stageEnemies.Clear();
                EnemyManager.AddAllEnemiesInStage(5);

                Game.player.stageItemsAvailable.Clear();
                ItemManager.AddAllItemsInStage(5);
                ItemManager.AddAllItemsInStage(7);
            }
            else if (currentStage == Stage.last)
            {
                EnemyManager.stageEnemies.Clear();
                EnemyManager.AddAllEnemiesInStage(6);

                Game.player.stageItemsAvailable.Clear();
                ItemManager.AddAllItemsInStage(6);
                ItemManager.AddAllItemsInStage(7);
            }
        }
    }

}
