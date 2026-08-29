using UnityEngine;

namespace DigitalOpus.MB.Core
{
	public class TextureBlenderLegacyBumpDiffuse : TextureBlender
	{
		private bool doColor;

		private Color m_tintColor;

		private Color m_defaultTintColor;

		public bool DoesShaderNameMatch(string shaderName)
		{
			return false;
		}

		public void OnBeforeTintTexture(Material sourceMat, string shaderTexturePropertyName)
		{
		}

		public Color OnBlendTexturePixel(string propertyToDoshaderPropertyName, Color pixelColor)
		{
			return default;
		}

		public bool NonTexturePropertiesAreEqual(Material a, Material b)
		{
			return false;
		}

		public void SetNonTexturePropertyValuesOnResultMaterial(Material resultMaterial)
		{
		}

		public Color GetColorIfNoTexture(Material m, ShaderTextureProperty texPropertyName)
		{
			return default;
		}
	}
}
