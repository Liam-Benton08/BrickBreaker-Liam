using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrickBreaker
{
    public partial class MenuScreen : UserControl
    {
        public MenuScreen()
        {
            InitializeComponent();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        Rectangle backgroundImageRec = new Rectangle(0, 0, 855 * 4, 855);
        Rectangle backgrounImageStitchRec = new Rectangle(855, 0, 855 * 4, 855);
        Rectangle minecraftLogo = new Rectangle((855 / 2) - 300, 50, 600, 300);

        bool isResetting;
        bool stitchingImageVisible;

        private void playButton_Click(object sender, EventArgs e)
        {
            // Goes to the game screen
            GameScreen gs = new GameScreen();
            Form form = this.FindForm();

            form.Controls.Add(gs);
            form.Controls.Remove(this);

            gs.Location = new Point((form.Width - gs.Width) / 2, (form.Height - gs.Height) / 2);
        }

        private void menuTimer_Tick(object sender, EventArgs e)
        {
            if (!isResetting)
            {
                backgroundImageRec.X -= 5;

                if (backgroundImageRec.X <= -855 * 3)
                {
                    stitchingImageVisible = true;
                    backgrounImageStitchRec.X -= 5;
                }

                if (backgrounImageStitchRec.X <= 0)
                {
                    // Prepare for reset
                    isResetting = true;
                }
            }
            else
            {
                // Reset both rectangles
                backgroundImageRec.X = 0;
                backgrounImageStitchRec.X = 855;
                stitchingImageVisible = false;
                isResetting = false;
            }

            Refresh();
        }


        private void MenuScreen_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Properties.Resources.BackgroundScaled, backgroundImageRec);

            if (stitchingImageVisible)
            {
                e.Graphics.DrawImage(Properties.Resources.BackgroundScaled, backgrounImageStitchRec);
            }

            e.Graphics.DrawImage(Properties.Resources.Minebrick, minecraftLogo);
        }

    }
}