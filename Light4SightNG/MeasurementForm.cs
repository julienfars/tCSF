using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Light4SightNG
{
    public partial class MeasurementForm : Form
    {
        //Kanalobjekkte erzeugen
        // public static ChannelDescription[] Channels = new ChannelDescription[8];
        public static ChannelDescription IRChannel = new ChannelDescription();
        public static ChannelDescription IGChannel = new ChannelDescription();
        public static ChannelDescription IBChannel = new ChannelDescription();
        public static ChannelDescription ICChannel = new ChannelDescription();
        public static ChannelDescription ORChannel = new ChannelDescription();
        public static ChannelDescription OGChannel = new ChannelDescription();
        public static ChannelDescription OBChannel = new ChannelDescription();
        public static ChannelDescription OCChannel = new ChannelDescription();

        public static int cff;

        public bool testeCFF = false;

        public bool UseBestPEST;

        public int freq = -1;

        StdStrategie stdStrategie;
        TestStrategy Strategie;

        List<ChannelDescription> channels = new List<ChannelDescription>();

        MainForm mainProgram;

        //Objekte für Logging und Debugging

        public LogWriter logfiletmp;//Wird durch die Funktion LogFile an die Namenskonvention von DebugFile angepasst
        //public LogWriter DebugFile = new LogWriter("debugdata.txt", true);

        public MeasurementForm(MainForm gpObject)
        {
            // Create ChannelDescriptions
            // for (int i = 0; i < 8; i++) Channels[i] = new ChannelDescription();

            this.mainProgram = gpObject;
            UseBestPEST = mainProgram.UseBestPEST;

            InitializeComponent();

            if (Globals.ReadCalibrationData() == 0)
            {
                this.lblIRMHMax.Text = Globals.dMaxMH(0).ToString();
                IRChannel.MaxCandelaPerSquareMeter = Globals.dMaxMH(0);
                IRChannel.ParameterPolynom(Globals.poly4(0), Globals.poly3(0), Globals.poly2(0), Globals.poly1(0), Globals.intercept(0));

                this.lblIGMHMax.Text = Globals.dMaxMH(1).ToString();
                IGChannel.MaxCandelaPerSquareMeter = Globals.dMaxMH(1);
                IGChannel.ParameterPolynom(Globals.poly4(1), Globals.poly3(1), Globals.poly2(1), Globals.poly1(1), Globals.intercept(1));

                this.lblIBMHMax.Text = Globals.dMaxMH(2).ToString();
                IBChannel.MaxCandelaPerSquareMeter = Globals.dMaxMH(2);
                IBChannel.ParameterPolynom(Globals.poly4(2), Globals.poly3(2), Globals.poly2(2), Globals.poly1(2), Globals.intercept(2));

                this.lblICMHMax.Text = Globals.dMaxMH(3).ToString();
                ICChannel.MaxCandelaPerSquareMeter = Globals.dMaxMH(3);
                ICChannel.ParameterPolynom(Globals.poly4(3), Globals.poly3(3), Globals.poly2(3), Globals.poly1(3), Globals.intercept(3));

                this.lblORMHMax.Text = Globals.dMaxMH(4).ToString();
                ORChannel.MaxCandelaPerSquareMeter = Globals.dMaxMH(4);
                ORChannel.ParameterPolynom(Globals.poly4(4), Globals.poly3(4), Globals.poly2(4), Globals.poly1(4), Globals.intercept(4));

                this.lblOGMHMax.Text = Globals.dMaxMH(5).ToString();
                OGChannel.MaxCandelaPerSquareMeter = Globals.dMaxMH(5);
                OGChannel.ParameterPolynom(Globals.poly4(5), Globals.poly3(5), Globals.poly2(5), Globals.poly1(5), Globals.intercept(5));

                this.lblOBMHMax.Text = Globals.dMaxMH(6).ToString();
                OBChannel.MaxCandelaPerSquareMeter = Globals.dMaxMH(6);
                OBChannel.ParameterPolynom(Globals.poly4(6), Globals.poly3(6), Globals.poly2(6), Globals.poly1(6), Globals.intercept(6));

                this.lblOCMHMax.Text = Globals.dMaxMH(7).ToString();
                OCChannel.MaxCandelaPerSquareMeter = Globals.dMaxMH(7);
                OCChannel.ParameterPolynom(Globals.poly4(7), Globals.poly3(7), Globals.poly2(7), Globals.poly1(7), Globals.intercept(7));
            }
            else
            {
                this.lblIRMHMax.Text = "Kalibrierung durchführen!";
                this.lblIGMHMax.Text = "Kalibrierung durchführen!";
                this.lblIBMHMax.Text = "Kalibrierung durchführen!";
                this.lblICMHMax.Text = "Kalibrierung durchführen!";
                this.lblORMHMax.Text = "Kalibrierung durchführen!";
                this.lblOGMHMax.Text = "Kalibrierung durchführen!";
                this.lblOBMHMax.Text = "Kalibrierung durchführen!";
                this.lblOCMHMax.Text = "Kalibrierung durchführen!";
                btnUntersuchungAbbrechenActive(false);
                btnUntersuchungStartenActive(false);
            }

            channels.Add(IRChannel);
            channels.Add(IGChannel);
            channels.Add(IBChannel);
            channels.Add(ICChannel);
            channels.Add(ORChannel);
            channels.Add(OGChannel);
            channels.Add(OBChannel);
            channels.Add(OCChannel);

            this.btnUntersuchungAbbrechenActive(false);
            this.btnUntersuchungStartenActive(true);

            this.KeyPreview = true;

            this.showEnvFreq.Text = SignalGeneration.Envelope;
            this.showEnvPause.Text = SignalGeneration.PauseEnvelope.ToString();
        }

        public void ShowDialog(String Probandennummer, String Augenseite, String Presets)
        {
            tbProbandenNummer.Text = Probandennummer;
            cbAugenseite.Text = Augenseite;
            this.btnLoadPreset_Click(this, null, Presets);
            this.btnUntersuchungStarten_Click(this, null);
            this.prepareLogFile();
            if (UseBestPEST) Strategie.StarteStrategie();
            else stdStrategie.StartStdStrategie();
            this.ShowDialog();
        }

        public void LogFile(string text, bool header)
        {
            if (Globals.flagDebugLog == true)
            {
                logfiletmp.add(text);
                //DebugFile.add("LOGFILE: " + logfiletmp.add(text));
                this.tbUntersuchungsVerlauf.AppendText("\r\n" + text);
            }
            else
            {
                logfiletmp.add(text);
                if (!header)
                {
                    this.tbUntersuchungsVerlauf.AppendText("\r\n" + text);
                }
            }
        }

        void prepareLogFile()
        {
            string line2, line3, line4, line5, line6, line7, line8, line9, line10;
            LogFile(";;Centerfield;;;;;Surroundfield;;", true);
            LogFile(";;R;G;B;C;;R;G;B;C;", true);

            LogFile("Frequenz Envelope;;" + SignalGeneration.Envelope, true);
            LogFile("Pausiere Envelope;;" + Convert.ToString(SignalGeneration.PauseEnvelope), true);

            if (MeasurementForm.IRChannel.IsActive == true)
            {
                line2 = ("Signal aktiv;;" + MeasurementForm.IRChannel.IsActive.ToString() + ";");
                line3 = ("Signalform;;" + MeasurementForm.IRChannel.SignalType + ";");
                line4 = ("Helligkeit;;" + MeasurementForm.IRChannel.CandelaPerSquareMeter.ToString() + ";");
                line5 = ("Frequenz;;" + MeasurementForm.IRChannel.Frequency.ToString() + ";");
                line6 = ("Phase;;" + MeasurementForm.IRChannel.GetPhase().ToString() + ";");
                line7 = ("Kontrast SC1;;" + MeasurementForm.IRChannel.StartContrastDownStaircase.ToString() + ";");
                line8 = ("Kontrast SC2;;" + MeasurementForm.IRChannel.StartContrastUpStaircase.ToString() + ";");
                line9 = ("Delta Kontrast SC1;;" + MeasurementForm.IRChannel.StepsizeDownStaircase.ToString() + ";");
                line10 = ("Delta Kontrast SC2;;" + MeasurementForm.IRChannel.StepsizeUpStaircase.ToString() + ";");
            }
            else
            {
                line2 = ("Signal aktiv;;");
                line3 = ("Signalform;;;");
                line4 = ("Helligkeit;;;");
                line5 = ("Frequenz;;;");
                line6 = ("Phase;;;");
                line7 = ("Kontrast SC1;;;");
                line8 = ("Kontrast SC2;;;");
                line9 = ("Delta Kontrast SC1;;;");
                line10 = ("Delta Kontrast SC2;;;");
            }

            if (MeasurementForm.IGChannel.IsActive == true)
            {
                line2 = (line2 + MeasurementForm.IGChannel.IsActive.ToString() + ";");
                line3 = (line3 + MeasurementForm.IGChannel.SignalType + ";");
                line4 = (line4 + MeasurementForm.IGChannel.CandelaPerSquareMeter.ToString() + ";");
                line5 = (line5 + MeasurementForm.IGChannel.Frequency.ToString() + ";");
                line6 = (line6 + MeasurementForm.IGChannel.GetPhase().ToString() + ";");
                line7 = (line7 + MeasurementForm.IGChannel.StartContrastDownStaircase.ToString() + ";");
                line8 = (line8 + MeasurementForm.IGChannel.StartContrastUpStaircase.ToString() + ";");
                line9 = (line9 + MeasurementForm.IGChannel.StepsizeDownStaircase.ToString() + ";");
                line10 = (line10 + MeasurementForm.IGChannel.StepsizeUpStaircase.ToString() + ";");
            }
            else
            {
                line2 = (line2 + ";");
                line3 = (line3 + ";");
                line4 = (line4 + ";");
                line5 = (line5 + ";");
                line6 = (line6 + ";");
                line7 = (line7 + ";");
                line8 = (line8 + ";");
                line9 = (line9 + ";");
                line10 = (line10 + ";");
            }


            if (MeasurementForm.IBChannel.IsActive == true)
            {
                line2 = (line2 + MeasurementForm.IBChannel.IsActive.ToString() + ";");
                line3 = (line3 + MeasurementForm.IBChannel.SignalType + ";");
                line4 = (line4 + MeasurementForm.IBChannel.CandelaPerSquareMeter.ToString() + ";");
                line5 = (line5 + MeasurementForm.IBChannel.Frequency.ToString() + ";");
                line6 = (line6 + MeasurementForm.IBChannel.GetPhase().ToString() + ";");
                line7 = (line7 + MeasurementForm.IBChannel.StartContrastDownStaircase.ToString() + ";");
                line8 = (line8 + MeasurementForm.IBChannel.StartContrastUpStaircase.ToString() + ";");
                line9 = (line9 + MeasurementForm.IBChannel.StepsizeDownStaircase.ToString() + ";");
                line10 = (line10 + MeasurementForm.IBChannel.StepsizeUpStaircase.ToString() + ";");
            }
            else
            {
                line2 = (line2 + ";");
                line3 = (line3 + ";");
                line4 = (line4 + ";");
                line5 = (line5 + ";");
                line6 = (line6 + ";");
                line7 = (line7 + ";");
                line8 = (line8 + ";");
                line9 = (line9 + ";");
                line10 = (line10 + ";");
            }

            if (MeasurementForm.ICChannel.IsActive == true)
            {
                line2 = (line2 + MeasurementForm.ICChannel.IsActive.ToString() + ";;");
                line3 = (line3 + MeasurementForm.ICChannel.SignalType + ";;");
                line4 = (line4 + MeasurementForm.ICChannel.CandelaPerSquareMeter.ToString() + ";;");
                line5 = (line5 + MeasurementForm.ICChannel.Frequency.ToString() + ";;");
                line6 = (line6 + MeasurementForm.ICChannel.GetPhase().ToString() + ";;");
                line7 = (line7 + MeasurementForm.ICChannel.StartContrastDownStaircase.ToString() + ";;");
                line8 = (line8 + MeasurementForm.ICChannel.StartContrastUpStaircase.ToString() + ";;");
                line9 = (line9 + MeasurementForm.ICChannel.StepsizeDownStaircase.ToString() + ";;");
                line10 = (line10 + MeasurementForm.ICChannel.StepsizeUpStaircase.ToString() + ";;");
            }
            else
            {
                line2 = (line2 + ";;;");
                line3 = (line3 + ";;;");
                line4 = (line4 + ";;;");
                line5 = (line5 + ";;;");
                line6 = (line6 + ";;;");
                line7 = (line7 + ";;;");
                line8 = (line8 + ";;;");
                line9 = (line9 + ";;;");
                line10 = (line10 + ";;;");
            }

            if (MeasurementForm.ORChannel.IsActive == true)
            {
                line2 = (line2 + MeasurementForm.ORChannel.IsActive.ToString() + ";");
                line3 = (line3 + MeasurementForm.ORChannel.SignalType + ";");
                line4 = (line4 + MeasurementForm.ORChannel.CandelaPerSquareMeter.ToString() + ";");
                line5 = (line5 + MeasurementForm.ORChannel.Frequency.ToString() + ";");
                line6 = (line6 + MeasurementForm.ORChannel.GetPhase().ToString() + ";");
                line7 = (line7 + MeasurementForm.ORChannel.StartContrastDownStaircase.ToString() + ";");
                line8 = (line8 + MeasurementForm.ORChannel.StartContrastUpStaircase.ToString() + ";");
                line9 = (line9 + MeasurementForm.ORChannel.StepsizeDownStaircase.ToString() + ";");
                line10 = (line10 + MeasurementForm.ORChannel.StepsizeUpStaircase.ToString() + ";");
            }
            else
            {
                line2 = (line2 + ";");
                line3 = (line3 + ";");
                line4 = (line4 + ";");
                line5 = (line5 + ";");
                line6 = (line6 + ";");
                line7 = (line7 + ";");
                line8 = (line8 + ";");
                line9 = (line9 + ";");
                line10 = (line10 + ";");
            }

            if (MeasurementForm.OGChannel.IsActive == true)
            {
                line2 = (line2 + MeasurementForm.OGChannel.IsActive.ToString() + ";");
                line3 = (line3 + MeasurementForm.OGChannel.SignalType + ";");
                line4 = (line4 + MeasurementForm.OGChannel.CandelaPerSquareMeter.ToString() + ";");
                line5 = (line5 + MeasurementForm.OGChannel.Frequency.ToString() + ";");
                line6 = (line6 + MeasurementForm.OGChannel.GetPhase().ToString() + ";");
                line7 = (line7 + MeasurementForm.OGChannel.StartContrastDownStaircase.ToString() + ";");
                line8 = (line8 + MeasurementForm.OGChannel.StartContrastUpStaircase.ToString() + ";");
                line9 = (line9 + MeasurementForm.OGChannel.StepsizeDownStaircase.ToString() + ";");
                line10 = (line10 + MeasurementForm.OGChannel.StepsizeUpStaircase.ToString() + ";");
            }
            else
            {
                line2 = (line2 + ";");
                line3 = (line3 + ";");
                line4 = (line4 + ";");
                line5 = (line5 + ";");
                line6 = (line6 + ";");
                line7 = (line7 + ";");
                line8 = (line8 + ";");
                line9 = (line9 + ";");
                line10 = (line10 + ";");
            }

            if (MeasurementForm.OBChannel.IsActive == true)
            {
                line2 = (line2 + MeasurementForm.OBChannel.IsActive.ToString() + ";");
                line3 = (line3 + MeasurementForm.OBChannel.SignalType + ";");
                line4 = (line4 + MeasurementForm.OBChannel.CandelaPerSquareMeter.ToString() + ";");
                line5 = (line5 + MeasurementForm.OBChannel.Frequency.ToString() + ";");
                line6 = (line6 + MeasurementForm.OBChannel.GetPhase().ToString() + ";");
                line7 = (line7 + MeasurementForm.OBChannel.StartContrastDownStaircase.ToString() + ";");
                line8 = (line8 + MeasurementForm.OBChannel.StartContrastUpStaircase.ToString() + ";");
                line9 = (line9 + MeasurementForm.OBChannel.StepsizeDownStaircase.ToString() + ";");
                line10 = (line10 + MeasurementForm.OBChannel.StepsizeUpStaircase.ToString() + ";");
            }
            else
            {
                line2 = (line2 + ";");
                line3 = (line3 + ";");
                line4 = (line4 + ";");
                line5 = (line5 + ";");
                line6 = (line6 + ";");
                line7 = (line7 + ";");
                line8 = (line8 + ";");
                line9 = (line9 + ";");
                line10 = (line10 + ";");
            }

            if (MeasurementForm.OCChannel.IsActive == true)
            {
                line2 = (line2 + MeasurementForm.OCChannel.IsActive.ToString() + ";");
                line3 = (line3 + MeasurementForm.OCChannel.SignalType + ";");
                line4 = (line4 + MeasurementForm.OCChannel.CandelaPerSquareMeter.ToString() + ";");
                line5 = (line5 + MeasurementForm.OCChannel.Frequency.ToString() + ";");
                line6 = (line6 + MeasurementForm.OCChannel.GetPhase().ToString() + ";");
                line7 = (line7 + MeasurementForm.OCChannel.StartContrastDownStaircase.ToString() + ";");
                line8 = (line8 + MeasurementForm.OCChannel.StartContrastUpStaircase.ToString() + ";");
                line9 = (line9 + MeasurementForm.OCChannel.StepsizeDownStaircase.ToString() + ";");
                line10 = (line10 + MeasurementForm.OCChannel.StepsizeUpStaircase.ToString() + ";");
            }
            else
            {
                line2 = (line2 + ";");
                line3 = (line3 + ";");
                line4 = (line4 + ";");
                line5 = (line5 + ";");
                line6 = (line6 + ";");
                line7 = (line7 + ";");
                line8 = (line8 + ";");
                line9 = (line9 + ";");
                line10 = (line10 + ";");
            }

            LogFile(line2, true);
            LogFile(line3, true);
            LogFile(line4, true);
            LogFile(line5, true);
            LogFile(line6, true);
            LogFile(line7, true);
            LogFile(line8, true);
            LogFile(line9, true);
            LogFile(line10, true);
            LogFile("", true);
        }

        void closeAllPanels()
        {
            this.pnlOuterRed.Hide();
            this.pnlOuterRed.Visible = false;
            this.pnlOuterGreen.Hide();
            this.pnlOuterGreen.Visible = false;
            this.pnlOuterBlue.Hide();
            this.pnlOuterBlue.Visible = false;
            this.pnlOuterCyan.Hide();
            this.pnlOuterCyan.Visible = false;
            this.pnlInnerRed.Hide();
            this.pnlInnerRed.Visible = false;
            this.pnlInnerGreen.Hide();
            this.pnlInnerGreen.Visible = false;
            this.pnlInnerBlue.Hide();
            this.pnlInnerBlue.Visible = false;
            this.pnlInnerCyan.Hide();
            this.pnlInnerCyan.Visible = false;
        }

        void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            #region Panel Control
            switch (e.Node.Name)
            {
                case "tnOuterRed":
                    {
                        if (this.pnlOuterRed.Visible == false)
                        {
                            this.closeAllPanels();
                            this.btnUntersuchungStartenActive(true);
                            this.pnlOuterRed.Show();
                            this.pnlOuterRed.Visible = true;
                        }
                        break;
                    }
                case "tnOuterGreen":
                    {
                        if (this.pnlOuterGreen.Visible == false)
                        {
                            this.closeAllPanels();
                            this.btnUntersuchungStartenActive(true);
                            this.pnlOuterGreen.Show();
                            this.pnlOuterGreen.Visible = true;
                        }
                        break;
                    }
                case "tnOuterBlue":
                    {
                        if (this.pnlOuterBlue.Visible == false)
                        {
                            this.closeAllPanels();
                            this.btnUntersuchungStartenActive(true);
                            this.pnlOuterBlue.Show();
                            this.pnlOuterBlue.Visible = true;
                        }
                        break;
                    }
                case "tnOuterCyan":
                    {
                        if (this.pnlOuterCyan.Visible == false)
                        {
                            this.closeAllPanels();
                            this.btnUntersuchungStartenActive(true);
                            this.pnlOuterCyan.Show();
                            this.pnlOuterCyan.Visible = true;
                        }
                        break;
                    }
                case "tnInnerRed":
                    {
                        if (this.pnlInnerRed.Visible == false)
                        {
                            this.closeAllPanels();
                            this.btnUntersuchungStartenActive(true);
                            this.pnlInnerRed.Show();
                            this.pnlInnerRed.Visible = true;
                        }
                        break;
                    }
                case "tnInnerGreen":
                    {
                        if (this.pnlInnerGreen.Visible == false)
                        {
                            this.closeAllPanels();
                            this.btnUntersuchungStartenActive(true);
                            this.pnlInnerGreen.Show();
                            this.pnlInnerGreen.Visible = true;
                        }
                        break;
                    }
                case "tnInnerBlue":
                    {
                        if (this.pnlInnerBlue.Visible == false)
                        {
                            this.closeAllPanels();
                            this.btnUntersuchungStartenActive(true);
                            this.pnlInnerBlue.Show();
                            this.pnlInnerBlue.Visible = true;
                        }
                        break;
                    }
                case "tnInnerCyan":
                    {
                        if (this.pnlInnerCyan.Visible == false)
                        {
                            this.closeAllPanels();
                            this.btnUntersuchungStartenActive(true);
                            this.pnlInnerCyan.Show();
                            this.pnlInnerCyan.Visible = true;
                        }
                        break;
                    }

                default:
                    this.closeAllPanels();
                    break;
            }
            #endregion


        }

        public string LCheck(string KanalInfoString, double MHcdm2_max, double MHcdm2, int Kontrast)
        {
            double dLMax = (1 + (Kontrast / 100)) * MHcdm2;
            double dLMin = (1 - (Kontrast / 100)) * MHcdm2;

            return dLMax > MHcdm2_max || dLMin < 0 ? "Kontrastwert für " + KanalInfoString + " ist ungültig!" : "OK";
        }

        void ReadSignalDescription()
        {
            IRChannel.IsActive = cbAktivIR.Checked;
            if (cbAktivIR.Checked)
            {
                IRChannel.SignalType = cbSigFormIR.Text;
                IRChannel.Frequency = int.Parse(tbFreqIR.Text);
                IRChannel.StartContrastDownStaircase = double.Parse(tbKonIRSC1.Text);
                IRChannel.StartContrastUpStaircase = double.Parse(tbKonIRSC2.Text);
                IRChannel.StepsizeDownStaircase = double.Parse(tbIRSC1DeltaK.Text);
                IRChannel.StepsizeUpStaircase = double.Parse(tbIRSC2DeltaK.Text);
                IRChannel.CandelaPerSquareMeter = double.Parse(tbMHIR.Text);
                IRChannel.SetPhase(int.Parse(tbPhasVerschIR.Text));
            }

            IGChannel.IsActive = cbAktivIG.Checked;
            if (cbAktivIG.Checked)
            {
                IGChannel.SignalType = cbSigFormIG.Text;
                IGChannel.Frequency = int.Parse(tbFreqIG.Text);
                IGChannel.StartContrastDownStaircase = double.Parse(tbKonIGSC1.Text);
                IGChannel.StartContrastUpStaircase = double.Parse(tbKonIGSC2.Text);
                IGChannel.StepsizeDownStaircase = double.Parse(tbIGSC1DeltaK.Text);
                IGChannel.StepsizeUpStaircase = double.Parse(tbIGSC2DeltaK.Text);
                IGChannel.CandelaPerSquareMeter = double.Parse(tbMHIG.Text);
                IGChannel.SetPhase(int.Parse(tbPhasVerschIG.Text));
            }

            IBChannel.IsActive = cbAktivIB.Checked;
            if (cbAktivIB.Checked)
            {
                IBChannel.SignalType = cbSigFormIB.Text;
                IBChannel.Frequency = int.Parse(tbFreqIB.Text);
                IBChannel.StartContrastDownStaircase = double.Parse(tbKonIBSC1.Text);
                IBChannel.StartContrastUpStaircase = double.Parse(tbKonIBSC2.Text);
                IBChannel.StepsizeDownStaircase = double.Parse(tbIBSC1DeltaK.Text);
                IBChannel.StepsizeUpStaircase = double.Parse(tbIBSC2DeltaK.Text);
                IBChannel.CandelaPerSquareMeter = double.Parse(tbMHIB.Text);
                IBChannel.SetPhase(int.Parse(tbPhasVerschIB.Text));
            }

            ICChannel.IsActive = cbAktivIC.Checked;
            if (cbAktivIC.Checked)
            {
                ICChannel.SignalType = cbSigFormIC.Text;
                ICChannel.Frequency = int.Parse(tbFreqIC.Text);
                ICChannel.StartContrastDownStaircase = double.Parse(tbKonICSC1.Text);
                ICChannel.StartContrastUpStaircase = double.Parse(tbKonICSC2.Text);
                ICChannel.StepsizeDownStaircase = double.Parse(tbICSC1DeltaK.Text);
                ICChannel.StepsizeUpStaircase = double.Parse(tbICSC2DeltaK.Text);
                ICChannel.CandelaPerSquareMeter = double.Parse(tbMHIC.Text);
                ICChannel.SetPhase(int.Parse(tbPhasVerschIC.Text));
            }

            ORChannel.IsActive = cbAktivOR.Checked;
            if (cbAktivOR.Checked)
            {
                ORChannel.SignalType = cbSigFormOR.Text;
                ORChannel.Frequency = int.Parse(tbFreqOR.Text);
                ORChannel.StartContrastDownStaircase = double.Parse(tbKonORSC1.Text);
                ORChannel.StartContrastUpStaircase = double.Parse(tbKonORSC2.Text);
                ORChannel.StepsizeDownStaircase = double.Parse(tbORSC1DeltaK.Text);
                ORChannel.StepsizeUpStaircase = double.Parse(tbORSC2DeltaK.Text);
                ORChannel.CandelaPerSquareMeter = double.Parse(tbMHOR.Text);
                ORChannel.SetPhase(int.Parse(tbPhasVerschOR.Text));
            }

            OGChannel.IsActive = cbAktivOG.Checked;
            if (cbAktivOG.Checked)
            {
                OGChannel.SignalType = cbSigFormOG.Text;
                OGChannel.Frequency = int.Parse(tbFreqOG.Text);
                OGChannel.StartContrastDownStaircase = double.Parse(tbKonOGSC1.Text);
                OGChannel.StartContrastUpStaircase = double.Parse(tbKonOGSC2.Text);
                OGChannel.StepsizeDownStaircase = double.Parse(tbOGSC1DeltaK.Text);
                OGChannel.StepsizeUpStaircase = double.Parse(tbOGSC2DeltaK.Text);
                OGChannel.CandelaPerSquareMeter = double.Parse(tbMHOG.Text);
                OGChannel.SetPhase(int.Parse(tbPhasVerschOG.Text));
            }

            OBChannel.IsActive = cbAktivOB.Checked;
            if (cbAktivOB.Checked)
            {
                OBChannel.SignalType = cbSigFormOB.Text;
                OBChannel.Frequency = int.Parse(tbFreqOB.Text);
                OBChannel.StartContrastDownStaircase = double.Parse(tbKonOBSC1.Text);
                OBChannel.StartContrastUpStaircase = double.Parse(tbKonOBSC2.Text);
                OBChannel.StepsizeDownStaircase = double.Parse(tbOBSC1DeltaK.Text);
                OBChannel.StepsizeUpStaircase = double.Parse(tbOBSC2DeltaK.Text);
                OBChannel.CandelaPerSquareMeter = double.Parse(tbMHOB.Text);
                OBChannel.SetPhase(int.Parse(tbPhasVerschOB.Text));
            }

            OCChannel.IsActive = cbAktivOC.Checked;
            if (cbAktivOC.Checked)
            {
                OCChannel.SignalType = cbSigFormOC.Text;
                OCChannel.Frequency = int.Parse(tbFreqOC.Text);
                OCChannel.StartContrastDownStaircase = double.Parse(tbKonOCSC1.Text);
                OCChannel.StartContrastUpStaircase = double.Parse(tbKonOCSC2.Text);
                OCChannel.StepsizeDownStaircase = double.Parse(tbOCSC1DeltaK.Text);
                OCChannel.StepsizeUpStaircase = double.Parse(tbOCSC2DeltaK.Text);
                OCChannel.CandelaPerSquareMeter = double.Parse(tbMHOC.Text);
                OCChannel.SetPhase(int.Parse(tbPhasVerschOC.Text));
            }


        }

        void btnUntersuchungStarten_Click(object sender, EventArgs e)
        {
            if (CheckProband())
            {
                DateTime time = DateTime.Now;
                string format = "yyyy-MM-dd_HHmmss";

                if (this.testeCFF) logfiletmp = new LogWriter(tbProbandenNummer.Text + "_" + this.cbAugenseite.Text + "_" + time.ToString(format) + ".cff", false);
                else logfiletmp = new LogWriter(tbProbandenNummer.Text + "_" + this.cbAugenseite.Text + "_" + time.ToString(format) + ".txt", false);

                this.ReadSignalDescription();

                Globals.flagUntersuchunglaeuft = true;
                this.btnUntersuchungAbbrechen.Enabled = true;
                this.btnUntersuchungStarten.Enabled = false;
                this.tbUntersuchungsVerlauf.Clear();
                this.tbUntersuchungsVerlauf.Visible = true;
                this.tbProbandenNummer.Enabled = false;
                this.cbAugenseite.Enabled = false;
                this.KeyPreview = true;

                if ((IRChannel.StepsizeDownStaircase < float.Epsilon) &&
                    (IGChannel.StepsizeDownStaircase < float.Epsilon) &&
                    (Math.Abs(IBChannel.StepsizeDownStaircase) < float.Epsilon) &&
                    (ICChannel.StepsizeDownStaircase < float.Epsilon))
                {
                    if (UseBestPEST) Strategie = new ConstantStimuli(mainProgram);
                    else stdStrategie = new StdStrategie(mainProgram, "außen", testeCFF);
                }
                else
                {
                    if (UseBestPEST) Strategie = new ConstantStimuli(mainProgram);
                    else stdStrategie = new StdStrategie(mainProgram, "innen", testeCFF);
                }
                if (UseBestPEST) Strategie.Abbruch += new EventHandler<AbbruchEventArgs>(stdStrategie_abbruch);
                else stdStrategie.abbruch += new EventHandler<AbbruchEventArgs>(stdStrategie_abbruch);

                if (!testeCFF)
                {
                    if (freq > 0)
                    {
                        if (UseBestPEST) Strategie._setNewFrequency(freq);
                        else stdStrategie._setNewFrequency(freq);
                    }
                }
            }
        }

        bool CheckProband()
        {
            if (tbProbandenNummer.Text.ToString() == "" || this.cbAugenseite.Text.ToString() == "")
            {
                MessageBox.Show("Probandendaten nicht korrekt" + "\nProbandennummer: " + tbProbandenNummer.Text + "\nAugenseite: " + this.cbAugenseite.Text);
                return false;
            }
            return true;
        }

        void stdStrategie_abbruch(object sender, AbbruchEventArgs e)
        {
            this.KeyPreview = false;
            if (stdStrategie != null)
            {
                stdStrategie.SignalStoppen();
                Thread.Sleep(100);
                stdStrategie = null;
            }
            this.logfiletmp.close();
            this.btnUntersuchungAbbrechen.Enabled = false;
            this.btnUntersuchungStarten.Enabled = true;
            this.tbProbandenNummer.Enabled = true;
            this.cbAugenseite.Enabled = true;
            this.Dispose();
        }

        void btnUntersuchungAbbrechen_Click(object sender, EventArgs e)
        {
            Thread.Sleep(100);
            AbbruchEventArgs mye = new AbbruchEventArgs("");
            stdStrategie_abbruch(this, mye);
            this.Close();
        }

        public void btnUntersuchungStartenActive(bool bstatus)
        {
            if (bstatus == true)
                this.btnUntersuchungStarten.Enabled = true;
            else
                this.btnUntersuchungStarten.Enabled = false;
        }

        public void btnUntersuchungAbbrechenActive(bool bstatus)
        {
            if (bstatus == true)
                this.btnUntersuchungAbbrechen.Enabled = true;
            else
                this.btnUntersuchungAbbrechen.Enabled = false;
        }

        void Light4SightNG_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Y)
            {
                if (UseBestPEST)
                {
                    if (Strategie != null) { Strategie.Gesehen_KeyDown(e); }
                }
                else
                {
                    if (stdStrategie != null) { stdStrategie.Gesehen_KeyDown(e); }
                }
            }
            if (e.KeyCode == Keys.M)
            {
                if (UseBestPEST)
                {
                    if (Strategie != null) { Strategie.Nichtgesehen_KeyDown(e); }
                }
                else
                {
                    if (stdStrategie != null) { stdStrategie.Nichtgesehen_KeyDown(e); }
                }
            }
            if (e.KeyCode == Keys.Q)
            {
                if (btnUntersuchungAbbrechen.Enabled == true) btnUntersuchungAbbrechen_Click(this, null);
            }
            if (e.KeyCode == Keys.T)
            {
                if (btnUntersuchungAbbrechen.Enabled == false) btnUntersuchungStarten_Click(this, null);
            }
        }

        void Light4SightNG_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.KeyPreview = false;
        }

        void btnLoadPreset_Click(object sender, EventArgs e)
        {
            btnLoadPreset_Click(sender, e, "");
        }

        void btnLoadPreset_Click(object sender, EventArgs e, string dateiname)
        {
            if (dateiname == "")
            {
                dateiname = openFileDialog1.FileName;
                openFileDialog1.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath) + @"\presets\";
                openFileDialog1.RestoreDirectory = true;
                openFileDialog1.Filter = "preset files (*.pre)|*.pre";
                if (openFileDialog1.ShowDialog() != DialogResult.OK) { dateiname = ""; } else { dateiname = openFileDialog1.FileName; }
            }

            if (dateiname != "")
            {
                StreamReader sr = new StreamReader(dateiname);

                string blah;
                List<string> temp = new List<string>();
                blah = sr.ReadToEnd();


                char[] delimiters = { '\n' };
                char[] delimiters2 = { ';' };
                List<string[]> columns = new List<string[]>();
                temp.AddRange(blah.Split(delimiters));
                foreach (string s in temp)
                {
                    columns.Add(s.Split(delimiters2));
                }

                string[] values = columns[0];
                if (values[0] == "True") cbAktivIR.Checked = true; else cbAktivIR.Checked = false;
                cbSigFormIR.Text = values[1];
                tbFreqIR.Text = values[2];
                tbMHIR.Text = values[3];
                tbPhasVerschIR.Text = values[4];
                tbKonIRSC1.Text = values[5];
                tbIRSC1DeltaK.Text = values[6];
                tbKonIRSC2.Text = values[7];
                tbIRSC2DeltaK.Text = values[8];

                values = columns[1];
                if (values[0] == "True") cbAktivIG.Checked = true; else cbAktivIG.Checked = false;
                cbSigFormIG.Text = values[1];
                tbFreqIG.Text = values[2];
                tbMHIG.Text = values[3];
                tbPhasVerschIG.Text = values[4];
                tbKonIGSC1.Text = values[5];
                tbIGSC1DeltaK.Text = values[6];
                tbKonIGSC2.Text = values[7];
                tbIGSC2DeltaK.Text = values[8];

                values = columns[2];
                if (values[0] == "True") cbAktivIB.Checked = true; else cbAktivIB.Checked = false;
                cbSigFormIB.Text = values[1];
                tbFreqIB.Text = values[2];
                tbMHIB.Text = values[3];
                tbPhasVerschIB.Text = values[4];
                tbKonIBSC1.Text = values[5];
                tbIBSC1DeltaK.Text = values[6];
                tbKonIBSC2.Text = values[7];
                tbIBSC2DeltaK.Text = values[8];

                values = columns[3];
                if (values[0] == "True") cbAktivIC.Checked = true; else cbAktivIC.Checked = false;
                cbSigFormIC.Text = values[1];
                tbFreqIC.Text = values[2];
                tbMHIC.Text = values[3];
                tbPhasVerschIC.Text = values[4];
                tbKonICSC1.Text = values[5];
                tbICSC1DeltaK.Text = values[6];
                tbKonICSC2.Text = values[7];
                tbICSC2DeltaK.Text = values[8];

                values = columns[4];
                if (values[0] == "True") cbAktivOR.Checked = true; else cbAktivOR.Checked = false;
                cbSigFormOR.Text = values[1];
                tbFreqOR.Text = values[2];
                tbMHOR.Text = values[3];
                tbPhasVerschOR.Text = values[4];
                tbKonORSC1.Text = values[5];
                tbORSC1DeltaK.Text = values[6];
                tbKonORSC2.Text = values[7];
                tbORSC2DeltaK.Text = values[8];

                values = columns[5];
                if (values[0] == "True") cbAktivOG.Checked = true; else cbAktivOG.Checked = false;
                cbSigFormOG.Text = values[1];
                tbFreqOG.Text = values[2];
                tbMHOG.Text = values[3];
                tbPhasVerschOG.Text = values[4];
                tbKonOGSC1.Text = values[5];
                tbOGSC1DeltaK.Text = values[6];
                tbKonOGSC2.Text = values[7];
                tbOGSC2DeltaK.Text = values[8];

                values = columns[6];
                if (values[0] == "True") cbAktivOB.Checked = true; else cbAktivOB.Checked = false;
                cbSigFormOB.Text = values[1];
                tbFreqOB.Text = values[2];
                tbMHOB.Text = values[3];
                tbPhasVerschOB.Text = values[4];
                tbKonOBSC1.Text = values[5];
                tbOBSC1DeltaK.Text = values[6];
                tbKonOBSC2.Text = values[7];
                tbOBSC2DeltaK.Text = values[8];

                values = columns[7];
                if (values[0] == "True") cbAktivOC.Checked = true; else cbAktivOC.Checked = false;
                cbSigFormOC.Text = values[1];
                tbFreqOC.Text = values[2];
                tbMHOC.Text = values[3];
                tbPhasVerschOC.Text = values[4];
                tbKonOCSC1.Text = values[5];
                tbOCSC1DeltaK.Text = values[6];
                tbKonOCSC2.Text = values[7];
                tbOCSC2DeltaK.Text = values[8];
            }
        }

        void btnSavePreset_Click(object sender, EventArgs e)
        {
            ReadSignalDescription();
            DirectoryInfo d = new DirectoryInfo(@".\presets\");
            d.Create();

            saveFileDialog1.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath) + @"\presets\";
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.Filter = "preset files (*.pre)|*.pre";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
                string temp = string.Empty;

                foreach (ChannelDescription chan in channels)
                {
                    temp = chan.IsActive.ToString() + ";";
                    temp = temp + chan.SignalType + ";";
                    temp = temp + chan.Frequency.ToString() + ";";
                    temp = temp + chan.CandelaPerSquareMeter.ToString() + ";";
                    temp = temp + chan.GetPhase().ToString() + ";";
                    temp = temp + chan.StartContrastDownStaircase.ToString() + ";";
                    temp = temp + chan.StepsizeDownStaircase.ToString() + ";";
                    temp = temp + chan.StartContrastUpStaircase.ToString() + ";";
                    temp = temp + chan.StepsizeUpStaircase.ToString() + ";";
                    sw.WriteLine(temp);
                }
                sw.Flush();
                sw.Close();
            }

        }

        void Light4SightNG_Load(object sender, EventArgs e)
        {

        }

    }
}
