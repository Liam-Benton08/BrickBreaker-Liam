using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreaker
{
    internal class Powerups
    {

        public static int poweractivate, damage;
        // btfootb = beat the fuck out of those blocks
        public static bool btfootb;
        // list of extra balls
        public static List<Ball> extraballs = new List<Ball>();

        public Powerups()
        {
            
        }
        // Makes the player move two pixels faster
        public static void Speed_I(Paddle p)
        {
            p.speed = 8 + 2;
        }
        // Makes the player move four pixels faster
        public static void Speed_II(Paddle p)
        {
            p.speed = 8 + 4;
        }
        // Makes the player move six pixels faster
        public static void Speed_III(Paddle p)
        {
            p.speed = 8 + 6;
        }
        // Adds a life to the player
        public static void Golden_Carrot()
        {
            if (GameScreen.lives > 3)
            {
                GameScreen.lives++;
            }
        }
        // Adds three life to the player
        public static void Golden_Apple()
        {
            if (GameScreen.lives >= 3)
            {
                GameScreen.lives = 4;
            }
        }
        // Adds one more ball to the field
        public static void Slime()
        {
            //foreach (Ball ball in extraballs)
            //{
            //    Ball eb = new Ball(ballX, ballY, xSpeed, ySpeed, ballSize);
            //    extraballs.Add(eb);
            //}
        }
        // Upgrades to stone tool, does 1 extra damage
        public static void stonetool()
        {
            damage = 2;
        }
        // Upgrades to iron tool, does 2 extra damage
        public static void irontool()
        {
            damage = 3;
        }
        // Upgrades to diamond tool, does 3 extra damage
        public static void diamondtool()
        {
            damage = 4;
        }
        // Upgrades to netherite tool, does 4 extra damage, can break unbreakable blocks
        public static void netheritetool()
        {
            damage = 5;
            btfootb = true;
        }
    }
}
