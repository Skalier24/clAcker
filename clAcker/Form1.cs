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

        int TotalStrClick = 0;
        int QUp1Str;
        int QUp2Str;

        int QUp1 = 0; //кол-во купленных апалок 1 гр.
        int QUp2 = 0;  //кол-во купленных апалок 2 грейда
        string l22T;
        string l23T;

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
                    writer.WriteLine("QUp1:0");
                    writer.WriteLine("QUp2:0");
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
            keys.TryGetValue("WYSI", out WYSI); // запись значений из файла в переменные
            keys.TryGetValue("Hacker", out IsBughunter);
            keys.TryGetValue("QUp1", out QUp1); 
            keys.TryGetValue("QUp2", out QUp2);

            label2.Text = SavedClicks.ToString();
            NowActiveBut = button1; //назначение первого скина
            NowActiveBut.Enabled = false; // отключение для баланса *вселенной*

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
                writer.WriteLine("Clicks:{0}", tempClickers);
                writer.WriteLine("Kiwi:1");
                writer.WriteLine("Bronze:{0}", IsBronze);
                writer.WriteLine("Gold:{0}", IsGold);
                writer.WriteLine("Endgame:{0}", IsEG);
                writer.WriteLine("WYSI:{0}", WYSI); //записывает в файл все значения из переменных 
                writer.WriteLine("Hacker:{0}", IsBughunter);
                writer.WriteLine("QUp1:{0}", QUp1);
                writer.WriteLine("QUp2:{0}", QUp2);
                writer.Flush();
            }
        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.Size = new Size(250, 250);
            pictureBox1.Location = new Point(pictureBox1.Location.X + 50, pictureBox1.Location.Y + 50);

            QUp1Str = QUp1 * 5; //итоговая сила усилки 1 гр.
            QUp2Str = QUp2 * 150; //итоговая сила усилки 2 гр.

            TotalStrClick = 1 + QUp1Str + QUp2Str; //вычисление конечной силы крика

            if (tempClickers >= 100) button7.Enabled = true;
            else button7.Enabled = false;

            if (tempClickers >= 2000) button8.Enabled = true;
            else button8.Enabled = false;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox1.Visible = true;
            tempClickers = Int32.Parse(label2.Text);

            if (tempClickers % 10 == 0) { DecWriter(); } //при делении остаток 0, то будет сохранение в файл

           
            tempClickers+=TotalStrClick; //теперь клики начисляются с влиянием силы клика
            label2.Text = tempClickers.ToString();
            pictureBox1.Size = new Size(300, 300);
            pictureBox1.Location = new Point(pictureBox1.Location.X - 50, pictureBox1.Location.Y - 50); //изменение размеров picBox

            if (tempClickers >= SkinOpener["Bronze"] && IsBronze == 0) // открытие скина: бронза (и других)
            {
                IsBronze = 1;
                button4.Enabled = true;
                MessageBox.Show("Бронза разблокирована!");
            }
            if (tempClickers >= SkinOpener["Gold"] && IsGold == 0)
            {
                IsGold = 1;
                button3.Enabled = true;
                MessageBox.Show("Золото разблокировано");
            }
            if (tempClickers >= SkinOpener["Endgame"] && IsEG == 0)
            {
                IsEG = 1;
                button6.Enabled = true;
                MessageBox.Show("Эндгейм достигнут.");
            }
            if (tempClickers >= SkinOpener["WYSI"] && WYSI == 0)
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

            if (pictureBox1.Location.Y >= 300) pictureBox1.Location = new Point(12, 61); //возврат PicBox после фичи
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

            l22T = $"Количество: {QUp1}"; //загрузка кол-ва апов 1 гр.
            label22.Text = l22T;

            l23T = $"Количество: {QUp2}";
            label23.Text = l23T;

            if (tempClickers >= 100) button7.Enabled = true; //загрузка кнопок исходя из кол-ва кликов
            else button7.Enabled = false;

            if (tempClickers >= 2000) button8.Enabled = true;
            else button8.Enabled = false;
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

        private void button7_Click(object sender, EventArgs e) //добавление апов 1 грейда
        {
            QUp1++;
            tempClickers -= 100;
            label2.Text = tempClickers.ToString();
            l22T = $"Количество: {QUp1}"; //обновление кол-ва апов на форме
            label22.Text = l22T;

            if (tempClickers < 100) button7.Enabled = false; //выкл кнопки если не хватает
        }

        private void button8_Click(object sender, EventArgs e) //все то же для 2 гр.
        {
            QUp2++;
            tempClickers -= 2000;
            label2.Text = tempClickers.ToString();
            l23T = $"Количество: {QUp2}";
            label23.Text = l23T;

            if (tempClickers < 2000) button8.Enabled = false;
        }
    }
}
