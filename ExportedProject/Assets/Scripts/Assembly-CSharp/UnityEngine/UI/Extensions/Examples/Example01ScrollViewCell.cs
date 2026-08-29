namespace UnityEngine.UI.Extensions.Examples
{
	public class Example01ScrollViewCell : FancyScrollViewCell<Example01CellDto>
	{
		[SerializeField]
		private Animator animator;

		[SerializeField]
		private Text message;

		private readonly int scrollTriggerHash;

		private void Start()
		{
		}

		public override void UpdateContent(Example01CellDto itemData)
		{
		}

		public override void UpdatePosition(float position)
		{
		}
	}
}
