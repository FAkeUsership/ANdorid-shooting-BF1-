using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Image))]
	[AddComponentMenu("UI/Effects/Extensions/Curly UI Image")]
	public class CUIImage : CUIGraphic
	{
		public static int SlicedImageCornerRefVertexIdx;

		public static int FilledImageCornerRefVertexIdx;

		[SerializeField]
		[HideInInspector]
		[Tooltip("For changing the size of the corner for tiled or sliced Image")]
		public Vector2 cornerPosRatio;

		[HideInInspector]
		[SerializeField]
		protected Vector2 oriCornerPosRatio;

		public Vector2 OriCornerPosRatio => default;

		public Image UIImage => null;

		public static int ImageTypeCornerRefVertexIdx(Image.Type _type)
		{
			return 0;
		}

		public override void ReportSet()
		{
		}

		protected override void modifyVertices(List<UIVertex> _verts)
		{
		}
	}
}
