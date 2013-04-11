using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Florence
{
    public interface IPlotWidget
    {
        InteractivePlotSurface2D InteractivePlotSurface2D { get; }
    }
}
