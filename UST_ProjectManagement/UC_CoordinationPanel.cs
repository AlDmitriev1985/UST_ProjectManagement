using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UST_UILibrary;
using POSTServer.History;
using Newtonsoft.Json;

namespace UST_ProjectManagement
{
    public partial class UC_CoordinationPanel : UserControl
    {
        PositionInfo positionInfo;
        public UC_CoordinationPanel()
        {
            InitializeComponent();
        }

        public void UpdateCoordinates()
        {

        }

        private void dataGridView1_SizeChanged(object sender, EventArgs e)
        {
            int width = dataGridView_History.Width;

            dataGridView_History.Columns[0].Width = 50;
            dataGridView_History.Columns[1].Width = 150;
            dataGridView_History.Columns[2].Width = 100;

            int colwidth = (width - dataGridView_History.Columns[0].Width - dataGridView_History.Columns[1].Width - dataGridView_History.Columns[2].Width) / 5;
            dataGridView_History.Columns[3].Width = colwidth;
            dataGridView_History.Columns[4].Width = colwidth;
            dataGridView_History.Columns[5].Width = colwidth;
            dataGridView_History.Columns[6].Width = colwidth;
            dataGridView_History.Columns[7].Width = colwidth;

            dataGridView_History.Columns[2].Width = 100 - 2;
        }

        public void GetCoordinates()
        {
            if (GlobalData.SelectedPosition != null)
            {
                string[] sCoord = GlobalData.SelectedPosition.PositionCoordinate.Split(';');
                label7.Text = sCoord[0];
                label8.Text = sCoord[1];
                label9.Text = sCoord[2];
                double zRad = 0;
                try
                {
                    zRad = UILibrary.ConvertFromStringToDouble(sCoord[3]);
                }
                catch { }
                double zDeg = 0;
                if (zRad != 0) zDeg = Math.Round(zRad * 180 / 3.14, 2);
                label10.Text = zRad.ToString() + " рад. / " + zDeg.ToString() + "°";
            }
        }

        public void UpdateHistoryDG()
        {
            dataGridView_History.Rows.Clear();
            if (GlobalData.SelectedProject != null)
            {
                positionInfo = new PositionInfo(GlobalData.SelectedProject, GlobalData.SelectedStage, GlobalData.SelectedPosition);
                var history = positionInfo.coordinationHistory.OrderByDescending(x => x.Date);

                int i = history.Count();
                foreach (var _pos in history)
                {
                    DataGridViewRow _row = new DataGridViewRow();
                    _row.CreateCells(dataGridView_History);
                    _row.Cells[0].Value = i;
                    _row.Cells[1].Value = _pos.User;
                    LibraryDB.DB.User user = RequestInfo.lb.Users.FirstOrDefault(u => u.UserAccount == _pos.User);
                    if (user != null && user.FunctionId == 7)
                    {
                        _row.Cells[2].Value = "скоорд.";
                        for (int c = 0; c < _row.Cells.Count; c++)
                        {
                            _row.Cells[c].Style.BackColor = Color.LightGray;
                        }
                    }
                    else
                    {
                        _row.Cells[2].Value = "передано";
                    }
                    string[] splitcoord = _pos.Info.Split(';');
                    _row.Cells[3].Value = splitcoord[0];
                    _row.Cells[4].Value = splitcoord[1];
                    _row.Cells[5].Value = splitcoord[2];
                    _row.Cells[6].Value = splitcoord[3];
                    _row.Cells[7].Value = _pos.Date;
                    _row.Height = 30;
                    dataGridView_History.Rows.Add(_row);
                    i -= 1;
                } 
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
