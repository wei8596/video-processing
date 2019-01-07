namespace videoProcess
{
    partial class FormMain
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnPlay = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLoad1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLoad2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCompression = new System.Windows.Forms.ToolStripMenuItem();
            this.menuIntra = new System.Windows.Forms.ToolStripMenuItem();
            this.menuIntraSample = new System.Windows.Forms.ToolStripMenuItem();
            this.menuInter = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMotion = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBlockDiff = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTSS = new System.Windows.Forms.ToolStripMenuItem();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuInterSample = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 512);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnPause
            // 
            this.btnPause.Image = ((System.Drawing.Image)(resources.GetObject("btnPause.Image")));
            this.btnPause.Location = new System.Drawing.Point(84, 596);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(30, 30);
            this.btnPause.TabIndex = 2;
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnNext
            // 
            this.btnNext.Image = ((System.Drawing.Image)(resources.GetObject("btnNext.Image")));
            this.btnNext.Location = new System.Drawing.Point(120, 596);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(30, 30);
            this.btnNext.TabIndex = 3;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Image = ((System.Drawing.Image)(resources.GetObject("btnPrev.Image")));
            this.btnPrev.Location = new System.Drawing.Point(12, 596);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(30, 30);
            this.btnPrev.TabIndex = 4;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(530, 27);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(512, 512);
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // btnPlay
            // 
            this.btnPlay.Image = ((System.Drawing.Image)(resources.GetObject("btnPlay.Image")));
            this.btnPlay.Location = new System.Drawing.Point(48, 596);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(30, 30);
            this.btnPlay.TabIndex = 8;
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuCompression});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1052, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLoad1,
            this.menuLoad2,
            this.menuExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F)));
            this.menuFile.ShowShortcutKeys = false;
            this.menuFile.Size = new System.Drawing.Size(38, 20);
            this.menuFile.Text = "&File";
            // 
            // menuLoad1
            // 
            this.menuLoad1.Name = "menuLoad1";
            this.menuLoad1.Size = new System.Drawing.Size(147, 22);
            this.menuLoad1.Text = "Load Video1";
            this.menuLoad1.Click += new System.EventHandler(this.menuLoad1_Click);
            // 
            // menuLoad2
            // 
            this.menuLoad2.Name = "menuLoad2";
            this.menuLoad2.Size = new System.Drawing.Size(147, 22);
            this.menuLoad2.Text = "Load Video2";
            this.menuLoad2.Click += new System.EventHandler(this.menuLoad2_Click);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.menuExit.Size = new System.Drawing.Size(147, 22);
            this.menuExit.Text = "E&xit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuCompression
            // 
            this.menuCompression.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuIntra,
            this.menuInter});
            this.menuCompression.Name = "menuCompression";
            this.menuCompression.Size = new System.Drawing.Size(93, 20);
            this.menuCompression.Text = "&Compression";
            // 
            // menuIntra
            // 
            this.menuIntra.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuIntraSample});
            this.menuIntra.Name = "menuIntra";
            this.menuIntra.Size = new System.Drawing.Size(180, 22);
            this.menuIntra.Text = "Intraframe";
            // 
            // menuIntraSample
            // 
            this.menuIntraSample.Name = "menuIntraSample";
            this.menuIntraSample.Size = new System.Drawing.Size(180, 22);
            this.menuIntraSample.Text = "Sub-sampling";
            this.menuIntraSample.Click += new System.EventHandler(this.menuIntraSample_Click);
            // 
            // menuInter
            // 
            this.menuInter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMotion,
            this.menuBlockDiff,
            this.menuTSS,
            this.menuInterSample});
            this.menuInter.Name = "menuInter";
            this.menuInter.Size = new System.Drawing.Size(180, 22);
            this.menuInter.Text = "Interframe";
            // 
            // menuMotion
            // 
            this.menuMotion.Name = "menuMotion";
            this.menuMotion.Size = new System.Drawing.Size(272, 22);
            this.menuMotion.Text = "Block Based Motion Compensation";
            this.menuMotion.Click += new System.EventHandler(this.menuMotion_Click);
            // 
            // menuBlockDiff
            // 
            this.menuBlockDiff.Name = "menuBlockDiff";
            this.menuBlockDiff.Size = new System.Drawing.Size(272, 22);
            this.menuBlockDiff.Text = "Block Based Difference Coding";
            this.menuBlockDiff.Click += new System.EventHandler(this.menuBlockDiff_Click);
            // 
            // menuTSS
            // 
            this.menuTSS.Name = "menuTSS";
            this.menuTSS.Size = new System.Drawing.Size(272, 22);
            this.menuTSS.Text = "Three Step Search (TSS)";
            this.menuTSS.Click += new System.EventHandler(this.menuTSS_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(12, 545);
            this.trackBar1.Maximum = 58;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(138, 45);
            this.trackBar1.TabIndex = 13;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(156, 545);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(37, 23);
            this.textBox1.TabIndex = 14;
            this.textBox1.Text = "0";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // menuInterSample
            // 
            this.menuInterSample.Name = "menuInterSample";
            this.menuInterSample.Size = new System.Drawing.Size(272, 22);
            this.menuInterSample.Text = "Sub-sampling";
            this.menuInterSample.Click += new System.EventHandler(this.menuInterSample_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 633);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormMain";
            this.Text = "Video Processing  By ChenWeiFan";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuLoad1;
        private System.Windows.Forms.ToolStripMenuItem menuLoad2;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem menuCompression;
        private System.Windows.Forms.ToolStripMenuItem menuIntra;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripMenuItem menuInter;
        private System.Windows.Forms.ToolStripMenuItem menuMotion;
        private System.Windows.Forms.ToolStripMenuItem menuIntraSample;
        private System.Windows.Forms.ToolStripMenuItem menuBlockDiff;
        private System.Windows.Forms.ToolStripMenuItem menuTSS;
        private System.Windows.Forms.ToolStripMenuItem menuInterSample;
    }
}

