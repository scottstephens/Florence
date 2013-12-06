/*
 * Florence - A charting library for .NET
 * 
 * Line.cs
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
using System.Drawing;

namespace Florence
{
    /// <summary>
    /// Encapsulates functionality for drawing a line on a plot surface.
    /// </summary>
    public class Line : IPlot
    {
        /// <summary>
        /// Intercept of the line.
        /// </summary>
        public double Intercept { get; set; }

        /// <summary>
        /// Slope of the line.
        /// </summary>
        public double Slope { get; set; }

        /// <summary>
        /// Pen to draw the line with. Controls color, dash style, width, etc.
        /// </summary>
        public Pen Pen { get; set; }
        
        /// <summary>
        /// Whether to show an entry for this line in the legend. Defaults to false.
        /// </summary>
        public bool ShowInLegend { get; set; }

        /// <summary>
        /// A label for the legend entry, if shown. Defaults to an empty string.
        /// </summary>
        public string Label { get; set; }

        public Color Color
        {
            get
            {
                return this.Pen.Color;
            }
            set
            {
                if (this.Pen == null)
                    this.Pen = new Pen(value);
                else
                    this.Pen.Color = value;
            }
        }

        /// <summary>
        /// Default constructor; solid black line with intercept 0, slope 1
        /// </summary>
        public Line()
        {
            this.Intercept = 0;
            this.Slope = 1;
            this.Pen = new Pen(System.Drawing.Color.Black);
        }

        /// <summary>
        /// Constructor; line is described using intercept and slope.
        /// </summary>
        /// <param name="intercept">The value of the line at X=0.</param>
        /// <param name="slope">The slope of the line.</param>
        public Line(double intercept, double slope)
            : this()
        {
            this.Intercept = intercept;
            this.Slope = slope;
        }

        /// <summary>
        /// Draws the line on a GDI+ surface against the provided x and y axes.
        /// </summary>
        /// <param name="g">The GDI+ surface on which to draw.</param>
        /// <param name="xAxis">The X-Axis to draw against.</param>
        /// <param name="yAxis">The Y-Axis to draw against.</param>
        public void Draw(System.Drawing.Graphics g, PhysicalAxis xAxis, PhysicalAxis yAxis)
        {
            int xMin = xAxis.PhysicalMin.X;
            int xMax = xAxis.PhysicalMax.X;

            int yMin = yAxis.PhysicalMin.Y;
            int yMax = yAxis.PhysicalMax.Y;

            PointD[] world_points = new PointD[2];
            int point_index = 0;
            // intercept point on left border
            var value_at_left_border = this.Intercept + this.Slope * xAxis.Axis.WorldMin;
            if (value_at_left_border >= yAxis.Axis.WorldMin && value_at_left_border < yAxis.Axis.WorldMax)
            {
                world_points[point_index] = new PointD(xAxis.Axis.WorldMin, value_at_left_border);
                point_index += 1;                
            }

            // intercept point on right border
            var value_at_right_border = this.Intercept + this.Slope * xAxis.Axis.WorldMax;
            if (value_at_right_border > yAxis.Axis.WorldMin && value_at_right_border <= yAxis.Axis.WorldMax)
            {
                world_points[point_index] = new PointD(xAxis.Axis.WorldMax, value_at_right_border);
                point_index += 1;
            }
            
            // intercept point on bottom border
            var value_at_bottom_border = (yAxis.Axis.WorldMin - this.Intercept) / this.Slope;
            if (value_at_bottom_border > xAxis.Axis.WorldMin && value_at_bottom_border <= xAxis.Axis.WorldMax)
            {
                world_points[point_index] = new PointD(value_at_bottom_border, yAxis.Axis.WorldMin);
                point_index += 1;
            }

            // intercept point on top border
            var value_at_top_border = (yAxis.Axis.WorldMax - this.Intercept) / this.Slope;
            if (value_at_top_border >= xAxis.Axis.WorldMin && value_at_top_border < xAxis.Axis.WorldMax)
            {
                world_points[point_index] = new PointD(value_at_top_border, yAxis.Axis.WorldMax);
            }

            var physical_point = new Point[2];
            physical_point[0] = WorldToPhysical(world_points[0], xAxis, yAxis);
            physical_point[1] = WorldToPhysical(world_points[1], xAxis, yAxis);

            g.DrawLine(this.Pen, physical_point[0], physical_point[1]);

            // todo:  clip and proper logic for flipped axis min max.
        }

        private static Point WorldToPhysical(PointD world_point, PhysicalAxis xAxis, PhysicalAxis yAxis)
        {
            return new Point((int)xAxis.WorldToPhysical(world_point.X, false).X, (int)yAxis.WorldToPhysical(world_point.Y, false).Y);
        }

        /// <summary>
        /// Draws a representation of the line in the legend
        /// </summary>
        /// <param name="g">The graphics surface on which to draw.</param>
        /// <param name="startEnd">A rectangle specifying the bounds of the area in the legend set aside for drawing.</param>
        public void DrawInLegend(System.Drawing.Graphics g, System.Drawing.Rectangle startEnd)
        {
            g.DrawLine(this.Pen, startEnd.Left, (startEnd.Top + startEnd.Bottom) / 2,
                startEnd.Right, (startEnd.Top + startEnd.Bottom) / 2);
        }
        
        /// <summary>
        /// Returns null indicating that x extremities of the line are variable.
        /// </summary>
        /// <returns></returns>
        public Axis SuggestXAxis()
        {
            return null;
        }

        /// <summary>
        /// Returns null indicating that y extremities of the line are variable.
        /// </summary>
        /// <returns></returns>
        public Axis SuggestYAxis()
        {
            return null;
        }

        /// <summary>
        /// Writes text data describing the line object to the supplied string builder.
        /// </summary>
        /// <param name="sb">the StringBuilder object to write to.</param>
        /// <param name="region">a region used if onlyInRegion is true</param>
        /// <param name="onlyInRegion">If ture, data will be written on ly if the line is in the specified region.</param>
        public void WriteData(StringBuilder sb, RectangleD region, bool onlyInRegion)
        {
            sb.Append("Label: ");
            sb.Append(this.Label);
            sb.Append("\r\n");
            sb.Append("Intercept: ");
            sb.Append(this.Intercept.ToString());
            sb.Append(", Slope: ");
            sb.Append(this.Slope.ToString());
            sb.Append("\r\n");
        }


    }
}
