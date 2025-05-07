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
        public string bType;

        public static Random rand = new Random();

        public Block(int _x, int _y, int _hp, string _bType)
        {
            x = _x;
            y = _y;
            hp = _hp;
            bType = _bType;
        }
    }


}
