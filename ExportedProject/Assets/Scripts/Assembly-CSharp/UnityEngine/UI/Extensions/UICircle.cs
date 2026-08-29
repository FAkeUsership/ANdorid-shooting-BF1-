namespace UnityEngine.UI.Extensions
{
	[AddComponentMenu("UI/Extensions/Primitives/UI Circle")]
	public class UICircle : UIPrimitiveBase
	{
		[Tooltip("The circular fill percentage of the primitive, affected by FixedToSegments")]
		[Range(0f, 100f)]
		[SerializeField]
		private int m_fillPercent;

		[Tooltip("Should the primitive fill draw by segments or absolute percentage")]
		public bool FixedToSegments;

		[SerializeField]
		[Tooltip("Draw the primitive filled or as a line")]
		private bool m_fill;

		[SerializeField]
		[Tooltip("If not filled, the thickness of the primitive line")]
		private float m_thickness;

		[Range(0f, 360f)]
		[Tooltip("The number of segments to draw the primitive, more segments = smoother primitive")]
		[SerializeField]
		private int m_segments;

		public int FillPercent
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public bool Fill
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public float Thickness
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public int Segments
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		private void Update()
		{
		}

		protected override void OnPopulateMesh(VertexHelper vh)
		{
		}

		private void StepThroughPointsAndFill(float outer, float inner, ref Vector2 prevX, ref Vector2 prevY, out Vector2 pos0, out Vector2 pos1, out Vector2 pos2, out Vector2 pos3, float c, float s)
		{
			pos0 = default;
			pos1 = default;
			pos2 = default;
			pos3 = default;
		}
	}
}
