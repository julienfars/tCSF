using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Light4SightNG
{
    public partial class ZeigeErgebnis : Form
    {
        public ZeigeErgebnis()
        {
            InitializeComponent();
            this.button1.Enabled = Directory.GetFiles(@".\Untersuchungen", "*.txt").Length > 0;
        }

        void fertig_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void ladeBild(String pfad)
        {
            if (System.IO.File.Exists(pfad)) ergebnisBMP.Load(pfad);
        }

        public void ladeText(String pfad)
        {
            if (System.IO.File.Exists(pfad)) resultText.Text = File.ReadAllText(pfad);
        }

        void button1_Click(object sender, EventArgs e)
        {
            bool doppelt = false;
            DirectoryInfo dirInfo = new DirectoryInfo(@".\Untersuchungen\" + textBox1.Text);
            if (dirInfo.Exists == false)
                Directory.CreateDirectory(@".\Untersuchungen\" + textBox1.Text);

            List<String> Dateien = Directory.GetFiles(@".\Untersuchungen", "*.*").ToList();

            foreach (string file in Dateien)
            {

                FileInfo mFile = new FileInfo(file);
                if (new FileInfo(dirInfo + "\\" + mFile.Name).Exists == false)
                    mFile.MoveTo(dirInfo + "\\" + mFile.Name);
                else
                    doppelt = true;
            }
            if (doppelt) MessageBox.Show("Nicht alle Dateien konnten verschoben werden!");
            button1.Enabled = false;
        }

        private void fertig_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
