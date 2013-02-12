using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Florence
{
    public interface IInteractivePlotSurface2D : IPlotSurface2D
    {
        void AddInteraction(IInteraction interaction);
        void RemoveInteraction(IInteraction interaction);
        event Action<object, IInteraction> InteractionOccurred;
    }
}
