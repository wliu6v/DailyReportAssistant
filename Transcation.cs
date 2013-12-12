using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using System.Collections.ObjectModel;

namespace DailyReportAssistant
{
    public class GlobalVar
    {
		private static String _filePath = "";
		private static Encoding _fileEncoding = Encoding.Default;
		private static bool _shouldSvnCommit = false;
		private static String _svnUsername = "";
		private static String _svnPassword = "";

        private static String _date = "";
        private static String _allText = "";
        private static String[] _dailyReport = new String[5];

		public static String filePath
		{
			get { return _filePath; }
			set { _filePath = value; }
		}

		public static Encoding fileEncoding
		{
			get { return _fileEncoding; }
			set	{ _fileEncoding = value; }
		}

		public static bool shouldSvnCommit
		{
			get { return _shouldSvnCommit; }
			set { _shouldSvnCommit = value; }
		}

		public static String svnUsername
		{
			get { return _svnUsername; }
			set { _svnUsername = value; }
		}

		public static String svnPassword
		{
			get { return _svnPassword; }
			set { _svnPassword = value; }
		}

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

    }

    public class Transcation
    {
		private static Properties.Settings appSetting;

		private const uint EncodingANSI = 0x01;
		private const uint EncodingUTF8 = 0x02;
		private const uint EncodingUCS2LE = 0x04;

		/***********************************************************
		 * AppInitialize
		 * 应用程序初始化
		 * 当应用启动的时候执行一遍。
		 ***********************************************************/
		public static uint AppInitialize()
		{
			uint errorCode = ERR.SUCCESS;
			appSetting = Properties.Settings.Default;

			// 程序运行计数器
			if (appSetting.UseCount == 0) // 首次运行
			{
				appSetting.FirstUseTime = DateTime.Now;
			}
			appSetting.UseCount++;

			// 检查日报文件是否存在
			GlobalVar.filePath = appSetting.FilePath;
			if (appSetting.FilePath.Length == 0
				|| appSetting.FilePath == null)
			{
				GlobalVar.filePath = "";
				errorCode = ERR.AI_NO_FILE;
			}

			// 确定日报文件编码
			switch (appSetting.FileEncoding)
			{
				case EncodingANSI:
					GlobalVar.fileEncoding = Encoding.Default;
					break;
				case EncodingUTF8:
					GlobalVar.fileEncoding = Encoding.UTF8;
					break;
				case EncodingUCS2LE:
					GlobalVar.fileEncoding = Encoding.Unicode;
					break;
				default:
					GlobalVar.fileEncoding = Encoding.Default;
					break;
			}
			
			// 读取 SVN 相关项
			GlobalVar.shouldSvnCommit = appSetting.ShouldSvnCommit;
			GlobalVar.svnUsername = appSetting.SvnUsername;
			GlobalVar.svnPassword = appSetting.SvnPassword;

			errorCode = Transcation.getFileText();
			if (errorCode != ERR.SUCCESS)
			{
				return errorCode;
			}

			Transcation.getDate();

			errorCode = ERR.SUCCESS;
			return errorCode;
		}

		/***********************************************************
		 * SaveConfig
		 * 保存配置
		 * 当应用退出时与每次保存设置时执行一遍。
		 ***********************************************************/
		public static uint SaveConfig()
		{
			appSetting.FilePath = GlobalVar.filePath;
			appSetting.ShouldSvnCommit = GlobalVar.shouldSvnCommit;
			appSetting.SvnUsername = GlobalVar.svnUsername;
			appSetting.SvnPassword = GlobalVar.svnPassword;

			if (GlobalVar.fileEncoding == Encoding.Default) { appSetting.FileEncoding = EncodingANSI; }
			else if (GlobalVar.fileEncoding == Encoding.UTF8) { appSetting.FileEncoding = EncodingUTF8; }
			else if (GlobalVar.fileEncoding == Encoding.Unicode) { appSetting.FileEncoding = EncodingUCS2LE; }
			else { appSetting.FileEncoding = EncodingANSI; }

			return ERR.SUCCESS;
		}

		/***********************************************************
		 * setFilePath
		 * 设置日报路径
		 * 
		 ***********************************************************/
        private static uint setFilePath(String filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File Path Error. Cannot Find .txt File.");
                Console.WriteLine("Error encountered during Transtation.setFilePath().");
                return ERR.SFP_NO_FILE;
            }

            GlobalVar.filePath = filePath;
            return ERR.SUCCESS;
        }

        // 根据日报路径获取日报内容
        private static uint getFileText()
        {
            if (GlobalVar.filePath == null || GlobalVar.filePath.Length == 0)
            {
                Console.WriteLine("未找到日报文件");
                return ERR.GFT_NO_FILE;
            }

            // 要增加对文件编码的处理
            String text = File.ReadAllText(GlobalVar.filePath, GlobalVar.fileEncoding);
			if (text == null || text.Length == 0)
			{
				Console.WriteLine("未能成功读取日报文件，请检查文件编码是否正确");
				return ERR.GFT_CANNOT_READ_FILE;
			}

			GlobalVar.allText = text;
            return ERR.SUCCESS;
        }

        // 获取当天日期,并进行格式化
        private static String getDate()
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
        
        // 获取最上面一次的日报内容.
        private static bool readDailyReport()
        {
			String tmpStr = GlobalVar.allText.Substring(0, 1000);
			tmpStr = tmpStr.Replace("\r\n", "\n");
			tmpStr = tmpStr.Replace("\r", "\n");
			String topBlock = tmpStr.Substring(0, tmpStr.IndexOf("\n\n") + 1);
			String topLine = moveLine(ref topBlock);

			//----TODO: 应该判断这里是否为今天日报

            int i = 0;
            while (topBlock.Length > 0)
            {
                topLine = moveLine(ref topBlock);
				String number = (i + 1).ToString() + ".";
				if (topLine.IndexOf(number) == 0)
					topLine = topLine.Substring(number.Length, topLine.Length - number.Length);
                GlobalVar.dailyReport[i] = topLine;
                i++;
            }
            
            return true;
        }

        // 生成今天日报
        private static bool generateDailyReport(String[] texts) 
        {
			for (int i = 0; i < 5; i++)
			{
                GlobalVar.dailyReport[i] = texts[i];
				if (GlobalVar.dailyReport[i].LastIndexOf('。') != GlobalVar.dailyReport[i].Length - 1)
					GlobalVar.dailyReport[i] += "。";
				GlobalVar.dailyReport[i] = GlobalVar.dailyReport[i].Replace("。。","。");
				GlobalVar.dailyReport[i] = GlobalVar.dailyReport[i].Replace("，。", "。");
				GlobalVar.dailyReport[i] = GlobalVar.dailyReport[i].Replace("；。", "。");
				GlobalVar.dailyReport[i] = GlobalVar.dailyReport[i].Replace(".。", "。");
				GlobalVar.dailyReport[i] = GlobalVar.dailyReport[i].Replace(",。", "。");
            }
            return true;
        }

        // 将日报内容写入文件中.
        private static bool writeDailyReportToFile()
        {
            String writeStr = "";
            writeStr += GlobalVar.date;
            writeStr += Environment.NewLine;
            int i = 0;
            while (GlobalVar.dailyReport[i].Length > 0)
            {
				writeStr += (i + 1).ToString() + "." + GlobalVar.dailyReport[i] + Environment.NewLine;
                i++;
            }
            writeStr += Environment.NewLine;

            // 判断是否存在今天的日报,防止多次提交
            String allText = GlobalVar.allText;
			String topDate = copyLine(allText);
            if (topDate.IndexOf(GlobalVar.date) != -1)
            {
                // 这里的处理其实是存在问题的。如果今天的日报不是用此程序生成的话可能会有bug
				allText = allText.Substring(allText.IndexOf(Environment.NewLine + Environment.NewLine) + (Environment.NewLine + Environment.NewLine).Length);

				// 以下三行代码可能有效但稍微麻烦了一点，需要再斟酌。
				//String tmpStr = GlobalVar.allText.Substring(0, 1000); // 仅判断最上面一次日报，1000字节足够
				//int firstRNRN = tmpStr.IndexOf("\r\n\r\n");
				//int firstNN = tmpStr.IndexOf("\n\n");
            }

			allText = writeStr + allText;
			GlobalVar.allText = allText;
            FileStream fs = File.OpenWrite(GlobalVar.filePath);
            StreamWriter sw = new StreamWriter(fs, GlobalVar.fileEncoding);
            sw.Write(GlobalVar.allText);
            sw.Flush();
            sw.Close();
            return true;

        }

        public static String[] Read()
        {
            Transcation.readDailyReport();
            return GlobalVar.dailyReport;
        }

        public static bool Write(String[] texts)
        {
            generateDailyReport(texts);
            return writeDailyReportToFile();
        }

        public static String SVNCommit()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd";
            process.StartInfo.Arguments = " /c 日报提交.bat";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit(5000);
            process.Close();
            process = null;
            return "";
        }
    }




}

