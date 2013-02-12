using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Florence.WinForms
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
            this.RegisterInteraction(typeof(Florence.Interactions.AxisDrag), (i) => new Interactions.AxisDrag((Florence.Interactions.AxisDrag)i));
            this.RegisterInteraction(typeof(Florence.Interactions.HorizontalDrag), (i) => new Interactions.HorizontalDrag((Florence.Interactions.HorizontalDrag)i));
            this.RegisterInteraction(typeof(Florence.Interactions.HorizontalGuideline), (i) => new Interactions.HorizontalGuideline((Florence.Interactions.HorizontalGuideline)i));
            this.RegisterInteraction(typeof(Florence.Interactions.MouseWheelZoom), (i) => new Interactions.MouseWheelZoom((Florence.Interactions.MouseWheelZoom)i));
            this.RegisterInteraction(typeof(Florence.Interactions.HorizontalRangeSelection), (i) => new Interactions.HorizontalRangeSelection((Florence.Interactions.HorizontalRangeSelection)i));
            this.RegisterInteraction(typeof(Florence.Interactions.RubberBandSelection), (i) => new Interactions.RubberBandSelection((Florence.Interactions.RubberBandSelection)i));
            this.RegisterInteraction(typeof(Florence.Interactions.VerticalDrag), (i) => new Interactions.VerticalDrag((Florence.Interactions.VerticalDrag)i));
            this.RegisterInteraction(typeof(Florence.Interactions.VerticalGuideline), (i) => new Interactions.VerticalGuideline((Florence.Interactions.VerticalGuideline)i));
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
