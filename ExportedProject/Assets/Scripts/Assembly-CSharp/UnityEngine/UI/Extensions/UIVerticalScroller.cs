using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("Layout/Extensions/Vertical Scroller")]
	public class UIVerticalScroller : MonoBehaviour
	{
		[Tooltip("Scrollable area (content of desired ScrollRect)")]
		public RectTransform _scrollingPanel;

		[Tooltip("Elements to populate inside the scroller")]
		public GameObject[] _arrayOfElements;

		[Tooltip("Center display area (position of zoomed content)")]
		public RectTransform _center;

		[Tooltip("Select the item to be in center on start. (optional)")]
		public int StartingIndex;

		[Tooltip("Button to go to the next page. (optional)")]
		public GameObject ScrollUpButton;

		[Tooltip("Button to go to the previous page. (optional)")]
		public GameObject ScrollDownButton;

		[Tooltip("Event fired when a specific item is clicked, exposes index number of item. (optional)")]
		public UnityEvent<int> ButtonClicked;

		private float[] distReposition;

		private float[] distance;

		private int minElementsNum;

		private int elementLength;

		private float deltaY;

		private string result;

		public UIVerticalScroller()
		{
		}

		public UIVerticalScroller(RectTransform scrollingPanel, GameObject[] arrayOfElements, RectTransform center)
		{
		}

		public void Awake()
		{
		}

		public void Start()
		{
		}

		private void AddListener(GameObject button, int index)
		{
		}

		private void DoSomething(int index)
		{
		}

		public void Update()
		{
		}

		private void ScrollingElements(float position)
		{
		}

		public string GetResults()
		{
			return null;
		}

		public void SnapToElement(int element)
		{
		}

		public void ScrollUp()
		{
		}

		public void ScrollDown()
		{
		}
	}
}
