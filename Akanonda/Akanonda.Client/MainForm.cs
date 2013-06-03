using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Akanonda
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {        
        public MainForm()
        {
            InitializeComponent();
         

                     
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            
            DrawTimer.Start();
        }
        
        void MainFormPaint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            
            if(Program.game != null)
                Program.game.gamepaint(g);

        }
        
                
        void DrawTimerTick(object sender, System.EventArgs e)
        {
            this.Invalidate();
        }
        
        void MainFormKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode) 
            {
                case Keys.Right:
                    Program.game.setlocalsteering(GameLibrary.PlayerSteering.Right);
                    break;
                case Keys.Left:
                    Program.game.setlocalsteering(GameLibrary.PlayerSteering.Left);
                    break;
                case Keys.Up:
                    Program.game.setlocalsteering(GameLibrary.PlayerSteering.Up);
                    break;
                case Keys.Down:
                    Program.game.setlocalsteering(GameLibrary.PlayerSteering.Down);
                    break;
            }
        }
    }
}
