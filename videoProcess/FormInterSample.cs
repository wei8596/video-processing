using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace videoProcess
{
    public partial class FormInterSample : Form
    {
        public MyVideo myVid;   // MyVideo物件
        int vidLength;          // 影片frame數量
        Timer Timer1;           // 計時器

        public FormInterSample()
        {
            InitializeComponent();
        }

        private void FormInterSample_Load(object sender, EventArgs e)
        {
            // 影像index與處理過的影像初始化
            myVid.Init();
            vidLength = myVid.GetLength();
            // 設定picturebox寬高
            int width = myVid.GetWidth();
            int height = myVid.GetHeight();
            pictureBox1.Width = width;
            pictureBox1.Height = height;
            pictureBox2.Width = width;
            pictureBox2.Height = height;
            pictureBox1.Image = myVid.imgs[0];
            chart1.Visible = false;
            chart1.ChartAreas[0].AxisX.Title = "Frame Number";
            chart1.ChartAreas[0].AxisY.Title = "dB";
            // 壓縮前無法使用媒體功能
            btnPrev.Enabled = false;
            btnPlay.Enabled = false;
            btnPause.Enabled = false;
            btnNext.Enabled = false;
            trackBar1.Enabled = false;
            textBox1.Enabled = false;
            Timer1 = new Timer();
            Timer1.Interval = 100;                          // 時間間隔
            Timer1.Tick += new EventHandler(Timer1_Tick);   // 每過一個Interval就呼叫一次
            trackBar1.Maximum = vidLength - 1;
            trackBar1.Value = 0;
            textBox1.Text = "0";
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
                if (img != null) // 播放中
                {
                    pictureBox1.Image = img;
                    int index = myVid.GetIndex();
                    pictureBox2.Image = myVid.newImgs[index];
                    trackBar1.Value = index;
                }
                else // 播放結束
                {
                    // 計時器停止
                    Timer1.Stop();
                    myVid.isPause = true;
                }
            }
        }

        // Interframe Sub-sampling 
        private void menuCompression_Click(object sender, EventArgs e)
        {
            // 壓縮前無法使用媒體功能
            btnPrev.Enabled = false;
            btnPlay.Enabled = false;
            btnPause.Enabled = false;
            btnNext.Enabled = false;
            trackBar1.Enabled = false;
            textBox1.Enabled = false;

            // 影像index與處理過的影像初始化
            myVid.Init();

            double psnr = 0;  // PSNR

            // 清除圖表資料
            chart1.Series.Clear();

            chart1.Visible = true;
            chart1.Series.Add("PSNR");
            chart1.Series["PSNR"].ChartType = SeriesChartType.Line;
            // 設定圖表格式
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            if (myVid.video == 1)
            {
                chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 7.3F);
                chart1.ChartAreas[0].AxisX.Interval = 2;
                chart1.ChartAreas[0].AxisX.Maximum = 58;
            }
            else
            {
                chart1.ChartAreas[0].AxisX.Interval = 1;
                chart1.ChartAreas[0].AxisX.Maximum = 9;
            }

            // 建立newImgs大小為影像張數
            for (int i = 0; i < vidLength; ++i)
            {
                // 先隨意塞滿newImgs, 之後才能進行位置讀取
                myVid.newImgs.Add(new Bitmap(1, 1));
            }
            // 先將偶數張(0, 2, 4)放到newImgs
            for (int i = 0; i < vidLength; i += 2)
            {
                myVid.newImgs[i] = myVid.imgs[i];
            }
            // 若影像張數為偶數, 最後一張沒有後面的影像可以參考
            // 因此要再加入最後一張
            if(vidLength % 2 == 0)
            {
                int last = vidLength - 1;
                myVid.newImgs[last] = myVid.imgs[last];
            }

            // 第0張的psnr
            // 相同的影像psnr為無限大, 將最大psnr設為100
            int maxPsnr = 30;
            chart1.Series["PSNR"].Points.AddXY(0, maxPsnr);

            // 只處理中間缺的影像
            for (int i = 1; i < vidLength - 1; i += 2)
            {
                myVid.InterSample(i);
                pictureBox1.Image = myVid.imgs[i];
                pictureBox2.Image = myVid.newImgs[i];
                // 處理佇列避免畫面卡死
                Application.DoEvents();

                // 計算PSNR
                psnr = myVid.GetPSNR(pictureBox1.Image, pictureBox2.Image);
                chart1.Series["PSNR"].Points.AddXY(i, psnr);
                if(i + 1 < vidLength)
                {
                    // 相同的影像psnr為無限大, 將最大psnr設為100
                    chart1.Series["PSNR"].Points.AddXY(i + 1, maxPsnr);
                }

                trackBar1.Value = i;
            }
            // 若影像張數為偶數, 再計算最後一張的psnr
            if (vidLength % 2 == 0)
            {
                int last = vidLength - 1;
                // 相同的影像psnr為無限大, 將最大psnr設為100
                chart1.Series["PSNR"].Points.AddXY(last, maxPsnr);
            }
            // 壓縮後開放功能
            btnPrev.Enabled = true;
            btnPlay.Enabled = true;
            btnPause.Enabled = true;
            btnNext.Enabled = true;
            trackBar1.Enabled = true;
            textBox1.Enabled = true;
        }

        // 播放
        private void btnPlay_Click(object sender, EventArgs e)
        {
            myVid.isPause = false;
            // 計時器開始, 執行Timer1_Tick()函式
            Timer1.Start();
        }

        // 暫停
        private void btnPause_Click(object sender, EventArgs e)
        {
            myVid.isPause = true;
            // 計時器停止
            Timer1.Stop();
            pictureBox1.Image = myVid.Pause();
            int index = myVid.GetIndex();
            pictureBox2.Image = myVid.newImgs[index];
        }

        // 下一張
        private void btnNext_Click(object sender, EventArgs e)
        {
            Image img = myVid.Next();
            if (img != null)
            {
                pictureBox1.Image = img;
                int index = myVid.GetIndex();
                pictureBox2.Image = myVid.newImgs[index];
                trackBar1.Value = index;
            }
        }

        // 上一張
        private void btnPrev_Click(object sender, EventArgs e)
        {
            Image img = myVid.Prev();
            if (img != null)
            {
                pictureBox1.Image = img;
                int index = myVid.GetIndex();
                pictureBox2.Image = myVid.newImgs[index];
                trackBar1.Value = index;
            }
        }

        // 播放進度條
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            int value = trackBar1.Value;
            textBox1.Text = value.ToString();
            myVid.SetIndex(value);
            pictureBox1.Image = myVid.GetCurFrame();
            pictureBox2.Image = myVid.newImgs[value];
        }

        // 目前播放frame number
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int value))
            {
                if (value >= vidLength)
                {
                    value = vidLength - 1;
                    textBox1.Text = value.ToString();
                }
                else if (value < 0)
                {
                    value = 0;
                    textBox1.Text = "0";
                }
                trackBar1.Value = value;
                myVid.SetIndex(value);
                pictureBox1.Image = myVid.GetCurFrame();
                pictureBox2.Image = myVid.newImgs[value];
            }
        }
    }
}
