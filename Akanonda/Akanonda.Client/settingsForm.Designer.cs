namespace Akanonda
{
    partial class settingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(settingsForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.use2ButtonsRadio = new System.Windows.Forms.RadioButton();
            this.use4ButtonsRadio = new System.Windows.Forms.RadioButton();
            this.closeSettingsButton = new System.Windows.Forms.Button();
            this.GamePort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ChatPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ServerAdress = new System.Windows.Forms.TextBox();
            this.saveSettingsButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.use2ButtonsRadio);
            this.groupBox1.Controls.Add(this.use4ButtonsRadio);
            this.groupBox1.Location = new System.Drawing.Point(23, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 102);
            this.groupBox1.TabIndex = 96;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Control Mode";
            // 
            // use2ButtonsRadio
            // 
            this.use2ButtonsRadio.AutoSize = true;
            this.use2ButtonsRadio.Location = new System.Drawing.Point(29, 66);
            this.use2ButtonsRadio.Name = "use2ButtonsRadio";
            this.use2ButtonsRadio.Size = new System.Drawing.Size(96, 17);
            this.use2ButtonsRadio.TabIndex = 1;
            this.use2ButtonsRadio.TabStop = true;
            this.use2ButtonsRadio.Text = "Dynamic Mode";
            this.use2ButtonsRadio.UseVisualStyleBackColor = true;
            this.use2ButtonsRadio.CheckedChanged += new System.EventHandler(this.use2ButtonsRadio_CheckedChanged);
            // 
            // use4ButtonsRadio
            // 
            this.use4ButtonsRadio.AutoSize = true;
            this.use4ButtonsRadio.Location = new System.Drawing.Point(29, 31);
            this.use4ButtonsRadio.Name = "use4ButtonsRadio";
            this.use4ButtonsRadio.Size = new System.Drawing.Size(82, 17);
            this.use4ButtonsRadio.TabIndex = 0;
            this.use4ButtonsRadio.TabStop = true;
            this.use4ButtonsRadio.Text = "Static Mode";
            this.use4ButtonsRadio.UseVisualStyleBackColor = true;
            this.use4ButtonsRadio.CheckedChanged += new System.EventHandler(this.use4ButtonsRadio_CheckedChanged);
            // 
            // closeSettingsButton
            // 
            this.closeSettingsButton.Location = new System.Drawing.Point(189, 421);
            this.closeSettingsButton.Name = "closeSettingsButton";
            this.closeSettingsButton.Size = new System.Drawing.Size(93, 40);
            this.closeSettingsButton.TabIndex = 6;
            this.closeSettingsButton.Text = "Close";
            this.closeSettingsButton.UseVisualStyleBackColor = true;
            this.closeSettingsButton.Click += new System.EventHandler(this.closeSettingsButton_Click);
            // 
            // GamePort
            // 
            this.GamePort.Location = new System.Drawing.Point(29, 96);
            this.GamePort.Name = "GamePort";
            this.GamePort.Size = new System.Drawing.Size(197, 20);
            this.GamePort.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 98;
            this.label1.Text = "Game Port";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 99;
            this.label2.Text = "Chat Port";
            // 
            // ChatPort
            // 
            this.ChatPort.Location = new System.Drawing.Point(29, 135);
            this.ChatPort.Name = "ChatPort";
            this.ChatPort.Size = new System.Drawing.Size(197, 20);
            this.ChatPort.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 97;
            this.label3.Text = "Server Adress";
            // 
            // ServerAdress
            // 
            this.ServerAdress.Location = new System.Drawing.Point(29, 59);
            this.ServerAdress.Name = "ServerAdress";
            this.ServerAdress.Size = new System.Drawing.Size(197, 20);
            this.ServerAdress.TabIndex = 2;
            // 
            // saveSettingsButton
            // 
            this.saveSettingsButton.Location = new System.Drawing.Point(29, 179);
            this.saveSettingsButton.Name = "saveSettingsButton";
            this.saveSettingsButton.Size = new System.Drawing.Size(93, 40);
            this.saveSettingsButton.TabIndex = 5;
            this.saveSettingsButton.Text = "Save";
            this.saveSettingsButton.UseVisualStyleBackColor = true;
            this.saveSettingsButton.Click += new System.EventHandler(this.saveSettingsButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ChatPort);
            this.groupBox2.Controls.Add(this.saveSettingsButton);
            this.groupBox2.Controls.Add(this.GamePort);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.ServerAdress);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(23, 151);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(259, 250);
            this.groupBox2.TabIndex = 95;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Server Settings";
            // 
            // settingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 473);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.closeSettingsButton);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "settingsForm";
            this.Text = "Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton use2ButtonsRadio;
        private System.Windows.Forms.RadioButton use4ButtonsRadio;
        private System.Windows.Forms.Button closeSettingsButton;
        private System.Windows.Forms.TextBox GamePort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ChatPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ServerAdress;
        private System.Windows.Forms.Button saveSettingsButton;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}