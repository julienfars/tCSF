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

        AudioControlClass AudioControl;

        public event EventHandler<AbbruchEventArgs> abbruch;

        #region Initialisierung
        public StdStrategie(Steuerung strg, string LEDgruppe, bool CFFtest)
        {
            this.AudioControl = strg.AudioControl;
            LED_Gruppe = LEDgruppe;
            testeCFF = CFFtest;
        }

        public StdStrategie(Steuerung strg)
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
        protected virtual void OnAbbruch(AbbruchEventArgs e)
        {
            KontrolliereMessungen.cff = finalCFF;
            this.abbruch(this, e);
        }

        public void StartStdStrategie()
        {
            if (!testeCFF) InitValuesContrast(); else InitValuesCFF();

            #region Kontrastwerte der aktiven Kanäle einlesen
            if (KontrolliereMessungen.IRChannel.SignalAktiv)
            {
                Kontrast_100[0] = KontrolliereMessungen.IRChannel.KonSC1_100;
            }
            if (KontrolliereMessungen.IGChannel.SignalAktiv)
            {
                Kontrast_100[1] = KontrolliereMessungen.IGChannel.KonSC1_100;
            }
            if (KontrolliereMessungen.IBChannel.SignalAktiv)
            {
                Kontrast_100[2] = KontrolliereMessungen.IBChannel.KonSC1_100;
            }
            if (KontrolliereMessungen.ICChannel.SignalAktiv)
            {
                Kontrast_100[3] = KontrolliereMessungen.ICChannel.KonSC1_100;
            }
            if (KontrolliereMessungen.ORChannel.SignalAktiv)
            {
                Kontrast_100[4] = KontrolliereMessungen.ORChannel.KonSC1_100;
            }
            if (KontrolliereMessungen.OGChannel.SignalAktiv)
            {
                Kontrast_100[5] = KontrolliereMessungen.OGChannel.KonSC1_100;
            }
            if (KontrolliereMessungen.OBChannel.SignalAktiv)
            {
                Kontrast_100[6] = KontrolliereMessungen.OBChannel.KonSC1_100;
            }
            if (KontrolliereMessungen.OCChannel.SignalAktiv)
            {
                Kontrast_100[7] = KontrolliereMessungen.OCChannel.KonSC1_100;
            }
            #endregion

            if (LED_Gruppe == "innen")
            #region unterscheidung welche led_gruppe und entsprechnde index zuweisung
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
            #endregion
            Randomisierung();
        }
        #endregion

        #region Strategie
        void Randomisierung()
        {
            if (SC1_7 && SC2_7)
            {
                this.OnAbbruch(new AbbruchEventArgs(""));
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
                    this.OnAbbruch(new AbbruchEventArgs(""));
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
                    this.OnAbbruch(new AbbruchEventArgs(""));
                }

                if (SC1_7 == false)
                {
                    if (iDurchlaufSC1 == 1) SC1_gesehen_alt = true;
                    else SC1_gesehen_alt = SC1_gesehen;
                    SignalWiedergeben();
                    iDurchlaufSC1++;
                }
                else Randomisierung();

            }
            else
            {
                this.OnAbbruch(new AbbruchEventArgs(""));
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
                    this.OnAbbruch(new AbbruchEventArgs(""));
                }

                if (SC2_7 == false)
                {
                    if (iDurchlaufSC2 == 1) SC2_gesehen_alt = false;
                    else SC2_gesehen_alt = SC2_gesehen;
                    SignalWiedergeben();
                    iDurchlaufSC2++;
                }
                else Randomisierung();

            }
            else
            {
                this.OnAbbruch(new AbbruchEventArgs(""));
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
                    KontrolliereMessungen.IRChannel.Kontrast_100 = Kontrast_100[rot] * neuerFaktor / maxFaktor;
                    KontrolliereMessungen.IGChannel.Kontrast_100 = Kontrast_100[gruen] * neuerFaktor / maxFaktor;
                    KontrolliereMessungen.IBChannel.Kontrast_100 = Kontrast_100[blau] * neuerFaktor / maxFaktor;
                    KontrolliereMessungen.ICChannel.Kontrast_100 = Kontrast_100[cyan] * neuerFaktor / maxFaktor;
                    logKontraste = logmessage + KontrolliereMessungen.IRChannel.Kontrast_100 + ";" + KontrolliereMessungen.IGChannel.Kontrast_100 + ";" + KontrolliereMessungen.IBChannel.Kontrast_100 + ";" + KontrolliereMessungen.ICChannel.Kontrast_100;
                }
                else
                {
                    KontrolliereMessungen.ORChannel.Kontrast_100 = Kontrast_100[rot] * neuerFaktor / maxFaktor;
                    KontrolliereMessungen.OGChannel.Kontrast_100 = Kontrast_100[gruen] * neuerFaktor / maxFaktor;
                    KontrolliereMessungen.OBChannel.Kontrast_100 = Kontrast_100[blau] * neuerFaktor / maxFaktor;
                    KontrolliereMessungen.OCChannel.Kontrast_100 = Kontrast_100[cyan] * neuerFaktor / maxFaktor;
                    logKontraste = logmessage + KontrolliereMessungen.ORChannel.Kontrast_100 + ";" + KontrolliereMessungen.OGChannel.Kontrast_100 + ";" + KontrolliereMessungen.OBChannel.Kontrast_100 + ";" + KontrolliereMessungen.OCChannel.Kontrast_100;
                }
            }
            else
            {
                if (LED_Gruppe == "innen")
                {
                    KontrolliereMessungen.IRChannel.Kontrast_100 = Kontrast_100[rot];
                    KontrolliereMessungen.IGChannel.Kontrast_100 = Kontrast_100[gruen];
                    KontrolliereMessungen.IBChannel.Kontrast_100 = Kontrast_100[blau];
                    KontrolliereMessungen.ICChannel.Kontrast_100 = Kontrast_100[cyan];
                    FaktorCFF = (1 - Convert.ToDouble(neuerFaktor) / Convert.ToDouble(maxFaktor));
                    KontrolliereMessungen.IRChannel.Frequenz = Convert.ToInt16(Frequenz_100[rot] * FaktorCFF);
                    KontrolliereMessungen.IGChannel.Frequenz = Convert.ToInt16(Frequenz_100[gruen] * FaktorCFF);
                    KontrolliereMessungen.IBChannel.Frequenz = Convert.ToInt16(Frequenz_100[blau] * FaktorCFF);
                    KontrolliereMessungen.ICChannel.Frequenz = Convert.ToInt16(Frequenz_100[cyan] * FaktorCFF);
                    currentCFF = Convert.ToInt16(Frequenz_100[rot] * FaktorCFF);
                    logKontraste = logmessage + KontrolliereMessungen.IRChannel.Frequenz + ";" + KontrolliereMessungen.IGChannel.Frequenz + ";" + KontrolliereMessungen.IBChannel.Frequenz + ";" + KontrolliereMessungen.ICChannel.Frequenz;
                }
                else
                {
                    KontrolliereMessungen.ORChannel.Kontrast_100 = Kontrast_100[rot];
                    KontrolliereMessungen.OGChannel.Kontrast_100 = Kontrast_100[gruen];
                    KontrolliereMessungen.OBChannel.Kontrast_100 = Kontrast_100[blau];
                    KontrolliereMessungen.OCChannel.Kontrast_100 = Kontrast_100[cyan];
                    FaktorCFF = (1 - Convert.ToDouble(neuerFaktor) / Convert.ToDouble(maxFaktor));
                    KontrolliereMessungen.ORChannel.Frequenz = Convert.ToInt16(Frequenz_100[rot] * FaktorCFF);
                    KontrolliereMessungen.OGChannel.Frequenz = Convert.ToInt16(Frequenz_100[gruen] * FaktorCFF);
                    KontrolliereMessungen.OBChannel.Frequenz = Convert.ToInt16(Frequenz_100[blau] * FaktorCFF);
                    KontrolliereMessungen.OCChannel.Frequenz = Convert.ToInt16(Frequenz_100[cyan] * FaktorCFF);
                    currentCFF = Convert.ToInt16(Frequenz_100[rot] * FaktorCFF);
                    logKontraste = logmessage + KontrolliereMessungen.ORChannel.Frequenz + ";" + KontrolliereMessungen.OGChannel.Frequenz + ";" + KontrolliereMessungen.OBChannel.Frequenz + ";" + KontrolliereMessungen.OCChannel.Frequenz;
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
            KontrolliereMessungen.IRChannel.Frequenz = Frequenz;
            KontrolliereMessungen.IGChannel.Frequenz = Frequenz;
            KontrolliereMessungen.IBChannel.Frequenz = Frequenz;
            KontrolliereMessungen.ICChannel.Frequenz = Frequenz;
            KontrolliereMessungen.ORChannel.Frequenz = Frequenz;
            KontrolliereMessungen.OGChannel.Frequenz = Frequenz;
            KontrolliereMessungen.OBChannel.Frequenz = Frequenz;
            KontrolliereMessungen.OCChannel.Frequenz = Frequenz;
        }
        #endregion

        #region Signalsteuerung
        void SignalWiedergeben()
        {
            AudioControl.InitWaveContainer();
            SignalGeneration.Untersuchungssignal();
            AudioControl.PlaySignal();
        }

        public void SignalStoppen()
        {
            AudioControl.StopSignal();
        }
        #endregion

        #region Responses
        public void Gesehen_KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Y)
            {
                SignalStoppen();
                Thread.Sleep(10);
                if (SC1_aktiv == true)
                {
                    SC1_gesehen = true;
                    if (!testeCFF) Logmessage("Down:;gesehen;" + KontrolliereMessungen.IRChannel.Kontrast_100 + ";" + KontrolliereMessungen.IGChannel.Kontrast_100 + ";" + KontrolliereMessungen.IBChannel.Kontrast_100 + ";" + KontrolliereMessungen.ICChannel.Kontrast_100 + ";;" + KontrolliereMessungen.ORChannel.Kontrast_100 + ";" + KontrolliereMessungen.OGChannel.Kontrast_100 + ";" + KontrolliereMessungen.OBChannel.Kontrast_100 + ";" + KontrolliereMessungen.OCChannel.Kontrast_100, false);
                    else Logmessage("Down:;gesehen;" + KontrolliereMessungen.IRChannel.Frequenz + ";" + KontrolliereMessungen.IGChannel.Frequenz + ";" + KontrolliereMessungen.IBChannel.Frequenz + ";" + KontrolliereMessungen.ICChannel.Frequenz + ";;" + KontrolliereMessungen.ORChannel.Frequenz + ";" + KontrolliereMessungen.OGChannel.Frequenz + ";" + KontrolliereMessungen.OBChannel.Frequenz + ";" + KontrolliereMessungen.OCChannel.Frequenz, false);
                }
                else
                {
                    SC2_gesehen = true;
                    if (!testeCFF) Logmessage("Up:;gesehen;" + KontrolliereMessungen.IRChannel.Kontrast_100 + ";" + KontrolliereMessungen.IGChannel.Kontrast_100 + ";" + KontrolliereMessungen.IBChannel.Kontrast_100 + ";" + KontrolliereMessungen.ICChannel.Kontrast_100 + ";;" + KontrolliereMessungen.ORChannel.Kontrast_100 + ";" + KontrolliereMessungen.OGChannel.Kontrast_100 + ";" + KontrolliereMessungen.OBChannel.Kontrast_100 + ";" + KontrolliereMessungen.OCChannel.Kontrast_100, false);
                    else Logmessage("Up:;gesehen;" + KontrolliereMessungen.IRChannel.Frequenz + ";" + KontrolliereMessungen.IGChannel.Frequenz + ";" + KontrolliereMessungen.IBChannel.Frequenz + ";" + KontrolliereMessungen.ICChannel.Frequenz + ";;" + KontrolliereMessungen.ORChannel.Frequenz + ";" + KontrolliereMessungen.OGChannel.Frequenz + ";" + KontrolliereMessungen.OBChannel.Frequenz + ";" + KontrolliereMessungen.OCChannel.Frequenz, false);
                }
                Randomisierung();
            }
        }

        public void Nichtgesehen_KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.M)
            {
                Thread.Sleep(10);
                SignalStoppen();
                if (SC1_aktiv == true)
                {
                    SC1_gesehen = false;
                    if (!testeCFF) Logmessage("Down:;nicht gesehen;" + KontrolliereMessungen.IRChannel.Kontrast_100 + ";" + KontrolliereMessungen.IGChannel.Kontrast_100 + ";" + KontrolliereMessungen.IBChannel.Kontrast_100 + ";" + KontrolliereMessungen.ICChannel.Kontrast_100 + ";;" + KontrolliereMessungen.ORChannel.Kontrast_100 + ";" + KontrolliereMessungen.OGChannel.Kontrast_100 + ";" + KontrolliereMessungen.OBChannel.Kontrast_100 + ";" + KontrolliereMessungen.OCChannel.Kontrast_100, false);
                    else Logmessage("Down:;nicht gesehen;" + KontrolliereMessungen.IRChannel.Frequenz + ";" + KontrolliereMessungen.IGChannel.Frequenz + ";" + KontrolliereMessungen.IBChannel.Frequenz + ";" + KontrolliereMessungen.ICChannel.Frequenz + ";;" + KontrolliereMessungen.ORChannel.Frequenz + ";" + KontrolliereMessungen.OGChannel.Frequenz + ";" + KontrolliereMessungen.OBChannel.Frequenz + ";" + KontrolliereMessungen.OCChannel.Frequenz, false);
                }
                else
                {
                    SC2_gesehen = false;
                    if (!testeCFF) Logmessage("Up:;nicht gesehen;" + KontrolliereMessungen.IRChannel.Kontrast_100 + ";" + KontrolliereMessungen.IGChannel.Kontrast_100 + ";" + KontrolliereMessungen.IBChannel.Kontrast_100 + ";" + KontrolliereMessungen.ICChannel.Kontrast_100 + ";;" + KontrolliereMessungen.ORChannel.Kontrast_100 + ";" + KontrolliereMessungen.OGChannel.Kontrast_100 + ";" + KontrolliereMessungen.OBChannel.Kontrast_100 + ";" + KontrolliereMessungen.OCChannel.Kontrast_100, false);
                    else Logmessage("Up:;nicht gesehen;" + KontrolliereMessungen.IRChannel.Frequenz + ";" + KontrolliereMessungen.IGChannel.Frequenz + ";" + KontrolliereMessungen.IBChannel.Frequenz + ";" + KontrolliereMessungen.ICChannel.Frequenz + ";;" + KontrolliereMessungen.ORChannel.Frequenz + ";" + KontrolliereMessungen.OGChannel.Frequenz + ";" + KontrolliereMessungen.OBChannel.Frequenz + ";" + KontrolliereMessungen.OCChannel.Frequenz, false);
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
                if (frm is KontrolliereMessungen)
                {
                    (frm as KontrolliereMessungen).LogFile(text, header);
                }
            }
        }
        #endregion

    }
}
