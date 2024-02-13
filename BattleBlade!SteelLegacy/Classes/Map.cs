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
                    Text.Color("Would you like to progress to the next stage?", Text.TC.c);
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
                Enemy.stageEnemies.Clear();
                Enemy.currentEnemy.AddAllEnemiesInStage(1);

                //SETTING STAGE ITEMS
                Game.player.stageItemsAvailable.Clear();
                Item.AddAllItemsInStage(1);
                Game.player.stageItemsAvailable.Add(Item.getItem("health potion"));
            }
            else if (currentStage == Stage.second)
            {
                Enemy.stageEnemies.Clear();
                Enemy.currentEnemy.AddAllEnemiesInStage(2);

                Game.player.stageItemsAvailable.Clear();
                Item.AddAllItemsInStage(2);
                Item.AddAllItemsInStage(7);
            }
            else if (currentStage == Stage.third)
            {
                Enemy.stageEnemies.Clear();
                Enemy.currentEnemy.AddAllEnemiesInStage(3);

                Game.player.stageItemsAvailable.Clear();
                Item.AddAllItemsInStage(3);
                Item.AddAllItemsInStage(7);
            }
            else if (currentStage == Stage.fourth)
            {
                Enemy.stageEnemies.Clear();
                Enemy.currentEnemy.AddAllEnemiesInStage(4);

                Game.player.stageItemsAvailable.Clear();
                Item.AddAllItemsInStage(4);
                Item.AddAllItemsInStage(7);
            }
            else if (currentStage == Stage.fifth)
            {
                Enemy.stageEnemies.Clear();
                Enemy.currentEnemy.AddAllEnemiesInStage(5);

                Game.player.stageItemsAvailable.Clear();
                Item.AddAllItemsInStage(5);
                Item.AddAllItemsInStage(7);
            }
            else if (currentStage == Stage.last)
            {
                Enemy.stageEnemies.Clear();
                Enemy.currentEnemy.AddAllEnemiesInStage(6);

                Game.player.stageItemsAvailable.Clear();
                Item.AddAllItemsInStage(6);
                Item.AddAllItemsInStage(7);
            }
        }
    }

}
