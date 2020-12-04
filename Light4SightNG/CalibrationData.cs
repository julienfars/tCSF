using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light4SightNG
{
    class CalibrationData
    {
        static string[,] luminance = new string[10000, 8];
        static string[,] internalValue = new string[10000, 8];
        static string[] dMaxMHTmp = new string[8];

        public CalibrationData()
        {
            StreamReader srKalibrierungsdaten = new StreamReader(".\\lookupTable.csv");
            char[] charSep = { ';' };

            dMaxMHTmp = srKalibrierungsdaten.ReadLine().Split(charSep, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < 10000; i++)
            luminance = srKalibrierungsdaten.ReadLine().Split(charSep, StringSplitOptions.RemoveEmptyEntries)[i, ];
            internalValue = srKalibrierungsdaten.ReadLine().Split(charSep, StringSplitOptions.RemoveEmptyEntries)[i, ];
        }
    }
}
