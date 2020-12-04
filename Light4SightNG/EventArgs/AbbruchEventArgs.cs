using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light4SightNG
{
    class AbbruchEventArgs : EventArgs
    {
        public readonly string info;

        internal AbbruchEventArgs(string sinfo)
        {
            this.info = sinfo;
        }
    }
}
