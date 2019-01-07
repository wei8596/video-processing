using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace videoProcess
{
    public class MyVideo
    {
        public int video;                                       // 記錄選擇的影片(1 or 2)
        int imgsIndex;                                          // 影像index
        int vecIndex;                                           // 記錄位移向量在List中的位置
        public bool isPause = false;                            // 儲存是否為暫停狀態
        public List<Image> imgs = new List<Image>();            // 儲存每張影像
        public List<Image> newImgs = new List<Image>();         // 儲存處理後的每張影像
        public List<List<int>> vectors = new List<List<int>>(); // 儲存位移向量
        public List<Bitmap> vecImg = new List<Bitmap>();        // 儲存位移向量圖
        public List<List<int>> pos = new List<List<int>>();     // 儲存座標與pixel值

        /*
         * class myVideo constructor
         * @video : 選擇的影片
         */
        public MyVideo(int num)
        {
            imgs.Clear();
            imgsIndex = 0;
            if (num == 1)
            {
                video = 1;
                Load();
            }
            else
            {
                video = 2;
                Load();
            }
        }

        /*
         * class myVideo destructor
         */
        ~MyVideo()
        {
            imgs.Clear();
            newImgs.Clear();
            vectors.Clear();
            vecImg.Clear();
        }

        /*
         * 影像index與處理過的影像初始化
         */
        public void Init()
        {
            imgsIndex = 0;
            // 清空已處理過的影像
            newImgs.Clear();
        }

        /*
         * 讀取選擇的影片
         */
        private void Load()
        {
            string filePre, num, extension, fileName;
            if(video == 1)
            {
                filePre = @"D:\image_processing\course\sequences\6.1.";
                num = "";
                extension = ".tiff";
                fileName = "";
                for (int i = 1; i <= 16; ++i)
                {
                    num = "";
                    if (i < 10)
                    {
                        num = "0";
                    }
                    num += i.ToString();
                    fileName = (filePre + num + extension);
                    using (Bitmap bmp = (Bitmap)Image.FromFile(fileName))
                    {
                        using (MemoryStream byteStream = new MemoryStream())
                        {
                            // 將資料寫入記憶體
                            bmp.Save(byteStream, ImageFormat.Tiff);
                            // 從記憶體中的資料建立圖像
                            imgs.Add(Image.FromStream(byteStream));
                        }
                    }
                }
                filePre = @"D:\image_processing\course\sequences\6.2.";
                for (int i = 1; i <= 32; ++i)
                {
                    num = "";
                    if (i < 10)
                    {
                        num = "0";
                    }
                    num += i.ToString();
                    fileName = (filePre + num + extension);
                    using (Bitmap bmp = (Bitmap)Image.FromFile(fileName))
                    {
                        using (MemoryStream byteStream = new MemoryStream())
                        {
                            // 將資料寫入記憶體
                            bmp.Save(byteStream, ImageFormat.Tiff);
                            // 從記憶體中的資料建立圖像
                            imgs.Add(Image.FromStream(byteStream));
                        }
                    }
                }
                filePre = @"D:\image_processing\course\sequences\6.3.";
                for (int i = 1; i <= 11; ++i)
                {
                    num = "";
                    if (i < 10)
                    {
                        num = "0";
                    }
                    num += i.ToString();
                    fileName = (filePre + num + extension);
                    using (Bitmap bmp = (Bitmap)Image.FromFile(fileName))
                    {
                        using (MemoryStream byteStream = new MemoryStream())
                        {
                            // 將資料寫入記憶體
                            bmp.Save(byteStream, ImageFormat.Tiff);
                            // 從記憶體中的資料建立圖像
                            imgs.Add(Image.FromStream(byteStream));
                        }
                    }
                }
            }
            else
            {
                filePre = @"D:\image_processing\course\sequences\motion";
                num = "";
                extension = ".512.tiff";
                fileName = "";
                for(int i = 1; i <= 10; ++i)
                {
                    num = "";
                    if (i < 10)
                    {
                        num = "0";
                    }
                    num += i.ToString();
                    fileName = (filePre + num + extension);
                    using (Bitmap bmp = (Bitmap)Image.FromFile(fileName))
                    {
                        using (MemoryStream byteStream = new MemoryStream())
                        {
                            // 將資料寫入記憶體
                            bmp.Save(byteStream, ImageFormat.Tiff);
                            // 從記憶體中的資料建立圖像
                            imgs.Add(Image.FromStream(byteStream));
                        }
                    }
                }
            }
        }

        /*
         * 播放
         */
        public Image Play()
        {
            if (imgsIndex + 1 >= imgs.Count)
            {
                Debug.Print("Play index error");
                return null;
            }
            return imgs[++imgsIndex];
        }

        /*
         * 暫停
         */
        public Image Pause()
        {
            return imgs[imgsIndex];
        }

        /*
         * 下一張
         */
        public Image Next()
        {
            if (imgsIndex + 1 >= imgs.Count)
            {
                Debug.Print("Next index error");
                return null;
            }
            return imgs[++imgsIndex];
        }

        /*
         * 取得下一張, 不增加imgsIndex
         */
        public Image GetNext()
        {
            int next = imgsIndex + 1;
            if (next >= imgs.Count)
            {
                return null;
            }
            return imgs[next];
        }

        /*
         * 上一張
         */
        public Image Prev()
        {
            if(imgsIndex - 1 < 0)
            {
                Debug.Print("Prev index error");
                return null;
            }
            return imgs[--imgsIndex];
        }

        /*
         * 取得上一張, 不增加imgsIndex
         */
        public Image GetPrev()
        {
            int prev = imgsIndex - 1;
            if (prev < 0)
            {
                return null;
            }
            return imgs[prev];
        }

        /*
         * 取得目前影像的位置
         * 
         * @return : 目前影像的位置
         */
        public int GetIndex()
        {
            return imgsIndex;
        }

        /*
         * 設定目前影像的位置
         */
        public void SetIndex(int index)
        {
            imgsIndex = index;
        }

        /*
         * 取得影像寬度
         * 
         * @return : 影像寬度
         */
        public int GetWidth()
        {
            return imgs[0].Width;
        }

        /*
         * 取得影像高度
         * 
         * @return : 影像高度
         */
        public int GetHeight()
        {
            return imgs[0].Height;
        }

        /*
         * 取得影像張數
         * 
         * @return : 影像數量
         */
        public int GetLength()
        {
            return imgs.Count;
        }

        /*
         * 取得目前frame
         * 
         * @return : frame
         */
        public Image GetCurFrame()
        {
            return imgs[imgsIndex];
        }

        /*
         * 取得指定index的frame
         * 
         * @return : frame
         */
        public Image GetIdxFrame(int index)
        {
            return imgs[index];
        }

        /*
         * 取得在位移向量List中的位置
         * 
         * @reutrn : 位置
         */
        public int GetVecIdx()
        {
            return vecIndex;
        }

        /*
         * 設定在位移向量List中的位置
         */
        public void SetVecIdx(int index)
        {
            vecIndex = index;
        }

        /*
         * 在位移向量List中的位置增加1
         */
        public void IncVecIdx()
        {
            ++vecIndex;
        }

        /*
         * 取得在座標List中的位置
         * 
         * @reutrn : 位置
         */
        public int GetPosIdx()
        {
            return vecIndex;
        }

        /*
         * 設定在座標List中的位置
         */
        public void SetPosIdx(int index)
        {
            vecIndex = index;
        }

        /*
         * 在座標List中的位置增加1
         */
        public void IncPosIdx()
        {
            ++vecIndex;
        }

        /*
         * 取得灰階影像峰值信噪比(PSNR)
         * @src1 : 影像1
         * @src2 : 影像2
         * 
         * @return : PSNR值
         */
        public double GetPSNR(Image src1, Image src2)
        {
            Bitmap bmpSrc1 = (Bitmap)src1;
            // 鎖定影像內容到記憶體
            // 將圖的資料存到記憶體, 可以直接對它操作
            BitmapData dataSrc1 = bmpSrc1.LockBits(new Rectangle(0, 0, bmpSrc1.Width, bmpSrc1.Height), ImageLockMode.ReadOnly, bmpSrc1.PixelFormat);
            // Stride - 影像scan的寬度
            byte[] bytesSrc1 = new byte[dataSrc1.Stride * dataSrc1.Height]; // 存放整個圖像資料
            // 將圖像資料複製到陣列
            // Marshal.Copy(srcPtr, dst, startIndex, length)
            // Scan0 - 影像資料的起始位置
            Marshal.Copy(dataSrc1.Scan0, bytesSrc1, 0, bytesSrc1.Length);

            Bitmap bmpSrc2 = (Bitmap)src2;
            // 鎖定影像內容到記憶體
            // 將圖的資料存到記憶體, 可以直接對它操作
            BitmapData dataSrc2 = bmpSrc2.LockBits(new Rectangle(0, 0, bmpSrc2.Width, bmpSrc2.Height), ImageLockMode.ReadOnly, bmpSrc2.PixelFormat);
            // Stride - 影像scan的寬度
            byte[] bytesSrc2 = new byte[dataSrc2.Stride * dataSrc2.Height]; // 存放整個圖像資料
            // 將圖像資料複製到陣列
            // Marshal.Copy(srcPtr, dst, startIndex, length)
            // Scan0 - 影像資料的起始位置
            Marshal.Copy(dataSrc2.Scan0, bytesSrc2, 0, bytesSrc2.Length);

            int index = 0;
            int height = bmpSrc1.Height;
            int width = bmpSrc1.Width;
            double pixel1, pixel2;
            double noise = 0;
            double frameSize = width * height;
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    pixel1 = bytesSrc1[index];
                    pixel2 = bytesSrc2[index];
                    noise += (pixel1 - pixel2) * (pixel1 - pixel2);
                    ++index;
                }
            }
            double mse = noise / frameSize;

            // 解除鎖定記憶體
            bmpSrc1.UnlockBits(dataSrc1);
            bmpSrc2.UnlockBits(dataSrc2);

            return 10.0 * Math.Log10(255 * 255 / mse); // PSNR
        }

        /*
         * Intraframe Sub-sampling (複製pixel方法)
         * 每次跳過一行和一列, 並用左上角的點填補周圍其他三個點
         * 可以先複製給右邊, 下一列直接複製上一列
         * @imgIndex    :   the number of the frame
         */
        public void IntraSampleCopy(int imgIndex)
        {
            int height = GetHeight();
            int width = GetWidth();

            // 原圖
            Bitmap img = new Bitmap(imgs[imgIndex]);
            BitmapData imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadOnly, img.PixelFormat);
            byte[] imgBytes = new byte[imgData.Stride * imgData.Height];
            Marshal.Copy(imgData.Scan0, imgBytes, 0, imgBytes.Length);

            // 壓縮後的圖
            Bitmap comImg = new Bitmap(width, height);
            BitmapData comImgData = comImg.LockBits(new Rectangle(0, 0, comImg.Width, comImg.Height), ImageLockMode.ReadWrite, comImg.PixelFormat);
            byte[] comImgBytes = new byte[comImgData.Stride * comImgData.Height];
            Marshal.Copy(comImgData.Scan0, comImgBytes, 0, comImgBytes.Length);

            int index;
            byte pixelR, pixelG, pixelB;
            for(int y = 0; y < height; y += 2)
            {
                for(int x = 0; x < width; x += 2)
                {
                    index = 4 * (y * width + x);
                    comImgBytes[index] = imgBytes[index];           // R
                    comImgBytes[index + 1] = imgBytes[index + 1];   // G
                    comImgBytes[index + 2] = imgBytes[index + 2];   // B
                    comImgBytes[index + 3] = 255;                   // A (255 : 不透明)
                    // 記住左上角的像素
                    pixelR = comImgBytes[index];
                    pixelG = comImgBytes[index + 1];
                    pixelB = comImgBytes[index + 2];
                    // 移動到右邊的點
                    index += 4;
                    comImgBytes[index] = pixelR;
                    comImgBytes[index + 1] = pixelG;
                    comImgBytes[index + 2] = pixelB;
                    comImgBytes[index + 3] = 255;
                    // 移動到下方的點
                    index = 4 * ((y + 1) * width + x);
                    comImgBytes[index] = pixelR;
                    comImgBytes[index + 1] = pixelG;
                    comImgBytes[index + 2] = pixelB;
                    comImgBytes[index + 3] = 255;
                    // 移動到右下角的點
                    index += 4;
                    comImgBytes[index] = pixelR;
                    comImgBytes[index + 1] = pixelG;
                    comImgBytes[index + 2] = pixelB;
                    comImgBytes[index + 3] = 255;
                }
            }

            // 將資料複製到圖像物件
            // Marshal.Copy(src, startIndex, dstPtr, length)
            // Scan0 - 影像資料的起始位置
            Marshal.Copy(comImgBytes, 0, comImgData.Scan0, comImgBytes.Length);
            // 解除鎖定記憶體
            img.UnlockBits(imgData);
            comImg.UnlockBits(comImgData);
            // 壓縮後存入新的list
            newImgs.Add(comImg);
        }

        /*
         * Intraframe Sub-sampling (平均pixel方法)
         * 每塊方形內的4個pixel取平均
         * 並讓這4個點的pixel值改為平均值
         * @imgIndex    :   the number of the frame
         */
        public void IntraSampleAvg(int imgIndex)
        {
            int height = GetHeight();
            int width = GetWidth();

            // 原圖
            Bitmap img = new Bitmap(imgs[imgIndex]);
            BitmapData imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadOnly, img.PixelFormat);
            byte[] imgBytes = new byte[imgData.Stride * imgData.Height];
            Marshal.Copy(imgData.Scan0, imgBytes, 0, imgBytes.Length);

            // 壓縮後的圖
            Bitmap comImg = new Bitmap(width, height);
            BitmapData comImgData = comImg.LockBits(new Rectangle(0, 0, comImg.Width, comImg.Height), ImageLockMode.ReadWrite, comImg.PixelFormat);
            byte[] comImgBytes = new byte[comImgData.Stride * comImgData.Height];
            Marshal.Copy(comImgData.Scan0, comImgBytes, 0, comImgBytes.Length);

            int index, index2, index3, index4;
            byte[] pixelR = new byte[4];
            byte[] pixelG = new byte[4];
            byte[] pixelB = new byte[4];
            float avgR_f, avgG_f, avgB_f;
            byte avgR, avgG, avgB;
            for (int y = 0; y < height; y += 2)
            {
                for (int x = 0; x < width; x += 2)
                {
                    index = 4 * (y * width + x);
                    index2 = index + 4;
                    index3 = 4 * ((y + 1) * width + x);
                    index4 = index3 + 4;
                    // 儲存左上像素
                    pixelR[0] = imgBytes[index];
                    pixelG[0] = imgBytes[index + 1];
                    pixelB[0] = imgBytes[index + 2];
                    // 儲存右上像素
                    pixelR[1] = imgBytes[index2];
                    pixelG[1] = imgBytes[index2 + 1];
                    pixelB[1] = imgBytes[index2 + 2];
                    // 儲存左下像素
                    pixelR[2] = imgBytes[index3];
                    pixelG[2] = imgBytes[index3 + 1];
                    pixelB[2] = imgBytes[index3 + 2];
                    // 儲存右下像素
                    pixelR[3] = imgBytes[index4];
                    pixelG[3] = imgBytes[index4 + 1];
                    pixelB[3] = imgBytes[index4 + 2];

                    // 計算平均值
                    avgR_f = 0;
                    avgG_f = 0;
                    avgB_f = 0;
                    for (int i = 0; i < 4; ++i)
                    {
                        avgR_f += pixelR[i];
                        avgG_f += pixelG[i];
                        avgB_f += pixelB[i];
                    }
                    avgR_f /= 4.0F;
                    avgG_f /= 4.0F;
                    avgB_f /= 4.0F;

                    // 用平均值填補
                    avgR = (byte)avgR_f;
                    avgG = (byte)avgG_f;
                    avgB = (byte)avgB_f;
                    comImgBytes[index] = avgR;
                    comImgBytes[index + 1] = avgG;
                    comImgBytes[index + 2] = avgB;
                    comImgBytes[index + 3] = 255;
                    comImgBytes[index2] = avgR;
                    comImgBytes[index2 + 1] = avgG;
                    comImgBytes[index2 + 2] = avgB;
                    comImgBytes[index2 + 3] = 255;
                    comImgBytes[index3] = avgR;
                    comImgBytes[index3 + 1] = avgG;
                    comImgBytes[index3 + 2] = avgB;
                    comImgBytes[index3 + 3] = 255;
                    comImgBytes[index4] = avgR;
                    comImgBytes[index4 + 1] = avgG;
                    comImgBytes[index4 + 2] = avgB;
                    comImgBytes[index4 + 3] = 255;
                }
            }

            // 將資料複製到圖像物件
            // Marshal.Copy(src, startIndex, dstPtr, length)
            // Scan0 - 影像資料的起始位置
            Marshal.Copy(comImgBytes, 0, comImgData.Scan0, comImgBytes.Length);
            // 解除鎖定記憶體
            img.UnlockBits(imgData);
            comImg.UnlockBits(comImgData);
            // 壓縮後存入新的list
            newImgs.Add(comImg);
        }

        /*
         * Square Difference
         * @x       :   pixel1
         * @y       :   pixel2
         * 
         * @return  :   the square difference between x and y
         */
        private double SquareDiff(byte x, byte y)
        {
            return Math.Pow(x - y, 2);
        }

        /*
         * Absolute Difference
         * @x       :   pixel1
         * @y       :   pixel2
         * 
         * @return  :   the absolute difference between x and y
         */
        private double AbsoluteDiff(byte x, byte y)
        {
            return Math.Abs(x - y);
        }

        /*
         * Block Based Difference Coding
         * @imgIndex            :   the number of the frame
         * @blockPicBox         :   current block之PictureBox
         * @searchPicBox        :   candidate block之PictureBox
         * @curPicBox           :   current frame之PictureBox
         * @refPicBox           :   reference frame之PictureBox
         * @checkBox            :   決定是否畫出block
         * @squareOrAbsolute    :   選擇計算相似度的方法
         */
        public void BlockDiffCO(int imgIndex, PictureBox blockPicBox, PictureBox searchPicBox, PictureBox curPicBox, PictureBox refPicBox, CheckBox checkBox, bool squareOrAbsolute)
        {
            // current frame
            Bitmap current = new Bitmap(imgs[imgIndex]);  // 預設為PixelFormat.Format32bppArgb
            BitmapData currentData = current.LockBits(new Rectangle(0, 0, current.Width, current.Height), ImageLockMode.ReadOnly, current.PixelFormat);
            byte[] currentBytes = new byte[currentData.Stride * currentData.Height];
            Marshal.Copy(currentData.Scan0, currentBytes, 0, currentBytes.Length);

            // current block
            Bitmap curBlock = new Bitmap(8, 8); // block size = 8 x 8
            BitmapData curBlockData = curBlock.LockBits(new Rectangle(0, 0, curBlock.Width, curBlock.Height), ImageLockMode.ReadWrite, curBlock.PixelFormat);
            byte[] curBlockBytes = new byte[curBlockData.Stride * curBlockData.Height];
            Marshal.Copy(curBlockData.Scan0, curBlockBytes, 0, curBlockBytes.Length);

            // reference frame
            Bitmap reference = new Bitmap(imgs[imgIndex - 1]); // 前一張
            BitmapData referenceData = reference.LockBits(new Rectangle(0, 0, reference.Width, reference.Height), ImageLockMode.ReadOnly, reference.PixelFormat);
            byte[] referenceBytes = new byte[referenceData.Stride * referenceData.Height];
            Marshal.Copy(referenceData.Scan0, referenceBytes, 0, referenceBytes.Length);

            // candidate block
            Bitmap candBlock = new Bitmap(8, 8); // block size = 8 x 8
            BitmapData candBlockData = candBlock.LockBits(new Rectangle(0, 0, candBlock.Width, candBlock.Height), ImageLockMode.ReadWrite, candBlock.PixelFormat);
            byte[] candBlockBytes = new byte[candBlockData.Stride * candBlockData.Height];
            Marshal.Copy(candBlockData.Scan0, candBlockBytes, 0, candBlockBytes.Length);

            int height = current.Height;
            int width = current.Width;
            int totalBlocks = (int)((height / 8.0) * (width / 8.0));
            int curIdx, curBlockIdx, tmpX, tmpY, curX = 0, curY = 0;
            int searchX, searchY, refIdx;
            double sum, diff;
            const int THRESHOLD = 128;
            bool showBlock = checkBox.Checked;  // 是否要畫出block

            // 
            MyEvent ShowRec = new MyEvent();
            MyEvent DrawRec = new MyEvent();
            // 
            ShowRec.ShowBlockEvent += new MyEvent.ShowBlockDelegate(ShowBlock);
            DrawRec.DrawBlockEvent += new MyEvent.DrawBlockDelegate(DrawBlock);

            // 掃描整張current frame(blocks不重疊)
            for (int i = 0; i < totalBlocks; ++i)
            {
                Array.Clear(curBlockBytes, 0, curBlockBytes.Length);
                // 備份current block左上角座標
                tmpX = curX;
                tmpY = curY;
                curBlockIdx = 0;
                // 取出current block
                for (int y = 0; y < 8; ++y)
                {
                    curX = tmpX;
                    for (int x = 0; x < 8; ++x)
                    {
                        curIdx = 4 * (curY * width + curX);
                        curBlockBytes[curBlockIdx] = currentBytes[curIdx];          // R
                        curBlockBytes[curBlockIdx + 1] = currentBytes[curIdx + 1];  // G
                        curBlockBytes[curBlockIdx + 2] = currentBytes[curIdx + 2];  // B
                        curBlockBytes[curBlockIdx + 3] = 255;                       // A (255 : 不透明)
                        curBlockIdx += 4;
                        ++curX;
                    }
                    ++curY;
                }
                if (showBlock)
                {
                    // 將current block放大顯示在PictureBox
                    ShowRec.RaiseShowBlock(curBlockBytes, blockPicBox);
                    // 在current frame畫出current block的邊框
                    DrawRec.RaiseDrawBlock(tmpX, tmpY, curPicBox);
                }

                // 檢查reference frame對應位置的block
                searchX = tmpX;
                searchY = tmpY;
                sum = 0;
                curBlockIdx = 0;
                for (int y = 0; y < 8; ++y)
                {
                    searchX = tmpX;
                    for (int x = 0; x < 8; ++x)
                    {
                        refIdx = 4 * (searchY * width + searchX);
                        candBlockBytes[curBlockIdx] = referenceBytes[refIdx];           // R
                        candBlockBytes[curBlockIdx + 1] = referenceBytes[refIdx + 1];   // G
                        candBlockBytes[curBlockIdx + 2] = referenceBytes[refIdx + 2];   // B
                        candBlockBytes[curBlockIdx + 3] = 255;                          // A (255 : 不透明)
                        // 灰階的rgb皆相同, 取一個通道計算即可
                        if(squareOrAbsolute == true)
                        {
                            sum += SquareDiff(curBlockBytes[curBlockIdx], referenceBytes[refIdx]);
                        }
                        else
                        {
                            sum += AbsoluteDiff(curBlockBytes[curBlockIdx], referenceBytes[refIdx]);
                        }
                        curBlockIdx += 4;
                        ++searchX;
                    }
                    ++searchY;
                }
                // 若超過threshold則記錄座標
                diff = sum / 64;
                if (diff > THRESHOLD)
                {
                    pos.Add(new List<int>() { tmpX, tmpY });
                }
                if (showBlock)
                {
                    // 將candidate block放大顯示在PictureBox
                    ShowRec.RaiseShowBlock(candBlockBytes, searchPicBox);
                    // 在reference frame畫出search window的邊框
                    DrawRec.RaiseDrawBlock(tmpX, tmpY, refPicBox);
                }

                // 回復與更新座標
                curY = tmpY;
                if (curX == width)
                {
                    curX = 0;
                    curY += 8;
                }
            }
            // 將資料複製到圖像物件
            // Marshal.Copy(src, startIndex, dstPtr, length)
            // Scan0 - 影像資料的起始位置
            Marshal.Copy(curBlockBytes, 0, curBlockData.Scan0, curBlockBytes.Length);
            // 解除鎖定記憶體
            current.UnlockBits(currentData);
            curBlock.UnlockBits(curBlockData);
            reference.UnlockBits(referenceData);
            current.Dispose();
            curBlock.Dispose();
            reference.Dispose();

            // 將位移向量寫入檔案
            string fileName;
            if (video == 1)
            {
                fileName = "diff1.txt";
            }
            else
            {
                fileName = "diff2.txt";
            }
            //using (TextWriter tw = new StreamWriter("diff.txt"))
            using (TextWriter tw = new StreamWriter(fileName))
            {
                foreach (List<int> l in pos)
                {
                    tw.WriteLine(l[0]);
                    tw.WriteLine(l[1]);
                }
            }
            Debug.Print("Frame" + imgIndex.ToString() + " Compression Done!");
        }

        /*
         * Block Based Difference Coding Decompression
         * @imgIndex    :   the number of the frame
         */
        public void BlockDiffDEC(int imgIndex)
        {
            int height = GetHeight();
            int width = GetWidth();
            int totalBlocks = (int)((height / 8.0) * (width / 8.0));

            // decompression frame
            Bitmap decFrame = new Bitmap(newImgs[imgIndex - 1]);// 解壓後的前一張
            BitmapData decData = decFrame.LockBits(new Rectangle(0, 0, decFrame.Width, decFrame.Height), ImageLockMode.ReadWrite, decFrame.PixelFormat);
            byte[] decBytes = new byte[decData.Stride * decData.Height];
            Marshal.Copy(decData.Scan0, decBytes, 0, decBytes.Length);

            // reference frame
            Bitmap reference = new Bitmap(imgs[imgIndex - 1]); // 原本的前一張
            BitmapData referenceData = reference.LockBits(new Rectangle(0, 0, reference.Width, reference.Height), ImageLockMode.ReadOnly, reference.PixelFormat);
            byte[] referenceBytes = new byte[referenceData.Stride * referenceData.Height];
            Marshal.Copy(referenceData.Scan0, referenceBytes, 0, referenceBytes.Length);

            int index, tmpX, tmpY, curX = 0, curY = 0;
            int posIdx;

            // 掃描整張current frame(blocks不重疊)
            for (int i = 0; i < totalBlocks; ++i)
            {
                // 取出紀錄中的位移向量, 計算目標block起始位置
                posIdx = GetPosIdx();
                // 備份current block座標
                tmpX = curX;
                tmpY = curY;

                if(posIdx < pos.Count)
                {
                    // 若是記錄中的座標, 保留原本的值
                    if (curX == pos[posIdx][0] && curY == pos[posIdx][1])
                    {
                        IncPosIdx();
                        continue;
                    }
                }

                // current block位置
                for (int y = 0; y < 8; ++y)
                {
                    curX = tmpX;
                    for (int x = 0; x < 8; ++x)
                    {
                        index = 4 * (curY * width + curX);
                        decBytes[index] = referenceBytes[index];            // R
                        decBytes[index + 1] = referenceBytes[index + 1];    // G
                        decBytes[index + 2] = referenceBytes[index + 2];    // B
                        decBytes[index + 3] = 255;                          // A (255 : 不透明)
                        ++curX;
                    }
                    ++curY;
                }

                // 回復與更新座標
                curY = tmpY;
                if (curX == width)
                {
                    curX = 0;
                    curY += 8;
                }
            }

            // 將資料複製到圖像物件
            // Marshal.Copy(src, startIndex, dstPtr, length)
            // Scan0 - 影像資料的起始位置
            Marshal.Copy(decBytes, 0, decData.Scan0, decBytes.Length);
            // 解除鎖定記憶體
            decFrame.UnlockBits(decData);
            reference.UnlockBits(referenceData);

            // 解壓縮後存入新的list
            newImgs.Add(decFrame);
            Debug.Print("Frame" + imgIndex.ToString() + " Decompression Done!");
        }

        /*
         * Block Based Motion Compensation Compression
         * @imgIndex    :   the number of the frame
         * @blockPicBox :   current block之PictureBox
         * @searchPicBox:   candidate block之PictureBox
         * @curPicBox   :   current frame之PictureBox
         * @refPicBox   :   reference frame之PictureBox
         * @checkBox    :   決定是否畫出block
         */
        public void MotionCO(int imgIndex, PictureBox blockPicBox, PictureBox searchPicBox, PictureBox curPicBox, PictureBox refPicBox, CheckBox checkBox)
        {
            // current frame
            Bitmap current = new Bitmap(imgs[imgIndex]);  // 預設為PixelFormat.Format32bppArgb
            BitmapData currentData = current.LockBits(new Rectangle(0, 0, current.Width, current.Height), ImageLockMode.ReadOnly, current.PixelFormat);
            byte[] currentBytes = new byte[currentData.Stride * currentData.Height];
            Marshal.Copy(currentData.Scan0, currentBytes, 0, currentBytes.Length);

            // current block
            Bitmap curBlock = new Bitmap(8, 8); // block size = 8 x 8
            BitmapData curBlockData = curBlock.LockBits(new Rectangle(0, 0, curBlock.Width, curBlock.Height), ImageLockMode.ReadWrite, curBlock.PixelFormat);
            byte[] curBlockBytes = new byte[curBlockData.Stride * curBlockData.Height];
            Marshal.Copy(curBlockData.Scan0, curBlockBytes, 0, curBlockBytes.Length);

            // reference frame
            Bitmap reference = new Bitmap(imgs[imgIndex - 1]); // 前一張
            BitmapData referenceData = reference.LockBits(new Rectangle(0, 0, reference.Width, reference.Height), ImageLockMode.ReadOnly, reference.PixelFormat);
            byte[] referenceBytes = new byte[referenceData.Stride * referenceData.Height];
            Marshal.Copy(referenceData.Scan0, referenceBytes, 0, referenceBytes.Length);

            // candidate block
            Bitmap candBlock = new Bitmap(8, 8); // block size = 8 x 8
            BitmapData candBlockData = candBlock.LockBits(new Rectangle(0, 0, candBlock.Width, candBlock.Height), ImageLockMode.ReadWrite, candBlock.PixelFormat);
            byte[] candBlockBytes = new byte[candBlockData.Stride * candBlockData.Height];
            Marshal.Copy(candBlockData.Scan0, candBlockBytes, 0, candBlockBytes.Length);

            int height = current.Height;
            int width = current.Width;
            int totalBlocks = (int)((height / 8.0) * (width / 8.0));
            int curIdx, curBlockIdx, tmpX, tmpY, curX = 0, curY = 0;
            int searchX, searchY, refIdx, candidateX = 0, candidateY = 0;
            double sum;
            double diff, minDiff;
            bool showBlock = checkBox.Checked;  // 是否要畫出block

            // 
            MyEvent ShowRec = new MyEvent();
            MyEvent DrawRec = new MyEvent();
            // 
            ShowRec.ShowBlockEvent += new MyEvent.ShowBlockDelegate(ShowBlock);
            DrawRec.DrawBlockEvent += new MyEvent.DrawBlockDelegate(DrawBlock);

            // 掃描整張current frame(blocks不重疊)
            for (int i = 0; i < totalBlocks; ++i)
            {
                Array.Clear(curBlockBytes, 0, curBlockBytes.Length);
                // 備份current block左上角座標
                tmpX = curX;
                tmpY = curY;
                curBlockIdx = 0;
                // 取出current block
                for (int y = 0; y < 8; ++y)
                {
                    curX = tmpX;
                    for (int x = 0; x < 8; ++x)
                    {
                        curIdx = 4 * (curY * width + curX);
                        curBlockBytes[curBlockIdx] = currentBytes[curIdx];          // R
                        curBlockBytes[curBlockIdx + 1] = currentBytes[curIdx + 1];  // G
                        curBlockBytes[curBlockIdx + 2] = currentBytes[curIdx + 2];  // B
                        curBlockBytes[curBlockIdx + 3] = 255;                       // A (255 : 不透明)
                        curBlockIdx += 4;
                        ++curX;
                    }
                    ++curY;
                }
                if (showBlock)
                {
                    // 將current block放大顯示在PictureBox
                    ShowRec.RaiseShowBlock(curBlockBytes, blockPicBox);
                    // 在current frame畫出current block的邊框
                    DrawRec.RaiseDrawBlock(tmpX, tmpY, curPicBox);
                }

                // 掃描整張reference frame(blocks重疊)
                minDiff = double.MaxValue;
                ////////////////////////////////////////////////// -7
                for (int y = 0; y < height - 8; ++y)
                {
                    for (int x = 0; x < width - 8; ++x)
                    {
                        // 備份search window左上角座標
                        searchX = x;
                        searchY = y;
                        // 計算相似度
                        sum = 0;
                        curBlockIdx = 0;
                        for (int y1 = 0; y1 < 8; ++y1)
                        {
                            searchX = x;
                            for (int x1 = 0; x1 < 8; ++x1)
                            {
                                refIdx = 4 * (searchY * width + searchX);
                                candBlockBytes[curBlockIdx] = referenceBytes[refIdx];           // R
                                candBlockBytes[curBlockIdx + 1] = referenceBytes[refIdx + 1];   // G
                                candBlockBytes[curBlockIdx + 2] = referenceBytes[refIdx + 2];   // B
                                candBlockBytes[curBlockIdx + 3] = 255;                          // A (255 : 不透明)
                                // 灰階的rgb皆相同, 取一個通道計算即可
                                sum += Math.Pow(curBlockBytes[curBlockIdx] - referenceBytes[refIdx], 2);
                                curBlockIdx += 4;
                                ++searchX;
                            }
                            ++searchY;
                        }
                        diff = sum / 64.0;
                        // 記錄最相似(差異最小)的座標
                        if (diff < minDiff)
                        {
                            minDiff = diff;
                            candidateX = x;
                            candidateY = y;
                        }

                        if (showBlock)
                        {
                            // 將candidate block放大顯示在PictureBox
                            ShowRec.RaiseShowBlock(candBlockBytes, searchPicBox);
                            // 在reference frame畫出search window的邊框
                            DrawRec.RaiseDrawBlock(x, y, refPicBox);
                        }
                    }
                }
                // 計算位移
                int vectorX = candidateX - tmpX;
                int vectorY = candidateY - tmpY;
                // 儲存位移向量
                vectors.Add(new List<int>() { vectorX, vectorY });

                // 回復與更新座標
                curY = tmpY;
                if (curX == width)
                {
                    curX = 0;
                    curY += 8;
                }
            }
            // 將資料複製到圖像物件
            // Marshal.Copy(src, startIndex, dstPtr, length)
            // Scan0 - 影像資料的起始位置
            Marshal.Copy(curBlockBytes, 0, curBlockData.Scan0, curBlockBytes.Length);
            // 解除鎖定記憶體
            current.UnlockBits(currentData);
            curBlock.UnlockBits(curBlockData);
            reference.UnlockBits(referenceData);
            current.Dispose();
            curBlock.Dispose();
            reference.Dispose();

            // 將位移向量寫入檔案
            string fileName;
            if (video == 1)
            {
                fileName = "vectors1.txt";
            }
            else
            {
                fileName = "vectors2.txt";
            }
            //using (TextWriter tw = new StreamWriter("vectors.txt"))
            using (TextWriter tw = new StreamWriter(fileName))
            {
                foreach (List<int> l in vectors)
                {
                    tw.WriteLine(l[0]);
                    tw.WriteLine(l[1]);
                }
            }
            Debug.Print("Frame" + imgIndex.ToString() + " Compression Done!");
        }

        /*
         * 將current block放大顯示在PictureBox
         * @srcBytes    :   current block資料陣列
         * @blockPicBox :   current block之PictureBox
         */
        public void ShowBlock(byte[] srcBytes, PictureBox blockPicBox)
        {
            // 原本速度太快會遇到物件正在使用的Exception
            // 因此每次暫停一小段時間
            //Thread.Sleep(1);
            Bitmap block;
            while (true)
            {
                try
                {
                    block = new Bitmap(8, 8);
                    // 成功後離開迴圈
                    break;
                }
                // 遇到物件正在使用的Exception
                catch (InvalidOperationException)
                {
                    // 等待一段時間後繼續嘗試
                    Thread.Sleep(17);
                    continue;
                }
            }
            BitmapData data = block.LockBits(new Rectangle(0, 0, block.Width, block.Height), ImageLockMode.ReadWrite, block.PixelFormat);
            // 將資料複製到圖像物件
            Marshal.Copy(srcBytes, 0, data.Scan0, srcBytes.Length);
            // 解除鎖定記憶體
            block.UnlockBits(data);
            while (true)
            {
                try
                {
                    blockPicBox.Image = block;
                    // 成功後離開迴圈
                    break;
                }
                // 遇到物件正在使用的Exception
                catch (InvalidOperationException)
                {
                    // 等待一段時間後繼續嘗試
                    Thread.Sleep(17);
                    continue;
                }
            }
        }

        /*
         * 在frame的PictureBox上框出block
         * @blockX  :   block起始x座標
         * @blockY  :   block起始y座標
         * @picBox  :   frame的PictureBox
         */
        public void DrawBlock(int blockX, int blockY, PictureBox picBox)
        {
            int width;
            int height;
            while (true)
            {
                try
                {
                    width = GetWidth();
                    // 成功後離開迴圈
                    break;
                }
                // 遇到物件正在使用的Exception
                catch (InvalidOperationException)
                {
                    // 等待一段時間後繼續嘗試
                    Thread.Sleep(17);
                    continue;
                }
            }
            while (true)
            {
                try
                {
                    height = GetHeight();
                    // 成功後離開迴圈
                    break;
                }
                // 遇到物件正在使用的Exception
                catch (InvalidOperationException)
                {
                    // 等待一段時間後繼續嘗試
                    Thread.Sleep(17);
                    continue;
                }
            }
            // index格式無法畫圖, 需轉換格式
            Bitmap bmp;
            Thread.Sleep(100);
            while (true)
            {
                try
                {
                    bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                    // 成功後離開迴圈
                    break;
                }
                // 遇到物件正在使用的Exception
                catch (InvalidOperationException)
                {
                    // 等待一段時間後繼續嘗試
                    Thread.Sleep(17);
                    continue;
                }
            }
            Graphics g;
            while (true)
            {
                try
                {
                    g = Graphics.FromImage(bmp);
                    // 成功後離開迴圈
                    break;
                }
                // 遇到物件正在使用的Exception
                catch (InvalidOperationException)
                {
                    // 等待一段時間後繼續嘗試
                    Thread.Sleep(17);
                    continue;
                }
            }
            using (Pen p = new Pen(Color.Red))
            {
                while (true)
                {
                    try
                    {
                        g.DrawRectangle(p, new Rectangle(blockX, blockY, 8, 8));
                        // 成功後離開迴圈
                        break;
                    }
                    // 遇到物件正在使用的Exception
                    catch (InvalidOperationException)
                    {
                        // 等待一段時間後繼續嘗試
                        Thread.Sleep(17);
                        continue;
                    }
                }
            } // p will be disposed at this line
            g.Dispose();
            /*using (Graphics g = Graphics.FromImage(bmp))
            {
                using (Pen p = new Pen(Color.Red))
                {
                    while(true)
                    {
                        try
                        {
                            g.DrawRectangle(p, new Rectangle(blockX, blockY, 8, 8));
                            // 成功後離開迴圈
                            break;
                        }
                        // 遇到物件正在使用的Exception
                        catch (InvalidOperationException)
                        {
                            // 等待一段時間後繼續嘗試
                            Thread.Sleep(17);
                            continue;
                        }
                    }
                } // p will be disposed at this line
            } // g will be disposed at this line*/
            while (true)
            {
                try
                {
                    picBox.Image = bmp;
                    // 成功後離開迴圈
                    break;
                }
                // 遇到物件正在使用的Exception
                catch (InvalidOperationException)
                {
                    // 等待一段時間後繼續嘗試
                    Thread.Sleep(17);
                    continue;
                }
            }
        }

        /*
         * Block Based Motion Compensation Decompression
         * @imgIndex    :   the number of the frame
         * @vectorBmp   :   motion vector圖
         */
        public void MotionDEC(int imgIndex, Bitmap vectorBmp)
        {
            int height = GetHeight();
            int width = GetWidth();
            int totalBlocks = (int)((height / 8.0) * (width / 8.0));

            // decompression frame
            Bitmap decFrame = new Bitmap(width, height);
            BitmapData decData = decFrame.LockBits(new Rectangle(0, 0, decFrame.Width, decFrame.Height), ImageLockMode.ReadWrite, decFrame.PixelFormat);
            byte[] decBytes = new byte[decData.Stride * decData.Height];
            Marshal.Copy(decData.Scan0, decBytes, 0, decBytes.Length);

            // reference frame
            Bitmap reference = new Bitmap(newImgs[imgIndex - 1]); // 解壓後的前一張
            BitmapData referenceData = reference.LockBits(new Rectangle(0, 0, reference.Width, reference.Height), ImageLockMode.ReadOnly, reference.PixelFormat);
            byte[] referenceBytes = new byte[referenceData.Stride * referenceData.Height];
            Marshal.Copy(referenceData.Scan0, referenceBytes, 0, referenceBytes.Length);

            // 建立此frame的motion vector圖
            vectorBmp = new Bitmap(256, 256, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(vectorBmp))
            {
                // 整張刷白
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, 256, 256));
                /*using (Pen p = new Pen(Color.Black, 1))
                {
                    // 畫block的邊框
                    for(int i = 0; i < 256; i += 8)
                    {
                        // 直線
                        g.DrawLine(p, i, 0, i, 255);
                        g.DrawLine(p, i + 7, 0, i + 7, 255);
                        // 橫線
                        g.DrawLine(p, 0, i, 255, i);
                        g.DrawLine(p, 0, i + 7, 255, i + 7);
                    }
                }*/
            }

            int curIdx, refIdx, tmpX, tmpY, curX = 0, curY = 0;
            int dstX, dstY, backupX, vecIdx;

            // 掃描整張current frame(blocks不重疊)
            for (int i = 0; i < totalBlocks; ++i)
            {
                // 取出紀錄中的位移向量, 計算目標block起始位置
                vecIdx = GetVecIdx();
                dstX = curX + vectors[vecIdx][0];
                dstY = curY + vectors[vecIdx][1];
                backupX = dstX;
                // 備份current block座標
                tmpX = curX;
                tmpY = curY;

                // 畫motion vector
                DrawVector(vectorBmp, curX, curY, dstX, dstY);

                // current block位置
                for (int y = 0; y < 8; ++y)
                {
                    dstX = backupX;
                    curX = tmpX;
                    for (int x = 0; x < 8; ++x)
                    {
                        refIdx = 4 * (dstY * width + dstX);
                        curIdx = 4 * (curY * width + curX);
                        decBytes[curIdx] = referenceBytes[refIdx];          // R
                        decBytes[curIdx + 1] = referenceBytes[refIdx + 1];  // G
                        decBytes[curIdx + 2] = referenceBytes[refIdx + 2];  // B
                        decBytes[curIdx + 3] = 255;                         // A (255 : 不透明)
                        ++dstX;
                        ++curX;
                    }
                    ++dstY;
                    ++curY;
                }

                // 回復與更新座標
                curY = tmpY;
                if (curX == width)
                {
                    curX = 0;
                    curY += 8;
                }
                // 位移向量List的位置增加1
                IncVecIdx();
            }
            // 保存位移向量圖
            vecImg.Add(vectorBmp);

            // 將資料複製到圖像物件
            // Marshal.Copy(src, startIndex, dstPtr, length)
            // Scan0 - 影像資料的起始位置
            Marshal.Copy(decBytes, 0, decData.Scan0, decBytes.Length);
            // 解除鎖定記憶體
            decFrame.UnlockBits(decData);
            reference.UnlockBits(referenceData);

            // 解壓縮後存入新的list
            newImgs.Add(decFrame);
            Debug.Print("Frame" + imgIndex.ToString() + " Decompression Done!");
        }

        /*
         * 畫出一張frame的motion vectors
         * @img         :   motion vector圖
         * @origX       :   block原始x座標
         * @origY       :   block原始y座標
         * @dstX        :   block目標x座標
         * @dstY        :   block目標y座標
         */
        public void DrawVector(Bitmap img, int origX, int origY, int dstX, int dstY)
        {
            // 影片2的大小為512*512, vector圖為256*256
            // 若為影片2, 須將座標除以2
            if (video == 2)
            {
                origX /= 2;
                origY /= 2;
                dstX /= 2;
                dstY /= 2;
            }
            // block center
            float origMidX = (origX + origX + 7) / 2.0F;
            float origMidY = (origY + origY + 7) / 2.0F;
            float dstMidX = (dstX + dstX + 7) / 2.0F;
            float dstMidY = (dstY + dstY + 7) / 2.0F;

            using (Graphics g = Graphics.FromImage(img))
            {
                using (Pen p = new Pen(Color.Red, 1))
                {
                    // 設定線條外觀
                    p.StartCap = LineCap.RoundAnchor;   // 起始為圓點
                    p.EndCap = LineCap.ArrowAnchor;     // 結束為箭頭
                    // 畫線
                    g.DrawLine(p, origMidX, origMidY, dstMidX, dstMidY);
                }
            }
        }

        /*
         * Three Step Search (TSS) Compression
         * @imgIndex    :   the number of the frame
         * @blockPicBox :   current block之PictureBox
         * @searchPicBox:   candidate block之PictureBox
         * @curPicBox   :   current frame之PictureBox
         * @refPicBox   :   reference frame之PictureBox
         * @checkBox    :   決定是否畫出block
         */
        public void TSSCO(int imgIndex, PictureBox blockPicBox, PictureBox searchPicBox, PictureBox curPicBox, PictureBox refPicBox, CheckBox checkBox)
        {
            // current frame
            Bitmap current = new Bitmap(imgs[imgIndex]);  // 預設為PixelFormat.Format32bppArgb
            BitmapData currentData = current.LockBits(new Rectangle(0, 0, current.Width, current.Height), ImageLockMode.ReadOnly, current.PixelFormat);
            byte[] currentBytes = new byte[currentData.Stride * currentData.Height];
            Marshal.Copy(currentData.Scan0, currentBytes, 0, currentBytes.Length);

            // current block
            Bitmap curBlock = new Bitmap(8, 8); // block size = 8 x 8
            BitmapData curBlockData = curBlock.LockBits(new Rectangle(0, 0, curBlock.Width, curBlock.Height), ImageLockMode.ReadWrite, curBlock.PixelFormat);
            byte[] curBlockBytes = new byte[curBlockData.Stride * curBlockData.Height];
            Marshal.Copy(curBlockData.Scan0, curBlockBytes, 0, curBlockBytes.Length);

            // reference frame
            Bitmap reference = new Bitmap(imgs[imgIndex - 1]); // 前一張
            BitmapData referenceData = reference.LockBits(new Rectangle(0, 0, reference.Width, reference.Height), ImageLockMode.ReadOnly, reference.PixelFormat);
            byte[] referenceBytes = new byte[referenceData.Stride * referenceData.Height];
            Marshal.Copy(referenceData.Scan0, referenceBytes, 0, referenceBytes.Length);

            // candidate block
            Bitmap candBlock = new Bitmap(8, 8); // block size = 8 x 8
            BitmapData candBlockData = candBlock.LockBits(new Rectangle(0, 0, candBlock.Width, candBlock.Height), ImageLockMode.ReadWrite, candBlock.PixelFormat);
            byte[] candBlockBytes = new byte[candBlockData.Stride * candBlockData.Height];
            Marshal.Copy(candBlockData.Scan0, candBlockBytes, 0, candBlockBytes.Length);

            int height = current.Height;
            int width = current.Width;
            int totalBlocks = (int)((height / 8.0) * (width / 8.0));
            int curIdx, curBlockIdx, tmpX, tmpY, curX = 0, curY = 0;
            int centerX, centerY, searchX, searchY, refIdx, candidateX = 0, candidateY = 0;
            double sum;
            double diff, minDiff;
            bool showBlock = checkBox.Checked;  // 是否要畫出block

            // 
            MyEvent ShowRec = new MyEvent();
            MyEvent DrawRec = new MyEvent();
            // 
            ShowRec.ShowBlockEvent += new MyEvent.ShowBlockDelegate(ShowBlock);
            DrawRec.DrawBlockEvent += new MyEvent.DrawBlockDelegate(DrawBlock);

            // 掃描整張current frame(blocks不重疊)
            for (int i = 0; i < totalBlocks; ++i)
            {
                Array.Clear(curBlockBytes, 0, curBlockBytes.Length);
                // 備份current block左上角座標
                tmpX = curX;
                tmpY = curY;
                curBlockIdx = 0;
                // 取出current block
                for (int y = 0; y < 8; ++y)
                {
                    curX = tmpX;
                    for (int x = 0; x < 8; ++x)
                    {
                        curIdx = 4 * (curY * width + curX);
                        curBlockBytes[curBlockIdx] = currentBytes[curIdx];          // R
                        curBlockBytes[curBlockIdx + 1] = currentBytes[curIdx + 1];  // G
                        curBlockBytes[curBlockIdx + 2] = currentBytes[curIdx + 2];  // B
                        curBlockBytes[curBlockIdx + 3] = 255;                       // A (255 : 不透明)
                        curBlockIdx += 4;
                        ++curX;
                    }
                    ++curY;
                }
                if (showBlock)
                {
                    // 將current block放大顯示在PictureBox
                    ShowRec.RaiseShowBlock(curBlockBytes, blockPicBox);
                    // 在current frame畫出current block的邊框
                    DrawRec.RaiseDrawBlock(tmpX, tmpY, curPicBox);
                }

                // 從reference frame的對應位置開始找 (Three Step Search)
                minDiff = double.MaxValue;
                centerX = tmpX;
                centerY = tmpY;
                for (int s = 3; s > 0; --s)  // step size = 3
                {
                    // 左上
                    // 備份search window左上角座標
                    searchX = centerX - s;
                    searchY = centerY - s;
                    if(searchX >= 0 && searchY >= 0)
                    {
                        if (showBlock)
                        {
                            // 將candidate block放大顯示在PictureBox
                            ShowRec.RaiseShowBlock(candBlockBytes, searchPicBox);
                            // 在reference frame畫出search window的邊框
                            DrawRec.RaiseDrawBlock(searchX, searchY, refPicBox);
                        }

                        // 計算相似度
                        sum = 0;
                        curBlockIdx = 0;
                        for (int y = 0; y < 8; ++y)
                        {
                            searchX = tmpX;
                            for (int x = 0; x < 8; ++x)
                            {
                                refIdx = 4 * (searchY * width + searchX);
                                candBlockBytes[curBlockIdx] = referenceBytes[refIdx];           // R
                                candBlockBytes[curBlockIdx + 1] = referenceBytes[refIdx + 1];   // G
                                candBlockBytes[curBlockIdx + 2] = referenceBytes[refIdx + 2];   // B
                                candBlockBytes[curBlockIdx + 3] = 255;                          // A (255 : 不透明)
                                                                                                // 灰階的rgb皆相同, 取一個通道計算即可
                                sum += Math.Pow(curBlockBytes[curBlockIdx] - referenceBytes[refIdx], 2);
                                curBlockIdx += 4;
                                ++searchX;
                            }
                            ++searchY;
                        }
                        diff = sum / 64.0;
                        // 記錄最相似(差異最小)的座標
                        if (diff < minDiff)
                        {
                            minDiff = diff;
                            candidateX = centerX - s;
                            candidateY = centerY - s;
                        }
                    }

                    // 左
                    // 備份search window左上角座標
                    searchX = centerX - s;
                    searchY = centerY;
                    if (searchX >= 0)
                    {
                        if (showBlock)
                        {
                            // 將candidate block放大顯示在PictureBox
                            ShowRec.RaiseShowBlock(candBlockBytes, searchPicBox);
                            // 在reference frame畫出search window的邊框
                            DrawRec.RaiseDrawBlock(searchX, searchY, refPicBox);
                        }

                        // 計算相似度
                        sum = 0;
                        curBlockIdx = 0;
                        for (int y = 0; y < 8; ++y)
                        {
                            searchX = tmpX;
                            for (int x = 0; x < 8; ++x)
                            {
                                refIdx = 4 * (searchY * width + searchX);
                                candBlockBytes[curBlockIdx] = referenceBytes[refIdx];           // R
                                candBlockBytes[curBlockIdx + 1] = referenceBytes[refIdx + 1];   // G
                                candBlockBytes[curBlockIdx + 2] = referenceBytes[refIdx + 2];   // B
                                candBlockBytes[curBlockIdx + 3] = 255;                          // A (255 : 不透明)
                                                                                                // 灰階的rgb皆相同, 取一個通道計算即可
                                sum += Math.Pow(curBlockBytes[curBlockIdx] - referenceBytes[refIdx], 2);
                                curBlockIdx += 4;
                                ++searchX;
                            }
                            ++searchY;
                        }
                        diff = sum / 64.0;
                        // 記錄最相似(差異最小)的座標
                        if (diff < minDiff)
                        {
                            minDiff = diff;
                            candidateX = centerX - s;
                            candidateY = centerY;
                        }
                    }

                    // 左下
                    searchX = centerX - s;
                    searchY = centerY + s;
                    if (searchX >= 0 && (searchY + 7) < height)
                    {
                        if (showBlock)
                        {
                            // 將candidate block放大顯示在PictureBox
                            ShowRec.RaiseShowBlock(candBlockBytes, searchPicBox);
                            // 在reference frame畫出search window的邊框
                            DrawRec.RaiseDrawBlock(searchX, searchY, refPicBox);
                        }

                        // 計算相似度
                        sum = 0;
                        curBlockIdx = 0;
                        for (int y = 0; y < 8; ++y)
                        {
                            searchX = tmpX;
                            for (int x = 0; x < 8; ++x)
                            {
                                refIdx = 4 * (searchY * width + searchX);
                                candBlockBytes[curBlockIdx] = referenceBytes[refIdx];           // R
                                candBlockBytes[curBlockIdx + 1] = referenceBytes[refIdx + 1];   // G
                                candBlockBytes[curBlockIdx + 2] = referenceBytes[refIdx + 2];   // B
                                candBlockBytes[curBlockIdx + 3] = 255;                          // A (255 : 不透明)
                                                                                                // 灰階的rgb皆相同, 取一個通道計算即可
                                sum += Math.Pow(curBlockBytes[curBlockIdx] - referenceBytes[refIdx], 2);
                                curBlockIdx += 4;
                                ++searchX;
                            }
                            ++searchY;
                        }
                        diff = sum / 64.0;
                        // 記錄最相似(差異最小)的座標
                        if (diff < minDiff)
                        {
                            minDiff = diff;
                            candidateX = centerX - s;
                            candidateY = centerY + s;
                        }
                    }

                    // 中上
                    searchX = centerX;
                    searchY = centerY - s;
                    if (searchY >= 0)
                    {
                        if (showBlock)
                        {
                            // 將candidate block放大顯示在PictureBox
                            ShowRec.RaiseShowBlock(candBlockBytes, searchPicBox);
                            // 在reference frame畫出search window的邊框
                            DrawRec.RaiseDrawBlock(searchX, searchY, refPicBox);
                        }

                        // 計算相似度
                        sum = 0;
                        curBlockIdx = 0;
                        for (int y = 0; y < 8; ++y)
                        {
                            searchX = tmpX;
                            for (int x = 0; x < 8; ++x)
                            {
                                refIdx = 4 * (searchY * width + searchX);
                                candBlockBytes[curBlockIdx] = referenceBytes[refIdx];           // R
                                candBlockBytes[curBlockIdx + 1] = referenceBytes[refIdx + 1];   // G
                                candBlockBytes[curBlockIdx + 2] = referenceBytes[refIdx + 2];   // B
                                candBlockBytes[curBlockIdx + 3] = 255;                          // A (255 : 不透明)
                                                                                                // 灰階的rgb皆相同, 取一個通道計算即可
                                sum += Math.Pow(curBlockBytes[curBlockIdx] - referenceBytes[refIdx], 2);
                                curBlockIdx += 4;
                                ++searchX;
                            }
                            ++searchY;
                        }
                        diff = sum / 64.0;
                        // 記錄最相似(差異最小)的座標
                        if (diff < minDiff)
                        {
                            minDiff = diff;
                            candidateX = centerX;
                            candidateY = centerY - s;
                        }
                    }

                    // 中
                    // 備份search window左上角座標
                    searchX = centerX;
                    searchY = centerY;
                    if (showBlock)
                    {
                        // 將candidate block放大顯示在PictureBox
                        ShowRec.RaiseShowBlock(candBlockBytes, searchPicBox);
                        // 在reference frame畫出search window的邊框
                        DrawRec.RaiseDrawBlock(searchX, searchY, refPicBox);
                    }
                    // 計算相似度
                    sum = 0;
                    curBlockIdx = 0;
                    for (int y = 0; y < 8; ++y)
                    {
                        searchX = tmpX;
                        for (int x = 0; x < 8; ++x)
                        {
                            refIdx = 4 * (searchY * width + searchX);
                            candBlockBytes[curBlockIdx] = referenceBytes[refIdx];           // R
                            candBlockBytes[curBlockIdx + 1] = referenceBytes[refIdx + 1];   // G
                            candBlockBytes[curBlockIdx + 2] = referenceBytes[refIdx + 2];   // B
                            candBlockBytes[curBlockIdx + 3] = 255;                          // A (255 : 不透明)
                                                                                            // 灰階的rgb皆相同, 取一個通道計算即可
                            sum += Math.Pow(curBlockBytes[curBlockIdx] - referenceBytes[refIdx], 2);
                            curBlockIdx += 4;
                            ++searchX;
                        }
                        ++searchY;
                    }
                    diff = sum / 64.0;
                    // 記錄最相似(差異最小)的座標
                    if (diff < minDiff)
                    {
                        minDiff = diff;
                        candidateX = centerX;
                        candidateY = centerY;
                    }

                    // 中下
                    searchX = centerX;
                    searchY = centerY + s;
                    if ((searchY + 7) < height)
                    {
                        if (showBlock)
                        {
                            // 將candidate block放大顯示在PictureBox
                            ShowRec.RaiseShowBlock(candBlockBytes, searchPicBox);
                            // 在reference frame畫出search window的邊框
                            DrawRec.RaiseDrawBlock(searchX, searchY, refPicBox);
                        }

                        // 計算相似度
                        sum = 0;
                        curBlockIdx = 0;
                        for (int y = 0; y < 8; ++y)
                        {
                            searchX = tmpX;
                            for (int x = 0; x < 8; ++x)
                            {
                                refIdx = 4 * (searchY * width + searchX);
                                candBlockBytes[curBlockIdx] = referenceBytes[refIdx];           // R
                                candBlockBytes[curBlockIdx + 1] = referenceBytes[refIdx + 1];   // G
                                candBlockBytes[curBlockIdx + 2] = referenceBytes[refIdx + 2];   // B
                                candBlockBytes[curBlockIdx + 3] = 255;                          // A (255 : 不透明)
                                                                                                // 灰階的rgb皆相同, 取一個通道計算即可
                                sum += Math.Pow(curBlockBytes[curBlockIdx] - referenceBytes[refIdx], 2);
                                curBlockIdx += 4;
                                ++searchX;
                            }
                            ++searchY;
                        }
                        diff = sum / 64.0;
                        // 記錄最相似(差異最小)的座標
                        if (diff < minDiff)
                        {
                            minDiff = diff;
                            candidateX = centerX;
                            candidateY = centerY + s;
                        }
                    }

                    // 右上
                    searchX = centerX + s;
                    searchY = centerY - s;
                    if ((searchX + 7) < width && searchY >= 0)
                    {
                        if (showBlock)
                        {
                            // 將candidate block放大顯示在PictureBox
                            ShowRec.RaiseShowBlock(candBlockBytes, searchPicBox);
                            // 在reference frame畫出search window的邊框
                            DrawRec.RaiseDrawBlock(searchX, searchY, refPicBox);
                        }

                        // 計算相似度
                        sum = 0;
                        curBlockIdx = 0;
                        for (int y = 0; y < 8; ++y)
                        {
                            searchX = tmpX;
                            for (int x = 0; x < 8; ++x)
                            {
                                refIdx = 4 * (searchY * width + searchX);
                                candBlockBytes[curBlockIdx] = referenceBytes[refIdx];           // R
                                candBlockBytes[curBlockIdx + 1] = referenceBytes[refIdx + 1];   // G
                                candBlockBytes[curBlockIdx + 2] = referenceBytes[refIdx + 2];   // B
                                candBlockBytes[curBlockIdx + 3] = 255;                          // A (255 : 不透明)
                                                                                                // 灰階的rgb皆相同, 取一個通道計算即可
                                sum += Math.Pow(curBlockBytes[curBlockIdx] - referenceBytes[refIdx], 2);
                                curBlockIdx += 4;
                                ++searchX;
                            }
                            ++searchY;
                        }
                        diff = sum / 64.0;
                        // 記錄最相似(差異最小)的座標
                        if (diff < minDiff)
                        {
                            minDiff = diff;
                            candidateX = centerX + s;
                            candidateY = centerY - s;
                        }
                    }

                    // 右
                    searchX = centerX + s;
                    searchY = centerY;
                    if ((searchX + 7) < width)
                    {
                        if (showBlock)
                        {
                            // 將candidate block放大顯示在PictureBox
                            ShowRec.RaiseShowBlock(candBlockBytes, searchPicBox);
                            // 在reference frame畫出search window的邊框
                            DrawRec.RaiseDrawBlock(searchX, searchY, refPicBox);
                        }

                        // 計算相似度
                        sum = 0;
                        curBlockIdx = 0;
                        for (int y = 0; y < 8; ++y)
                        {
                            searchX = tmpX;
                            for (int x = 0; x < 8; ++x)
                            {
                                refIdx = 4 * (searchY * width + searchX);
                                candBlockBytes[curBlockIdx] = referenceBytes[refIdx];           // R
                                candBlockBytes[curBlockIdx + 1] = referenceBytes[refIdx + 1];   // G
                                candBlockBytes[curBlockIdx + 2] = referenceBytes[refIdx + 2];   // B
                                candBlockBytes[curBlockIdx + 3] = 255;                          // A (255 : 不透明)
                                                                                                // 灰階的rgb皆相同, 取一個通道計算即可
                                sum += Math.Pow(curBlockBytes[curBlockIdx] - referenceBytes[refIdx], 2);
                                curBlockIdx += 4;
                                ++searchX;
                            }
                            ++searchY;
                        }
                        diff = sum / 64.0;
                        // 記錄最相似(差異最小)的座標
                        if (diff < minDiff)
                        {
                            minDiff = diff;
                            candidateX = centerX + s;
                            candidateY = centerY;
                        }
                    }

                    // 右下
                    searchX = centerX + s;
                    searchY = centerY + s;
                    if ((searchX + 7) < width && (searchY + 7) < height)
                    {
                        if (showBlock)
                        {
                            // 將candidate block放大顯示在PictureBox
                            ShowRec.RaiseShowBlock(candBlockBytes, searchPicBox);
                            // 在reference frame畫出search window的邊框
                            DrawRec.RaiseDrawBlock(searchX, searchY, refPicBox);
                        }

                        // 計算相似度
                        sum = 0;
                        curBlockIdx = 0;
                        for (int y = 0; y < 8; ++y)
                        {
                            searchX = tmpX;
                            for (int x = 0; x < 8; ++x)
                            {
                                refIdx = 4 * (searchY * width + searchX);
                                candBlockBytes[curBlockIdx] = referenceBytes[refIdx];           // R
                                candBlockBytes[curBlockIdx + 1] = referenceBytes[refIdx + 1];   // G
                                candBlockBytes[curBlockIdx + 2] = referenceBytes[refIdx + 2];   // B
                                candBlockBytes[curBlockIdx + 3] = 255;                          // A (255 : 不透明)
                                                                                                // 灰階的rgb皆相同, 取一個通道計算即可
                                sum += Math.Pow(curBlockBytes[curBlockIdx] - referenceBytes[refIdx], 2);
                                curBlockIdx += 4;
                                ++searchX;
                            }
                            ++searchY;
                        }
                        diff = sum / 64.0;
                        // 記錄最相似(差異最小)的座標
                        if (diff < minDiff)
                        {
                            minDiff = diff;
                            candidateX = centerX + s;
                            candidateY = centerY + s;
                        }
                    }

                    // 中心點改為失真最小的點
                    centerX = candidateX;
                    centerY = candidateY;
                }

                // 計算位移
                int vectorX = candidateX - tmpX;
                int vectorY = candidateY - tmpY;
                // 儲存位移向量
                vectors.Add(new List<int>() { vectorX, vectorY });

                // 回復與更新座標
                curY = tmpY;
                if (curX == width)
                {
                    curX = 0;
                    curY += 8;
                }
            }
            // 將資料複製到圖像物件
            // Marshal.Copy(src, startIndex, dstPtr, length)
            // Scan0 - 影像資料的起始位置
            Marshal.Copy(curBlockBytes, 0, curBlockData.Scan0, curBlockBytes.Length);
            // 解除鎖定記憶體
            current.UnlockBits(currentData);
            curBlock.UnlockBits(curBlockData);
            reference.UnlockBits(referenceData);
            current.Dispose();
            curBlock.Dispose();
            reference.Dispose();

            // 將位移向量寫入檔案
            string fileName;
            if (video == 1)
            {
                fileName = "tss1.txt";
            }
            else
            {
                fileName = "tss2.txt";
            }
            //using (TextWriter tw = new StreamWriter("tss.txt"))
            using (TextWriter tw = new StreamWriter(fileName))
            {
                foreach (List<int> l in vectors)
                {
                    tw.WriteLine(l[0]);
                    tw.WriteLine(l[1]);
                }
            }
            Debug.Print("Frame" + imgIndex.ToString() + " Compression Done!");
        }

        /*
         * Three Step Search (TSS) Decompression
         * @imgIndex    :   the number of the frame
         * @vectorBmp   :   motion vector圖
         */
        public void TSSDEC(int imgIndex, Bitmap vectorBmp)
        {
            int height = GetHeight();
            int width = GetWidth();
            int totalBlocks = (int)((height / 8.0) * (width / 8.0));

            // decompression frame
            Bitmap decFrame = new Bitmap(width, height);
            BitmapData decData = decFrame.LockBits(new Rectangle(0, 0, decFrame.Width, decFrame.Height), ImageLockMode.ReadWrite, decFrame.PixelFormat);
            byte[] decBytes = new byte[decData.Stride * decData.Height];
            Marshal.Copy(decData.Scan0, decBytes, 0, decBytes.Length);

            // newImgs?????????????????????????????????????????????????????????????
            // reference frame
            Bitmap reference = new Bitmap(imgs[imgIndex - 1]); // 原本的前一張
            BitmapData referenceData = reference.LockBits(new Rectangle(0, 0, reference.Width, reference.Height), ImageLockMode.ReadOnly, reference.PixelFormat);
            byte[] referenceBytes = new byte[referenceData.Stride * referenceData.Height];
            Marshal.Copy(referenceData.Scan0, referenceBytes, 0, referenceBytes.Length);

            // 建立此frame的motion vector圖
            vectorBmp = new Bitmap(256, 256, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(vectorBmp))
            {
                // 整張刷白
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, 256, 256));
                /*using (Pen p = new Pen(Color.Black, 1))
                {
                    // 畫block的邊框
                    for(int i = 0; i < 256; i += 8)
                    {
                        // 直線
                        g.DrawLine(p, i, 0, i, 255);
                        g.DrawLine(p, i + 7, 0, i + 7, 255);
                        // 橫線
                        g.DrawLine(p, 0, i, 255, i);
                        g.DrawLine(p, 0, i + 7, 255, i + 7);
                    }
                }*/
            }

            int curIdx, refIdx, tmpX, tmpY, curX = 0, curY = 0;
            int dstX, dstY, backupX, vecIdx;

            // 掃描整張current frame(blocks不重疊)
            for (int i = 0; i < totalBlocks; ++i)
            {
                // 取出紀錄中的位移向量, 計算目標block起始位置
                vecIdx = GetVecIdx();
                dstX = curX + vectors[vecIdx][0];
                dstY = curY + vectors[vecIdx][1];
                backupX = dstX;
                // 備份current block座標
                tmpX = curX;
                tmpY = curY;

                // 畫motion vector
                DrawVector(vectorBmp, curX, curY, dstX, dstY);

                // current block位置
                for (int y = 0; y < 8; ++y)
                {
                    dstX = backupX;
                    curX = tmpX;
                    for (int x = 0; x < 8; ++x)
                    {
                        refIdx = 4 * (dstY * width + dstX);
                        curIdx = 4 * (curY * width + curX);
                        decBytes[curIdx] = referenceBytes[refIdx];          // R
                        decBytes[curIdx + 1] = referenceBytes[refIdx + 1];  // G
                        decBytes[curIdx + 2] = referenceBytes[refIdx + 2];  // B
                        decBytes[curIdx + 3] = 255;                         // A (255 : 不透明)
                        ++dstX;
                        ++curX;
                    }
                    ++dstY;
                    ++curY;
                }

                // 回復與更新座標
                curY = tmpY;
                if (curX == width)
                {
                    curX = 0;
                    curY += 8;
                }
                // 位移向量List的位置增加1
                IncVecIdx();
            }
            // 保存位移向量圖
            vecImg.Add(vectorBmp);

            // 將資料複製到圖像物件
            // Marshal.Copy(src, startIndex, dstPtr, length)
            // Scan0 - 影像資料的起始位置
            Marshal.Copy(decBytes, 0, decData.Scan0, decBytes.Length);
            // 解除鎖定記憶體
            decFrame.UnlockBits(decData);
            reference.UnlockBits(referenceData);

            // 解壓縮後存入新的list
            newImgs.Add(decFrame);
            Debug.Print("Frame" + imgIndex.ToString() + " Decompression Done!");
        }

        /*
         * Interframe Sub-sampling
         * 保留偶數張(0, 2, 4), 中間缺的用前後張取平均
         * @imgIndex    :   the number of the frame
         */
        public void InterSample(int imgIndex)
        {
            int height = GetHeight();
            int width = GetWidth();

            // 前一張
            Bitmap imgPrev = new Bitmap(newImgs[imgIndex - 1]);
            BitmapData imgPrevData = imgPrev.LockBits(new Rectangle(0, 0, imgPrev.Width, imgPrev.Height), ImageLockMode.ReadOnly, imgPrev.PixelFormat);
            byte[] imgPrevBytes = new byte[imgPrevData.Stride * imgPrevData.Height];
            Marshal.Copy(imgPrevData.Scan0, imgPrevBytes, 0, imgPrevBytes.Length);

            // 後一張
            Bitmap imgNext = new Bitmap(newImgs[imgIndex + 1]);
            BitmapData imgNextData = imgNext.LockBits(new Rectangle(0, 0, imgNext.Width, imgNext.Height), ImageLockMode.ReadOnly, imgNext.PixelFormat);
            byte[] imgNextBytes = new byte[imgNextData.Stride * imgNextData.Height];
            Marshal.Copy(imgNextData.Scan0, imgNextBytes, 0, imgNextBytes.Length);

            // 壓縮後的圖
            Bitmap comImg = new Bitmap(width, height);
            BitmapData comImgData = comImg.LockBits(new Rectangle(0, 0, comImg.Width, comImg.Height), ImageLockMode.ReadWrite, comImg.PixelFormat);
            byte[] comImgBytes = new byte[comImgData.Stride * comImgData.Height];
            Marshal.Copy(comImgData.Scan0, comImgBytes, 0, comImgBytes.Length);

            int index = 0;
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    index = 4 * (y * width + x);
                    comImgBytes[index] = (byte)((imgPrevBytes[index] + imgNextBytes[index]) / 2.0F);
                    comImgBytes[index + 1] = (byte)((imgPrevBytes[index + 1] + imgNextBytes[index + 1]) / 2.0F);
                    comImgBytes[index + 2] = (byte)((imgPrevBytes[index + 2] + imgNextBytes[index + 2]) / 2.0F);
                    comImgBytes[index + 3] = 255;
                }
            }

            // 將資料複製到圖像物件
            // Marshal.Copy(src, startIndex, dstPtr, length)
            // Scan0 - 影像資料的起始位置
            Marshal.Copy(comImgBytes, 0, comImgData.Scan0, comImgBytes.Length);
            // 解除鎖定記憶體
            imgPrev.UnlockBits(imgPrevData);
            imgNext.UnlockBits(imgNextData);
            comImg.UnlockBits(comImgData);
            // 壓縮後存入新的list
            newImgs[imgIndex] = comImg;
        }
    }
}
