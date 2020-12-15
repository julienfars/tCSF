using System;
using System.Windows.Forms;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace Light4SightNG
{
    class ConstantStimuli : Teststrategie
    {
        double std;

        public int NTrials { get; set; }
        public int PThreshold { get; set; }


        public List<int> contrastPresentations;
        public List<int> PTcontrast;
        public List<int> contrasts;
        private readonly List<int> contrasts_basic = new List<int>() { 0, 10, 25, 50, 100, 300, 500, 700, 900, 1000 }; //2Hz - Low freq

        Random shuffle;
        int nextContrast;
        public int step;

        public ConstantStimuli(Steuerung parent) : base(parent)
        {

            _SchwelleErreichtMessage = "ConstantStimulus;;";

            NTrials = parent.NTrials;
            PThreshold = parent.PThreshold *10;
            //step = PThreshold / 10; not used, was written to generate a list of numbers by mathematical contrainst 
            // generate the list of contrasts here 
            PTcontrast = new List<int>() { PThreshold-40, PThreshold-30, PThreshold-20, PThreshold-10, PThreshold-5, PThreshold-2, PThreshold+2, PThreshold+5, PThreshold+10, PThreshold+20, PThreshold+30, PThreshold+40 };
            PTcontrast = PTcontrast.Where(i => i >= 0).ToList();
            PTcontrast = PTcontrast.Where(i => i < 1000).ToList();
            var contrasts = contrasts_basic.Concat(PTcontrast);

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
