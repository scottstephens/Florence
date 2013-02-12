using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPlot.WinForms
{
    public class InteractionRegistry
    {
        private static InteractionRegistry _instance = null;

        public static InteractionRegistry Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new InteractionRegistry();
                return _instance;
            }
        }


        private Dictionary<Type, Func<IInteraction, IWinFormsInteraction>> _interactions;

        private InteractionRegistry()
        {
            _interactions = new Dictionary<Type, Func<IInteraction, IWinFormsInteraction>>();
            this.RegisterInteraction(typeof(NPlot.Interactions.AxisDrag), (i) => new Interactions.AxisDrag((NPlot.Interactions.AxisDrag)i));
            this.RegisterInteraction(typeof(NPlot.Interactions.HorizontalDrag), (i) => new Interactions.HorizontalDrag((NPlot.Interactions.HorizontalDrag)i));
            this.RegisterInteraction(typeof(NPlot.Interactions.HorizontalGuideline), (i) => new Interactions.HorizontalGuideline((NPlot.Interactions.HorizontalGuideline)i));
            this.RegisterInteraction(typeof(NPlot.Interactions.MouseWheelZoom), (i) => new Interactions.MouseWheelZoom((NPlot.Interactions.MouseWheelZoom)i));
            this.RegisterInteraction(typeof(NPlot.Interactions.HorizontalRangeSelection), (i) => new Interactions.HorizontalRangeSelection((NPlot.Interactions.HorizontalRangeSelection)i));
            this.RegisterInteraction(typeof(NPlot.Interactions.RubberBandSelection), (i) => new Interactions.RubberBandSelection((NPlot.Interactions.RubberBandSelection)i));
            this.RegisterInteraction(typeof(NPlot.Interactions.VerticalDrag), (i) => new Interactions.VerticalDrag((NPlot.Interactions.VerticalDrag)i));
            this.RegisterInteraction(typeof(NPlot.Interactions.VerticalGuideline), (i) => new Interactions.VerticalGuideline((NPlot.Interactions.VerticalGuideline)i));
        }

        public void RegisterInteraction(Type interaction_type, Func<IInteraction, IWinFormsInteraction> toolkit_specific_retriever)
        {
            _interactions.Add(interaction_type, toolkit_specific_retriever);
        }

        public IWinFormsInteraction GetWinFormsInteraction(IInteraction interaction)
        {
            Func<IInteraction, IWinFormsInteraction> getter;
            if (!_interactions.TryGetValue(interaction.GetType(), out getter))
                return null;
            else
            {
                return getter(interaction);
            }
        }
    }
}
