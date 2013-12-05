using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DailyReportAssistant
{
    public class GlobalVar
    {
        private static String _date = "";
        private static String _filePath = "";
        private static String _allText = "";
        private static Encoding _fileEncoding = Encoding.Default;
        private static String[] _yesterdayDailyReport = new String[5];
        private static String[] _todayDailyReport = new String[5];
        private static String[] _dailyReport = new String[5];
        

        public static String date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }
        public static String filePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
            }
        }
        public static String allText
        {
            get
            {
                return _allText;
            }
            set
            {
                _allText = value;
            }
        }
        public static Encoding fileEncoding
        {
            get
            {
                return _fileEncoding;
            }
            set
            {
                _fileEncoding = value;
            }
        }
        public static String[] dailyReport
        {
            get
            {
                return _dailyReport;
            }
            set
            {
                String[] _dailyReport = (String[])value.Clone();
            }
        }

        public static String[] yesterdayDailyReport
        {
            get
            {
                return _yesterdayDailyReport;
            }
            set
            {
                String[] _yesterdayDailyReport = (String[])value.Clone();
            }
        }
         
        public static String[] todayDailyReport
        {
            get
            {
                return _todayDailyReport;
            }
            set
            {
                String[] _todayDailyReport = (String[])value.Clone();
            }
        }
    }

    public class Transcation
    {
        // 设置日报路径
        public static bool setFilePath(String filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File Path Error. Cannot Find .txt File.");
                Console.WriteLine("Error encountered during Transtation.setFilePath().");
                return false;
            }

            GlobalVar.filePath = filePath;
            return true;
        }

        // 根据日报路径获取日报内容
        public static String getFileText()
        {
            if (GlobalVar.filePath.Length == 0)
            {
                Console.WriteLine("未找到");
                return "";
            }
            // 要增加对文件编码的处理
            String text = File.ReadAllText(GlobalVar.filePath, Encoding.Default);
            if (text.Length > 1000)
            {
                text = text.Substring(0, 1000);
            }
            return text;
        }

        // 获取当天日期,并进行格式化
        public static String getDate()
        {
            GlobalVar.date = DateTime.Now.ToString("yyyy-MM-dd");
            GlobalVar.date = GlobalVar.date.Replace("-", "/");
            GlobalVar.date = GlobalVar.date + "/" + ((Convert.ToInt32(DateTime.Now.DayOfWeek)).ToString());
            return GlobalVar.date;
        }
        
        private static String moveLine(ref String text)
        {
            String topLine = "";
            while (topLine.Length <= 2) // 字符串过短,有误.可能是空行,故去除.
            {
                topLine = text.Substring(0, text.IndexOf("\n"));
                text = text.Substring(topLine.Length + 1, text.Length - topLine.Length - 1);
            }
            return topLine;
        }

        private static String copyLine(String text)
        {
            String topLine = "";
            while (topLine.Length <= 2)
            {
                topLine = text.Substring(0, text.IndexOf("\n"));
            }
            return topLine;
        }
        
        // 获取当天日报内容/获取前一次日报内容.
        public static bool readDailyReport()
        {
            GlobalVar.allText = getFileText();
            String text = GlobalVar.allText.Replace("\r\n", "\n");
            text = text.Replace("\r", "\n");
            String topLine = "";
            String topBlock = "";   // 每天为一块

            // 取出最上面一天的日报
            topLine = copyLine(text);
            
            // 判断最上面一天日报是否为今天日报
            if (topLine.IndexOf(GlobalVar.date) != -1)
            {
                topLine = moveLine(ref text);
                topBlock = text.Substring(0, text.IndexOf("\n\n"));
                text = text.Substring(topBlock.Length + 2, text.Length - topBlock.Length - 2);

                int i = 0;
                while (topBlock.Length > 0)
                {
                    topLine = moveLine(ref topBlock);
                    topLine = topLine.Substring(2, topLine.Length - 2);
                    GlobalVar.yesterdayDailyReport[i] = topLine;
                    i++;
                }
            }

            // 找到昨天日报
            String yesterdayDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            yesterdayDate = yesterdayDate.Replace("-","/");
            yesterdayDate = yesterdayDate + "/" + ((Convert.ToInt32(DateTime.Now.AddDays(-1).DayOfWeek)).ToString());
            if (topLine.IndexOf(yesterdayDate) != -1)
            {
                // 获取昨天日报
                topLine = moveLine(ref text);
                topBlock = text.Substring(0, text.IndexOf("\n\n") + 1);
                text = text.Substring(topBlock.Length + 1, text.Length - topBlock.Length - 1);

                int i = 0;
                while (topBlock.Length > 0)
                {
                    topLine = moveLine(ref topBlock);
                    topLine = topLine.Substring(2, topLine.Length - 2);
                    GlobalVar.yesterdayDailyReport[i] = topLine;
                    i++;
                }
            }
            return true;
        }

        // 生成今天日报
        public static bool generateDailyReport(String []texts) 
        {
            int i = 0;
            while(i < 5)
            {
                GlobalVar.todayDailyReport[i] = texts[i];
                i++;
            }
            return true;
        }

        // 将日报内容写入文件中.
        public static bool writeDailyReportToFile()
        {
            // 在文件末尾写了
            String writeStr = "";
            writeStr += GlobalVar.date;
            writeStr += "\r\n";
            int i = 0;
            while (GlobalVar.todayDailyReport[i].Length > 0)
            {
                writeStr += (i + 1).ToString() + "." + GlobalVar.todayDailyReport[i];
                writeStr += "\r\n";
                i++;
            }
            writeStr += "\r\n";

            GlobalVar.allText = writeStr + GlobalVar.allText;

            FileStream fs = File.OpenWrite(GlobalVar.filePath);
            StreamWriter sw = new StreamWriter(fs, GlobalVar.fileEncoding);
            sw.Write(GlobalVar.allText);
            sw.Flush();
            sw.Close();
            return true;

        }

        public static bool Initialize()
        {
            GlobalVar.filePath = "C:\\Users\\liuwei\\Documents\\MyDocuments\\DailyReport\\刘巍的日报.txt";
            GlobalVar.fileEncoding = Encoding.Default;

            if (!Transcation.setFilePath(GlobalVar.filePath))
            {
                return false;
            }
            Transcation.getFileText();
            Transcation.getDate();
            Transcation.readDailyReport();
            return true;
        }
    }
}
