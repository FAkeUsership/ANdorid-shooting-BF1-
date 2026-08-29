using UnityEngine;

namespace DigitalOpus.MB.Core
{
	public class MB_TexSet
	{
		public MeshBakerMaterialTexture[] ts;

		public MatsAndGOs matsAndGOs;

		public bool allTexturesUseSameMatTiling;

		public Vector2 obUVoffset;

		public Vector2 obUVscale;

		public int idealWidth;

		public int idealHeight;

		internal DRect obUVrect => default;

		public MB_TexSet(MeshBakerMaterialTexture[] tss, Vector2 uvOffset, Vector2 uvScale)
		{
		}

		internal bool IsEqual(object obj, bool fixOutOfBoundsUVs, bool considerNonTextureProperties, MB3_TextureCombinerNonTextureProperties resultMaterialTextureBlender)
		{
			return false;
		}

		public void CalcInitialFullSamplingRects(bool fixOutOfBoundsUVs)
		{
		}

		public void CalcMatAndUVSamplingRects()
		{
		}

		public bool AllTexturesAreSameForMerge(MB_TexSet other, bool considerNonTextureProperties, MB3_TextureCombinerNonTextureProperties resultMaterialTextureBlender)
		{
			return false;
		}

		internal void DrawRectsToMergeGizmos(Color encC, Color innerC)
		{
		}

		internal string GetDescription()
		{
			return null;
		}

		internal string GetMatSubrectDescriptions()
		{
			return null;
		}
	}
}
