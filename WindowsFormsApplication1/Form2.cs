﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Contrinex.RFID;
using System.IO;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        ConIDHF BusRfidHF;
        IniParser myConfig;
        System.Windows.Forms.Timer timer_status = new System.Windows.Forms.Timer();
        public Form2()
        {
            InitializeComponent();
            comboBox_Name.SelectedIndex = 0;
            comboBox_Baud.SelectedIndex = 1;
            comboBox_DataSize.SelectedIndex = 1;
            comboBox_Parity.SelectedIndex = 0;
            comboBox_Handshake.SelectedIndex = 0;
            comboBox_Mode.SelectedIndex = 0;
            btn_Status.Enabled = false;
            btn_Read.Enabled = false;
            btn_Write.Enabled = false;
        }
        public int ComPort
        {
            get
            {
                if (File.Exists("config.ini"))
                {
                    myConfig = new IniParser("config.ini");
                    if (!myConfig.KeyExists("ComPort", "Params"))
                    {
                        return -1;
                    }
                    string s_max = myConfig.GetSetting("Params", "ComPort");
                    try
                    {
                        return Convert.ToInt16(s_max);
                    }
                    catch (Exception)
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                myConfig = new IniParser("config.ini");
                myConfig.AddSetting("Params","ComPort", value.ToString());
                myConfig.SaveSettings();
            }
        }
        public int RWM_Nb
        {
            get
            {
                if (File.Exists("config.ini"))
                {
                    myConfig = new IniParser("config.ini");
                    if (!myConfig.KeyExists("RWM_Nb", "Params"))
                    {
                        return -1;
                    }
                    string s_max = myConfig.GetSetting("Params", "RWM_Nb");
                    try
                    {
                        return Convert.ToInt16(s_max);
                    }
                    catch (Exception)
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                myConfig = new IniParser("config.ini");
                myConfig.AddSetting("Params", "RWM_Nb", value.ToString());
                myConfig.SaveSettings();
            }
        }
        Form1 mf;
        string videoPath = "";
        private void Form2_Load(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists("config.ini"))
            {
                System.IO.File.Create("config.ini");
            }
            myConfig = new IniParser("config.ini");
            if (ComPort!=-1)
            {
                nUD_ComPort.Value = Convert.ToInt16(ComPort);
            }
            if (RWM_Nb!=-1)
            {
                nUD_RWMNb.Value = Convert.ToInt16(RWM_Nb);
            }

            //System.Threading.Thread thrT = new System.Threading.Thread(new System.Threading.ThreadStart(NewForm));
            //thrT.Start();
            //while (thrT.ThreadState != System.Threading.ThreadState.Running) ;
            //this.WindowState = FormWindowState.Minimized;
            //Application.Run(new Form1());
           
            tsBtn_Main_Click(sender, e);
            mf = new Form1();
            //this.Hide();
            mf.Show();

            timer_status = new System.Windows.Forms.Timer();
            timer_status.Interval = 100;
            timer_status.Tick += new EventHandler(timer_status_Tick);
            timer_status.Enabled = true;            
            
        }

        string lastpath = "";
        void timer_status_Tick(object sender, EventArgs e)
        {
            try
            {
                btn_Init_Click(sender, e);
                if (btn_Status.Enabled)
                {
                    if (lastpath == "" && videoPath == "")
                    {
                        videoPath = myConfig.GetSetting("binding", "");
                        mf.axWindowsMediaPlayer1.URL = videoPath;
                        mf.axWindowsMediaPlayer1.Ctlcontrols.play();
                        mf.axWindowsMediaPlayer1.settings.setMode("loop", true);
                        mf.WindowState = FormWindowState.Maximized;
                        mf.FormBorderStyle = FormBorderStyle.None;
                        timer1.Enabled = true;
                    }
                    
                    btn_Status_Click(sender, e);
                    if (btn_Status.BackColor == Color.GreenYellow)
                    {
                        videoPath = myConfig.GetSetting("binding", tBx_Status.Text.Split('\r')[0]);
                        if (videoPath != null)
                        {
                            if (videoPath != lastpath)
                            {
                                if (File.Exists(videoPath))
                                {
                                    try
                                    {
                                        mf.axWindowsMediaPlayer1.URL = videoPath;
                                        mf.axWindowsMediaPlayer1.Ctlcontrols.play();
                                        mf.axWindowsMediaPlayer1.settings.setMode("loop", true);
                                        timer1.Enabled = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        writeLog(ex.Message, logFormat.File);
                                    }
                                }
                                else
                                {
                                    writeLog("File not exist", logFormat.File);
                                }
                            }
                            lastpath = videoPath;
                        }
                        else
                        {
                            writeLog("myConfig.GetSetting is null", logFormat.File);
                        }
                    }//got status
                }
            }
            catch (Exception ex)
            {
                writeLog(ex.Message, logFormat.File);
            }            
        }

        private void NewForm()
        {
            Application.Run(new Form1());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mf.axWindowsMediaPlayer1.URL = "C:\\Users\\Public\\Videos\\Sample Videos\\Wildlife.wmv";
            mf.axWindowsMediaPlayer1.Ctlcontrols.play();
            mf.axWindowsMediaPlayer1.settings.setMode("loop", true);
            //timer1 = new System.Windows.Forms.Timer(this.components);
            timer1.Enabled = true;
            //timer1.Tick += new System.EventHandler(this.timer1_Tick);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mf.axWindowsMediaPlayer1.URL = @"C:\Users\CN0YLBBC\Videos\m.avi";
            mf.axWindowsMediaPlayer1.Ctlcontrols.play();
            mf.axWindowsMediaPlayer1.settings.setMode("loop", true);
            //timer1 = new System.Windows.Forms.Timer(this.components);
            timer1.Enabled = true;
            //timer1.Tick += new System.EventHandler(this.timer1_Tick);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mf.axWindowsMediaPlayer1.URL = @"C:\Users\CN0YLBBC\Videos\999.avi";
            mf.axWindowsMediaPlayer1.Ctlcontrols.play();
            mf.axWindowsMediaPlayer1.settings.setMode("loop", true);
            mf.axWindowsMediaPlayer1.uiMode = "Full";
            //timer1 = new System.Windows.Forms.Timer(this.components);
            timer1.Enabled = true;
            //timer1.Tick += new System.EventHandler(this.timer1_Tick);
            //mf.axWindowsMediaPlayer1.fullScreen = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(mf.axWindowsMediaPlayer1.playState) == 3)
                {
                    mf.axWindowsMediaPlayer1.fullScreen = true;
                    //timer1.Stop(); timer1.Dispose(); timer1 = null;
                    timer1.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                timer1.Enabled = false;
                writeLog(ex.Message, logFormat.File);
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (txt_Path.Text == "")
            {
                MessageBox.Show("视频路径不能为空。");
            }
            else
            {
                if (myConfig.KeyExists(txt_Label.Text, "binding"))
                {
                    //提示是否删除原有并写入新的
                    if (MessageBox.Show("已存在此标签，确认要覆盖原纪录？", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        myConfig.DeleteSetting("binding", txt_Label.Text);
                        myConfig.AddSetting("binding", txt_Label.Text, txt_Path.Text);
                        myConfig.SaveSettings();
                    }
                }
                else
                {
                    myConfig.AddSetting("binding", txt_Label.Text, txt_Path.Text);
                    myConfig.SaveSettings();
                }

                dataGridView1.Rows.Clear();
                String[] mEnumSection = myConfig.EnumSection("binding");
                for (int i = 0; i < mEnumSection.Length; i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = mEnumSection[i];
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value = myConfig.GetSetting("binding", mEnumSection[i]);
                }
            }
        }

        private void btn_Init_Click(object sender, EventArgs e)
        {
            tBx_Status.Clear();
            tBx_DataRead.Clear();
            tBx_Data.Clear();
            btn_Init.BackColor = Color.Transparent;
            btn_Read.BackColor = Color.Transparent;
            btn_Write.BackColor = Color.Transparent;
            btn_Status.BackColor = Color.Transparent;
            if (BusRfidHF != null)// Est-ce que l'objet existe en m閙oire
            {
                BusRfidHF.Dispose();//Destruction de l'objet
            }
            BusRfidHF = new ConIDHF(); // cr閍tion de l'Objet BusRfidHF de type ConIDHF
            bool InitOK = BusRfidHF.Init(250, 115200, (int)(nUD_ComPort.Value));
            if (InitOK == true)
            {
                ConIDHF.Info InfoRWM;//
                InfoRWM = BusRfidHF.RWMInfo((byte)(nUD_RWMNb.Value));//
                lbl_RwmType.ForeColor = Color.Black;
                if (InfoRWM.AckCode == ConIDHF.Acknldg.OperationSuccessful)//
                {
                    btn_Status.Enabled = true;
                    btn_Read.Enabled = true;
                    btn_Write.Enabled = true;
                    btn_Init.BackColor = Color.GreenYellow;
                    if (InfoRWM.Type == (byte)(RWM.Type.RWM_M30))//
                    {
                        lbl_RwmType.Text = "RWM M30";
                    }
                    else
                    {
                        lbl_RwmType.Text = "RWM M18";
                    }
                }
                else
                {
                    btn_Init.BackColor = Color.Red;
                    lbl_RwmType.ForeColor = Color.Red;
                    lbl_RwmType.Text = "Wrong RWM number";
                    btn_Status.Enabled = false;
                    btn_Read.Enabled = false;
                    btn_Write.Enabled = false;
                }
            }
            else
            {
                btn_Init.BackColor = Color.Red;
                btn_Status.Enabled = false;
                btn_Read.Enabled = false;
                btn_Write.Enabled = false;
            }
        }

        private void btn_Open_Click(object sender, EventArgs e)
        {

        }

        private void btn_Read_Click(object sender, EventArgs e)
        {
            btn_Init.BackColor = Color.Transparent;
            btn_Write.BackColor = Color.Transparent;
            btn_Status.BackColor = Color.Transparent;
            ConIDHF.Data ResRead;//
            ulong UID;
            TAG[] StatusData = BusRfidHF.Status((byte)(nUD_RWMNb.Value));//
            if (StatusData != null)
            {
                if (StatusData.Length > 1)
                {
                    if (chkBx_Addr.Checked == true)
                    {
                        UID = System.Convert.ToUInt64(tBx_UID.Text, 16);
                        ResRead = BusRfidHF.Read((byte)(nUD_RWMNb.Value), 0x20, (byte)(nUD_StartAddr.Value), (byte)(nUD_NbBlocks.Value), UID);//
                        if (ResRead.AckCode == ConIDHF.Acknldg.OperationSuccessful)//
                        {
                            btn_Read.BackColor = Color.GreenYellow;
                            DispData(ResRead);
                        }
                        else
                        {
                            btn_Read.BackColor = Color.Red;
                        }
                    }
                    else
                    {
                        tBx_DataRead.Text = "Impossible to read more than one transponder";
                        btn_Read.BackColor = Color.Red;
                    }
                }
                else if (StatusData.Length == 1)
                {
                    ResRead = BusRfidHF.Read((byte)(nUD_RWMNb.Value), 0, (byte)(nUD_StartAddr.Value), (byte)(nUD_NbBlocks.Value));//
                    if (ResRead.AckCode == ConIDHF.Acknldg.OperationSuccessful)//
                    {
                        btn_Read.BackColor = Color.GreenYellow;
                        DispData(ResRead);
                    }
                    else
                    {
                        btn_Read.BackColor = Color.Red;
                    }
                }
                else
                {
                    tBx_DataRead.Text = "No transponder";
                    btn_Read.BackColor = Color.Red;
                }
            }
            else
            {
                tBx_DataRead.Text = "No RWM";
            }
        }

        // DISPLAYING READING RESULTS
        private void DispData(ConIDHF.Data ResRead)
        {
            tBx_DataRead.Clear();
            int nbLines = Convert.ToInt32(nUD_NbBlocks.Value);
            String[] TablRead = new string[nbLines];
            int Addr = Convert.ToInt32(nUD_StartAddr.Value);
            int i;
            int j;
            for (j = 0; j < nUD_NbBlocks.Value; j++)
            {
                string StLine = "";
                for (i = 0; i < 4; i++)
                {
                    string StData = ResRead.TabData[j * 4 + i].ToString("X02");
                    StLine = StLine + StData + " ";
                }
                TablRead[j] = Convert.ToString(Addr + j) + ": " + StLine;
                tBx_DataRead.AppendText(TablRead[j]);
                tBx_DataRead.AppendText(Environment.NewLine);
            }
        }
        private void btn_Send_Click(object sender, EventArgs e)
        {

        }
        TAG[] StatusData_Cur;
        private void btn_Status_Click(object sender, EventArgs e)
        {
            tBx_Status.Clear();
            tBx_DataRead.Clear();
            tBx_Data.Clear();
            btn_Read.BackColor = Color.Transparent;
            btn_Write.BackColor = Color.Transparent;
            btn_Init.BackColor = Color.Transparent;
            TAG[] StatusData = BusRfidHF.Status((byte)(nUD_RWMNb.Value));//
            StatusData_Cur = StatusData;
            if (StatusData != null)
            {
                if (StatusData.Length != 0)
                {
                    btn_Status.BackColor = Color.GreenYellow;
                    foreach (TAG tag in StatusData)//
                    {
                        tBx_Status.AppendText(tag.UID.ToString("X08"));
                        tBx_Status.AppendText(Environment.NewLine);
                    }
                }
                else
                {
                    tBx_Status.Text = "No transponder";
                    btn_Status.BackColor = Color.Red;
                }
            }
            else
            {
                tBx_Status.Text = "No RWM";
                btn_Status.BackColor = Color.Red;
            }
        }

        private void btn_Write_Click(object sender, EventArgs e)
        {
            btn_Write.BackColor = Color.Transparent;
            btn_Read.BackColor = Color.Transparent;
            btn_Status.BackColor = Color.Transparent;
            btn_Init.BackColor = Color.Transparent;
            ConIDHF.Acknldg WriteAcknldg = ConIDHF.Acknldg.OperationSuccessful;//
            int nbLinesW = Convert.ToInt32(nUD_NbBlocksW.Value);
            string[] TablWrite = new string[nbLinesW];
            byte[] DataToWrite = new byte[nbLinesW * 4];
            int StrLength = tBx_Data.Text.Length;
            string Data0 = tBx_Data.Text;
            if (StrLength != (nbLinesW * 8 + (nbLinesW - 1) * 2))
            {
                btn_Write.BackColor = Color.Red;
                tBx_Data.AppendText(Environment.NewLine);
                tBx_Data.AppendText("Invalid data!");
            }
            else
            {
                for (int j = 0; j < nbLinesW; j++)
                {
                    TablWrite[j] = tBx_Data.Text.Substring(j * 10, 8);
                    for (int i = 0; i < 4; i++)
                    {
                        DataToWrite[4 * j + i] = Convert.ToByte((TablWrite[j].Substring(2 * i, 2)), 16);
                        ulong UID;
                        if (chkBx_Addr.Checked == true)
                        {
                            UID = System.Convert.ToUInt64(tBx_UID.Text, 16);
                            WriteAcknldg = BusRfidHF.Write((byte)(nUD_RWMNb.Value), 0x20, (byte)(nUD_StartAddrW.Value), (byte)(nUD_NbBlocksW.Value), DataToWrite, UID);//
                        }
                        else
                        {
                            WriteAcknldg = BusRfidHF.Write((byte)(nUD_RWMNb.Value), 0x0, (byte)(nUD_StartAddrW.Value), (byte)(nUD_NbBlocksW.Value), DataToWrite);//
                        }
                    }
                }
                if (WriteAcknldg == ConIDHF.Acknldg.OperationSuccessful)//
                {
                    btn_Write.BackColor = Color.GreenYellow;
                }
                else
                {
                    btn_Write.BackColor = Color.Red;
                }
            }
        }

        private void tsBtn_Binding_Click(object sender, EventArgs e)
        {
            panel_Main.Dock = DockStyle.None;
            panel_Main.Visible = false;
            panel_Binding.Visible = true;
            panel_Binding.Dock = DockStyle.Fill;

            dataGridView1.Rows.Clear();
            String[] mEnumSection = myConfig.EnumSection("binding");
            for (int i = 0; i < mEnumSection.Length; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = mEnumSection[i];
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value = myConfig.GetSetting("binding", mEnumSection[i]);
            }
        }

        private void tsBtn_Main_Click(object sender, EventArgs e)
        {
            panel_Binding.Dock = DockStyle.None;
            panel_Binding.Visible = false;
            panel_Main.Visible = true;
            panel_Main.Dock = DockStyle.Fill;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            ComPort = Convert.ToInt16(nUD_ComPort.Value);
            RWM_Nb = Convert.ToInt16(nUD_RWMNb.Value);
           
            if (BusRfidHF != null)//
            {
                BusRfidHF.Dispose();
            }
        }

        private void chkBx_Addr_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBx_Addr.Checked == true)
            {
                tBx_UID.Enabled = true;
            }
            else
            {
                tBx_UID.Enabled = false;
            }
        }

        private void tBx_Data_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || ((e.KeyChar >= 'A') && (e.KeyChar <= 'F')) || ((e.KeyChar >= 'a') && (e.KeyChar <= 'f')) || (e.KeyChar == '\n') || (e.KeyChar == '\r'))
            {

            }
            else
            {
                e.Handled = true;
            }
        }

        private void txt_Path_DoubleClick(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_Path.Text = openFileDialog1.FileName;
            }
        }

        #region LOG
        public enum logFormat
        {
            Screen = 3,
            File,
            Both
        }
        bool moreThanOneInstance
        {
            get
            {
                string currentName = Process.GetCurrentProcess().ProcessName;
                return Process.GetProcessesByName(currentName).Length > 1;
            }
        }
        object logLock = new object();
        public void writeLog(string content, logFormat format, params object[] logStringFormatArgs)
        {
            string line = string.Empty;

            //lock (logLock)
            //{
            // show in screen
            if (format != logFormat.File)
            {
                line = string.IsNullOrWhiteSpace(content) ? "\r\n" : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF") + ": " + String.Format(content.TrimEnd(), logStringFormatArgs) + "\r\n";
                this.Invoke(
                (EventHandler)delegate
                { textBox_log.AppendText(line); }
                );
            }
            // store in log file
            if (format != logFormat.Screen)
            {
                const string LOG_DIR = "logs";
                string logFilePath = Path.Combine(LOG_DIR, DateTime.Now.ToString("yyyy-MM-dd") + ".log");
                if (!Directory.Exists(LOG_DIR)) Directory.CreateDirectory(LOG_DIR);
                line = string.IsNullOrWhiteSpace(content) ? "\r\n" : DateTime.Now.ToString("HH:mm:ss") + ": " + String.Format(content.TrimEnd(), logStringFormatArgs) + "\r\n";
                StreamWriter logFile = new StreamWriter(logFilePath, true, Encoding.UTF8);
                logFile.Write(line);
                logFile.Close();
                logFile.Dispose();
            }
            //}
        }

        private void ClearLog()
        {
            textBox_log.Clear();
        }
        #endregion

    }
}
