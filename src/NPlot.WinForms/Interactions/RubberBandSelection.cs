using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace NPlot.WinForms.Interactions
{
    /// <summary>
    /// 
    /// </summary>
    public class RubberBandSelection : WinFormsInteraction<NPlot.Interactions.RubberBandSelection>
    {
        public RubberBandSelection(NPlot.Interactions.RubberBandSelection interaction) : base(interaction) { }

        private bool selectionInitiated_ = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ctr"></param>
        public override bool DoMouseDown(MouseEventArgs e, Control ctr)
        {
            // keep track of the start point and flag that select initiated.
            selectionInitiated_ = true;
            startPoint_.X = e.X;
            startPoint_.Y = e.Y;

            // invalidate the end point
            endPoint_ = unset_;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ctr"></param>
        /// <param name="lastKeyEventArgs"></param>
        public override bool DoMouseMove(MouseEventArgs e, Control ctr, KeyEventArgs lastKeyEventArgs)
        {
            if ((e.Button == MouseButtons.Left) && selectionInitiated_)
            {
                // we are here
                Point here = new Point(e.X, e.Y);

                // delete the previous box
                if (endPoint_ != unset_)
                {
                    this.DrawRubberBand(startPoint_, endPoint_, ctr);
                }
                endPoint_ = here;

                // and redraw the last one
                this.DrawRubberBand(startPoint_, endPoint_, ctr);
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ctr"></param>
        public override bool DoMouseUp(MouseEventArgs e, Control ctr)
        {
            NPlot.PlotSurface2D ps = ((WinForms.WinFormsPlotSurface2D)ctr).Inner;

            // handle left button (selecting region).
            if ((e.Button == MouseButtons.Left) && selectionInitiated_)
            {
                endPoint_.X = e.X;
                endPoint_.Y = e.Y;

                // flag stopped selecting.
                selectionInitiated_ = false;

                if (endPoint_ != unset_)
                {
                    this.DrawRubberBand(startPoint_, endPoint_, ctr);
                }

                Point minPoint = new Point(0, 0);
                minPoint.X = Math.Min(startPoint_.X, endPoint_.X);
                minPoint.Y = Math.Min(startPoint_.Y, endPoint_.Y);

                Point maxPoint = new Point(0, 0);
                maxPoint.X = Math.Max(startPoint_.X, endPoint_.X);
                maxPoint.Y = Math.Max(startPoint_.Y, endPoint_.Y);

                // We also need the point coordinates at the other two corners
                // to check if the rubberband selection is inbounds 
                //(i.e. at least one corner is located inside the chart
                Point cornerUpperRight = new Point(0, 0);
                cornerUpperRight.X = Math.Max(startPoint_.X, endPoint_.X);
                cornerUpperRight.Y = Math.Min(startPoint_.Y, endPoint_.Y);

                Point cornerLowerLeft = new Point(0, 0);
                cornerLowerLeft.X = Math.Min(startPoint_.X, endPoint_.X);
                cornerLowerLeft.Y = Math.Max(startPoint_.Y, endPoint_.Y);

                Rectangle r = ps.PlotAreaBoundingBoxCache;
                bool isRubberbandInbounds = r.Contains(minPoint) ||
                                            r.Contains(maxPoint) ||
                                            r.Contains(cornerUpperRight) ||
                                            r.Contains(cornerLowerLeft);

                if (isRubberbandInbounds && minPoint != maxPoint)
                {
                    ((WinForms.WinFormsPlotSurface2D)ctr).CacheAxes();

                    ((WinForms.WinFormsPlotSurface2D)ctr).PhysicalXAxis1Cache.SetWorldLimitsFromPhysical(minPoint, maxPoint);
                    ((WinForms.WinFormsPlotSurface2D)ctr).PhysicalXAxis2Cache.SetWorldLimitsFromPhysical(minPoint, maxPoint);
                    ((WinForms.WinFormsPlotSurface2D)ctr).PhysicalYAxis1Cache.SetWorldLimitsFromPhysical(maxPoint, minPoint);
                    ((WinForms.WinFormsPlotSurface2D)ctr).PhysicalYAxis2Cache.SetWorldLimitsFromPhysical(maxPoint, minPoint);

                    // reset the start/end points
                    startPoint_ = unset_;
                    endPoint_ = unset_;
                    
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Draws a rectangle representing selection area. 
        /// </summary>
        /// <param name="start">a corner of the rectangle.</param>
        /// <param name="end">a corner of the rectangle diagonally opposite the first.</param>
        /// <param name="ctr">The control to draw to - this may not be us, if we have
        /// been contained by a PlotSurface.</param>
        private void DrawRubberBand(Point start, Point end, System.Windows.Forms.Control ctr)
        {
            NPlot.PlotSurface2D ps = ((WinForms.WinFormsPlotSurface2D)ctr).Inner;

            Rectangle rect = new Rectangle();

            // the clipping rectangle in screen coordinates
            Rectangle clip = ctr.RectangleToScreen(
                new Rectangle(
                (int)ps.PlotAreaBoundingBoxCache.X,
                (int)ps.PlotAreaBoundingBoxCache.Y,
                (int)ps.PlotAreaBoundingBoxCache.Width,
                (int)ps.PlotAreaBoundingBoxCache.Height));

            // convert to screen coords
            start = ctr.PointToScreen(start);
            end = ctr.PointToScreen(end);

            // now, "normalize" the rectangle
            if (start.X < end.X)
            {
                rect.X = start.X;
                rect.Width = end.X - start.X;
            }
            else
            {
                rect.X = end.X;
                rect.Width = start.X - end.X;
            }
            if (start.Y < end.Y)
            {
                rect.Y = start.Y;
                rect.Height = end.Y - start.Y;
            }
            else
            {
                rect.Y = end.Y;
                rect.Height = start.Y - end.Y;
            }
            rect = Rectangle.Intersect(rect, clip);

            ControlPaint.DrawReversibleFrame(
                new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height),
                Color.White, FrameStyle.Dashed);
        }

        private Point startPoint_ = new Point(-1, -1);
        private Point endPoint_ = new Point(-1, -1);
        // this is the condition for an unset point
        private Point unset_ = new Point(-1, -1);
    }
}
