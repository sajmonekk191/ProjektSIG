using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using ProjektSIG.Essentials;

namespace ProjektSIG
{
    public partial class Main : Form
    {
        Point mousedownpoint = Point.Empty;
        Miroring frm = new Miroring();
        public Main()
        {
            InitializeComponent();
        }
        private void spammer_Tick(object sender, EventArgs e)
        {
            if (Values.RemoteActivated && Values.ImgLocation != String.Empty && !Values.ActivatedForm)
            {
                WebClient webClient = new WebClient();
                webClient.UseDefaultCredentials = true;
                webClient.Proxy.Credentials = CredentialCache.DefaultCredentials;
                if (webClient.DownloadString("http://81.162.196.29/server/projektSIG.htm").Contains("Activated"))
                {
                    checkBox1.Checked = true;
                    Values.ActivatedForm = true;
                    this.Visible = false;
                    frm.Show();
                }
            }
        }
        private void Updater_Tick(object sender, EventArgs e)
        {
            if (pictureBox1.Location.X < panel1.Location.X - 320) pictureBox1.Left += 5;
            if (pictureBox1.Location.X > panel1.Location.X + 80) pictureBox1.Left -= 5;
            if (pictureBox1.Location.Y < panel1.Location.Y - 25) pictureBox1.Top += 5;
            if (pictureBox1.Location.Y > panel1.Location.Y + 320) pictureBox1.Top -= 5;

            if(Properties.Settings.Default.IsRemote && !checkBox2.Checked) checkBox2.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse Image File",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "Image File",
                Filter = "All files (*.*)|*.*" + "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                checkBox1.Enabled = true;
                Values.ImgLocation = openFileDialog1.FileName;
                label1.Left -= 60;
                label1.Text = openFileDialog1.FileName;
                pictureBox1.ImageLocation = Values.ImgLocation;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                checkBox1.Enabled = true;
                Values.ImgLocation = textBox1.Text;
                pictureBox1.ImageLocation = Values.ImgLocation;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mousedownpoint = new Point(e.X, e.Y);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousedownpoint.IsEmpty)
                return;
            pictureBox1.Location = new Point(pictureBox1.Location.X + (e.X - mousedownpoint.X), pictureBox1.Location.Y + (e.Y - mousedownpoint.Y));
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mousedownpoint = Point.Empty;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked && !Values.ActivatedForm)
            {
                Values.ActivatedForm = true;
                this.Visible = false;
                frm.Show();
            }
            else
            {
                Values.ActivatedForm = false;
                this.Visible = false;
                frm.Close();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                Values.RemoteActivated = true;
                Properties.Settings.Default.IsRemote = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Values.RemoteActivated = false;
                Properties.Settings.Default.IsRemote = false;
                Properties.Settings.Default.Save();
            }
        }
    }
}
