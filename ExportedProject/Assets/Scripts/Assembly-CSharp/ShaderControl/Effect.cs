using UnityEngine;

namespace ShaderControl
{
	[ExecuteInEditMode]
	public class Effect : MonoBehaviour
	{
		private Material mat;

		private string[] keywords;

		private void Start()
		{
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
