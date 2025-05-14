using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml;
using System.Dynamic;

namespace BrickBreaker
{
    public class Block
    {
        public int width = 60;
        public int height = 60;
          
        public int x;
        public int y; 
        public int hp;
        public int fullHp;
        public string bType;

        public Image blockImage;
        public Image durabilityImage = Properties.Resources.noBreak;

        public static Random rand = new Random();

        public Block(int _x, int _y, int _hp, string _bType)
        {
            x = _x;
            y = _y;
            hp = _hp;
            fullHp = _hp;
            bType = _bType;

            if (_bType == "Dirt")
            {
                blockImage = Properties.Resources.dirtBlock;
            }
            else if (_bType == "Stone")
            {
                blockImage = Properties.Resources.stoneBlock;
            }
            else if (_bType == "Iron")
            {
                blockImage = Properties.Resources.ironBlock;
            }
            else if (_bType == "Diamond")
            {
                blockImage = Properties.Resources.diamondBlock;
            }
            else if (_bType == "Obsidian")
            {
                blockImage = Properties.Resources.obsidianBlock;
            }
            else if (_bType == "Netherack")
            {
                blockImage = Properties.Resources.netherackBlock;
            }
            else if (_bType == "Netherite")
            {
                blockImage = Properties.Resources.netheriteBlock;
            }
            else if (_bType == "Endstone")
            {
                blockImage = Properties.Resources.endstoneBlock;
            }
            else if (_bType == "Dragon")
            {
                blockImage = Properties.Resources.dragonBlock;
            }
        }

        public static void BlockBreaking(Block b)
        {
                if (b.hp <= b.fullHp / 4)
                {
                    b.durabilityImage = Properties.Resources.threeQuartersBreak;
                }
                else if(b.hp <= b.fullHp / 2)
                {
                    b.durabilityImage = Properties.Resources.halfBreak;
                }
                else if (b.hp <= b.fullHp * 3 / 4)
                {
                    b.durabilityImage = Properties.Resources.oneQuartersBreak;
                }
                else if (b.hp == b.fullHp)
                {
                    b.durabilityImage = Properties.Resources.noBreak;
                }
        }
    }


}
