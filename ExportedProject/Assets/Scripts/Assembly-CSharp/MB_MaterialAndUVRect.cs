using System;
using UnityEngine;

[Serializable]
public class MB_MaterialAndUVRect
{
	public Material material;

	public Rect atlasRect;

	public string srcObjName;

	public Rect samplingRectMatAndUVTiling;

	public Rect sourceMaterialTiling;

	public Rect samplingEncapsulatinRect;

	public MB_MaterialAndUVRect(Material m, Rect destRect, Rect samplingRectMatAndUVTiling, Rect sourceMaterialTiling, Rect samplingEncapsulatinRect, string objName)
	{
	}

	public override int GetHashCode()
	{
		return 0;
	}

	public override bool Equals(object obj)
	{
		return false;
	}
}
