﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BreakDesktopClient
{
    public partial class Form1 : Form
    {
        int count = 0;

        private void screen_capture()
        {

            // 전체 화면 캡처
            Size sz = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(0, 0, 0, 0, sz);
            bitmap.Save("x2.jpg", ImageFormat.Jpeg); // 비트맵에 캡처한 화면 저장
            pictureBox1.ImageLocation = "x2.jpg"; // picture box에 보여줌




        }
        public Form1()
        {            
            InitializeComponent();

            //전체화면 캡처와 픽처박스 사이즈 조절
            screen_capture();
            pictureBox1.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            pictureBox1.SendToBack();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;

            textBox1.Text = e.Location.X + ":" + e.Location.Y;



            label1.Location = new Point(e.Location.X, e.Location.Y);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {

            if(e.Button == MouseButtons.Left)
            {
                count++;
                label1.Text = count.ToString();

                PictureBox picture = new PictureBox();


                controlControl<PictureBox>(picture, e.Location, new Size(100,100));
                picture.ImageLocation = "item/stamp.PNG";
                picture.SizeMode = PictureBoxSizeMode.StretchImage;

                picture.Click += new_Click_Event;
                pictureBox1.Controls.Add(picture);
            }
        }

        private void controlControl<T>(Control c , Point p, Size s) where T : Control
        {
            c.Size = s;
            c.Name = String.Format("{0} img", count);
            c.Location = p;
        }

        private void new_Click_Event(object sender, EventArgs e)
        {
            PictureBox btn = (PictureBox)sender;

            MessageBox.Show(btn.Location.ToString());
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {

        }
    }
}
