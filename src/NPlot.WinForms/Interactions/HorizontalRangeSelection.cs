using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace NPlot.WinForms.Interactions
{
    /// <summary>
    /// This plot intraction allows the user to select horizontal regions.
    /// </summary>
    public class HorizontalRangeSelection : WinFormsInteraction<NPlot.Interactions.HorizontalRangeSelection>
    {
        public HorizontalRangeSelection(NPlot.Interactions.HorizontalRangeSelection interaction) : base(interaction) { }

        private bool selectionInitiated_ = false;
        private Point startPoint_ = new Point(-1, -1);
        private Point endPoint_ = new Point(-1, -1);
        private Point previousPoint_ = new Point(-1, -1);
        private Point unset_ = new Point(-1, -1);
        private int minimumPixelDistanceForSelect_ = 5;

        /// <summary>
        /// The minimum width of the selected region (in pixels) for the interaction to zoom.
        /// </summary>
        public int MinimumPixelDistanceForSelect
        {
            get
            {
                return minimumPixelDistanceForSelect_;
            }
            set
            {
                minimumPixelDistanceForSelect_ = value;
            }
        }


        /// <summary>
        /// Handler for mouse down event for this interaction
        /// </summary>
        /// <param name="e">the mouse event args</param>
        /// <param name="ctr">the plot surface this event applies to</param>
        public override bool DoMouseDown(MouseEventArgs e, Control ctr)
        {
            NPlot.PlotSurface2D ps = ((WinForms.WinFormsPlotSurface2D)ctr).Inner;

            if (e.X > ps.PlotAreaBoundingBoxCache.Left && e.X < ps.PlotAreaBoundingBoxCache.Right &&
                e.Y > ps.PlotAreaBoundingBoxCache.Top && e.Y < ps.PlotAreaBoundingBoxCache.Bottom)
            {

                // keep track of the start point and flag that select initiated.
                selectionInitiated_ = true;
                startPoint_.X = e.X;
                startPoint_.Y = e.Y;

                previousPoint_.X = e.X;
                previousPoint_.Y = e.Y;

                // invalidate the end point
                endPoint_ = unset_;

                return false;
            }

            selectionInitiated_ = false;
            endPoint_ = unset_;
            startPoint_ = unset_;

            return false;
        }


        /// <summary>
        /// Handler for mouse move event for this interaction
        /// </summary>
        /// <param name="e">the mouse event args</param>
        /// <param name="ctr">the plot surface this event applies to</param>
        /// <param name="lastKeyEventArgs"></param>
        public override bool DoMouseMove(MouseEventArgs e, Control ctr, KeyEventArgs lastKeyEventArgs)
        {
            NPlot.PlotSurface2D ps = ((WinForms.WinFormsPlotSurface2D)ctr).Inner;

            // if dragging on axis to zoom.
            if ((e.Button == MouseButtons.Left) && selectionInitiated_)
            {
                Point endPoint_ = previousPoint_;
                if (e.X > ps.PlotAreaBoundingBoxCache.Left && e.X < ps.PlotAreaBoundingBoxCache.Right &&
                    e.Y > ps.PlotAreaBoundingBoxCache.Top && e.Y < ps.PlotAreaBoundingBoxCache.Bottom)
                {
                    endPoint_ = new Point(e.X, e.Y);
                    this.DrawHorizontalSelection(previousPoint_, endPoint_, ctr);
                    previousPoint_ = endPoint_;
                }
                else
                {
                    endPoint_ = new Point(e.X, e.Y);
                    if (e.X < ps.PlotAreaBoundingBoxCache.Left) endPoint_.X = ps.PlotAreaBoundingBoxCache.Left + 1;
                    if (e.X > ps.PlotAreaBoundingBoxCache.Right) endPoint_.X = ps.PlotAreaBoundingBoxCache.Right - 1;
                    this.DrawHorizontalSelection(previousPoint_, endPoint_, ctr);
                    previousPoint_ = endPoint_;
                }
            }

            return false;
        }


        /// <summary>
        /// Handler for mouse up event for this interaction
        /// </summary>
        /// <param name="e">the mouse event args</param>
        /// <param name="ctr">the plot surface this event applies to</param>
        public override bool DoMouseUp(MouseEventArgs e, Control ctr)
        {
            NPlot.PlotSurface2D ps = ((WinForms.WinFormsPlotSurface2D)ctr).Inner;

            if ((e.Button == MouseButtons.Left) && selectionInitiated_)
            {
                endPoint_.X = e.X;
                endPoint_.Y = e.Y;
                if (e.X < ps.PlotAreaBoundingBoxCache.Left)
                {
                    endPoint_.X = ps.PlotAreaBoundingBoxCache.Left + 1;
                }

                if (e.X > ps.PlotAreaBoundingBoxCache.Right)
                {
                    endPoint_.X = ps.PlotAreaBoundingBoxCache.Right - 1;
                }

                // flag stopped selecting.
                selectionInitiated_ = false;

                if (endPoint_ != unset_)
                {
                    this.DrawHorizontalSelection(startPoint_, endPoint_, ctr);
                }

                // ignore very small selections
                if (Math.Abs(endPoint_.X - startPoint_.X) < minimumPixelDistanceForSelect_)
                {
                    return false;
                }

                ((WinForms.WinFormsPlotSurface2D)ctr).CacheAxes();

                // determine the new x axis 1 world limits (and check to see if they are far enough appart).
                double xAxis1Min = double.NaN;
                double xAxis1Max = double.NaN;
                if (ps.XAxis1 != null)
                {
                    int x1 = (int)Math.Min(endPoint_.X, startPoint_.X);
                    int x2 = (int)Math.Max(endPoint_.X, startPoint_.X);
                    int y = ps.PhysicalXAxis1Cache.PhysicalMax.Y;

                    xAxis1Min = ps.PhysicalXAxis1Cache.PhysicalToWorld(new Point(x1, y), true);
                    xAxis1Max = ps.PhysicalXAxis1Cache.PhysicalToWorld(new Point(x2, y), true);
                    if (xAxis1Max - xAxis1Min < this.Interaction.SmallestAllowedRange)
                    {
                        return false;
                    }
                }

                // determine the new x axis 2 world limits (and check to see if they are far enough appart).
                double xAxis2Min = double.NaN;
                double xAxis2Max = double.NaN;
                if (ps.XAxis2 != null)
                {
                    int x1 = (int)Math.Min(endPoint_.X, startPoint_.X);
                    int x2 = (int)Math.Max(endPoint_.X, startPoint_.X);
                    int y = ps.PhysicalXAxis2Cache.PhysicalMax.Y;

                    xAxis2Min = ps.PhysicalXAxis2Cache.PhysicalToWorld(new Point(x1, y), true);
                    xAxis2Max = ps.PhysicalXAxis2Cache.PhysicalToWorld(new Point(x2, y), true);
                    if (xAxis2Max - xAxis2Min < this.Interaction.SmallestAllowedRange)
                    {
                        return false;
                    }
                }

                // now actually update the world limits.
                if (ps.XAxis1 != null)
                {
                    ps.XAxis1.WorldMax = xAxis1Max;
                    ps.XAxis1.WorldMin = xAxis1Min;
                }

                if (ps.XAxis2 != null)
                {
                    ps.XAxis2.WorldMax = xAxis2Max;
                    ps.XAxis2.WorldMin = xAxis2Min;
                }

                this.FireInteractionOccurred();

                return true;
            }

            return false;
        }


        private void DrawHorizontalSelection(Point start, Point end, System.Windows.Forms.Control ctr)
        {
            NPlot.PlotSurface2D ps = ((WinForms.WinFormsPlotSurface2D)ctr).Inner;

            // the clipping rectangle in screen coordinates
            Rectangle clip = ctr.RectangleToScreen(
                new Rectangle(
                (int)ps.PlotAreaBoundingBoxCache.X,
                (int)ps.PlotAreaBoundingBoxCache.Y,
                (int)ps.PlotAreaBoundingBoxCache.Width,
                (int)ps.PlotAreaBoundingBoxCache.Height));

            start = ctr.PointToScreen(start);
            end = ctr.PointToScreen(end);

            ControlPaint.FillReversibleRectangle(
                new Rectangle((int)Math.Min(start.X, end.X), (int)clip.Y, (int)Math.Abs(end.X - start.X), (int)clip.Height),
                Color.White);
        }
    }
}
