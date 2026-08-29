namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/Tooltip")]
	public class ToolTip : MonoBehaviour
	{
		private Text _text;

		private RectTransform _rectTransform;

		private bool _inside;

		private float width;

		private float height;

		private float YShift;

		private float xShift;

		private RenderMode _guiMode;

		private Camera _guiCamera;

		public void Awake()
		{
		}

		public void SetTooltip(string ttext)
		{
		}

		public void HideTooltip()
		{
		}

		private void FixedUpdate()
		{
		}

		public void OnScreenSpaceCamera()
		{
		}
	}
}
