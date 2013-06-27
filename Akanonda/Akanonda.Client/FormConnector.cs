using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Akanonda
{
    public class FormConnector
    {
        //private static FormConnector instance = new FormConnector();
        //public static FormConnector Instance
        //{
        //    get
        //    {
        //        return instance;
        //    }
        //}
       
            private Form mMainForm;

            private List<Form> mConnectedForms = new List<Form>();

            private Point mMainLocation;

            public FormConnector(Form mainForm)
            {
                this.mMainForm = mainForm;
                this.mMainLocation = new Point(this.mMainForm.Location.X, this.mMainForm.Location.Y);
                this.mMainForm.LocationChanged += new EventHandler(MainForm_LocationChanged);
            }

            public void ConnectForm(Form form)
            {
                if (!this.mConnectedForms.Contains(form))
                {
                    this.mConnectedForms.Add(form);
                }
            }

            void MainForm_LocationChanged(object sender, EventArgs e)
            {
                Point relativeChange = new Point(this.mMainForm.Location.X - this.mMainLocation.X, this.mMainForm.Location.Y - this.mMainLocation.Y);
                foreach (Form form in this.mConnectedForms)
                {
                    
                    form.Location = new Point(form.Location.X + relativeChange.X, form.Location.Y + relativeChange.Y);
                    if (!IsOnScreen(form))
                    form.Location = new Point(form.Location.X - relativeChange.X, form.Location.Y - relativeChange.Y);
                }
                
                this.mMainLocation = new Point(this.mMainForm.Location.X, this.mMainForm.Location.Y);
            
            }

            public bool IsOnScreen(Form form)
            {
                Screen[] screens = Screen.AllScreens;
                foreach (Screen screen in screens)
                {
                    Rectangle formRectangle = new Rectangle(form.Left, form.Top, form.Width, form.Height);

                    if (screen.WorkingArea.Contains(formRectangle))
                    {
                        return true;
                    }
                }

                return false;
            }

    }
}
