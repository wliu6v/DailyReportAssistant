using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace DailyReportAssistant
{
	public class TextEncoding
	{
		public TextEncoding() { }
		public String encodeName { set; get; }
		public Encoding encoding { set; get; }
	}

	public class TextEncodingList : ObservableCollection<TextEncoding>
	{
		public TextEncodingList()
		{
			this.Add(new TextEncoding { encodeName = "ANSI/GB2312", encoding = Encoding.Default });	// 936
			this.Add(new TextEncoding { encodeName = "ANSI/GB18030", encoding = Encoding.GetEncoding(54936) });	// 54936
			this.Add(new TextEncoding { encodeName = "UTF8", encoding = Encoding.UTF8 });	// 65001
			this.Add(new TextEncoding { encodeName = "Unicode/UCS2LE", encoding = Encoding.Unicode });	// 65005 ??
		}
	}
}
