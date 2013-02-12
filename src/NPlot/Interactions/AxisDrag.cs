using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPlot.Interactions
{
    public class AxisDrag : IInteraction
    {
        public bool EnableDragWithCtr { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enableDragWithCtr"></param>
        public AxisDrag(bool enableDragWithCtr)
        {
            this.EnableDragWithCtr = enableDragWithCtr;
        }

        public override bool Equals(object obj)
        {
            var tobj = obj as AxisDrag;
            if (tobj == null)
                return false;
            else 
                return this.EnableDragWithCtr == tobj.EnableDragWithCtr;
        }
    }
}
