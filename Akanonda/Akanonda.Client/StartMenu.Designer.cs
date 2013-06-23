namespace Akanonda
{
    partial class StartMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartMenu));
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.buttonLogIn = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.colorBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(24, 129);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(239, 20);
            this.textBoxName.TabIndex = 0;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.BackColor = System.Drawing.Color.Transparent;
            this.labelName.Font = new System.Drawing.Font("Motorwerk", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelName.Location = new System.Drawing.Point(15, 111);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(260, 15);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "Choose your Name and Color";
            // 
            // buttonLogIn
            // 
            this.buttonLogIn.Enabled = false;
            this.buttonLogIn.Font = new System.Drawing.Font("Motorwerk", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLogIn.Location = new System.Drawing.Point(24, 182);
            this.buttonLogIn.Name = "buttonLogIn";
            this.buttonLogIn.Size = new System.Drawing.Size(239, 38);
            this.buttonLogIn.TabIndex = 3;
            this.buttonLogIn.Text = "Connect to GameLobby";
            this.buttonLogIn.UseVisualStyleBackColor = true;
            this.buttonLogIn.Click += new System.EventHandler(this.buttonLogIn_Click);
            // 
            // colorBox
            // 
            this.colorBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.colorBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colorBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.colorBox.FormattingEnabled = true;
            this.colorBox.Items.AddRange(new object[] {
            "Green",
            "Yellow",
            "Orange",
            "Black",
            "Red",
            "Blue",
            "Grey",
            "Turquoise",
            "Pink",
            "Purple",
            "Brown"});
            this.colorBox.Location = new System.Drawing.Point(24, 155);
            this.colorBox.Name = "colorBox";
            this.colorBox.Size = new System.Drawing.Size(239, 21);
            this.colorBox.TabIndex = 4;
            this.colorBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.colorBox_DrawItem);
            this.colorBox.SelectedIndexChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(26, 159);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Choose a Color";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-38, -18);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(383, 217);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // StartMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lime;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(287, 232);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.colorBox);
            this.Controls.Add(this.buttonLogIn);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StartMenu";
            this.Text = "Start Menu";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Button buttonLogIn;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ComboBox colorBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}