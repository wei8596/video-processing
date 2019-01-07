using System;
using System.Threading;
using System.Windows.Forms;

namespace videoProcess
{
    public class MyEvent
    {
        // 定義delegate(委派)
        public delegate void ShowBlockDelegate(byte[] srcBytes, PictureBox blockPicBox);
        public delegate void DrawBlockDelegate(int blockX, int blockY, PictureBox picBox);
        // 定義event(事件)
        public event ShowBlockDelegate ShowBlockEvent;
        public event DrawBlockDelegate DrawBlockEvent;

        public void RaiseShowBlock(byte[] srcBytes, PictureBox blockPicBox)
        {
            Thread thread = new Thread(new ThreadStart(new Action(() => {
                ShowBlockEvent(srcBytes, blockPicBox);
            })));
            thread.IsBackground = true;
            thread.Start();
        }

        public void RaiseDrawBlock(int blockX, int blockY, PictureBox picBox)
        {
            Thread thread = new Thread(new ThreadStart(new Action(() => {
                DrawBlockEvent(blockX, blockY, picBox);
            })));
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
