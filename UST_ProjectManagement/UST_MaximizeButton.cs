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
    public class UST_MaximizeButton : Control
    {
        #region --- Settings ---
        [Description("Цвет кнопки при выделении")]
        public Color UST_SelectedColor { get; set; } = Color.SkyBlue;

        [Description("Степень изменения цвета кнопки при выделении. От 0 до 255")]
        public int UST_SelectionAlpha { get; set; } = 100;

        [Description("Цвет кнопки при нажатии")]
        public Color UST_PressedColor { get; set; } = Color.SkyBlue;

        [Description("Степень изменения цвета кнопки при нажантии. От 0 до 255")]
        public int UST_PressedAlpha { get; set; } = 160;

        [Description("Цвет иконки (Условного обозначения)")]
        public Color UST_IconColor { get; set; } = Color.White;

        [Description("Вес линии иконки (Условного обозначения)")]
        public float UST_IconLineSize { get; set; } = 2;

        [Description("Вес линии иконки (Условного обозначения)")]
        public int UST_IconOffset { get; set; } = 10;

        #endregion

        #region --- Variables ---
        private StringFormat SF = new StringFormat();
        private bool MouseEntered = false;
        private bool MousePressed = false;
        private bool Maximized = false;
        #endregion

        public UST_MaximizeButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            Cursor = Cursors.Hand;

            Size = new Size(100, 100);

            SF.Alignment = StringAlignment.Center;
            SF.LineAlignment = StringAlignment.Center;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;
            graph.SmoothingMode = SmoothingMode.HighQuality;
            graph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            graph.Clear(Parent.BackColor);
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            graph.DrawRectangle(new Pen(Parent.BackColor), rect);
            graph.FillRectangle(new SolidBrush(BackColor), rect);



            if (MouseEntered)
            {
                ///Отрисовка контура
                graph.DrawRectangle(new Pen(Color.FromArgb(UST_SelectionAlpha, UST_SelectedColor)), rect);
                ///Заливка кнопки
                graph.FillRectangle(new SolidBrush(Color.FromArgb(UST_SelectionAlpha, UST_SelectedColor)), rect);
            }

            if (MousePressed)
            {
                ///Отрисовка контура
                graph.DrawRectangle(new Pen(Color.FromArgb(UST_PressedAlpha, UST_PressedColor)), rect);
                ///Заливка кнопки
                graph.FillRectangle(new SolidBrush(Color.FromArgb(UST_PressedAlpha, UST_PressedColor)), rect);
            }

            Pen IconPen = new Pen(UST_IconColor, UST_IconLineSize);
            int offset = 2;
            if (Maximized)
            {
                graph.DrawLine(IconPen, UST_IconOffset, UST_IconOffset + offset, Width - UST_IconOffset - offset - 1, UST_IconOffset + offset);
                graph.DrawLine(IconPen, Width - UST_IconOffset - offset - 1, UST_IconOffset + offset, Width - UST_IconOffset - offset - 1, Height - UST_IconOffset - 1);
                graph.DrawLine(IconPen, Width - UST_IconOffset - offset - 1, Height - UST_IconOffset - 1, UST_IconOffset, Height - UST_IconOffset - 1);
                graph.DrawLine(IconPen, UST_IconOffset, Height - UST_IconOffset - 1, UST_IconOffset, UST_IconOffset + offset);

                graph.DrawLine(IconPen, UST_IconOffset + offset, UST_IconOffset + offset, UST_IconOffset + offset, UST_IconOffset);
                graph.DrawLine(IconPen, UST_IconOffset + offset, UST_IconOffset, Width - UST_IconOffset - 1, UST_IconOffset);
                graph.DrawLine(IconPen, Width - UST_IconOffset - 1, UST_IconOffset, Width - UST_IconOffset - 1, Height - UST_IconOffset - offset - 1);
                graph.DrawLine(IconPen, Width - UST_IconOffset - 1, Height - UST_IconOffset - offset - 1, Width - UST_IconOffset - offset - 1, Height - UST_IconOffset - offset - 1);
            }
            else
            {
                graph.DrawLine(IconPen, UST_IconOffset, UST_IconOffset, Width - UST_IconOffset - 1, UST_IconOffset);
                graph.DrawLine(IconPen, Width - UST_IconOffset - 1, UST_IconOffset, Width - UST_IconOffset - 1, Height - UST_IconOffset - 1);
                graph.DrawLine(IconPen, Width - UST_IconOffset - 1, Height - UST_IconOffset - 1, UST_IconOffset, Height - UST_IconOffset - 1);
                graph.DrawLine(IconPen, UST_IconOffset, Height - UST_IconOffset - 1, UST_IconOffset, UST_IconOffset);
            }

        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            //NormalSize = false;
            MouseEntered = true;

            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseEnter(e);
            //NormalSize = true;
            MouseEntered = false;

            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MousePressed = true;
            if (Maximized == false)
            {
                Maximized = true;
            }
            else
            {
                Maximized = false;
            }

            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MousePressed = false;

            Invalidate();
        }
    }
}
