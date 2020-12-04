using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light4SightNG
{
    public class LogWriter
    {
        readonly StreamWriter LogFile;
        bool debugfile = false;
        readonly string filename;

        public LogWriter(string dateiname, bool debug)
        {
            this.filename = @".\Untersuchungen\" + dateiname;

            if (debug == true)
            {
                this.debugfile = true;
            }

            try
            {
                DirectoryInfo d = new DirectoryInfo(@".\Untersuchungen\");
                d.Create();
                this.LogFile = new StreamWriter(this.filename);
            }
            catch (Exception e)
            {
            }
        }

        public string add(string info)
        {
            try
            {
                this.LogFile.WriteLine(info);
                this.LogFile.Flush();
                return this.debugfile == false ? "In " + this.filename + "geschrieben: " + info : "";
            }
            catch
            {
                return ("Fehler beim Schreiben in " + this.filename);
            }
        }

        public string close()
        {
            try
            {
                this.LogFile.Close();
                return this.debugfile == false ? this.filename + " wurde geschlossen" : "";
            }
            catch
            {
                return this.debugfile == false ? "Beim Schließen von " + this.filename + " ist ein Fehler aufgetreten." : "";
            }
        }


    }
}
