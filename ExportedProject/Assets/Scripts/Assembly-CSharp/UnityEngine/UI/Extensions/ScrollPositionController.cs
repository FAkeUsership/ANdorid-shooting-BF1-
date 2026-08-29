using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	public class ScrollPositionController : UIBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler
	{
		[Serializable]
		public class UpdatePositionEvent : UnityEvent<float>
		{
		}

		[Serializable]
		public class ItemSelectedEvent : UnityEvent<int>
		{
		}

		[Serializable]
		private struct Snap
		{
			public bool Enable;

			public float VelocityThreshold;

			public float Duration;
		}

		private enum ScrollDirection
		{
			Vertical = 0,
			Horizontal = 1
		}

		private enum MovementType
		{
			Unrestricted = 0,
			Elastic = 1,
			Clamped = 2
		}

		[SerializeField]
		private RectTransform viewport;

		[SerializeField]
		private ScrollDirection directionOfRecognize;

		[SerializeField]
		private MovementType movementType;

		[SerializeField]
		private float elasticity;

		[SerializeField]
		private float scrollSensitivity;

		[SerializeField]
		private bool inertia;

		[SerializeField]
		[Tooltip("Only used when inertia is enabled")]
		private float decelerationRate;

		[Tooltip("Only used when inertia is enabled")]
		[SerializeField]
		private Snap snap;

		[SerializeField]
		private int dataCount;

		[Tooltip("Event that fires when the position of an item changes")]
		public UpdatePositionEvent OnUpdatePosition;

		[Tooltip("Event that fires when an item is selected/focused")]
		public ItemSelectedEvent OnItemSelected;

		private Vector2 pointerStartLocalPosition;

		private float dragStartScrollPosition;

		private float currentScrollPosition;

		private bool dragging;

		private float velocity;

		private float prevScrollPosition;

		private bool autoScrolling;

		private float autoScrollDuration;

		private float autoScrollStartTime;

		private float autoScrollPosition;

		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
		}

		void IDragHandler.OnDrag(PointerEventData eventData)
		{
		}

		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
		}

		private float GetViewportSize()
		{
			return 0f;
		}

		private float CalculateOffset(float position)
		{
			return 0f;
		}

		private void UpdatePosition(float position)
		{
		}

		private float RubberDelta(float overStretching, float viewSize)
		{
			return 0f;
		}

		public void SetDataCount(int dataCont)
		{
		}

		private void Update()
		{
		}

		public void ScrollTo(int index, float duration)
		{
		}

		private float CalculateClosestPosition(int index)
		{
			return 0f;
		}

		private float GetLoopPosition(float position, int length)
		{
			return 0f;
		}

		private float EaseInOutCubic(float start, float end, float value)
		{
			return 0f;
		}
	}
}
