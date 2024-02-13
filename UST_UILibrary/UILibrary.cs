using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UST_UILibrary
{
    public static class UILibrary
    {
        /// <summary>
        /// Получить коллекцию дочерних контролов звденного типа, содержащихся в выбранном контроле
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="ctrlList"></param>
        /// <param name="type"></param>
        public static void GetAllTypedControls(Control ctrl, List<Control> ctrlList, Type type)
        {
            if (ctrl.GetType() == type)
            {
                ctrlList.Add(ctrl);
            }
            foreach (Control ctrlChild in ctrlList)
            {
                GetAllTypedControls(ctrlChild, ctrlList, type);
            }
        }

        public static double ConvertFromStringToDouble(string txt)
        {
            double result = 0;
            if (txt != "")
            {
                try
                {
                    result = Convert.ToDouble(txt);
                }
                catch
                {
                    if (txt.Contains("."))
                    {
                        txt = txt.Replace(".", ",");
                    }
                    else
                    {
                        txt = txt.Replace(",", ".");
                    }
                    result = Convert.ToDouble(txt);
                }
            }
            return result;
        }
    }
}
