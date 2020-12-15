using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace Light4SightNG
{
    public partial class Steuerung : Form
    {
        public static String RodPath = @"presets/rod.pre";
        public static String LConePath = @"presets/lcone.pre";
        public static String MConePath = @"presets/mcone.pre";
        public static String SConePath = @"presets/scone.pre";

        public bool UseConstantStimuli;
        public bool UseBestPEST;
        public int NTrials;
        public int PThreshold;

        //Objekt für die Audioschnittstelle. Dient der Steuerung der Wiedergabe(Start,Stop,Puffer)
        public AudioControlClass AudioControl = new AudioControlClass();

        public Steuerung()
        {

            InitializeComponent();
            tbProbandenNummer.Text = "9999";
            cbAugenseite.Text = "OD";
            RCFF.Enabled = false;
            LCFF.Enabled = false;
            MCFF.Enabled = false;
            SCFF.Enabled = false;
            mRCFF.Enabled = false;
            mLCFF.Enabled = false;
            mMCFF.Enabled = false;
            mSCFF.Enabled = false;
            mACFF.Enabled = false;
            ACFF.Enabled = false;
            NumberOfTrials.Value = 15;
            comboBox1.Items.AddRange(Directory.GetFiles(@".\presets\", "*.pre"));
            comboBox2.Items.AddRange(Directory.GetFiles(@".\presets", "*.pre"));
            comboBox3.Items.AddRange(Directory.GetFiles(@".\presets", "*.pre"));
            comboBox4.Items.AddRange(Directory.GetFiles(@".\presets", "*.pre"));
            selectAlgorithm.SelectedIndex = 0;
            NumberOfTrials.Enabled = false;
            SignalGeneration.InitializeValues();
            envFreq.SelectedIndex = 0;
            pEnv.Checked = false;
            SignalGeneration.Envelope = "0";
            SignalGeneration.PauseEnvelope = false;
            //comboBox1.SelectedIndex = 1;
            //comboBox2.SelectedIndex = 2;
            //comboBox3.SelectedIndex = 3;
            //comboBox4.SelectedIndex = 4;
            if (Directory.GetFiles(@".\Untersuchungen", "*.txt").Length > 0)
            { result.Enabled = true; }
            else
            { result.Enabled = false; }
        }

        void mRCFF_Click(object sender, EventArgs e)
        {
            String bkCFF = RCFF.Text;
            KontrolliereMessungen.cff = -1;
            KontrolliereMessungen determineCFF = new KontrolliereMessungen(this);
            determineCFF.testeCFF = true;
            determineCFF.SetDesktopLocation(this.Location.X + this.Size.Width, this.Location.Y);
            determineCFF.ShowDialog(tbProbandenNummer.Text, cbAugenseite.Text, RodPath);
            RCFF.Text = Convert.ToString(KontrolliereMessungen.cff);
            if (RCFF.Text == "-1") { MessageBox.Show("Messung der CFF nicht erfolgreich."); RCFF.Text = bkCFF; }
            determineCFF.Dispose();
        }

        void mLCFF_Click(object sender, EventArgs e)
        {
            String bkCFF = LCFF.Text;
            KontrolliereMessungen.cff = -1;
            KontrolliereMessungen determineCFF = new KontrolliereMessungen(this);
            determineCFF.testeCFF = true;
            determineCFF.SetDesktopLocation(this.Location.X + this.Size.Width, this.Location.Y);
            determineCFF.ShowDialog(tbProbandenNummer.Text, cbAugenseite.Text, LConePath);
            LCFF.Text = Convert.ToString(KontrolliereMessungen.cff);
            if (LCFF.Text == "-1") { MessageBox.Show("Messung der CFF nicht erfolgreich."); LCFF.Text = bkCFF; }
            determineCFF.Dispose();
        }

        void mMCFF_Click(object sender, EventArgs e)
        {
            String bkCFF = MCFF.Text;
            KontrolliereMessungen.cff = -1;
            KontrolliereMessungen determineCFF = new KontrolliereMessungen(this);
            determineCFF.testeCFF = true;
            determineCFF.SetDesktopLocation(this.Location.X + this.Size.Width, this.Location.Y);
            determineCFF.ShowDialog(tbProbandenNummer.Text, cbAugenseite.Text, MConePath);
            MCFF.Text = Convert.ToString(KontrolliereMessungen.cff);
            if (MCFF.Text == "-1") { MessageBox.Show("Messung der CFF nicht erfolgreich."); MCFF.Text = bkCFF; }
            determineCFF.Dispose();
        }

        void mSCFF_Click(object sender, EventArgs e)
        {
            String bkCFF = SCFF.Text;
            KontrolliereMessungen.cff = -1;
            KontrolliereMessungen determineCFF = new KontrolliereMessungen(this);
            determineCFF.testeCFF = true;
            determineCFF.SetDesktopLocation(this.Location.X + this.Size.Width, this.Location.Y);
            determineCFF.ShowDialog(tbProbandenNummer.Text, cbAugenseite.Text, SConePath);
            SCFF.Text = Convert.ToString(KontrolliereMessungen.cff);
            if (SCFF.Text == "-1") { MessageBox.Show("Messung der CFF nicht erfolgreich."); SCFF.Text = bkCFF; }
            determineCFF.Dispose();
        }

        void start_Click(object sender, EventArgs e)
        {
            if (!(Rod.Checked || LCone.Checked || MCone.Checked || SCone.Checked)) { MessageBox.Show("Bitte mindestens einen Photorezeptor auswählen"); }
            else
            {
                MesseSchwellen messung = new MesseSchwellen(tbProbandenNummer.Text, cbAugenseite.Text, Rod.Checked, RCFF.Text, LCone.Checked, LCFF.Text, MCone.Checked, MCFF.Text, SCone.Checked, SCFF.Text, this);
                messung.ShowDialog();
                messung.Dispose();
            }
        }

        void ACFF_TextChanged(object sender, EventArgs e)
        {
            if (Rod.Checked) RCFF.Text = ACFF.Text;
            if (LCone.Checked) LCFF.Text = ACFF.Text;
            if (MCone.Checked) MCFF.Text = ACFF.Text;
            if (SCone.Checked) SCFF.Text = ACFF.Text;
        }

        void Rod_CheckedChanged(object sender, EventArgs e)
        {
            if (Rod.Checked)
            {
                if (!File.Exists(RodPath))
                {
                    MessageBox.Show("Standard-Preset-Datei (rod.pre) nicht vorhanden!\nBitte manuell wählen.");
                    Rod.Checked = false;
                }
                else
                {
                    RCFF.Enabled = true;
                    mRCFF.Enabled = true;
                    mACFF.Enabled = true;
                    if (RCFF.Text == "")
                        if (ACFF.Text != "") RCFF.Text = ACFF.Text; else RCFF.Text = "";
                }
            }
            else
            {
                RCFF.Enabled = false;
                mRCFF.Enabled = false;
                if (!(Rod.Checked || LCone.Checked || MCone.Checked || SCone.Checked)) mACFF.Enabled = false;
            }
        }

        void LCone_CheckedChanged(object sender, EventArgs e)
        {
            if (LCone.Checked)
            {
                if (!File.Exists(LConePath))
                {
                    MessageBox.Show("Standard-Preset-Datei (lcone.pre) nicht vorhanden!\nBitte manuell wählen.");
                    LCone.Checked = false;
                }
                else
                {
                    LCFF.Enabled = true;
                    mLCFF.Enabled = true;
                    mACFF.Enabled = true;
                    if (LCFF.Text == "")
                        if (ACFF.Text != "") LCFF.Text = ACFF.Text; else LCFF.Text = "";
                }
            }
            else
            {
                LCFF.Enabled = false;
                mLCFF.Enabled = false;
                if (!(Rod.Checked || LCone.Checked || MCone.Checked || SCone.Checked)) mACFF.Enabled = false;
            }

        }

        void MCone_CheckedChanged(object sender, EventArgs e)
        {
            if (MCone.Checked)
            {
                if (!File.Exists(MConePath))
                {
                    MessageBox.Show("Standard-Preset-Datei (mcone.pre) nicht vorhanden!\nBitte manuell wählen.");
                    MCone.Checked = false;
                }
                else
                {
                    MCFF.Enabled = true;
                    mMCFF.Enabled = true;
                    mACFF.Enabled = true;
                    if (MCFF.Text == "")
                        if (ACFF.Text != "") MCFF.Text = ACFF.Text; else MCFF.Text = "";
                }
            }
            else
            {
                MCFF.Enabled = false;
                mMCFF.Enabled = false;
                if (!(Rod.Checked || LCone.Checked || MCone.Checked || SCone.Checked)) mACFF.Enabled = false;

            }
        }

        void SCone_CheckedChanged(object sender, EventArgs e)
        {
            if (SCone.Checked)
            {
                if (!File.Exists(SConePath))
                {
                    MessageBox.Show("Standard-Preset-Datei (scone.pre) nicht vorhanden!\nBitte manuell wählen.");
                    SCone.Checked = false;
                }
                else
                {
                    SCFF.Enabled = true;
                    mSCFF.Enabled = true;
                    mACFF.Enabled = true;
                    if (SCFF.Text == "")
                        if (ACFF.Text != "") SCFF.Text = ACFF.Text; else SCFF.Text = "";
                }
            }
            else
            {
                SCFF.Enabled = false;
                mSCFF.Enabled = false;
                if (!(Rod.Checked || LCone.Checked || MCone.Checked || SCone.Checked)) mACFF.Enabled = false;

            }
        }

        void button1_Click(object sender, EventArgs e)
        {
            String bkCFF = "";
            KontrolliereMessungen determineCFF;

            if (Rod.Checked)
            {
                bkCFF = RCFF.Text;
                KontrolliereMessungen.cff = -1;
                determineCFF = new KontrolliereMessungen(this);
                determineCFF.testeCFF = true;
                determineCFF.SetDesktopLocation(this.Location.X + this.Size.Width, this.Location.Y);
                determineCFF.ShowDialog(tbProbandenNummer.Text, cbAugenseite.Text, RodPath);
                RCFF.Text = Convert.ToString(KontrolliereMessungen.cff);
                determineCFF.Dispose();
            }

            if (LCone.Checked)
            {
                bkCFF = LCFF.Text;
                KontrolliereMessungen.cff = -1;
                determineCFF = new KontrolliereMessungen(this);
                determineCFF.testeCFF = true;
                determineCFF.SetDesktopLocation(this.Location.X + this.Size.Width, this.Location.Y);
                determineCFF.ShowDialog(tbProbandenNummer.Text, cbAugenseite.Text, LConePath);
                LCFF.Text = Convert.ToString(KontrolliereMessungen.cff);
                determineCFF.Dispose();
            }

            if (MCone.Checked)
            {
                bkCFF = MCFF.Text;
                KontrolliereMessungen.cff = -1;
                determineCFF = new KontrolliereMessungen(this);
                determineCFF.testeCFF = true;
                determineCFF.SetDesktopLocation(this.Location.X + this.Size.Width, this.Location.Y);
                determineCFF.ShowDialog(tbProbandenNummer.Text, cbAugenseite.Text, MConePath);
                MCFF.Text = Convert.ToString(KontrolliereMessungen.cff);
                determineCFF.Dispose();
            }

            if (SCone.Checked)
            {
                bkCFF = SCFF.Text;
                KontrolliereMessungen.cff = -1;
                determineCFF = new KontrolliereMessungen(this);
                determineCFF.testeCFF = true;
                determineCFF.SetDesktopLocation(this.Location.X + this.Size.Width, this.Location.Y);
                determineCFF.ShowDialog(tbProbandenNummer.Text, cbAugenseite.Text, SConePath);
                SCFF.Text = Convert.ToString(KontrolliereMessungen.cff);
                determineCFF.Dispose();
            }

            Console.Beep();
            Thread.Sleep(100);
            Console.Beep();

            if (RCFF.Text == "-1") { MessageBox.Show("Messung der Stäbchen-CFF nicht erfolgreich."); RCFF.Text = bkCFF; }
            if (LCFF.Text == "-1") { MessageBox.Show("Messung der L-Zapfen-CFF nicht erfolgreich."); LCFF.Text = bkCFF; }
            if (MCFF.Text == "-1") { MessageBox.Show("Messung der M-Zapfen-CFF nicht erfolgreich."); MCFF.Text = bkCFF; }
            if (SCFF.Text == "-1") { MessageBox.Show("Messung der S-Zapfen-CFF nicht erfolgreich."); SCFF.Text = bkCFF; }

        }

        void result_Click(object sender, EventArgs e)
        {
            Process myProcess = new Process();
            myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.FileName = @"launch.bat";
            myProcess.EnableRaisingEvents = true;
            myProcess.Exited += new EventHandler(myProcess_Exited);
            myProcess.Start();
            myProcess.WaitForExit();
            ZeigeErgebnis Erg = new ZeigeErgebnis();
            Erg.ladeBild(@".\Untersuchungen\plot.bmp");
            Erg.ladeText(@".\Untersuchungen\daten.csv");
            Erg.ShowDialog();
        }

        void ergebnisseVorhanden_Changed(object sender, FileSystemEventArgs e)
        {
            if (Directory.GetFiles(@".\Untersuchungen", "*.txt").Length > 0)
            { result.Enabled = true; }
            else
            { result.Enabled = false; }
        }

        void ergebnisseVorhanden_Created(object sender, FileSystemEventArgs e)
        {
            if (Directory.GetFiles(@".\Untersuchungen", "*.txt").Length > 0)
            { result.Enabled = true; }
            else
            { result.Enabled = false; }
        }

        void ergebnisseVorhanden_Deleted(object sender, FileSystemEventArgs e)
        {
            if (Directory.GetFiles(@".\Untersuchungen", "*.txt").Length > 0)
            { result.Enabled = true; }
            else
            { result.Enabled = false; }
        }

        void myProcess_Exited(object sender, System.EventArgs e) { }

        void UseDefaultCFF_CheckedChanged(object sender, EventArgs e)
        {
            if (UseDefaultCFF.Checked) ACFF.Enabled = true;
            else
            {
                ACFF.Enabled = false;
                ACFF.Text = "";
            }
        }

        void dateinamen_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = !(comboBox1.Enabled);
            comboBox2.Enabled = !(comboBox2.Enabled);
            comboBox3.Enabled = !(comboBox3.Enabled);
            comboBox4.Enabled = !(comboBox4.Enabled);
        }

        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dateiname = comboBox1.SelectedItem.ToString();
            if (File.Exists(dateiname))
            {
                RodPath = dateiname;
            }
            else
            {
                MessageBox.Show("Dieser Dateiname existiert nicht.");
            }
        }

        void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dateiname = comboBox2.SelectedItem.ToString();
            if (File.Exists(dateiname))
            {
                LConePath = dateiname;
            }
            else
            {
                MessageBox.Show("Dieser Dateiname existiert nicht.");
            }
        }

        void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dateiname = comboBox3.SelectedItem.ToString();
            if (File.Exists(dateiname))
            {
                MConePath = dateiname;
            }
            else
            {
                MessageBox.Show("Dieser Dateiname existiert nicht.");
            }
        }

        void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dateiname = comboBox4.SelectedItem.ToString();
            if (File.Exists(dateiname))
            {
                SConePath = dateiname;
            }
            else
            {
                MessageBox.Show("Dieser Dateiname existiert nicht.");
            }
        }

        void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            SignalGeneration.Envelope = envFreq.SelectedItem.ToString();
        }

        void pEnv_CheckedChanged(object sender, EventArgs e)
        {
            SignalGeneration.PauseEnvelope = pEnv.Checked;
        }

        void selectAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (selectAlgorithm.Text)
            {
                case "Constant Stimuli":
                    UseConstantStimuli = true;
                    NumberOfTrials.Enabled = true;
                    break;
                case "Randomly-Interleaved-Staircases":
                    UseConstantStimuli = false;
                    Globals.Staircase = "beide";
                    NumberOfTrials.Enabled = false;
                    break;
                case "Threshold finder (BP)":
                    UseConstantStimuli = false;
                    UseBestPEST = true;
                    NumberOfTrials.Enabled = true;
                    break;
            }
        }

        void NumberOfTrials_ValueChanged(object sender, EventArgs e)
        {
            NTrials = (int)NumberOfTrials.Value;
        }

        private void Steuerung_Load(object sender, EventArgs e)
        {

        }

        private void PredictedThreshold_ValueChanged(object sender, EventArgs e)
        {
            PThreshold = (int)PredictedThreshold.Value;
        }
    }
}
