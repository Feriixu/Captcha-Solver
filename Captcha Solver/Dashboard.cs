using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Captcha_Solver
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Bitmap bmpRaw = new Bitmap(@"captcha.png");
            pictureBox1.BackgroundImage = bmpRaw;
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.BackgroundImage = CleanBitmap(bmpRaw, 1, 0.5f, 4);
        }

        private static Bitmap CleanBitmap(Bitmap raw, int thickness, float noiseThresh, int neighThresh)
        {
            Bitmap dirty = new Bitmap(raw);
            for (int x = 0; x < dirty.Width; x++)
            {
                for (int y = 0; y < dirty.Height; y++)
                {
                    Color pixCol = dirty.GetPixel(x, y);
                    if (pixCol.GetBrightness() > noiseThresh)
                    {
                        dirty.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        for (int i = -thickness; i < 1; i++)
                        {
                            for (int j = -thickness; j < 1; j++)
                            {
                                if ((x + i > dirty.Width) || (x + i < 0) || (y + j > dirty.Height) || (y + j < 0))
                                {
                                    continue;
                                }
                                else
                                {
                                    dirty.SetPixel(x + i, y + j, Color.Black);
                                }
                            }
                        }
                    }

                    if (x == 0 || x == dirty.Width - 1 || y == 0 || y == dirty.Height - 1)
                    {
                        dirty.SetPixel(x, y, Color.Black);
                    }
                }
            }

            for (int x = 1; x < dirty.Width - 1; x++)
            {
                for (int y = 1; y < dirty.Height - 1; y++)
                {
                    int neigh = 0;
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (dirty.GetPixel(x + i, y + j).GetBrightness() < noiseThresh)
                            {
                                neigh++;
                            }
                        }
                    }

                    if (neigh > neighThresh)
                    {
                        dirty.SetPixel(x, y, Color.Black);
                    }
                }
            }

            return new Bitmap(dirty);
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = CleanBitmap(new Bitmap(@"captcha.png"), trackBar1.Value, trackBar2.Value / 100.0f, 4);
        }

        private void TrackBar2_Scroll(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = CleanBitmap(new Bitmap(@"captcha.png"), trackBar1.Value, trackBar2.Value / 100.0f, 4);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Bitmap cleaned = new Bitmap(pictureBox1.BackgroundImage);
            cleaned.Save(@"captcha_cleaned.png");
        }
    }
}
