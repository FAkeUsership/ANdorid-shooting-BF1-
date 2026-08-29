using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	[AddComponentMenu("UI/Extensions/UI_Knob")]
	[RequireComponent(typeof(Image))]
	public class UI_Knob : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler
	{
		public enum Direction
		{
			CW = 0,
			CCW = 1
		}

		[Tooltip("Direction of rotation CW - clockwise, CCW - counterClockwise")]
		public Direction direction;

		[HideInInspector]
		public float knobValue;

		[Tooltip("Max value of the knob, maximum RAW output value knob can reach, overrides snap step, IF set to 0 or higher than loops, max value will be set by loops")]
		public float maxValue;

		[Tooltip("How many rotations knob can do, if higher than max value, the latter will limit max value")]
		public int loops;

		[Tooltip("Clamp output value between 0 and 1, usefull with loops > 1")]
		public bool clampOutput01;

		[Tooltip("snap to position?")]
		public bool snapToPosition;

		[Tooltip("Number of positions to snap")]
		public int snapStepsPerLoop;

		[Space(30f)]
		public KnobFloatValueEvent OnValueChanged;

		private float _currentLoops;

		private float _previousValue;

		private float _initAngle;

		private float _currentAngle;

		private Vector2 _currentVector;

		private Quaternion _initRotation;

		private bool _canDrag;

		public void OnPointerDown(PointerEventData eventData)
		{
		}

		public void OnPointerUp(PointerEventData eventData)
		{
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
		}

		public void OnPointerExit(PointerEventData eventData)
		{
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
		}

		private void SetInitPointerData(PointerEventData eventData)
		{
		}

		public void OnDrag(PointerEventData eventData)
		{
		}

		private void SnapToPosition(ref float knobValue)
		{
		}

		private void InvokeEvents(float value)
		{
		}
	}
}
