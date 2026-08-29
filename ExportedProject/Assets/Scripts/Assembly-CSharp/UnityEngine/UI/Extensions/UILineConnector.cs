namespace UnityEngine.UI.Extensions
{
	[AddComponentMenu("UI/Extensions/UI Line Connector")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(UILineRenderer))]
	public class UILineConnector : MonoBehaviour
	{
		public RectTransform[] transforms;

		private Vector2[] previousPositions;

		private RectTransform canvas;

		private RectTransform rt;

		private UILineRenderer lr;

		private void Awake()
		{
		}

		private void Update()
		{
		}
	}
}
