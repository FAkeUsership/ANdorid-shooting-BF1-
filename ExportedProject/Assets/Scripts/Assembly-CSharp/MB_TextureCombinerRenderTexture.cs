using System.Collections.Generic;
using DigitalOpus.MB.Core;
using UnityEngine;

public class MB_TextureCombinerRenderTexture
{
	public MB2_LogLevel LOG_LEVEL;

	private Material mat;

	private RenderTexture _destinationTexture;

	private Camera myCamera;

	private int _padding;

	private bool _isNormalMap;

	private bool _fixOutOfBoundsUVs;

	private bool _doRenderAtlas;

	private Rect[] rs;

	private List<MB_TexSet> textureSets;

	private int indexOfTexSetToRender;

	private ShaderTextureProperty _texPropertyName;

	private TextureBlender _resultMaterialTextureBlender;

	private Texture2D targTex;

	private MB3_TextureCombiner combiner;

	public Texture2D DoRenderAtlas(GameObject gameObject, int width, int height, int padding, Rect[] rss, List<MB_TexSet> textureSetss, int indexOfTexSetToRenders, ShaderTextureProperty texPropertyname, TextureBlender resultMaterialTextureBlender, bool isNormalMap, bool fixOutOfBoundsUVs, bool considerNonTextureProperties, MB3_TextureCombiner texCombiner, MB2_LogLevel LOG_LEV)
	{
		return null;
	}

	public void OnRenderObject()
	{
	}

	private Color32 ConvertNormalFormatFromUnity_ToStandard(Color32 c)
	{
		return default;
	}

	private bool IsOpenGL()
	{
		return false;
	}

	private void CopyScaledAndTiledToAtlas(MB_TexSet texSet, MeshBakerMaterialTexture source, Vector2 obUVoffset, Vector2 obUVscale, Rect rec, ShaderTextureProperty texturePropertyName, TextureBlender resultMatTexBlender)
	{
	}

	private void _printTexture(Texture2D t)
	{
	}
}
