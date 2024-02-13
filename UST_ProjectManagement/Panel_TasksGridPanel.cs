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
    public class Panel_TasksGridPanel: Panel
    {
        #region --- Settings ---
        [Description("Список разделов")]
        public List<string> SetsList { get; set; }

        [Description("Шаг сетки")]
        public int GridStep { get; set; } = 100;

        [Description("Высота шапки")]
        public int HeaderHeight { get; set; } = 30;

        [Description("Высота шапки")]
        public Color GridColor { get; set; } = Color.Silver;
        #endregion

        private StringFormat SF = new StringFormat();
        private StringFormat SFV = new StringFormat();

        public Panel_TasksGridPanel()
        {
            SF.Alignment = StringAlignment.Center;
            SF.LineAlignment = StringAlignment.Center;

            SFV.Alignment = StringAlignment.Center;
            SFV.LineAlignment = StringAlignment.Center;
            SFV.FormatFlags = StringFormatFlags.DirectionVertical;


            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;
            graph.SmoothingMode = SmoothingMode.HighQuality;
            graph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            int step = GridStep;
            int num = 0;
            if (SetsList != null && SetsList.Count > 0)
            {
                num = SetsList.Count;
            }
            int X = HeaderHeight;
            int Y = HeaderHeight;
            float L = 0;
            //float L = Math.Max(Parent.Width, (num + 1) * GridStep);
            if (num > 0)
            {
                L = HeaderHeight + (num + 1) * step;
                
            }
            if (L < 200)
            {
                num = 200 / step;
                L = HeaderHeight + (num + 1) * step;
            }
            //float H = Math.Max(Parent.Height, (num + 1) * GridStep);
            float H = HeaderHeight + (num + 1) * step;

            Rectangle rectTop = new Rectangle(0, 0, Convert.ToInt32(L), HeaderHeight);
            graph.DrawRectangle(new Pen(Color.FromArgb(60, MainForm.HeaderColor)), rectTop);
            graph.FillRectangle(new SolidBrush(Color.FromArgb(60, MainForm.HeaderColor)), rectTop);
            graph.DrawString("Получатель", Font, new SolidBrush(Color.Black), rectTop, SF);

            Rectangle rectLeft = new Rectangle(0, 0, HeaderHeight, Convert.ToInt32(H));
            graph.DrawRectangle(new Pen(Color.FromArgb(60, MainForm.HeaderColor)), rectLeft);
            graph.FillRectangle(new SolidBrush(Color.FromArgb(60, MainForm.HeaderColor)), rectLeft);
            graph.DrawString("Отправитель", Font, new SolidBrush(Color.Black), rectLeft, SFV);

            Rectangle tableFrame = new Rectangle(0, 0, Convert.ToInt32(L), Convert.ToInt32(H));
            graph.DrawRectangle(new Pen(GridColor), tableFrame);

            if (num == 0)
            {
                while (X < this.Width)
                {
                    GraphicsPath gp = new GraphicsPath();
                    X += step;
                    gp.AddLine(X, HeaderHeight, X, H);
                    graph.DrawPath(new Pen(GridColor), gp);
                    //graph.FillPath(new SolidBrush(Color.Silver), gp);
                }
                while (Y < this.Height)
                {
                    GraphicsPath gp = new GraphicsPath();
                    Y += step;
                    gp.AddLine(HeaderHeight, Y, L, Y);
                    graph.DrawPath(new Pen(GridColor), gp);
                    //graph.FillPath(new SolidBrush(Color.Silver), gp);
                }
            }
            else
            {
                for (int i = 0; i <= num; i++)
                {
                    X += step;
                    Y += step;
                    if (i <= num)
                    {
                        GraphicsPath gp = new GraphicsPath();                     
                        gp.AddLine(X, HeaderHeight, X, H);
                        graph.DrawPath(new Pen(GridColor), gp);
                        //graph.FillPath(new SolidBrush(Color.Silver), gp);

                        GraphicsPath gpY = new GraphicsPath();                 
                        gpY.AddLine(HeaderHeight, Y, L, Y);
                        graph.DrawPath(new Pen(GridColor), gpY);

                    }

                    Rectangle rectX = new Rectangle(X - step, HeaderHeight, step, step);
                    try
                    {
                        graph.DrawString(SetsList[i - 1], Font, new SolidBrush(Color.Black), rectX, SF);
                    }
                    catch { }

                    Rectangle rectY = new Rectangle(HeaderHeight, Y - step, step, step);
                    try
                    {
                        graph.DrawString(SetsList[i - 1], Font, new SolidBrush(Color.Black), rectY, SF);
                    }
                    catch { }
                }
                
            }

            CreateTaskPanels(num, graph);

            this.Size = new Size(Convert.ToInt32(Math.Max((L + 1), Parent.Width)), Convert.ToInt32(Math.Max((H + 1), Parent.Height)));
        }

        private void CreateTaskPanels(int num, Graphics graph)
        {
            int X = HeaderHeight;
            int Y = HeaderHeight;
            int step = GridStep;

            for (int x = 0; x <= num; x++)
            {
                for(int y = 0; y <= num; y++)
                {
                    if (x == y)
                    {
                        Rectangle rectClose = new Rectangle(X + x * step, Y + y * step, step, step);
                        graph.FillRectangle(new SolidBrush(Color.FromArgb(30, MainForm.HeaderColor)), rectClose);
                    }
                }
            }
        }
    }
}
