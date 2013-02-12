using System;
using NPlot;

namespace NPlot.WinForms
{
    public interface IWinFormsInteraction
    {
        bool DoMouseDown(System.Windows.Forms.MouseEventArgs e, System.Windows.Forms.Control ctr);
        bool DoMouseLeave(EventArgs e, System.Windows.Forms.Control ctr);
        bool DoMouseMove(System.Windows.Forms.MouseEventArgs e, System.Windows.Forms.Control ctr, System.Windows.Forms.KeyEventArgs lastKeyEventArgs);
        bool DoMouseUp(System.Windows.Forms.MouseEventArgs e, System.Windows.Forms.Control ctr);
        bool DoMouseWheel(System.Windows.Forms.MouseEventArgs e, System.Windows.Forms.Control ctr);
        void DoPaint(System.Windows.Forms.PaintEventArgs pe, int width, int height);
        IInteraction GenericInteraction { get; }
        event Action<object, IInteraction> InteractionOccurred;
    }
}
