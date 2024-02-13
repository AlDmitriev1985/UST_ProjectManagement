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
    public class UST_HorizontalTabControl_Settings : Control
    {
        #region --- Settings ---
        [Description("Цвет заливки кнопки при выделении")]
        public Color UST_SelectedColor { get; set; } = Color.White;

        [Description("Цвет кнопки при нажатии")]
        public Color UST_PressedColor { get; set; } = Color.SkyBlue;

        [Description("Цвет текста при нажатии")]
        public Color UST_PressedForeColor { get; set; } = Color.Black;

        [Description("Цвет границы")]
        public Color UST_BorderColor { get; set; } = Color.WhiteSmoke;

        [Description("Тип контроля: 0 - крайний; 1 - рядовой")]
        public int UST_TabType { get; set; } = 1;

        #endregion

        #region --- Variables ---
        private StringFormat SF = new StringFormat();
        private bool MouseEntered = false;
        private bool MousePressed = false;
        public bool PressedStatus = false;
        #endregion

        public UST_HorizontalTabControl_Settings()
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

            int b1 = 0;
            if (UST_TabType == 0)
            {
                b1 = 0;
            }
            else
            {
                b1 = 0;
            }
            
            int b2 = 0;

            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(b1, 0, Width - 1, 0);
            gp.AddLine(Width - 1, 0, Width - b2 - 1, Height - 1);
            gp.AddLine(Width - b2 - 1, Height - 1, 0, Height - 1);
            gp.CloseFigure();
            Rectangle rect = new Rectangle(b1, 0, Width - b1 - b2 - 1, Height-1);

           // graph.DrawLine(new Pen(UST_BorderColor, 4), b1, 0, 0, Height - 1);
            graph.DrawLine(new Pen(UST_BorderColor, 4), Width - 2, 0, Width - b2 - 2, Height);

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
