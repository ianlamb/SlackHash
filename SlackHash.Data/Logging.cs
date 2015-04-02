using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlackHash.Data
{
	public class Logging
	{
		static string lastLog;

		static Logging()
		{
			lastLog = "";
		}

		public static void Info(string message = "", object data = null)
		{
			var sb = new StringBuilder(message);
			var log = sb.Append(data.ToString()).ToString();
			if (log != lastLog)
			{
				//Console.Write(Environment.NewLine);
				//Console.Write(log);
			}
			else
			{
				//Console.Write(".");
			}
			lastLog = log;
		}
	}
}
