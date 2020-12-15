﻿using System;
using System.Windows.Forms;
using System.Threading;
namespace Light4SightNG
{
    class BestPEST : Teststrategie
    {
        double std;
        readonly double[] plgit;
        readonly double[] mlgit;
        readonly double[] probability;

        public int NTrials { get; set; }

        int trial = 0;
        int ratio; 

        public BestPEST(Steuerung parent) : base(parent)
        {
            double lgit;

            plgit = new double[2 * maxSignalStrength];
            mlgit = new double[2 * maxSignalStrength];

            _SchwelleErreichtMessage = "BestPEST: Schwelle erreicht!;;";

            NTrials = parent.NTrials;

            std = maxSignalStrength / 5;

            for (int i = 0; i < (2 * maxSignalStrength); i++)
            {
                lgit = 1 / (1 + Math.Exp((maxSignalStrength - i) / std));
                plgit[i] = Math.Log(lgit);
                mlgit[i] = Math.Log(1 - lgit);
            }

            probability = new double[2 * maxSignalStrength];

            signalStrength = maxSignalStrength;
            Gesehen = true;

            Threshold();

            signalStrength = 0;
            Gesehen = false;

            //Threshold() called once more first run of ZeigeNeueSignalstaerke()
        }

        protected override void ZeigeNeueSignalstaerke()
        {
            if (!TesteAbbruch())
            {
                trial++;
                Threshold();
                if (signalStrength < maxSignalStrength/8)//new & trial > NTrials / 2
                {
                    signalStrength = signalStrength / 2;
                }
                this.ChangeContrast(signalStrength);
                this.PlaySignal();
            }
            else
            {
                if (Gesehen == false | signalStrength == 0) // The aim is to continue to test until a good answer, or until a answer different than 0 is given
                {
                    NTrials++;
                    trial++;
                    Threshold();
                    if (signalStrength < maxSignalStrength / 8)//new & trial > NTrials / 2
                    {
                        signalStrength = signalStrength / 2;
                    }
                    this.ChangeContrast(signalStrength);
                    this.PlaySignal();
                }
                else
                {
                    Threshold();
                    this.OnAbbruch(new AbbruchEventArgs(""));
                }
                //Threshold();
                //this.OnAbbruch(new AbbruchEventArgs(""));
            }
        }

        protected override bool TesteAbbruch()
        {
            return (trial >= NTrials);
        }


        void Threshold()
        {
            int p1 = 0;
            int p2 = 0;
            double max = -10000;

            for (int i = 0; i < maxSignalStrength; i++)
            {
                if (Gesehen) probability[i] = probability[i] + plgit[maxSignalStrength + signalStrength - i - 1];
                if (!Gesehen) probability[i] = probability[i] + mlgit[maxSignalStrength + signalStrength - i - 1];
                if (probability[i] > max)
                {
                    max = probability[i];
                    p1 = i;
                }
                if (probability[i] == max)
                {
                    p2 = i;
                }
            }
            signalStrength = (int)Math.Floor((double)(p1 + p2) / 2);
        }
    }
}