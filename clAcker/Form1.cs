using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace clAcker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int tempClickers; //кол-во кликов в проге
        int SavedClicks; //кол-во кликов из файла 
        int IsBronze;
        int IsGold;
        int IsEG;
        int WYSI;
        int IsBughunter;


        Button NowActiveBut;

        Dictionary<string, int> keys = new Dictionary<string, int>(); //словарь сохраненок с файла

        Dictionary<string, int> SkinOpener = new Dictionary<string, int>(); //Открывашка скинов :WW

        private void Exists() //проверяет есть ли файл, если не - создает
        {
            FileInfo fInfo = new FileInfo("Stats.txt");

            if (!fInfo.Exists)
            {
                using (FileStream files = new FileStream("Stats.txt", FileMode.Create))
                {
                    files.Close();
                }
            }
            else if (fInfo.Length == 0)
            {
                using (StreamWriter writer = new StreamWriter("Stats.txt", true))
                {
                    writer.WriteLine("Clicks:0");
                    writer.WriteLine("Kiwi:1");
                    writer.WriteLine("Bronze:0");
                    writer.WriteLine("Gold:0");
                    writer.WriteLine("Endgame:0");
                    writer.WriteLine("WYSI:0");
                    writer.WriteLine("Hacker:0");
                    writer.Flush();
                }
            }
        }

        private void DecRead()
        {
            string key;
            int value;
            using (StreamReader reader = new StreamReader("Stats.txt"))
            {
                string CurLine;
                while ((CurLine = reader.ReadLine()) != null)
                {
                    string[] parts = CurLine.Split(':');
                    if (parts.Length == 2)
                    {
                        key = parts[0].Trim();
                        value = Convert.ToInt32(parts[1].Trim());

                        if (!keys.ContainsKey(key))
                        {
                            keys.Add(key, value);
                        }
                        else
                        {
                            keys[key] = value;
                        }
                    }
                }
            }

            keys.TryGetValue("Clicks", out SavedClicks);
            keys.TryGetValue("Bronze", out IsBronze);
            keys.TryGetValue("Gold", out IsGold);
            keys.TryGetValue("Endgame", out IsEG);
            keys.TryGetValue("WYSI", out WYSI);
            keys.TryGetValue("Hacker", out IsBughunter);

            label2.Text = SavedClicks.ToString();
            NowActiveBut = button1;
            NowActiveBut.Enabled = false;

            if (IsBronze == 1) button4.Enabled = true; else button4.Enabled = false;
            if (IsGold == 1) button3.Enabled = true; else button3.Enabled = false;
            if (IsEG == 1) button6.Enabled = true; else button6.Enabled = false;    //ладно.
            if (WYSI == 1) button2.Enabled = true; else button2.Enabled = false;
            if (IsBughunter == 1) button5.Enabled = true; else button5.Enabled = false;
        }


        private void ChangeSkin(Button Changedbutton, Image image) //меняет скин 
        {
            if (Changedbutton.Enabled == true) //излишняя проверка пусть останется хихихаха
            {
                Changedbutton.Enabled = false; //нажатая кнопка отключается
                pictureBox1.Image = image; //введеное изобр. ставится в PicBox
                NowActiveBut.Enabled = true; //обратно включить прошлый скин
                NowActiveBut = Changedbutton; // нажатая кнопка перетекает в "сейчас активная"
                //MessageBox.Show(NowActiveBut.Name); //а точно работает??
            }
        }


        private void DecWriter()
        {
            using (StreamWriter writer = new StreamWriter("Stats.txt", false))
            {
                writer.WriteLine($"Clicks:{tempClickers}");
                writer.WriteLine($"Kiwi:1");
                writer.WriteLine($"Bronze:{IsBronze}");
                writer.WriteLine($"Gold:{IsGold}");
                writer.WriteLine($"Endgame:{IsEG}");
                writer.WriteLine($"WYSI:{WYSI}");
                writer.WriteLine($"Hacker:{IsBughunter}");
                writer.Flush();
            }
        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.Size = new Size(250, 250);
            pictureBox1.Location = new Point(pictureBox1.Location.X + 50, pictureBox1.Location.Y + 50);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox1.Visible = true;
            tempClickers = Int32.Parse(label2.Text);

            if (tempClickers % 10 == 0) { DecWriter(); } //при делении остаток 0, то будет сохранение в файл

            tempClickers++;
            label2.Text = tempClickers.ToString();
            pictureBox1.Size = new Size(300, 300);
            pictureBox1.Location = new Point(pictureBox1.Location.X - 50, pictureBox1.Location.Y - 50);

            if (tempClickers == SkinOpener["Bronze"] && IsBronze == 0)
            {
                IsBronze = 1;
                button4.Enabled = true;
                MessageBox.Show("Бронза разблокирована!");
            }
            if (tempClickers == SkinOpener["Gold"] && IsGold == 0)
            {
                IsGold = 1;
                button3.Enabled = true;
                MessageBox.Show("Золото разблокировано");
            }
            if (tempClickers == SkinOpener["Endgame"] && IsEG == 0)
            {
                IsEG = 1;
                button6.Enabled = true;
                MessageBox.Show("Эндгейм достигнут.");
            }
            if (tempClickers == SkinOpener["WYSI"] && WYSI == 0)
            {
                WYSI = 1;
                button2.Enabled = true;
                MessageBox.Show("ААА WYSI");
            }
            if (pictureBox1.Location.Y >= SkinOpener["Hacker"] && IsBughunter == 0)
            {
                IsBughunter = 1;
                button5.Enabled = true;
                MessageBox.Show("Поздравляю, вы Хуцкер!");
            }

            if (pictureBox1.Location.Y >= 300) pictureBox1.Location = new Point(12, 61);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Exists(); //проверяет есть ли файл, если не - создает
            DecRead();//читает файл
            SkinOpener.Add("Bronze", 100);
            SkinOpener.Add("Gold", 1200);
            SkinOpener.Add("Endgame", 10000); // для кликов
            SkinOpener.Add("WYSI", 727);
            SkinOpener.Add("Hacker", 300); // для Y пиктюрбокса
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ChangeSkin(button6, Properties.Resources.EG1); //МАРИЯ В ГЛАЗУРИ
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChangeSkin(button1, Properties.Resources.Kiwi_Clickjpg); //обратно киви
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ChangeSkin(button4, Properties.Resources.Bronze); //Бронза
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChangeSkin(button2, Properties.Resources.WYSI); //ААААА 727
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ChangeSkin(button5, Properties.Resources.Hacker); //че.
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChangeSkin(button3, Properties.Resources.Gold); //золото
        }
    }
}
