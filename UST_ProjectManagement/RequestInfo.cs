using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryDB;
using Newtonsoft;

namespace UST_ProjectManagement
{
    public static class RequestInfo
    {
        public static LibraryDB.LibraryDB lb = new LibraryDB.LibraryDB();
        public static void requestInfoThree()
        {        
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/Info/");

                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";

                string text = "";
                text = "InfoThree";

                byte[] byteArray = Encoding.UTF8.GetBytes(text);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
                text = "";
                try
                {
                    string txt = "";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                txt += line + Environment.NewLine;
                            }
                        }
                        //lb = System.Text.Json.JsonSerializer.Deserialize<LibraryDB.LibraryDB>(stream);
                        lb = Newtonsoft.Json.JsonConvert.DeserializeObject<LibraryDB.LibraryDB>(txt);
                    }
                    response.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                    //MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
    }
}
