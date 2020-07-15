using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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

    class Item_id
    {

    }

    class XY
    {
        public int x
        {
            get; set;
        }
        public int y
        {
            get; set;
        }
        public XY(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

    }
    enum Items_List
    {
        Stemp,
        Minigun,
        Ladder
    }
    public partial class Form1 : Form
    {

        private SqlConnection sqlconn = null;
        private string constr = "SERVER= 127.0.0.1,6975; DATABASE=game;" + "UID=sa2; PASSWORD=5142";




        ItemSelecter itemSelecter;

        PictureBox ladder;

        Point mousePoint;

        int count = 0;
        public Form1()
        {
            InitializeComponent();


            Create();
        }

        private void Create()
        {

            try
            {
                sqlconn = new SqlConnection(constr);
                sqlconn.Open();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


            //전체화면 캡처와 픽처박스 사이즈 조절
            screen_capture();
            pictureBox1.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            pictureBox1.SendToBack();



            DoubleBuffered = true;  // 이중버퍼

            this.KeyPreview = true;

            itemSelecter = new ItemSelecter();

            ladder = new PictureBox();
            ladder.ImageLocation = "curcor/ladder.png";
            ladder.Size = new Size(100, 1000);
            ladder.SizeMode = PictureBoxSizeMode.StretchImage;

            login login = new login();
            login.ShowDialog();

           
            
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


            
            MouseCursor.ImageLocation = "curcor/minigun.png";
            MouseCursor.SizeMode = PictureBoxSizeMode.StretchImage;


        }



        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            mousePoint = e.Location;


            textBox1.Text = e.Location.X + ":" + e.Location.Y;


            MouseCursor.Location = e.Location;

            if (itemSelecter.select == (int)Items_List.Ladder)
            {
                ladder.Location = new Point(e.Location.X, ladder.Location.Y);
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {


        }

        public static void pictureControl(PictureBox c, Point p, Size s)
        {
            c.Size = s;
            c.Name = String.Format("img");
            c.Location = p;
            c.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void new_Click_Event(object sender, EventArgs e)
        {

            MessageBox.Show("불가능합니다 \n Developed by SHsongs, seojin");
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Item item = itemSelecter.items[itemSelecter.select];
            item.operate(mousePoint);

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
                if (item is MiniGun)
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
                PictureBox bullet = m.operate(mousePoint);


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
                MouseCursor.ImageLocation = "curcor/minigun.png";
                itemSelecter.select = (int)Items_List.Stemp;
            }
            else if (e.KeyCode == Keys.D2)
            {

                itemSelecter.select = (int)Items_List.Minigun;
            }
            else if (e.KeyCode == Keys.D3)
            {

                MouseCursor.ImageLocation = "curcor/human.jpg";

                itemSelecter.select = (int)Items_List.Ladder;
                pictureBox1.Controls.Add(ladder);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                MessageBox.Show("저장이 끝나면 자동으로 종료됩니다 \n Developed by SHsongs, seojin");

                SaveData();
            }

        }

        private void SaveData()
        {


            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                foreach (Item item in itemSelecter.items)
                {
                    long cnt = item.cnt;

                    foreach (XY xy in item.XYuseitem)
                    {
                        

                        SqlCommand command = new SqlCommand();

                        command.Connection = conn;
                        command.CommandText = String.Format("INSERT INTO Logs(USER_ID,ITEM_ID,LOCATION_X,LOCATION_Y) VALUES('{0}','{1}','{2}','{3}');",0 ,item.id, xy.x, xy.y);
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch(Exception e)
                        
                        {
                            Console.WriteLine(e);
                        }


                    }
                }

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        class ItemSelecter
        {

            public int select = 0;
            public List<Item> items;

            public ItemSelecter()
            {
                items = new List<Item>();


                items.Add(new Item("item/stamp.PNG", "",0));
                items.Add(new MiniGun()); 
                items.Add(new Item("item/stamp.PNG", "",3));
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


            public List<XY> XYuseitem;
            public int id;

            public long cnt = 0;
            public Item(String image, String sound,int id)
            {
                bitmap = new Bitmap(image);
                soundPlayer = new SoundPlayer();
                soundPlayer.SoundLocation = sound;
                this.id = id;
                XYuseitem = new List<XY>();
            }

            virtual public PictureBox operate(Point mousePoint)
            {
                cnt++;

                XYuseitem.Add(new XY(mousePoint.X, mousePoint.Y));

                return null;
            }

        }

        class MiniGun : Item
        {

            public bool isFire = false;

            public MiniGun() : base("item/bullet.PNG", "sound/m134.wav",1)
            {
            }

            override public PictureBox operate(Point mousePoint)
            {
                base.operate(new Point(mousePoint.X + 150, mousePoint.Y - 150));

                PictureBox bullet = new PictureBox();

                Point p = new Point(mousePoint.X + 150, mousePoint.Y - 150);
                Form1.pictureControl(bullet, p, new Size(10, 10));

                bullet.Image = bitmap;


                bullet.SizeMode = PictureBoxSizeMode.StretchImage;
                bullet.BringToFront();

                return bullet;
            }


        }
    }
}
