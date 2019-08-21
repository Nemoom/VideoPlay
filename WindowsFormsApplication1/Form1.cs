using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<string> listSong = new List<string>();
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择文件";
            ofd.Multiselect = true;
            ofd.InitialDirectory = @"C:\Users\Public\Videos\Sample Videos";
            ofd.Filter = "音乐文件|*.wav|mp3文件|*.mp3|所有文件|*.*";
            ofd.ShowDialog();
            //获得我们在文件夹中选择所有文件的全路径
            string[] path = ofd.FileNames;
            for(int i=0;i<path.Length;i++)
            {                
                //将音乐文件的全路径存储到泛型集合中
                listSong.Add(path[i]);
            }

        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (playToolStripMenuItem.Text == "Play")
            {
                axWindowsMediaPlayer1.URL = listSong[0];
                axWindowsMediaPlayer1.Ctlcontrols.play();
                playToolStripMenuItem.Text = "Pause";
            }
            else
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
                playToolStripMenuItem.Text = "Play";
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
