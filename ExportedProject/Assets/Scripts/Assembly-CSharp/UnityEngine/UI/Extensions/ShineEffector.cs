namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(Image))]
	[AddComponentMenu("UI/Effects/Extensions/Shining Effect")]
	[ExecuteInEditMode]
	public class ShineEffector : MonoBehaviour
	{
		public ShineEffect effector;

		[HideInInspector]
		[SerializeField]
		private GameObject effectRoot;

		[Range(-1f, 1f)]
		public float yOffset;

		[Range(0.1f, 1f)]
		public float width;

		private RectTransform effectorRect;

		public float YOffset
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		private void OnEnable()
		{
		}

		private void OnValidate()
		{
		}

		private void ChangeVal(float value)
		{
		}

		private void OnDestroy()
		{
		}
	}
}
