using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	public class MB2_TexturePackerRegular : MB2_TexturePacker
	{
		private class ProbeResult
		{
			public int w;

			public int h;

			public int outW;

			public int outH;

			public Node root;

			public bool largerOrEqualToMaxDim;

			public float efficiency;

			public float squareness;

			public float totalAtlasArea;

			public int numAtlases;

			public void Set(int ww, int hh, int outw, int outh, Node r, bool fits, float e, float sq)
			{
			}

			public float GetScore(bool doPowerOfTwoScore)
			{
				return 0f;
			}

			public void PrintTree()
			{
			}
		}

		internal class Node
		{
			internal NodeType isFullAtlas;

			internal Node[] child;

			internal PixRect r;

			internal Image img;

			private ProbeResult bestRoot;

			internal Node(NodeType rootType)
			{
			}

			private bool isLeaf()
			{
				return false;
			}

			internal Node Insert(Image im, bool handed)
			{
				return null;
			}
		}

		private ProbeResult bestRoot;

		public int atlasY;

		private static void printTree(Node r, string spc)
		{
		}

		private static void flattenTree(Node r, List<Image> putHere)
		{
		}

		private static void drawGizmosNode(Node r)
		{
		}

		private static Texture2D createFilledTex(Color c, int w, int h)
		{
			return null;
		}

		public void DrawGizmos()
		{
		}

		private bool ProbeSingleAtlas(Image[] imgsToAdd, int idealAtlasW, int idealAtlasH, float imgArea, int maxAtlasDimX, int maxAtlasDimY, ProbeResult pr)
		{
			return false;
		}

		private bool ProbeMultiAtlas(Image[] imgsToAdd, int idealAtlasW, int idealAtlasH, float imgArea, int maxAtlasDimX, int maxAtlasDimY, ProbeResult pr)
		{
			return false;
		}

		internal void GetExtent(Node r, ref int x, ref int y)
		{
		}

		private int StepWidthHeight(int oldVal, int step, int maxDim)
		{
			return 0;
		}

		public override AtlasPackingResult[] GetRects(List<Vector2> imgWidthHeights, int maxDimensionX, int maxDimensionY, int atPadding)
		{
			return null;
		}

		public override AtlasPackingResult[] GetRects(List<Vector2> imgWidthHeights, List<AtlasPadding> paddings, int maxDimensionX, int maxDimensionY, bool doMultiAtlas)
		{
			return null;
		}

		private AtlasPackingResult _GetRectsSingleAtlas(List<Vector2> imgWidthHeights, List<AtlasPadding> paddings, int maxDimensionX, int maxDimensionY, int minImageSizeX, int minImageSizeY, int masterImageSizeX, int masterImageSizeY, int recursionDepth)
		{
			return null;
		}

		private AtlasPackingResult[] _GetRectsMultiAtlas(List<Vector2> imgWidthHeights, List<AtlasPadding> paddings, int maxDimensionPassedX, int maxDimensionPassedY, int minImageSizeX, int minImageSizeY, int masterImageSizeX, int masterImageSizeY)
		{
			return null;
		}
	}
}
