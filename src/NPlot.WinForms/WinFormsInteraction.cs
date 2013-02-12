using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace NPlot.WinForms
{

    /// <summary>
    /// Base class for an interaction. All methods are virtual. Not abstract as not all interactions
    /// need to use all methods. Default functionality for each method is to do nothing. 
    /// </summary>
    public class WinFormsInteraction<T> : NPlot.WinForms.IWinFormsInteraction where T : IInteraction
    {
        public IInteraction GenericInteraction { get { return this.Interaction; } }
        public T Interaction { get; private set; }

        public WinFormsInteraction(T interaction)
        {
            this.Interaction = interaction;
        }

        public event Action<object, IInteraction> InteractionOccurred;

        public event Action<object, T> InteractionOccurredTyped;

        protected void FireInteractionOccurred()
        {
            if (InteractionOccurred != null)
                this.InteractionOccurred(this, this.Interaction);
            if (InteractionOccurredTyped != null)
                this.InteractionOccurredTyped(this, this.Interaction);
        }

        /// <summary>
        /// Handler for this interaction if a mouse down event is received.
        /// </summary>
        /// <param name="e">event args</param>
        /// <param name="ctr">reference to the control</param>
        /// <returns>true if plot surface needs refreshing.</returns>
        public virtual bool DoMouseDown(MouseEventArgs e, System.Windows.Forms.Control ctr) { return false; }

        /// <summary>
        /// Handler for this interaction if a mouse up event is received.
        /// </summary>
        /// <param name="e">event args</param>
        /// <param name="ctr">reference to the control</param>
        /// <returns>true if plot surface needs refreshing.</returns>
        public virtual bool DoMouseUp(MouseEventArgs e, System.Windows.Forms.Control ctr) { return false; }

        /// <summary>
        /// Handler for this interaction if a mouse move event is received.
        /// </summary>
        /// <param name="e">event args</param>
        /// <param name="ctr">reference to the control</param>
        /// <param name="lastKeyEventArgs"></param>
        /// <returns>true if plot surface needs refreshing.</returns>
        public virtual bool DoMouseMove(MouseEventArgs e, System.Windows.Forms.Control ctr, KeyEventArgs lastKeyEventArgs) { return false; }

        /// <summary>
        /// Handler for this interaction if a mouse move event is received.
        /// </summary>
        /// <param name="e">event args</param>
        /// <param name="ctr">reference to the control</param>
        /// <returns>true if plot surface needs refreshing.</returns>
        public virtual bool DoMouseWheel(MouseEventArgs e, System.Windows.Forms.Control ctr) { return false; }

        /// <summary>
        /// Handler for this interaction if a mouse Leave event is received.
        /// </summary>
        /// <param name="e">event args</param>
        /// <param name="ctr">reference to the control</param>
        /// <returns>true if the plot surface needs refreshing.</returns>
        public virtual bool DoMouseLeave(EventArgs e, System.Windows.Forms.Control ctr) { return false; }

        /// <summary>
        /// Handler for this interaction if a paint event is received.
        /// </summary>
        /// <param name="pe">paint event args</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public virtual void DoPaint(PaintEventArgs pe, int width, int height) { }
    }

}
