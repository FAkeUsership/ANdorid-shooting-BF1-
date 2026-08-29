using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(Selectable))]
	public class Segment : UIBehaviour, IPointerClickHandler, IEventSystemHandler, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
	{
		internal int index;

		[SerializeField]
		private Color textColor;

		internal bool leftmost => false;

		internal bool rightmost => false;

		public bool selected
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		internal SegmentedControl segmentControl => null;

		internal Selectable button => null;

		protected Segment()
		{
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
		}

		public virtual void OnPointerEnter(PointerEventData eventData)
		{
		}

		public virtual void OnPointerExit(PointerEventData eventData)
		{
		}

		public virtual void OnPointerDown(PointerEventData eventData)
		{
		}

		public virtual void OnPointerUp(PointerEventData eventData)
		{
		}

		public virtual void OnSelect(BaseEventData eventData)
		{
		}

		public virtual void OnDeselect(BaseEventData eventData)
		{
		}

		public virtual void OnSubmit(BaseEventData eventData)
		{
		}

		private void SetSelected(bool value)
		{
		}

		private void Deselect()
		{
		}

		private void MaintainSelection()
		{
		}

		internal void TransitionButton()
		{
		}

		internal void TransitionButton(bool instant)
		{
		}

		private void StartColorTween(Color targetColor, bool instant)
		{
		}

		internal void StoreTextColor()
		{
		}

		private void ChangeTextColor(Color targetColor)
		{
		}

		private void DoSpriteSwap(Sprite newSprite)
		{
		}

		private void TriggerAnimation(string triggername)
		{
		}
	}
}
