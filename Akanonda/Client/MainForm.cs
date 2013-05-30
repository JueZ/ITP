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
        int offset = 10;
        
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
            
            Program.game.gamepaint(g);
            

        }
        
                
        void DrawTimerTick(object sender, System.EventArgs e)
        {
            this.offset++;
            
            Program.game.gametick();
            
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
