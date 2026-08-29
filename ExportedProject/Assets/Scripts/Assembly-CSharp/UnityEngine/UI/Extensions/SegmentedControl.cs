using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	[AddComponentMenu("UI/Extensions/Segmented Control")]
	[RequireComponent(typeof(RectTransform))]
	public class SegmentedControl : UIBehaviour
	{
		[Serializable]
		public class SegmentSelectedEvent : UnityEvent<int>
		{
		}

		private Selectable[] m_segments;

		[Tooltip("A GameObject with an Image to use as a separator between segments. Size of the RectTransform will determine the size of the separator used.\nNote, make sure to disable the separator GO so that it does not affect the scene")]
		[SerializeField]
		private Graphic m_separator;

		private float m_separatorWidth;

		[Tooltip("When True, it allows each button to be toggled on/off")]
		[SerializeField]
		private bool m_allowSwitchingOff;

		[Tooltip("The selected default for the control (zero indexed array)")]
		[SerializeField]
		private int m_selectedSegmentIndex;

		[Tooltip("Event to fire once the selection has been changed")]
		[SerializeField]
		private SegmentSelectedEvent m_onValueChanged;

		protected internal Selectable selectedSegment;

		[SerializeField]
		public Color selectedColor;

		protected float SeparatorWidth => 0f;

		public Selectable[] segments => null;

		public Graphic separator
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public bool allowSwitchingOff
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public int selectedSegmentIndex
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public SegmentSelectedEvent onValueChanged
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		protected SegmentedControl()
		{
		}

		protected override void Start()
		{
		}

		private Selectable[] GetChildSegments()
		{
			return null;
		}

		public void SetAllSegmentsOff()
		{
		}

		private void RecreateSprites()
		{
		}

		public void LayoutSegments()
		{
		}
	}
}
