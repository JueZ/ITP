/*
 * Erstellt mit SharpDevelop.
 * Benutzer: U00V1E5
 * Datum: 24.05.2013
 * Zeit: 14:38
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
namespace Akanonda
{
    partial class MainForm
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.DrawTimer = new System.Windows.Forms.Timer(this.components);
            this.SurvivalTimeBox = new System.Windows.Forms.RichTextBox();
            this.replay = new System.Windows.Forms.Button();
            this.overlayGroupBox = new System.Windows.Forms.Panel();
            this.overlayGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // DrawTimer
            // 
            this.DrawTimer.Interval = 5;
            this.DrawTimer.Tick += new System.EventHandler(this.DrawTimerTick);
            // 
            // SurvivalTimeBox
            // 
            this.SurvivalTimeBox.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.SurvivalTimeBox.BackColor = System.Drawing.SystemColors.Control;
            this.SurvivalTimeBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SurvivalTimeBox.Cursor = System.Windows.Forms.Cursors.No;
            this.SurvivalTimeBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.SurvivalTimeBox.Enabled = false;
            this.SurvivalTimeBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SurvivalTimeBox.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.SurvivalTimeBox.Location = new System.Drawing.Point(0, 0);
            this.SurvivalTimeBox.Name = "SurvivalTimeBox";
            this.SurvivalTimeBox.ReadOnly = true;
            this.SurvivalTimeBox.Size = new System.Drawing.Size(290, 116);
            this.SurvivalTimeBox.TabIndex = 4;
            this.SurvivalTimeBox.Text = "";
            // 
            // replay
            // 
            this.replay.Location = new System.Drawing.Point(108, 122);
            this.replay.Name = "replay";
            this.replay.Size = new System.Drawing.Size(75, 23);
            this.replay.TabIndex = 5;
            this.replay.Text = "Play again";
            this.replay.UseVisualStyleBackColor = true;
            this.replay.Click += new System.EventHandler(this.replay_Click);
            // 
            // overlayGroupBox
            // 
            this.overlayGroupBox.BackColor = System.Drawing.SystemColors.Control;
            this.overlayGroupBox.Controls.Add(this.SurvivalTimeBox);
            this.overlayGroupBox.Controls.Add(this.replay);
            this.overlayGroupBox.Location = new System.Drawing.Point(3, 25);
            this.overlayGroupBox.Name = "overlayGroupBox";
            this.overlayGroupBox.Size = new System.Drawing.Size(290, 150);
            this.overlayGroupBox.TabIndex = 6;
            this.overlayGroupBox.Visible = false;
            // 
            // MainForm
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(467, 284);
            this.Controls.Add(this.overlayGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Akanonda Game";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainFormPaint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainFormKeyDown);
            this.overlayGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Timer DrawTimer;
        private System.Windows.Forms.RichTextBox SurvivalTimeBox;
        private System.Windows.Forms.Button replay;
        private System.Windows.Forms.Panel overlayGroupBox;
    }
}
