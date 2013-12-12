using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DailyReportAssistant
{
	class ERR
	{
		// success
		public const uint SUCCESS = 0x00;

		// Transcation.AppInitialize
		public const uint AI_NO_FILE = 0x01;

		// Transcation.getFileText
		public const uint GFT_NO_FILE = 0x11;
		public const uint GFT_CANNOT_READ_FILE = 0x12;

		// Transcation.setFilePath
		public const uint SFP_NO_FILE = 0x21;


	}
}
