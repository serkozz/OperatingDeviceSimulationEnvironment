using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace СourseWork
{
    class CircleButton : Button
    {

        private Color color = Color.Cyan;

        private float cirlceSize;

        private bool cross;

        //Properties
        public Color Color
        { 
            get => color; 
            set
            {
                color = value;
                this.Invalidate();
            }
        }

        public float CircleSize
        {
            get => cirlceSize;
            set
            {
                cirlceSize = value;
                this.Invalidate();
            }
        }

        public bool Cross
        {
            get => cross;
            set
            {
                cross = value;
                this.Invalidate();
            }
        }

        public CircleButton()
        {
            this.Size = new Size(22, 22);
            cirlceSize = 16F;
            cross = false;
        }

        //Overridden methods
        protected override void OnPaint(PaintEventArgs pevent)
        {
            //Fields
            Graphics graphics = pevent.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF rectRbBorder = new RectangleF()
            {
                X = 0.5F,
                Y = (this.Height - cirlceSize) / 2, //Center
                Width = cirlceSize,
                Height = cirlceSize
            };


            //Drawing
            using Pen penBorder = new(Color, 1.6F);
            using SolidBrush brushText = new(this.ForeColor);
            //Draw surface
            graphics.Clear(this.BackColor);
            //Draw Radio Button

            graphics.DrawEllipse(penBorder, rectRbBorder);//Circle border

            if (cross)
            {
                graphics.DrawLine(penBorder, 5, 6, 14, 15);
                graphics.DrawLine(penBorder, 5, 15, 14, 6);
            }
            //Draw text
            graphics.DrawString(this.Text, this.Font, brushText,
                cirlceSize + 8, (this.Height - TextRenderer.MeasureText(this.Text, this.Font).Height) / 2);//Y=Center
        }
    }


}
