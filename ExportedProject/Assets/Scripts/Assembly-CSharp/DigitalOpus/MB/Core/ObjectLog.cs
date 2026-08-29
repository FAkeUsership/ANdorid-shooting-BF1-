namespace DigitalOpus.MB.Core
{
	public class ObjectLog
	{
		private int pos;

		private string[] logMessages;

		private void _CacheLogMessage(string msg)
		{
		}

		public ObjectLog(short bufferSize)
		{
		}

		public void Log(MB2_LogLevel l, string msg, MB2_LogLevel currentThreshold)
		{
		}

		public void Error(string msg, params object[] args)
		{
		}

		public void Warn(string msg, params object[] args)
		{
		}

		public void Info(string msg, params object[] args)
		{
		}

		public void LogDebug(string msg, params object[] args)
		{
		}

		public void Trace(string msg, params object[] args)
		{
		}

		public string Dump()
		{
			return null;
		}
	}
}
