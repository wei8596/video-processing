using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace videoProcess
{
    public partial class FormIntraSample : Form
    {
        public MyVideo myVid;   // MyVideo物件
        int vidLength;          // 影片frame數量
        Timer Timer1;           // 計時器

        public FormIntraSample()
        {
            InitializeComponent();
        }

        private void FormSample_Load(object sender, EventArgs e)
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

        // Intraframe Sub-sampling (複製pixel方法)
        private void menuCopy_Click(object sender, EventArgs e)
        {
            // 壓縮前無法使用媒體功能
            btnPrev.Enabled = false;
            btnPlay.Enabled = false;
            btnPause.Enabled = false;
            btnNext.Enabled = false;
            trackBar1.Enabled = false;
            textBox1.Enabled = false;

            double psnr;  // PSNR

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

            // 影像index與處理過的影像初始化
            myVid.Init();
            for (int i = 0; i < vidLength; ++i)
            {
                myVid.IntraSampleCopy(i);
                pictureBox1.Image = myVid.imgs[i];
                pictureBox2.Image = myVid.newImgs[i];
                // 處理佇列避免畫面卡死
                Application.DoEvents();

                // 計算PSNR
                psnr = myVid.GetPSNR(pictureBox1.Image, pictureBox2.Image);
                chart1.Series["PSNR"].Points.AddXY(i, psnr);

                trackBar1.Value = i;
            }
            // 壓縮後開放功能
            btnPrev.Enabled = true;
            btnPlay.Enabled = true;
            btnPause.Enabled = true;
            btnNext.Enabled = true;
            trackBar1.Enabled = true;
            textBox1.Enabled = true;
        }

        // Intraframe Sub-sampling (平均pixel方法)
        private void menuAverage_Click(object sender, EventArgs e)
        {
            // 壓縮前無法使用媒體功能
            btnPrev.Enabled = false;
            btnPlay.Enabled = false;
            btnPause.Enabled = false;
            btnNext.Enabled = false;
            trackBar1.Enabled = false;
            textBox1.Enabled = false;

            double psnr;  // PSNR

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

            // 影像index與處理過的影像初始化
            myVid.Init();
            for (int i = 0; i < vidLength; ++i)
            {
                myVid.IntraSampleAvg(i);
                pictureBox1.Image = myVid.imgs[i];
                pictureBox2.Image = myVid.newImgs[i];
                // 處理佇列避免畫面卡死
                Application.DoEvents();

                // 計算PSNR
                psnr = myVid.GetPSNR(pictureBox1.Image, pictureBox2.Image);
                chart1.Series["PSNR"].Points.AddXY(i, psnr);

                trackBar1.Value = i;
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
