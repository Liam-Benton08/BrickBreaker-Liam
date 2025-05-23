using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrickBreaker
{
    public class Ball
    {
        public int x, y, xSpeed, ySpeed, size, lastX;
        public Color colour;

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

            bool verticalCollision = false, horizontalCollision = false;

            if (ballRec.IntersectsWith(blockRec))
            {
                if (y + size > b.y) // ball hits bottom of block
                {
                    ySpeed *= -1;
                    y = y + 2;   // slightly bigger push
                    verticalCollision = true;
            }
                else if (y < b.y + b.height) // ball hits top of block
                {
                    ySpeed *= -1;
                    y = y - 2;
                    horizontalCollision = true;
                }

                if (x + size > b.x && x < b.x) // ball hits left side
                {
                    xSpeed *= -1;
                    if (!verticalCollision)
                    {
                        x = x - 2;
                    }
                horizontalCollision = true;
                }
                else if (x + size > b.x + b.width && x < b.x + b.width) // ball hits right side
                {
                    xSpeed *= -1;
                if (!verticalCollision)
                {
                
                    x = x + 2;
                }
                horizontalCollision = true;
                }
            }

            return blockRec.IntersectsWith(ballRec);
        }


        public bool LuckCollision(Paddle p)
        {
            Rectangle ballRec = new Rectangle(x, y, size, size);
            Rectangle paddleRec = new Rectangle(p.x, p.y, p.width, p.height);

            if (ballRec.IntersectsWith(paddleRec))
            {
                //Powerups.poweractivate = 1;
                return true;
            }

            return false;
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
                    xSpeed = 4;

                    if (lastX > x)
                    {
                        xSpeed *= -1;
                    }
                    
                }
                else if (ballCentre > p.x + pSectionWidth * 2) //hit right side
                {
                    xSpeed = 4;

                    if (lastX > x)
                    {
                        xSpeed *= -1;
                    }
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

                ySpeed = -ySpeed;
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
