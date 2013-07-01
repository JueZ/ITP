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
            this.SurvivalTimeBox.Enabled = false;
            this.SurvivalTimeBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SurvivalTimeBox.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.SurvivalTimeBox.Location = new System.Drawing.Point(0, 0);
            this.SurvivalTimeBox.Name = "SurvivalTimeBox";
            this.SurvivalTimeBox.ReadOnly = true;
            this.SurvivalTimeBox.Size = new System.Drawing.Size(467, 87);
            this.SurvivalTimeBox.TabIndex = 4;
            this.SurvivalTimeBox.Text = "";
            this.SurvivalTimeBox.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(467, 284);
            this.Controls.Add(this.SurvivalTimeBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Akanonda Game";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainFormPaint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainFormKeyDown);
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Timer DrawTimer;
        private System.Windows.Forms.RichTextBox SurvivalTimeBox;
    }
}
