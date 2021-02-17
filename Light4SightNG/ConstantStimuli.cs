using System;
using System.Windows.Forms;
using System.Threading;
namespace Light4SightNG
{
    class ConstantStimuli : Teststrategie
    {
        double std;

        public int NTrials { get; set; }


        public ConstantStimuli(Steuerung parent) : base(parent)
        {

            _SchwelleErreichtMessage = "ConstantStimulus;;";

            NTrials = parent.NTrials;

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
                Threshold();
                this.ChangeContrast(signalStrength);
                this.PlaySignal();
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

    }
}
