namespace UnityEngine.UI.Extensions.Examples
{
	public class Example02ScrollViewCell : FancyScrollViewCell<Example02CellDto, Example02ScrollViewContext>
	{
		[SerializeField]
		private Animator animator;

		[SerializeField]
		private Text message;

		[SerializeField]
		private Image image;

		[SerializeField]
		private Button button;

		private readonly int scrollTriggerHash;

		private Example02ScrollViewContext context;

		private void Start()
		{
		}

		public override void SetContext(Example02ScrollViewContext context)
		{
		}

		public override void UpdateContent(Example02CellDto itemData)
		{
		}

		public override void UpdatePosition(float position)
		{
		}

		public void OnPressedCell()
		{
		}
	}
}
