using UnityEngine;

namespace DigitalOpus.MB.Core
{
	public class TextureBlenderStandardSpecular : TextureBlender
	{
		private enum Prop
		{
			doColor = 0,
			doSpecular = 1,
			doEmission = 2,
			doNone = 3
		}

		private Color m_tintColor;

		private Color m_emission;

		private Prop propertyToDo;

		private Color m_defaultColor;

		private Color m_defaultSpecular;

		private float m_defaultGlossiness;

		private Color m_defaultEmission;

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

		public Color GetColorIfNoTexture(Material mat, ShaderTextureProperty texPropertyName)
		{
			return default;
		}
	}
}
