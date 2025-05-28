/*  Created by: 
 *  Project: Brick Breaker
 *  Date: 
 */ 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace BrickBreaker
{
    public partial class GameScreen : UserControl
    {
        #region global values

        //player1 button control keys - DO NOT CHANGE
        Boolean leftArrowDown, rightArrowDown;

        // Game values
        Random Randgen = new Random();
        public static int lives, slimex, slimey;
        int count;
        int powerupchance;
        int poweruptype;
        int level;

        int SpeedVariable;
        int DamageVariable;

        // Paddle and Ball objects
        public static Paddle paddle;
        Ball ball;

        // list of all blocks for current level
        public List<Block> blocks = new List<Block>();

        // list of powerup balls that has a chance to spawn when a block is hit
        public static List<Ball> powerupballs = new List<Ball>();

        //Pen
        Pen whitePen = new Pen (Color.White);

        // Brushes
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        SolidBrush redBrush = new SolidBrush(Color.Red);

        Rectangle heartBox0 = new Rectangle(0, 500, 50, 50);
        Rectangle heartBox1 = new Rectangle(0, 575, 50, 50);
        Rectangle heartBox2 = new Rectangle(0, 650, 50, 50);
        Rectangle heartBox3 = new Rectangle(0, 725, 50, 50);

        Image background;
        Image pickaxe;

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
            int ballY = this.Height - paddle.height - 60;

            // Creates a new ball
            int xSpeed = 6;
            int ySpeed = 6;
            int ballSize = 30;
            ball = new Ball(ballX, ballY, xSpeed, ySpeed, ballSize);

            dragonBarLabel.Visible = false;

            level = 1;

            ExtractLevel();

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

               //checks to see if the player loses
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
                    //take away certain amount of health
                    b.hp -= Powerups.damage;
                    Block.BlockBreaking(b);

                    //if the block is at 0 or lower remove it
                    if (b.hp <= 0)
                    {
                        blocks.Remove(b);

                        //Creates random number for powerups
                        powerupchance = Randgen.Next(0, 101);

                        //50 50 chance to make a new powerup
                        if (powerupchance <= 65)
                        {
                            Ball pub = new Ball(b.x + 20, b.y, 0, 200, 20);
                            powerupballs.Add(pub);
                        }
                    }
                    
                    break;
                }
            }

            //if screen is empty go to next level
            if (blocks.Count == 0)
            {
                level++;

                //delete extra balls
                while (Powerups.extraballs.Count > 0)
                {
                    foreach (Ball eb in Powerups.extraballs)
                    {
                        Powerups.extraballs.Remove(eb);
                        break;
                    }
                }
                //delete extra powerups
                while (powerupballs.Count > 0)
                {
                    foreach (Ball pub in powerupballs)
                    {
                        powerupballs.Remove(pub);
                        break;
                    }
                }

                //reset paddle and ball
                 paddle.x = ((this.Width / 2) - (150 / 2));
                 paddle.y = (this.Height - 20) - 20;

                 ball.x = this.Width / 2 - 10;
                 ball.y = this.Height - paddle.height - 60;

                //if game is over stop the clock
                if (level == 7)
                {
                    gameTimer.Stop();
                    OnEnd();
                }
                else if (level < 7)
                {         
                    ExtractLevel();
                }
            }

            // makes the powerup ball fall and checks if the powerup ball has been hit yet, if it has then it chooses a random powerup
            foreach (Ball pub in powerupballs)
            {
                pub.y++;

                if (pub.LuckCollision(paddle))
                {
                    int powerupselect;

                    //Checks to see if powerup is full, if it is full the powerup wont be created

                    if (SpeedVariable == 3 && DamageVariable == 4)
                    {
                        powerupselect = Randgen.Next(2, 5);
                    }
                    else if (SpeedVariable == 3)
                    {
                        powerupselect = Randgen.Next(2, 6);
                    }
                    else if (DamageVariable == 4)
                    {
                        powerupselect = Randgen.Next(1, 5);
                    }
                    else
                    {
                        powerupselect = Randgen.Next(1, 6);
                    }

                   
                    if (powerupselect == 1)
                    {
                        //SPEED POWERUP
                        powerUpLabel.Text = "SPEED";
                        SpeedVariable++;

                        if (SpeedVariable == 1)
                        {
                            Powerups.Speed_I(paddle);
                        }
                        else if (SpeedVariable == 2)
                        {
                            Powerups.Speed_II(paddle);
                        }
                        else if (SpeedVariable >= 3)
                        {
                            Powerups.Speed_III(paddle);
                        }

                    }
                    else if (powerupselect == 2)
                    {
                        //HEALTH POWERUP
                        powerUpLabel.Text = "GOLDEN CARROT";
                        Powerups.Golden_Carrot();
                    }
                    else if (powerupselect == 3)
                    {
                        //BETTER HEALTH POWERUP
                        powerUpLabel.Text = "GOLDEN APPLE";
                        Powerups.Golden_Apple();
                    }
                    else if (powerupselect == 4)
                    {
                        //ADDS NEW BALL POWERUP
                        powerUpLabel.Text = "SLIME";
                        slimex = pub.x;
                        slimey = pub.y;

                        Powerups.Slime();
                    }
                    else if (powerupselect == 5)
                    {
                        //DAMAGE POWERUP
                        powerUpLabel.Text = "DAMAGE";
                        DamageVariable++;

                        if (DamageVariable == 1)
                        {
                            Powerups.stonetool();
                        }   
                        else if (DamageVariable == 2)
                        {
                            Powerups.irontool();
                        }
                        else if (DamageVariable == 3)
                        {
                            Powerups.diamondtool();
                        }
                        else if (DamageVariable == 4)
                        {
                            Powerups.netheritetool();
                        }
                    }
                   
                    //destroys powerup orb
                    powerupballs.Remove(pub);
                    break;
                }
            }

            foreach (Ball eb in Powerups.extraballs)
            {
                //Mimic code for fake balls, doing real ball things
                eb.Move();

                eb.WallCollision(this);

                eb.PaddleCollision(paddle);

                foreach (Block b in blocks)
                {
                    if (eb.BlockCollision(b))
                    {
                        b.hp -= Powerups.damage;
                        Block.BlockBreaking(b);

                        if (b.hp <= 0)
                        {
                            blocks.Remove(b);
                            break;
                        }
                    }
                }

                if (eb.BottomCollision(this))
                {
                    Powerups.extraballs.Remove(eb);
                    break;
                }
            }

            //Check if ball is pushed out of bounds, reset ball
            if (ball.PushedOutOfBounds(paddle, this))
            {
                ball.x = paddle.x / 2;
                ball.y = paddle.y - 80;
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
            //Reads an XML FILE for level that player is on
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

            //Changes background depending on what level user is on
            if (level == 1)
            {
                background = Properties.Resources.backgroundLVL1;
            }
            else if (level == 2)
            {
                background = Properties.Resources.backgroundLVL2;
            }
            else if (level == 3)
            {
                background = Properties.Resources.backgroundLVL3;
            }
            else if (level == 4)
            {
                background = Properties.Resources.backgroundLVL4;
            }
            else if (level == 5)
            {
                background = Properties.Resources.backgroundLVL5;
            }
            else if (level == 6)
            {
                dragonBarLabel.Visible = true;
                background = Properties.Resources.backgroundLVL6;
            }
        }

        public void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //Paints background
            e.Graphics.DrawImage(background, 0, 0, this.Width, this.Height);

            //Draws hearts
            switch (lives)
            {
                case 4:
                    e.Graphics.DrawImage(Properties.Resources.goldenHeartIcon, heartBox0);
                    e.Graphics.DrawImage(Properties.Resources.heartIcon, heartBox1);
                    e.Graphics.DrawImage(Properties.Resources.heartIcon, heartBox2);
                    e.Graphics.DrawImage(Properties.Resources.heartIcon, heartBox3);
                    break;
                case 3:
                    e.Graphics.DrawImage(Properties.Resources.heartIcon, heartBox1);
                    e.Graphics.DrawImage(Properties.Resources.heartIcon, heartBox2);
                    e.Graphics.DrawImage(Properties.Resources.heartIcon, heartBox3);
                    break;
                case 2:
                    e.Graphics.DrawImage(Properties.Resources.heartIcon, heartBox3);
                    e.Graphics.DrawImage(Properties.Resources.heartIcon, heartBox2);
                    e.Graphics.DrawImage(Properties.Resources.emptyHeartIcon, heartBox1);
                    break;
                case 1:
                    e.Graphics.DrawImage(Properties.Resources.heartIcon, heartBox3);
                    e.Graphics.DrawImage(Properties.Resources.emptyHeartIcon, heartBox2);
                    e.Graphics.DrawImage(Properties.Resources.emptyHeartIcon, heartBox1);
                    break;
            }

            // Draws paddle
            whiteBrush.Color = paddle.colour;
            e.Graphics.FillRectangle(whiteBrush, paddle.x, paddle.y, paddle.width, paddle.height);

            // Draws blocks
            foreach (Block b in blocks)
            {
                e.Graphics.DrawImage(b.blockImage, b.x, b.y, b.width, b.height);
                e.Graphics.DrawImage(b.durabilityImage, b.x, b.y, b.width, b.height);
            }

            if(level == 6)
            {
                e.Graphics.FillRectangle(redBrush, 60, 40, Block.dragonHp * 4, 10);
                e.Graphics.DrawRectangle(whitePen, 60, 40, 200, 10);
            }

            // Draws power up balls
            foreach (Ball pub in powerupballs)
            {
                e.Graphics.FillRectangle(redBrush, pub.x, pub.y, pub.size, pub.size);
            }

            // Draws extra balls from power up
            foreach (Ball eb in Powerups.extraballs)
            {
                e.Graphics.DrawImage(Properties.Resources.slime, eb.x, eb.y, eb.size + 10, eb.size + 10);
            }

            // Draws ball
            switch (Powerups.damage)
            {
                case 1:
                    e.Graphics.DrawImage(Properties.Resources.woodenPickaxe, ball.x, ball.y, ball.size, ball.size);
                    break;
                case 2:
                    e.Graphics.DrawImage(Properties.Resources.stonePickaxe, ball.x, ball.y, ball.size, ball.size);
                    break;
                case 3:
                    e.Graphics.DrawImage(Properties.Resources.ironPickaxe, ball.x, ball.y, ball.size, ball.size);
                    break;
                case 4:
                    e.Graphics.DrawImage(Properties.Resources.diamondPickaxe, ball.x, ball.y, ball.size , ball.size );
                    break;
                case 5:
                    e.Graphics.DrawImage(Properties.Resources.netheritePickaxe, ball.x, ball.y, ball.size, ball.size);
                    break;
            }
           
        }
    }
}
