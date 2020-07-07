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

    enum Items_List
    {
        Stemp,
        Minigun
    }
    public partial class Form1 : Form
    {
        ItemSelecter itemSelecter;


        Point mousePoint;

        int count = 0;
        public Form1()
        {
            InitializeComponent();


            Create();
        }

        private void Create()
        {
            //전체화면 캡처와 픽처박스 사이즈 조절
            screen_capture();
            pictureBox1.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            pictureBox1.SendToBack();



            DoubleBuffered = true;  // 이중버퍼

            this.KeyPreview = true;

            itemSelecter = new ItemSelecter();
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



        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            mousePoint = e.Location;


            textBox1.Text = e.Location.X + ":" + e.Location.Y;


            MouseCursor.Location = e.Location;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {


        }

        public void pictureControl(PictureBox c, Point p, Size s)
        {
            c.Size = s;
            c.Name = String.Format("img");
            c.Location = p;
            c.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void new_Click_Event(object sender, EventArgs e)
        {
            PictureBox btn = (PictureBox)sender;

            MessageBox.Show(btn.Location.ToString());
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Item item = itemSelecter.items[itemSelecter.select];
            item.operate();

            if (itemSelecter.select == (int)Items_List.Stemp)
            {
                if (e.Button == MouseButtons.Left)
                {
                    count++;
                    label1.Text = count.ToString();

                    PictureBox picture = new PictureBox();


                    pictureControl(picture, e.Location, new Size(100, 100));


                    picture.Image = itemSelecter.getBitmap((int)Items_List.Stemp);

                    picture.Click += new_Click_Event;
                    pictureBox1.Controls.Add(picture);
                }
            }
            else if (itemSelecter.select == (int)Items_List.Minigun)
            {
                (item as MiniGun).isFire = true;

                Thread thread = new Thread(Fire);
                thread.Start();
            }



        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Item item = itemSelecter.items[itemSelecter.select];


            try
            {
                (item as MiniGun).isFire = false;
          
            }
            catch (Exception)
            {

            }

            if (itemSelecter.select == (int)Items_List.Stemp)
            {

            }
            else if (itemSelecter.select == (int)Items_List.Minigun)
            {
            }
        }


        public void Fire()
        {
            MiniGun m = (MiniGun)itemSelecter.getItem(Items_List.Minigun);

            m.soundPlayer.Play();
            while (m.isFire)
            {
                m.operate();
                PictureBox bullet = new PictureBox();

                Point p = new Point(mousePoint.X + 150, mousePoint.Y - 150);
                pictureControl(bullet, p, new Size(10, 10));

                bullet.Image = itemSelecter.getBitmap((int)Items_List.Minigun);


                bullet.SizeMode = PictureBoxSizeMode.StretchImage;
                bullet.BringToFront();

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
                Thread.Sleep(20);
            }
            m.soundPlayer.Stop();
        }



        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D1)
            {
                itemSelecter.select = (int)Items_List.Stemp;
            }
            else if (e.KeyCode == Keys.D2)
            {

                itemSelecter.select = (int)Items_List.Minigun;
            }

        }
    }

    class ItemSelecter
    {

        public int select = 0;
        public List<Item> items;

        public ItemSelecter()
        {
            items = new List<Item>();


            items.Add(new Item("item/stamp.PNG", ""));
            items.Add(new MiniGun());
        }

        public Bitmap getBitmap(int i)
        {
            return items[i].bitmap;
        }

        public Item getItem(Items_List i)
        {
            return items[(int)i];
        }
    }

    class Item
    {
        public SoundPlayer soundPlayer;

        public Bitmap bitmap;


        public long cnt = 0;

        public Item(String image, String sound)
        {
            bitmap = new Bitmap(image);
            soundPlayer = new SoundPlayer();
            soundPlayer.SoundLocation = sound;
        }

        public void operate()
        {
            cnt++;
        }

    }

    class MiniGun : Item
    {


        public bool isFire = false;

        public MiniGun() : base("item/stamp.PNG", "sound/m134.wav")
        {

        }

      
    }
}
