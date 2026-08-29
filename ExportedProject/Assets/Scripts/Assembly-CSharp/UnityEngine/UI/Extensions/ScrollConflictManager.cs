using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	[AddComponentMenu("UI/Extensions/Scrollrect Conflict Manager")]
	[RequireComponent(typeof(ScrollRect))]
	public class ScrollConflictManager : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler
	{
		public ScrollRect ParentScrollRect;

		private ScrollRect _myScrollRect;

		private bool scrollOther;

		private bool scrollOtherHorizontally;

		private void Awake()
		{
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
		}

		public void OnEndDrag(PointerEventData eventData)
		{
		}

		public void OnDrag(PointerEventData eventData)
		{
		}
	}
}
