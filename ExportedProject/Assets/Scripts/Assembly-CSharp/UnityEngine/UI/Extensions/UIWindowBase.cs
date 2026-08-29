using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	[AddComponentMenu("UI/Extensions/UI Window Base")]
	[RequireComponent(typeof(RectTransform))]
	public class UIWindowBase : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
	{
		private RectTransform m_transform;

		private bool _isDragging;

		public static bool ResetCoords;

		private Vector3 m_originalCoods;

		private Canvas m_canvas;

		private RectTransform m_canvasRectTransform;

		public int KeepWindowInCanvas;

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void OnDrag(PointerEventData eventData)
		{
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
		}

		public void OnEndDrag(PointerEventData eventData)
		{
		}

		private void resetCoordinatePosition()
		{
		}

		private Vector3 ScreenToCanvas(Vector3 screenPosition)
		{
			return default;
		}
	}
}
