using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	[AddComponentMenu("UI/Extensions/UI Infinite Scroll")]
	public class UI_InfiniteScroll : MonoBehaviour
	{
		[Tooltip("If false, will Init automatically, otherwise you need to call Init() method")]
		public bool InitByUser;

		private ScrollRect _scrollRect;

		private ContentSizeFitter _contentSizeFitter;

		private VerticalLayoutGroup _verticalLayoutGroup;

		private HorizontalLayoutGroup _horizontalLayoutGroup;

		private GridLayoutGroup _gridLayoutGroup;

		private bool _isVertical;

		private bool _isHorizontal;

		private float _disableMarginX;

		private float _disableMarginY;

		private bool _hasDisabledGridComponents;

		private List<RectTransform> items;

		private Vector2 _newAnchoredPosition;

		private float _treshold;

		private int _itemCount;

		private float _recordOffsetX;

		private float _recordOffsetY;

		private void Awake()
		{
		}

		public void Init()
		{
		}

		private void DisableGridComponents()
		{
		}

		public void OnScroll(Vector2 pos)
		{
		}
	}
}
