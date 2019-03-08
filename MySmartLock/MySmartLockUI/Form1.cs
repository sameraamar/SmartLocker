using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MySmartLockUI
{
    public partial class Form1 : Form
    {
        private string oldText;
        
        public Form1()
        {
            InitializeComponent();

            KeyPreview = true;  // indicates that key events for controls on the form
            // should be registered with the form

            //KeyDown += new KeyEventHandler(Form1_KeyDown);
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Modifiers & Keys.Control) == Keys.Control && (e.Modifiers & Keys.Alt) == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        MessageBox.Show("Leaving!");
                        Close();
                        break;
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000 * 60 * 60;
            ActiveControl = txtCode2;

            var screen = Screen.GetWorkingArea(this);

            int domin = 3;
            Height = screen.Height * domin / 5 ;
            Width = screen.Width * domin / 5;

            CenterToScreen();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void LockScreen()
        {
            if (!ProcessExtensions.LockWorkStation())
            {
                var winEx = new Win32Exception(Marshal.GetLastWin32Error());
                MessageBox.Show(winEx.ToString());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            timer1.Enabled = false;
        }

        private string ReadPassCode()
        {
            string content = "";
            try
            {
                FileStream fs = new FileStream(@"c:\log\PassCode.txt", FileMode.Open, FileAccess.Read);
                StreamReader sw = new StreamReader(fs);

                content = sw.ReadToEnd();

                sw.Close();
            }
            catch (Exception e)
            {
            }

            return content.Trim();
        }
        
        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            if (!long.TryParse(txtCode2.Text, out var temp))
            {
                txtCode2.Text = oldText;
                txtCode2.SelectionStart = txtCode2.Text.Length;
                return;
            }

            oldText = txtCode2.Text;
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            CenterToScreen();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            txtCode2.Focus();
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            
        }

        private void btnCheckCode_Click_1(object sender, EventArgs e)
        {
            if (!long.TryParse(txtCode1.Text, out long code1))
            {
                txtCode1.Text = "";
                txtCode1.Focus();
            }

            if (!long.TryParse(txtCode2.Text, out long code2))
            {
                txtCode2.Text = "";
                txtCode2.Focus();
            }

            if (OneTimePasswordUtil.IsValidV2(txtName.Text, code1, code2))
            {
                timer1.Enabled = true;
                WindowState = FormWindowState.Minimized;
            }

            if (OneTimePasswordUtil.IsMasterCodeV2(txtName.Text, code1, code2))
            {
                Close();
            }
        }

        private void txtCode_TextChanged_1(object sender, EventArgs e)
        {
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }

}
