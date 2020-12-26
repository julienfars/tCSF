using System;
using System.Windows.Forms;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace Light4SightNG
{
    class ConstantStimuli : TestStrategy
    {
        double std;

        public int NTrials { get; set; }
        public double PThreshold { get; set; }

        public List<int> contrastPresentations;
        public List<int> PTcontrast;
        public List<int> contrasts;
        private readonly List<int> contrasts_basic = new List<int>() { 0, 1000, 2500, 5000, 10000, 30000, 50000, 70000, 90000, 100000 }; 

        Random shuffle;
        int nextContrast;
        public int step;

        public ConstantStimuli(Mainform parent) : base(parent)
        {

            _SchwelleErreichtMessage = "ConstantStimulus;;";
            NTrials = parent.NTrials;
            PThreshold = parent.PThreshold *1000; // to get the accurate threshold considering our actual list of basic contrasts (that range from 0 to 100000)
            // generate the list of contrasts here 
            //here is a temporary change 
            PTcontrast = new List<int>() { (int)(PThreshold*.6), (int)(PThreshold * .7), (int)(PThreshold * .8), (int)(PThreshold * .9), (int)(PThreshold * .95), (int)(PThreshold * .98), (int)(PThreshold * 1.02), (int)(PThreshold * 1.05), (int)(PThreshold * 1.1), (int)(PThreshold * 1.2), (int)(PThreshold * 1.3), (int)(PThreshold * 1.4) };
            PTcontrast = PTcontrast.Where(i => i >= 0).ToList();
            PTcontrast = PTcontrast.Where(i => i < 100000).ToList();
            var contrasts = contrasts_basic.Concat(PTcontrast);

            contrastPresentations = new List<int>();
            foreach (int i in contrasts)
               for (int j = 0; j < NTrials; j++) contrastPresentations.Add(i);

            shuffle = new Random();

        }

        protected override void PresentNextStimulus()
        {
            if (!AbortCriterionReached())
            {
                nextContrast = shuffle.Next(contrastPresentations.Count);
                this.ChangeContrastCS(contrastPresentations[nextContrast]);
                contrastPresentations.RemoveAt(nextContrast);
                this.PlaySignal();
            }
            else
            {
                this.OnAbbruch(new AbbruchEventArgs(""));
            }
        }

        protected override bool AbortCriterionReached()
        {
            return (contrastPresentations.Count == 0);
        }

    }
}
