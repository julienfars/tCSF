using System;
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

        public BestPEST(Steuerung parent) : base(parent)
        {
            double lgit;

            plgit = new double[2 * MaximaleSignalStaerke];
            mlgit = new double[2 * MaximaleSignalStaerke];

            _SchwelleErreichtMessage = "BestPEST: Schwelle erreicht!;;";

            NTrials = parent.NTrials;

            std = MaximaleSignalStaerke / 5;

            for (int i = 0; i < (2 * MaximaleSignalStaerke); i++)
            {
                lgit = 1 / (1 + Math.Exp((MaximaleSignalStaerke - i) / std));
                plgit[i] = Math.Log(lgit);
                mlgit[i] = Math.Log(1 - lgit);
            }

            probability = new double[2 * MaximaleSignalStaerke];

            SignalStaerke = MaximaleSignalStaerke;
            Gesehen = true;

            Threshold();

            SignalStaerke = 0;
            Gesehen = false;

            //Threshold() called once more first run of ZeigeNeueSignalstaerke()
        }

        protected override void ZeigeNeueSignalstaerke()
        {
            if (!TesteAbbruch())
            {
                trial++;
                Threshold();
<<<<<<< HEAD
<<<<<<< HEAD
=======
                if (signalStrength < maxSignalStrength/8)//new & trial > NTrials / 2
                {
                    signalStrength = signalStrength / 2;
                }
>>>>>>> parent of 58089e3 (Add files via upload)
                this.ChangeContrast(signalStrength);
                this.PlaySignal();
=======
                this.ChangeContrast(SignalStaerke);
                this.SignalWiedergeben();
>>>>>>> parent of e1303e6 (Add files via upload)
            }
            else
            {
                Threshold();
                this.OnAbbruch(new AbbruchEventArgs(""));
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

            for (int i = 0; i < MaximaleSignalStaerke; i++)
            {
                if (Gesehen) probability[i] = probability[i] + plgit[MaximaleSignalStaerke + SignalStaerke - i - 1];
                if (!Gesehen) probability[i] = probability[i] + mlgit[MaximaleSignalStaerke + SignalStaerke - i - 1];
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
            SignalStaerke = (int)Math.Floor((double)(p1 + p2) / 2);
        }
    }
}
