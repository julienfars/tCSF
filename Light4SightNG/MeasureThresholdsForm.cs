using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Light4SightNG
{
    public partial class MeasureThresholdsForm : Form
    {
        int[] frequenzen = { 1, 2, 4, 6, 8, 10, 12, 16, 20, 28, 36, 44 };
        int gesamtzahl = 0;
        int RFF, LFF, MFF, SFF;
        List<String> messungen = new List<String>();
        String SubjectID;
        String Augenseite;

        MainForm parentObject;

        public MeasureThresholdsForm(String pnummer, String aseite, bool ractive, String r, bool lactive, String l, bool mactive, String m, bool sactive, String s, MainForm parentObj)
        {

            parentObject = parentObj;

            InitializeComponent();
            SubjectID = pnummer;
            Augenseite = aseite;
            try
            {
                RFF = Convert.ToInt16(r);
            }
            catch
            {
                RFF = 999;
            }
            try
            {
                LFF = Convert.ToInt16(l);
            }
            catch
            {
                LFF = 999;
            }
            try
            {
                MFF = Convert.ToInt16(m);
            }
            catch
            {
                MFF = 999;
            }
            try
            {
                SFF = Convert.ToInt16(s);
            }
            catch
            {
                SFF = 999;
            }
            if (!ractive) RodList.Enabled = false;
            if (!lactive) LConeList.Enabled = false;
            if (!mactive) MConeList.Enabled = false;
            if (!sactive) SConeList.Enabled = false;
            for (int i = 0; i < 12; i++)
            {
                if (ractive)
                    if (frequenzen[i] <= RFF) { RodList.Items.Add(frequenzen[i], true); gesamtzahl++; }
                if (lactive)
                    if (frequenzen[i] <= LFF) { LConeList.Items.Add(frequenzen[i], true); gesamtzahl++; }
                if (mactive)
                    if (frequenzen[i] <= MFF) { MConeList.Items.Add(frequenzen[i], true); gesamtzahl++; }
                if (sactive)
                    if (frequenzen[i] <= SFF) { SConeList.Items.Add(frequenzen[i], true); gesamtzahl++; }
            }
            RodList.CheckOnClick = true;
            LConeList.CheckOnClick = true;
            MConeList.CheckOnClick = true;
            SConeList.CheckOnClick = true;
            fortschritt.Maximum = gesamtzahl;
            fortschritt.Value = 0;
            if (Directory.GetFiles(@".\Untersuchungen", "*.txt").Length > 0)
            { result.Enabled = true; }
            else
            { result.Enabled = false; }
        }

        void button1_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        void start_Click(object sender, EventArgs e)
        {
            CheckedListBox Liste = RodList;
            foreach (object o in RodList.CheckedItems)
            {
                messungen.Add(String.Concat("R", o.ToString()));
            }
            foreach (object o in LConeList.CheckedItems)
            {
                messungen.Add(String.Concat("L", o.ToString()));
            }
            foreach (object o in MConeList.CheckedItems)
            {
                messungen.Add(String.Concat("M", o.ToString()));
            }
            foreach (object o in SConeList.CheckedItems)
            {
                messungen.Add(String.Concat("S", o.ToString()));
            }
            var messungenZufall = messungen.OrderBy(a => Guid.NewGuid());
            foreach (String f in messungenZufall)
            {
                MeasurementForm messeSchwelle = new MeasurementForm(parentObject);
                messeSchwelle.testeCFF = false;
                messeSchwelle.freq = Convert.ToInt16(f.Substring(1));
                messeSchwelle.SetDesktopLocation(this.Location.X + this.Size.Width, this.Location.Y);
                switch (f.Substring(0, 1))
                {
                    case "R":
                        messeSchwelle.ShowDialog(SubjectID, Augenseite, MainForm.RodPath);
                        Liste = RodList;
                        break;
                    case "L":
                        messeSchwelle.ShowDialog(SubjectID, Augenseite, MainForm.LConePath);
                        Liste = LConeList;
                        break;
                    case "M":
                        messeSchwelle.ShowDialog(SubjectID, Augenseite, MainForm.MConePath);
                        Liste = MConeList;
                        break;
                    case "S":
                        messeSchwelle.ShowDialog(SubjectID, Augenseite, MainForm.SConePath);
                        Liste = SConeList;
                        break;
                }
                messeSchwelle.Dispose();
                int i = 0, index = -1;
                foreach (object o in Liste.Items)
                {
                    if (o.ToString() == f.Substring(1))
                    {
                        index = i;
                    }
                    i++;
                }
                Liste.SetItemCheckState(index, CheckState.Unchecked);
                fortschritt.Value++;
                messungen.Clear();
            }
            fortschritt.Value = 0;
            Thread.Sleep(100);
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
            ShowResultsForm Erg = new ShowResultsForm();
            Erg.LoadImage(@"Untersuchungen\plot.bmp");
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

    }
}
