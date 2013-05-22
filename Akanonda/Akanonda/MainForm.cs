using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Akanonda
{
    public partial class MainForm : Form
    {

        private Snake mySnake; // struct snake
        private GameData myData;  // struct mit den game informationen
        private Timer SnakeTimer;  //game timer
        private Timer ExtensionTimer;
        public Timer SurvivalTimer;
        public int SurvivalSecond = 0;
        public int SurvivalMinute = 0;
        private resetForm reset;

        public MainForm()
        {
            InitializeComponent();
            Init();
            ResetGame();
        }

        private void Draw(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            //zeichnet rand
            Rectangle r = this.ClientRectangle;
            System.Drawing.Graphics g = e.Graphics;
            Pen blackPen = new Pen(Color.Black, 5);
            g.DrawRectangle(blackPen , r);


            //zeichnet schlange
            int BodyLength = mySnake.length;
            for (int i = 0; i < BodyLength; i++)
            {
                int x = mySnake.Body[i].X;
                int y = mySnake.Body[i].Y;
                int width = mySnake.Body[i].Width;
                int height = mySnake.Body[i].Height;

                g.FillRectangle(new SolidBrush(Color.Green), x, y, width, height);
            }

            //game over pop up
            if (myData.gameOver == true)
            {
                this.reset = new resetForm(SurvivalSecond, SurvivalMinute);
                //MessageBox.Show(SurvivalSecond.ToString());
                DialogResult dialogresult = reset.ShowDialog();
                if (dialogresult == DialogResult.Yes)
                {
                   // MessageBox.Show("test");
                 
                    ResetGame();
                }
                else
                {
                    MainForm.ActiveForm.Dispose();
                    return;
                }
                reset.Dispose();
                //MessageBox.Show("GAME OVER");
            }
        }

        public void SurvivalTimer_Tick(object sender, EventArgs e)
        {
            SurvivalSecond++;

            if (SurvivalSecond == 60)
            {
                SurvivalMinute++;
                SurvivalSecond = 0;
            }
        }
        void MoveBody()
        {
            Rectangle Dir = mySnake.Dir;
            Dir.X = mySnake.Dir.X * mySnake.bodySize;
            Dir.Y = mySnake.Dir.Y * mySnake.bodySize;

            
            mySnake.Body[mySnake.length] = mySnake.Body[mySnake.length - 1];

            
            int BodyLength = mySnake.length;
            for (int i = BodyLength; i > 0; i--)
            {
                mySnake.Body[i] = mySnake.Body[i - 1];

            }
            
            mySnake.Body[0].X = mySnake.Body[0].X + Dir.X;
            mySnake.Body[0].Y = mySnake.Body[0].Y + Dir.Y;

            
            if (mySnake.growLength > 0)
            {
                
                mySnake.length++;
                mySnake.growLength--;
            }

        }

        private void KeyPressed(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            string result = e.KeyData.ToString();
            switch (result)
            {
                case "Left":
                    if(mySnake.Dir.X != 1)
                    {
                    mySnake.Dir.X = -1; mySnake.Dir.Y = 0;
                    }
                    break;
                case "Right":
                    if (mySnake.Dir.X != -1)
                    {
                    mySnake.Dir.X = 1; mySnake.Dir.Y = 0;
                    }
                    break;
                case "Up":
                    if (mySnake.Dir.Y != 1)
                    {
                    mySnake.Dir.X = 0; mySnake.Dir.Y = -1;
                    }
                    break;
                case "Down":
                    if (mySnake.Dir.Y != -1)
                    {
                    mySnake.Dir.X = 0; mySnake.Dir.Y = 1;
                    }
                    break;
                //case "Enter":
                //    if (myData.gameOver == true)
                //    {
                //        myData.gameOver = false;
                //        ResetGame();
                //    }
                //    break;
                default:
                    break;
            }

        }


        void CheckGameOver()
        {
            
            int BodyLength = mySnake.length;
            Rectangle r = this.ClientRectangle;
            for (int i = 1; i < BodyLength; i++)
            {
                //berührt der kopf die eigene schlange?
                if (mySnake.Body[i].IntersectsWith(mySnake.Body[0]))
                {
                    myData.gameOver = true;
                    
                    break;
                }
            }
            //berührt der kpf en rand?
            if (!mySnake.Body[0].IntersectsWith(r))
                 myData.gameOver = true;
                
        }

        private void TimerEvent(object sender, System.EventArgs e)
        {
            if (myData.gameOver == true) 
                return;

            MoveBody();   //bewegt die schlange um ein feld weiter
            Invalidate(); //refresht den screen
            CheckGameOver();

        }

        private void ExtensionEvent(object sender, System.EventArgs e)
        {
            mySnake.growLength += 1;
        }

        private void Init()
        {
            //enable double buffering
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            //set events
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyPressed);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Draw);

            //bewegungstimer
            this.SnakeTimer = new Timer();
            this.SnakeTimer.Interval = 30;
            this.SnakeTimer.Tick += new System.EventHandler(this.TimerEvent);
            SnakeTimer.Enabled = true;

            //verlängerungstimer
            this.ExtensionTimer = new Timer();
            this.ExtensionTimer.Interval = 400;
            this.ExtensionTimer.Tick += new System.EventHandler(this.ExtensionEvent);
            ExtensionTimer.Enabled = true;


            this.SurvivalTimer = new Timer();
            this.SurvivalTimer.Interval = 1000;
            this.SurvivalTimer.Tick += SurvivalTimer_Tick;
            
        }

        void ResetGame()
        {

            myData.gameOver = false;

            //startvariablen
            SurvivalSecond = 0;
            SurvivalMinute = 0;
            mySnake.Dir.X = 1;
            mySnake.Dir.Y = 0;
            mySnake.length = 2;
            mySnake.Body = new Rectangle[10000];
            mySnake.bodySize = 5;
            int xstartpos = 50;
            int ystartpos = 100;
            for (int i = 0; i < mySnake.length + 5; i++)
            {
                mySnake.Body[i].X = 0 + xstartpos; ;
                mySnake.Body[i].Y = i * mySnake.bodySize + ystartpos;
                mySnake.Body[i].Width = mySnake.bodySize;
                mySnake.Body[i].Height = mySnake.bodySize;
            }
            mySnake.growLength = 0;
            SurvivalTimer.Start();
        }
    }

    public struct Snake
    {
        public Rectangle Dir; //richtung der schlange
        public int length;    // länge der schlange
        public Rectangle[] Body; // array mit den einzelnen schlangenteilen
        public int bodySize;  // größe jedes teils
        public int growLength;
    }

    public struct GameData
    {
        public bool gameOver;  
    }
}
