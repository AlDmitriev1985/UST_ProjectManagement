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
    public class UST_TaskButton: Button
    {
        #region --- Settings ---
        [Description("Цвет заливки кнопки при выделении")]
        public Color UST_SelectedColor { get; set; } = Color.White;

        [Description("Цвет кнопки при нажатии")]
        public Color UST_PressedColor { get; set; } = Color.SkyBlue;

        [Description("Цвет текста при нажатии")]
        public Color UST_PressedForeColor { get; set; } = Color.Black;

        [Description("ID передающего раздела")]
        public int IdFrom { get; set; }

        [Description("ID принимающего раздела")]
        public int IdTo { get; set; }

        public bool Created { get; set; } = false;

        [Description("Статус задания")]
        public int Status { get; set; }
        #endregion

        #region --- Variables ---
        private StringFormat SF = new StringFormat();
        private bool MouseEntered = false;
        private bool MousePressed = false;
        #endregion

        public UST_TaskButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            Cursor = Cursors.Hand;

            Size = new Size(100, 100);

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

            Rectangle rect = new Rectangle(0, 0, Size.Width, Size.Height);

            Color statusColor = Parent.BackColor;
            //Color statusColor = Color.LightGray;
            if (!Created)
            {

                if (MouseEntered)
                {
                    statusColor = Color.Gainsboro;
                    graph.DrawRectangle(new Pen(statusColor), rect);
                    graph.FillRectangle(new SolidBrush(statusColor), rect);
                }
                else
                {
                    statusColor = Parent.BackColor;
                    graph.DrawRectangle(new Pen(statusColor), rect);
                    graph.FillRectangle(new SolidBrush(statusColor), rect);
                }
            }
            else
            {
                switch (Status)
                {
                    case 5:
                        statusColor = Color.Gainsboro;
                        break;
                    case 6:
                        statusColor = Color.Yellow;
                        break;
                    case 8:
                        statusColor = Color.GreenYellow;
                        break;
                    case 9:
                        statusColor = Color.GreenYellow;
                        break;
                    case 10:
                        statusColor = Color.Green;
                        break;
                }
                if (MouseEntered)
                {
                    graph.DrawRectangle(new Pen(Color.FromArgb(60, statusColor)), rect);
                    graph.FillRectangle(new SolidBrush(Color.FromArgb(60, statusColor)), rect);
                }
                else
                {
                    graph.DrawRectangle(new Pen(statusColor), rect);
                    graph.FillRectangle(new SolidBrush(statusColor), rect);
                }

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
