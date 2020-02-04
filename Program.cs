using System;
using System.IO;
using System.Text;

namespace mansionApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string result = "ID,マンション名,住所,画像URL,PAGE\n";
            //string appPath = @"C:\Users\matsu\Desktop\mansion\";
            string appPath = Directory.GetCurrentDirectory() + @"\";
            string filePath = appPath + "mansiondata.csv";
            string logPath = appPath + "log.txt";

            if (!File.Exists(filePath)) File.WriteAllText(filePath, result);

            DateTime dt = DateTime.Now;
            string log = dt.ToString() + "\n";
            File.WriteAllText(logPath, log);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            StreamReader sr = new StreamReader("read.csv", Encoding.GetEncoding("UTF-8"));
            string rd = sr.ReadLine();

            if (rd == "all")
            {

                while (sr.Peek() != -1)
                {
                    string date = sr.ReadLine();

                    File.AppendAllText(logPath, date);

                    Console.WriteLine(date);
                    result = transtext(appPath, date);

                    File.AppendAllText(filePath, result);

                }
            }
            else if (rd == "range")
            {
                string date = sr.ReadLine();
                int start = Int32.Parse(date);
                date = sr.ReadLine();
                int end = Int32.Parse(date);

                for(int i=start; i-1 < end; i++)
                {
                    string j = i.ToString();

                    File.AppendAllText(logPath, j);

                    Console.WriteLine(j);
                    result = transtext(appPath, j);

                    File.AppendAllText(filePath, result);
                }
            }

            sr.Close();

        }

        static string transtext(string path, string name)
        {
            string result = "";
            string result2 = "";

            bool comp = false;
            string logPath = path + "log.txt";

            string fn = path + name + ".html";

            if (File.Exists(fn))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                StreamReader sr = new StreamReader(fn, Encoding.GetEncoding("UTF-8"));

                while (sr.Peek() != -1)
                {
                    String line = sr.ReadLine();

                    line = line.Replace("<li class=\"raListItem\"><a class=\"building\" href=\"/buildings/", "\n");
                    line = line.Replace("/\"><div class=\"photo\">", ",");
                    line = line.Replace("<img src=\"", "");
                    line = line.Replace("<p class=\"none\">", "");
                    line = line.Replace("</p></div>", "");
                    line = line.Replace("\" /></div>", "");
                    line = line.Replace("<div class=\"spec\"><h2 class=\"name\">", ",");
                    line = line.Replace("</h2><p class=\"address\">", ",");
                    line = line.Replace("</a></li>", ",");
                    line = line.Replace("&#39;", "'");
                    line = line.Replace("&amp;", "&");
                    line = line.Replace("&lt;", "<");
                    line = line.Replace("&gt;", ">");


                    string[] arr2 = line.Split('!');
                    string[] arr = line.Split(',');

                    if (arr.Length > 3)
                    {
                        if (arr2.Length < 5)
                        {
                            result = result + line;
                            comp = true;
                        }
                    }
                }

                sr.Close();

                System.IO.StringReader rs = new System.IO.StringReader(result);

                rs.ReadLine();

                while (rs.Peek() > -1)
                {
                    String line = rs.ReadLine();

                    string[] arr = line.Split(',');

                    result2 = result2 + arr[0] + "," + arr[2] + "," + arr[3] + "," + arr[1] + "," + name + "\n";

                }

                rs.Close();



                if (comp)
                {
                    File.AppendAllText(logPath, ":OK!\n");
                }
                else
                {
                    File.AppendAllText(logPath, ":NG!\n");
                }

            }
            else
            {
                File.AppendAllText(logPath, ":No File\n");
            }

            return result2;

        }
    }
}
