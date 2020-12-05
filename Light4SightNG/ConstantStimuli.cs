using System;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;

namespace Light4SightNG
{
    class ConstantStimuli : Teststrategie
    {
        double std;

        public int NTrials { get; set; }

        public List<int> contrastPresentations;
        private readonly List<int> contrasts = new List<int>() { 0, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };

        Random shuffle;
        int nextContrast;

        public ConstantStimuli(Steuerung parent) : base(parent)
        {

            _SchwelleErreichtMessage = "ConstantStimulus;;";

            NTrials = parent.NTrials;

            contrastPresentations = new List<int>();
            foreach (int i in contrasts)
                for (int j = 0; j < NTrials; j++) contrastPresentations.Add(i);

            shuffle = new Random();

        }

        protected override void ZeigeNeueSignalstaerke()
        {
            if (!TesteAbbruch())
            {
                nextContrast = shuffle.Next(contrastPresentations.Count);
                this.ChangeContrast(contrastPresentations[nextContrast]);
                contrastPresentations.RemoveAt(nextContrast);
                this.PlaySignal();
            }
            else
            {
                this.OnAbbruch(new AbbruchEventArgs(""));
            }
        }

        protected override bool TesteAbbruch()
        {
            return (contrastPresentations.Count == 0);
        }

    }
}
