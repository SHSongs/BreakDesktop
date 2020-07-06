using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BreakDesktopClient
{
    public partial class Form1 : Form
    {
        MiniGun miniGun;
        Point mousePoint;

        int count = 0;
        public Form1()
        {
            InitializeComponent();

            //전체화면 캡처와 픽처박스 사이즈 조절
            screen_capture();
            pictureBox1.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            pictureBox1.SendToBack();


            miniGun = new MiniGun(pictureBox1, ref mousePoint);

            DoubleBuffered = true;  // 이중버퍼

        }



        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }



        private void screen_capture()
        {

            // 전체 화면 캡처
            Size sz = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(0, 0, 0, 0, sz);
            bitmap.Save("x2.jpg", ImageFormat.Jpeg); // 비트맵에 캡처한 화면 저장
            pictureBox1.ImageLocation = "x2.jpg"; // picture box에 보여줌


            Cursor.Hide();
            MouseCursor.ImageLocation = "curcor/minigun.png";
            MouseCursor.SizeMode = PictureBoxSizeMode.StretchImage;


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
            mousePoint = e.Location;


            textBox1.Text = e.Location.X + ":" + e.Location.Y;



            MouseCursor.Location = e.Location;


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

        public void controlControl<T>(Control c , Point p, Size s) where T : Control
        {
            c.Size = s;
            c.Name = String.Format("img");
            c.Location = p;
        }

        private void new_Click_Event(object sender, EventArgs e)
        {
            PictureBox btn = (PictureBox)sender;

            MessageBox.Show(btn.Location.ToString());
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            miniGun.isFire = true;
            Thread thread = new Thread(Fire);
            thread.Start();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            miniGun.isFire = false;
        }


        public void Fire()
        {

            miniGun.soundPlayer.Play();
            while (miniGun.isFire)
            {
                PictureBox bullet = new PictureBox();

                Point p = new Point(mousePoint.X + 150, mousePoint.Y - 150);
                controlControl<PictureBox>(bullet, p, new Size(10, 10));

                bullet.Image = miniGun.bulletBitmap;
                bullet.SizeMode = PictureBoxSizeMode.StretchImage;


                if (pictureBox1.InvokeRequired)
                {
                    pictureBox1.Invoke(new MethodInvoker(delegate ()
                    {
                        pictureBox1.Controls.Add(bullet);
                        
                    }));
                }
                else
                {
                    pictureBox1.Controls.Add(bullet);
                }
                miniGun.cnt++;
                Thread.Sleep(20);
            }
            miniGun.soundPlayer.Stop();
        }


    }


    class MiniGun
    {

        public SoundPlayer soundPlayer;

        PictureBox pictureBox;
        Point mousePoint;

        public Bitmap bulletBitmap;

        public bool isFire = false;

        public long cnt = 0;

        public MiniGun(PictureBox pictureBox, ref Point mousePoint)
        {
            this.pictureBox = pictureBox;
            this.mousePoint = mousePoint;

            bulletBitmap = new Bitmap("item/stamp.PNG");
            soundPlayer = new SoundPlayer();
            soundPlayer.SoundLocation = "sound/m134.wav";
        }
        
    }
}
