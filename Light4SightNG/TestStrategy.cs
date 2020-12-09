
using System;
using System.Threading;
using System.Windows.Forms;

namespace Light4SightNG
{
    /// <summary>
    /// Diese abstrakte Klasse dient als Vorlage für eine Teststrategie und implementiert alle wesentlichen Eigenschaften und Methoden.
    /// </summary>
    abstract class TestStrategy
    {
        protected AudioControl AudioControl;
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

        protected TestStrategy(MainForm parent)
        {
            this.AudioControl = parent.AudioControl;
        }

        /// <summary>
        /// Call this method to start the strategy.
        /// </summary>
        public virtual void StarteStrategie()
        {
            if (MeasurementForm.IRChannel.IsActive)
            {
                Kontrast_100[0] = MeasurementForm.IRChannel.StartContrastDownStaircase;
            }
            if (MeasurementForm.IGChannel.IsActive)
            {
                Kontrast_100[1] = MeasurementForm.IGChannel.StartContrastDownStaircase;
            }
            if (MeasurementForm.IBChannel.IsActive)
            {
                Kontrast_100[2] = MeasurementForm.IBChannel.StartContrastDownStaircase;
            }
            if (MeasurementForm.ICChannel.IsActive)
            {
                Kontrast_100[3] = MeasurementForm.ICChannel.StartContrastDownStaircase;
            }
            if (MeasurementForm.ORChannel.IsActive)
            {
                Kontrast_100[4] = MeasurementForm.ORChannel.StartContrastDownStaircase;
            }
            if (MeasurementForm.OGChannel.IsActive)
            {
                Kontrast_100[5] = MeasurementForm.OGChannel.StartContrastDownStaircase;
            }
            if (MeasurementForm.OBChannel.IsActive)
            {
                Kontrast_100[6] = MeasurementForm.OBChannel.StartContrastDownStaircase;
            }
            if (MeasurementForm.OCChannel.IsActive)
            {
                Kontrast_100[7] = MeasurementForm.OCChannel.StartContrastDownStaircase;
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

            PresentNextStimulus();
        }

        /// <summary>
        /// Diese Methode muss implementiert werden um die Signalstärke zu setzen und die Signalwiedergabe zu starten.
        /// </summary>
        protected abstract void PresentNextStimulus();
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
                MeasurementForm.IRChannel.CurrentContrast + ";" +
                MeasurementForm.IGChannel.CurrentContrast + ";" +
                MeasurementForm.IBChannel.CurrentContrast + ";" +
                MeasurementForm.ICChannel.CurrentContrast + ";",
                false
                );
            }
            else
            {
                Logmessage(_SchwelleErreichtMessage +
                MeasurementForm.ORChannel.CurrentContrast + ";" +
                MeasurementForm.OGChannel.CurrentContrast + ";" +
                MeasurementForm.OBChannel.CurrentContrast + ";" +
                MeasurementForm.OCChannel.CurrentContrast + ";",
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
                MeasurementForm.IRChannel.CurrentContrast = Kontrast_100[red] * newSignalStrength / maxSignalStrength;
                MeasurementForm.IGChannel.CurrentContrast = Kontrast_100[green] * newSignalStrength / maxSignalStrength;
                MeasurementForm.IBChannel.CurrentContrast = Kontrast_100[blue] * newSignalStrength / maxSignalStrength;
                MeasurementForm.ICChannel.CurrentContrast = Kontrast_100[cyan] * newSignalStrength / maxSignalStrength;
            }
            else
            {
                MeasurementForm.ORChannel.CurrentContrast = Kontrast_100[red] * newSignalStrength / maxSignalStrength;
                MeasurementForm.OGChannel.CurrentContrast = Kontrast_100[green] * newSignalStrength / maxSignalStrength;
                MeasurementForm.OBChannel.CurrentContrast = Kontrast_100[blue] * newSignalStrength / maxSignalStrength;
                MeasurementForm.OCChannel.CurrentContrast = Kontrast_100[cyan] * newSignalStrength / maxSignalStrength;
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
            MeasurementForm.IRChannel.Frequency = frequency;
            MeasurementForm.IGChannel.Frequency = frequency;
            MeasurementForm.IBChannel.Frequency = frequency;
            MeasurementForm.ICChannel.Frequency = frequency;
            MeasurementForm.ORChannel.Frequency = frequency;
            MeasurementForm.OGChannel.Frequency = frequency;
            MeasurementForm.OBChannel.Frequency = frequency;
            MeasurementForm.OCChannel.Frequency = frequency;
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
                Logmessage("Strategie:;gesehen;" + MeasurementForm.IRChannel.CurrentContrast + ";" +
                    MeasurementForm.IGChannel.CurrentContrast + ";" + MeasurementForm.IBChannel.CurrentContrast + ";" +
                    MeasurementForm.ICChannel.CurrentContrast + ";;" + MeasurementForm.ORChannel.CurrentContrast + ";" +
                    MeasurementForm.OGChannel.CurrentContrast + ";" + MeasurementForm.OBChannel.CurrentContrast + ";" +
                    MeasurementForm.OCChannel.CurrentContrast, false);
            }
            PresentNextStimulus();
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
                Logmessage("Strategie:;nicht gesehen;" + MeasurementForm.IRChannel.CurrentContrast + ";" +
                    MeasurementForm.IGChannel.CurrentContrast + ";" + MeasurementForm.IBChannel.CurrentContrast + ";" +
                    MeasurementForm.ICChannel.CurrentContrast + ";;" + MeasurementForm.ORChannel.CurrentContrast + ";" +
                    MeasurementForm.OGChannel.CurrentContrast + ";" + MeasurementForm.OBChannel.CurrentContrast + ";" +
                    MeasurementForm.OCChannel.CurrentContrast, false);
            }
            PresentNextStimulus();
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
