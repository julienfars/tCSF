using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light4SightNG
{
    class TesteChannels
    {
        public static void CreateTestChannelArrays()
        {
            Globals.Kanal_1_IR = new double[AudioControlClass.AbtastFrequenz];
            Globals.Kanal_2_IG = new double[AudioControlClass.AbtastFrequenz];
            Globals.Kanal_3_IB = new double[AudioControlClass.AbtastFrequenz];
            Globals.Kanal_4_IC = new double[AudioControlClass.AbtastFrequenz];
            Globals.Kanal_5_OR = new double[AudioControlClass.AbtastFrequenz];
            Globals.Kanal_6_OG = new double[AudioControlClass.AbtastFrequenz];
            Globals.Kanal_7_OB = new double[AudioControlClass.AbtastFrequenz];
            Globals.Kanal_8_OC = new double[AudioControlClass.AbtastFrequenz];
        }

        public static void WriteToWaveTestContainer(double dValue, int iChannel, int iPosition)
        {
            AudioControlClass.WaveDaten[(((iPosition * 8) + iChannel) * 2)] = AudioControlClass.Lowbyte((Int16)dValue);
            AudioControlClass.WaveDaten[(((iPosition * 8) + iChannel) * 2) + 1] = AudioControlClass.Highbyte((Int16)dValue);
        }

        public static void ConcatTestChannels()
        {
            for (int i = 0; i <= AudioControlClass.AbtastFrequenz - 1; i++)
            {
                WriteToWaveTestContainer(Globals.Kanal_1_IR[i], 0, i);
                WriteToWaveTestContainer(Globals.Kanal_2_IG[i], 1, i);
                WriteToWaveTestContainer(Globals.Kanal_3_IB[i], 2, i);
                WriteToWaveTestContainer(Globals.Kanal_4_IC[i], 3, i);
                WriteToWaveTestContainer(Globals.Kanal_5_OR[i], 4, i);
                WriteToWaveTestContainer(Globals.Kanal_6_OG[i], 5, i);
                WriteToWaveTestContainer(Globals.Kanal_7_OB[i], 6, i);
                WriteToWaveTestContainer(Globals.Kanal_8_OC[i], 7, i);
            }
        }
    }
}
