using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;

namespace Plotter.Tweet.Processing
{
	public class Rendering
	{
		private Models.Chart _chart;
		private Models.Point[] _points;

		public Rendering(Models.Chart chart, Models.Point[] points)
		{
			_chart = chart;
			_points = points;
		}

		public byte[] RenderPng()
		{
            var chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(223)))), ((int)(((byte)(193)))));
            chart.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chart.BorderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(64)))), ((int)(((byte)(1)))));
            chart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chart.BorderlineWidth = 2;
            chart.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Emboss;
            
            //chart.PostPaint += chart_PostPaint;
            var series = new Series(string.IsNullOrEmpty(_chart.Title) ? "Series" : _chart.Title);
            series.ChartType = SeriesChartType.Line;
            series.XValueType = ChartValueType.DateTime;
            
            foreach(var pt in _points)
            {
                series.Points.AddXY(pt.X.ToOADate(), (double)pt.Y);
            }

            chart.Series.Add(series);

            ChartArea chartArea1 = new ChartArea();
            chartArea1.Area3DStyle.Inclination = 15;
            chartArea1.Area3DStyle.IsClustered = true;
            chartArea1.Area3DStyle.IsRightAngleAxes = false;
            chartArea1.Area3DStyle.Perspective = 10;
            chartArea1.Area3DStyle.Rotation = 10;
            chartArea1.Area3DStyle.WallWidth = 0;
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisX.IsLabelAutoFit = true;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisX.LabelStyle.Interval = 0;
            chartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.MajorTickMark.Size = 2F;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.IsStartedFromZero = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.BackColor = System.Drawing.Color.OldLace;
            chartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chartArea1.BackSecondaryColor = System.Drawing.Color.White;
            chartArea1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.Name = "Default";
            chartArea1.ShadowColor = System.Drawing.Color.Transparent;

            //Make this smarter
            chartArea1.AxisX.LabelStyle.Format = GetDateTimeAxisFormat(_points.First().X, _points.Last().X);

            chart.ChartAreas.Add(chartArea1);

            if(!string.IsNullOrEmpty(_chart.Title))
            {
                chart.Legends.Add(new Legend()
                {
                    Docking = Docking.Top
                });
            }

            using (MemoryStream ms = new MemoryStream())
            {
                chart.SaveImage(ms, ChartImageFormat.Png);
                return ms.ToArray();
            }
		}


        private void chart_PostPaint(object sender, System.Windows.Forms.DataVisualization.Charting.ChartPaintEventArgs e)
        {
            // Painting series object
            if (sender is System.Windows.Forms.DataVisualization.Charting.ChartArea)
            {
                ChartArea area = (ChartArea)sender;

                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;


                RectangleF rect = new RectangleF(area.Position.X,
                                                    area.Position.Y,
                                                    area.Position.Width,
                                                    6);

                rect = e.ChartGraphics.GetAbsoluteRectangle(rect);
                e.ChartGraphics.Graphics.DrawString(area.Name,
                                        new Font("Arial", 10),
                                        Brushes.Black,
                                        rect,
                                        format);

            }
        }

        private string GetDateTimeAxisFormat(DateTime min, DateTime max)
        {
            var interval = GetSuitableIntervalFromTimespan(max - min);

            switch(interval)
            {
                case TimeInterval.Milliseconds:
                    return "HH:mm:ss.fff";
                case TimeInterval.Seconds:
                    return "HH:mm:ss";
                case TimeInterval.Minutes:
                    return "HH:mm";
                case TimeInterval.Hours:
                    return "HH:mm";
                case TimeInterval.Days:
                    return "MMM dd";
                case TimeInterval.Weeks:
                    return "MMM dd";
                case TimeInterval.Months:
                    return "MMM yyyy";
                case TimeInterval.Quarters:
                    return "MMM yyyy";
                case TimeInterval.Years:
                case TimeInterval.Decades:
                case TimeInterval.Centuries:
                    return "yyyy";
                default:
                    return "HH:mm";
            }
        }

        private TimeInterval GetSuitableIntervalFromTimespan(TimeSpan timeSpan)
        {
            if(timeSpan < TimeSpan.FromSeconds(2.5))
            {
                return TimeInterval.Milliseconds;
            } 
            else if (timeSpan < TimeSpan.FromMinutes(5))
            {
                return TimeInterval.Seconds;
            }
            else if (timeSpan < TimeSpan.FromHours(4))
            {
                return TimeInterval.Minutes;
            }
            else if (timeSpan < TimeSpan.FromHours(23))
            {
                return TimeInterval.Hours;
            }
            else if (timeSpan < TimeSpan.FromDays(15))
            {
                return TimeInterval.Days;
            }
            else if(timeSpan < TimeSpan.FromDays(45))
            {
                return TimeInterval.Weeks;
            }
            else if(timeSpan < TimeSpan.FromDays(200))
            {
                return TimeInterval.Months;
            }
            else if(timeSpan < TimeSpan.FromDays(500))
            {
                return TimeInterval.Quarters;
            }
            else if(timeSpan < TimeSpan.FromDays(365 * 10))
            {
                return TimeInterval.Years;
            }
            else if(timeSpan < TimeSpan.FromDays(365 * 100))
            {
                return TimeInterval.Decades;
            }
            else
            {
                return TimeInterval.Centuries;
            }
        }

        private enum TimeInterval
        {
            Milliseconds,
            Seconds,
            Minutes,
            Hours,
            Days,
            Weeks,
            Months,
            Quarters,
            Years,
            Decades,
            Centuries
        }
	}
}