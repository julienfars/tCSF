
using System;
using System.Threading;
using System.Windows.Forms;

namespace Light4SightNG
{
    /// <summary>
    /// Diese abstrakte Klasse dient als Vorlage für eine Teststrategie und implementiert alle wesentlichen Eigenschaften und Methoden.
    /// </summary>
    abstract class Teststrategie
    {
        protected AudioControlClass AudioControl;
        protected int signalStrength;
        protected int maxSignalStrength = 1000;
        protected double[] Kontrast_100 = new double[8];

        protected string LED_Gruppe = "außen";

        // protected bool AntwortGegeben;

        protected String _SchwelleErreichtMessage;

        public bool Gesehen { get; set; }

        int frequency = 20;

        int red, green, blue, cyan;

        public event EventHandler<AbbruchEventArgs> Abbruch;

        protected Teststrategie(Steuerung parent)
        {
            this.AudioControl = parent.AudioControl;
        }

        /// <summary>
        /// Call this method to start the strategy.
        /// </summary>
        public virtual void StarteStrategie()
        {
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

            if (LED_Gruppe == "innen")
            {
                red = 0;
                green = 1;
                blue = 2;
                cyan = 3;
            }
            else
            {
                red = 4;
                green = 5;
                blue = 6;
                cyan = 7;
            }

            ZeigeNeueSignalstaerke();
        }

        /// <summary>
        /// Diese Methode muss implementiert werden um die Signalstärke zu setzen und die Signalwiedergabe zu starten.
        /// </summary>
        protected abstract void ZeigeNeueSignalstaerke();
        // SignalStaerke setzen
        // SignalWiedergeben();

        /// <summary>
        /// Diese Methode muss implementiert werden um zu testen, ob ein Abbruchkriterien erfüllt ist.
        /// </summary>
        /// <returns></returns>
        protected abstract bool TesteAbbruch();

        protected virtual void OnAbbruch(AbbruchEventArgs e)
        {
            // log threshold
            if (LED_Gruppe == "innen")
            {
                Logmessage(_SchwelleErreichtMessage +
                KontrolliereMessungen.IRChannel.Kontrast_100 + ";" +
                KontrolliereMessungen.IGChannel.Kontrast_100 + ";" +
                KontrolliereMessungen.IBChannel.Kontrast_100 + ";" +
                KontrolliereMessungen.ICChannel.Kontrast_100 + ";",
                false
                );
            }
            else
            {
                Logmessage(_SchwelleErreichtMessage +
                KontrolliereMessungen.ORChannel.Kontrast_100 + ";" +
                KontrolliereMessungen.OGChannel.Kontrast_100 + ";" +
                KontrolliereMessungen.OBChannel.Kontrast_100 + ";" +
                KontrolliereMessungen.OCChannel.Kontrast_100 + ";",
                false
                );
            }

            this.Abbruch(this, e);
        }

        #region Signalsteuerung
        /// <summary>
        /// Startet das Signal in AudioControl. Dort wird eine Endlosschleife eingeleitet
        /// bis AudioControl.StopSignal() von einem exterenen Prozess aufgerufen wird.
        /// </summary>
        public void PlaySignal()
        {
            AudioControl.InitWaveContainer();
            SignalGeneration.Untersuchungssignal();
            AudioControl.PlaySignal();
        }

        public void StopSignal()
        {
            AudioControl.StopSignal();
        }
        #endregion

        protected bool ChangeContrast(int newSignalStrength)
        {
            if (newSignalStrength < 0) { return (false); }
            if (newSignalStrength > maxSignalStrength) { return (false); }

            if (LED_Gruppe == "innen")
            {
                KontrolliereMessungen.IRChannel.Kontrast_100 = Kontrast_100[red] * newSignalStrength / maxSignalStrength;
                KontrolliereMessungen.IGChannel.Kontrast_100 = Kontrast_100[green] * newSignalStrength / maxSignalStrength;
                KontrolliereMessungen.IBChannel.Kontrast_100 = Kontrast_100[blue] * newSignalStrength / maxSignalStrength;
                KontrolliereMessungen.ICChannel.Kontrast_100 = Kontrast_100[cyan] * newSignalStrength / maxSignalStrength;
            }
            else
            {
                KontrolliereMessungen.ORChannel.Kontrast_100 = Kontrast_100[red] * newSignalStrength / maxSignalStrength;
                KontrolliereMessungen.OGChannel.Kontrast_100 = Kontrast_100[green] * newSignalStrength / maxSignalStrength;
                KontrolliereMessungen.OBChannel.Kontrast_100 = Kontrast_100[blue] * newSignalStrength / maxSignalStrength;
                KontrolliereMessungen.OCChannel.Kontrast_100 = Kontrast_100[cyan] * newSignalStrength / maxSignalStrength;
            }

            return (true);
        }

        public void _setNewFrequency(int f)
        {
            frequency = f;
            _setNewFrequency();
        }

        void _setNewFrequency()
        {
            KontrolliereMessungen.IRChannel.Frequenz = frequency;
            KontrolliereMessungen.IGChannel.Frequenz = frequency;
            KontrolliereMessungen.IBChannel.Frequenz = frequency;
            KontrolliereMessungen.ICChannel.Frequenz = frequency;
            KontrolliereMessungen.ORChannel.Frequenz = frequency;
            KontrolliereMessungen.OGChannel.Frequenz = frequency;
            KontrolliereMessungen.OBChannel.Frequenz = frequency;
            KontrolliereMessungen.OCChannel.Frequenz = frequency;
        }

        #region Responses

        /// <summary>
        /// Stimulus wurde gesehen: Die Wiedergabe wird gestoppt, die Antwort ins Log geschrieben 
        /// und ZeigeNeueSignalstaerke zur Berechnung des neuen Stimulus, sowie zur Anzeige desselben gestartet.
        /// </summary>
        /// <param name="e"></param>
        public void Gesehen_KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Y)
            {
                StopSignal();
                Gesehen = true;
                Logmessage("Strategie:;gesehen;" + KontrolliereMessungen.IRChannel.Kontrast_100 + ";" +
                    KontrolliereMessungen.IGChannel.Kontrast_100 + ";" + KontrolliereMessungen.IBChannel.Kontrast_100 + ";" +
                    KontrolliereMessungen.ICChannel.Kontrast_100 + ";;" + KontrolliereMessungen.ORChannel.Kontrast_100 + ";" +
                    KontrolliereMessungen.OGChannel.Kontrast_100 + ";" + KontrolliereMessungen.OBChannel.Kontrast_100 + ";" +
                    KontrolliereMessungen.OCChannel.Kontrast_100, false);
            }
            ZeigeNeueSignalstaerke();
        }

        /// <summary>
        /// Stimulus wurde nicht gesehen: die Wiedergabe wird gestoppt, die Antwort ins Log geschrieben 
        /// und ZeigeNeueSignalstaerke zur Berechnung des neuen Stimulus, sowie zur Anzeige desselben gestartet.
        /// </summary>
        /// <param name="e"></param>
        public void Nichtgesehen_KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.M)
            {
                StopSignal();
                Gesehen = false;
                Logmessage("Strategie:;nicht gesehen;" + KontrolliereMessungen.IRChannel.Kontrast_100 + ";" +
                    KontrolliereMessungen.IGChannel.Kontrast_100 + ";" + KontrolliereMessungen.IBChannel.Kontrast_100 + ";" +
                    KontrolliereMessungen.ICChannel.Kontrast_100 + ";;" + KontrolliereMessungen.ORChannel.Kontrast_100 + ";" +
                    KontrolliereMessungen.OGChannel.Kontrast_100 + ";" + KontrolliereMessungen.OBChannel.Kontrast_100 + ";" +
                    KontrolliereMessungen.OCChannel.Kontrast_100, false);
            }
            ZeigeNeueSignalstaerke();
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
