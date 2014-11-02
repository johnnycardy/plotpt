using Plotter.Tweet.Processing;
using System;
using System.Collections.Concurrent;
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
    public partial class IOTest : Form
    {
        private ConcurrentQueue<Tweet.Tweet> _incomingQueue;

        public IOTest()
        {
            InitializeComponent();
            label3.Text = "";
            var syncContext = new WindowsFormsSynchronizationContext();

            _incomingQueue = new ConcurrentQueue<Tweet.Tweet>();
            QueueProcessor processor = new QueueProcessor(_incomingQueue, new TestDBContext());

            processor.TweetReady += (object sender, Tweet.Tweet e) => 
            {
                syncContext.Post((object userData) =>
                {
                    label3.Text = e.Text;

                    if (e.Image != null && e.Image.Length > 0)
                    {
                        MemoryStream ms = new MemoryStream(e.Image);
                        ms.Write(e.Image, 0, e.Image.Length);
                        ms.Position = 0;
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                    else
                    {
                        pictureBox1.Image = null;
                    }
                }, null);
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            label3.Text = "";

            _incomingQueue.Enqueue(new Tweet.Tweet()
            {
                CreatorScreenName = "@test",
                Text = textBox1.Text
            });
            textBox1.Text = "";
        }
    }
}
