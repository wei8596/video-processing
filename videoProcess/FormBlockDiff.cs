using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace videoProcess
{
    public partial class FormBlockDiff : Form
    {
        public MyVideo myVid;               // MyVideo物件
        int vidLength;                      // 影片frame數量
        Thread thread;                      // 避免畫面卡死, 使用thread執行功能
        System.Windows.Forms.Timer Timer1;  // 計時器
        bool squareOrAbsolute = true;       // true: Mean Square Difference, false: Mean Absolute Difference

        public FormBlockDiff()
        {
            InitializeComponent();
        }

        private void FormBlockDiff_Load(object sender, EventArgs e)
        {
            // 影像index與處理過的影像初始化
            myVid.Init();
            // 將第0張影像放到處理後的list第0張
            myVid.newImgs.Add(myVid.imgs[0]);
            vidLength = myVid.GetLength();
            // 設定picturebox寬高
            int width = myVid.GetWidth();
            int height = myVid.GetHeight();
            pictureBox1.Width = width;
            pictureBox1.Height = height;
            pictureBox2.Width = width;
            pictureBox2.Height = height;
            // 將圖放在background, 可在image畫框框
            pictureBox1.BackgroundImage = new Bitmap(myVid.imgs[0]);
            pictureBox2.BackgroundImage = new Bitmap(myVid.imgs[1]);
            // image預設顏色為透明
            pictureBox1.BackColor = Color.Transparent;
            pictureBox2.BackColor = Color.Transparent;
            // 大小模式為延伸
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            // 預設不畫出block
            checkBox1.Checked = false;
            chart1.Visible = false;
            chart1.ChartAreas[0].AxisX.Title = "Frame Number";
            chart1.ChartAreas[0].AxisY.Title = "dB";
            // 解壓縮前無法使用媒體功能
            btnPrev.Enabled = false;
            btnPlay.Enabled = false;
            btnPause.Enabled = false;
            btnNext.Enabled = false;
            trackBar1.Enabled = false;
            textBox1.Enabled = false;
            Timer1 = new System.Windows.Forms.Timer();
            Timer1.Interval = 100;                          // 時間間隔
            Timer1.Tick += new EventHandler(Timer1_Tick);   // 每過一個Interval就呼叫一次
            trackBar1.Maximum = vidLength - 1;
            trackBar1.Value = 0;
            textBox1.Text = "0";
        }

        // 關閉視窗
        private void FormBlockDiff_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 釋放picturebox資源
            if (pictureBox1.BackgroundImage != null)
            {
                pictureBox1.BackgroundImage.Dispose();
            }
            if (pictureBox2.BackgroundImage != null)
            {
                pictureBox2.BackgroundImage.Dispose();
            }
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image.Dispose();
            }
            if (pictureBox3.Image != null)
            {
                pictureBox3.Image.Dispose();
            }
            if (pictureBox4.Image != null)
            {
                pictureBox4.Image.Dispose();
            }
            // 影像index初始化
            myVid.Init();
            // 若thread未宣告則不另外處理
            if (thread == null)
            {
                return;
            }
            // 否則檢查thread是否還在執行
            else if (thread.IsAlive)
            {
                /*
                 * 1.對執行緒下Abort時，前景執行緒會完成目前工作後才結束，而背景執行緒則會直接中斷工作
                 * 2.背景執行緒在所有前景執行緒都結束後，即會被CLR直接下Abort結束掉
                 */
                thread.Abort();  // 使用Abort()停止它
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

        // Block Based Difference Coding
        private void menuCompression_Click(object sender, EventArgs e)
        {
            // 影像index初始化
            myVid.Init();
            // 將第0張影像放到處理後的list第0張
            myVid.newImgs.Add(myVid.imgs[0]);
            // picturebox初始化
            if (pictureBox1.BackgroundImage != null)
            {
                pictureBox1.BackgroundImage.Dispose();
                pictureBox1.BackgroundImage = new Bitmap(myVid.imgs[0]);
            }
            if (pictureBox2.BackgroundImage != null)
            {
                pictureBox2.BackgroundImage.Dispose();
                pictureBox2.BackgroundImage = new Bitmap(myVid.imgs[1]);
            }
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image.Dispose();
            }
            if (pictureBox3.Image != null)
            {
                pictureBox3.Image.Dispose();
            }
            if (pictureBox4.Image != null)
            {
                pictureBox4.Image.Dispose();
            }
            // 壓縮中無法改變是否畫出block
            checkBox1.Visible = false;

            thread = new Thread(new ThreadStart(new Action(() => {
                for (int i = 1; i < vidLength; ++i)
                {
                    // BlockDiffCO(imgIndex, blockPicBox, searchPicBox, curPicBox, refPicBox, checkBox, squareOrAbsolute)
                    myVid.BlockDiffCO(i, pictureBox3, pictureBox4, pictureBox2, pictureBox1, checkBox1, squareOrAbsolute);
                    if (pictureBox1.BackgroundImage != null)
                    {
                        pictureBox1.BackgroundImage.Dispose();
                        pictureBox1.BackgroundImage = new Bitmap(myVid.Next());
                    }
                    if (pictureBox2.BackgroundImage != null)
                    {
                        pictureBox2.BackgroundImage.Dispose();
                        if (i != myVid.GetLength() - 1)
                        {
                            pictureBox2.BackgroundImage = new Bitmap(myVid.GetNext());
                        }
                        else
                        {
                            pictureBox2.BackgroundImage = null;
                        }
                    }
                }
                Debug.Print("-----All Compression Done-----");
            })));
            // the .NET Framework will automatically kill any threads whose
            // IsBackground property is set to "True".
            thread.IsBackground = true;  // 設為背景執行緒
            thread.Start();

            // 結束後可改變是否畫出block
            checkBox1.Visible = true;
        }

        // Block Based Difference Coding Decompression
        private void menuDecompression_Click(object sender, EventArgs e)
        {
            // 影像index初始化
            myVid.Init();
            // 將第0張影像放到處理後的list第0張
            myVid.newImgs.Add(myVid.imgs[0]);
            // 座標List位置初始化
            myVid.SetPosIdx(0);
            // picturebox初始化
            if (pictureBox1.BackgroundImage != null)
            {
                pictureBox1.BackgroundImage.Dispose();
                // 原始影像
                pictureBox1.BackgroundImage = new Bitmap(myVid.imgs[0]);
            }
            if (pictureBox2.BackgroundImage != null)
            {
                pictureBox2.BackgroundImage.Dispose();
                // 解壓縮後影像
                pictureBox2.BackgroundImage = new Bitmap(myVid.newImgs[0]);
            }
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image.Dispose();
            }
            if (pictureBox3.Image != null)
            {
                pictureBox3.Image.Dispose();
            }
            if (pictureBox4.Image != null)
            {
                pictureBox4.Image.Dispose();
            }
            // 解壓縮不提供畫block功能
            checkBox1.Visible = false;

            // 若還沒計算過向量, 直接從檔案中讀取
            if (myVid.vectors.Count == 0)
            {
                string fileName;
                int vectorX, vectorY;
                try
                {
                    if (myVid.video == 1)
                    {
                        fileName = "diff1.txt";
                    }
                    else
                    {
                        fileName = "diff2.txt";
                    }
                    // 從檔案讀取向量
                    //using (StreamReader sr = new StreamReader("diff.txt"))
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        while (!sr.EndOfStream)
                        {
                            vectorX = int.Parse(sr.ReadLine());
                            vectorY = int.Parse(sr.ReadLine());
                            myVid.pos.Add(new List<int>() { vectorX, vectorY });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The file could not be read:" + ex.Message);
                }
            }

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

            // 解壓縮並與原影像同時播放提供對照
            for (int i = 1; i < vidLength; ++i)
            {
                // BlockDiffDEC(imgIndex)
                myVid.BlockDiffDEC(i);
                if (pictureBox1.BackgroundImage != null)
                {
                    pictureBox1.BackgroundImage.Dispose();
                    // 原始影像
                    pictureBox1.BackgroundImage = new Bitmap(myVid.Next());
                    // 處理佇列避免畫面卡死
                    Application.DoEvents();
                }
                if (pictureBox2.BackgroundImage != null)
                {
                    pictureBox2.BackgroundImage.Dispose();
                    // 解壓縮後影像
                    pictureBox2.BackgroundImage = new Bitmap(myVid.newImgs[i]);
                    // 處理佇列避免畫面卡死
                    Application.DoEvents();
                }

                // 計算PSNR
                psnr = myVid.GetPSNR(pictureBox1.BackgroundImage, pictureBox2.BackgroundImage);
                chart1.Series["PSNR"].Points.AddXY(i, psnr);

                trackBar1.Value = myVid.GetIndex();
            }
            Debug.Print("-----All Decompression Done-----");

            // 解壓縮後開放功能
            btnPrev.Enabled = true;
            btnPlay.Enabled = true;
            btnPause.Enabled = true;
            btnNext.Enabled = true;
            trackBar1.Enabled = true;
            textBox1.Enabled = true;
            checkBox1.Visible = true;
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

        private void radioSquare_CheckedChanged(object sender, EventArgs e)
        {
            if(radioSquare.Checked == true)
            {
                squareOrAbsolute = true;
            }
            else
            {
                squareOrAbsolute = false;
            }
        }

        private void radioAbsolute_CheckedChanged(object sender, EventArgs e)
        {
            if (radioAbsolute.Checked == true)
            {
                squareOrAbsolute = false;
            }
            else
            {
                squareOrAbsolute = true;
            }
        }
    }
}
