/*
 * Florence - A charting library for .NET
 * 
 * InteractionRegistry.cs
 * Copyright (C) 2013 Scott Stephens
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 * 
 * 1. Redistributions of source code must retain the above copyright notice, this
 *    list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution.
 * 3. Neither the name of Florence nor the names of its contributors may
 *    be used to endorse or promote products derived from this software without
 *    specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
 * OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 */

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
