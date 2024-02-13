using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UST_ProjectManagement
{
    public partial class UC_PositionItemPanel : UserControl
    {
        public UC_PositionItemPanel(PositionInfo positionInfo = null, ProductStageInfo productStageInfo = null)
        {
            InitializeComponent();
            if (positionInfo != null)
            {
                label1.Text = positionInfo.Code;
                label2.Text = positionInfo.PersentComplete;
            }
        }
    }
}
