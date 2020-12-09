using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Light4SightNG
{
    class StdStrategie
    {
        double[] Kontrast_100 = new double[8];
        double[] Frequenz_100 = new double[8];

        int maxFaktor = 40960;

        int faktorSC1;
        int faktorStepSC1;

        int faktorSC2;
        int faktorStepSC2;

        int iDurchlaufSC1 = 1, iDurchlaufSC2 = 1;
        int iSC1K100 = 0, iSC1K0 = 0, iSC2K100 = 0, iSC2K0 = 0;
        int rot, gruen, blau, cyan;

        bool SC1_gesehen_alt = false, SC2_gesehen_alt = false, SC1_gesehen = false, SC2_gesehen = false;
        bool SC1_aktiv = false, SC1_7 = false, SC2_7 = false;

        string LED_Gruppe = "außen";

        public bool testeCFF = true;

        int finalCFF = -1;
        int currentCFF = -1;

        int cDirsSC1 = 0;
        int cDirsSC2 = 0;

        string logKontraste;

        int Frequenz = 20;

        Random dRand = new Random();

        AudioControl AudioControl;

        public event EventHandler<AbbruchEventArgs> abbruch;

        #region Initialisierung
        public StdStrategie(MainForm strg, string LEDgruppe, bool CFFtest)
        {
            this.AudioControl = strg.AudioControl;
            LED_Gruppe = LEDgruppe;
            testeCFF = CFFtest;
        }

        public StdStrategie(MainForm strg)
        {
            this.AudioControl = strg.AudioControl;
        }

        void InitValuesCFF()
        {
            faktorSC1 = maxFaktor - maxFaktor / 128;
            faktorSC2 = 0;
            faktorStepSC1 = -faktorSC1 / 5;
            faktorStepSC2 = faktorSC1 / 5;
            Frequenz_100[0] = 100;
            Frequenz_100[1] = 100;
            Frequenz_100[2] = 100;
            Frequenz_100[3] = 100;
            Frequenz_100[4] = 100;
            Frequenz_100[5] = 100;
            Frequenz_100[6] = 100;
            Frequenz_100[7] = 100;

        }

        void InitValuesContrast()
        {
            faktorSC1 = maxFaktor;
            faktorStepSC1 = -maxFaktor / 5;
            faktorSC2 = 0;
            faktorStepSC2 = maxFaktor / 5;
        }
        #endregion

        #region Messung starten/beenden
        /// <summary>
        /// Handles the abort of the current strategy.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void AbortStrategyEventHandler(AbbruchEventArgs e)
        {
            MeasurementForm.cff = finalCFF;
            this.abbruch(this, e);
        }

        public void StartStdStrategie()
        {
            if (!testeCFF) InitValuesContrast(); else InitValuesCFF();

            for (int i = 0; i < 8; i++)
                if (MeasurementForm.Channels[i].IsActive)
                    Kontrast_100[i] = MeasurementForm.Channels[i].StartContrastDownStaircase;

            if (LED_Gruppe == "innen")
            {
                rot = 0;
                gruen = 1;
                blau = 2;
                cyan = 3;
            }
            else
            {
                rot = 4;
                gruen = 5;
                blau = 6;
                cyan = 7;
            }
            Randomisierung();
        }
        #endregion

        #region Strategie
        void Randomisierung()
        {
            if (SC1_7 && SC2_7)
            {
                this.AbortStrategyEventHandler(new AbbruchEventArgs(""));
            }

            if (Globals.Staircase == "up" && !SC2_7)
            {
                StaircaseUp();
                SC1_aktiv = false;
            }

            if (Globals.Staircase == "down" && !SC1_7)
            {
                StaircaseDown();
                SC1_aktiv = true;
            }

            if (Globals.Staircase == "beide")
            {
                if (SC1_7 && SC2_7)
                {
                    this.AbortStrategyEventHandler(new AbbruchEventArgs(""));
                }

                double tmpRand = dRand.NextDouble();

                if ((tmpRand >= 0.5 || SC2_7) && !SC1_7)
                {
                    SC1_aktiv = true;
                    StaircaseDown();//sc1
                }
                else if ((tmpRand < 0.5 || SC1_7) && !SC2_7)
                {
                    SC1_aktiv = false;
                    StaircaseUp();//sc2
                }
            }
        }

        void StaircaseDown()
        {
            bool bk7 = false;
            string logmessage = "Down: Schwelle erreicht!;;";
            int neuerFaktorSC1;

            if (iSC1K0 < 3 && iSC1K100 < 3)
            {
                #region Ausnahme für ersten Durchlauf
                if (iDurchlaufSC1 == 1)
                {
                    ChangeContrast(faktorSC1, logmessage);
                }
                #endregion

                #region Kontrastwert überprüfen und neuen Kontrast einstellen
                else
                {
                    #region Proband hat ein Flackern gesehen
                    if (this.SC1_gesehen == true)
                    {
                        #region Proband hat beim Mal davor auch ein Flackern gesehen
                        if (this.SC1_gesehen_alt == true)
                        {
                            neuerFaktorSC1 = faktorSC1 + faktorStepSC1;
                            if (ChangeContrast(neuerFaktorSC1, logmessage))
                            {
                                faktorSC1 = neuerFaktorSC1;
                            }
                            else
                            {
                                ChangeContrast(faktorSC1, logmessage);
                                iSC1K100++;
                                Logmessage("Versuch Überschreitung: " + Convert.ToString(iSC1K100), false);
                            }
                        }
                        #endregion
                        #region Proband hat beim letzten Mal kein Flackern gesehen
                        else
                        {
                            cDirsSC1++;
                            faktorStepSC1 = -faktorStepSC1 / 2;
                            neuerFaktorSC1 = faktorSC1 + faktorStepSC1;
                            if (ChangeContrast(neuerFaktorSC1, logmessage))
                            {
                                faktorSC1 = neuerFaktorSC1;
                                bk7 = TesteAbbruch(faktorSC1, faktorStepSC1, true);
                            }
                            else
                            {
                                ChangeContrast(faktorSC1, logmessage);
                                iSC1K100++;
                                Logmessage("Versuch Überschreitung: " + Convert.ToString(iSC1K100), false);
                            }
                        }
                        #endregion
                    }
                    #endregion
                    #region Proband hat KEIN Flackern gesehen
                    else
                    {
                        #region Proband hat beim Mal davor auch KEIN Flackern gesehen
                        //Flackern wurde beim letzten Mal NICHT gesehen (ab 2tem Durchlauf)
                        if (this.SC1_gesehen_alt == true)
                        {
                            cDirsSC1++;
                            faktorStepSC1 = -faktorStepSC1 / 2;
                            neuerFaktorSC1 = faktorSC1 + faktorStepSC1;
                            if (ChangeContrast(neuerFaktorSC1, logmessage))
                            {

                                faktorSC1 = neuerFaktorSC1;
                                bk7 = TesteAbbruch(faktorSC1, faktorStepSC1, true);
                            }
                            else
                            {
                                ChangeContrast(faktorSC1, logmessage);
                                iSC1K100++;
                                Logmessage("Versuch Überschreitung: " + Convert.ToString(iSC1K100), false);
                            }

                        }
                        #endregion

                        #region Proband hat beim letzten Mal Flackern gesehen
                        //Flackern wurde beim letzten Mal gesehen
                        else
                        {
                            neuerFaktorSC1 = faktorSC1 + faktorStepSC1;
                            if (ChangeContrast(neuerFaktorSC1, logmessage))
                            {
                                faktorSC1 = neuerFaktorSC1;
                            }
                            else
                            {
                                ChangeContrast(faktorSC1, logmessage);
                                iSC1K100++;
                                Logmessage("Versuch Überschreitung: " + Convert.ToString(iSC1K100), false);
                            }

                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion

                if (bk7) Logmessage(logKontraste, false);

                if (SC1_7 == true && SC2_7 == true)
                {
                    this.AbortStrategyEventHandler(new AbbruchEventArgs(""));
                }

                if (SC1_7 == false)
                {
                    if (iDurchlaufSC1 == 1) SC1_gesehen_alt = true;
                    else SC1_gesehen_alt = SC1_gesehen;
                    Start();
                    iDurchlaufSC1++;
                }
                else Randomisierung();

            }
            else
            {
                this.AbortStrategyEventHandler(new AbbruchEventArgs(""));
            }
        }

        void StaircaseUp()
        {
            bool bk7 = false;
            string logmessage = "Up: Schwelle erreicht!;;";
            int neuerFaktorSC2;

            if (iSC2K0 < 3 && iSC2K100 < 3)
            {
                #region Ausnahme für ersten Durchlauf
                if (iDurchlaufSC2 == 1)
                {
                    ChangeContrast(faktorSC2, logmessage);
                }
                #endregion

                #region Kontrastwert überprüfen und neuen Kontrast einstellen
                else
                {
                    #region Proband hat ein Flackern gesehen
                    if (this.SC2_gesehen == true)
                    {
                        #region Proband hat beim Mal davor auch ein Flackern gesehen
                        if (this.SC2_gesehen_alt == true)
                        {
                            neuerFaktorSC2 = faktorSC2 + faktorStepSC2;
                            if (ChangeContrast(neuerFaktorSC2, logmessage))
                            {
                                faktorSC2 = neuerFaktorSC2;
                            }
                            else
                            {
                                ChangeContrast(faktorSC2, logmessage);
                                iSC2K100++;
                                Logmessage("Versuch Überschreitung: " + Convert.ToString(iSC2K100), false);
                            }
                        }
                        #endregion
                        #region Proband hat beim letzten Mal kein Flackern gesehen
                        else
                        {
                            cDirsSC2++;
                            faktorStepSC2 = -faktorStepSC2 / 2;
                            neuerFaktorSC2 = faktorSC2 + faktorStepSC2;
                            if (ChangeContrast(neuerFaktorSC2, logmessage))
                            {
                                faktorSC2 = neuerFaktorSC2;
                                bk7 = TesteAbbruch(faktorSC2, faktorStepSC2, false);
                            }
                            else
                            {
                                ChangeContrast(faktorSC2, logmessage);
                                iSC2K100++;
                                Logmessage("Versuch Überschreitung: " + Convert.ToString(iSC2K100), false);
                            }
                        }
                        #endregion
                    }
                    #endregion
                    #region Proband hat KEIN Flackern gesehen
                    else
                    {
                        #region Proband hat beim Mal davor auch KEIN Flackern gesehen
                        //Flackern wurde beim letzten Mal NICHT gesehen (ab 2tem Durchlauf)
                        if (this.SC2_gesehen_alt == true)
                        {
                            cDirsSC2++;
                            faktorStepSC2 = -faktorStepSC2 / 2;
                            neuerFaktorSC2 = faktorSC2 + faktorStepSC2;
                            if (ChangeContrast(neuerFaktorSC2, logmessage))
                            {
                                faktorSC2 = neuerFaktorSC2;
                                bk7 = TesteAbbruch(faktorSC2, faktorStepSC2, false);
                            }
                            else
                            {
                                ChangeContrast(faktorSC2, logmessage);
                                iSC2K100++;
                                Logmessage("Versuch Überschreitung: " + Convert.ToString(iSC2K100), false);
                            }

                        }
                        #endregion

                        #region Proband hat beim letzten Mal Flackern gesehen
                        //Flackern wurde beim letzten Mal gesehen
                        else
                        {
                            neuerFaktorSC2 = faktorSC2 + faktorStepSC2;
                            if (ChangeContrast(neuerFaktorSC2, logmessage))
                            {
                                faktorSC2 = neuerFaktorSC2;
                            }
                            else
                            {
                                ChangeContrast(faktorSC2, logmessage);
                                iSC2K100++;
                                Logmessage("Versuch Überschreitung: " + Convert.ToString(iSC2K100), false);
                            }

                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion

                if (bk7) Logmessage(logKontraste, false);

                if (SC1_7 == true && SC2_7 == true)
                {
                    this.AbortStrategyEventHandler(new AbbruchEventArgs(""));
                }

                if (SC2_7 == false)
                {
                    if (iDurchlaufSC2 == 1) SC2_gesehen_alt = false;
                    else SC2_gesehen_alt = SC2_gesehen;
                    Start();
                    iDurchlaufSC2++;
                }
                else Randomisierung();

            }
            else
            {
                this.AbortStrategyEventHandler(new AbbruchEventArgs(""));
            }
        }

        bool TesteAbbruch(int faktor, int step, bool SC1)
        {
            double Kriterium;
            int cDirs;
            if (testeCFF)
            {
                faktor = maxFaktor - faktor;
                Kriterium = 15;
            }
            else
            {
                Kriterium = 7;
            }
            if (SC1) cDirs = cDirsSC1; else cDirs = cDirsSC2;

            if ((Math.Abs(step) < (faktor / Kriterium)) && cDirs > 2)
            {
                if (testeCFF)
                {
                    if (finalCFF == -1) finalCFF = currentCFF;
                    else finalCFF = (finalCFF + currentCFF) / 2;
                }
                if (SC1) SC1_7 = true; else SC2_7 = true;
                return (true);
            }
            return (false);
        }

        #endregion

        #region Signalparameter ändern

        bool ChangeContrast(int neuerFaktor, string logmessage)
        {
            double FaktorCFF;
            if (neuerFaktor < 0) { return (false); }
            if (neuerFaktor > maxFaktor) { return (false); }

            if (!testeCFF)
            {
                if (LED_Gruppe == "innen")
                {
                    MeasurementForm.IRChannel.CurrentContrast = Kontrast_100[rot] * neuerFaktor / maxFaktor;
                    MeasurementForm.IGChannel.CurrentContrast = Kontrast_100[gruen] * neuerFaktor / maxFaktor;
                    MeasurementForm.IBChannel.CurrentContrast = Kontrast_100[blau] * neuerFaktor / maxFaktor;
                    MeasurementForm.ICChannel.CurrentContrast = Kontrast_100[cyan] * neuerFaktor / maxFaktor;
                    logKontraste = logmessage + MeasurementForm.IRChannel.CurrentContrast + ";" + MeasurementForm.IGChannel.CurrentContrast + ";" + MeasurementForm.IBChannel.CurrentContrast + ";" + MeasurementForm.ICChannel.CurrentContrast;
                }
                else
                {
                    MeasurementForm.ORChannel.CurrentContrast = Kontrast_100[rot] * neuerFaktor / maxFaktor;
                    MeasurementForm.OGChannel.CurrentContrast = Kontrast_100[gruen] * neuerFaktor / maxFaktor;
                    MeasurementForm.OBChannel.CurrentContrast = Kontrast_100[blau] * neuerFaktor / maxFaktor;
                    MeasurementForm.OCChannel.CurrentContrast = Kontrast_100[cyan] * neuerFaktor / maxFaktor;
                    logKontraste = logmessage + MeasurementForm.ORChannel.CurrentContrast + ";" + MeasurementForm.OGChannel.CurrentContrast + ";" + MeasurementForm.OBChannel.CurrentContrast + ";" + MeasurementForm.OCChannel.CurrentContrast;
                }
            }
            else
            {
                if (LED_Gruppe == "innen")
                {
                    MeasurementForm.IRChannel.CurrentContrast = Kontrast_100[rot];
                    MeasurementForm.IGChannel.CurrentContrast = Kontrast_100[gruen];
                    MeasurementForm.IBChannel.CurrentContrast = Kontrast_100[blau];
                    MeasurementForm.ICChannel.CurrentContrast = Kontrast_100[cyan];
                    FaktorCFF = (1 - Convert.ToDouble(neuerFaktor) / Convert.ToDouble(maxFaktor));
                    MeasurementForm.IRChannel.Frequency = Convert.ToInt16(Frequenz_100[rot] * FaktorCFF);
                    MeasurementForm.IGChannel.Frequency = Convert.ToInt16(Frequenz_100[gruen] * FaktorCFF);
                    MeasurementForm.IBChannel.Frequency = Convert.ToInt16(Frequenz_100[blau] * FaktorCFF);
                    MeasurementForm.ICChannel.Frequency = Convert.ToInt16(Frequenz_100[cyan] * FaktorCFF);
                    currentCFF = Convert.ToInt16(Frequenz_100[rot] * FaktorCFF);
                    logKontraste = logmessage + MeasurementForm.IRChannel.Frequency + ";" + MeasurementForm.IGChannel.Frequency + ";" + MeasurementForm.IBChannel.Frequency + ";" + MeasurementForm.ICChannel.Frequency;
                }
                else
                {
                    MeasurementForm.ORChannel.CurrentContrast = Kontrast_100[rot];
                    MeasurementForm.OGChannel.CurrentContrast = Kontrast_100[gruen];
                    MeasurementForm.OBChannel.CurrentContrast = Kontrast_100[blau];
                    MeasurementForm.OCChannel.CurrentContrast = Kontrast_100[cyan];
                    FaktorCFF = (1 - Convert.ToDouble(neuerFaktor) / Convert.ToDouble(maxFaktor));
                    MeasurementForm.ORChannel.Frequency = Convert.ToInt16(Frequenz_100[rot] * FaktorCFF);
                    MeasurementForm.OGChannel.Frequency = Convert.ToInt16(Frequenz_100[gruen] * FaktorCFF);
                    MeasurementForm.OBChannel.Frequency = Convert.ToInt16(Frequenz_100[blau] * FaktorCFF);
                    MeasurementForm.OCChannel.Frequency = Convert.ToInt16(Frequenz_100[cyan] * FaktorCFF);
                    currentCFF = Convert.ToInt16(Frequenz_100[rot] * FaktorCFF);
                    logKontraste = logmessage + MeasurementForm.ORChannel.Frequency + ";" + MeasurementForm.OGChannel.Frequency + ";" + MeasurementForm.OBChannel.Frequency + ";" + MeasurementForm.OCChannel.Frequency;
                }

            }
            return (true);
        }

        public void _setNewFrequency(int f)
        {
            Frequenz = f;
            _setNewFrequency();
        }

        void _setNewFrequency()
        {
            foreach (ChannelDescription c in MeasurementForm.Channels)
                c.Frequency = Frequenz;
        }
        #endregion

        #region Signalsteuerung
        /// <summary>
        /// Creates wave description and starts the audio signal.
        /// </summary>
        void Start()
        {
            AudioControl.InitWaveContainer();
            SignalGeneration.CreateSignalWave();
            AudioControl.PlaySignal();
        }

        /// <summary>
        /// Stops the audio signal.
        /// </summary>
        public void Stop()
        {
            AudioControl.StopSignal();
        }
        #endregion

        #region Responses
        public void Gesehen_KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Y)
            {
                Stop();
                Thread.Sleep(10);
                if (SC1_aktiv == true)
                {
                    SC1_gesehen = true;
                    if (!testeCFF) Logmessage("Down:;gesehen;" + MeasurementForm.IRChannel.CurrentContrast + ";" + MeasurementForm.IGChannel.CurrentContrast + ";" + MeasurementForm.IBChannel.CurrentContrast + ";" + MeasurementForm.ICChannel.CurrentContrast + ";;" + MeasurementForm.ORChannel.CurrentContrast + ";" + MeasurementForm.OGChannel.CurrentContrast + ";" + MeasurementForm.OBChannel.CurrentContrast + ";" + MeasurementForm.OCChannel.CurrentContrast, false);
                    else Logmessage("Down:;gesehen;" + MeasurementForm.IRChannel.Frequency + ";" + MeasurementForm.IGChannel.Frequency + ";" + MeasurementForm.IBChannel.Frequency + ";" + MeasurementForm.ICChannel.Frequency + ";;" + MeasurementForm.ORChannel.Frequency + ";" + MeasurementForm.OGChannel.Frequency + ";" + MeasurementForm.OBChannel.Frequency + ";" + MeasurementForm.OCChannel.Frequency, false);
                }
                else
                {
                    SC2_gesehen = true;
                    if (!testeCFF) Logmessage("Up:;gesehen;" + MeasurementForm.IRChannel.CurrentContrast + ";" + MeasurementForm.IGChannel.CurrentContrast + ";" + MeasurementForm.IBChannel.CurrentContrast + ";" + MeasurementForm.ICChannel.CurrentContrast + ";;" + MeasurementForm.ORChannel.CurrentContrast + ";" + MeasurementForm.OGChannel.CurrentContrast + ";" + MeasurementForm.OBChannel.CurrentContrast + ";" + MeasurementForm.OCChannel.CurrentContrast, false);
                    else Logmessage("Up:;gesehen;" + MeasurementForm.IRChannel.Frequency + ";" + MeasurementForm.IGChannel.Frequency + ";" + MeasurementForm.IBChannel.Frequency + ";" + MeasurementForm.ICChannel.Frequency + ";;" + MeasurementForm.ORChannel.Frequency + ";" + MeasurementForm.OGChannel.Frequency + ";" + MeasurementForm.OBChannel.Frequency + ";" + MeasurementForm.OCChannel.Frequency, false);
                }
                Randomisierung();
            }
        }

        public void Nichtgesehen_KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.M)
            {
                Thread.Sleep(10);
                Stop();
                if (SC1_aktiv == true)
                {
                    SC1_gesehen = false;
                    if (!testeCFF) Logmessage("Down:;nicht gesehen;" + MeasurementForm.IRChannel.CurrentContrast + ";" + MeasurementForm.IGChannel.CurrentContrast + ";" + MeasurementForm.IBChannel.CurrentContrast + ";" + MeasurementForm.ICChannel.CurrentContrast + ";;" + MeasurementForm.ORChannel.CurrentContrast + ";" + MeasurementForm.OGChannel.CurrentContrast + ";" + MeasurementForm.OBChannel.CurrentContrast + ";" + MeasurementForm.OCChannel.CurrentContrast, false);
                    else Logmessage("Down:;nicht gesehen;" + MeasurementForm.IRChannel.Frequency + ";" + MeasurementForm.IGChannel.Frequency + ";" + MeasurementForm.IBChannel.Frequency + ";" + MeasurementForm.ICChannel.Frequency + ";;" + MeasurementForm.ORChannel.Frequency + ";" + MeasurementForm.OGChannel.Frequency + ";" + MeasurementForm.OBChannel.Frequency + ";" + MeasurementForm.OCChannel.Frequency, false);
                }
                else
                {
                    SC2_gesehen = false;
                    if (!testeCFF) Logmessage("Up:;nicht gesehen;" + MeasurementForm.IRChannel.CurrentContrast + ";" + MeasurementForm.IGChannel.CurrentContrast + ";" + MeasurementForm.IBChannel.CurrentContrast + ";" + MeasurementForm.ICChannel.CurrentContrast + ";;" + MeasurementForm.ORChannel.CurrentContrast + ";" + MeasurementForm.OGChannel.CurrentContrast + ";" + MeasurementForm.OBChannel.CurrentContrast + ";" + MeasurementForm.OCChannel.CurrentContrast, false);
                    else Logmessage("Up:;nicht gesehen;" + MeasurementForm.IRChannel.Frequency + ";" + MeasurementForm.IGChannel.Frequency + ";" + MeasurementForm.IBChannel.Frequency + ";" + MeasurementForm.ICChannel.Frequency + ";;" + MeasurementForm.ORChannel.Frequency + ";" + MeasurementForm.OGChannel.Frequency + ";" + MeasurementForm.OBChannel.Frequency + ";" + MeasurementForm.OCChannel.Frequency, false);
                }
                Randomisierung();
            }
        }
        #endregion

        #region Logging
        void Logmessage(string text, bool header)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is MeasurementForm)
                {
                    (frm as MeasurementForm).LogFile(text, header);
                }
            }
        }
        #endregion

    }
}
