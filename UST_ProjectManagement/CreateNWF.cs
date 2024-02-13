using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSTServer
{
    public class CreateNWF
    {
        public string PathRoamer = @"C:\Program Files\Autodesk\Navisworks Manage 2022\Roamer.exe";
       
        public string PathFileGFrvt { get; set; }        

        public string PathFileGFnwf { get; set; }        

        public string PathFileGFnwd { get; set; }        

        public string PathFileGFnwfGlobal { get; set; }        

        public string PathFileGFnwdGlobal { get; set; }        


        string _arg1 { get; set; } = "";
        public string arg1
        {
           get
            {
                if (_arg1 == "")
                {
                    string _arg = string.Format(" -NoGui -SaveFile {0}{1}{0} -SaveFile {0}{2}{0} -Exit {0}{3}{0}", '"', PathFileGFnwf, PathFileGFnwd, PathFileGFrvt);
                    return _arg;// _PathFileGFrvt + "8888"; 
                }
                else return _arg1;
            }
            set
            {
                _arg1 = value;
            }
        }

        string _arg2 { get; set; } = "";
        public string arg2
        {
            get
            {
                if (_arg2 == "")
                {
                    string _arg = string.Format(" -NoGui -OpenFile {0}{1}{0} -AppendFile {0}{3}{0} -SaveFile {0}{1}{0} -SaveFile {0}{2}{0} -Exit", '"', PathFileGFnwf, PathFileGFnwd, PathFileGFrvt);
                    return _arg;// _PathFileGFrvt + "8888"; 
                }
                else
                    return _arg2;
            }
            set
            {
                _arg2 = value;
            }
        }

        string _arg3 { get; set; } = "";
        public string arg3
        {
            get
            {
                if (_arg3 == "")
                {
                    string _arg = string.Format(" -NoGui -SaveFile {0}{1}{0} -SaveFile {0}{2}{0} -Exit {0}{3}{0}", '"', PathFileGFnwfGlobal, PathFileGFnwdGlobal, PathFileGFnwd);
                    return _arg;// _PathFileGFrvt + "8888"; 
                }
                else
                    return _arg3;
            }
            set
            {
                _arg3 = value;
            }
        }

        string _arg4 { get; set; } = "";
        public string arg4
        {
            get
            {
                if (_arg4 == "")
                {
                    string _arg = string.Format(" -NoGui -OpenFile {0}{1}{0} -AppendFile {0}{3}{0} -SaveFile {0}{1}{0} -SaveFile {0}{2}{0} -Exit", '"', PathFileGFnwfGlobal, PathFileGFnwdGlobal, PathFileGFnwd);
                    return _arg;// _PathFileGFrvt + "8888"; 
                }
                else
                    return _arg4;
            }
            set
            {
                _arg4 = value;
            }
        }
    }
}
