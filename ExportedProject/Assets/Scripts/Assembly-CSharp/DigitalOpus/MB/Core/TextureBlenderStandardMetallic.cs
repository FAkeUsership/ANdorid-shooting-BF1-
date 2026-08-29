using UnityEngine;

namespace DigitalOpus.MB.Core
{
	public class TextureBlenderStandardMetallic : TextureBlender
	{
		private enum Prop
		{
			doColor = 0,
			doMetallic = 1,
			doEmission = 2,
			doBump = 3,
			doNone = 4
		}

		private Color m_tintColor;

		private float m_smoothness;

		private float m_metallic;

		private float m_bumpScale;

		private Color m_emissionColor;

		private float m_emissionSlider;

		private Prop propertyToDo;

		private Color m_defaultColor;

		private float m_defaultMetallic;

		private float m_defaultGlossiness;

		private float m_defaultBumpScale;

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
