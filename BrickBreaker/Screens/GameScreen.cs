/*  Created by: 
 *  Project: Brick Breaker
 *  Date: 
 */ 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Xml;

namespace BrickBreaker
{
    public partial class GameScreen : UserControl
    {
        #region global values

        //player1 button control keys - DO NOT CHANGE
        Boolean leftArrowDown, rightArrowDown;

        // Game values
        Random Randgen = new Random();
        public static int lives;
        public static int SlimeNum;
        int count;
        int powerupchance;
        int poweruptype;
        int level = 1;

        // Paddle and Ball objects
        Paddle paddle;
        Ball ball;

        // list of all blocks for current level
        public List<Block> blocks = new List<Block>();

        // Brushes
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        SolidBrush redBrush = new SolidBrush(Color.Red);


        public Rectangle heartBox1 = new Rectangle(0, 400, 50, 50);
        public Rectangle heartBox2 = new Rectangle(0, 400, 50, 50);
        public Rectangle heartBox3 = new Rectangle(0, 475, 50, 50);

        #endregion

        public GameScreen()
        {
            InitializeComponent();
            OnStart();
        }

        public void OnStart()
        {
            //set life counter
            lives = 3;

            //set all button presses to false.
            leftArrowDown = rightArrowDown = false;
            
            // setup starting paddle values and create paddle object
            int paddleWidth = 150;
            int paddleHeight = 20;
            int paddleX = ((this.Width / 2) - (paddleWidth / 2));
            int paddleY = (this.Height - paddleHeight) - 20;
            int paddleSpeed = 8;
            paddle = new Paddle(paddleX, paddleY, paddleWidth, paddleHeight, paddleSpeed, Color.White);

            // setup starting ball values
            int ballX = this.Width / 2 - 10;
            int ballY = this.Height - paddle.height - 40;

            // Creates a new ball
            int xSpeed = 6;
            int ySpeed = 6;
            int ballSize = 20;
            ball = new Ball(ballX, ballY, xSpeed, ySpeed, ballSize);

            #region Creates blocks for generic level. Need to replace with code that loads levels.


            //TODO - replace all the code in this region eventually with code that loads levels from xml files

            ExtractLevel();




            //blocks.Clear();
            //int x = 10;

            //while (blocks.Count < 12)
            //{
            //    x += 57;
            //    Block b1 = new Block(x, 10, 1, "White");
            //    blocks.Add(b1);
            //}

            #endregion

            // start the game engine loop
            gameTimer.Enabled = true;
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player 1 button presses
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                case Keys.Escape:
                    this.FindForm().Close();
                    break;
                default:
                    break;
            }
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player 1 button releases
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
                default:
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            // Move the paddle
            if (leftArrowDown && paddle.x > 0)
            {
                paddle.Move("left");
            }
            if (rightArrowDown && paddle.x < (this.Width - paddle.width))
            {
                paddle.Move("right");
            }
            
            // Move ball
            ball.Move();

            // Check for collision with top and side walls
           ball.WallCollision(this);

            // Check for ball hitting bottom of screen
            if (ball.BottomCollision(this))
            {
                lives--;

                // Moves the ball back to origin
                ball.x = ((paddle.x - (ball.size / 2)) + (paddle.width / 2));
                ball.y = (this.Height - paddle.height) - 85;


                if (lives == 0)
                {
                    gameTimer.Enabled = false;
                    OnEnd();
                }
            }

            // Check for collision of ball with paddle, (incl. paddle movement)
            ball.PaddleCollision(paddle);

            // Check if ball has collided with any blocks
            foreach (Block b in blocks)
            {
                if (ball.BlockCollision(b))
                {
                    b.hp--;
                    Block.BlockBreaking(b);

                    if (b.hp == 0)
                    {
                        blocks.Remove(b);
                    }

                    if (blocks.Count == 0)
                    {
                        level++;

                        if(level == 7)
                        {
                            gameTimer.Stop();
                            OnEnd();
                        }
                        else if (level < 7)
                        {
                            ExtractLevel();
                        }
                    }

                    powerupchance = Randgen.Next(0, 101);

                    if (powerupchance <= 20)
                    {

                    }
                    break;
                }
            }

            //Check if ball is pushed out of bounds, reset ball
            if (ball.PushedOutOfBounds(paddle, this))
            {

            }

            //redraw the screen
            Refresh();
        }

        public void OnEnd()
        {
            // Goes to the game over screen
            Form form = this.FindForm();
            MenuScreen ps = new MenuScreen();
            
            ps.Location = new Point((form.Width - ps.Width) / 2, (form.Height - ps.Height) / 2);

            form.Controls.Add(ps);
            form.Controls.Remove(this);
        }

        public void ExtractLevel()
        {
            XmlReader reader = XmlReader.Create($"Resources/level{level}.xml");

            while (reader.Read())
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Text)
                    {
                        int x = Convert.ToInt16(reader.ReadString());

                        reader.ReadToNextSibling("y");
                        int y = Convert.ToInt16(reader.ReadString());

                        reader.ReadToNextSibling("hp");
                        int hp = Convert.ToInt16(reader.ReadString());

                        reader.ReadToNextSibling("bType");
                        string bType = reader.ReadString();

                        Block b = new Block(x, y, hp, bType);
                        blocks.Add(b);
                    }
                }
                reader.Close();

            }
        }


        private void label2_Click(object sender, EventArgs e)
        {
            blocks.Clear();

            level++;

            if (level == 7)
            {
                gameTimer.Enabled = false;
                OnEnd();
            }
            else if (level < 7)
            {
                ExtractLevel();
            }
        }

        public void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            // Draws paddle
            whiteBrush.Color = paddle.colour;
            e.Graphics.FillRectangle(whiteBrush, paddle.x, paddle.y, paddle.width, paddle.height);

            // Draws blocks
            foreach (Block b in blocks)
            {
                e.Graphics.DrawImage(b.blockImage, b.x, b.y, b.width, b.height);
                e.Graphics.DrawImage(b.durabilityImage, b.x, b.y, b.width, b.height);
            }

            // Draws ball
            e.Graphics.FillRectangle(whiteBrush, ball.x, ball.y, ball.size, ball.size);

            //Draws hearts

           
            //switch (lives)
            //{
            //    case 3:
            //        ///e.Graphics.FillRectangle(whiteBrush, heartBox1);
            //        e.Graphics.DrawImage(Properties.Resources.heartIcon, heartBox1);
            //        e.Graphics.DrawImage(Properties.Resources.heartIcon, heartBox2);
            //        e.Graphics.DrawImage(Properties.Resources.heartIcon, heartBox3);
            //        break;
            //    case 2:
            //        e.Graphics.DrawImage(Properties.Resources.heartIcon, heartBox1);
            //        e.Graphics.DrawImage(Properties.Resources.heartIcon, heartBox2);
            //        e.Graphics.DrawImage(Properties.Resources.emptyHeartIcon, heartBox3);
            //        break;
            //    case 1:
            //        e.Graphics.DrawImage(Properties.Resources.heartIcon, heartBox1);
            //        e.Graphics.DrawImage(Properties.Resources.emptyHeartIcon, heartBox2);
            //        e.Graphics.DrawImage(Properties.Resources.emptyHeartIcon, heartBox3);
            //        break;
            //}
        }
    }
}
