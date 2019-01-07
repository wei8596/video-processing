using System;
using System.Drawing;
using System.Windows.Forms;

namespace videoProcess
{
    public partial class FormMain : Form
    {
        MyVideo myVid;                  // MyVideo物件
        int vidLength;                  // 影片frame數量
        Timer Timer1;                   // 計時器

        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Timer1 = new Timer();
            Timer1.Interval = 100;                          // 時間間隔
            Timer1.Tick += new EventHandler(Timer1_Tick);   // 每過一個Interval就呼叫一次
            // 剛開啟僅可使用載入影片按鈕
            menuLoad1.Enabled = true;
            menuLoad2.Enabled = true;
            menuCompression.Enabled = false;
            btnPrev.Enabled = false;
            btnPlay.Enabled = false;
            btnPause.Enabled = false;
            btnNext.Enabled = false;
            trackBar1.Value = 0;
            textBox1.Text = "0";
        }

        // 載入影片1
        private void menuLoad1_Click(object sender, EventArgs e)
        {
            myVid = new MyVideo(1);
            pictureBox1.Image = myVid.imgs[0];
            pictureBox2.Image = myVid.imgs[1];
            // 載入影片後可使用媒體按鈕
            btnPrev.Enabled = true;
            btnPlay.Enabled = true;
            btnPause.Enabled = true;
            btnNext.Enabled = true;
            menuCompression.Enabled = true;
            trackBar1.Value = 0;
            vidLength = myVid.GetLength();
            trackBar1.Maximum = vidLength - 1;
        }

        // 載入影片2
        private void menuLoad2_Click(object sender, EventArgs e)
        {
            myVid = new MyVideo(2);
            pictureBox1.Image = myVid.imgs[0];
            pictureBox2.Image = myVid.imgs[1];
            // 載入影片後可使用媒體按鈕
            btnPrev.Enabled = true;
            btnPlay.Enabled = true;
            btnPause.Enabled = true;
            btnNext.Enabled = true;
            menuCompression.Enabled = true;
            trackBar1.Value = 0;
            vidLength = myVid.GetLength();
            trackBar1.Maximum = vidLength - 1;
        }

        // 結束程式
        private void menuExit_Click(object sender, EventArgs e)
        {
            if (Application.MessageLoop)
            {
                // WinForms app
                Application.Exit();
            }
            else
            {
                // Console app
                Environment.Exit(1);
            }
        }

        // Timer1觸發函式
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (myVid.isPause)
            {
                // 若被暫停, 則計時器停止
                Timer1.Stop();
            }
            else
            {
                Image img = myVid.Play();
                if(img != null) // 播放中
                {
                    pictureBox1.Image = img;
                    pictureBox2.Image = myVid.GetNext();
                    trackBar1.Value = myVid.GetIndex();
                }
                else // 播放結束
                {
                    // 計時器停止
                    Timer1.Stop();
                    myVid.isPause = true;
                    // 暫停後可使用其他功能
                    menuLoad1.Enabled = true;
                    menuLoad2.Enabled = true;
                    menuCompression.Enabled = true;
                }
            }
        }

        // 播放
        private void btnPlay_Click(object sender, EventArgs e)
        {
            myVid.isPause = false;
            // 計時器開始, 執行Timer1_Tick()函式
            Timer1.Start();
            // 播放時無法使用其他功能
            menuLoad1.Enabled = false;
            menuLoad2.Enabled = false;
            menuCompression.Enabled = false;
        }

        // 暫停
        private void btnPause_Click(object sender, EventArgs e)
        {
            myVid.isPause = true;
            // 計時器停止
            Timer1.Stop();
            pictureBox1.Image = myVid.Pause();
            // 暫停後可使用其他功能
            menuLoad1.Enabled = true;
            menuLoad2.Enabled = true;
            menuCompression.Enabled = true;
        }

        // 下一張
        private void btnNext_Click(object sender, EventArgs e)
        {
            Image img = myVid.Next();
            if(img != null)
            {
                pictureBox1.Image = img;
                pictureBox2.Image = myVid.GetNext();
                trackBar1.Value = myVid.GetIndex();
            }
        }

        // 上一張
        private void btnPrev_Click(object sender, EventArgs e)
        {
            Image img = myVid.Prev();
            if (img != null)
            {
                pictureBox1.Image = img;
                pictureBox2.Image = myVid.GetPrev();
                trackBar1.Value = myVid.GetIndex();
            }
        }

        // 播放進度條
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            int value = trackBar1.Value;
            textBox1.Text = value.ToString();
            myVid.SetIndex(value);
            pictureBox1.Image = myVid.GetCurFrame();
            if(value != vidLength - 1)
            {
                pictureBox2.Image = myVid.GetNext();
            }
            else
            {
                pictureBox2.Image = null;
            }
        }

        // 目前播放frame number
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(int.TryParse(textBox1.Text, out int value))
            {
                if (value >= vidLength)
                {
                    value = vidLength - 1;
                    textBox1.Text = value.ToString();
                }
                else if(value < 0)
                {
                    value = 0;
                    textBox1.Text = "0";
                }
                trackBar1.Value = value;
                myVid.SetIndex(value);
                pictureBox1.Image = myVid.GetCurFrame();
                if (value != vidLength - 1)
                {
                    pictureBox2.Image = myVid.GetNext();
                }
                else
                {
                    pictureBox2.Image = null;
                }
            }
        }

        // 開啟Intraframe Sub-sampling功能視窗
        private void menuIntraSample_Click(object sender, EventArgs e)
        {
            // 建立FormIntraSample物件並傳送MyVideo物件
            using (FormIntraSample f = new FormIntraSample { myVid = myVid })
            {
                f.ShowDialog(this); // 設定FormIntraSample為FormMain的上層，並開啟FormIntraSample視窗
                                    // 由於在FormMain的程式碼內使用this，所以this為FormMain物件本身
            }  // 自動釋放資源
        }

        // 開啟Block Based Motion Compensation功能視窗
        private void menuMotion_Click(object sender, EventArgs e)
        {
            // 建立FormMotion物件並傳送MyVideo物件
            using (FormMotion f = new FormMotion { myVid = myVid })
            {
                f.ShowDialog(this); // 設定FormMotion為FormMain的上層，並開啟FormMotion視窗
                                    // 由於在FormMain的程式碼內使用this，所以this為FormMain物件本身
            }  // 自動釋放資源
        }

        // 開啟Block Based Difference Coding功能視窗
        private void menuBlockDiff_Click(object sender, EventArgs e)
        {
            // 建立FormBlockDiff物件並傳送MyVideo物件
            using (FormBlockDiff f = new FormBlockDiff { myVid = myVid })
            {
                f.ShowDialog(this); // 設定FormBlockDiff為FormMain的上層，並開啟FormBlockDiff視窗
                                    // 由於在FormMain的程式碼內使用this，所以this為FormMain物件本身
            }  // 自動釋放資源
        }

        // 開啟Three Step Search功能視窗
        private void menuTSS_Click(object sender, EventArgs e)
        {
            // 建立FormTSS物件並傳送MyVideo物件
            using (FormTSS f = new FormTSS { myVid = myVid })
            {
                f.ShowDialog(this); // 設定FormTSS為FormMain的上層，並開啟FormTSS視窗
                                    // 由於在FormMain的程式碼內使用this，所以this為FormMain物件本身
            }  // 自動釋放資源
        }

        // 開啟Interframe Sub-sampling功能視窗
        private void menuInterSample_Click(object sender, EventArgs e)
        {
            // 建立FormInterSample物件並傳送MyVideo物件
            using (FormInterSample f = new FormInterSample { myVid = myVid })
            {
                f.ShowDialog(this); // 設定FormInterSample為FormMain的上層，並開啟FormInterSample視窗
                                    // 由於在FormMain的程式碼內使用this，所以this為FormMain物件本身
            }  // 自動釋放資源
        }
    }
}
