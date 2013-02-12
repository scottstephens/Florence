using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Florence.Interactions
{
    public class VerticalGuideline : IInteraction
    {
        public Color Color { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public VerticalGuideline()
        {
            this.Color = Color.Black;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineColor"></param>
        public VerticalGuideline(Color lineColor)
        {
            this.Color = lineColor;
        }

        public override bool Equals(object obj)
        {
            var tobj = obj as VerticalGuideline;
            if (tobj == null)
                return false;
            else
                return this.Color == tobj.Color;
        }
    }
}
