﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Akanonda.GameLibrary;
using Lidgren.Network;

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

            Game game = Game.Instance;
            game.adjustGameFormSize(this);
            
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
            NetOutgoingMessage sendMsg;
            
            switch (e.KeyCode) 
            {
                case Keys.Right:
                    Program.game.setlocalsteering(GameLibrary.PlayerSteering.Right);
                    
                    sendMsg = Program.netclient.CreateMessage();

                    sendMsg.Write(Program.guid.ToString());
                    sendMsg.Write((Int32)PlayerSteering.Right);
             
                    Program.netclient.SendMessage(sendMsg, NetDeliveryMethod.ReliableSequenced);
                    
                    break;
                case Keys.Left:
                    Program.game.setlocalsteering(GameLibrary.PlayerSteering.Left);
                    
                    sendMsg = Program.netclient.CreateMessage();

                    sendMsg.Write(Program.guid.ToString());
                    sendMsg.Write((Int32)PlayerSteering.Left);
             
                    Program.netclient.SendMessage(sendMsg, NetDeliveryMethod.ReliableSequenced);
                    
                    break;
                case Keys.Up:
                    Program.game.setlocalsteering(GameLibrary.PlayerSteering.Up);
                    
                    sendMsg = Program.netclient.CreateMessage();

                    sendMsg.Write(Program.guid.ToString());
                    sendMsg.Write((Int32)PlayerSteering.Up);
             
                    Program.netclient.SendMessage(sendMsg, NetDeliveryMethod.ReliableSequenced);
                    
                    break;
                case Keys.Down:
                    Program.game.setlocalsteering(GameLibrary.PlayerSteering.Down);
                    
                    sendMsg = Program.netclient.CreateMessage();

                    sendMsg.Write(Program.guid.ToString());
                    sendMsg.Write((Int32)PlayerSteering.Down);
             
                    Program.netclient.SendMessage(sendMsg, NetDeliveryMethod.ReliableSequenced);
                    
                    break;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.netclient.Shutdown(Program.guid.ToString());

            while(Program.netclient.Status != NetPeerStatus.NotRunning)
                System.Threading.Thread.Sleep(10);
        }
    }
}
