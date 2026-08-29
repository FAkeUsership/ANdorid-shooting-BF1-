using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	public class MB3_TextureCombinerNonTextureProperties
	{
		private MB2_LogLevel LOG_LEVEL;

		private bool _considerNonTextureProperties;

		private TextureBlender resultMaterialTextureBlender;

		private TextureBlender[] textureBlenders;

		public TextureBlender GetTextureBlender()
		{
			return null;
		}

		public MB3_TextureCombinerNonTextureProperties(MB2_LogLevel ll, bool considerNonTextureProps)
		{
		}

		private static bool InterfaceFilter(Type typeObj, object criteriaObj)
		{
			return false;
		}

		internal void FindBestTextureBlender(Material resultMaterial)
		{
		}

		internal void LoadTextureBlenders()
		{
		}

		internal Color GetColorIfNoTexture(Material m, ShaderTextureProperty shaderPropertyName)
		{
			return default;
		}

		internal bool NonTexturePropertiesAreEqual(Material a, Material b)
		{
			return false;
		}

		internal Texture2D TintTextureWithTextureCombiner(Texture2D t, MB_TexSet sourceMaterial, ShaderTextureProperty shaderPropertyName)
		{
			return null;
		}

		internal TextureBlender FindMatchingTextureBlender(string shaderName)
		{
			return null;
		}

		internal void AdjustNonTextureProperties(Material mat, List<ShaderTextureProperty> texPropertyNames, List<MB_TexSet> distinctMaterialTextures, bool considerTintColor, MB2_EditorMethodsInterface editorMethods)
		{
		}

		internal static Color GetColorIfNoTexture(ShaderTextureProperty texProperty)
		{
			return default;
		}
	}
}
