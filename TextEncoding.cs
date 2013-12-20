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
			this.Add(new TextEncoding { encodeName = "ANSI", encoding = Encoding.Default });
			this.Add(new TextEncoding { encodeName = "UTF8", encoding = Encoding.UTF8 });
			this.Add(new TextEncoding { encodeName = "Unicode/UCS2LE", encoding = Encoding.Unicode });
		}
	}
}
