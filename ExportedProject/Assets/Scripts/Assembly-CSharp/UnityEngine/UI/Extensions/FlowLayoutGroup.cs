using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	[AddComponentMenu("Layout/Extensions/Flow Layout Group")]
	public class FlowLayoutGroup : LayoutGroup
	{
		public float SpacingX;

		public float SpacingY;

		public bool ExpandHorizontalSpacing;

		public bool ChildForceExpandWidth;

		public bool ChildForceExpandHeight;

		private float _layoutHeight;

		private readonly IList<RectTransform> _rowList;

		protected bool IsCenterAlign => false;

		protected bool IsRightAlign => false;

		protected bool IsMiddleAlign => false;

		protected bool IsLowerAlign => false;

		public override void CalculateLayoutInputHorizontal()
		{
		}

		public override void SetLayoutHorizontal()
		{
		}

		public override void SetLayoutVertical()
		{
		}

		public override void CalculateLayoutInputVertical()
		{
		}

		public float SetLayout(float width, int axis, bool layoutInput)
		{
			return 0f;
		}

		private float CalculateRowVerticalOffset(float groupHeight, float yOffset, float currentRowHeight)
		{
			return 0f;
		}

		protected void LayoutRow(IList<RectTransform> contents, float rowWidth, float rowHeight, float maxWidth, float xOffset, float yOffset, int axis)
		{
		}

		public float GetGreatestMinimumChildWidth()
		{
			return 0f;
		}
	}
}
