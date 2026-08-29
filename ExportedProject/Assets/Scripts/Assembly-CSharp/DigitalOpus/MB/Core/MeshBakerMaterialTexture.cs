using UnityEngine;

namespace DigitalOpus.MB.Core
{
	public class MeshBakerMaterialTexture
	{
		private Texture2D _t;

		public float texelDensity;

		internal static bool readyToBuildAtlases;

		public DRect encapsulatingSamplingRect;

		public DRect matTilingRect;

		public Texture2D t
		{
			set
			{
			}
		}

		public bool isNull => false;

		public int width => 0;

		public int height => 0;

		public MeshBakerMaterialTexture()
		{
		}

		public MeshBakerMaterialTexture(Texture tx)
		{
		}

		public MeshBakerMaterialTexture(Texture tx, Vector2 o, Vector2 s, float texelDens)
		{
		}

		public Texture2D GetTexture2D()
		{
			return null;
		}

		public string GetTexName()
		{
			return null;
		}

		public bool AreTexturesEqual(MeshBakerMaterialTexture b)
		{
			return false;
		}
	}
}
