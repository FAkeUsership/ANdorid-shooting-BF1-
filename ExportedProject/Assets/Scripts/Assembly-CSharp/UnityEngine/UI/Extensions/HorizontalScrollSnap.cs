using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("Layout/Extensions/Horizontal Scroll Snap")]
	public class HorizontalScrollSnap : ScrollSnapBase, IEndDragHandler, IEventSystemHandler
	{
		private void Start()
		{
		}

		private void Update()
		{
		}

		private bool IsRectMovingSlowerThanThreshold(float startingSpeed)
		{
			return false;
		}

		private void DistributePages()
		{
		}

		public void AddChild(GameObject GO)
		{
		}

		public void AddChild(GameObject GO, bool WorldPositionStays)
		{
		}

		public void RemoveChild(int index, out GameObject ChildRemoved)
		{
			ChildRemoved = null;
		}

		public void RemoveChild(int index, bool WorldPositionStays, out GameObject ChildRemoved)
		{
			ChildRemoved = null;
		}

		public void RemoveAllChildren(out GameObject[] ChildrenRemoved)
		{
			ChildrenRemoved = null;
		}

		public void RemoveAllChildren(bool WorldPositionStays, out GameObject[] ChildrenRemoved)
		{
			ChildrenRemoved = null;
		}

		private void SetScrollContainerPosition()
		{
		}

		public void UpdateLayout()
		{
		}

		private void OnRectTransformDimensionsChange()
		{
		}

		private void OnEnable()
		{
		}

		public void OnEndDrag(PointerEventData eventData)
		{
		}
	}
}
