namespace BrickBreaker
{
    partial class GameScreen
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.powerUpLabel = new System.Windows.Forms.Label();
            this.dragonBarLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // gameTimer
            // 
            this.gameTimer.Enabled = true;
            this.gameTimer.Interval = 1;
            this.gameTimer.Tick += new System.EventHandler(this.gameTimer_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // powerUpLabel
            // 
            this.powerUpLabel.BackColor = System.Drawing.Color.Transparent;
            this.powerUpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.powerUpLabel.ForeColor = System.Drawing.Color.White;
            this.powerUpLabel.Location = new System.Drawing.Point(3, 774);
            this.powerUpLabel.Name = "powerUpLabel";
            this.powerUpLabel.Size = new System.Drawing.Size(383, 34);
            this.powerUpLabel.TabIndex = 3;
            // 
            // dragonBarLabel
            // 
            this.dragonBarLabel.BackColor = System.Drawing.Color.Transparent;
            this.dragonBarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dragonBarLabel.ForeColor = System.Drawing.Color.White;
            this.dragonBarLabel.Location = new System.Drawing.Point(14, 0);
            this.dragonBarLabel.Name = "dragonBarLabel";
            this.dragonBarLabel.Size = new System.Drawing.Size(300, 34);
            this.dragonBarLabel.TabIndex = 4;
            this.dragonBarLabel.Text = "Dragon Health";
            this.dragonBarLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dragonBarLabel.Visible = false;
            // 
            // GameScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.dragonBarLabel);
            this.Controls.Add(this.powerUpLabel);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "GameScreen";
            this.Size = new System.Drawing.Size(855, 855);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GameScreen_Paint);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GameScreen_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.GameScreen_PreviewKeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label powerUpLabel;
        private System.Windows.Forms.Label dragonBarLabel;
    }
}
