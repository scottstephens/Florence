using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace NPlot.WinForms.Interactions
{
    /// <summary>
    /// 
    /// </summary>
    public class AxisDrag : WinFormsInteraction<NPlot.Interactions.AxisDrag>
    {
        public AxisDrag(NPlot.Interactions.AxisDrag interaction) : base(interaction) { }

        

        private bool enableDragWithCtr_ = false;
        private Axis axis_ = null;
        private bool doing_ = false;
        private Point lastPoint_ = new Point();
        private PhysicalAxis physicalAxis_ = null;
        private Point startPoint_ = new Point();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ctr"></param>
        public override bool DoMouseDown(MouseEventArgs e, Control ctr)
        {
            // if the mouse is inside the plot area [the tick marks are here and part of the 
            // axis], then don't invoke drag. 
            NPlot.PlotSurface2D ps = ((WinForms.WinFormsPlotSurface2D)ctr).Inner;
            if (e.X > ps.PlotAreaBoundingBoxCache.Left && e.X < ps.PlotAreaBoundingBoxCache.Right &&
                e.Y > ps.PlotAreaBoundingBoxCache.Top && e.Y < ps.PlotAreaBoundingBoxCache.Bottom)
            {
                return false;
            }

            if ((e.Button == MouseButtons.Left))
            {
                // see if hit with axis.
                ArrayList objects = ps.HitTest(new Point(e.X, e.Y));

                foreach (object o in objects)
                {
                    if (o is NPlot.Axis)
                    {
                        doing_ = true;
                        axis_ = (Axis)o;

                        PhysicalAxis[] physicalAxisList = new PhysicalAxis[] { ps.PhysicalXAxis1Cache, ps.PhysicalXAxis2Cache, ps.PhysicalYAxis1Cache, ps.PhysicalYAxis2Cache };

                        if (ps.PhysicalXAxis1Cache.Axis == axis_)
                        {
                            physicalAxis_ = ps.PhysicalXAxis1Cache;
                        }
                        else if (ps.PhysicalXAxis2Cache.Axis == axis_)
                        {
                            physicalAxis_ = ps.PhysicalXAxis2Cache;
                        }
                        else if (ps.PhysicalYAxis1Cache.Axis == axis_)
                        {
                            physicalAxis_ = ps.PhysicalYAxis1Cache;
                        }
                        else if (ps.PhysicalYAxis2Cache.Axis == axis_)
                        {
                            physicalAxis_ = ps.PhysicalYAxis2Cache;
                        }

                        lastPoint_ = startPoint_ = new Point(e.X, e.Y);

                        return false;
                    }
                }
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
            NPlot.PlotSurface2D ps = ((WinForms.WinFormsPlotSurface2D)ctr).Inner;

            // if dragging on axis to zoom.
            if ((e.Button == MouseButtons.Left) && doing_ && physicalAxis_ != null)
            {
                if (enableDragWithCtr_ && lastKeyEventArgs != null && lastKeyEventArgs.Control)
                {
                }
                else
                {
                    float dist =
                        (e.X - lastPoint_.X) +
                        (-e.Y + lastPoint_.Y);

                    lastPoint_ = new Point(e.X, e.Y);

                    if (dist > sensitivity_ / 3.0f)
                    {
                        dist = sensitivity_ / 3.0f;
                    }

                    PointF pMin = physicalAxis_.PhysicalMin;
                    PointF pMax = physicalAxis_.PhysicalMax;
                    double physicalWorldLength = Math.Sqrt((pMax.X - pMin.X) * (pMax.X - pMin.X) + (pMax.Y - pMin.Y) * (pMax.Y - pMin.Y));

                    float prop = (float)(physicalWorldLength * dist / sensitivity_);
                    prop *= 2;

                    ((WinForms.WinFormsPlotSurface2D)ctr).CacheAxes();

                    float relativePosX = (startPoint_.X - pMin.X) / (pMax.X - pMin.X);
                    float relativePosY = (startPoint_.Y - pMin.Y) / (pMax.Y - pMin.Y);

                    if (float.IsInfinity(relativePosX) || float.IsNaN(relativePosX))
                    {
                        relativePosX = 0.0f;
                    }

                    if (float.IsInfinity(relativePosY) || float.IsNaN(relativePosY))
                    {
                        relativePosY = 0.0f;
                    }

                    PointF physicalWorldMin = pMin;
                    PointF physicalWorldMax = pMax;

                    physicalWorldMin.X += relativePosX * prop;
                    physicalWorldMax.X -= (1 - relativePosX) * prop;
                    physicalWorldMin.Y -= relativePosY * prop;
                    physicalWorldMax.Y += (1 - relativePosY) * prop;

                    double newWorldMin = axis_.PhysicalToWorld(physicalWorldMin, pMin, pMax, false);
                    double newWorldMax = axis_.PhysicalToWorld(physicalWorldMax, pMin, pMax, false);
                    axis_.WorldMin = newWorldMin;
                    axis_.WorldMax = newWorldMax;

                    this.FireInteractionOccurred();

                    return true;
                }
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
            if (doing_)
            {
                doing_ = false;
                axis_ = null;
                physicalAxis_ = null;
                lastPoint_ = new Point();
            }

            return false;
        }
        private float sensitivity_ = 200.0f;


        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public float Sensitivity
        {
            get
            {
                return sensitivity_;
            }
            set
            {
                sensitivity_ = value;
            }
        }
    }
}
