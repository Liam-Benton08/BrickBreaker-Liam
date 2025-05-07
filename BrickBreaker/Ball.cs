﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrickBreaker
{
    public class Ball
    {
        public int x, y, xSpeed, ySpeed, size, lastX;
        public Color colour;

        public static Random rand = new Random();

        public Ball(int _x, int _y, int _xSpeed, int _ySpeed, int _ballSize)
        {
            x = _x;
            y = _y;
            xSpeed = _xSpeed;
            ySpeed = _ySpeed;
            size = _ballSize;

        }

        public void Move()
        {
            x = x + xSpeed;
            y = y + ySpeed;
        }

        public bool BlockCollision(Block b)
        {
            Rectangle blockRec = new Rectangle(b.x, b.y, b.width, b.height);
            Rectangle ballRec = new Rectangle(x, y, size, size);

            if (ballRec.IntersectsWith(blockRec))
            {
                ySpeed *= -1;

                if (y < b.y + b.height) //if ballY (top of ball) is more than blockY + blockHeight (bottom of block)
                {
                    xSpeed *= -1; //switch x direction (left and right)
                    //x = b.x + b.width; // move ball to right side
                }
            }

            return blockRec.IntersectsWith(ballRec);
        }

        public void PaddleCollision(Paddle p)
        {
            Rectangle ballRec = new Rectangle(x, y, size, size);
            Rectangle paddleRec = new Rectangle(p.x, p.y, p.width, p.height);


            if (ballRec.IntersectsWith(paddleRec))
            {
                ySpeed *= -1;
                

                int pSectionWidth = p.width / 3;
                int ballCentre = x + (size / 2);

                if (ballCentre < p.x + pSectionWidth) //hit left side
                {
                    xSpeed = 5;
                    xSpeed *= -1;
                }
                else if (ballCentre > p.x + pSectionWidth * 2) //hit right side
                {
                    xSpeed = 5;
                    xSpeed *= -1;
                }
                else //middle
                {
                    xSpeed = 2;
                }

                if (y + size > p.y) //if ballY + ballsize is more than paddleY (bottom of ball is more than top of paddle) //hitting paddle sides
                {
                    //xSpeed *= -1;

                    if (x < p.x) //hit left side
                    {
                        x = p.x - size; //move ball to the left side

                    }
                    else if (x + size > p.x + p.width + 1) //hit right side
                    {
                        x = p.x + p.width; // move ball to right side
                    }
                }
            }

            lastX = x;
            y -= 1;
        }

        public void WallCollision(UserControl UC)
        {
            // Collision with left wall
            if (x <= 0)
            {
                xSpeed *= -1;
            }
            // Collision with right wall
            if (x >= (UC.Width - size))
            {
                xSpeed *= -1;
            }
            // Collision with top wall
            if (y <= 2)
            {
                ySpeed *= -1;
                y = 1;
            }
        }

        public bool BottomCollision(UserControl UC)
        {
            Boolean didCollide = false;

            if (y >= UC.Height)
            {
                didCollide = true;
            }

            return didCollide;
        }


        public bool PushedOutOfBounds(Paddle p, UserControl UC)
        {
            Rectangle ballRec = new Rectangle(x, y, size, size);
            Rectangle paddleRec = new Rectangle(p.x, p.y, p.width, p.height);

            Boolean pushedOut = false;

            if (x >= UC.Width || x < 0 - size)
            {
                pushedOut = true;

                ySpeed = +ySpeed;
                x = p.x + p.width / 2;
                y = p.y - size;
            }

            return pushedOut;
        }

    }
}
