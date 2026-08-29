using UnityEngine;

namespace DigitalOpus.MB.Core
{
	public class MB_Utility
	{
		public struct MeshAnalysisResult
		{
			public Rect uvRect;

			public bool hasOutOfBoundsUVs;

			public bool hasOverlappingSubmeshVerts;

			public bool hasOverlappingSubmeshTris;

			public bool hasUVs;

			public float submeshArea;
		}

		private class MB_Triangle
		{
			private int submeshIdx;

			private int[] vs;

			public bool isSame(object obj)
			{
				return false;
			}

			public bool sharesVerts(MB_Triangle obj)
			{
				return false;
			}

			public void Initialize(int[] ts, int idx, int sIdx)
			{
			}
		}

		public static Texture2D createTextureCopy(Texture2D source)
		{
			return null;
		}

		public static bool ArrayBIsSubsetOfA(object[] a, object[] b)
		{
			return false;
		}

		public static Material[] GetGOMaterials(GameObject go)
		{
			return null;
		}

		public static Mesh GetMesh(GameObject go)
		{
			return null;
		}

		public static void SetMesh(GameObject go, Mesh m)
		{
		}

		public static Renderer GetRenderer(GameObject go)
		{
			return null;
		}

		public static void DisableRendererInSource(GameObject go)
		{
		}

		public static bool hasOutOfBoundsUVs(Mesh m, ref Rect uvBounds)
		{
			return false;
		}

		public static bool hasOutOfBoundsUVs(Mesh m, ref MeshAnalysisResult putResultHere, int submeshIndex = -1, int uvChannel = 0)
		{
			return false;
		}

		public static bool hasOutOfBoundsUVs(Vector2[] uvs, Mesh m, ref MeshAnalysisResult putResultHere, int submeshIndex = -1)
		{
			return false;
		}

		public static void setSolidColor(Texture2D t, Color c)
		{
		}

		public static Texture2D resampleTexture(Texture2D source, int newWidth, int newHeight)
		{
			return null;
		}

		public static bool AreAllSharedMaterialsDistinct(Material[] sharedMaterials)
		{
			return false;
		}

		public static int doSubmeshesShareVertsOrTris(Mesh m, ref MeshAnalysisResult mar)
		{
			return 0;
		}

		public static bool GetBounds(GameObject go, out Bounds b)
		{
			b = default;
			return false;
		}

		public static void Destroy(Object o)
		{
		}
	}
}
