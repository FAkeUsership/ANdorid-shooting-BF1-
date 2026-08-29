namespace UnityEngine.UI.Extensions.Examples
{
	public class Example03ScrollViewCell : FancyScrollViewCell<Example03CellDto, Example03ScrollViewContext>
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

		private Example03ScrollViewContext context;

		private void Start()
		{
		}

		public override void SetContext(Example03ScrollViewContext context)
		{
		}

		public override void UpdateContent(Example03CellDto itemData)
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
