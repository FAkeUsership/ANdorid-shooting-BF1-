using System;

namespace DigitalOpus.MB.Core
{
	[Serializable]
	public struct AtlasPadding
	{
		public int topBottom;

		public int leftRight;

		public AtlasPadding(int p)
		{
			topBottom = 0;
			leftRight = 0;
		}

		public AtlasPadding(int px, int py)
		{
			topBottom = 0;
			leftRight = 0;
		}
	}
}
