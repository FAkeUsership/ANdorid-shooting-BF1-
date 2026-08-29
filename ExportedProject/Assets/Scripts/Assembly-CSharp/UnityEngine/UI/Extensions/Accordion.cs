namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(VerticalLayoutGroup), typeof(ContentSizeFitter), typeof(ToggleGroup))]
	[AddComponentMenu("UI/Extensions/Accordion/Accordion Group")]
	public class Accordion : MonoBehaviour
	{
		public enum Transition
		{
			Instant = 0,
			Tween = 1
		}

		[SerializeField]
		private Transition m_Transition;

		[SerializeField]
		private float m_TransitionDuration;

		public Transition transition
		{
			get
			{
				return Transition.Instant;
			}
			set
			{
			}
		}

		public float transitionDuration
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}
	}
}
