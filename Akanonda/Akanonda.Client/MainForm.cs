using System;
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
    /// 
    public partial class MainForm : Form
    {
        
        public static FormConnector mainConnector;
        public static MainForm M_Form;
        public MainForm()
        {
            InitializeComponent();
            
            mainConnector = new FormConnector(this);
            M_Form = this;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            Game game = Game.Instance;
            game.adjustGameFormSize(this);
            //panel1.Size = this.Size;
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
            gamePanel.Invalidate();
        }
        
        void MainFormKeyDown(object sender, KeyEventArgs e)
        {
            NetOutgoingMessage sendMsg;
            
            switch (e.KeyCode) 
            {
                case Keys.Right:
                    if (Program.game.LocalSteering != GameLibrary.PlayerSteering.Left)
                    {
                        Program.game.LocalSteering = GameLibrary.PlayerSteering.Right;

                        sendMsg = Program.netclient.CreateMessage();

                        sendMsg.Write(Program.guid.ToString());
                        sendMsg.Write((Int32)PlayerSteering.Right);

                        Program.netclient.SendMessage(sendMsg, NetDeliveryMethod.ReliableSequenced);
                    }
                    
                    break;
                case Keys.Left:
                    if (Program.game.LocalSteering != GameLibrary.PlayerSteering.Right)
                    {
                        Program.game.LocalSteering = GameLibrary.PlayerSteering.Left;

                        sendMsg = Program.netclient.CreateMessage();

                        sendMsg.Write(Program.guid.ToString());
                        sendMsg.Write((Int32)PlayerSteering.Left);

                        Program.netclient.SendMessage(sendMsg, NetDeliveryMethod.ReliableSequenced);
                    }
                    
                    break;
                case Keys.Up:
                    if (Program.game.LocalSteering != GameLibrary.PlayerSteering.Down)
                    {
                        Program.game.LocalSteering = GameLibrary.PlayerSteering.Up;

                        sendMsg = Program.netclient.CreateMessage();

                        sendMsg.Write(Program.guid.ToString());
                        sendMsg.Write((Int32)PlayerSteering.Up);

                        Program.netclient.SendMessage(sendMsg, NetDeliveryMethod.ReliableSequenced);
                    }

                    break;
                case Keys.Down:
                    if (Program.game.LocalSteering != GameLibrary.PlayerSteering.Up)
                    {
                        Program.game.LocalSteering = GameLibrary.PlayerSteering.Down;

                        sendMsg = Program.netclient.CreateMessage();

                        sendMsg.Write(Program.guid.ToString());
                        sendMsg.Write((Int32)PlayerSteering.Down);

                        Program.netclient.SendMessage(sendMsg, NetDeliveryMethod.ReliableSequenced);

                    }
                    break;
            }
        }


        public void showOverlay(int minutes, int seconds)
        {

            //overlay.Size = this.Size;
            //overlay.Visible = true;
            //overlay.BringToFront();
            if (minutes == 0)
            {
                SurvivalTimeBox.Text = seconds > 1 ? "\nGAME OVER!\nYou survived " + seconds + " seconds!\nCongratulations " + LobbyForm.L_form.name + "!!" : "\nGAME OVER!\nYou survived one second!\nCongratulations " + LobbyForm.L_form.name + "!!";
            }
            else
            {
                SurvivalTimeBox.Text = minutes > 1 ? "\nGAME OVER!\nYou survived " + minutes + " minutes and " + seconds + " seconds!\nCongratulations " + LobbyForm.L_form.name + "!!" : "\nGAME OVER!\nYou survived " + minutes + " minute and " + seconds + " seconds !\nCongratulations " + LobbyForm.L_form.name + "!!";
            }


            gamePanel.BackColor = Color.Gray;
            SurvivalTimeBox.SelectAll();
            SurvivalTimeBox.SelectionAlignment = HorizontalAlignment.Center;
            SurvivalTimeBox.Visible = true;
            //overlay.SendToBack();
            //overlay.Visible = false;

        }

        public void closeOverlay()
        {

            //overlay.Size = this.Size;
            gamePanel.BackColor = Color.White;
            SurvivalTimeBox.Visible = false;

            //overlay.SendToBack();
            //overlay.Visible = false;

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            Program.netclient.Shutdown(Program.guid.ToString());
            Program.s_client.Shutdown(Program.guid.ToString());
            this.Dispose();

            while (Program.netclient.Status != NetPeerStatus.NotRunning || Program.s_client.Status != NetPeerStatus.NotRunning)
                System.Threading.Thread.Sleep(10);

            Environment.Exit(0);
        }
    }
    class ShadowPanel : Panel
    {
        public ShadowPanel()
        {
        }
        protected enum ViewState
        {
            Visible, Hidden
        }
        protected int m_CurrentState = 0;

        override protected CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(59, Color.Black)), this.ClientRectangle);
            Application.DoEvents();
        }
    } 



    
}
