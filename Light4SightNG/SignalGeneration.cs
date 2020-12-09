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

        public static void InitializeValues()
        {

            env = new double[AudioControl.AbtastFrequenz * AudioControl.SampleLaenge];

            Envelope = Convert.ToString(fEnvelope);

            Globals.Kanal_1_IR = new double[AudioControl.AbtastFrequenz];
            Globals.Kanal_2_IG = new double[AudioControl.AbtastFrequenz];
            Globals.Kanal_3_IB = new double[AudioControl.AbtastFrequenz];
            Globals.Kanal_4_IC = new double[AudioControl.AbtastFrequenz];
            Globals.Kanal_5_OR = new double[AudioControl.AbtastFrequenz];
            Globals.Kanal_6_OG = new double[AudioControl.AbtastFrequenz];
            Globals.Kanal_7_OB = new double[AudioControl.AbtastFrequenz];
            Globals.Kanal_8_OC = new double[AudioControl.AbtastFrequenz];
        }

        public static void Untersuchungssignal()
        {
            #region IR Kanal

            if (MeasurementForm.IRChannel.IsActive)
            {
                IR_MHLR = 32700 * MeasurementForm.IRChannel.PercentMeanIntensity;
                IR_KLR = (IR_MHLR * (1 + MeasurementForm.IRChannel.CurrentContrast / 100)) - IR_MHLR;
                Globals.Kanal_1_IR = Sinus(IR_MHLR, IR_KLR, MeasurementForm.IRChannel.Frequency, MeasurementForm.IRChannel.GetPhase());
            }

            #endregion

            #region IG Kanal
            if (MeasurementForm.IGChannel.IsActive)
            {
                IG_MHLR = 32700 * MeasurementForm.IGChannel.PercentMeanIntensity;
                IG_KLR = (IG_MHLR * (1 + MeasurementForm.IGChannel.CurrentContrast / 100)) - IG_MHLR;

                Globals.Kanal_2_IG = Sinus(IG_MHLR, IG_KLR, MeasurementForm.IGChannel.Frequency, MeasurementForm.IGChannel.GetPhase());

            }
            #endregion

            #region IB Kanal
            if (MeasurementForm.IBChannel.IsActive)
            {
                IB_MHLR = 32700 * MeasurementForm.IBChannel.PercentMeanIntensity;
                IB_KLR = (IB_MHLR * (1 + MeasurementForm.IBChannel.CurrentContrast / 100)) - IB_MHLR;
                Globals.Kanal_3_IB = Sinus(IB_MHLR, IB_KLR, MeasurementForm.IBChannel.Frequency, MeasurementForm.IBChannel.GetPhase());
            }

            #endregion

            #region IC Kanal
            if (MeasurementForm.ICChannel.IsActive)
            {
                IC_MHLR = 32700 * MeasurementForm.ICChannel.PercentMeanIntensity;
                IC_KLR = (IC_MHLR * (1 + MeasurementForm.ICChannel.CurrentContrast / 100)) - IC_MHLR;
                Globals.Kanal_4_IC = Sinus(IC_MHLR, IC_KLR, MeasurementForm.ICChannel.Frequency, MeasurementForm.ICChannel.GetPhase());
            }
            #endregion

            #region OR Kanal
            if (MeasurementForm.ORChannel.IsActive)
            {
                OR_MHLR = 32700 * MeasurementForm.ORChannel.PercentMeanIntensity;
                OR_KLR = (OR_MHLR * (1 + MeasurementForm.ORChannel.CurrentContrast / 100)) - OR_MHLR;
                Globals.Kanal_5_OR = Sinus(OR_MHLR, OR_KLR, MeasurementForm.ORChannel.Frequency, MeasurementForm.ORChannel.GetPhase());
            }
            #endregion

            #region OG Kanal
            if (MeasurementForm.OGChannel.IsActive)
            {
                OG_MHLR = 32700 * MeasurementForm.OGChannel.PercentMeanIntensity;
                OG_KLR = (OG_MHLR * (1 + MeasurementForm.OGChannel.CurrentContrast / 100)) - OG_MHLR;
                Globals.Kanal_6_OG = Sinus(OG_MHLR, OG_KLR, MeasurementForm.OGChannel.Frequency, MeasurementForm.OGChannel.GetPhase());
            }
            #endregion

            #region OB Kanal
            if (MeasurementForm.OBChannel.IsActive)
            {
                OB_MHLR = 32700 * MeasurementForm.OBChannel.PercentMeanIntensity;
                OB_KLR = (OB_MHLR * (1 + MeasurementForm.OBChannel.CurrentContrast / 100)) - OB_MHLR;
                Globals.Kanal_7_OB = Sinus(OB_MHLR, OB_KLR, MeasurementForm.OBChannel.Frequency, MeasurementForm.OBChannel.GetPhase());
            }
            #endregion

            #region OC Kanal
            if (MeasurementForm.OCChannel.IsActive)
            {
                OC_MHLR = 32700 * MeasurementForm.OCChannel.PercentMeanIntensity;
                OC_KLR = (OC_MHLR * (1 + MeasurementForm.OCChannel.CurrentContrast / 100)) - OC_MHLR;
                Globals.Kanal_8_OC = Sinus(OC_MHLR, OC_KLR, MeasurementForm.OCChannel.Frequency, MeasurementForm.OCChannel.GetPhase());
            }

            #endregion

            ConcatChannels();

        }

        static void ConcatChannels()
        {
            for (int i = 0; i <= ((AudioControl.AbtastFrequenz * AudioControl.SampleLaenge) - 1); i++)
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
            AudioControl.WaveDaten[(((iPosition * 8) + iChannel) * 2)] = AudioControl.Lowbyte((Int16)dValue);
            AudioControl.WaveDaten[(((iPosition * 8) + iChannel) * 2) + 1] = AudioControl.Highbyte((Int16)dValue);
        }

        static double[] Sinus(double MHLR, double KLR, int Frequenz, int Phasenwinkel)
        {
            double[] TempSinus = new double[AudioControl.AbtastFrequenz * AudioControl.SampleLaenge];
            double dWinkel = 0.0;

            for (int i = 0; i <= (AudioControl.AbtastFrequenz * AudioControl.SampleLaenge) - 1; i++)
            {
                // main signal

                TempSinus[i] = ((MHLR + KLR * Math.Sin(Frequenz * AudioControl.DeltaPI * i + AudioControl.DeltaPhiSinus * Phasenwinkel)));

                // add envelope

                TempSinus[i] = (TempSinus[i] - MHLR) * env[i] + MHLR;

                // add carrier

                TempSinus[i] = TempSinus[i] * Math.Sin(dWinkel);

                dWinkel += 2 * Math.PI * AudioControl.TraegerFrequenz / AudioControl.AbtastFrequenz;
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
                for (int i = 0; i <= (AudioControl.AbtastFrequenz * AudioControl.SampleLaenge) - 1; i++)
                {
                    env[i] = Math.Cos(fEnvelope * AudioControl.DeltaPI * i);
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
                for (int i = 0; i <= (AudioControl.AbtastFrequenz * AudioControl.SampleLaenge) - 1; i++)
                {
                    env[i] = Math.Cos(fEnvelope * AudioControl.DeltaPI * i);
                    if (_pauseEnvelope)
                        if (env[i] < 0) env[i] = 0;
                }
            }
        }
    }
}
