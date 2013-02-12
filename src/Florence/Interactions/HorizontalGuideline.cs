using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Florence.Interactions
{
    public class HorizontalGuideline : IInteraction
    {
        public Color Color { get; private set; }
        public HorizontalGuideline(Color color)
        {
            this.Color = color;
        }
        public override bool Equals(object obj)
        {
            var tobj = obj as HorizontalGuideline;
            if (tobj == null)
                return false;
            else
                return this.Color == tobj.Color;
        }
    }
}
