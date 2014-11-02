using Plotter.Models;
using Plotter.Tweet.Processing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Plotter.Tests.WinForms
{
    public partial class RenderingTest : Form
    {
        public RenderingTest()
        {
            InitializeComponent();
            Redraw();
        }

        private double _maxMilliseconds = 0;
        private double _maxY = 0;

        private void Redraw()
        {
            var maxMs = Math.Max(_maxMilliseconds, 500);
            var startTime = DateTime.Now;
            var midTime = DateTime.Now.AddMilliseconds(maxMs / 2);
            var maxTime = DateTime.Now.AddMilliseconds(maxMs);

            var maxY = (decimal)Math.Max(_maxY, 1);
            var midY = (decimal)(maxY / 2);

            var points = new Models.Point[]
            {
                new Models.Point(){ X = startTime, Y = 1 },
                new Models.Point(){ X = midTime, Y = midY },
                new Models.Point(){ X = maxTime, Y = maxY }
            };

            Rendering r = new Rendering(new Chart() { Title = "Test chart - adjust X and Y axes" }, points);
            byte[] result = r.RenderPng();

            MemoryStream ms = new MemoryStream(result);
            ms.Write(result, 0, result.Length);
            ms.Position = 0;
            pictureBox1.Image = Image.FromStream(ms);
        }

        //Y Axis
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            double sliderVal = (double)trackBar1.Value / 100.0; //0 to 1, linear
            double exponentialVal = Math.Pow(sliderVal, 10); //0 to 1, rising exponentially
            _maxY = exponentialVal * 1000000000; //0 to 1bn
            Redraw();

            label5.Text = "0 to " + _maxY;
        }

        //X Axis
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            double sliderVal = (double)trackBar2.Value / 100.0; //0 to 1, linear
            double exponentialVal = Math.Pow(sliderVal, 10); //0 to 1, rising exponentially
            _maxMilliseconds = exponentialVal * 1577846298735; //0 to 50 years (in milliseconds)
            Redraw();

            label6.Text = TimeSpan.FromMilliseconds(_maxMilliseconds).ToString("g");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new IOTest().Show(this);
        }
    }
}
