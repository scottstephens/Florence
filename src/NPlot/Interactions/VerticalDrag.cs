using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Florence.Interactions
{
    public class VerticalDrag : IInteraction 
    {
        public override bool Equals(object obj)
        {
            var tobj = obj as VerticalDrag;
            if (tobj == null)
                return false;
            else
                return true;
        }
    }
}
