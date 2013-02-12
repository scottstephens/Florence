using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Florence.WinForms.Interactions
{
    /// <summary>
    /// 
    /// </summary>
    public class HorizontalDrag : WinFormsInteraction<Florence.Interactions.HorizontalDrag>
    {

        public HorizontalDrag(Florence.Interactions.HorizontalDrag interaction) : base(interaction) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ctr"></param>
        public override bool DoMouseDown(MouseEventArgs e, Control ctr)
        {
            Florence.PlotSurface2D ps = ((WinForms.WinFormsPlotSurface2D)ctr).Inner;

            if (e.X > ps.PlotAreaBoundingBoxCache.Left && e.X < (ps.PlotAreaBoundingBoxCache.Right) &&
                e.Y > ps.PlotAreaBoundingBoxCache.Top && e.Y < ps.PlotAreaBoundingBoxCache.Bottom)
            {
                dragInitiated_ = true;

                lastPoint_.X = e.X;
                lastPoint_.Y = e.Y;
            }

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
            Florence.PlotSurface2D ps = ((WinForms.WinFormsPlotSurface2D)ctr).Inner;

            if ((e.Button == MouseButtons.Left) && dragInitiated_)
            {
                int diffX = e.X - lastPoint_.X;

                ((WinForms.WinFormsPlotSurface2D)ctr).CacheAxes();

                // original code was using PixelWorldLength of the physical axis
                // but it was not working for non-linear axes - the code below works
                // in all cases
                if (ps.XAxis1 != null)
                {
                    Axis axis = ps.XAxis1;
                    PointF pMin = ps.PhysicalXAxis1Cache.PhysicalMin;
                    PointF pMax = ps.PhysicalXAxis1Cache.PhysicalMax;

                    PointF physicalWorldMin = pMin;
                    PointF physicalWorldMax = pMax;
                    physicalWorldMin.X -= diffX;
                    physicalWorldMax.X -= diffX;
                    double newWorldMin = axis.PhysicalToWorld(physicalWorldMin, pMin, pMax, false);
                    double newWorldMax = axis.PhysicalToWorld(physicalWorldMax, pMin, pMax, false);
                    axis.WorldMin = newWorldMin;
                    axis.WorldMax = newWorldMax;
                }

                if (ps.XAxis2 != null)
                {
                    Axis axis = ps.XAxis2;
                    PointF pMin = ps.PhysicalXAxis2Cache.PhysicalMin;
                    PointF pMax = ps.PhysicalXAxis2Cache.PhysicalMax;

                    PointF physicalWorldMin = pMin;
                    PointF physicalWorldMax = pMax;
                    physicalWorldMin.X -= diffX;
                    physicalWorldMax.X -= diffX;
                    double newWorldMin = axis.PhysicalToWorld(physicalWorldMin, pMin, pMax, false);
                    double newWorldMax = axis.PhysicalToWorld(physicalWorldMax, pMin, pMax, false);
                    axis.WorldMin = newWorldMin;
                    axis.WorldMax = newWorldMax;
                }

                lastPoint_ = new Point(e.X, e.Y);
                this.FireInteractionOccurred();
                return true;
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
            if ((e.Button == MouseButtons.Left) && dragInitiated_)
            {
                lastPoint_ = unset_;
                dragInitiated_ = false;
            }

            return false;
        }

        private bool dragInitiated_ = false;
        private Point lastPoint_ = new Point(-1, -1);
        // this is the condition for an unset point
        private Point unset_ = new Point(-1, -1);
    }
}
