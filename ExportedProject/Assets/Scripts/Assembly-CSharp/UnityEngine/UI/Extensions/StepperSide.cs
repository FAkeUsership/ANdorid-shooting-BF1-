using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(Selectable))]
	public class StepperSide : UIBehaviour, IPointerClickHandler, IEventSystemHandler, ISubmitHandler
	{
		private Selectable button => null;

		private Stepper stepper => null;

		private bool leftmost => false;

		protected StepperSide()
		{
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
		}

		public virtual void OnSubmit(BaseEventData eventData)
		{
		}

		private void Press()
		{
		}
	}
}
