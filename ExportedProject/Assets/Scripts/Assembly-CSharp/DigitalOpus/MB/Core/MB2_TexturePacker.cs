using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	public abstract class MB2_TexturePacker
	{
		internal enum NodeType
		{
			Container = 0,
			maxDim = 1,
			regular = 2
		}

		internal class PixRect
		{
			public int x;

			public int y;

			public int w;

			public int h;

			public PixRect()
			{
			}

			public PixRect(int xx, int yy, int ww, int hh)
			{
			}

			public override string ToString()
			{
				return null;
			}
		}

		internal class Image
		{
			public int imgId;

			public int w;

			public int h;

			public int x;

			public int y;

			public Image(int id, int tw, int th, AtlasPadding padding, int minImageSizeX, int minImageSizeY)
			{
			}

			public Image(Image im)
			{
			}
		}

		internal class ImgIDComparer : IComparer<Image>
		{
			public int Compare(Image x, Image y)
			{
				return 0;
			}
		}

		internal class ImageHeightComparer : IComparer<Image>
		{
			public int Compare(Image x, Image y)
			{
				return 0;
			}
		}

		internal class ImageWidthComparer : IComparer<Image>
		{
			public int Compare(Image x, Image y)
			{
				return 0;
			}
		}

		internal class ImageAreaComparer : IComparer<Image>
		{
			public int Compare(Image x, Image y)
			{
				return 0;
			}
		}

		public MB2_LogLevel LOG_LEVEL;

		public bool atlasMustBePowerOfTwo;

		public static int RoundToNearestPositivePowerOfTwo(int x)
		{
			return 0;
		}

		public static int CeilToNearestPowerOfTwo(int x)
		{
			return 0;
		}

		public abstract AtlasPackingResult[] GetRects(List<Vector2> imgWidthHeights, int maxDimensionX, int maxDimensionY, int padding);

		public abstract AtlasPackingResult[] GetRects(List<Vector2> imgWidthHeights, List<AtlasPadding> paddings, int maxDimensionX, int maxDimensionY, bool doMultiAtlas);

		internal bool ScaleAtlasToFitMaxDim(Vector2 rootWH, List<Image> images, int maxDimensionX, int maxDimensionY, AtlasPadding padding, int minImageSizeX, int minImageSizeY, int masterImageSizeX, int masterImageSizeY, ref int outW, ref int outH, out float padX, out float padY, out int newMinSizeX, out int newMinSizeY)
		{
			padX = default;
			padY = default;
			newMinSizeX = default;
			newMinSizeY = default;
			return false;
		}

		public void normalizeRects(AtlasPackingResult rr, AtlasPadding padding)
		{
		}
	}
}
