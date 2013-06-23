namespace Akanonda
{
    partial class LobbyForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LobbyForm));
            this.StartGame = new System.Windows.Forms.Button();
            this.MessageBox = new System.Windows.Forms.TextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.ChatBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PlayersInLobbyList = new System.Windows.Forms.ListBox();
            this.PlayersInGameList = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // StartGame
            // 
            this.StartGame.Font = new System.Drawing.Font("Motorwerk", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartGame.Location = new System.Drawing.Point(668, 442);
            this.StartGame.Name = "StartGame";
            this.StartGame.Size = new System.Drawing.Size(112, 40);
            this.StartGame.TabIndex = 4;
            this.StartGame.Text = "Start Playin\'";
            this.StartGame.UseVisualStyleBackColor = true;
            this.StartGame.Click += new System.EventHandler(this.StartGame_Click);
            // 
            // MessageBox
            // 
            this.MessageBox.Location = new System.Drawing.Point(11, 416);
            this.MessageBox.Name = "MessageBox";
            this.MessageBox.Size = new System.Drawing.Size(162, 20);
            this.MessageBox.TabIndex = 1;
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(179, 416);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(75, 20);
            this.SendButton.TabIndex = 3;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // ChatBox
            // 
            this.ChatBox.Location = new System.Drawing.Point(11, 133);
            this.ChatBox.Name = "ChatBox";
            this.ChatBox.ReadOnly = true;
            this.ChatBox.Size = new System.Drawing.Size(243, 277);
            this.ChatBox.TabIndex = 0;
            this.ChatBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Motorwerk", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 117);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Chat";
            // 
            // PlayersInLobbyList
            // 
            this.PlayersInLobbyList.FormattingEnabled = true;
            this.PlayersInLobbyList.Location = new System.Drawing.Point(260, 133);
            this.PlayersInLobbyList.Name = "PlayersInLobbyList";
            this.PlayersInLobbyList.Size = new System.Drawing.Size(257, 303);
            this.PlayersInLobbyList.TabIndex = 6;
            // 
            // PlayersInGameList
            // 
            this.PlayersInGameList.FormattingEnabled = true;
            this.PlayersInGameList.Location = new System.Drawing.Point(523, 133);
            this.PlayersInGameList.Name = "PlayersInGameList";
            this.PlayersInGameList.Size = new System.Drawing.Size(257, 303);
            this.PlayersInGameList.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Motorwerk", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(257, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Players in Lobby:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Motorwerk", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(520, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "Players in Game:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-6, -2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(412, 241);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // LobbyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(792, 491);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PlayersInGameList);
            this.Controls.Add(this.PlayersInLobbyList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ChatBox);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.MessageBox);
            this.Controls.Add(this.StartGame);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LobbyForm";
            this.Text = "Akanonda Lobby";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LobbyForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartGame;
        private System.Windows.Forms.TextBox MessageBox;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.RichTextBox ChatBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox PlayersInLobbyList;
        private System.Windows.Forms.ListBox PlayersInGameList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}