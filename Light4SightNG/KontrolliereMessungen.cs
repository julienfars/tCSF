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
    public partial class KontrolliereMessungen : Form
    {
        //Kanalobjekkte erzeugen
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

        public bool UseConstantStimuli;
        public bool UseBestPEST;
        public int freq = -1;

        StdStrategie stdStrategie;
        Teststrategie Strategie;

        List<ChannelDescription> channels = new List<ChannelDescription>();

        Steuerung mainProgram;

        //Objekte für Logging und Debugging

        public LogWriter logfiletmp;//Wird durch die Funktion LogFile an die Namenskonvention von DebugFile angepasst
        //public LogWriter DebugFile = new LogWriter("debugdata.txt", true);

        public KontrolliereMessungen(Steuerung gpObject)
        {

            this.mainProgram = gpObject;
            UseConstantStimuli = mainProgram.UseConstantStimuli;
            UseBestPEST = mainProgram.UseBestPEST;

            InitializeComponent();

            #region Kalibrierung einlesen und MaxWerte berechnen/anzeigen
            if (Globals.ReadCalibrationData() == 0)
            {
                this.lblIRMHMax.Text = Globals.dMaxMH(0).ToString();
                IRChannel.MaxMHCal = Globals.dMaxMH(0);
                IRChannel.ParameterPolynom(Globals.poly4(0), Globals.poly3(0), Globals.poly2(0), Globals.poly1(0), Globals.intercept(0));

                this.lblIGMHMax.Text = Globals.dMaxMH(1).ToString();
                IGChannel.MaxMHCal = Globals.dMaxMH(1);
                IGChannel.ParameterPolynom(Globals.poly4(1), Globals.poly3(1), Globals.poly2(1), Globals.poly1(1), Globals.intercept(1));

                this.lblIBMHMax.Text = Globals.dMaxMH(2).ToString();
                IBChannel.MaxMHCal = Globals.dMaxMH(2);
                IBChannel.ParameterPolynom(Globals.poly4(2), Globals.poly3(2), Globals.poly2(2), Globals.poly1(2), Globals.intercept(2));

                this.lblICMHMax.Text = Globals.dMaxMH(3).ToString();
                ICChannel.MaxMHCal = Globals.dMaxMH(3);
                ICChannel.ParameterPolynom(Globals.poly4(3), Globals.poly3(3), Globals.poly2(3), Globals.poly1(3), Globals.intercept(3));

                this.lblORMHMax.Text = Globals.dMaxMH(4).ToString();
                ORChannel.MaxMHCal = Globals.dMaxMH(4);
                ORChannel.ParameterPolynom(Globals.poly4(4), Globals.poly3(4), Globals.poly2(4), Globals.poly1(4), Globals.intercept(4));

                this.lblOGMHMax.Text = Globals.dMaxMH(5).ToString();
                OGChannel.MaxMHCal = Globals.dMaxMH(5);
                OGChannel.ParameterPolynom(Globals.poly4(5), Globals.poly3(5), Globals.poly2(5), Globals.poly1(5), Globals.intercept(5));

                this.lblOBMHMax.Text = Globals.dMaxMH(6).ToString();
                OBChannel.MaxMHCal = Globals.dMaxMH(6);
                OBChannel.ParameterPolynom(Globals.poly4(6), Globals.poly3(6), Globals.poly2(6), Globals.poly1(6), Globals.intercept(6));

                this.lblOCMHMax.Text = Globals.dMaxMH(7).ToString();
                OCChannel.MaxMHCal = Globals.dMaxMH(7);
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
            #endregion

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
            if (UseConstantStimuli) Strategie.StarteStrategie();
            else if (UseBestPEST) Strategie.StarteStrategie();
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

            if (KontrolliereMessungen.IRChannel.SignalAktiv == true)
            {
                line2 = ("Signal aktiv;;" + KontrolliereMessungen.IRChannel.SignalAktiv.ToString() + ";");
                line3 = ("Signalform;;" + KontrolliereMessungen.IRChannel.Signalform + ";");
                line4 = ("Helligkeit;;" + KontrolliereMessungen.IRChannel.MittlereHelligkeit_cdm2.ToString() + ";");
                line5 = ("Frequenz;;" + KontrolliereMessungen.IRChannel.Frequenz.ToString() + ";");
                line6 = ("Phase;;" + KontrolliereMessungen.IRChannel.Phasenverschiebung.ToString() + ";");
                line7 = ("Kontrast SC1;;" + KontrolliereMessungen.IRChannel.KonSC1_100.ToString() + ";");
                line8 = ("Kontrast SC2;;" + KontrolliereMessungen.IRChannel.KonSC2_100.ToString() + ";");
                line9 = ("Delta Kontrast SC1;;" + KontrolliereMessungen.IRChannel.SC1DeltaK_100.ToString() + ";");
                line10 = ("Delta Kontrast SC2;;" + KontrolliereMessungen.IRChannel.SC2DeltaK_100.ToString() + ";");
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

            if (KontrolliereMessungen.IGChannel.SignalAktiv == true)
            {
                line2 = (line2 + KontrolliereMessungen.IGChannel.SignalAktiv.ToString() + ";");
                line3 = (line3 + KontrolliereMessungen.IGChannel.Signalform + ";");
                line4 = (line4 + KontrolliereMessungen.IGChannel.MittlereHelligkeit_cdm2.ToString() + ";");
                line5 = (line5 + KontrolliereMessungen.IGChannel.Frequenz.ToString() + ";");
                line6 = (line6 + KontrolliereMessungen.IGChannel.Phasenverschiebung.ToString() + ";");
                line7 = (line7 + KontrolliereMessungen.IGChannel.KonSC1_100.ToString() + ";");
                line8 = (line8 + KontrolliereMessungen.IGChannel.KonSC2_100.ToString() + ";");
                line9 = (line9 + KontrolliereMessungen.IGChannel.SC1DeltaK_100.ToString() + ";");
                line10 = (line10 + KontrolliereMessungen.IGChannel.SC2DeltaK_100.ToString() + ";");
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


            if (KontrolliereMessungen.IBChannel.SignalAktiv == true)
            {
                line2 = (line2 + KontrolliereMessungen.IBChannel.SignalAktiv.ToString() + ";");
                line3 = (line3 + KontrolliereMessungen.IBChannel.Signalform + ";");
                line4 = (line4 + KontrolliereMessungen.IBChannel.MittlereHelligkeit_cdm2.ToString() + ";");
                line5 = (line5 + KontrolliereMessungen.IBChannel.Frequenz.ToString() + ";");
                line6 = (line6 + KontrolliereMessungen.IBChannel.Phasenverschiebung.ToString() + ";");
                line7 = (line7 + KontrolliereMessungen.IBChannel.KonSC1_100.ToString() + ";");
                line8 = (line8 + KontrolliereMessungen.IBChannel.KonSC2_100.ToString() + ";");
                line9 = (line9 + KontrolliereMessungen.IBChannel.SC1DeltaK_100.ToString() + ";");
                line10 = (line10 + KontrolliereMessungen.IBChannel.SC2DeltaK_100.ToString() + ";");
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

            if (KontrolliereMessungen.ICChannel.SignalAktiv == true)
            {
                line2 = (line2 + KontrolliereMessungen.ICChannel.SignalAktiv.ToString() + ";;");
                line3 = (line3 + KontrolliereMessungen.ICChannel.Signalform + ";;");
                line4 = (line4 + KontrolliereMessungen.ICChannel.MittlereHelligkeit_cdm2.ToString() + ";;");
                line5 = (line5 + KontrolliereMessungen.ICChannel.Frequenz.ToString() + ";;");
                line6 = (line6 + KontrolliereMessungen.ICChannel.Phasenverschiebung.ToString() + ";;");
                line7 = (line7 + KontrolliereMessungen.ICChannel.KonSC1_100.ToString() + ";;");
                line8 = (line8 + KontrolliereMessungen.ICChannel.KonSC2_100.ToString() + ";;");
                line9 = (line9 + KontrolliereMessungen.ICChannel.SC1DeltaK_100.ToString() + ";;");
                line10 = (line10 + KontrolliereMessungen.ICChannel.SC2DeltaK_100.ToString() + ";;");
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

            if (KontrolliereMessungen.ORChannel.SignalAktiv == true)
            {
                line2 = (line2 + KontrolliereMessungen.ORChannel.SignalAktiv.ToString() + ";");
                line3 = (line3 + KontrolliereMessungen.ORChannel.Signalform + ";");
                line4 = (line4 + KontrolliereMessungen.ORChannel.MittlereHelligkeit_cdm2.ToString() + ";");
                line5 = (line5 + KontrolliereMessungen.ORChannel.Frequenz.ToString() + ";");
                line6 = (line6 + KontrolliereMessungen.ORChannel.Phasenverschiebung.ToString() + ";");
                line7 = (line7 + KontrolliereMessungen.ORChannel.KonSC1_100.ToString() + ";");
                line8 = (line8 + KontrolliereMessungen.ORChannel.KonSC2_100.ToString() + ";");
                line9 = (line9 + KontrolliereMessungen.ORChannel.SC1DeltaK_100.ToString() + ";");
                line10 = (line10 + KontrolliereMessungen.ORChannel.SC2DeltaK_100.ToString() + ";");
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

            if (KontrolliereMessungen.OGChannel.SignalAktiv == true)
            {
                line2 = (line2 + KontrolliereMessungen.OGChannel.SignalAktiv.ToString() + ";");
                line3 = (line3 + KontrolliereMessungen.OGChannel.Signalform + ";");
                line4 = (line4 + KontrolliereMessungen.OGChannel.MittlereHelligkeit_cdm2.ToString() + ";");
                line5 = (line5 + KontrolliereMessungen.OGChannel.Frequenz.ToString() + ";");
                line6 = (line6 + KontrolliereMessungen.OGChannel.Phasenverschiebung.ToString() + ";");
                line7 = (line7 + KontrolliereMessungen.OGChannel.KonSC1_100.ToString() + ";");
                line8 = (line8 + KontrolliereMessungen.OGChannel.KonSC2_100.ToString() + ";");
                line9 = (line9 + KontrolliereMessungen.OGChannel.SC1DeltaK_100.ToString() + ";");
                line10 = (line10 + KontrolliereMessungen.OGChannel.SC2DeltaK_100.ToString() + ";");
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

            if (KontrolliereMessungen.OBChannel.SignalAktiv == true)
            {
                line2 = (line2 + KontrolliereMessungen.OBChannel.SignalAktiv.ToString() + ";");
                line3 = (line3 + KontrolliereMessungen.OBChannel.Signalform + ";");
                line4 = (line4 + KontrolliereMessungen.OBChannel.MittlereHelligkeit_cdm2.ToString() + ";");
                line5 = (line5 + KontrolliereMessungen.OBChannel.Frequenz.ToString() + ";");
                line6 = (line6 + KontrolliereMessungen.OBChannel.Phasenverschiebung.ToString() + ";");
                line7 = (line7 + KontrolliereMessungen.OBChannel.KonSC1_100.ToString() + ";");
                line8 = (line8 + KontrolliereMessungen.OBChannel.KonSC2_100.ToString() + ";");
                line9 = (line9 + KontrolliereMessungen.OBChannel.SC1DeltaK_100.ToString() + ";");
                line10 = (line10 + KontrolliereMessungen.OBChannel.SC2DeltaK_100.ToString() + ";");
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

            if (KontrolliereMessungen.OCChannel.SignalAktiv == true)
            {
                line2 = (line2 + KontrolliereMessungen.OCChannel.SignalAktiv.ToString() + ";");
                line3 = (line3 + KontrolliereMessungen.OCChannel.Signalform + ";");
                line4 = (line4 + KontrolliereMessungen.OCChannel.MittlereHelligkeit_cdm2.ToString() + ";");
                line5 = (line5 + KontrolliereMessungen.OCChannel.Frequenz.ToString() + ";");
                line6 = (line6 + KontrolliereMessungen.OCChannel.Phasenverschiebung.ToString() + ";");
                line7 = (line7 + KontrolliereMessungen.OCChannel.KonSC1_100.ToString() + ";");
                line8 = (line8 + KontrolliereMessungen.OCChannel.KonSC2_100.ToString() + ";");
                line9 = (line9 + KontrolliereMessungen.OCChannel.SC1DeltaK_100.ToString() + ";");
                line10 = (line10 + KontrolliereMessungen.OCChannel.SC2DeltaK_100.ToString() + ";");
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

        void SignalEigenschaftenEinlesen()
        {
            IRChannel.SignalAktiv = cbAktivIR.Checked;
            if (cbAktivIR.Checked)
            {
                IRChannel.Signalform = cbSigFormIR.Text;
                IRChannel.Frequenz = int.Parse(tbFreqIR.Text);
                IRChannel.KonSC1_100 = double.Parse(tbKonIRSC1.Text);
                IRChannel.KonSC2_100 = double.Parse(tbKonIRSC2.Text);
                IRChannel.SC1DeltaK_100 = double.Parse(tbIRSC1DeltaK.Text);
                IRChannel.SC2DeltaK_100 = double.Parse(tbIRSC2DeltaK.Text);
                IRChannel.MittlereHelligkeit_cdm2 = double.Parse(tbMHIR.Text);
                IRChannel.Phasenverschiebung = int.Parse(tbPhasVerschIR.Text);
            }

            IGChannel.SignalAktiv = cbAktivIG.Checked;
            if (cbAktivIG.Checked)
            {
                IGChannel.Signalform = cbSigFormIG.Text;
                IGChannel.Frequenz = int.Parse(tbFreqIG.Text);
                IGChannel.KonSC1_100 = double.Parse(tbKonIGSC1.Text);
                IGChannel.KonSC2_100 = double.Parse(tbKonIGSC2.Text);
                IGChannel.SC1DeltaK_100 = double.Parse(tbIGSC1DeltaK.Text);
                IGChannel.SC2DeltaK_100 = double.Parse(tbIGSC2DeltaK.Text);
                IGChannel.MittlereHelligkeit_cdm2 = double.Parse(tbMHIG.Text);
                IGChannel.Phasenverschiebung = int.Parse(tbPhasVerschIG.Text);
            }

            IBChannel.SignalAktiv = cbAktivIB.Checked;
            if (cbAktivIB.Checked)
            {
                IBChannel.Signalform = cbSigFormIB.Text;
                IBChannel.Frequenz = int.Parse(tbFreqIB.Text);
                IBChannel.KonSC1_100 = double.Parse(tbKonIBSC1.Text);
                IBChannel.KonSC2_100 = double.Parse(tbKonIBSC2.Text);
                IBChannel.SC1DeltaK_100 = double.Parse(tbIBSC1DeltaK.Text);
                IBChannel.SC2DeltaK_100 = double.Parse(tbIBSC2DeltaK.Text);
                IBChannel.MittlereHelligkeit_cdm2 = double.Parse(tbMHIB.Text);
                IBChannel.Phasenverschiebung = int.Parse(tbPhasVerschIB.Text);
            }

            ICChannel.SignalAktiv = cbAktivIC.Checked;
            if (cbAktivIC.Checked)
            {
                ICChannel.Signalform = cbSigFormIC.Text;
                ICChannel.Frequenz = int.Parse(tbFreqIC.Text);
                ICChannel.KonSC1_100 = double.Parse(tbKonICSC1.Text);
                ICChannel.KonSC2_100 = double.Parse(tbKonICSC2.Text);
                ICChannel.SC1DeltaK_100 = double.Parse(tbICSC1DeltaK.Text);
                ICChannel.SC2DeltaK_100 = double.Parse(tbICSC2DeltaK.Text);
                ICChannel.MittlereHelligkeit_cdm2 = double.Parse(tbMHIC.Text);
                ICChannel.Phasenverschiebung = int.Parse(tbPhasVerschIC.Text);
            }

            ORChannel.SignalAktiv = cbAktivOR.Checked;
            if (cbAktivOR.Checked)
            {
                ORChannel.Signalform = cbSigFormOR.Text;
                ORChannel.Frequenz = int.Parse(tbFreqOR.Text);
                ORChannel.KonSC1_100 = double.Parse(tbKonORSC1.Text);
                ORChannel.KonSC2_100 = double.Parse(tbKonORSC2.Text);
                ORChannel.SC1DeltaK_100 = double.Parse(tbORSC1DeltaK.Text);
                ORChannel.SC2DeltaK_100 = double.Parse(tbORSC2DeltaK.Text);
                ORChannel.MittlereHelligkeit_cdm2 = double.Parse(tbMHOR.Text);
                ORChannel.Phasenverschiebung = int.Parse(tbPhasVerschOR.Text);
            }

            OGChannel.SignalAktiv = cbAktivOG.Checked;
            if (cbAktivOG.Checked)
            {
                OGChannel.Signalform = cbSigFormOG.Text;
                OGChannel.Frequenz = int.Parse(tbFreqOG.Text);
                OGChannel.KonSC1_100 = double.Parse(tbKonOGSC1.Text);
                OGChannel.KonSC2_100 = double.Parse(tbKonOGSC2.Text);
                OGChannel.SC1DeltaK_100 = double.Parse(tbOGSC1DeltaK.Text);
                OGChannel.SC2DeltaK_100 = double.Parse(tbOGSC2DeltaK.Text);
                OGChannel.MittlereHelligkeit_cdm2 = double.Parse(tbMHOG.Text);
                OGChannel.Phasenverschiebung = int.Parse(tbPhasVerschOG.Text);
            }

            OBChannel.SignalAktiv = cbAktivOB.Checked;
            if (cbAktivOB.Checked)
            {
                OBChannel.Signalform = cbSigFormOB.Text;
                OBChannel.Frequenz = int.Parse(tbFreqOB.Text);
                OBChannel.KonSC1_100 = double.Parse(tbKonOBSC1.Text);
                OBChannel.KonSC2_100 = double.Parse(tbKonOBSC2.Text);
                OBChannel.SC1DeltaK_100 = double.Parse(tbOBSC1DeltaK.Text);
                OBChannel.SC2DeltaK_100 = double.Parse(tbOBSC2DeltaK.Text);
                OBChannel.MittlereHelligkeit_cdm2 = double.Parse(tbMHOB.Text);
                OBChannel.Phasenverschiebung = int.Parse(tbPhasVerschOB.Text);
            }

            OCChannel.SignalAktiv = cbAktivOC.Checked;
            if (cbAktivOC.Checked)
            {
                OCChannel.Signalform = cbSigFormOC.Text;
                OCChannel.Frequenz = int.Parse(tbFreqOC.Text);
                OCChannel.KonSC1_100 = double.Parse(tbKonOCSC1.Text);
                OCChannel.KonSC2_100 = double.Parse(tbKonOCSC2.Text);
                OCChannel.SC1DeltaK_100 = double.Parse(tbOCSC1DeltaK.Text);
                OCChannel.SC2DeltaK_100 = double.Parse(tbOCSC2DeltaK.Text);
                OCChannel.MittlereHelligkeit_cdm2 = double.Parse(tbMHOC.Text);
                OCChannel.Phasenverschiebung = int.Parse(tbPhasVerschOC.Text);
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

                this.SignalEigenschaftenEinlesen();

                Globals.flagUntersuchunglaeuft = true;
                this.btnUntersuchungAbbrechen.Enabled = true;
                this.btnUntersuchungStarten.Enabled = false;
                this.tbUntersuchungsVerlauf.Clear();
                this.tbUntersuchungsVerlauf.Visible = true;
                this.tbProbandenNummer.Enabled = false;
                this.cbAugenseite.Enabled = false;
                this.KeyPreview = true;

                if ((IRChannel.SC1DeltaK_100 < float.Epsilon) &&
                    (IGChannel.SC1DeltaK_100 < float.Epsilon) &&
                    (Math.Abs(IBChannel.SC1DeltaK_100) < float.Epsilon) &&
                    (ICChannel.SC1DeltaK_100 < float.Epsilon))
                {
                    if (UseConstantStimuli) Strategie = new ConstantStimuli(mainProgram);
                    else if (UseBestPEST) Strategie = new BestPEST(mainProgram);
                    else stdStrategie = new StdStrategie(mainProgram, "außen", testeCFF);
                }
                else
                {
                    if (UseConstantStimuli) Strategie = new ConstantStimuli(mainProgram);
                    else if (UseBestPEST) Strategie = new BestPEST(mainProgram);
                    else stdStrategie = new StdStrategie(mainProgram, "innen", testeCFF);
                }
                if (UseConstantStimuli) Strategie.Abbruch += new EventHandler<AbbruchEventArgs>(stdStrategie_abbruch);
                else if (UseBestPEST) Strategie.Abbruch += new EventHandler<AbbruchEventArgs>(stdStrategie_abbruch);
                else stdStrategie.abbruch += new EventHandler<AbbruchEventArgs>(stdStrategie_abbruch);

                if (!testeCFF)
                {
                    if (freq > 0)
                    {
                        if (UseConstantStimuli) Strategie._setNewFrequency(freq);
                        else if (UseBestPEST) Strategie._setNewFrequency(freq);
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
                if (UseConstantStimuli | UseBestPEST)
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
                if (UseConstantStimuli | UseBestPEST)
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
            SignalEigenschaftenEinlesen();
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
                    temp = chan.SignalAktiv.ToString() + ";";
                    temp = temp + chan.Signalform + ";";
                    temp = temp + chan.Frequenz.ToString() + ";";
                    temp = temp + chan.MittlereHelligkeit_cdm2.ToString() + ";";
                    temp = temp + chan.Phasenverschiebung.ToString() + ";";
                    temp = temp + chan.KonSC1_100.ToString() + ";";
                    temp = temp + chan.SC1DeltaK_100.ToString() + ";";
                    temp = temp + chan.KonSC2_100.ToString() + ";";
                    temp = temp + chan.SC2DeltaK_100.ToString() + ";";
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
