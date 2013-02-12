using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPlot.Interactions
{
    public class HorizontalDrag : IInteraction 
    {
        public override bool Equals(object obj)
        {
            var tobj = obj as HorizontalDrag;
            if (tobj == null)
                return false;
            else
                return true;
        }
    }
}
