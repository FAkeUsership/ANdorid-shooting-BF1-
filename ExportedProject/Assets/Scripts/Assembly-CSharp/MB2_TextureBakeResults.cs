using System.Collections.Generic;
using DigitalOpus.MB.Core;
using UnityEngine;

public class MB2_TextureBakeResults : ScriptableObject
{
	public class Material2AtlasRectangleMapper
	{
		private MB2_TextureBakeResults tbr;

		private int[] numTimesMatAppearsInAtlas;

		private MB_MaterialAndUVRect[] matsAndSrcUVRect;

		public Material2AtlasRectangleMapper(MB2_TextureBakeResults res)
		{
		}

		public bool TryMapMaterialToUVRect(Material mat, Mesh m, int submeshIdx, int idxInResultMats, MB3_MeshCombinerSingle.MeshChannelsCache meshChannelCache, Dictionary<int, MB_Utility.MeshAnalysisResult[]> meshAnalysisCache, out Rect rectInAtlas, out Rect encapsulatingRect, out Rect sourceMaterialTilingOut, ref string errorMsg, MB2_LogLevel logLevel)
		{
			rectInAtlas = default;
			encapsulatingRect = default;
			sourceMaterialTilingOut = default;
			return false;
		}
	}

	private const int VERSION = 3230;

	public int version;

	public MB_MaterialAndUVRect[] materialsAndUVRects;

	public MB_MultiMaterial[] resultMaterials;

	public bool doMultiMaterial;

	public Material[] materials;

	public bool fixOutOfBoundsUVs;

	public Material resultMaterial;

	private void OnEnable()
	{
	}

	public static MB2_TextureBakeResults CreateForMaterialsOnRenderer(GameObject[] gos, List<Material> matsOnTargetRenderer)
	{
		return null;
	}

	public bool DoAnyResultMatsUseConsiderMeshUVs()
	{
		return false;
	}

	public bool ContainsMaterial(Material m)
	{
		return false;
	}

	public string GetDescription()
	{
		return null;
	}

	public static bool IsMeshAndMaterialRectEnclosedByAtlasRect(Rect uvR, Rect sourceMaterialTiling, Rect samplingEncapsulatinRect, MB2_LogLevel logLevel)
	{
		return false;
	}
}
