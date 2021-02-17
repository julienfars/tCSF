using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Light4SightNG
{
    /// <summary>
    /// Diese Klasse berechnet die wave-Daten aus einer abstrakteren Beschreibung des Signals und schreibt sie in den wave containter der Klasse AudioControllClass.
    /// </summary>
    static class SignalGeneration
    {
        static double IR_MHLR, IR_KLR, IG_MHLR, IG_KLR, IB_MHLR, IB_KLR, IC_MHLR, IC_KLR;
        static double OR_MHLR, OR_KLR, OG_MHLR, OG_KLR, OB_MHLR, OB_KLR, OC_MHLR, OC_KLR;

        static double fEnvelope = 1;
        static double[] env;
        static bool _pauseEnvelope = true;

        static bool TIFC = false; // two interval forced-choice?
        static bool FI = true; // first Interval?

        public static void InitializeValues()
        {

            env = new double[AudioControlClass.AbtastFrequenz * AudioControlClass.SampleLaenge];

            Envelope = Convert.ToString(fEnvelope);

            Globals.Kanal_1_IR = new double[AudioControlClass.AbtastFrequenz];
            Globals.Kanal_2_IG = new double[AudioControlClass.AbtastFrequenz];
            Globals.Kanal_3_IB = new double[AudioControlClass.AbtastFrequenz];
            Globals.Kanal_4_IC = new double[AudioControlClass.AbtastFrequenz];
            Globals.Kanal_5_OR = new double[AudioControlClass.AbtastFrequenz];
            Globals.Kanal_6_OG = new double[AudioControlClass.AbtastFrequenz];
            Globals.Kanal_7_OB = new double[AudioControlClass.AbtastFrequenz];
            Globals.Kanal_8_OC = new double[AudioControlClass.AbtastFrequenz];
        }

        public static void Untersuchungssignal(bool twoIntervalForcedChoice = false, bool firstInterval = true)
        {
            #region IR Kanal

            TIFC = twoIntervalForcedChoice;
            FI = firstInterval;

            if (KontrolliereMessungen.IRChannel.SignalAktiv)
            {
                IR_MHLR = 32700 * KontrolliereMessungen.IRChannel.MittlereHelligkeit_100;
                IR_KLR = (IR_MHLR * (1 + KontrolliereMessungen.IRChannel.Kontrast_100 / 100)) - IR_MHLR;
                Globals.Kanal_1_IR = Sinus(IR_MHLR, IR_KLR, KontrolliereMessungen.IRChannel.Frequenz, KontrolliereMessungen.IRChannel.Phasenverschiebung);
            }

            #endregion

            #region IG Kanal
            if (KontrolliereMessungen.IGChannel.SignalAktiv)
            {
                IG_MHLR = 32700 * KontrolliereMessungen.IGChannel.MittlereHelligkeit_100;
                IG_KLR = (IG_MHLR * (1 + KontrolliereMessungen.IGChannel.Kontrast_100 / 100)) - IG_MHLR;

                Globals.Kanal_2_IG = Sinus(IG_MHLR, IG_KLR, KontrolliereMessungen.IGChannel.Frequenz, KontrolliereMessungen.IGChannel.Phasenverschiebung);

            }
            #endregion

            #region IB Kanal
            if (KontrolliereMessungen.IBChannel.SignalAktiv)
            {
                IB_MHLR = 32700 * KontrolliereMessungen.IBChannel.MittlereHelligkeit_100;
                IB_KLR = (IB_MHLR * (1 + KontrolliereMessungen.IBChannel.Kontrast_100 / 100)) - IB_MHLR;
                Globals.Kanal_3_IB = Sinus(IB_MHLR, IB_KLR, KontrolliereMessungen.IBChannel.Frequenz, KontrolliereMessungen.IBChannel.Phasenverschiebung);
            }

            #endregion

            #region IC Kanal
            if (KontrolliereMessungen.ICChannel.SignalAktiv)
            {
                IC_MHLR = 32700 * KontrolliereMessungen.ICChannel.MittlereHelligkeit_100;
                IC_KLR = (IC_MHLR * (1 + KontrolliereMessungen.ICChannel.Kontrast_100 / 100)) - IC_MHLR;
                Globals.Kanal_4_IC = Sinus(IC_MHLR, IC_KLR, KontrolliereMessungen.ICChannel.Frequenz, KontrolliereMessungen.ICChannel.Phasenverschiebung);
            }
            #endregion

            #region OR Kanal
            if (KontrolliereMessungen.ORChannel.SignalAktiv)
            {
                OR_MHLR = 32700 * KontrolliereMessungen.ORChannel.MittlereHelligkeit_100;
                OR_KLR = (OR_MHLR * (1 + KontrolliereMessungen.ORChannel.Kontrast_100 / 100)) - OR_MHLR;
                Globals.Kanal_5_OR = Sinus(OR_MHLR, OR_KLR, KontrolliereMessungen.ORChannel.Frequenz, KontrolliereMessungen.ORChannel.Phasenverschiebung);
            }
            #endregion

            #region OG Kanal
            if (KontrolliereMessungen.OGChannel.SignalAktiv)
            {
                OG_MHLR = 32700 * KontrolliereMessungen.OGChannel.MittlereHelligkeit_100;
                OG_KLR = (OG_MHLR * (1 + KontrolliereMessungen.OGChannel.Kontrast_100 / 100)) - OG_MHLR;
                Globals.Kanal_6_OG = Sinus(OG_MHLR, OG_KLR, KontrolliereMessungen.OGChannel.Frequenz, KontrolliereMessungen.OGChannel.Phasenverschiebung);
            }
            #endregion

            #region OB Kanal
            if (KontrolliereMessungen.OBChannel.SignalAktiv)
            {
                OB_MHLR = 32700 * KontrolliereMessungen.OBChannel.MittlereHelligkeit_100;
                OB_KLR = (OB_MHLR * (1 + KontrolliereMessungen.OBChannel.Kontrast_100 / 100)) - OB_MHLR;
                Globals.Kanal_7_OB = Sinus(OB_MHLR, OB_KLR, KontrolliereMessungen.OBChannel.Frequenz, KontrolliereMessungen.OBChannel.Phasenverschiebung);
            }
            #endregion

            #region OC Kanal
            if (KontrolliereMessungen.OCChannel.SignalAktiv)
            {
                OC_MHLR = 32700 * KontrolliereMessungen.OCChannel.MittlereHelligkeit_100;
                OC_KLR = (OC_MHLR * (1 + KontrolliereMessungen.OCChannel.Kontrast_100 / 100)) - OC_MHLR;
                Globals.Kanal_8_OC = Sinus(OC_MHLR, OC_KLR, KontrolliereMessungen.OCChannel.Frequenz, KontrolliereMessungen.OCChannel.Phasenverschiebung);
            }

            #endregion

            ConcatChannels();

        }

        static void ConcatChannels()
        {
            for (int i = 0; i <= ((AudioControlClass.AbtastFrequenz * AudioControlClass.SampleLaenge) - 1); i++)
            {
                WriteToWaveContainer(Globals.Kanal_1_IR[i], 0, i);
                WriteToWaveContainer(Globals.Kanal_2_IG[i], 1, i);
                WriteToWaveContainer(Globals.Kanal_3_IB[i], 2, i);
                WriteToWaveContainer(Globals.Kanal_4_IC[i], 3, i);
                WriteToWaveContainer(Globals.Kanal_5_OR[i], 4, i);
                WriteToWaveContainer(Globals.Kanal_6_OG[i], 5, i);
                WriteToWaveContainer(Globals.Kanal_7_OB[i], 6, i);
                WriteToWaveContainer(Globals.Kanal_8_OC[i], 7, i);
            }
        }

        static void WriteToWaveContainer(double dValue, int iChannel, int iPosition)
        {
            AudioControlClass.WaveDaten[(((iPosition * 8) + iChannel) * 2)] = AudioControlClass.Lowbyte((Int16)dValue);
            AudioControlClass.WaveDaten[(((iPosition * 8) + iChannel) * 2) + 1] = AudioControlClass.Highbyte((Int16)dValue);
        }

        static double[] Sinus(double MHLR, double KLR, int Frequenz, int Phasenwinkel)
        {
            int sinusLength = AudioControlClass.AbtastFrequenz * AudioControlClass.SampleLaenge;
            double[] TempSinus = new double[sinusLength];
            double dWinkel = 0.0;

            for (int i = 0; i <= (AudioControlClass.AbtastFrequenz * AudioControlClass.SampleLaenge) - 1; i++)
            {
                // main signal

                TempSinus[i] = ((MHLR + KLR * Math.Sin(Frequenz * AudioControlClass.DeltaPI * i + AudioControlClass.DeltaPhiSinus * Phasenwinkel)));

                // add envelope

                TempSinus[i] = (TempSinus[i] - MHLR) * env[i] + MHLR;

                // create two intervals

                if (TIFC)
                {
                    if (FI && i > (sinusLength / 2)) TempSinus[i] = MHLR;
                    if (!FI && i <= (sinusLength / 2)) TempSinus[i] = MHLR;
                    if (Math.Abs(i - (sinusLength / 2)) < 10) TempSinus[i] = 0;

                }

                // add carrier

                TempSinus[i] = TempSinus[i] * Math.Sin(dWinkel);

                dWinkel += 2 * Math.PI * AudioControlClass.TraegerFrequenz / AudioControlClass.AbtastFrequenz;
                if (dWinkel > 2 * Math.PI)
                    dWinkel -= 2 * Math.PI;


            }

            return TempSinus;
        }

        public static string Envelope
        {
            get { return Convert.ToString(fEnvelope); }
            set
            {
                switch (value)
                {
                    case "1/8":
                        fEnvelope = 0.125;
                        break;
                    case "1/4":
                        fEnvelope = 0.25;
                        break;
                    case "1/2":
                        fEnvelope = 0.5;
                        break;
                    case "1":
                        fEnvelope = 1.0;
                        break;
                    case "0":
                        fEnvelope = 0.0;
                        break;
                }
                for (int i = 0; i <= (AudioControlClass.AbtastFrequenz * AudioControlClass.SampleLaenge) - 1; i++)
                {
                    env[i] = Math.Cos(fEnvelope * AudioControlClass.DeltaPI * i);
                    if (_pauseEnvelope)
                        if (env[i] < 0) env[i] = 0;
                }
            }
        }

        public static bool PauseEnvelope
        {
            get { return _pauseEnvelope; }
            set
            {
                _pauseEnvelope = value;
                for (int i = 0; i <= (AudioControlClass.AbtastFrequenz * AudioControlClass.SampleLaenge) - 1; i++)
                {
                    env[i] = Math.Cos(fEnvelope * AudioControlClass.DeltaPI * i);
                    if (_pauseEnvelope)
                        if (env[i] < 0) env[i] = 0;
                }
            }
        }
    }
}
