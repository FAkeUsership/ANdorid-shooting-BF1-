using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/Stepper")]
	public class Stepper : UIBehaviour
	{
		[Serializable]
		public class StepperValueChangedEvent : UnityEvent<int>
		{
		}

		private Selectable[] _sides;

		[Tooltip("The current step value of the control")]
		[SerializeField]
		private int _value;

		[SerializeField]
		[Tooltip("The minimum step value allowed by the control. When reached it will disable the '-' button")]
		private int _minimum;

		[SerializeField]
		[Tooltip("The maximum step value allowed by the control. When reached it will disable the '+' button")]
		private int _maximum;

		[Tooltip("The step increment used to increment / decrement the step value")]
		[SerializeField]
		private int _step;

		[Tooltip("Does the step value loop around from end to end")]
		[SerializeField]
		private bool _wrap;

		[Tooltip("A GameObject with an Image to use as a separator between segments. Size of the RectTransform will determine the size of the separator used.\nNote, make sure to disable the separator GO so that it does not affect the scene")]
		[SerializeField]
		private Graphic _separator;

		private float _separatorWidth;

		[SerializeField]
		private StepperValueChangedEvent _onValueChanged;

		private float separatorWidth => 0f;

		public Selectable[] sides => null;

		public int value
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public int minimum
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public int maximum
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public int step
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public bool wrap
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

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

		public StepperValueChangedEvent onValueChanged
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		protected Stepper()
		{
		}

		private Selectable[] GetSides()
		{
			return null;
		}

		public void StepUp()
		{
		}

		public void StepDown()
		{
		}

		private void Step(int amount)
		{
		}

		private void DisableAtExtremes(Selectable[] sides)
		{
		}

		private void RecreateSprites(Selectable[] sides)
		{
		}

		public void LayoutSides(Selectable[] sides = null)
		{
		}
	}
}
