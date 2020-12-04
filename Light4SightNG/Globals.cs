using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Light4SightNG
{
    static class Globals
    {


        public static bool bPlaySignal = false;
        public static bool flagSignalWiedergabe = false;
        public static bool flagDebugLog = false;
        public static bool flagUntersuchunglaeuft = false;

        public static string LEDBereich = "alle";
        public static string Staircase = "beide";

        public static double[] Kanal_1_IR;
        public static double[] Kanal_2_IG;
        public static double[] Kanal_3_IB;
        public static double[] Kanal_4_IC;
        public static double[] Kanal_5_OR;
        public static double[] Kanal_6_OG;
        public static double[] Kanal_7_OB;
        public static double[] Kanal_8_OC;

        static string[] poly4Tmp = new string[8];
        static string[] poly3Tmp = new string[8];
        static string[] poly2Tmp = new string[8];
        static string[] poly1Tmp = new string[8];
        static string[] interceptTmp = new string[8];
        static string[] dMaxMHTmp = new string[8];

        public static int ReadCalibrationData()
        {
            try
            {
                StreamReader srKalibrierungsdaten = new StreamReader(".\\calibrationdataPoly4.csv");
                char[] charSep = { ';' };

                dMaxMHTmp = srKalibrierungsdaten.ReadLine().Split(charSep, StringSplitOptions.RemoveEmptyEntries);
                poly4Tmp = srKalibrierungsdaten.ReadLine().Split(charSep, StringSplitOptions.RemoveEmptyEntries);
                poly3Tmp = srKalibrierungsdaten.ReadLine().Split(charSep, StringSplitOptions.RemoveEmptyEntries);
                poly2Tmp = srKalibrierungsdaten.ReadLine().Split(charSep, StringSplitOptions.RemoveEmptyEntries);
                poly1Tmp = srKalibrierungsdaten.ReadLine().Split(charSep, StringSplitOptions.RemoveEmptyEntries);
                interceptTmp = srKalibrierungsdaten.ReadLine().Split(charSep, StringSplitOptions.RemoveEmptyEntries);
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        public static double dMaxMH(int Kanal)
        {
            return (double.Parse(dMaxMHTmp[Kanal]));
        }

        public static double poly4(int Kanal)
        {
            return (double.Parse(poly4Tmp[Kanal]));
        }

        public static double poly3(int Kanal)
        {
            return (double.Parse(poly3Tmp[Kanal]));
        }

        public static double poly2(int Kanal)
        {
            return (double.Parse(poly2Tmp[Kanal]));
        }

        public static double poly1(int Kanal)
        {
            return (double.Parse(poly1Tmp[Kanal]));
        }

        public static double intercept(int Kanal)
        {
            return (double.Parse(interceptTmp[Kanal]));
        }
    }
}
