using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Light4SightNG
{
    /// <summary>
    /// This static class is used for calculating wave data from the ChannelDescriptions and writes it to the Audio-Control wave container.
    /// </summary>
    static class SignalGeneration
    {
        static double fEnvelope = 1;
        static double[] env;
        static bool _pauseEnvelope = true;

        public static double[] Kanal_1_IR;
        public static double[] Kanal_2_IG;
        public static double[] Kanal_3_IB;
        public static double[] Kanal_4_IC;
        public static double[] Kanal_5_OR;
        public static double[] Kanal_6_OG;
        public static double[] Kanal_7_OB;
        public static double[] Kanal_8_OC;
        public static double[][] ChannelFunctions;

        public static void InitializeValues()
        {

            env = new double[AudioControl.ScanRate * AudioControl.SampleLength];

            Envelope = Convert.ToString(fEnvelope);

            Kanal_1_IR = new double[AudioControl.ScanRate];
            Kanal_3_IB = new double[AudioControl.ScanRate];
            Kanal_4_IC = new double[AudioControl.ScanRate];
            Kanal_5_OR = new double[AudioControl.ScanRate];
            Kanal_6_OG = new double[AudioControl.ScanRate];
            Kanal_7_OB = new double[AudioControl.ScanRate];
            Kanal_8_OC = new double[AudioControl.ScanRate];
            ChannelFunctions = new double[][] { Kanal_1_IR, Kanal_2_IG, Kanal_3_IB, Kanal_4_IC,
                                                Kanal_5_OR, Kanal_6_OG, Kanal_7_OB, Kanal_8_OC };
        }

        /// <summary>
        /// Converts the current channel description and writes it to wave container.
        /// </summary>
        public static void CreateSignalWave()
        {
            for (int i = 0; i < 8; i++)
            {
                ChannelDescription c = MeasurementForm.Channels[i];
                if(c.IsActive)
                {
                    double MHLR = 32700 * c.PercentMeanIntensity;
                    double KLR = (MHLR * (1 + c.CurrentContrast / 100)) - MHLR;
                    ChannelFunctions[i] = Sinus(MHLR, KLR, c.Frequency, c.GetPhase());
                }
            }

            for (int i = 0; i <= ((AudioControl.ScanRate * AudioControl.SampleLength) - 1); i++)
            {
                WriteToWaveContainer(Kanal_1_IR[i], 0, i);
                WriteToWaveContainer(Kanal_2_IG[i], 1, i);
                WriteToWaveContainer(Kanal_3_IB[i], 2, i);
                WriteToWaveContainer(Kanal_4_IC[i], 3, i);
                WriteToWaveContainer(Kanal_5_OR[i], 4, i);
                WriteToWaveContainer(Kanal_6_OG[i], 5, i);
                WriteToWaveContainer(Kanal_7_OB[i], 6, i);
                WriteToWaveContainer(Kanal_8_OC[i], 7, i);
            }
        }

        static void WriteToWaveContainer(double dValue, int iChannel, int iPosition)
        {
            AudioControl.WaveDaten[(((iPosition * 8) + iChannel) * 2)] = AudioControl.Lowbyte((Int16)dValue);
            AudioControl.WaveDaten[(((iPosition * 8) + iChannel) * 2) + 1] = AudioControl.Highbyte((Int16)dValue);
        }

        static double[] Sinus(double MHLR, double KLR, int Frequenz, int Phasenwinkel)
        {
            double[] TempSinus = new double[AudioControl.ScanRate * AudioControl.SampleLength];
            double dWinkel = 0.0;

            for (int i = 0; i <= (AudioControl.ScanRate * AudioControl.SampleLength) - 1; i++)
            {
                // main signal

                TempSinus[i] = ((MHLR + KLR * Math.Sin(Frequenz * AudioControl.DeltaPI * i + AudioControl.DeltaPhiSinus * Phasenwinkel)));

                // add envelope

                TempSinus[i] = (TempSinus[i] - MHLR) * env[i] + MHLR;

                // add carrier

                TempSinus[i] = TempSinus[i] * Math.Sin(dWinkel);

                dWinkel += 2 * Math.PI * AudioControl.TraegerFrequenz / AudioControl.ScanRate;
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
                for (int i = 0; i <= (AudioControl.ScanRate * AudioControl.SampleLength) - 1; i++)
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
                for (int i = 0; i <= (AudioControl.ScanRate * AudioControl.SampleLength) - 1; i++)
                {
                    env[i] = Math.Cos(fEnvelope * AudioControl.DeltaPI * i);
                    if (_pauseEnvelope)
                        if (env[i] < 0) env[i] = 0;
                }
            }
        }
    }
}
