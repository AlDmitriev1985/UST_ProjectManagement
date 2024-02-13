using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UST_ProjectManagement
{
    public class UST_HorizontalTabControl : Control
    {
        #region --- Settings ---
        [Description("Цвет заливки кнопки при выделении")]
        public Color UST_SelectedColor { get; set; } = Color.White;

        [Description("Цвет кнопки при нажатии")]
        public Color UST_PressedColor { get; set; } = Color.SkyBlue;

        [Description("Цвет текста при нажатии")]
        public Color UST_PressedForeColor { get; set; } = Color.Black;

        #endregion

        #region --- Variables ---
        private StringFormat SF = new StringFormat();
        private bool MouseEntered = false;
        private bool MousePressed = false;
        public bool PressedStatus = false;
        #endregion

        public UST_HorizontalTabControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            Cursor = Cursors.Hand;

            Size = new Size(50, 100);

            SF.Alignment = StringAlignment.Center;
            SF.LineAlignment = StringAlignment.Center;
            //SF.FormatFlags = StringFormatFlags.DirectionVertical;

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;
            graph.SmoothingMode = SmoothingMode.HighQuality;
            graph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            graph.Clear(Parent.BackColor);

            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(0, 0, Width - 1 - Height/2, 0);
            gp.AddLine(Width - 1 - Height / 2, 0, Width - 1, Height - 1);
            gp.AddLine(Width - 1, Height - 1, 0, Height - 1);
            gp.CloseFigure();
            Rectangle rect = new Rectangle(0, 1, Width - Height / 2, Height-3);
            

            if (PressedStatus == false)
            {
                graph.DrawPath(new Pen(Parent.BackColor), gp);
                graph.FillPath(new SolidBrush(Parent.BackColor), gp);
                graph.DrawRectangle(new Pen(Parent.BackColor), rect);
                graph.DrawString(Text, Font, new SolidBrush(ForeColor), rect, SF);

                if (MouseEntered)
                {
                    graph.DrawPath(new Pen(Color.FromArgb(60, UST_SelectedColor)), gp);
                    graph.FillPath(new SolidBrush(Color.FromArgb(60, UST_SelectedColor)), gp);
                }
            }
            else
            {
                graph.DrawPath(new Pen(UST_PressedColor), gp);
                graph.FillPath(new SolidBrush(UST_PressedColor), gp);
                graph.DrawString(Text, Font, new SolidBrush(UST_PressedForeColor), rect, SF);
            }


        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            MouseEntered = true;

            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseEnter(e);
            MouseEntered = false;

            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MousePressed = true;

            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MousePressed = false;

            Invalidate();
        }

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
        }
    }
}
