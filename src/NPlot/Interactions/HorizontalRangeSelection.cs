using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPlot.Interactions
{
    public class HorizontalRangeSelection : IInteraction
    {

        public double SmallestAllowedRange { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public HorizontalRangeSelection()
        {
            this.SmallestAllowedRange = double.Epsilon * 100.0;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="smallestAllowedRange">the smallest distance between the selected xmin and xmax for the selection to be performed.</param>
        public HorizontalRangeSelection(double smallestAllowedRange)
        {
            this.SmallestAllowedRange = smallestAllowedRange;
        }

        public override bool Equals(object obj)
        {
            var tobj = obj as HorizontalRangeSelection;
            if (tobj == null)
                return false;
            else
                return this.SmallestAllowedRange == tobj.SmallestAllowedRange;
        }
    }
}
